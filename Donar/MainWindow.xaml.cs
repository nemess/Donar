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
using DonarDatabase;
using TranslateDb = DonarDatabase.Translate;

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

            foreach (KeyValuePair<string, TranslateDb.IUnit> pair in App.Current.Database.Translate)
            {
                IdList.Items.Add(pair.Key);
            }
        }
        private void WorkspaceCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void WorkspaceCommand_Execute(object sender, ExecutedRoutedEventArgs e)
        {
            string cmd = e.Parameter as string;
            
            switch (cmd)
            {
                case "New":
                    Dialogs.NewDatabase dlg = new Dialogs.NewDatabase();
                    dlg.Owner = this;
                    dlg.ShowDialog();
                    break;

                case "Load":
                    break;
            //    case "Save": (App.Current as App).CmdSave(); break;
            //    case "Import": (App.Current as App).CmdImport(); break;
            //    case "Export": (App.Current as App).CmdExport(); break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public static RoutedCommand WorkspaceCommand = new RoutedCommand();


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
    }
}
