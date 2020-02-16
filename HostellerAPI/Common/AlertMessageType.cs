using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI.RIS.Enumerators.Common
{
    public enum AlertMessageType : int
    {
        /// <summary>
        /// Represent OTP Message to Patient
        /// </summary>
        Pat_OTP = 0,

        /// <summary>
        /// Represents Red Flag Notification to Emp
        /// </summary>
        Emp_RedFlagNotification =1,

        /// <summary>
        /// Represent OTP Message to Emp
        /// </summary>
        Emp_OTP = 2,

        /// <summary>
        /// Represent Urgent Finding Message to Emp
        /// </summary>
        Emp_UrgentFinding = 3
    }
}
