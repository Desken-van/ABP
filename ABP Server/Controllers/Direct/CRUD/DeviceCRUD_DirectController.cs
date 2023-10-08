using ABP.Application.Models;
using ABP_Server.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using ABP.Repository.ContractImplementation.DirectImplementations;

namespace ABP_Server.Controllers.Direct.CRUD
{
    public class DeviceCRUD_DirectController : Controller
    {
        private readonly IDeviceDirectRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<DeviceCRUD_DirectController> _logger;

        public DeviceCRUD_DirectController(IDeviceDirectRepository repo,
            ILogger<DeviceCRUD_DirectController> logger, IMapper mapper)
        {
            _repo = repo;

            _logger = logger;

            _mapper = mapper;
        }

        [HttpGet("api/direct/device/crud/list")]
        public async Task<ActionResult<IEnumerable<Device>>> GetDeviceList()
        {
            try
            {
                var list = await _repo.GetDeviceListAsync();

                if ((list != null) || (list.Count() > 0))
                {
                    return Json(list);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/direct/device/crud/{id}")]
        public async Task<ActionResult<Device>> GetDeviceById(int id)
        {
            try
            {
                var device = await _repo.GetDeviceByIdAsync(id);

                if ((device != null))
                {
                    return Json(device);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/direct/device/crud/getbyname/{name}")]
        public async Task<ActionResult<Device>> GetDeviceByName(string name)
        {
            try
            {
                var device = await _repo.GetDeviceByNameAsync(name);

                if ((device != null))
                {
                    return Json(device);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("api/direct/device/crud/add")]
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

        [HttpPut("api/direct/device/crud/update")]
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

        [HttpDelete("api/direct/device/crud/{id}")]
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
