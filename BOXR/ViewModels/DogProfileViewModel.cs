using BOXR.DataAccess.Repositories;
using BOXR.UI.ViewModels.EntityViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOXR.UI.ViewModels
{
    public class DogProfileViewModel : BaseViewModel
    {
        private readonly DogRepository dogRepository;

        public DogDTO Dog { get; private set; } = new DogDTO();
        public override string Name { get; set; } = "Dog profile";

        public DogProfileViewModel(DogRepository dogRepository)
        {
            this.dogRepository = dogRepository;
        }

        public void LoadDog(int id)
        {
            Dog = new DogDTO(dogRepository.Get(id));
        }
    }
}
