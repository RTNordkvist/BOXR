using BOXR.Data.Enums;
using BOXR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BOXR.UI.ViewModels.EntityViewModels
{
    public class DogDTO : BaseDTO
    {
        public DogDTO() { }

        public DogDTO(Dog dog)
        {
            Id = dog.Id;
            PedigreeNumber = dog.PedigreeNumber;
            Name = dog.Name;
            BirthDate = dog.BirthDate;
            Gender = dog.Gender;
            ChipNumber = dog.ChipNumber;
            HdGrade = dog.HdGrade;
            SpondylosisGrade = dog.SpondylosisGrade;
            HeartGrade = dog.HeartGrade;
            Color = dog.Color;
            IsAlive = dog.IsAlive;
            MotherPedigreeNumber = dog.MotherPedigreeNumber;
            FatherPedigreeNumber = dog.FatherPedigreeNumber;
            Breeder = dog.Breeder;
            Image = dog.Image;
            Owner = dog.Owner;
        }
        public int Id { get; set; }

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

        private Gender _gender = Gender.Undecided;
        public Gender Gender
        {
            get { return _gender; }
            set
            {
                if (Gender != value)
                {
                    _gender = value;
                }
                RaisePropertyChanged("Gender");
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

        private HdGrade _hdGrade;
        public HdGrade HdGrade
        {
            get { return _hdGrade; }
            set
            {
                if (HdGrade != value)
                {
                    _hdGrade = value;
                }
                RaisePropertyChanged("HdGrade");
            }
        }

        private SpondylosisGrade _spondylosisGrade;
        public SpondylosisGrade SpondylosisGrade
        {
            get { return _spondylosisGrade; }
            set
            {
                if (SpondylosisGrade != value)
                {
                    _spondylosisGrade = value;
                }
                RaisePropertyChanged("SpondylosisGrade");
            }
        }

        private HeartGrade _heartGrade;
        public HeartGrade HeartGrade
        {
            get { return _heartGrade; }
            set
            {
                if (HeartGrade != value)
                {
                    _heartGrade = value;
                }
                RaisePropertyChanged("HeartGrade");
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

        private bool _isAlive = true;
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

        private string _image;

        public string Image
        {
            get { return _image; }
            set 
            {
                _image = value;
                RaisePropertyChanged(nameof(Image));
            }
        }
    }
}
