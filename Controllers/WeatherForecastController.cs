using Base64.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Base64.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Base64File file)
        {
            // 1) Get the file extension
            var extension = $".{file.Mime.Substring(file.Mime.IndexOf("/") + 1)}";

            // 2) Generate a unique name base on the date
            var uniqueName = $"{DateTime.Now:ddMMyyyyhhmmssfff}{extension}";

            // 3) Create the full path of the file
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", uniqueName);

            // 4) Convert file to array of bytes
            byte[] fileBytes = Convert.FromBase64String(file.Data);

            // 5) Save file
            await System.IO.File.WriteAllBytesAsync(filePath, fileBytes);

            return Ok();
        }

    }
}
