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


        //Get the ancestor tree and opens the window for presenting it
        private void OpenAncesterTree(object sender, RoutedEventArgs e)
        {
            var viewModel = (DogProfileViewModel)DataContext;
            var ancestorViewModel = new AncestorTreeViewModel(viewModel.DogRepository, viewModel.Dog.PedigreeNumber);
            
            //Subscribes on the event that a dog is clicked on in the ancester tree and then updates the dog in DogProfileViewModel
            ancestorViewModel.OnDogClicked += (d) => 
            { 
                viewModel.LoadDog(d);
                Application.Current.MainWindow.Activate();
            };
            
            if(_ancestorTreeView == null || _ancestorTreeView != null && !Application.Current.Windows.OfType<AncestorTreeView>().Any(x => x == _ancestorTreeView))
            {
                Application.Current.Windows.OfType<AncestorTreeView>().ToList().ForEach(x => x.Close());
                _ancestorTreeView = new AncestorTreeView();
            }
            _ancestorTreeView.DataContext = ancestorViewModel;
            _ancestorTreeView.GenerateTree(ancestorViewModel);
            _ancestorTreeView.Show();
            _ancestorTreeView.Activate();
        }
    }
}
