using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.RIS.Enumerators.Common
{
    /// <summary>
    /// To define the common reference type(Eg : For scanning or sticky note etc)
    /// </summary>
    public enum RefType : int
    {
        /// <summary>
        /// Represent OrderWise in worklist
        /// </summary>
        WorkListOrderWise=0,

        /// <summary>
        /// Represent User profile 
        /// </summary>
        UserProfile = 1,
    }
}
