using BOXR.DataAccess.Repositories;
using BOXR.Domain;
using BOXR.UI.Commands;
using BOXR.UI.ViewModels.EntityViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BOXR.UI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public override string Name { get; } = "Main Window";

        public List<BaseViewModel> PageViewModels { get; private set; } = new(); //Contains all viewmodels for views displayed in the MainWindow content

        public List<BaseViewModel> MenuViewModels { get; private set; } = new(); //Contains viewmodels for views the can be navigated to from the top navigation bar

        private BaseViewModel _currentViewModel; //Viewmodel for the view currently displayed
        public BaseViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                RaisePropertyChanged(nameof(CurrentViewModel));
            }
        }

        private ICommand _navigateHomeCommand;
        public ICommand NavigateHomeCommand
        {
            get
            {
                if (_navigateHomeCommand == null)
                {
                    _navigateHomeCommand = new RelayCommand(
                        p => ChangeViewModel(PageViewModels.First(x => x.GetType() == typeof(HomeViewModel))),
                        p => true);
                }

                return _navigateHomeCommand;
            }
        }

        private ICommand _changePageCommand;
        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((BaseViewModel)p),
                        p => p is BaseViewModel);
                }

                return _changePageCommand;
            }
        }

        /// <summary>
        /// The implmentation of all navigation commands happens in MainViewModel, 
        /// so that none of the individual viewModels know how to navigate to a different page = loose coupling
        /// </summary>
        public MainViewModel(DogRepository dogRepository)
        {
            //All view models with a corresponding view to be displayed as content in MainWindow are created here
            PageViewModels.Add(new HomeViewModel());

            //Search Dog
            var searchDogViewModel = new SearchDogViewModel(dogRepository);
            searchDogViewModel.NavigateToDogProfileCommand = new RelayCommand(d =>
            {
                var viewModel = (DogProfileViewModel)PageViewModels.First(x => x.GetType() == typeof(DogProfileViewModel));
                viewModel.LoadDog(((DogDTO)d).Id);
                ChangeViewModel(viewModel);
            });
            PageViewModels.Add(searchDogViewModel);

            //Register Dog
            var registerDogViewModel = new RegisterDogViewModel(dogRepository);
            registerDogViewModel.NavigateToDogProfileCommand = new RelayCommand(d =>
            {
                var viewModel = (DogProfileViewModel)PageViewModels.First(x => x.GetType() == typeof(DogProfileViewModel));
                viewModel.LoadDog(((DogDTO)d).Id);
                ChangeViewModel(viewModel);
            });
            PageViewModels.Add(registerDogViewModel);

            //Update Dog
            var updateDogViewModel = new UpdateDogViewModel(dogRepository);
            updateDogViewModel.NavigateToDogProfileCommand = new RelayCommand(d =>
            {
                var viewModel = (DogProfileViewModel)PageViewModels.First(x => x.GetType() == typeof(DogProfileViewModel));
                viewModel.LoadDog(((DogDTO)d).Id);
                ChangeViewModel(viewModel);
            });
            PageViewModels.Add(updateDogViewModel);

            //Dog profile
            var dogProfileViewModel = new DogProfileViewModel(dogRepository);
            dogProfileViewModel.NavigateToUpdateDogCommand = new RelayCommand(d =>
            {
                var viewModel = (UpdateDogViewModel)PageViewModels.First(x => x.GetType() == typeof(UpdateDogViewModel));
                viewModel.LoadDog(((DogDTO)d).Id);
                ChangeViewModel(viewModel);
            });
            dogProfileViewModel.NavigateToOffspringCommand = new RelayCommand(d =>
            {
                var viewModel = (DogProfileViewModel)PageViewModels.First(x => x.GetType() == typeof(DogProfileViewModel));
                viewModel.LoadDog(((DogDTO)d).Id);
                ChangeViewModel(viewModel);
            });
            dogProfileViewModel.NavigateToParentCommand = new RelayCommand(d =>
            {
                var viewModel = (DogProfileViewModel)PageViewModels.First(x => x.GetType() == typeof(DogProfileViewModel));
                if (d != null)
                {
                    viewModel.LoadDog(d.ToString());
                    ChangeViewModel(viewModel);
                }
            });
            PageViewModels.Add(dogProfileViewModel);

            //Default view on startup is set
            CurrentViewModel = PageViewModels.First(x => x.GetType() == typeof(HomeViewModel));

            //Views with links in the top menu are added to MenuViewModels
            MenuViewModels.Add(PageViewModels.First(x => x.GetType() == typeof(RegisterDogViewModel)));
            MenuViewModels.Add(PageViewModels.First(x => x.GetType() == typeof(SearchDogViewModel)));
        }

        //This method defines which view should be showed as the content of the MainWindow by changing the viewmodel
        private void ChangeViewModel(BaseViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
        }
    }
}
