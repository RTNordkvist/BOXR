using BOXR.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOXR.Data.Repositories
{
    public class ColorRepository
    {
        private List<Color> colors = new List<Color>();

        public ColorRepository()
        {
            InitialzeRepository();
        }

        private void InitialzeRepository()
        {
            colors.Add(new Color { Name = "Fawn" });
            colors.Add(new Color { Name = "Brindle" });
            colors.Add(new Color { Name = "Fawn with white" });
            colors.Add(new Color { Name = "Brindle with white" });
            colors.Add(new Color { Name = "Unwanted color" });
        }
        public List<Color> GetAll()
        {
            return colors;
        }
    }
}
