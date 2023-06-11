using Core.Model;
using Core.Services;
using Data;
using Microsoft.AspNetCore.Mvc;

namespace ManagmentTools.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IService<Aplication> service;
        public WeatherForecastController(IService<Aplication> service)
        {
           

            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> AplicationGetAll()
        {

            var AplicationGetAll = await service.GetAllAsync();


            return Ok(AplicationGetAll);
        }

    }
}