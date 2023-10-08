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
    public class ExperimentCRUD_DirectController : Controller
    {
        private readonly IExperimentDirectRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<ExperimentCRUD_DirectController> _logger;

        public ExperimentCRUD_DirectController(IExperimentDirectRepository repo,
            ILogger<ExperimentCRUD_DirectController> logger, IMapper mapper)
        {
            _repo = repo;

            _logger = logger;

            _mapper = mapper;
        }

        [HttpGet("api/direct/experiment/crud/list")]
        public async Task<ActionResult<IEnumerable<Experiment>>> GetExperimentList()
        {
            try
            {
                var list = await _repo.GetExperimentListAsync();

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

        [HttpGet("api/direct/experiment/crud/{id}")]
        public async Task<ActionResult<Experiment>> GetExperimentById(int id)
        {
            try
            {
                var experiment = await _repo.GetExperimentByIdAsync(id);

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

        [HttpGet("api/direct/experiment/crud/device/{deviceId}")]
        public async Task<ActionResult<Experiment>> GetExperimentByDeviceId(int deviceId)
        {
            try
            {
                var experiment = await _repo.GetExperimentsByDeviceIdAsync(deviceId);

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

        [HttpGet("api/direct/experiment/crud/model/{request}")]
        public async Task<ActionResult<Experiment>> GetExperimentByRequest(ExperimentRequest request)
        {
            try
            {
                var experiment = await _repo.GetExperimentByRequest(request);

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

        [HttpPost("api/direct/experiment/crud/add")]
        public async Task<ActionResult> CreateExperiment(ExperimentRequest experiment)
        {
            try
            {
                var result = _mapper.Map<Experiment>(experiment);

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

        [HttpPut("api/direct/experiment/crud/update")]
        public async Task<ActionResult> UpdateExperiment(Experiment experiment)
        {
            try
            {
                await _repo.UpdateAsync(experiment);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("api/direct/experiment/crud/{id}")]
        public async Task<ActionResult> DeleteExperimentByID(int id)
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
