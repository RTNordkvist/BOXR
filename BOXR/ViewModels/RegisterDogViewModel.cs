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

        public DogDTO Dog { get; set; }

        /// <summary>
        /// Implementationen af denne sker i MainViewModel, dermed har RegisterDogViewModel ingen kendskab til hvordan man navigerer til home == loose coupling (hvilket er godt!)
        /// </summary>
        public ICommand NavigateHomeCommand { get; set; }
        public ICommand NavigateToDogProfileCommand { get; set; }
        public ICommand SaveDogCommand => new RelayCommand(d => SaveDog());

        public override string Name { get; set; } = "Register";

        public List<Color> Colors { get; set; }

        public RegisterDogViewModel(DogRepository dogRepository, ColorRepository colorRepository)
        {
            Dog = new DogDTO();
            DogRepository = dogRepository;
            Colors = colorRepository.GetAll();
        }

        public void SaveDog()
        {
            var dogEntity = DogRepository.Get(Dog.PedigreeNumber);
            if (dogEntity == null)
            {
                var dog = new Dog()
                {
                    Name = Dog.Name,
                    PedigreeNumber = Dog.PedigreeNumber,
                    BirthDate = Dog.BirthDate,
                    //Gender = Dog.Gender,
                    ChipNumber = Dog.ChipNumber,
                    InbreedingCoefficient = Dog.InbreedingCoefficient,
                    //HdStatus = Dog.HdStatus,
                    HdIndex = Dog.HdIndex,
                    //SpondylosisStatus = Dog.SpondylosisStatus,
                    //HeartStatus = Dog.HeartStatus,
                    Color = Dog.Color,
                    IsAlive = Dog.IsAlive,
                    MotherPedigreeNumber = Dog.MotherPedigreeNumber,
                    FatherPedigreeNumber = Dog.FatherPedigreeNumber,
                    Owner = Dog.Owner,
                    Breeder = Dog.Breeder
                };
                var id = DogRepository.Add(dog);
                Dog = new DogDTO();

                var savedDog = DogRepository.Get(id);

                NavigateToDogProfileCommand.Execute(new DogDTO { PedigreeNumber = savedDog.PedigreeNumber, Breeder = savedDog.Breeder, Name = savedDog.Name });
            }
            else
            {
                NavigateToDogProfileCommand.Execute(new DogDTO { PedigreeNumber = dogEntity.PedigreeNumber, Breeder = dogEntity.Breeder, Name = dogEntity.Name });
            }
        }
    }
}
