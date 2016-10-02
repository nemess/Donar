using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DonarDatabase;

namespace Donar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static new App Current { get { return Application.Current as App; } }

        public IDonarDb Database = DatabaseMain.CreateDatabase();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string dbFolder = Donar.Properties.Settings.Default.DefaultDatabase;

            if (dbFolder != null && dbFolder.Length > 0)
            {
                Database.Open(dbFolder);
            }
        }
    }
}
