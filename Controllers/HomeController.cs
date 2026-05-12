using ColegioSanJose.Controllers;
using ColegioSanJose.Models;
using Desafio_2.Data;
using Desafio_2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace Desafio_2.Controllers
{
    public class HomeController : BaseController
    {
        // Usar el DbContext protegido del BaseController (_db)

        public HomeController(ApplicationDbContext context) : base(context)
        {
        }

        public IActionResult Index()
        {
            ViewBag.TotalAlumnos = _db.Alumno.Count();

            ViewBag.TotalMaterias = _db.Materia.Count();

            ViewBag.TotalExpedientes = _db.Expediente.Count();

            var promedios = _db.Expediente
                .GroupBy(e => e.Alumno.Nombre + " " + e.Alumno.Apellido)
                .Select(g => new
                {
                    Alumno = g.Key,
                    Promedio = g.Average(x => x.NotaFinal)
                })
                .ToList();

            ViewBag.Alumnos = promedios.Select(x => x.Alumno).ToList();

            ViewBag.Promedios = promedios.Select(x => x.Promedio).ToList();

            return View();
        }
    }
}
