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
    public class AdminRepository:IAdminRepository
    {
        /// <summary>
        /// TBA until the admin procs are done, you do you Jonah
        /// </summary>
        ParkingAppDatabaseContext db;

        public AdminRepository(ParkingAppDatabaseContext _db)
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
        
        public async Task<List<AdminViewModel>> GetAllAdmins()
        {
            if(db != null)
            {
                var admins = await db.Admin.FromSql("p_admin_select_raw").ToListAsync();
                var adminViewModels = new List<AdminViewModel>();
                foreach (var admin in admins)
                {
                    adminViewModels.Add(new AdminViewModel{
                        Id = admin.Id,
                        Username = admin.Username,
                        Password = admin.Password
                    });
                }

                return adminViewModels;
            }

            return null;
        }

        public async Task<int> AddAdmin(Admin admin)
        {
            if (db != null)
            {
                var userParam = new SqlParameter()
                {
                    ParameterName = "@username",
                    Value = admin.Username,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Size = -1
                };
                var passParam = new SqlParameter()
                {
                    ParameterName = "@password",
                    Value = admin.Password,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Size = -1
                };
                await db.Database.ExecuteSqlCommandAsync("exec p_admin_iu @audit_user,@call_stack,@username,@password,@status_cd OUT,@status_desc OUT",auditUser,callStack,userParam,passParam,statusCd,statusDesc);
                return (int)statusCd.Value;
            }

            return 0;
        }

        public async Task<int> DeleteAdmin(string adminUsername)
        {
            int result = 0;

            if (db != null)
            {
                var userParam = new SqlParameter()
                {
                    ParameterName = "@username",
                    Value = adminUsername,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Size = -1
                };
                await db.Database.ExecuteSqlCommandAsync("exec p_admin_d @audit_user,@call_stack,@username,@status_cd OUT,@status_desc OUT", auditUser, callStack, userParam, statusCd, statusDesc);
                return (int)statusCd.Value;
            }

            return result;
        }

        public async Task<int> UpdateAdmin(Admin admin)
        {
            if (db != null)
            {
                var userParam = new SqlParameter()
                {
                    ParameterName = "@username",
                    Value = admin.Username,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Size = -1
                };
                var passParam = new SqlParameter()
                {
                    ParameterName = "@password",
                    Value = admin.Password,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Size = -1
                };
                await db.Database.ExecuteSqlCommandAsync("exec p_admin_iu @audit_user,@call_stack,@username,@password,@status_cd OUT,@status_desc OUT", auditUser, callStack, userParam, passParam, statusCd, statusDesc);
                return (int)statusCd.Value;
            }

            return 0;
        }

        public async Task<int> ValidateLogin(Admin admin)
        {
            if (db != null)
            {
                var lv = new SqlParameter()
                {
                    ParameterName = "@lv",
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Output,
                    Size = -1
                };
                var userParam = new SqlParameter(){
                    ParameterName = "@username",
                    Value = admin.Username,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Size = -1
                };
                var passParam = new SqlParameter()
                {
                    ParameterName = "@password",
                    Value = admin.Password,
                    SqlDbType = SqlDbType.VarChar,
                    Direction = ParameterDirection.Input,
                    Size = -1
                };
                var result = db.Database.ExecuteSqlCommand("exec p_validate_login @username, @password, @lv OUT",userParam,passParam,lv);
                if(lv.Value.ToString() == "VALID")
                {
                    return 1;
                }
                else if(lv.Value.ToString() == "INVALID_PASSWORD")
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}
