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

        public List<BaseViewModel> PageViewModels { get; private set; } = new();

        public List<BaseViewModel> MenuViewModels { get; private set; } = new();

        private BaseViewModel _currentViewModel;
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

        public MainViewModel(DogRepository dogRepository)
        {
            new InbreedingCalculator("20382/89", 3, dogRepository);

            PageViewModels.Add(new HomeViewModel());

            var searchDogViewModel = new SearchDogViewModel(dogRepository);
            searchDogViewModel.NavigateToDogProfileCommand = new RelayCommand(d =>
            {
                var viewModel = (DogProfileViewModel)PageViewModels.First(x => x.GetType() == typeof(DogProfileViewModel));
                viewModel.LoadDog(((DogDTO)d).Id);
                ChangeViewModel(viewModel);
            });
            PageViewModels.Add(searchDogViewModel);

            var registerDogViewModel = new RegisterDogViewModel(dogRepository);
            //registerDogViewModel.NavigateHomeCommand = new RelayCommand(d => ChangeViewModel(PageViewModels.First(x => x.GetType() == typeof(HomeViewModel))));
            registerDogViewModel.NavigateToDogProfileCommand = new RelayCommand(d =>
            {
                var viewModel = (DogProfileViewModel)PageViewModels.First(x => x.GetType() == typeof(DogProfileViewModel));
                viewModel.LoadDog(((DogDTO)d).Id);
                ChangeViewModel(viewModel);
            });
            PageViewModels.Add(registerDogViewModel);

            var updateDogViewModel = new UpdateDogViewModel(dogRepository);
            updateDogViewModel.NavigateToDogProfileCommand = new RelayCommand(d =>
            {
                var viewModel = (DogProfileViewModel)PageViewModels.First(x => x.GetType() == typeof(DogProfileViewModel));
                viewModel.LoadDog(((DogDTO)d).Id);
                ChangeViewModel(viewModel);
            });
            PageViewModels.Add(updateDogViewModel);

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

            CurrentViewModel = PageViewModels.First(x => x.GetType() == typeof(HomeViewModel));

            MenuViewModels.Add(PageViewModels.First(x => x.GetType() == typeof(RegisterDogViewModel)));
            MenuViewModels.Add(PageViewModels.First(x => x.GetType() == typeof(SearchDogViewModel)));
        }

        private void ChangeViewModel(BaseViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentViewModel = PageViewModels.FirstOrDefault(vm => vm == viewModel);
        }
    }
}
