using MVCInventarios.Models;
using MVCInventarios.ViewModels;

namespace MVCInventarios.Helpers;
public class ProductoFactoria
{
    public ProductoFactoria()
    {

    }

    public Producto CrearProducto(ProductoCreacionEdicionDto productoDto)
    {
        return new Producto
        {
            Id = productoDto.Id,
            Costo = productoDto.Costo,
            Descripcion = productoDto.Descripcion,
            Estatus = productoDto.Estatus,
            MarcaId = productoDto.MarcaId,
            Nombre = productoDto.Nombre            
        };
    }

    public ProductoCreacionEdicionDto CrearProducto(Producto producto)
    {
        return new ProductoCreacionEdicionDto
        {
            Id = producto.Id,
            Costo = producto.Costo,
            Descripcion = producto.Descripcion,
            Estatus = producto.Estatus,
            MarcaId = producto.MarcaId,
            Nombre = producto.Nombre
        };
    }

    public void ActualizarDatosProducto(ProductoCreacionEdicionDto productoDto, Producto productoBd)
    {
        productoBd.Id = productoDto.Id;
        productoBd.Costo = productoDto.Costo;
        productoBd.Descripcion = productoDto.Descripcion;
        productoBd.Estatus = productoDto.Estatus;
        productoBd.MarcaId = productoDto.MarcaId;
        productoBd.Nombre = productoDto.Nombre;        
    }

}
