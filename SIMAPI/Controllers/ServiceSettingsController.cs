using Microsoft.AspNetCore.Mvc;
using System.ServiceProcess;

namespace MyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceSettingsController : ControllerBase
    {
        private readonly string serviceName = "MyWindowsServiceName"; // change this

        [HttpPost("start")]
        public IActionResult StartService()
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    if (sc.Status != ServiceControllerStatus.Running)
                    {
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(30));
                    }
                }
                return Ok("Service started successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error starting service: {ex.Message}");
            }
        }

        [HttpPost("stop")]
        public IActionResult StopService()
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    if (sc.Status != ServiceControllerStatus.Stopped)
                    {
                        sc.Stop();
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(30));
                    }
                }
                return Ok("Service stopped successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error stopping service: {ex.Message}");
            }
        }

        [HttpGet("status")]
        public IActionResult GetServiceStatus()
        {
            try
            {
                using (ServiceController sc = new ServiceController(serviceName))
                {
                    return Ok(sc.Status.ToString());
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting service status: {ex.Message}");
            }
        }
    }
}
