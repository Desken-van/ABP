using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using ABP.Application.Models;
using ABP.Repository.ContractImplementation.EFImplementations;
using ABP_Server.Models;
using AutoMapper;

namespace ABP_Server.Controllers.EF.CRUD
{
    public class DeviceCRUD_EFController : Controller
    {
        private readonly IDeviceEFRepository _repo;
        private readonly IMapper _mapper;

        public DeviceCRUD_EFController(IDeviceEFRepository repo, IMapper mapper)
        {
            _repo = repo;

            _mapper = mapper;
        }

        [HttpGet("api/ef/device/crud/list")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDeviceList()
        {
            try
            {
                var list = await _repo.GetDeviceListAsync();

                if ((list != null) || (list.Count() > 0))
                {
                    return Ok(list);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/ef/device/crud/{id}")]
        public async Task<ActionResult<Device>> GetDeviceById(int id)
        {
            try
            {
                var device = await _repo.GetDeviceByIdAsync(id);

                if ((device != null))
                {
                    return Ok(device);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/ef/device/crud/getbyname/{name}")]
        public async Task<ActionResult<Device>> GetDeviceByName(string name)
        {
            try
            {
                var device = await _repo.GetDeviceByNameAsync(name);

                if ((device != null))
                {
                    return Ok(device);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("api/ef/device/crud/add")]
        public async Task<ActionResult> CreateDevice(DeviceRequest device)
        {
            try
            {
                var result = _mapper.Map<Device>(device);

                await _repo.Create(result);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("api/ef/device/crud/update")]
        public async Task<ActionResult> UpdateDevice(Device device)
        {
            try
            {
                await _repo.UpdateAsync(device);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("api/ef/device/crud/{id}")]
        public async Task<ActionResult> DeleteDeviceByID(int id)
        {
            try
            {
                await _repo.DeleteAsync(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
