using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoTasksController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoTasksController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoTask>>> GetTodoTasks()
        {
          if (_context.TodoTasks == null)
          {
              return NotFound();
          }
            return await _context.TodoTasks.ToListAsync();
        }

        // GET: api/TodoTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoTask>> GetTodoTask(int id)
        {
          if (_context.TodoTasks == null)
          {
              return NotFound();
          }
            var todoTask = await _context.TodoTasks.FindAsync(id);

            if (todoTask == null)
            {
                return NotFound();
            }

            return todoTask;
        }

        // PUT: api/TodoTasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoTask(int id, TodoTask todoTask)
        {
            if (id != todoTask.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoTask).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoTaskExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoTasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoTask>> PostTodoTask(TodoTask todoTask)
        {
          if (_context.TodoTasks == null)
          {
              return Problem("Entity set 'TodoContext.TodoTasks'  is null.");
          }
            _context.TodoTasks.Add(todoTask);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoTask), new { id = todoTask.Id }, todoTask);
        }

        // DELETE: api/TodoTasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoTask(int id)
        {
            if (_context.TodoTasks == null)
            {
                return NotFound();
            }
            var todoTask = await _context.TodoTasks.FindAsync(id);
            if (todoTask == null)
            {
                return NotFound();
            }

            _context.TodoTasks.Remove(todoTask);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoTaskExists(int id)
        {
            return (_context.TodoTasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
