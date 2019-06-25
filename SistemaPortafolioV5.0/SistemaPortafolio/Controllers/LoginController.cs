using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SistemaPortafolio.Models;
using SistemaPortafolio.Filters;

namespace SistemaPortafolio.Controllers
{

    public class LoginController : Controller
    {
        // GET: Admin/Login
        private Usuario usuario = new Usuario();

        [NoLogin]

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Validar(string Usuario, string Password)
        {
            var rm = usuario.ValidarLogin(Usuario, Password);

            if (rm.response)
            {
                if(rm.message.Equals("admin"))
                {
                    rm.href = Url.Content("~/Admin/Home/Index");
                }
                else if(rm.message.Equals("user"))
                {
                    rm.href = Url.Content("~/User/Home/Index");
                }
            }

            return Json(rm);
        }

        public ActionResult Logout()
        {
            SessionHelper.DestroyUserSession();

            return Redirect("~/Login");
        }
    }
}