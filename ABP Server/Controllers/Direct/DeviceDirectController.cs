using ABP.Application.Models;
using ABP_Server.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using ABP.Infrastructure.Services.Direct;

namespace ABP_Server.Controllers.Direct
{
    public class DeviceDirectController : Controller
    {
        private readonly ILogger<DeviceDirectController> _logger;
        private readonly IMapper _mapper;
        private readonly IDeviceDirectService _service;

        public DeviceDirectController(IDeviceDirectService service, ILogger<DeviceDirectController> logger, IMapper mapper)
        {
            _service = service;

            _logger = logger;

            _mapper = mapper;
        }

        [HttpPost("api/direct/device/add")]
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

        //public IActionResult Index()
        //{
        //return View();
        //}

    }
}
