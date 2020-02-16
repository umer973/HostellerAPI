using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.RIS.Enumerators.Common
{
    public enum DTOState : int
    {
        /// <summary>
        /// Represent insert
        /// </summary>
        Insert = 0,

        /// <summary>
        /// Represents Update
        /// </summary>
        Update,

        /// <summary>
        /// Represents Delete
        /// </summary>
        Delete
    }
}
