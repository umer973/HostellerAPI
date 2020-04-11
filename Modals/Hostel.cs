using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modals
{
    public class Hostel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string emailId { get; set; }
        public string address { get; set; }
        public string websiteLink { get; set; }
        public string profilePic { get; set; }
        public Int32 hostelDormRoomwithBunks { get; set; }
        public Int32 hostelDormRoomwithoutBunks { get; set; }
        public Int32 femaleDormRooms { get; set; }
        public Int32 singleBedRooms { get; set; }
        public Int32 doubleBedRooms { get; set; }
        public string hostelName { get; set; }
    }
}
