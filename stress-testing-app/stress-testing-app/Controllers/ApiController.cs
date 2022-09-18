using System;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Operations;
using Raven.Client.Documents.Queries;
using Raven.Client.ServerWide.Operations;
using Raven.Embedded;

namespace stress_testing_app.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<ApiController> _logger;

    public ApiController(ILogger<ApiController> logger)
    {
        _logger = logger;
    }




    [HttpDelete]
    [Route("reset-database")]
    public IActionResult Reset()
    {

        using (var store = EmbeddedServer.Instance.GetDocumentStore("Embedded"))
        {

            var parameters = new DeleteDatabasesOperation.Parameters
            {
                DatabaseNames = new[] { "Embedded" },
                HardDelete = true
            };
            store.Maintenance.Server.Send(new DeleteDatabasesOperation(parameters));
        }

        return Ok();
    }


    [HttpGet]
    [Route("add-weather")]
    public IActionResult AddWeather(int index=1)
    {
        var newWeatherForecast = new WeatherForecast
        {
            ID = Guid.NewGuid(),
            CreateDate = DateTime.Now,
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        };



        using (var store = EmbeddedServer.Instance.GetDocumentStore("Embedded"))
        {
            using (var session = store.OpenSession())
            {
                session.Store(newWeatherForecast, "WeatherForecast/" + newWeatherForecast.ID);
                session.SaveChanges();

            }
        }

        return Ok();
    }

    [HttpGet]
    [Route("add-user")]
    public IActionResult AddUser(int index = 1)
    {
        var newUser = new User
        {
            ID = Guid.NewGuid(),
            CreateDate = DateTime.Now,
            UserName = "UserName-"+Guid.NewGuid()
        };



        using (var store = EmbeddedServer.Instance.GetDocumentStore("Embedded"))
        {
            using (var session = store.OpenSession())
            {
                session.Store(newUser, "WeatherForecast/" + newUser.ID);
                session.SaveChanges();

            }
        }

        return Ok();
    }




}

