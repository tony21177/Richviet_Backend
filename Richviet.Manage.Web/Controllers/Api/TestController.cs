using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Richviet.Services.Contracts;
using Richviet.Services.Models;

namespace Richviet.Manage.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IBankService bankService;

        public TestController(IBankService bankService)
        {
            this.bankService = bankService;
        }

        [HttpPost("Test")]
        public IActionResult Test()
        {
            List<ReceiveBank> receiveBanks = bankService.GetReceiveBanks();
            return Ok(new { Value = true, Msg = receiveBanks.Count() });
        }
    }
}
