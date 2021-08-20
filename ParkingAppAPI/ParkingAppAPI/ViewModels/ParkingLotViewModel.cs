using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingAppAPI.ViewModels
{
    public class ParkingLotViewModel
    {
        public long Id { get; set; }
        public string ShortDesc { get; set; }
        public TimeSpan ExclusivePassStart { get; set; }
        public TimeSpan ExclusivePassEnd { get; set; }
        public TimeSpan AnyPassStart { get; set; }
        public TimeSpan AllPassStart { get; set; }
        public string Address { get; set; }
        public bool WeekendEnforcementFlag { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
