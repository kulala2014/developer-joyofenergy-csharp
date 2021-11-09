using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JOIEnergy.Domain;
using JOIEnergy.Services;
using log4net.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace JOIEnergy.Controllers
{
    [Route("readings")]
    public class MeterReadingController : Controller
    {
        private readonly IMeterReadingService _meterReadingService;
        private readonly ILogger<MeterReadingController> _logger;

        public MeterReadingController(IMeterReadingService meterReadingService, ILogger<MeterReadingController> logger)
        {
            _meterReadingService = meterReadingService;
            _logger = logger;
        }
        // POST api/values
        [HttpPost ("store")]
        public ObjectResult Post([FromBody]MeterReadings meterReadings)
        {
            if (!IsMeterReadingsValid(meterReadings)) {
                return new BadRequestObjectResult("Internal Server Error");
            }
            _meterReadingService.StoreReadings(meterReadings.SmartMeterId,meterReadings.ElectricityReadings);
            return new OkObjectResult("{}");
        }

        private bool IsMeterReadingsValid(MeterReadings meterReadings)
        {
            String smartMeterId = meterReadings.SmartMeterId;
            List<ElectricityReading> electricityReadings = meterReadings.ElectricityReadings;
            return smartMeterId != null && smartMeterId.Any()
                    && electricityReadings != null && electricityReadings.Any();
        }

        [HttpGet("read/{smartMeterId}")]
        public ObjectResult GetReading(string smartMeterId) {
            _logger.LogInformation($"Read meter with smartMeterId: {smartMeterId}");
            return new OkObjectResult(_meterReadingService.GetReadings(smartMeterId));
        }
    }
}
