using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Domain.Trolley;

namespace ProductCatalog.Api.Controllers
{
    [Route("trolleyTotal")]
    public class TrolleyController : ControllerBase
    {
        private readonly CalculateTrolleyQueryHandler _calculateTrolleyQueryHandler;

        public TrolleyController(CalculateTrolleyQueryHandler calculateTrolleyQueryHandler)
        {
            _calculateTrolleyQueryHandler = calculateTrolleyQueryHandler;
        }

        [HttpPost]
        public ActionResult<double> TrolleyTotal([FromBody] CalculateTrolleyQuery query)
        {
            return Ok(_calculateTrolleyQueryHandler.Handle(query));
        }
    }
}