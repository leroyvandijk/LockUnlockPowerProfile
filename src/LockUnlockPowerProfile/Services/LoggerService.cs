using System;
using System.Collections.Generic;

namespace LockUnlockPowerProfile.Services
{
	internal sealed class LoggerService
	{
		private static LoggerService _instance = null;

		public static LoggerService Instance => _instance ?? (_instance = new LoggerService());

		private readonly List<string> _logs;

		public delegate void Observer();

		private Observer _observers;

		/// <summary>
		/// Function to add an observer (observer pattern).
		/// </summary>
		/// <param name="obs"></param>
		public void AddObserver(Observer obs)
		{
			_observers += obs;
		}

		public LoggerService()
		{
			_logs = new List<string> { $"[{DateTime.Now:G}] Log initialized" };
		}

		public List<string> GetLogs()
		{
			return _logs;
		}

		public void AddLog(string text)
		{
			_logs.Add($"[{DateTime.Now:G}] {text}");
			_observers?.Invoke();
		}
	}
}