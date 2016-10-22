using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
            PrjName.Text = PrjName.Text.Trim();
            if (PrjName.Text.Length <= 0)
            {
                MessageBox.Show("Name of the databse cannot be empty", "Missing data!", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Folder.Text = Folder.Text.Trim();
            if (Folder.Text.Length <= 0)
            {
                MessageBox.Show("Folder of the databse cannot be empty", "Missing data!", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!Directory.Exists(Folder.Text))
            {
                MessageBoxResult res = MessageBox.Show("Folder is not exists! Do you want to create it?", "Question!",
                    MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (res == MessageBoxResult.Yes)
                {
                    try
                    {
                        Folder.Text = Directory.CreateDirectory(Folder.Text).FullName;
                        DialogResult = true;
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message, "Directory cannot be created!",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                Folder.Text = Directory.CreateDirectory(Folder.Text).FullName;
                DialogResult = true;
            }
        }

        private void FolderButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            dialog.Description = "Please select the root folder of Donar database. The selected folder should be empty.";

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Folder.Text = dialog.SelectedPath;
            }
        }
    }
}
