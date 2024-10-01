using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Data;
using TaskManager.Models;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public class TarefasController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TarefasController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Método para obter todas as tarefas com suporte a paginação
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tarefa>>> GetTarefas([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tarefas = await _context.Tarefas
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalTarefas = await _context.Tarefas.CountAsync();

        return Ok(new { tarefas, totalTarefas });
    }

    // Método para obter uma tarefa específica por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Tarefa>> GetTarefa(int id)
    {
        var tarefa = await _context.Tarefas.FindAsync(id);
        if (tarefa == null)
        {
            return NotFound();
        }
        return tarefa;
    }

    // Método para criar uma nova tarefa
    [HttpPost]
    public async Task<ActionResult<Tarefa>> PostTarefa(Tarefa tarefa)
    {
        _context.Tarefas.Add(tarefa);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTarefa), new { id = tarefa.Id }, tarefa);
    }

    // Método para atualizar uma tarefa existente
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTarefa(int id, Tarefa tarefa)
    {
        if (id != tarefa.Id)
        {
            return BadRequest();
        }

        _context.Entry(tarefa).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // Método para excluir uma tarefa (apenas Admin)
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTarefa(int id)
    {
        var tarefa = await _context.Tarefas.FindAsync(id);
        if (tarefa == null)
        {
            return NotFound();
        }

        _context.Tarefas.Remove(tarefa);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
