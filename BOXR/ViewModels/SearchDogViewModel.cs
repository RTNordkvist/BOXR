using BOXR.Data.Repositories;
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
        private DogRepository DogRepository { get; set; }

        public override string Name { get; set; } = "Search";

        public SearchDogDTO SearchCriteria { get; set; }

        private DogDTO _selectedDog;
        public DogDTO SelectedDog
        {
            get => _selectedDog;
            set
            {
                _selectedDog = value;
                RaisePropertyChanged(nameof(SelectedDog));
                NavigateToDogProfileCommand.Execute(SelectedDog);
            }
        }

        public ObservableCollection<DogDTO> Dogs { get; set; }

        public ICommand SearchDogCommand => new RelayCommand(d => SearchDog());

        public ICommand NavigateToDogProfileCommand { get; set; }


        public SearchDogViewModel(DogRepository dogRepository)
        {
            DogRepository = dogRepository;
            SearchCriteria = new();
            Dogs = new();
        }

        private void SearchDog()
        {
            Dogs.RemoveAll();

            var seachresult = DogRepository.Find(SearchCriteria.PedigreeNumber, SearchCriteria.Name, SearchCriteria.Breeder)
                .Select(x => new DogDTO
                {
                    Breeder = x.Breeder,
                    Name = x.Name,
                    PedigreeNumber = x.PedigreeNumber
                });

            foreach (var dog in seachresult)
            {
                Dogs.Add(dog);
            }
        }
    }
}
