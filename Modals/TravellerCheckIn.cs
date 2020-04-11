using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modals
{
    public class TravellerCheckIn
    {
        public string travellerQRCode { get; set; }
        public Int64 hostelId { get; set; }
        public DateTime checkInDate { get; set; }
        public DateTime checkOutDate { get; set; }
    }
}
