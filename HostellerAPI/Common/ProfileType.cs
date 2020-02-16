using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*-----------------------------------------------------------------------
<Copyright file="Filename.cs" Company="Kameda Infologics">
    Copyright@Kameda Infologics Pvt Ltd. All rights reserved.
</Copyright>

 Description     :To define the login users (employee,patient etc) 
 Created  By     :CreatedBy 
 Created  Date   :CreatedDate 
 Modified By     :ModifiedBy  
 Modified Date   :ModifiedDate 
 Modified Purpose:ModifiedPur 
 -----------------------------------------------------------------------*/


namespace KI.RIS.Enumerators.Common
{
    public enum ProfileType : int
    {
        /// <summary>
        /// Represent patient
        /// </summary>
        Patient = 0,

        /// <summary>
        /// Represents employee
        /// </summary>
        Employee,

        /// <summary>
        /// Represents Hospital
        /// </summary>
        Hospital
    }
}
