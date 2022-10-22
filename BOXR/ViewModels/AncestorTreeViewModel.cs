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
        //Defines how many generations should be included in the tree structure
        private const int ANCESTOR_GENERATIONS = 4;
        public override string Name => nameof(AncestorTreeViewModel);

        //AncesterTreeView code-behind subscribes on this event
        public Action<string> OnDogClicked;

        public ICommand ChangeProfileViewCommand => new RelayCommand(d => OnDogClicked.Invoke(d.ToString()));

        private readonly DogRepository _dogRepository;
        private readonly string _pedigreeNumber; //Base dog for the ancester tree

        public AncestorTreeViewModel(DogRepository dogRepository, string pedigreeNumber)
        {
            _dogRepository = dogRepository;
            _pedigreeNumber = pedigreeNumber;
        }


        public List<DogNode> LoadAncestorTree()
        {
            var ancestorTreeLogic = new AncestorTreeLogic(_dogRepository);
            var dogs = new List<DogNode>(); //Flat structure of the ancester tree
            var rootNode = ancestorTreeLogic.PopulateAncestorTree(_pedigreeNumber, ANCESTOR_GENERATIONS, null, 0, dogs);
            return dogs;
        }
    }
}
