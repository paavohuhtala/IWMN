using System;
using System.IO;

using ColossalFramework;
using ColossalFramework.Plugins;

namespace IWantMoreNames
{
	public static class DebugConsole
	{
		public static void Log(string message)
		{
			var time = DateTime.Now;
			var stampedMessage = string.Format("[{0}] [IWMN] {1}", time.ToString("hh:mm:ss"), message);
			DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, stampedMessage);
		}

		public static void Log(string message, params object[] args)
		{
			Log(string.Format(message, args));
		}

		public static void Log(object message)
		{
			Log(message.ToString());
		}

	}
}