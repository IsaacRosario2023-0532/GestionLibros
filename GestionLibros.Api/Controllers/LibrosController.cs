using Microsoft.AspNetCore.Mvc;
using GestionLibros.Data.Models;
using GestionLibros.Services.Services;
using GestionLibros.Shared;

namespace GestionLibros.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LibrosController(LibrosService librosService) : ControllerBase
{
    // GET: api/Libros
    [HttpGet]
    public async Task<ActionResult<List<Libros>>> Get()
    {
        var librosResult = await librosService.GetList(l => true);

        if (librosResult.IsFailure)
        {
            return BadRequest(librosResult.Message);
        }

        return Ok(librosResult.Data);
    }

    // GET: api/Libros/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Libros>> Get(int id)
    {
        var libroResult = await librosService.Buscar(id);

        if (libroResult.IsFailure)
        {
            return BadRequest(libroResult.Message);
        }

        if (libroResult.Data == null)
        {
            return NotFound();
        }

        return Ok(libroResult.Data);
    }

    // POST: api/Libros
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Libros libro)
    {
        var resultado = await librosService.Guardar(libro);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Message);
        }

        return Created();
    }

    // PUT: api/Libros/5
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, [FromBody] Libros libro)
    {
        if (id != libro.LibroId)
        {
            return BadRequest();
        }

        var libroActual = await librosService.Buscar(id);

        if (libroActual.IsFailure)
        {
            return BadRequest(libroActual.Message);
        }

        if (libroActual.Data == null)
        {
            return NotFound();
        }

        var resultado = await librosService.Guardar(libro);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Message);
        }

        return NoContent();
    }

    // DELETE: api/Libros/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var libro = await librosService.Buscar(id);

        if (libro.IsFailure)
        {
            return BadRequest(libro.Message);
        }

        if (libro.Data == null)
        {
            return NotFound();
        }

        var resultado = await librosService.Eliminar(id);

        if (resultado.IsFailure)
        {
            return BadRequest(resultado.Message);
        }

        var eliminacion = (Result<bool>)resultado;
        if (!eliminacion.Data)
        {
            return Conflict("No se puede eliminar el libro porque tiene préstamos activos.");
        }

        return NoContent();
    }
}