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
    [Authorize(Policy = "AdminOnly")] // controller 별 권한 검증 정책 할당, startup에 등록해둔 정책 이름 사용
    public class AdminController : ControllerBase
    {
        public AdminController()
        {

        }

        [HttpGet("admin")]
        public ActionResult getAdmin() => Ok("Yes Admin");
    }
}
