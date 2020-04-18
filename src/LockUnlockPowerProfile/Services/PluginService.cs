using LockUnlockPowerProfile.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LockUnlockPowerProfile.Models;

namespace LockUnlockPowerProfile.Services
{
	internal class PluginService
	{
		// Dictionary with plugins. Bool in dictionary represents the status of the plugin (enabled/disabled)
		private Dictionary<IPlugin, bool> Plugins { get; }

		/// <summary>
		/// Constructor which loads all plugins from the Plugins directory.
		/// </summary>
		public PluginService()
		{
			Plugins = new Dictionary<IPlugin, bool>();
			try
			{
				LoadPlugins();
			}
			catch (Exception e)
			{
				LoggerService.Instance.AddLog($"Error loading plugins. Message: {e.Message}");
			}
		}

		/// <summary>
		/// Function to load all plugins.
		/// </summary>
		private void LoadPlugins()
		{
			string pluginsFolder = $"{Directory.GetCurrentDirectory()}\\Plugins";
			//Load the DLLs from the Plugins directory
			if (Directory.Exists(pluginsFolder))
			{
				string[] files = Directory.GetFiles(pluginsFolder);
				foreach (string file in files)
				{
					if (file.Contains("LockUnlockPowerProfile.Plugins") && file.EndsWith(".dll"))
					{
						Assembly.LoadFile(Path.GetFullPath(file));
					}
				}
			}

			Type interfaceType = typeof(IPlugin);
			//Fetch all types that implement the interface IPlugin and are a class
			Type[] types = AppDomain.CurrentDomain.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(p => interfaceType.IsAssignableFrom(p) && p.IsClass)
				.ToArray();
			foreach (Type type in types)
			{
				//Create a new instance of all found types
				Plugins.Add((IPlugin) Activator.CreateInstance(type), false);
			}
		}

		/// <summary>
		/// Function to get all enabled plugins.
		/// </summary>
		/// <returns></returns>
		public List<IPlugin> GetEnabledPlugins()
		{
			return Plugins.Where(pair => pair.Value).Select(pair => pair.Key).ToList();
		}

		/// <summary>
		/// Function to get all available plugins.
		/// </summary>
		/// <returns></returns>
		public List<IPlugin> GetAllPlugins()
		{
			return Plugins.Select(pair => pair.Key).ToList();
		}

		/// <summary>
		/// Function to enable a given plugin.
		/// </summary>
		/// <param name="plugin"></param>
		public void EnablePlugin(IPlugin plugin)
		{
			if (Plugins[plugin]) return;

			LoggerService.Instance.AddLog($"Enabling plugin {plugin.Name}");
			Plugins[plugin] = true;
			plugin.LogEvent += PluginOnLogEvent;
			plugin.OnStart();
		}

		/// <summary>
		/// Function to disable a given plugin.
		/// </summary>
		/// <param name="plugin"></param>
		public void DisablePlugin(IPlugin plugin)
		{
			if (!Plugins[plugin]) return;

			LoggerService.Instance.AddLog($"Disabling plugin {plugin.Name}");
			Plugins[plugin] = false;
			plugin.OnEnd();
			plugin.LogEvent -= PluginOnLogEvent;
		}

		private static void PluginOnLogEvent(object sender, LogMessageEventsArgs e)
		{
			if(sender is IPlugin plugin)
				LoggerService.Instance.AddLog($"{plugin.Name}: {e.Message}");
		}
	}
}