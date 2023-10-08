using ABP.Application.Models.Requests;
using ABP.Application.Models;
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
    public class DeviceTokenCRUD_DirectController : Controller
    {
        private readonly IDeviceTokenDirectRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<DeviceTokenCRUD_DirectController> _logger;

        public DeviceTokenCRUD_DirectController(IDeviceTokenDirectRepository repo,
            ILogger<DeviceTokenCRUD_DirectController> logger, IMapper mapper)
        {
            _repo = repo;

            _logger = logger;

            _mapper = mapper;
        }

        [HttpGet("api/direct/device/token/crud/list")]
        public async Task<ActionResult<IEnumerable<DeviceToken>>> GetDeviceTokenList()
        {
            try
            {
                var list = await _repo.GetDeviceTokenListAsync();

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

        [HttpGet("api/direct/device/token/crud/{id}")]
        public async Task<ActionResult<DeviceToken>> GetDeviceTokenById(int id)
        {
            try
            {
                var token = await _repo.GetDeviceTokenByIdAsync(id);

                if ((token != null))
                {
                    return Json(token);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("api/direct/device/token/crud/list/{deviceId}")]
        public async Task<ActionResult<List<DeviceToken>>> GetDeviceTokensByDeviceId(int deviceId)
        {
            try
            {
                var list = await _repo.GetDeviceTokenByDeviceIdAsync(deviceId);

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

        [HttpGet("api/direct/device/token/crud/model/{request}")]
        public async Task<ActionResult<Device>> GetDeviceTokenByRequest(DeviceTokenRequest request)
        {
            try
            {
                var experiment = await _repo.GetDeviceTokenByRequestAsync(request);

                if ((experiment != null))
                {
                    return Json(experiment);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("api/direct/device/token/crud/add")]
        public async Task<ActionResult> CreateDeviceToken(DeviceTokenRequest tokenRequest)
        {
            try
            {
                var result = _mapper.Map<DeviceToken>(tokenRequest);

                var response = await _repo.Create(result);

                if (response)
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

        [HttpPut("api/direct/device/token/crud/update")]
        public async Task<ActionResult> UpdateDeviceToken(DeviceToken token)
        {
            try
            {
                await _repo.UpdateAsync(token);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("api/direct/device/token/crud/{id}")]
        public async Task<ActionResult> DeleteDeviceTokenByID(int id)
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
