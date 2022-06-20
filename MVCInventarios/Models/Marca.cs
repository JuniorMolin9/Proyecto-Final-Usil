
using System.ComponentModel.DataAnnotations;

namespace MVCInventarios.Models;
public class Marca
{
    public int Id { get; set; }
    [Required(ErrorMessage ="El nombre de la marca es requerido.")]
    [Display(Name ="Marca")]
    [MinLength(4,ErrorMessage ="El nombre de la marca debe ser mayor o igual a 4 caracteres.")]
    [MaxLength(50, ErrorMessage = "El nombre de la marca no debe ser mayor a 50 caracteres.")]
    public string Nombre { get; set; }
    public ICollection<Producto> Productos { get; set; }
}
