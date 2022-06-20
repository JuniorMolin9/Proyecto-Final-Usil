using X.PagedList;

namespace MVCInventarios.ViewModels;
public class ListadoViewModel<T>
{
    public string TerminoBusqueda { get; set; }
    public int? Pagina { get; set; }
    public IPagedList<T> Registros { get; set; }
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
