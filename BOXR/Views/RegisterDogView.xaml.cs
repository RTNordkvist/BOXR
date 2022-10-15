using BOXR.UI.ViewModels;
using Microsoft.Win32;
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

namespace BOXR.UI.Views
{
    /// <summary>
    /// Interaction logic for RegisterDogView.xaml
    /// </summary>
    public partial class RegisterDogView : UserControl
    {
        public RegisterDogView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new();
            openDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openDialog.FilterIndex = 1;
            if(openDialog.ShowDialog() == true)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(openDialog.FileName);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                ((RegisterDogViewModel)DataContext).Dog.Image = base64ImageRepresentation;
            }
        }
    }
}
