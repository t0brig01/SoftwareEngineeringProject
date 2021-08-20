using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ParkingAppAPI.Models;
using System.Data.SqlClient;
using ParkingAppAPI.ViewModels;
using System.Data;


namespace ParkingAppAPI.Repository
{
    public class LotsRepository:ILotsRepository
    {
        ParkingAppDatabaseContext db;
        public LotsRepository(ParkingAppDatabaseContext _db)
        {
            db = _db;
        }

        public async Task<long> AddLot(ParkingLot lot)
        {
            if (db != null)
            {
                await db.ParkingLot.AddAsync(lot);
                await db.SaveChangesAsync();
                long id = lot.Id;
                return id;
            }

            return 0;
        }

        public async Task<int> DeleteLot(int lotId)
        {
            int result = 0;

            if (db != null)
            {
                var lot = await db.ParkingLot.FirstOrDefaultAsync(x => x.Id == lotId);

                if (lot != null)
                {
                    db.ParkingLot.Remove(lot);
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<ParkingLotViewModel>> GetAllLots()
        {
            if (db != null)
            {
                var parkingLots = await db.ParkingLot.FromSql("exec p_parking_lot_select_raw").ToListAsync();
                var parkingLotViewModels = new List<ParkingLotViewModel>();
                foreach (var lot in parkingLots)
                {
                    parkingLotViewModels.Add(new ParkingLotViewModel{
                        Id = lot.Id,
                        ShortDesc = lot.ShortDesc,
                        ExclusivePassStart = lot.ExclusivePassStart,
                        ExclusivePassEnd = lot.AnyPassStart,
                        AnyPassStart = lot.AnyPassStart,
                        AllPassStart = lot.NoPassStart,
                        Address = lot.Address,
                        Latitude = lot.Latitude,
                        Longitude = lot.Longitude,
                        WeekendEnforcementFlag = lot.WeekendEnforcementFlag
                    });
                }

                return parkingLotViewModels;
            }
           
            return null;
        }

        public async Task<ParkingLotViewModel> GetLot(int lotID)
        {
            if (db != null)
            {
                var param = new SqlParameter("@id", lotID);
                var lot = await db.ParkingLot.FromSql("exec p_parking_lot_select_raw @id",param).FirstOrDefaultAsync();
                var parkingLotViewModel = new ParkingLotViewModel{
                    Id = lot.Id,
                    ShortDesc = lot.ShortDesc,
                    ExclusivePassStart = lot.ExclusivePassStart,
                    ExclusivePassEnd = lot.AnyPassStart,
                    AnyPassStart = lot.AnyPassStart,
                    AllPassStart = lot.NoPassStart,
                    Address = lot.Address,
                    Latitude = lot.Latitude,
                    Longitude = lot.Longitude,
                    WeekendEnforcementFlag = lot.WeekendEnforcementFlag
                };
                return parkingLotViewModel;
            }
            return null;
        }

        public async Task<List<ParkingLotViewModel>> GetLotsByPass(string pass)
        {
            if (db != null)
            {
                System.Diagnostics.Debug.WriteLine("PASS");
                
                var param = new SqlParameter("@parking_pass_cd", pass);
                var parkingLots = await db.ParkingLot.FromSql("exec p_parking_lot_select_by_parking_pass_cd @parking_pass_cd", param).ToListAsync();
                var parkingLotViewModels = new List<ParkingLotViewModel>();
                foreach (var lot in parkingLots)
                {
                    parkingLotViewModels.Add(new ParkingLotViewModel
                    {
                        Id = lot.Id,
                        ShortDesc = lot.ShortDesc,
                        ExclusivePassStart = lot.ExclusivePassStart,
                        ExclusivePassEnd = lot.AnyPassStart,
                        AnyPassStart = lot.AnyPassStart,
                        AllPassStart = lot.NoPassStart,
                        Address = lot.Address,
                        Latitude = lot.Latitude,
                        Longitude = lot.Longitude,
                        WeekendEnforcementFlag = lot.WeekendEnforcementFlag
                    });
                }

                return parkingLotViewModels;
            }

            return null;
        }

        public async Task UpdateLot(ParkingLot lot)
        {
            if (db != null)
            {
                db.ParkingLot.Update(lot);
                await db.SaveChangesAsync();
            }
        }
    }
}
