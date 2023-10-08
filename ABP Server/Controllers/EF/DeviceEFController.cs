using ABP.Application.Models;
using ABP_Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using ABP.Infrastructure.Services.EF;
using AutoMapper;

namespace ABP_Server.Controllers.EF
{
    public class DeviceEFController : Controller
    {
        private readonly ILogger<DeviceEFController> _logger;
        private readonly IMapper _mapper;
        private readonly IDeviceEFService _service;

        public DeviceEFController(IDeviceEFService service, ILogger<DeviceEFController> logger, IMapper mapper)
        {
            _service = service;

            _logger = logger;

            _mapper = mapper;
        }

        [HttpPost("api/ef/device/add")]
        public async Task<ActionResult> AddDevice([FromBody] DeviceRequest device)
        {
            try
            {
                var result = _mapper.Map<Device>(device);

                var addedDevice = await _service.AddDeviceAsync(result.DeviceName);

                if (addedDevice)
                {
                    return Ok();
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
