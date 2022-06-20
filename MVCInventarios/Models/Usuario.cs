using System.ComponentModel.DataAnnotations;

namespace MVCInventarios.Models;

public class Usuario
{
    public int Id { get; set; }
    [Required(ErrorMessage = "El nombre del usuario es obligatorio.")]
    [MinLength(5, ErrorMessage = "El nombre del usuario debe ser mayor o igual a 2 caracteres."),
        MaxLength(50, ErrorMessage = "El nombre del usuario no debe exceder los 25 caracteres.")]
    public string Nombre { get; set; }
    public string Apellidos { get; set; }
    [Required(ErrorMessage = "El nombre de la cuenta del usuario es obligatorio.")]
    [MinLength(5, ErrorMessage = "El nombre de la cuenta del usuario debe ser mayor o igual a 5 caracteres."),
        MaxLength(20, ErrorMessage = "El nombre de la cuenta del usuario no debe exceder los 20 caracteres.")]
    public string Username { get; set; }
    public string Contrasena { get; set; }
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    public string CorreoElectronico { get; set; }
    public string Celular { get; set; }
    [Required(ErrorMessage = "El perfil del usuario es obligatorio.")]
    [Display(Name = "Perfil")]
    public int PerfilId { get; set; }
    public Perfil Perfil { get; set; }
    public byte[] Foto { get; set; }
}