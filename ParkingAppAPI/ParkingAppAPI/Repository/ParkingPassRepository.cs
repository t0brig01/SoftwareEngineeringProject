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
    public class ParkingPassRepository:IParkingPassRepository
    {
        ParkingAppDatabaseContext db;
        public ParkingPassRepository(ParkingAppDatabaseContext _db)
        {
            db = _db;
        }

        public async Task<int> AddPass(ParkingPassCd pass)
        {
            if (db != null)
            {
                await db.ParkingPassCd.AddAsync(pass);
                await db.SaveChangesAsync();
                return 1;
            }

            return 0;
        }

        public async Task<int> DeletePass(string parkingPassCd1)
        {
            int result = 0;

            if (db != null)
            {
                var pass = await db.ParkingPassCd.FirstOrDefaultAsync(x => x.ParkingPassCd1 == parkingPassCd1);

                if (pass != null)
                {
                    db.ParkingPassCd.Remove(pass);
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<List<ParkingPassCdViewModel>> GetAllParkingPasses()
        {
            if(db != null)
            {
                var parkingPasses = await db.ParkingPassCd.FromSql("exec p_parking_pass_cd_select").ToListAsync();
                var parkingPassViewModels = new List<ParkingPassCdViewModel>();
                foreach (var parkingPass in parkingPasses)
                {
                    parkingPassViewModels.Add(new ParkingPassCdViewModel{
                        ParkingPassCd1 = parkingPass.ParkingPassCd1,
                        ShortDesc = parkingPass.ShortDesc,
                        DisplaySequence = parkingPass.DisplaySequence,
                        ActiveFlag = parkingPass.ActiveFlag
                    });
                }

                return parkingPassViewModels;
            }

            return null;
        }

        public async Task UpdatePass(ParkingPassCd pass)
        {
            if (db != null)
            {
                db.ParkingPassCd.Update(pass);
                await db.SaveChangesAsync();
            }
        }
    }
}
