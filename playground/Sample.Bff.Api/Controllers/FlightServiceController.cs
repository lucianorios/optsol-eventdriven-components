﻿using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Optsol.EventDriven.Components.MassTransit;
using Sample.Flight.Contracts;

namespace Sample.Bff.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FlightServiceController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ISendEndpointProvider sendEndpointProvider;

        public FlightServiceController(ILogger<BookingServiceController> logger, ISendEndpointProvider sendEndpointProvider)
        {
            this.logger = logger;
            this.sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> BookFlightAsync(BookFlight request)
        {
            logger.LogInformation("Book Flight outside saga - Async");

            //var endpoint = await sendEndpointProvider.GetSendEndpoint(new Uri("queue:book-flight"));
            
            // nome da fila ou exchange baseada no nome da classe do request
            var endpoint = await sendEndpointProvider.GetSendEndpoint(request);

            await endpoint.Send(request);

            return Accepted();
        }
    }
}
