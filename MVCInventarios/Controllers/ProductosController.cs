using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVCInventarios.Data;
using MVCInventarios.Helpers;
using MVCInventarios.Models;
using MVCInventarios.ViewModels;
using X.PagedList;

namespace MVCInventarios.Controllers;

//[Authorize(Roles = "Administrador,Empleado,Servicio Social")]
[Authorize(Policy ="Organizacion")]
public class ProductosController : Controller
{
    private readonly InventariosContext _context;
    private readonly IConfiguration _configuration;
    private readonly INotyfService _servicioNotificacion;
    private readonly ProductoFactoria _productoFactoria;

    public ProductosController(InventariosContext context, IConfiguration configuration,
        INotyfService servicioNotificacion,ProductoFactoria productoFactoria)
    {
        _context = context;
        _configuration = configuration;
        _servicioNotificacion = servicioNotificacion;
        _productoFactoria = productoFactoria;
    }
    // GET: Productos
    public async Task<IActionResult> Index(ListadoViewModel<Producto> viewModel)
    {
        var registrosPorPagina = _configuration.GetValue("RegistrosPorPagina", 5);

        var consulta = _context.Productos
                                .Include(u=>u.Marca)
                                .OrderBy(m => m.Nombre)
                                .AsQueryable()
                                .AsNoTracking();

        if (!String.IsNullOrEmpty(viewModel.TerminoBusqueda))
        {
            consulta = consulta.Where(u => u.Nombre.Contains(viewModel.TerminoBusqueda)
                                      ||u.Marca.Nombre.Contains(viewModel.TerminoBusqueda));
        }

        viewModel.TituloCrear = "Crear Producto";
        viewModel.Total = consulta.Count();
        var numeroPagina = viewModel.Pagina ?? 1;
        viewModel.Registros = await consulta.ToPagedListAsync(numeroPagina, registrosPorPagina);

        return View(viewModel);
    }

    // GET: Productos/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .Include(p => p.Marca)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    // GET: Productos/Create
    public IActionResult Create()
    {            
        AgregarEditarProductoViewModel viewModel = new AgregarEditarProductoViewModel();
        viewModel.ListadoMarcas = new SelectList(_context.Marcas.AsNoTracking(), "Id", "Nombre");
        return View("Producto",viewModel);
    }

    // POST: Productos/Create        
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Descripcion,MarcaId,Costo,Estatus,Imagen")] ProductoCreacionEdicionDto producto)
    {
        AgregarEditarProductoViewModel viewModel = new AgregarEditarProductoViewModel();
        viewModel.ListadoMarcas = new SelectList(_context.Marcas.AsNoTracking(), "Id", "Nombre",producto.MarcaId);
        viewModel.Producto = producto;

        if (ModelState.IsValid)
        {

            var existeElementoBd = _context.Productos
                                .Any(u => u.Nombre.ToLower().Trim() == producto.Nombre.ToLower().Trim());

            if (existeElementoBd)
            {
                ModelState.AddModelError("Producto.Nombre", "Lo sentimos, ya existe un elemento con el nombre indicado.");
                _servicioNotificacion.Warning("Ya existe un elemento con el nombre indicado");                    
                return View("Producto",viewModel);
            }

            try
            {
                var nuevoProducto = _productoFactoria.CrearProducto(producto);

                if (Request.Form.Files.Count > 0)
                {
                    IFormFile archivo = Request.Form.Files.FirstOrDefault();
                    nuevoProducto.Imagen = await Utilerias.LeerImagen(archivo);
                }

                _context.Add(nuevoProducto);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Success($"ÉXITO al crear el producto {producto.Nombre}");                    
            }
            catch (Exception)
            {
                _servicioNotificacion.Warning("Lo sentimos, ha ocurrido un error. Intente nuevamente.");
                return View("Producto",viewModel);
            }
            return RedirectToAction(nameof(Index));
        }
        return View("Producto",viewModel);
    }

    // GET: Productos/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound();
        }
        AgregarEditarProductoViewModel viewModel = new AgregarEditarProductoViewModel();
        viewModel.ListadoMarcas = new SelectList(_context.Marcas.AsNoTracking(), "Id", "Nombre", producto.MarcaId);
        viewModel.Producto = _productoFactoria.CrearProducto(producto);

        if (!String.IsNullOrEmpty(producto.Imagen))
        {
            viewModel.Producto.Imagen = await Utilerias.ConvertirImagenABytes(producto.Imagen);
        }


        return View("Producto",viewModel);
    }

    // POST: Productos/Edit/5        
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,MarcaId,Costo,Estatus,Imagen")] ProductoCreacionEdicionDto producto)
    {
        AgregarEditarProductoViewModel viewModel = new AgregarEditarProductoViewModel();
        viewModel.ListadoMarcas = new SelectList(_context.Marcas.AsNoTracking(), "Id", "Nombre", producto.MarcaId);
        viewModel.Producto = producto;  

        if (id != producto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {

            var existeElementoBd = _context.Productos
                               .Any(u => u.Nombre.ToLower().Trim() == producto.Nombre.ToLower().Trim()
                                       && u.Id != producto.Id);

            if (existeElementoBd)
            {
                ModelState.AddModelError("Producto.Nombre", "Lo sentimos, ya existe un elemento con el nombre indicado.");
                _servicioNotificacion.Warning("Lo sentimos, ya existe un elemento con el nombre indicado.");
                return View("Producto", viewModel);
            }

            try
            {
                var productoBd = await _context.Productos.FindAsync(producto.Id);

                _productoFactoria.ActualizarDatosProducto(producto, productoBd);


                if (Request.Form.Files.Count > 0)
                {
                    IFormFile archivo = Request.Form.Files.FirstOrDefault();
                    productoBd.Imagen = await Utilerias.LeerImagen(archivo);
                }

                _context.Update(productoBd);
                await _context.SaveChangesAsync();
                _servicioNotificacion.Success($"ÉXITO al actualizar el producto {producto.Nombre}");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(producto.Id))
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
        
        return View("Producto",viewModel);
    }

    // GET: Productos/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var producto = await _context.Productos
            .Include(p => p.Marca)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (producto == null)
        {
            return NotFound();
        }

        return View(producto);
    }

    // POST: Productos/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        _servicioNotificacion.Success($"ÉXITO al eliminar el producto {producto.Nombre}");
        return RedirectToAction(nameof(Index));
    }

    private bool ProductoExists(int id)
    {
        return _context.Productos.Any(e => e.Id == id);
    }
}
