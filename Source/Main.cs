using ICities;

namespace IWantMoreNames
{
	public class Main : IUserMod
	{
		public string Name
		{
			get
			{
				LocalisationLoader.Load();
				return "I Want More Names";
			}
		}

		public string Description
		{
			get
			{
				return "LOADED AUTOMAGICALLY, DO NOT ENABLE.\nLoads custom localisations from user supplied text files and online packages.";
			}
		}
	}
}
