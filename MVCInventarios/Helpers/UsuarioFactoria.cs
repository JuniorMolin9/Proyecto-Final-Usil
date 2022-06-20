using Microsoft.AspNetCore.Identity;
using MVCInventarios.Models;
using MVCInventarios.ViewModels;

namespace MVCInventarios.Helpers;
public class UsuarioFactoria
{

    private readonly IPasswordHasher<Usuario> _passwordHasher;

    public UsuarioFactoria(IPasswordHasher<Usuario> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public Usuario CrearUsuario(UsuarioRegistroDto usuarioDto)
    {
        var usuario = new Usuario()
        {
            Id = usuarioDto.Id,
            Apellidos = usuarioDto.Apellidos,
            Celular = usuarioDto.Celular,
            CorreoElectronico = usuarioDto.CorreoElectronico,
            Nombre = usuarioDto.Nombre,
            PerfilId = usuarioDto.PerfilId,
            Username = usuarioDto.Username
        };
        usuario.Contrasena = _passwordHasher.HashPassword(usuario, usuarioDto.Contrasena);
        return usuario;
    }

    public UsuarioRegistroDto CrearUsuarioRegistro(Usuario usuario)
    {
        return new UsuarioRegistroDto()
        {
            Id = usuario.Id,
            Apellidos = usuario.Apellidos,
            Celular = usuario.Celular,
            CorreoElectronico = usuario.CorreoElectronico,
            Nombre = usuario.Nombre,
            PerfilId = usuario.PerfilId,
            Username = usuario.Username,
            Foto = usuario.Foto
        };
    }

    public UsuarioEdicionDto CrearUsuarioEdicion(Usuario usuario)
    {
        return new UsuarioEdicionDto()
        {
            Id = usuario.Id,
            Apellidos = usuario.Apellidos,
            Celular = usuario.Celular,
            CorreoElectronico = usuario.CorreoElectronico,
            Nombre = usuario.Nombre,
            PerfilId = usuario.PerfilId,
            Username = usuario.Username,
            Foto = usuario.Foto
        };
    }

    public void ActualizarDatosUsuario(UsuarioEdicionDto usuario, Usuario usuarioBd)
    {
        usuarioBd.Celular = usuario.Celular;
        usuarioBd.CorreoElectronico = usuario.CorreoElectronico;
        usuarioBd.Nombre = usuario.Nombre;
        usuarioBd.Apellidos = usuario.Apellidos;
        usuarioBd.PerfilId = usuario.PerfilId;
    }

    public CambiarContrasenaViewModel CrearCambiarContrasenaViewModel(Usuario usuario)
    {
        return new CambiarContrasenaViewModel
        {
            Id = usuario.Id,
            Username = usuario.Username
        };
    }

    public void ActualizarContrasenaUsuario(CambiarContrasenaViewModel usuarioVM, Usuario usuarioBd)
    {
        usuarioBd.Contrasena = _passwordHasher.HashPassword(usuarioBd, usuarioVM.Contrasena);
    }

}
