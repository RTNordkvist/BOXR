using BOXR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOXR.UI.ViewModels.EntityViewModels
{
    public class DogDTO : BaseDTO
    {
        private string _pedigreeNumber;
        public string PedigreeNumber
        {
            get { return _pedigreeNumber; }
            set
            {
                if (PedigreeNumber != value)
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
                if (Name != value)
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

        private DateTime? _birthDate;
        public DateTime? BirthDate { 
            get { return _birthDate; }
            set
            {
                if (BirthDate != value)
                {
                    _birthDate = value;
                }
                RaisePropertyChanged("BirthDate");
            }
        }

        private char? _gender;
        public char? Gender
        {
            get { return _gender; }
            set
            {
                if (Gender != value)
                {
                    _gender = value;
                }
                RaisePropertyChanged("Breeder");
            }
        }

        private string _chipNumber;
        public string ChipNumber
        {
            get { return _chipNumber; }
            set
            {
                if (ChipNumber != value)
                {
                    _chipNumber = value;
                }
                RaisePropertyChanged("ChipNumber");
            }
        }

        private double? _inbreedingCoefficient;
        public double? InbreedingCoefficient
        {
            get { return _inbreedingCoefficient; }
            set
            {
                if (InbreedingCoefficient != value)
                {
                    _inbreedingCoefficient = value;
                }
                RaisePropertyChanged("InbreedingCoefficient");
            }
        }

        private char? _hdStatus;
        public char? HdStatus
        {
            get { return _hdStatus; }
            set
            {
                if (HdStatus != value)
                {
                    _hdStatus = value;
                }
                RaisePropertyChanged("HdStatus");
            }
        }

        private int? _hdIndex;
        public int? HdIndex
        {
            get { return _hdIndex; }
            set
            {
                if (HdIndex != value)
                {
                    _hdIndex = value;
                }
                RaisePropertyChanged("HdIndex");
            }
        }

        private int? _spondylosisStatus;
        public int? SpondylosisStatus
        {
            get { return _spondylosisStatus; }
            set
            {
                if (SpondylosisStatus != value)
                {
                    _spondylosisStatus = value;
                }
                RaisePropertyChanged("SpondylosisStatus");
            }
        }

        private int? _heartStatus;
        public int? HeartStatus
        {
            get { return _heartStatus; }
            set
            {
                if (HeartStatus != value)
                {
                    _heartStatus = value;
                }
                RaisePropertyChanged("HeartStatus");
            }
        }

        private Color _color;
        public Color Color
        {
            get { return _color; }
            set
            {
                if (Color != value)
                {
                    _color = value;
                }
                RaisePropertyChanged("Color");
            }
        }

        private bool _isAlive;
        public bool IsAlive
        {
            get { return _isAlive; }
            set
            {
                if (IsAlive != value)
                {
                    _isAlive = value;
                }
                RaisePropertyChanged("IsAlive");
            }
        }

        private string _motherPedigreeNumber;
        public string MotherPedigreeNumber
        {
            get { return _motherPedigreeNumber; }
            set
            {
                if (MotherPedigreeNumber != value)
                {
                    _motherPedigreeNumber = value;
                    RaisePropertyChanged("MotherPedigreeNumber");
                };
            }
        }

        private string _fatherPedigreeNumber;
        public string FatherPedigreeNumber
        {
            get { return _fatherPedigreeNumber; }
            set
            {
                if (FatherPedigreeNumber != value)
                {
                    _fatherPedigreeNumber = value;
                    RaisePropertyChanged("FatherPedigreeNumber");
                };
            }
        }

        private string _owner;
        public string Owner
        {
            get { return _owner; }
            set
            {
                if (Owner != value)
                {
                    _owner = value;
                    RaisePropertyChanged("Owner");
                };
            }
        }
    }
}
