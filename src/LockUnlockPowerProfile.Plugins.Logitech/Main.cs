using System;
using LockUnlockPowerProfile.Interfaces;
using System.Diagnostics;
using System.IO;
using LockUnlockPowerProfile.Models;
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
		public string Name { get; } = "Logitech G-Keys Plugin";

		public string Description { get; } = "Plugin to disable any LED when computer is put to lock mode. " +
		                                     "Newer Logitech software has this already built-in but not on computer locking.";

		private LogitechType _logitechType = LogitechType.None;

		public event EventHandler<LogMessageEventsArgs> LogEvent;

		private void LogMessage(string message)
		{
			LogEvent?.Invoke(this, new LogMessageEventsArgs {Message = message});
		}

		public void OnLock()
		{
			switch (_logitechType)
			{
				case (LogitechType.LCore):
					LogiLedSetLighting(0, 0, 0);
					break;
				case (LogitechType.LgHub):
					LogiLedInit();
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
			_logitechType = CheckLogitechType();

			if (_logitechType == LogitechType.LCore)
				LogiLedInit();
		}

		public void OnEnd()
		{
			switch (_logitechType)
			{
				case (LogitechType.LCore):
					LogiLedRestoreLighting();
					LogiLedShutdown();
					return;
				case LogitechType.LgHub:
					LogiLedShutdown();
					return;
			}
		}


		private LogitechType CheckLogitechType()
		{
			string programFiles = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
			string programFilesX86 = Environment.ExpandEnvironmentVariables("%ProgramFiles(x86)%");

			Process[] lHubProcess = Process.GetProcessesByName("lghub");

			if (lHubProcess.Length != 0 || Directory.Exists($"{programFiles}\\LGHUB") ||
			    Directory.Exists($"{programFilesX86}\\LGHUB"))
			{
				LogMessage("Found Logitech Hub (new version)");
				return LogitechType.LgHub;
			}


			Process[] lCoreProcess = Process.GetProcessesByName("LCore");
			if (lCoreProcess.Length != 0 || Directory.Exists($"{programFiles}\\Logitech Gaming Software") ||
			    Directory.Exists($"{programFilesX86}\\Logitech Gaming Software"))
			{
				LogMessage("Found Logitech Core (old version)");
				LogiLedInit();
				return LogitechType.LCore;
			}

			LogMessage("Could not find any Logitech software!");
			return LogitechType.None;
		}
	}
}