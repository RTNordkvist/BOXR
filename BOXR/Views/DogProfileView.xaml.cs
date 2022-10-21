using BOXR.DataAccess.Repositories;
using BOXR.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
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
    /// Interaction logic for DogProfileView.xaml
    /// </summary>
    public partial class DogProfileView : UserControl
    {
        public DogProfileView()
        {
            InitializeComponent();
        }

        private AncestorTreeView _ancestorTreeView;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DogProfileViewModel)DataContext;
            var ancestorViewModel = new AncestorTreeViewModel(viewModel.DogRepository, viewModel.Dog.PedigreeNumber);
            ancestorViewModel.OnDogClicked += (d) => 
            { 
                viewModel.LoadDog(d);
                Application.Current.MainWindow.Activate();
            };
            
            if(_ancestorTreeView == null)
            {
                _ancestorTreeView = new AncestorTreeView();
            }
            _ancestorTreeView.DataContext = ancestorViewModel;
            _ancestorTreeView.GenerateTree(ancestorViewModel);
            _ancestorTreeView.Show();
            _ancestorTreeView.Activate();
        }
    }
}
