using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using ColossalFramework;
using ColossalFramework.IO;

namespace IWantMoreNames
{
	public static class LocalisationLoader
	{
		internal static bool Loaded = false;

		public static string CustomNamesPath
		{
			get { return Path.Combine(DataLocation.addonsPath, "CustomNames"); }
		}

		public static string UserPath
		{
			get { return Path.Combine(CustomNamesPath, "User"); }
		}

		public static string PackagesPath
		{
			get { return Path.Combine(CustomNamesPath, "Packages"); }
		}

		public static string PackageListPath
		{
			get { return Path.Combine(CustomNamesPath, "packages.txt"); }
		}

		public static readonly List<string> Categories = new List<string>
		{
			"Chirp",
			"Male",
			"Female",
			"Surname"
		};

		public static void Load()
		{
			if (Loaded)
			{
				return;
			}

			Loaded = true;

			DebugConsole.Log("Hello!");

			try
			{
				EnsureDirectories();
				EnsureFiles();

				DebugConsole.Log("Reloading default localisation...");
				Localisations.Reload();
				Localisations.Init();

				var packages = PackageManager.GetPackages(PackageListPath);
				PackageManager.UpdatePackages(packages);

				var packageLocalisations = packages.Where(p => p.Installed).Select(p => p.GetLocalisations()).ToList();
				var userLocalisations = GetLocalisations(UserPath);

				packageLocalisations.Add(userLocalisations);

				var localisations = Merge(packageLocalisations);
				var count = localisations.SelectMany((kvp) => kvp.Value).Count();

				DebugConsole.Log("Built localisation database ({0} items).", count);

				foreach(var category in localisations.Keys)
				{
					if(!localisations.ContainsKey(category))
					{
						continue;
					}

					Localisations.Register(category, localisations[category]);
				}

				DebugConsole.Log("All localisations registered.");
			}
			catch(Exception e)
			{
				DebugConsole.Log("Exception: {0}", e.ToString());
			}
		}

		// Nested data structure madness: List<Dictionary<string, List<string>>> -> Dictionary<string, List<string>>
		private static Dictionary<string, List<string>> Merge(IEnumerable<Dictionary<string, List<string>>> dictionaries)
		{
			return dictionaries.Aggregate(new Dictionary<string, List<string>>(), (dict, items) =>
			{
				foreach(var kvp in items)
				{
					if (!dict.ContainsKey(kvp.Key))
					{
						dict[kvp.Key] = kvp.Value;
					}
					else
					{
						dict[kvp.Key].AddRange(kvp.Value);
					}
				}

				return dict;
			});
		}

		public static void Unload()
		{
			DebugConsole.Log("Unloading.");
			Localisations.Reload();
		}

		public static Dictionary<string, List<string>> GetLocalisations(string source)
		{
			var allLocalisations = new Dictionary<string, List<string>>();
			var directories = Directory.GetDirectories(source);

			int count = 0;

			foreach(var directory in directories)
			{
				var categoryName = new DirectoryInfo(directory).Name.ToLower();
				var category = new List<string>();
				var files = Directory.GetFiles(directory, "*.txt");

				foreach(var file in files)
				{
					var localisations = File.ReadAllLines(file).Select(n => n.Trim());
					count += localisations.Count();

					if (categoryName == "surname")
					{
						localisations = localisations.Select(n => "{0} " + n);
					}

					category.AddRange(localisations);
				}

				if(categoryName == "surname")
				{
					allLocalisations["surname_male"] = category;
					allLocalisations["surname_female"] = category;
				}
				else
				{
					allLocalisations[categoryName] = category;
				}
			}

			return allLocalisations;
		}

		private static void EnsureDirectories()
		{
			IOUtils.EnsureDirectory(CustomNamesPath);
			IOUtils.EnsureDirectory(PackagesPath);
			IOUtils.EnsureDirectory(UserPath);

			foreach(var category in Categories)
			{
				var subPath = Path.Combine(UserPath, category);
				IOUtils.EnsureDirectory(subPath);
			}
		}

		private static void EnsureFiles()
		{
			IOUtils.EnsureFile(PackageListPath);
		}
	}
}