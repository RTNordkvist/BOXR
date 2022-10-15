using BOXR.Data.Enums;
using BOXR.Data.Models;
using BOXR.DataAccess.Repositories;
using BOXR.UI.Commands;
using BOXR.UI.ViewModels.EntityViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BOXR.UI.ViewModels
{
    public class RegisterDogViewModel : BaseViewModel
    {
        private DogRepository DogRepository { get; set; }

        public DogDTO _dog { get; set; }
        public DogDTO Dog
        {
            get => _dog;
            set
            {
                _dog = value;
                RaisePropertyChanged(nameof(Dog));
            }
        }

        /// <summary>
        /// Implementationen af denne sker i MainViewModel, dermed har RegisterDogViewModel ingen kendskab til hvordan man navigerer til home == loose coupling (hvilket er godt!)
        /// </summary>
        public ICommand NavigateHomeCommand { get; set; }
        public ICommand NavigateToDogProfileCommand { get; set; }
        public ICommand SaveDogCommand => new RelayCommand(d => SaveDog());
        public ICommand ClearDogCommand => new RelayCommand(d => ClearDog());

        public override string Name { get; set; } = "Register";

        public RegisterDogViewModel(DogRepository dogRepository)
        {
            Dog = new DogDTO();
            DogRepository = dogRepository;
        }

        public void SaveDog()
        {
            try
            {
                var dogEntity = DogRepository.Get(Dog.PedigreeNumber);
                if (dogEntity == null)
                {
                    var dog = new Dog()
                    {
                        Name = Dog.Name,
                        PedigreeNumber = Dog.PedigreeNumber,
                        BirthDate = Dog.BirthDate,
                        Color = Dog.Color,
                        Gender = Dog.Gender,
                        ChipNumber = Dog.ChipNumber,
                        HdGrade = Dog.HdGrade,
                        SpondylosisGrade = Dog.SpondylosisGrade,
                        HeartGrade = Dog.HeartGrade,
                        IsAlive = Dog.IsAlive,
                        MotherPedigreeNumber = Dog.MotherPedigreeNumber,
                        FatherPedigreeNumber = Dog.FatherPedigreeNumber,
                        Owner = Dog.Owner,
                        Breeder = Dog.Breeder,
                        Image = Dog.Image
                    };
                    var id = DogRepository.Add(dog);
                    Dog = new DogDTO();

                    var savedDog = DogRepository.Get(id);

                    NavigateToDogProfileCommand.Execute(new DogDTO { Id = savedDog.Id, PedigreeNumber = savedDog.PedigreeNumber, Breeder = savedDog.Breeder, Name = savedDog.Name });
                }
                else
                {
                    NavigateToDogProfileCommand.Execute(new DogDTO { Id = dogEntity.Id, PedigreeNumber = dogEntity.PedigreeNumber, Breeder = dogEntity.Breeder, Name = dogEntity.Name });
                }
            }
            catch
            {
                // sæt error flag til true
            }
        }

        public void ClearDog()
        {
            Dog = new DogDTO();
        }
    }
}
