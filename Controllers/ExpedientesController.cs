using ColegioSanJose.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Desafio_2.Controllers
{
    public class ExpedientesController : Controller
    {
        private readonly ColegioDbContext _context;

        public ExpedientesController(ColegioDbContext context)
        {
            _context = context;
        }

        // GET: Expedientes
        public async Task<IActionResult> Index()
        {
            var lista = await _context.Expediente
                .Include(e => e.Alumno)
                .Include(e => e.Materia)
                .AsNoTracking()
                .ToListAsync();
            return View(lista);
        }

        // GET: Expedientes/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AlumnoId"] = new SelectList(await _context.Alumno.ToListAsync(), "AlumnoId", "Nombre");
            ViewData["MateriaId"] = new SelectList(await _context.Materia.ToListAsync(), "MateriaId", "NombreMateria");
            return View();
        }

        // POST: Expedientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpedienteId,AlumnoId,MateriaId,NotaFinal")] Expediente expediente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expediente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlumnoId"] = new SelectList(await _context.Alumno.ToListAsync(), "AlumnoId", "Nombre", expediente.AlumnoId);
            ViewData["MateriaId"] = new SelectList(await _context.Materia.ToListAsync(), "MateriaId", "NombreMateria", expediente.MateriaId);
            return View(expediente);
        }

        // --- MÉTODOS PARA ELIMINAR (LOS QUE TE FALTABAN) ---

        // GET: Expedientes/Delete/5
        // Muestra la pantalla de confirmación
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var expediente = await _context.Expediente
                .Include(e => e.Alumno)
                .Include(e => e.Materia)
                .FirstOrDefaultAsync(m => m.ExpedienteId == id);

            if (expediente == null) return NotFound();

            return View(expediente);
        }

        // POST: Expedientes/Delete/5
        // Ejecuta la eliminación real en la base de datos
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expediente = await _context.Expediente.FindAsync(id);
            if (expediente != null)
            {
                _context.Expediente.Remove(expediente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Expedientes/Promedios
        public async Task<IActionResult> Promedios()
        {
            ViewBag.TotalAlumnos = await _context.Alumno.CountAsync();
            ViewBag.TotalMaterias = await _context.Materia.CountAsync();
            ViewBag.TotalExpedientes = await _context.Expediente.CountAsync();

            var datosGrafica = await _context.Expediente
                .Include(e => e.Alumno)
                .GroupBy(e => new { e.Alumno.Nombre, e.Alumno.Apellido })
                .Select(g => new PromedioViewModel
                {
                    Alumno = g.Key.Nombre + " " + g.Key.Apellido,
                    Promedio = g.Average(e => (double)(e.NotaFinal ?? 0))
                })
                .ToListAsync();

            return View(datosGrafica);
        }

        private bool ExpedienteExists(int id)
        {
            return _context.Expediente.Any(e => e.ExpedienteId == id);
        }
    }
}