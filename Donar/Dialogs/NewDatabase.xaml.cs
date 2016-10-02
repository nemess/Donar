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
using System.Windows.Shapes;

namespace Donar.Dialogs
{
    /// <summary>
    /// Interaction logic for NewDatabase.xaml
    /// </summary>
    public partial class NewDatabase : Window
    {
        public NewDatabase()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (DatabaseName.Text.Length <= 0)
            {

            }
            DialogResult = true;
        }

        private void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "Please select the root folder of Donar database. The selected folder should be empty.";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DatabaseFolder.Text = dialog.SelectedPath;
            }
        }
    }
}
