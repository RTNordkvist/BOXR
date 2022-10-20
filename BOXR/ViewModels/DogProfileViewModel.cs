using BOXR.Data.Models;
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
    public class DogProfileViewModel : BaseViewModel
    {
        private readonly DogRepository dogRepository;
        public override string Name { get; } = "Dog profile";

        private DogDTO _dog;
        public DogDTO Dog
        {
            get => _dog;
            private set
            {
                _dog = value;
                RaisePropertyChanged(nameof(Dog));
            }
        }

        public ObservableCollection<DogDTO> Offspring { get; set; }

        private DogDTO _selectedOffspring;
        public DogDTO SelectedOffspring
        {
            get => _selectedOffspring;
            set
            {
                NavigateToOffspringCommand.Execute(value);
            }
        }

        public ICommand NavigateToUpdateDogCommand { get; set; }
        public ICommand NavigateToOffspringCommand { get; set; }
        public ICommand NavigateToParentCommand { get; set; }


        public DogProfileViewModel(DogRepository dogRepository)
        {
            this.dogRepository = dogRepository;
            Dog = new();
            Offspring = new ObservableCollection<DogDTO>();
        }

        public void LoadDog(int id)
        {
            var dog = dogRepository.Get(id);
            Dog = new DogDTO(dog);
            var seachresult = dogRepository.FindOffspring(Dog.PedigreeNumber)
                .Select(x => new DogDTO(x));

            Offspring.RemoveAll();
            foreach (var offspring in seachresult)
            {
                Offspring.Add(offspring);
            }
        }

        public void LoadDog(string pedigreeNumber)
        {
            var dog = dogRepository.Get(pedigreeNumber);
            if (dog == null)
            {
                return;
            }

            Dog = new DogDTO(dog);
            var seachresult = dogRepository.FindOffspring(Dog.PedigreeNumber)
                .Select(x => new DogDTO(x));

            Offspring.RemoveAll();
            foreach (var offspring in seachresult)
            {
                Offspring.Add(offspring);
            }
        }
    }
}
