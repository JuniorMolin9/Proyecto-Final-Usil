namespace MVCInventarios.Helpers;
public static class Utilerias
{
    public static async Task<string> LeerImagen(IFormFile archivo)
    {
        var rutaDirectorioArchivos = Path.Combine(Directory.GetCurrentDirectory() + "\\archivos\\");
        bool existeRutaDirectorioArchivos = System.IO.Directory.Exists(rutaDirectorioArchivos);
        if (!existeRutaDirectorioArchivos) Directory.CreateDirectory(rutaDirectorioArchivos);

        var extension = Path.GetExtension(archivo.FileName);
        var nombreArchivo = Path.GetFileNameWithoutExtension(archivo.FileName.Trim());
        nombreArchivo = $"{nombreArchivo}_{DateTime.Now:yyyy_MM_dd}_{DateTime.Now:HHmmss}{extension}";
        var rutaArchivo = Path.Combine(rutaDirectorioArchivos, nombreArchivo);
        if (!System.IO.File.Exists(rutaArchivo))
        {
            using (var stream = new FileStream(rutaArchivo, FileMode.Create))
            {
                await archivo.CopyToAsync(stream);
            }
            return nombreArchivo;
        }
        return null;
    }

    public static async Task<byte[]> ConvertirImagenABytes(string imagen)
    {
        var rutaDirectorioArchivos = Path.Combine(Directory.GetCurrentDirectory() + "\\archivos\\");
        bool existeRutaDirectorioArchivos = System.IO.Directory.Exists(rutaDirectorioArchivos);
        if (!existeRutaDirectorioArchivos) Directory.CreateDirectory(rutaDirectorioArchivos);
        var rutaArchivo = Path.Combine(rutaDirectorioArchivos, imagen);
        if (System.IO.File.Exists(rutaArchivo))
        {
            return await System.IO.File.ReadAllBytesAsync(rutaArchivo);
        }
        return null;
    }
}