using System.ComponentModel.DataAnnotations;

namespace MVCInventarios.ViewModels;

public class CambiarContrasenaViewModel
{
    public int Id { get; set; }
    [Display(Name = "Cuenta del Usuario")]
    public string Username { get; set; }
    [Required(ErrorMessage = "La contraseña es requerida")]
    [StringLength(50, ErrorMessage = "La {0} debe contener un mínimo de {2} " +
        "y un máximo de {1}.", MinimumLength = 5)]
    [DataType(DataType.Password)]
    public string Contrasena { get; set; }
    [Required(ErrorMessage = "La confirmación de la contraseña es requerida")]
    [StringLength(50, ErrorMessage = "La {0} debe contener un mínimo de {2} " +
        "y un máximo de {1}.", MinimumLength = 5)]
    [DataType(DataType.Password)]
    [Display(Name = "Confirmación")]
    [Compare("Contrasena", ErrorMessage = "La contraseña y su confirmación no coinciden.")]
    public string ConfirmarContrasena { get; set; }
}
