using LockUnlockPowerProfile.Interfaces;
using LockUnlockPowerProfile.Utils;
using System;

namespace LockUnlockPowerProfile.Services
{
	internal class PowerPlanService
	{
		private readonly SettingsService _settingsService;
		private readonly PluginService _pluginService;

		public PowerPlanService(SettingsService settingsService, PluginService pluginService)
		{
			_settingsService = settingsService;
			_pluginService = pluginService;
		}

		/// <summary>
		/// Function to restore the power plan from the settings.
		/// </summary>
		public void RestorePlan()
		{
			Guid activePolicyGuid = _settingsService.CurrentSettings.UnlockPowerPlan.Guid;
			PowerFunctions.PowerSetActiveScheme(IntPtr.Zero, ref activePolicyGuid);


			foreach (IPlugin plugin in _pluginService.GetEnabledPlugins())
			{
				try
				{
					plugin.OnUnlock();
				}
				catch (Exception e)
				{
					LoggerService.Instance.AddLog(
						$"Error while restoring plan in plugin {e.Message}. Plugin name: {plugin.Name}");
				}
			}
		}

		/// <summary>
		/// Function to change to the locked power plan.
		/// </summary>
		public void ChangeToLockPlan()
		{
			Guid activePolicyGuid = _settingsService.CurrentSettings.LockPowerPlan.Guid;
			PowerFunctions.PowerSetActiveScheme(IntPtr.Zero, ref activePolicyGuid);
			foreach (IPlugin plugin in _pluginService.GetEnabledPlugins())
			{
				try
				{
					plugin.OnLock();
				}
				catch (Exception e)
				{
					LoggerService.Instance.AddLog(
						$"Error applying lock plan in plugin {e.Message}. Plugin name: {plugin.Name}");
				}
			}
		}
	}
}