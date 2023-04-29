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
    public class TodoUserAccountsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoUserAccountsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoUserAccounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoUserAccount>>> GetTodoUsers()
        {
          if (_context.TodoUsers == null)
          {
              return NotFound();
          }
            return await _context.TodoUsers.ToListAsync();
        }

        // GET: api/TodoUserAccounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoUserAccount>> GetTodoUserAccount(int id)
        {
          if (_context.TodoUsers == null)
          {
              return NotFound();
          }
            var todoUserAccount = await _context.TodoUsers.FindAsync(id);

            if (todoUserAccount == null)
            {
                return NotFound();
            }

            return todoUserAccount;
        }

        // PUT: api/TodoUserAccounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoUserAccount(int id, TodoUserAccount todoUserAccount)
        {
            if (id != todoUserAccount.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoUserAccount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoUserAccountExists(id))
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

        // POST: api/TodoUserAccounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoUserAccount>> PostTodoUserAccount(TodoUserAccount todoUserAccount)
        {
          if (_context.TodoUsers == null)
          {
              return Problem("Entity set 'TodoContext.TodoUsers'  is null.");
          }
            _context.TodoUsers.Add(todoUserAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoUserAccount), new { id = todoUserAccount.Id }, todoUserAccount);
        }

        // DELETE: api/TodoUserAccounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoUserAccount(int id)
        {
            if (_context.TodoUsers == null)
            {
                return NotFound();
            }
            var todoUserAccount = await _context.TodoUsers.FindAsync(id);
            if (todoUserAccount == null)
            {
                return NotFound();
            }

            _context.TodoUsers.Remove(todoUserAccount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoUserAccountExists(int id)
        {
            return (_context.TodoUsers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
