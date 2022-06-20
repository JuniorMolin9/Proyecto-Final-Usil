using System.ComponentModel.DataAnnotations;

namespace MVCInventarios.Models;

public class Perfil
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El nombre del perfil obligatorio.")]
    [MinLength(5, ErrorMessage = "El nombre del perfil debe ser mayor o igual a 5 caracteres."),
        MaxLength(50, ErrorMessage = "El nombre del perfil no debe exceder los 50 caracteres.")]
    public string Nombre { get; set; }
    public virtual ICollection<Usuario> Usuarios { get; set; }
}
