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
        public Gender Gender { get; set; }
        public string ChipNumber { get; set; }
        public HdGrade HdGrade { get; set; }
        public SpondylosisGrade SpondylosisGrade { get; set; }
        public HeartGrade HeartGrade { get; set; }
        public Color Color { get; set; }
        public bool IsAlive { get; set; }
        public string MotherPedigreeNumber { get; set; }
        public string FatherPedigreeNumber { get; set; }
        public string Owner { get; set; } // TODO foreign key, Owner is another table
        public string Breeder { get; set; } // TODO foreign key, Breeder is another table
        public string Image { get; set; }
    }
}
