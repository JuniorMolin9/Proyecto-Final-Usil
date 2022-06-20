using System.ComponentModel.DataAnnotations;

namespace MVCInventarios.ViewModels;


public class LoginViewModel
{
    [Required(ErrorMessage = "La cuenta del usuario es requerida.")]
    [Display(Name = "Cuenta")]
    public string Username { get; set; }

    [Required(ErrorMessage = "La contraseña del usuario es requerida.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña")]
    public string Password { get; set; }

    [Display(Name = "¿Recordarme?")]
    public bool Recordarme { get; set; }

    public string ReturnUrl { get; set; } 
}
