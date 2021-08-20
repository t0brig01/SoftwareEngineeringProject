using System;
using System.Collections.Generic;

namespace ParkingAppAPI.Models
{
    public partial class ParkingLot
    {
        public ParkingLot()
        {
            ParkingMap = new HashSet<ParkingMap>();
        }

        public long Id { get; set; }
        public string ShortDesc { get; set; }
        public TimeSpan ExclusivePassStart { get; set; }
        public TimeSpan AnyPassStart { get; set; }
        public TimeSpan NoPassStart { get; set; }
        public string Address { get; set; }
        public bool WeekendEnforcementFlag { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public virtual ICollection<ParkingMap> ParkingMap { get; set; }
    }
}
