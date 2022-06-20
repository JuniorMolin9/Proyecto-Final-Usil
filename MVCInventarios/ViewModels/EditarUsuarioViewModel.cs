using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCInventarios.ViewModels;
public class EditarUsuarioViewModel
{
    public SelectList ListadoPerfiles { get; set; }
    public UsuarioEdicionDto Usuario { get; set; } = new UsuarioEdicionDto();
}
