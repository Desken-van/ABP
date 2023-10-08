using ABP.Application.Models;
using ABP.Infrastructure.Services.EF;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using ABP.AppCore.Enums;
using ABP.Application.Models.Requests;
using ABP.Application.Models.Table;

namespace ABP_Server.Controllers.EF
{
    public class ExperimentEFController : Controller
    {
        private readonly ILogger<ExperimentEFController> _logger;
        private readonly IMapper _mapper;
        private readonly IExperimentEFService _service;

        public ExperimentEFController(IExperimentEFService service, ILogger<ExperimentEFController> logger, IMapper mapper)
        {
            _service = service;

            _logger = logger;

            _mapper = mapper;
        }

        [HttpPost("api/ef/experiment/add")]
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

        [HttpPost("api/ef/experiment/table/{type}")]
        public async Task<ActionResult<TableData>> AddExperiment([FromBody] ExperimentType type)
        {
            try
            {
                var response = await _service.GetTableDataAsync(type);

                return Json(response);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        public IActionResult Index()
        {
            var exp1 = _service.GetTableDataAsync(ExperimentType.ButtonExperiment).Result;

            var exp2 = _service.GetTableDataAsync(ExperimentType.PriceExperiment).Result;

            ViewBag.Experiment1 = exp1;
            ViewBag.Experiment2 = exp2;

            return View();
        }
    }
}
