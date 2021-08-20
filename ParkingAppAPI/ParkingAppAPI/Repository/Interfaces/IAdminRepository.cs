using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingAppAPI.Models;
using ParkingAppAPI.ViewModels;

namespace ParkingAppAPI.Repository
{
    public interface IAdminRepository
    {
        Task<List<AdminViewModel>> GetAllAdmins();

        Task<int> AddAdmin(Admin admin);

        Task<int> DeleteAdmin(string adminUsername);

        Task<int> UpdateAdmin(Admin admin);

        Task<int> ValidateLogin(Admin admin);
    }
}
