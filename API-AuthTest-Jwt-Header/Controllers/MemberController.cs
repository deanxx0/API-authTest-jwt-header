using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_AuthTest_Jwt_Header.Controllers
{
    [Route("")]
    [ApiController]
    [Authorize(Policy = "MemberOnly")]
    public class MemberController : ControllerBase
    {
        public MemberController()
        {

        }

        [HttpGet("member")]
        public ActionResult getMember() => Ok("Yes Member");
    }
}
