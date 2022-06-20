using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCInventarios.Data;
using MVCInventarios.Models;
using MVCInventarios.ViewModels;
using X.PagedList;

namespace MVCInventarios.Controllers;

//[Authorize(Roles = "Administrador,Empleado")]
[Authorize(Policy = "EmpleadosEmpresa")]
public class MarcasController : Controller
{
    private readonly InventariosContext _context;
    private readonly IConfiguration _configuration;
    private readonly INotyfService _servicioNotificacion;

    public MarcasController(InventariosContext context,IConfiguration configuration,
        INotyfService servicioNotificacion)
    {
        _context = context;
        _configuration = configuration;
        _servicioNotificacion = servicioNotificacion;
    }

    // GET: Marcas
    public async Task<IActionResult> Index(ListadoViewModel<Marca> viewModel)
    {
        var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);

        var consulta = _context.Marcas
                                .OrderBy(m => m.Nombre)
                                .AsQueryable()
                                .AsNoTracking();

        if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
        {
            consulta = consulta.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda));
        }

        viewModel.TituloCrear = "Crear Marca";
        viewModel.Total = consulta.Count();
        var numeroPagina = viewModel.Pagina ?? 1;
        viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);

        return View(viewModel);
    }

    // GET: Marcas/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var marca = await _context.Marcas
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (marca == null)
        {
            return NotFound();
        }

        return View(marca);
    }

    // GET: Marcas/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Marcas/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre")] Marca marca)
    {
        if (ModelState.IsValid)
        {

            var existeElementoBd = _context.Marcas
                                .Any(u => u.Nombre.ToLower().Trim() == marca.Nombre.ToLower().Trim());

            if (existeElementoBd)
            {
                //ModelState.AddModelError("", "Lo sentimos, ya existe un elemento con el nombre indicado.");
                _servicioNotificacion.Warning("Ya existe un elemento con el nombre indicado");
                return View(marca);
            }

            try
            {
                _context.Add(marca);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Success($"ÉXITO al crear la marca {marca.Nombre}");
            }
            catch (DbUpdateException)
            {
                //ModelState.AddModelError("", "Lo sentimos, ha ocurrido un error. Intente nuevamente.");
                _servicioNotificacion.Warning("Lo sentimos, ha ocurrido un error. Intente nuevamente.");
                return View(marca);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(marca);
    }

    // GET: Marcas/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var marca = await _context.Marcas.FindAsync(id);
        if (marca == null)
        {
            return NotFound();
        }
        return View(marca);
    }

    // POST: Marcas/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Marca marca)
    {
        if (id != marca.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {

            var existeElementoBd = _context.Marcas
                                .Any(u => u.Nombre.ToLower().Trim() == marca.Nombre.ToLower().Trim()
                                        && u.Id!=marca.Id );

            if (existeElementoBd)
            {
                //ModelState.AddModelError("", "Lo sentimos, ya existe un elemento con el nombre indicado.");
                _servicioNotificacion.Warning("Lo sentimos, ya existe un elemento con el nombre indicado.");
                return View(marca);
            }



            try
            {
                _context.Update(marca);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Success($"ÉXITO al actualizar la marca {marca.Nombre}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MarcaExists(marca.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(marca);
    }

    // GET: Marcas/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var marca = await _context.Marcas
            .FirstOrDefaultAsync(m => m.Id == id);
        if (marca == null)
        {
            return NotFound();
        }

        return View(marca);
    }

    // POST: Marcas/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var marca = await _context.Marcas.FindAsync(id);
        _context.Marcas.Remove(marca);
        await _context.SaveChangesAsync();
        _servicioNotificacion.Success($"ÉXITO al eliminar la marca {marca.Nombre}");
        return RedirectToAction(nameof(Index));
    }

    private bool MarcaExists(int id)
    {
        return _context.Marcas.Any(e => e.Id == id);
    }
}
