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
    public class LotsController : ControllerBase
    {
        ILotsRepository lotRepository;
        public LotsController(ILotsRepository _lotRepository)
        {
            lotRepository = _lotRepository;
        }
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var passes = await lotRepository.GetAllLots();
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
        [HttpGet]
        [Route("GetByPass")]
        public async Task<IActionResult> GetByPass(string pass)
        {
            try
            {
                var passes = await lotRepository.GetLotsByPass(pass);
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

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var passes = await lotRepository.GetLot(id);
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
        public async Task<IActionResult> Add([FromBody]ParkingLot lot)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await lotRepository.AddLot(lot);
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
        public async Task<IActionResult> Delete(int lotID)
        {
            int result = 0;

            try
            {
                result = await lotRepository.DeleteLot(lotID);
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
        public async Task<IActionResult> Update([FromBody]ParkingLot lot)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await lotRepository.UpdateLot(lot);

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