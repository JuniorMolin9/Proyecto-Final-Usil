using System.ComponentModel.DataAnnotations;

namespace MVCInventarios.Models;
public class Departamento
{
    public int Id { get; set; }
    [Required(ErrorMessage="El nombre del departamento es requerido.")]
    [MinLength(4,ErrorMessage ="El nombre del departamento debe ser mayor o igual a 4 caracteres")]
    [MaxLength(100,ErrorMessage ="El nombre del departamento no debe exceder los 100 caracteres")]
    public string Nombre { get; set; }
    [Display(Name="Descripción")]
    [StringLength(200,MinimumLength =4,
        ErrorMessage ="La descripción del departamento debe contener entre 4 y 200 caracteres")]
    public string Descripcion { get; set; }
    [Display(Name = "Fecha de Creación")]
    [Required(ErrorMessage="La fecha de creación del departamento es requerida.")]
    [DataType(DataType.Date)]
    public DateTime FechaCreacion { get; set; }


}
