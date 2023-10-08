using ABP.Application.Models.Requests;
using ABP.Application.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using ABP.Infrastructure.Services.Direct;
using ABP.AppCore.Enums;

namespace ABP_Server.Controllers.Direct
{
    public class ExperimentDirectController : Controller
    {
        private readonly ILogger<ExperimentDirectController> _logger;
        private readonly IMapper _mapper;
        private readonly IExperimentDirectService _service;

        public ExperimentDirectController(IExperimentDirectService service, ILogger<ExperimentDirectController> logger, IMapper mapper)
        {
            _service = service;

            _logger = logger;

            _mapper = mapper;
        }

        [HttpPost("api/direct/experiment/add")]
        public async Task<ActionResult> AddExperiment([FromBody] DeviceTokenRequest tokenRequest)
        {
            try
            {
                var result = _mapper.Map<DeviceToken>(tokenRequest);

                var response = await _service.MakeExperimentAsync(result);

                if (response)
                {
                    return Ok();
                }

                return NotFound();

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("api/direct/experiment/table")]
        public async Task<ActionResult> AddExperiment([FromBody] ExperimentType type)
        {
            try
            {
                var response = await _service.GetTableDataAsync(type);

                return Ok();

            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
