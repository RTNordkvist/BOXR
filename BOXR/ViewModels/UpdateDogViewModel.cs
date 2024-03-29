﻿using BOXR.Data.Enums;
using BOXR.Data.Models;
using BOXR.DataAccess.Repositories;
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
    public class UpdateDogViewModel : BaseViewModel
    {
        private DogRepository dogRepository { get; set; }

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

        public string ViewTitle { get; } = "Update";
        public string SecondButtonPurpose { get; } = "Cancel";


        public ICommand NavigateToDogProfileCommand { get; set; }
        public ICommand SaveDogCommand => new RelayCommand(d => SaveDog(), d => CanSaveDog());
        public ICommand SecondButtonCommand => new RelayCommand(d => CancelUpdate());


        public override string Name { get; } = "Update";

        public UpdateDogViewModel(DogRepository dogRepository)
        {
            Dog = new DogDTO();
            this.dogRepository = dogRepository;
        }
        public void LoadDog(int id)
        {
            Dog = new DogDTO(dogRepository.Get(id));
        }

        /// <summary>
        /// Only enables the save button when certain information have been added by the user
        /// </summary>
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
                var dog = new Dog()
                {
                    Id = Dog.Id,
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
                var id = dogRepository.Update(dog);
                Dog = new DogDTO();

                var savedDog = dogRepository.Get(id);

                NavigateToDogProfileCommand.Execute(new DogDTO { Id = savedDog.Id });
            }
            catch (Exception e)
            {
                ErrorText = e.Message;
            }
        }

        public void CancelUpdate()
        {
            NavigateToDogProfileCommand.Execute(new DogDTO { Id = Dog.Id });
        }
    }
}
