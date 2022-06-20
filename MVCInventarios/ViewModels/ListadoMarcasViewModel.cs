using MVCInventarios.Models;
using X.PagedList;

namespace MVCInventarios.ViewModels;
public class ListadoMarcasViewModel
{
    public string TerminoBusqueda { get; set; }
    public int? Pagina { get; set; }
    public IPagedList<Marca> Marcas { get; set; }
    public int Total { get; set; } = 0;
    public string TituloCrear { get; set; }

    public CrearBusquedaViewModel CrearBusquedaViewModel()
    {
        return new CrearBusquedaViewModel
        {
            Total = Total,
            TituloCrear = TituloCrear,
            TerminoBusqueda = TerminoBusqueda
        };
    }

}
