using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;
using ColegioSanJose.Models;

namespace Desafio_2.Controllers
{
    public class ImportarController : Controller
    {
        private readonly ColegioDbContext _context;

        public ImportarController(ColegioDbContext context)
        {
            _context = context;
        }

        // ==============================
        // IMPORTAR ALUMNOS DESDE EXCEL
        // ==============================

        [HttpGet]
        public IActionResult AlumnosExcel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AlumnosExcel(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                ViewBag.Error = "Por favor seleccione un archivo válido.";
                return View();
            }

            using (var stream = new MemoryStream())
            {
                await archivoExcel.CopyToAsync(stream);

                using (var libro = new XLWorkbook(stream))
                {
                    var hoja = libro.Worksheet(1);

                    var filas = hoja
                        .RangeUsed()
                        .RowsUsed()
                        .Skip(1);

                    var materiasExistentes =
                        await _context.Materia.ToListAsync();

                    foreach (var fila in filas)
                    {
                        DateTime fechaNacimiento;

                        if (fila.Cell(3).DataType == XLDataType.DateTime)
                        {
                            fechaNacimiento =
                                fila.Cell(3).GetDateTime();
                        }
                        else
                        {
                            fechaNacimiento =
                                DateTime.Parse(
                                    fila.Cell(3).GetString());
                        }

                        var nuevoAlumno = new Alumno
                        {
                            Nombre = fila.Cell(1).GetString(),
                            Apellido = fila.Cell(2).GetString(),
                            FechaNacimiento = fechaNacimiento,
                            Grado = fila.Cell(4).GetString()
                        };

                        _context.Alumno.Add(nuevoAlumno);

                        await _context.SaveChangesAsync();

                        // Crear expedientes automáticos
                        foreach (var materia in materiasExistentes)
                        {
                            var expediente = new Expediente
                            {
                                AlumnoId = nuevoAlumno.AlumnoId,
                                MateriaId = materia.MateriaId,
                                NotaFinal = 0
                            };

                            _context.Expediente.Add(expediente);
                        }
                    }

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(
                "Index",
                "Alumnoes");
        }

        // ==============================
        // IMPORTAR MATERIAS DESDE EXCEL
        // ==============================

        [HttpGet]
        public IActionResult MateriasExcel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MateriasExcel(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                ViewBag.Error = "Por favor seleccione un archivo válido.";
                return View();
            }

            using (var stream = new MemoryStream())
            {
                await archivoExcel.CopyToAsync(stream);

                using (var libro = new XLWorkbook(stream))
                {
                    var hoja = libro.Worksheet(1);

                    var filas = hoja
                        .RangeUsed()
                        .RowsUsed()
                        .Skip(1);

                    foreach (var fila in filas)
                    {
                        var nuevaMateria = new Materia
                        {
                            NombreMateria =
                                fila.Cell(1).GetString(),

                            Docente =
                                fila.Cell(2).GetString()
                        };

                        _context.Materia.Add(nuevaMateria);
                    }

                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToAction(
                "Index",
                "Materias");
        }
    }
}