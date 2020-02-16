using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.RIS.Enumerators.Common
{
    public enum ValidationMessageType : int
    {
        /// <summary>
        /// Represent Information 
        /// </summary>
        Info = 1,
        /// <summary>
        /// Represents Blocking
        /// </summary>
        Blocking,
        /// <summary>
        /// Represents Warning
        /// </summary>
        Warning,
        /// <summary>
        /// Represents YesNoCancel
        /// </summary>
        YesNoCancel,
        /// <summary>
        /// Represents OkCancel
        /// </summary>
        OkCancel,
        /// <summary>
        /// Represents Hl7Interface to Mirth
        /// </summary>
        Hl7
    }
}
