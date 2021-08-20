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
    public class MapRepository : IMapRepository
    {
        ParkingAppDatabaseContext db;

        public MapRepository(ParkingAppDatabaseContext _db)
        {
            db = _db;
        }

        SqlParameter statusCd = new SqlParameter()
        {
            ParameterName = "@status_cd",
            SqlDbType = SqlDbType.Int,
            Direction = ParameterDirection.Output
        };
        SqlParameter statusDesc = new SqlParameter()
        {
            ParameterName = "@status_desc",
            SqlDbType = SqlDbType.VarChar,
            Direction = ParameterDirection.Output,
            Size = -1
        };

        SqlParameter callStack = new SqlParameter()
        {
            ParameterName = "@call_stack",
            Value = "Parking App",
            SqlDbType = SqlDbType.VarChar,
            Direction = ParameterDirection.Input,
            Size = -1
        };
        SqlParameter auditUser = new SqlParameter()
        {
            ParameterName = "@audit_user",
            Value = "User",
            SqlDbType = SqlDbType.VarChar,
            Direction = ParameterDirection.Input,
            Size = 50
        };

        public async Task<List<MapViewModel>> GetAllMaps()
        {
            if (db != null)
            {
                var maps = await db.ParkingMap.FromSql("p_parking_map_select_raw").ToListAsync();
                var mapViewModels = new List<MapViewModel>();
                foreach (var map in maps)
                {
                    mapViewModels.Add(new MapViewModel
                    {
                        Id = map.Id,
                        ParkingLotId = map.ParkingLotId,
                        ParkingPassCd = map.ParkingPassCd,
                        PrimaryFlag = map.PrimaryFlag
                    });
                }

                return mapViewModels;
            }

            return null;
        }

        public async Task<int> AddMap(ParkingMap map)
        {
            if (db != null)
            {
                await db.ParkingMap.AddAsync(map);
                await db.SaveChangesAsync();
                return 1;
            }

            return 0;
        }

        public async Task<int> DeleteMap(int id)
        {
            int result = 0;

            if (db != null)
            {
                var map = await db.ParkingMap.FirstOrDefaultAsync(x => x.Id == id);

                if (map != null)
                {
                    db.ParkingMap.Remove(map);
                    result = await db.SaveChangesAsync();
                }
                return result;
            }

            return result;
        }

        public async Task<int> DeleteMapByLotId(int lotId)
        {
            int result = 0;

            if (db != null)
            {
                var userParam = new SqlParameter()
                {
                    ParameterName = "@parking_lot_id",
                    Value = lotId,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Size = -1
                };
                await db.Database.ExecuteSqlCommandAsync("exec p_parking_map_d_by_parking_lot_id @audit_user,@call_stack,@parking_lot_id,@status_cd OUT,@status_desc OUT", auditUser, callStack, userParam, statusCd, statusDesc);
                return (int)statusCd.Value;
            }

            return result;
        }

        public async Task<int> DeleteMapByPassColor(string cd)
        {
            int result = 0;

            if (db != null)
            {
                var userParam = new SqlParameter()
                {
                    ParameterName = "@parking_pass_cd",
                    Value = cd,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Size = -1
                };
                await db.Database.ExecuteSqlCommandAsync("exec p_parking_map_d_by_parking_pass_cd @audit_user,@call_stack,@parking_pass_cd,@status_cd OUT,@status_desc OUT", auditUser, callStack, userParam, statusCd, statusDesc);
                return (int)statusCd.Value;
            }

            return result;
        }

        public async Task UpdateMap(ParkingMap map)
        {
            if (db != null)
            {
                db.ParkingMap.Update(map);
                await db.SaveChangesAsync();
            }
        }
    }
}
