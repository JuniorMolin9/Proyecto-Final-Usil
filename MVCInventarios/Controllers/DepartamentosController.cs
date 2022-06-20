using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCInventarios.Data;
using MVCInventarios.Models;
using MVCInventarios.ViewModels;
using X.PagedList;

namespace MVCInventarios.Controllers;

//[Authorize(Roles ="Administrador")]
[Authorize(Policy = "Administradores")]
public class DepartamentosController : Controller
{
    private readonly InventariosContext _context;        
    private readonly IConfiguration _configuration;
    private readonly INotyfService _servicioNotificacion;

    public DepartamentosController(InventariosContext context, IConfiguration configuration,
            INotyfService servicioNotificacion)
    {
        _context = context;
        _configuration = configuration;
        _servicioNotificacion = servicioNotificacion;
    }


    [AllowAnonymous]
    // GET: Departamentos
    public async Task<IActionResult> Index(ListadoViewModel<Departamento> viewModel)
    {
        var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);

        var consulta = _context.Departamentos
                                .OrderBy(m => m.Nombre)
                                .AsQueryable()
                                .AsNoTracking();

        if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
        {
            consulta = consulta.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda));
        }

        viewModel.TituloCrear = "Crear Departamento";
        viewModel.Total = consulta.Count();
        var numeroPagina = viewModel.Pagina ?? 1;
        viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);

        return View(viewModel);
    }

    // GET: Departamentos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamentos
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (departamento == null)
        {
            return NotFound();
        }

        return View(departamento);
    }

    // GET: Departamentos/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Departamentos/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Descripcion,FechaCreacion")] Departamento departamento)
    {
        if (ModelState.IsValid)
        {
            var existeElementoBd = _context.Departamentos
                               .Any(u => u.Nombre.ToLower().Trim() == departamento.Nombre.ToLower().Trim());

            if (existeElementoBd)
            {                    
                _servicioNotificacion.Warning("Ya existe un elemento con el nombre indicado");
                return View(departamento);
            }

            try
            {
                _context.Add(departamento);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Success($"ÉXITO al crear el departamento {departamento.Nombre}");
            }
            catch (DbUpdateException)
            {
                _servicioNotificacion.Warning("Lo sentimos, ha ocurrido un error. Intente nuevamente.");
                return View(departamento);
            }

            return RedirectToAction(nameof(Index));
        }
        return View(departamento);
    }

    // GET: Departamentos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamentos.FindAsync(id);
        if (departamento == null)
        {
            return NotFound();
        }
        return View(departamento);
    }

    // POST: Departamentos/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,FechaCreacion")] Departamento departamento)
    {
        if (id != departamento.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {

            var existeElementoBd = _context.Departamentos
                               .Any(u => u.Nombre.ToLower().Trim() == departamento.Nombre.ToLower().Trim()
                                       && u.Id != departamento.Id);

            if (existeElementoBd)
            {                    
                _servicioNotificacion.Warning("Lo sentimos, ya existe un elemento con el nombre indicado.");
                return View(departamento);
            }


            try
            {
                _context.Update(departamento);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Success($"ÉXITO al actualizar el departamento {departamento.Nombre}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartamentoExists(departamento.Id))
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
        return View(departamento);
    }

    // GET: Departamentos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var departamento = await _context.Departamentos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (departamento == null)
        {
            return NotFound();
        }

        return View(departamento);
    }

    // POST: Departamentos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var departamento = await _context.Departamentos.FindAsync(id);
        _context.Departamentos.Remove(departamento);
        await _context.SaveChangesAsync();
        _servicioNotificacion.Success($"ÉXITO al eliminar el departamento {departamento.Nombre}");
        return RedirectToAction(nameof(Index));
    }

    private bool DepartamentoExists(int id)
    {
        return _context.Departamentos.Any(e => e.Id == id);
    }
}
