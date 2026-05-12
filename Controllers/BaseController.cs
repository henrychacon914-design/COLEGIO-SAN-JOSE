using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ColegioSanJose.Models;

namespace ColegioSanJose.Controllers
{
    public class BaseController : Controller
    {
        protected readonly Desafio_2.Data.ApplicationDbContext _db;

        public BaseController(Desafio_2.Data.ApplicationDbContext db)
        {
            _db = db;
        }

        // Este método se ejecuta ANTES de cualquier acción en los controladores hijos
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Si no existe la marca "IsLogged" en la sesión, mandarlo al Login
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("IsLogged")))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            base.OnActionExecuting(context);
        }
    }
}