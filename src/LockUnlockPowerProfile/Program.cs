using LockUnlockPowerProfile.Interfaces;
using LockUnlockPowerProfile.Properties;
using LockUnlockPowerProfile.Services;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

namespace LockUnlockPowerProfile
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main()
		{
			// Set correct path.
			// If launched on start-up the current directory env variable isn't set to the executable path.
			// This will cause plugins to not be loaded.
			string path = Path.GetDirectoryName(Application.ExecutablePath);
			if (!string.IsNullOrEmpty(path))
				Environment.CurrentDirectory = path;

			Application.Run(new LockUnlockPowerProfileApplicationContext());
		}
	}

	internal class LockUnlockPowerProfileApplicationContext : ApplicationContext
	{
		private readonly NotifyIcon _trayIcon;
		private readonly MenuItem _startUp;
		private readonly RegistryKey _rk;
		private Form _settingsForm;
		private readonly PowerPlanService _powerPlanService;
		private readonly SettingsService _settingsService;
		private readonly PluginService _pluginService;

		/// <summary>
		/// Constructor
		/// </summary>
		public LockUnlockPowerProfileApplicationContext()
		{
			_pluginService = new PluginService();

			foreach (IPlugin plugin in _pluginService.GetEnabledPlugins())
			{
				plugin.OnStart();
				LoggerService.Instance.AddLog($"Plugin loaded {plugin.Name}");
			}

			_rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

			_startUp = new MenuItem("Run on startup", RunOnStartUp);

			_settingsService = new SettingsService(_pluginService);
			Application.ApplicationExit += Exit;
			// Initialize Tray Icon
			ContextMenu contextMenu = new ContextMenu(new MenuItem[]
			{
				_startUp,
				new MenuItem("Exit", Exit)
			});
			_trayIcon = new NotifyIcon()
			{
				Icon = Resources.logo,
				Text = Application.ProductName,
				ContextMenu = contextMenu,
				Visible = true
			};
			_trayIcon.DoubleClick += ShowSettings;

			SystemEvents.SessionSwitch += SystemEventsSessionSwitch;

			_startUp.Checked = _rk?.GetValue(Application.ProductName) != null;

			_powerPlanService = new PowerPlanService(_settingsService, _pluginService);
		}

		/// <summary>
		/// This event will happen when there is a system session switch (lock/unlock etc.)
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SystemEventsSessionSwitch(object sender, SessionSwitchEventArgs e)
		{
			switch (e.Reason)
			{
				// TODO: Make a setting of this
				case SessionSwitchReason.ConsoleDisconnect:
				case SessionSwitchReason.RemoteDisconnect:
				case SessionSwitchReason.SessionLock:
					Lock();
					break;
				case SessionSwitchReason.ConsoleConnect:
				case SessionSwitchReason.RemoteConnect:
				case SessionSwitchReason.SessionUnlock:
					Unlock();
					break;
			}
		}

		/// <summary>
		/// Function that is called when computer is locked
		/// </summary>
		private void Lock()
		{
			LoggerService.Instance.AddLog("Computer is locked");
			_powerPlanService.ChangeToLockPlan();
		}

		/// <summary>
		/// Function that is called when computer is unlocked
		/// </summary>
		private void Unlock()
		{
			LoggerService.Instance.AddLog("Computer is unlocked");
			_powerPlanService.RestorePlan();
		}

		private void Exit(object sender, EventArgs e)
		{
			_rk.Close();
			_trayIcon.Visible = false;
			Application.ApplicationExit -= Exit;
			foreach (IPlugin plugin in _pluginService.GetEnabledPlugins())
			{
				plugin.OnEnd();
			}

			Application.Exit();
		}

		/// <summary>
		/// This event listener will show a settings window if there has been double clicked on the tray icon
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShowSettings(object sender, EventArgs e)
		{
			if (_settingsForm == null || _settingsForm.IsDisposed)
			{
				_settingsForm = new SettingsForm(_settingsService, _pluginService);
			}

			LoggerService.Instance.AddLog("Opened settings form");
			_settingsForm.Show();
		}

		/// <summary>
		/// This event listener will enable or disable the run on start-up feature
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RunOnStartUp(object sender, EventArgs e)
		{
			if (!_startUp.Checked)
			{
				_startUp.Checked = true;
				_rk.SetValue(Application.ProductName, "\"" + Application.ExecutablePath + "\"");
				LoggerService.Instance.AddLog("Run on startup enabled");
			}
			else
			{
				_rk.DeleteValue(Application.ProductName, false);
				_startUp.Checked = false;
				LoggerService.Instance.AddLog("Run on startup disabled");
			}
		}
	}
}