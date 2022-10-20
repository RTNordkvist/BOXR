using BOXR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOXR.Domain
{
    public class DogNode
    {
        public string PedigreeNumber { get; set; }
        public DogNode Mother { get; set; }
        public DogNode Father { get; set; }
        public DogNode Child { get; set; }
        public int GenerationsFromBase { get; set; }
    }
}
