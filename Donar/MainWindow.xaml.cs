using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Donar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            UpdateIdList();
        }

        #region menu commands
        public static RoutedCommand ProjectCommand = new RoutedCommand();
        private void ProjectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            string cmd = e.Parameter as string;
            switch (cmd)
            {
                case "New":
                case "Load":
                    e.CanExecute = true;
                    break;

                case "Save":
                case "Import":
                case "Export":
                    e.CanExecute = App.Current.Database.IsOpen;
                    break;
            }
        }
        private void ProjectCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            string cmd = e.Parameter as string;
            
            switch (cmd)
            {
                case "New":
                    NewDatabase();
                    break;
                case "Load":
                    LoadDatabase();
                    break;
                case "Save":
                    SaveDatabase();
                    break;
                case "Import":
                    ImportDatabase();
                    break;
                case "Export":
                    ExportDatabase();
                    break;
            }
        }

        private void NewDatabase()
        {
            if (App.Current.Database.IsOpen && App.Current.Database.IsModified)
            {
                MessageBoxResult res = MessageBox.Show("You have noit saved data. Do you want to save before the new project is created?", 
                    "Question!", MessageBoxButton.YesNo, MessageBoxImage.Stop);
                if (res == MessageBoxResult.Yes)
                {
                    App.Current.Database.SaveAll();
                }
            }
            Dialogs.NewDatabase dlg = new Dialogs.NewDatabase();
            dlg.Owner = this;
            if (dlg.ShowDialog().Value)
            {
                try
                {
                    App.Current.Database.Create(dlg.Folder.Text, dlg.PrjName.Text);
                    Title = "Donar - " + App.Current.Database.Name;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Project creating error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                UpdateIdList();
            }
        }
        private void LoadDatabase()
        {
            if (App.Current.Database.IsModified)
            {
                MessageBoxResult res = MessageBox.Show("The current project is not saved! Do you want to save it before load a new one?",
                    "Your data could be lost!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    SaveDatabase();
                }
            }
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "Please select the root folder of an exist Donar project.";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    App.Current.Database.Open(dialog.SelectedPath);
                    Title = "Donar - " + App.Current.Database.Name;
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "Project cannot be loaded!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                UpdateIdList();
            }
        }

        private void SaveDatabase()
        {
            try
            {
                App.Current.Database.SaveAll();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error during the project saving!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ImportDatabase()
        {
            Dialogs.ImportDialog dlg = new Dialogs.ImportDialog();
            int select = 0;
            foreach (Lazy<Interfaces.IImport, Interfaces.IMetadata> i in App.Current.Imports)
            {
                int id = dlg.Plugins.Items.Add(i.Metadata.Name);
                if (i.Metadata.Name == App.Current.Database.DefaultImport)
                {
                    select = id;
                }
            }
            dlg.Plugins.SelectedIndex = select;
            if (dlg.ShowDialog().Value)
            {
                foreach (Lazy<Interfaces.IImport, Interfaces.IMetadata> i in App.Current.Imports)
                {
                    if (i.Metadata.Name == dlg.Plugins.SelectedItem.ToString())
                    {
                        App.Current.Database.DefaultImport = i.Metadata.Name;
                        i.Value.Import(App.Current.Database);
                        UpdateIdList();
                        break;
                    }
                }
            }
        }

        private void ExportDatabase()
        {
            Dialogs.ImportDialog dlg = new Dialogs.ImportDialog();
            dlg.Title = "Select Export Plugin!";
            dlg.PluginTitle.Text = "Export Plugin:";
            dlg.OK.Content = "Export";
            int select = 0;
            foreach (Lazy<Interfaces.IExport, Interfaces.IMetadata> i in App.Current.Exports)
            {
                int id = dlg.Plugins.Items.Add(i.Metadata.Name);
                if (i.Metadata.Name == App.Current.Database.DefaultExport)
                {
                    select = id;
                }
            }
            dlg.Plugins.SelectedIndex = select;
            if (dlg.ShowDialog().Value)
            {
                foreach (Lazy<Interfaces.IExport, Interfaces.IMetadata> i in App.Current.Exports)
                {
                    if (i.Metadata.Name == dlg.Plugins.SelectedItem.ToString())
                    {
                        App.Current.Database.DefaultExport = i.Metadata.Name;
                        i.Value.Export(App.Current.Database);
                        UpdateIdList();
                        break;
                    }
                }
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Local Control event handlers
        private void IdList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IdList.SelectedIndex < 0)
            {
                TrView.ParagraphCollection = null;
            }
            else
            {
                TrView.ParagraphCollection = new Models.ParagraphCollection(App.Current.Database.Translate[IdList.SelectedItem as string]);
            }
        }
        #endregion

        #region Private functions
        void UpdateIdList()
        {
            if (App.Current.Database.IsOpen)
            {
                foreach (KeyValuePair<string, Interfaces.IUnit> pair in App.Current.Database.Translate)
                {
                    IdList.Items.Add(pair.Key);
                }
            }
            else
            {
                IdList.Items.Clear();
            }
        }
        #endregion
    }
}
