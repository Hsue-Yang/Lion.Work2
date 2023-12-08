using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;


namespace LionTech.EDIService.ERP
{
    [RunInstaller(true)]
    public partial class ERPInstaller : System.Configuration.Install.Installer
    {
        public ERPInstaller()
        {
            InitializeComponent();
        }
    }
}
