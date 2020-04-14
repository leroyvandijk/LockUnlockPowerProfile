using LockUnlockPowerProfile.Interfaces;
using System.Diagnostics;
using static LedCSharp.LogitechGSDK;

namespace LockUnlockPowerProfile.Plugins.Logitech
{
	public enum LogitechType
	{
		LCore,
		LgHub,
		None
	}

	public class Main : IPlugin
	{
		private LogitechType _logitechType = LogitechType.None;

		public void OnLock()
		{
			switch (_logitechType)
			{
				case (LogitechType.LgHub):
					LogiLedInit();
					break;
				case (LogitechType.LCore):
					LogiLedSetLighting(0, 0, 0);
					break;
			}
		}

		public void OnUnlock()
		{
			switch (_logitechType)
			{
				case (LogitechType.LgHub):
					LogiLedShutdown();
					break;
				case (LogitechType.LCore):
					LogiLedRestoreLighting();
					break;
			}
		}

		public void OnStart()
		{
			Process[] lCoreProcess = Process.GetProcessesByName("LCore");
			if (lCoreProcess.Length != 0)
			{
				_logitechType = LogitechType.LCore;
				LogiLedInit();
				return;
			}

			Process[] lHubProcess = Process.GetProcessesByName("lghub");
			if (lHubProcess.Length != 0)
			{
				_logitechType = LogitechType.LgHub;
			}
		}

		public void OnEnd()
		{
			switch (_logitechType)
			{
				case (LogitechType.LCore):
					LogiLedRestoreLighting();
					break;
			}

			LogiLedShutdown();
		}

		public string Name { get; } = "Logitech G-Keys Plugin";
		public string Description { get; } = "Plugin to disable any LED when computer is put to lock mode. " +
											 "Newer Logitech software has this already built-in but not on computer locking.";
	}
}