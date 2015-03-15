using ICities;
using ColossalFramework;

namespace IWantMoreNames
{
	public class Extension : LoadingExtensionBase
	{
		public override void OnLevelLoaded(LoadMode loading)
		{
			Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;
		}
	}
}