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
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Assassin
{
	[RunInstaller(true)]
	public partial class BackgroundServiceInstaller : Installer
	{
		private static string m_serviceName = null;
		private static string m_userName = null;
		private static string m_password = null;
		private static ServiceStartMode m_startMode = ServiceStartMode.Manual;

		public static string ServiceName
		{
			get
			{
				return BackgroundServiceInstaller.m_serviceName;
			}
		}
		public static string UserName
		{
			get
			{
				return BackgroundServiceInstaller.m_userName;
			}
		}
		public static string Password
		{
			get
			{
				return BackgroundServiceInstaller.m_password;
			}
		}
		public static ServiceStartMode StartMode
		{
			get
			{
				return BackgroundServiceInstaller.m_startMode;
			}
		}

		public BackgroundServiceInstaller(string serviceName, ServiceStartMode startMode)
			: this(serviceName)
		{ }
		public BackgroundServiceInstaller(string serviceName)
			: this(serviceName, BackgroundServiceInstaller.UserName, BackgroundServiceInstaller.Password)
		{ }
		public BackgroundServiceInstaller(string serviceName, string userName, string password)
			: this(serviceName, BackgroundServiceInstaller.StartMode, userName, password)
		{ }

		//construction takes place here
		public BackgroundServiceInstaller(string serviceName, ServiceStartMode startMode, string userName, string password)
			: base()
		{
			//define static members
			BackgroundServiceInstaller.m_serviceName = serviceName;
			BackgroundServiceInstaller.m_startMode = startMode;
			BackgroundServiceInstaller.m_userName = userName;
			BackgroundServiceInstaller.m_password = password;

			//run installer
			this.Install();
		}

		private void Install()
		{
			ServiceInstaller servInstall = new ServiceInstaller();
			servInstall.ServiceName = BackgroundServiceInstaller.ServiceName;
			servInstall.StartType = BackgroundServiceInstaller.StartMode;

			ServiceProcessInstaller procInstall = new ServiceProcessInstaller();
			procInstall.Account = ServiceAccount.LocalSystem;
			procInstall.Username = BackgroundServiceInstaller.UserName;
			procInstall.Password = BackgroundServiceInstaller.Password;

			this.Installers.AddRange(new Installer[]{
				servInstall
				, procInstall
				});
		}
	}
}
