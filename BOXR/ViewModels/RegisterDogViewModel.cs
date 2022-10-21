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
        private DogRepository dogRepository { get; set; }

        private DogDTO _dog;
        public DogDTO Dog
        {
            get => _dog;
            set
            {
                _dog = value;
                RaisePropertyChanged(nameof(Dog));
            }
        }

        private string errorText;
        public string ErrorText
        {
            get { return errorText; }
            set 
            { 
                errorText = value;
                RaisePropertyChanged(nameof(ErrorText));
            }
        }


        public string ViewTitle { get; } = "Register new dog";

        /// <summary>
        /// Implementationen af denne sker i MainViewModel, dermed har RegisterDogViewModel ingen kendskab til hvordan man navigerer til home == loose coupling (hvilket er godt!)
        /// </summary>
        //public ICommand NavigateHomeCommand { get; set; }
        public ICommand NavigateToDogProfileCommand { get; set; }
        public ICommand SaveDogCommand => new RelayCommand(d => SaveDog(), d => CanSaveDog());
        public ICommand ClearDogCommand => new RelayCommand(d => ClearDog());

        public override string Name { get; } = "Register";

        public RegisterDogViewModel(DogRepository dogRepository)
        {
            Dog = new DogDTO();
            this.dogRepository = dogRepository;
        }

        public bool CanSaveDog()
        {
            if (Dog == null)
                return false;

            return !string.IsNullOrWhiteSpace(Dog.PedigreeNumber) 
                && !string.IsNullOrWhiteSpace(Dog.Name) 
                && Dog.BirthDate != null
                && !string.IsNullOrEmpty(Dog.ChipNumber)
                && !string.IsNullOrEmpty(Dog.Breeder)
                && Dog.Gender != Gender.Undecided
                && Dog.Color != Color.Undecided;
        }

        public void SaveDog()
        {
            try
            {
                var dogEntity = dogRepository.Get(Dog.PedigreeNumber);
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
                    var id = dogRepository.Add(dog);
                    Dog = new DogDTO();

                    var savedDog = dogRepository.Get(id);

                    NavigateToDogProfileCommand.Execute(new DogDTO { Id = savedDog.Id});
                }
                else
                {
                    throw new Exception("The pedigree number already exists in the system");
                    //NavigateToDogProfileCommand.Execute(new DogDTO { Id = dogEntity.Id});
                }
            }
            catch (Exception e)
            {
                ErrorText = e.Message;
            }
        }

        public void ClearDog()
        {
            Dog = new DogDTO();
        }
    }
}
