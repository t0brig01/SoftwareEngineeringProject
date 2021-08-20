using System;
using System.Collections.Generic;

namespace ParkingAppAPI.Models
{
    public partial class ParkingPassCd
    {
        public ParkingPassCd()
        {
            ParkingMap = new HashSet<ParkingMap>();
        }

        public string ParkingPassCd1 { get; set; }
        public string ShortDesc { get; set; }
        public int DisplaySequence { get; set; }
        public bool ActiveFlag { get; set; }

        public virtual ICollection<ParkingMap> ParkingMap { get; set; }
    }
}
