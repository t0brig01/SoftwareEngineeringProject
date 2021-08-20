using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkingAppAPI.Models;
using ParkingAppAPI.Repository;

namespace ParkingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        IAdminRepository adminRepository;
        public AdminController(IAdminRepository _adminRepository)
        {
            adminRepository = _adminRepository;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var admins = await adminRepository.GetAllAdmins();
                if (admins == null)
                {
                    return NotFound();
                }

                return Ok(admins);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPost]
        [Route("ValidateLogin")]
        public async Task<IActionResult> ValidateLogin([FromBody]Admin admin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var login = await adminRepository.ValidateLogin(admin);
                    if (login == 1)
                    {
                        return Ok(login);
                    }
                    else if(login == 0)
                    {
                        return BadRequest("Invalid password");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                catch (Exception ex)
                {

                    return BadRequest(ex);
                }

            }

            return BadRequest();
        }
        
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]Admin admin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await adminRepository.AddAdmin(admin);
                    
                    return Ok();
                }
                catch (Exception e)
                {
                    return BadRequest(e);
                }
            }
            return BadRequest();

        }
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string adminUsername)
        {
            int result = 0;

            if (adminUsername == null)
            {
                return BadRequest();
            }

            try
            {
                result = await adminRepository.DeleteAdmin(adminUsername);
                if (result != 0)
                {
                    return NotFound();
                }
                return Ok();
            }
            catch (Exception e)
            {

                return BadRequest(e);
            }
        }
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody]Admin admin)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await adminRepository.UpdateAdmin(admin);

                    return Ok();
                }
                catch (Exception ex)
                {
                    if (ex.GetType().FullName == "Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException")
                    {
                        return NotFound();
                    }

                    return BadRequest(ex);
                }
            }

            return BadRequest();
        }
    }
}