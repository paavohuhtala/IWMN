using System.Collections.Generic;
using ColossalFramework.Globalization;
using System.Reflection;

namespace IWantMoreNames
{
	public static class Localisations
	{
		private static Locale LocaleInstance;
		private static Dictionary<Locale.Key, int> LocalisationCounts;

		public static void Init()
		{
			// Already initialized -> return
			if (LocaleInstance != null)
			{
				return;
			}

			var localeManager = ColossalFramework.SingletonLite<LocaleManager>.instance;
			var localeField = typeof(LocaleManager).GetField("m_Locale", BindingFlags.NonPublic | BindingFlags.Instance);
			LocaleInstance = (Locale)localeField.GetValue(localeManager);

			var localisationCountsField = typeof(Locale).GetField("m_LocalizedStringsCount", BindingFlags.NonPublic | BindingFlags.Instance);
			LocalisationCounts = (Dictionary<Locale.Key, int>) localisationCountsField.GetValue(LocaleInstance);
		}

		public static void Register(string category, List<string> localisations)
		{
			var localeId = LocaleIds[category];

			var categoryKey = new Locale.Key()
			{
				m_Identifier = localeId,
				m_Key = null
			};

			var currentCount = LocaleInstance.Count(categoryKey);
			var toBeAdded = localisations.Count;

			int index = 0;

			foreach(var localisation in localisations)
			{
				var localeKey = new Locale.Key()
				{
					m_Identifier = localeId,
					m_Key = null,
					m_Index = currentCount + index
				};

				LocaleInstance.AddLocalizedString(localeKey, localisation);
				index++;
			}

			LocalisationCounts[categoryKey] = currentCount + toBeAdded;
		}

		private static Dictionary<string, string> LocaleIds = new Dictionary<string, string>
		{
			{"male", "NAME_MALE_FIRST"},
			{"surname_male", "NAME_MALE_LAST"},
			{"female", "NAME_FEMALE_FIRST"},
			{"surname_female", "NAME_FEMALE_LAST"},
			{"chirp", "CHIRP_RANDOM"}
		};

		public static void Reload()
		{
			LocaleManager.ForceUnload();
			LocaleManager.ForceReload();
		}

	}
}