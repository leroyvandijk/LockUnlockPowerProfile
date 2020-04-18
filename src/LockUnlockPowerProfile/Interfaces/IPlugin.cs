using System;
using LockUnlockPowerProfile.Models;

namespace LockUnlockPowerProfile.Interfaces
{

	public interface IPlugin
	{
		/// <summary>
		/// The name of the plugin.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// The description of the plugin.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// This function will be called if the system locks.
		/// </summary>
		void OnLock();

		/// <summary>
		/// This function will be called if the system unlocks.
		/// </summary>
		void OnUnlock();

		/// <summary>
		/// This function will be called if the plugin is being enabled.
		/// </summary>
		void OnStart();

		/// <summary>
		/// This function will be called if the plugin is being disabled.
		/// </summary>
		void OnEnd();

		/// <summary>
		/// Send events to this event handler if you want your plugin messages to be logged.
		/// </summary>
		event EventHandler<LogMessageEventsArgs> LogEvent;
	}
}