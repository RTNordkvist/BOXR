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
        public override string Name { get; set; } = "Dog profile";

        public DogDTO Dog { get; private set; }

        public ObservableCollection<DogDTO> Offspring { get; set; }

        public DogDTO SelectedOffspring { get; set; }


        public ICommand UpdateDogCommand => new RelayCommand(d => UpdateDog());
        public ICommand NavigateToUpdateDogCommand { get; set; }

        public DogProfileViewModel(DogRepository dogRepository)
        {
            this.dogRepository = dogRepository;
            Dog = new();
            Offspring = new ObservableCollection<DogDTO>();
        }

        public void LoadDog(int id)
        {
            Offspring.RemoveAll();
            var dog = dogRepository.Get(id);
            Dog = new DogDTO(dog);
            var seachresult = dogRepository.FindOffspring(Dog.PedigreeNumber)
                .Select(x => new DogDTO
                {
                    Id = x.Id,
                    PedigreeNumber = x.PedigreeNumber,
                    Name = x.Name
                });

            foreach (var offspring in seachresult)
            {
                Offspring.Add(offspring);
            }
        }

        public void UpdateDog()
        {
            NavigateToUpdateDogCommand.Execute(Dog);
        }
    }
}
