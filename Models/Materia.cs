namespace ColegioSanJose.Models
{
    public class Materia
    {
        public int MateriaId { get; set; }

        public string NombreMateria { get; set; }

        public string Docente { get; set; }

        public ICollection<Expediente>? Expedientes { get; set; }
    }
}