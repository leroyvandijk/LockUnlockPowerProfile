using LockUnlockPowerProfile.Interfaces;
using LockUnlockPowerProfile.Models;
using LockUnlockPowerProfile.Utils;
using System;
using System.Collections.Generic;

namespace LockUnlockPowerProfile.Services
{
	/// <summary>
	/// Class which manages all the settings through the application.
	/// </summary>
	internal class SettingsService
	{
		private readonly PluginService _pluginService;
		private readonly IniFile _settingsIniFile;

		public SettingsModel CurrentSettings { get; set; }

		public SettingsService(PluginService pluginService)
		{
			_pluginService = pluginService;
			_settingsIniFile = new IniFile("Settings.ini");
			ReadSettings();
		}

		/// <summary>
		/// Function to read all the settings from the settings.ini file using the IniFile from the constructor.
		/// </summary>
		private void ReadSettings()
		{
			if (!_settingsIniFile.KeyExists("LockProfileGuid", "Program") ||
				!_settingsIniFile.KeyExists("UnlockProfileGuid", "Program"))
			{
				InitializeIni();
			}

			try
			{
				CurrentSettings = new SettingsModel
				{
					LockPowerPlan = new PowerPlanModel(new Guid(_settingsIniFile.Read("LockProfileGuid", "Program"))),
					UnlockPowerPlan =
						new PowerPlanModel(new Guid(_settingsIniFile.Read("UnlockProfileGuid", "Program"))),
					Plugins = new Dictionary<IPlugin, bool>()
				};
				foreach (IPlugin plugin in _pluginService.GetAllPlugins())
				{
					string setting = _settingsIniFile.Read(plugin.Name, "Plugins");
					if (string.IsNullOrEmpty(setting))
						CurrentSettings.Plugins.Add(plugin, false);
					else
					{
						bool enabled = Convert.ToBoolean(setting);
						if (enabled)
							_pluginService.EnablePlugin(plugin);
						CurrentSettings.Plugins.Add(plugin, enabled);
					}
				}
			}
			catch (Exception)
			{
				InitializeIni();
			}
		}

		/// <summary>
		/// Function to generate first time usage settings for the application.
		/// </summary>
		private void InitializeIni()
		{
			// This is the guid of the default power safe profile in Windows
			const string defaultPowerSafeProfile = "a1841308-3541-4fab-bc81-f71556f20b4a";

			_settingsIniFile.Write("LockProfileGuid", defaultPowerSafeProfile, "Program");
			_settingsIniFile.Write("UnlockProfileGuid", PowerFunctions.GetActiveGuid().ToString(), "Program");

			foreach (IPlugin plugin in _pluginService.GetAllPlugins())
			{
				_settingsIniFile.Write(plugin.Name, false.ToString(), "Plugins");
			}
		}

		/// <summary>
		/// Function to save the settings based on the given model.
		/// </summary>
		/// <param name="settingsModel"></param>
		public void SaveSettings(SettingsModel settingsModel)
		{
			CurrentSettings.LockPowerPlan = settingsModel.LockPowerPlan;
			CurrentSettings.UnlockPowerPlan = settingsModel.UnlockPowerPlan;
			CurrentSettings.Plugins = settingsModel.Plugins;

			// Finally write
			_settingsIniFile.Write("LockProfileGuid", CurrentSettings.LockPowerPlan.Guid.ToString(), "Program");
			_settingsIniFile.Write("UnlockProfileGuid", CurrentSettings.UnlockPowerPlan.Guid.ToString(), "Program");

			// Loop through all plugins and set them enabled or disabled
			foreach (KeyValuePair<IPlugin, bool> kv in settingsModel.Plugins)
			{
				_settingsIniFile.Write(kv.Key.Name, kv.Value.ToString(), "Plugins");
			}
		}
	}
}