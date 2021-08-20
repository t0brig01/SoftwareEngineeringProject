using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ParkingAppAPI.ViewModels
{
	public class MapViewModel
	{
		public long Id { get; set; }
		public long ParkingLotId { get; set; }
		public string ParkingPassCd { get; set; }
		public bool PrimaryFlag { get; set; }
	}
}
