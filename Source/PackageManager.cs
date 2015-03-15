using System;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System.Linq;

namespace IWantMoreNames
{
	public static class PackageManager
	{
		public static readonly WebClient Client = new WebClient();

		public static List<Package> GetPackages(string path)
		{
			DebugConsole.Log("Reading packages.txt...");

			var packages = File.ReadAllLines(path)
			.Where(l => !l.IsNullOrWhitespace())
			.Select(rp => rp.Split(':'))
			.Select(e => new Package(e[0].Trim(), e[1].Trim()));

			return packages.ToList();
		}

		public static void UpdatePackages(List<Package> packages)
		{
			DebugConsole.Log("Checking {0} packages...", packages.Count);

			foreach(var package in packages)
			{
				package.Update();

				// Package is not installed and it can be insatlled -> install
				if (!package.Installed && package.CanBeInstalled)
				{
					DebugConsole.Log("Package {0} will be downloaded.", package.Name);
				}
				// Package is installed, it requires an update and it can be installed -> install
				else if (package.RequiresUpdate && package.CanBeInstalled)
				{
					DebugConsole.Log("Package {0} requires an update. (v{1} -> v{2})", package.Name, package.LocalVersion, package.LatestVersion);
				}
				// Package cannot be installed -> continue
				else if (!package.CanBeInstalled)
				{
					DebugConsole.Log("Package {0} cannot be updated.", package.Name);
					continue;
				}
				// Package is installed and up to date -> continue
				else
				{
					DebugConsole.Log("Package {0} is up to date. (v{1})", package.Name, package.LocalVersion);
					continue;
				}

				package.Install();
			}
		}
	}
}