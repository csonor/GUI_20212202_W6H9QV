using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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


namespace ColorCross.UserControls
{
    /// <summary>
    /// Interaction logic for CustomImageUserControl.xaml
    /// </summary>
    public partial class CustomImageUserControl : UserControl
    {
        public CustomImageUserControl()
        {
            InitializeComponent();
        }
        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
   
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Open Image";
            dialog.Filter = "bmp files (*.bmp)|*.bmp";
            if (dialog.ShowDialog() == true )
            {
                string path = dialog.FileName;

            }


        }
    }
}
