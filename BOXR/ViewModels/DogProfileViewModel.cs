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
        public DogDTO Dog { get; set; } = new DogDTO();
        public override string Name { get; set; } = "Dog profile";
    }
}
