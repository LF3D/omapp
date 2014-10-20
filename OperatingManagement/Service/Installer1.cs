using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;


namespace OperatingManagement.Service
{
    [RunInstaller(true)]
    public partial class Installer1 : Installer
    {
        public Installer1()
        {
            InitializeComponent();

            ServiceProxy.InitializeInstaller(serviceInstaller1);
        }
    }
}
