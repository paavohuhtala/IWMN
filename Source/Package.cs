using System;
using System.Net;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace IWantMoreNames
{
	public class Package
	{
		public readonly string Name;
		public readonly string Repository;

		private readonly string repoRoot;

		public Package(string name, string repository)
		{
			Name = name;
			Repository = repository;
			repoRoot = string.Format("https://raw.githubusercontent.com/{0}/master/", repository);

			LocalManifest = ParseManifest(LoadLocalManifest());
		}

		public bool Installed
		{
			get { return File.Exists(LocalManifestPath); }
		}

		private string ManifestUri
		{
			get { return repoRoot + "manifest.txt"; }
		}

		private string LocalPath
		{
			get { return Path.Combine(LocalisationLoader.PackagesPath, Name); }
		}

		private string LocalManifestPath
		{
			get { return Path.Combine(LocalPath, "manifest.txt"); }
		}

		private string repoManifestData;

		public Dictionary<string, string> RepoManifest { get; private set; }

		public Dictionary<string, string> LocalManifest {get; private set; }

		private List<string> fileNames;

		public List<string> FileNames
		{
			get { return fileNames ?? (fileNames = GetFileNames()); }
		}

		public int? LatestVersion
		{
			get
			{
				if (RepoManifest != null && RepoManifest.ContainsKey("Version"))
				{
					return int.Parse(RepoManifest["Version"]);
				}

				return null;
			}
		}

		public int? LocalVersion
		{
			get
			{
				if (LocalManifest != null && LocalManifest.ContainsKey("Version"))
				{
					return int.Parse(LocalManifest["Version"]);
				}

				return null;
			}
		}

		public bool RequiresUpdate
		{
			get { return (!LocalVersion.HasValue) || (LocalVersion.Value < LatestVersion.Value); }
		}

		public bool CanBeInstalled
		{
			get { return (RepoManifest != null) && (LatestVersion.HasValue); }
		}

		private string DownloadManifest()
		{
			try
			{
				string manifestData = string.Empty;
				DebugConsole.Log("Downloading manifest: {0}", ManifestUri);

				using (NoSSL.Instance)
				{
					manifestData = PackageManager.Client.DownloadString(ManifestUri);
				}

				return manifestData;
			}
			catch(WebException)
			{
				DebugConsole.Log("Couldn't download manifest for package {0}. Package won't be updated.", Name);
				DebugConsole.Log("Please make sure you're connected to the internet and the GitHub repository {0} exists.", Repository);
				return null;
			}
		}

		private string LoadLocalManifest()
		{
			if (!Installed)
			{
				return null;
			}

			return File.ReadAllText(LocalManifestPath);
		}

		private static Dictionary<string, string> ParseManifest(string manifestData)
		{
			if (manifestData == null)
			{
				return null;
			}

			var parsedManifest = manifestData.Split('\n')
			.Where(l => !l.IsNullOrWhitespace())
			.Select(l => l.Split(':'))
			.ToDictionary(e => e[0].Trim(), e => e[1].Trim());

			return parsedManifest;
		}

		private List<string> GetFileNames()
		{
			var nameCategories = RepoManifest.Keys.Intersect(LocalisationLoader.Categories);
			var fileNames = new List<string>();

			foreach(var category in nameCategories)
			{
				var files = RepoManifest[category].Split(','). Select(n => n.Trim()).Select(f => category + "/" + f);
				fileNames.AddRange(files);
			}

			return fileNames;
		}

		// Since this is a URI and not a file path, a string concation will do here just fine
		private string ToRepoFile(string fileName)
		{
			return repoRoot + fileName;
		}

		private string ToLocalFile(string fileName)
		{
			return Path.Combine(LocalPath, fileName);
		}

		private void DownloadFiles(List<string> fileNames)
		{
			DebugConsole.Log("Downloading {0} files for {1}...", fileNames.Count, Name);

			var stopwatch = new Stopwatch();
			stopwatch.Start();

			foreach(var fileName in fileNames)
			{
				var localFile = ToLocalFile(fileName);
				var directory = Path.GetDirectoryName(localFile);
				Directory.CreateDirectory(directory);
				File.Create(localFile).Dispose();

				using (NoSSL.Instance)
				{
					PackageManager.Client.DownloadFile(ToRepoFile(fileName), localFile);
				}
			}

			stopwatch.Stop();
			DebugConsole.Log("Downloaded in {0}ms.", stopwatch.ElapsedMilliseconds);
		}

		public Dictionary<string, List<string>> GetLocalisations()
		{
			return LocalisationLoader.GetLocalisations(LocalPath);
		}

		public void Update()
		{
			repoManifestData = DownloadManifest();
			RepoManifest = ParseManifest(repoManifestData);
		}

		public void Install()
		{
			Clean();

			if (!File.Exists(LocalManifestPath))
			{
				IOUtils.EnsureFileDeep(LocalManifestPath);
				File.WriteAllText(LocalManifestPath, repoManifestData);
			}

			if (RepoManifest != null)
			{
				DownloadFiles(FileNames);
			}
		}

		public void Clean()
		{
			if (Directory.Exists(LocalPath))
			{
				DebugConsole.Log("Cleaning package {0}...", Name);
				var dirInfo = new DirectoryInfo(LocalPath);
				dirInfo.Delete(true);
			}
		}

		public override string ToString()
		{
			return string.Format("{0} v{1}: {2}", Name, LatestVersion, repoRoot);
		}
	}
}