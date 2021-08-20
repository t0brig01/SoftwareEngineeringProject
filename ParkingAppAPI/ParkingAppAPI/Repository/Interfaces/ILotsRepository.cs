using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingAppAPI.Models;
using ParkingAppAPI.ViewModels;

namespace ParkingAppAPI.Repository
{
    public interface ILotsRepository
    {

        Task<List<ParkingLotViewModel>> GetAllLots();

        Task<ParkingLotViewModel> GetLot(int lotID);

        Task<long> AddLot(ParkingLot lot);

        Task<int> DeleteLot(int lotId);

        Task UpdateLot(ParkingLot lot);

        Task<List<ParkingLotViewModel>> GetLotsByPass(string pass);
    }
}
