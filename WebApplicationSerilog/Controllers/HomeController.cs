using Microsoft.AspNetCore.Mvc;
using SerilogTimings;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using WebApplicationSerilog.Models;

namespace WebApplicationSerilog.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
           // throw new AggregateException("something went wrong");
        //    throw new InvalidCastException("something went wrong");

            
            return View();
        }

        [HttpGet("ping/{id}")]
        public string CreatePing(int id)
        {

            _logger.LogInformation("Createing ping {id}", id);

            using (Operation.Time("Do something with DB Query"))
            {

                Thread.Sleep(500);

            }
            
            return "pong";
        }

        [HttpPost("customers")]
        public IActionResult CreateCustomer(CustomerclassDto customer)
        {
            // Note that validation of customer is done by ASP.NET Core automatically.

            // Simulate adding to DB
            _logger.LogInformation("Writing customer {CustomerName} to DB", customer.Name);

            return Ok();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public class CustomerclassDto
        {
            [Required]
            [MaxLength(5)]
            public string Name { get; set; }
            [Range(0, 100)]
            public int Age { get; set; }


        }


    }

}