using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingAppAPI.Models;
using ParkingAppAPI.ViewModels;

namespace ParkingAppAPI.Repository
{
	public interface IMapRepository
	{
		Task<List<MapViewModel>> GetAllMaps();

		Task<int> AddMap(ParkingMap map);

		Task<int> DeleteMap(int id);

		Task<int> DeleteMapByLotId(int lotId);

		Task<int> DeleteMapByPassColor(string cd);

		Task UpdateMap(ParkingMap map);
	}
}
