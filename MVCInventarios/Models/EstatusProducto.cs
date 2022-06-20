using System.ComponentModel.DataAnnotations;

namespace MVCInventarios.Models;
public enum EstatusProducto
{
    Baja = 0,
    Activo = 1,
    [Display(Name = "En Proceso de Activación")]
    EnProcesoActivacion = 2
}
