using Microsoft.AspNetCore.Mvc;
using ColegioSanJose.Models;

namespace ColegioSanJose.Controllers
{
    public class AccountController : Controller
    {
        // Muestra la pantalla de Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Procesa los datos del Login
        [HttpPost]
        public IActionResult Login(string Correo, string Password)
        {
            // Validación fija para tu proyecto
            if (Correo == "admin@sanjose.edu.sv" && Password == "admin123")
            {
                // GUARDAR MARCA DE LOGIN
                HttpContext.Session.SetString("IsLogged", "true");
                return RedirectToAction("Index", "Home");
            }

            // Si es incorrecto, muestra error
            ViewBag.Error = "Correo o contraseña incorrectos";
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }
}
