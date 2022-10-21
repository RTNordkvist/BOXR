using BOXR.DataAccess.Repositories;
using BOXR.Domain;
using BOXR.UI.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BOXR.UI.ViewModels
{
    public class AncestorTreeViewModel : BaseViewModel
    {
        private const int ANCESTOR_GENERATIONS = 4;

        public Action<string> OnDogClicked;

        public ICommand ChangeProfileViewCommand => new RelayCommand(d => OnDogClicked.Invoke(d.ToString()));

        private readonly DogRepository _dogRepository;
        private readonly string _pedigreeNumber;

        public AncestorTreeViewModel(DogRepository dogRepository, string pedigreeNumber)
        {
            _dogRepository = dogRepository;
            _pedigreeNumber = pedigreeNumber;
        }

        public override string Name => nameof(AncestorTreeViewModel);

        public List<DogNode> LoadAncestorTree()
        {
            var inbreedingCalculator = new InbreedingCalculator(_pedigreeNumber, ANCESTOR_GENERATIONS, _dogRepository);
            var dogs = new List<DogNode>();
            var rootNode = inbreedingCalculator.PopulateAncestorTree(_pedigreeNumber, null, 0, dogs);
            return dogs;
        }
    }
}
