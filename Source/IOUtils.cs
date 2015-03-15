using System.IO;

namespace IWantMoreNames
{
	public static class IOUtils
	{
		public static void EnsureFileDeep(string path)
		{
			var directory = Path.GetDirectoryName(path);
			Directory.CreateDirectory(directory);

			if (File.Exists(path))
			{
				return;
			}

			File.Create(path).Dispose();
		}

		public static void EnsureFile(string path)
		{
			if (!File.Exists(path))
			{
				File.Create(path).Dispose();
			}
		}

		public static void EnsureDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
	}
}