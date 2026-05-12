using System.ComponentModel.DataAnnotations;

namespace ColegioSanJose.Models
{
    public class Usuario
    {
        [Key] 
        public int Id { get; set; }

        [Required(ErrorMessage = "El correo es necesario")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es necesaria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}