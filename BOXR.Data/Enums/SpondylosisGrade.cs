using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOXR.Data.Enums
{
    public enum SpondylosisGrade
    {
        [Description("")]
        Undecided,
        [Description("0: None")]
        None,
        [Description("1: Transition State")]
        TransitionState,
        [Description("2: Mild")]
        Mild,
        [Description("3: Medium")]
        Medium,
        [Description("4: Severe")]
        Severe
    }
}
