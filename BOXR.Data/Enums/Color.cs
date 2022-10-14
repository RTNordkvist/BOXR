using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOXR.Data.Enums
{
    public enum Color
    {
        [Description("")]
        Undecided,
        [Description("Fawn")]
        Fawn,
        [Description("Brindle")]
        Brindle,
        [Description("Fawn with white")]
        FawnWithWhite,
        [Description("Brindle with white")]
        BrindleWithWhite,
        [Description("Unwanted color")]
        UnwantedColor
    }
}
