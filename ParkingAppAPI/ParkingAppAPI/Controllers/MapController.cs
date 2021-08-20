using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingAppAPI.Models;
using ParkingAppAPI.Repository;

namespace ParkingAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        IMapRepository mapRepository;
        public MapController(IMapRepository _mapRepository)
        {
            mapRepository = _mapRepository;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var maps = await mapRepository.GetAllMaps();
                if (maps == null)
                {
                    return NotFound();
                }

                return Ok(maps);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }

        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]ParkingMap map)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = await mapRepository.AddMap(map);
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
        public async Task<IActionResult> Delete(int id)
        {
            int result = 0;

            try
            {
                result = await mapRepository.DeleteMap(id);
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
        [Route("DeleteByLotId")]
        public async Task<IActionResult> DeleteByLotId(int id)
        {
            int result = 0;

            try
            {
                result = await mapRepository.DeleteMapByLotId(id);
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
        [Route("DeleteByPassColor")]
        public async Task<IActionResult> DeleteByPassColor(string cd)
        {
            int result = 0;

            try
            {
                result = await mapRepository.DeleteMapByPassColor(cd);
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
        public async Task<IActionResult> Update([FromBody]ParkingMap map)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await mapRepository.UpdateMap(map);

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