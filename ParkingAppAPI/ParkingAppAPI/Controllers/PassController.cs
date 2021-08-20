using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParkingAppAPI.Models;
using ParkingAppAPI.Repository;

namespace ParkingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassController : ControllerBase
    {
        IParkingPassRepository passRepository;
        public PassController(IParkingPassRepository _passRepository)
        {
            passRepository = _passRepository;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var passes = await passRepository.GetAllParkingPasses();
                if (passes == null)
                {
                    return NotFound();
                }

                return Ok(passes);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]ParkingPassCd pass)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await passRepository.AddPass(pass);
                    if (result > 0)
                    {
                        return Ok(result);
                    }

                    return NotFound();
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
        public async Task<IActionResult> DeletePost(string pass)
        {
            int result = 0;

            if (pass == null)
            {
                return BadRequest();
            }

            try
            {
                result = await passRepository.DeletePass(pass);
                if (result == 0)
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
        public async Task<IActionResult> UpdatePost([FromBody]ParkingPassCd pass)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await passRepository.UpdatePass(pass);

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