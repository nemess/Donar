using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace Donar
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(typeof(App).Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory));

            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                _container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                MessageBox.Show(compositionException.ToString(), "CompositionException", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static new App Current { get { return Application.Current as App; } }

        [Import(typeof(Interfaces.IDatabase))]
        public Interfaces.IDatabase Database;

        [ImportMany]
        public IEnumerable<Lazy<Interfaces.IExport, Interfaces.IMetadata>> Exports;

        [ImportMany]
        public IEnumerable<Lazy<Interfaces.IImport, Interfaces.IMetadata>> Imports;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string dbFolder = Donar.Properties.Settings.Default.DefaultDatabase;

            if (dbFolder != null && dbFolder.Length > 0)
            {
                Database.Open(dbFolder);
            }
        }

        private CompositionContainer _container;
    }
}
