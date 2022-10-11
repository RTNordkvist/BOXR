using BOXR.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOXR.Data.Models
{
    public class Dog
    {
        public int Id { get; set; }
        public string PedigreeNumber { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; } // TODO create an enum
        public string ChipNumber { get; set; }
        public double? InbreedingCoefficient { get; set; } // TODO remove from this class as this class should represent exactly what is in the database
        public HdGrade? HdGrade { get; set; } // TODO create an enum
        public int? HdIndex { get; set; } // TODO remove
        public SpondylosisGrade? SpondylosisGrade { get; set; } // TODO create an enum
        public HearthGrade? HeartGrade { get; set; } // TODO create an enum
        public Color Color { get; set; } // TODO foreign key, color is another table
        public bool IsAlive { get; set; }
        public string MotherPedigreeNumber { get; set; }
        public string FatherPedigreeNumber { get; set; }
        public string Owner { get; set; } // TODO foreign key, Owner is another table
        public string Breeder { get; set; } // TODO foreign key, Breeder is another table
    }
}
