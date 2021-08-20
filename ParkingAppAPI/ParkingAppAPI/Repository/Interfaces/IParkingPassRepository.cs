using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingAppAPI.Models;
using ParkingAppAPI.ViewModels;

namespace ParkingAppAPI.Repository
{
    public interface IParkingPassRepository
    {
        Task<List<ParkingPassCdViewModel>> GetAllParkingPasses();

        Task<int> AddPass(ParkingPassCd pass);

        Task<int> DeletePass(string ParkingPassCd1);

        Task UpdatePass(ParkingPassCd pass);
    }
}
