using ABP.Application.Models;
using ABP.Repository.ContractImplementation.EFImplementations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using ABP.Application.Models.Requests;

namespace ABP_Server.Controllers.EF.CRUD
{
    public class ExperimentCRUD_EFController : Controller
    {
        private readonly IExperimentEFRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<ExperimentCRUD_EFController> _logger;

        public ExperimentCRUD_EFController(IExperimentEFRepository repo,
            ILogger<ExperimentCRUD_EFController> logger, IMapper mapper)
        {
            _repo = repo;

            _logger = logger;

            _mapper = mapper;
        }

        [HttpGet("api/ef/experiment/crud/list")]
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

        [HttpGet("api/ef/experiment/crud/{id}")]
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

        [HttpGet("api/ef/experiment/crud/device/{deviceId}")]
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

        [HttpGet("api/ef/experiment/crud/model/{request}")]
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

        [HttpPost("api/ef/experiment/crud/add")]
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

        [HttpPut("api/ef/experiment/crud/update")]
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

        [HttpDelete("api/ef/experiment/crud/{id}")]
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
