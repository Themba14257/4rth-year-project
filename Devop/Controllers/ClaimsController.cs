using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Devop.Controllers
{
    public class ClaimsController : Controller
    {
        [Authorize]
        public ViewResult Index()
            => View(User?.Claims);
    }
}
