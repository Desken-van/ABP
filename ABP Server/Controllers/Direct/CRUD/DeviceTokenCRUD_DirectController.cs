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

        public DeviceTokenCRUD_DirectController(IDeviceTokenDirectRepository repo, IMapper mapper)
        {
            _repo = repo;

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
                    return Ok(list);
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
                    return Ok(token);
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
                    return Ok(list);
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
                var token = await _repo.GetDeviceTokenByRequestAsync(request);

                if ((token != null))
                {
                    return Ok(token);
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

                await _repo.Create(result);

                return Ok();
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
