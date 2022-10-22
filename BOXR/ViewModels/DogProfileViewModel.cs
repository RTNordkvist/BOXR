using BOXR.Data.Models;
using BOXR.DataAccess.Repositories;
using BOXR.Domain;
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
        //Defines how many generations should be included in the calculation of the inbreeding coefficient
        private const int ANCESTOR_GENERATIONS = 4;

        public DogRepository DogRepository { get; private set; }

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

        private double _inbreedingCoefficient;

        public double InbreedingCoefficient
        {
            get { return _inbreedingCoefficient; }
            set 
            { 
                _inbreedingCoefficient = value;
                RaisePropertyChanged(nameof(InbreedingCoefficient));
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
            this.DogRepository = dogRepository;
            Dog = new();
            Offspring = new ObservableCollection<DogDTO>();
        }

        public void LoadDog(int id)
        {
            var dog = DogRepository.Get(id);
            Dog = new DogDTO(dog);

            UpdateInbreedingCoefficient(dog);

            LoadOffspring();
        }

        public void LoadDog(string pedigreeNumber)
        {
            var dog = DogRepository.Get(pedigreeNumber);
            if (dog == null)
            {
                return;
            }

            Dog = new DogDTO(dog);

            UpdateInbreedingCoefficient(dog);

            LoadOffspring();
        }

        private void UpdateInbreedingCoefficient(Dog dog)
        {
            AncestorTreeLogic ancestorTreeLogic = new AncestorTreeLogic(DogRepository);
            InbreedingCoefficient = ancestorTreeLogic.CalculateInbreedingCoefficient(dog, ANCESTOR_GENERATIONS);
        }

        private void LoadOffspring()
        {
            var seachresult = DogRepository.FindOffspring(Dog.PedigreeNumber)
                .Select(x => new DogDTO(x));
            Offspring.RemoveAll();
            foreach (var offspring in seachresult)
            {
                Offspring.Add(offspring);
            }
        }
    }
}
