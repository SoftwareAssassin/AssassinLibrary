///
/// Original Author: Software Assassin
/// http://www.softwareassassin.com
/// 
/// GNU All-Permissive License:
/// Copying and distribution of this file, with or without modification,
/// are permitted in any medium without royalty provided the copyright
/// notice and this notice are preserved.  This file is offered as-is,
/// without any warranty.
/// 
/// Source code available at:
/// https://github.com/SoftwareAssassin/AssassinLibrary
/// 

using System;
using System.ServiceProcess;

namespace Assassin
{
	public class BackgroundService : ServiceBase
	{
		private BackgroundServiceState m_state = BackgroundServiceState.Initializing;
		public BackgroundServiceState State
		{
			get
			{
				return this.m_state;
			}
		}

		public BackgroundService()
		{
			this.Init();
		}
		~BackgroundService()
		{
			this.HandleDispose();
		}

		private void Init()
		{
			//TODO
		}
		private void HandleDispose()
		{
			//TODO
		}

		private void HandleStart(string[] args)
		{
			this.m_state = BackgroundServiceState.Running;
		}
		private void HandleStop()
		{
			this.m_state = BackgroundServiceState.Idle;
		}

		#region Event Handlers
		protected override void OnStart(string[] args)
		{
			this.HandleStart(args);
			base.OnStart(args);
		}
		protected override void OnStop()
		{
			this.HandleStop();
			base.OnStop();
		}
		protected override void OnPause()
		{
			this.HandleStop();
			base.OnPause();
		}
		#endregion
	}
}
