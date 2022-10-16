using BOXR.DataAccess.Repositories;
using BOXR.UI.Commands;
using BOXR.UI.Extensions;
using BOXR.UI.ViewModels.EntityViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BOXR.UI.ViewModels
{
    public class SearchDogViewModel : BaseViewModel
    {
        private DogRepository dogRepository { get; set; }

        public override string Name { get; set; } = "Search";


        private SearchDogDTO _searchCriteria;
        public SearchDogDTO SearchCriteria 
        { 
            get => _searchCriteria;
            set
            {
                _searchCriteria = value;
                RaisePropertyChanged(nameof(SearchCriteria));
            }
        }

        private DogDTO _selectedDog;
        public DogDTO SelectedDog
        {
            get => _selectedDog;
            set
            {
                NavigateToDogProfileCommand.Execute(value);
            }
        }

        public ObservableCollection<DogDTO> Dogs { get; set; }

        public ICommand SearchDogCommand => new RelayCommand(d => SearchDog());

        public ICommand ClearDogCommand => new RelayCommand(d => ClearDog());

        public ICommand NavigateToDogProfileCommand { get; set; }


        public SearchDogViewModel(DogRepository dogRepository)
        {
            this.dogRepository = dogRepository;
            SearchCriteria = new();
            Dogs = new();
        }

        private void SearchDog()
        {
            Dogs.RemoveAll();

            var seachresult = dogRepository.Find(SearchCriteria.PedigreeNumber, SearchCriteria.Name, SearchCriteria.Breeder)
                .Select(x => new DogDTO
                {
                    Id = x.Id,
                    Breeder = x.Breeder,
                    Name = x.Name,
                    PedigreeNumber = x.PedigreeNumber
                });

            foreach (var dog in seachresult)
            {
                Dogs.Add(dog);
            }
        }

        private void ClearDog()
        {
            SearchCriteria = new();
            Dogs.Clear();
        }
    }
}
