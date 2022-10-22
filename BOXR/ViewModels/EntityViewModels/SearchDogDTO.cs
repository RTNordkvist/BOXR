using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOXR.UI.ViewModels.EntityViewModels
{
    //Search can be done for pedigreeNumber, name or breeder
    public class SearchDogDTO : BaseDTO
    {
        private string _pedigreeNumber;
        public string PedigreeNumber
        {
            get { return _pedigreeNumber; }
            set
            {
                if (_pedigreeNumber != value)
                {
                    _pedigreeNumber = value;
                    RaisePropertyChanged("PedigreeNumber");
                };
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }

        private string _breeder;
        public string Breeder
        {
            get { return _breeder; }
            set
            {
                if (Breeder != value)
                {
                    _breeder = value;
                }
                RaisePropertyChanged("Breeder");
            }
        }
    }
}
