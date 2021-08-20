using System;
using System.Collections.Generic;

namespace ParkingAppAPI.Models
{
    public partial class ParkingMap
    {
        public long Id { get; set; }
        public long ParkingLotId { get; set; }
        public string ParkingPassCd { get; set; }
        public bool PrimaryFlag { get; set; }

        public virtual ParkingLot ParkingLot { get; set; }
        public virtual ParkingPassCd ParkingPassCdNavigation { get; set; }
    }
}
