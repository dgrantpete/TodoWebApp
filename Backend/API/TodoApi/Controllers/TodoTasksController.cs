using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/users/{userId}/tasks")]
    [ApiController]
    public class TodoTasksController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public TodoTasksController(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/users/{userId}/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTodoTasks(int userId)
        {
            bool userExists = await _context.TodoUsers.AnyAsync(user => user.Id == userId);

            if (!userExists)
            {
                return NotFound($"User ID {userId} not found.");
            }

            List<TaskDTO> result = await GetTasksByUserID(userId)
                .Select(task => _mapper.Map<TaskDTO>(task))
                .ToListAsync();

            return Ok(result);
        }

        // GET: api/users/{userId}/tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDTO>> GetTodoTask(int id, int userId)
        {
            TodoTask? todoTask = await GetTasksByUserID(userId)
                .FirstOrDefaultAsync(task => id == task.Id);

            if (todoTask == null)
            {
                return NotFound($"Task ID {id} not found under user ID {userId}.");
            }

            return Ok(todoTask);
        }

        // PUT: api/users/{userId}/tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoTask(int id, int userId, TaskDTO taskDTO)
        {
            if (id != taskDTO.Id)
            {
                return BadRequest($"Task ID in URL ({id}) does not match task ID in body ({taskDTO.Id}).");
            }

            TodoTask? task = await GetTasksByUserID(userId)
                .FirstOrDefaultAsync(task => id == task.Id);
            
            if (task == null)
            {
                return NotFound($"Task ID {id} not found under user ID {userId}.");
            }

            _mapper.Map(taskDTO, task);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/users/{userId}/tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskDTO>> PostTodoTask(int userId, CreateTaskDTO createTaskDTO)
        {
            bool userExists = await _context.TodoUsers.AnyAsync(user => user.Id == userId);

            if (!userExists)
            {
                return NotFound($"User ID {userId} not found.");
            }

            TodoTask task = _mapper.Map<TodoTask>(createTaskDTO);

            task.UserId = userId;

            _context.TodoTasks.Add(task);

            await _context.SaveChangesAsync();

            TaskDTO taskDTO = _mapper.Map<TaskDTO>(task);

            return CreatedAtAction(nameof(GetTodoTask), new { id = taskDTO.Id, userId = task.UserId }, taskDTO);
        }

        // DELETE: api/users/{userId}/tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoTask(int id)
        {
            TodoTask? todoTask = await _context.TodoTasks.FindAsync(id);

            if (todoTask == null)
            {
                return NotFound($"Task ID {id} not found.");
            }

            _context.TodoTasks.Remove(todoTask);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private Task<bool> TodoTaskExists(int id) => _context.TodoTasks.AnyAsync(e => e.Id == id);

        private IQueryable<TodoTask> GetTasksByUserID(int userId) => _context.TodoTasks.Where(e => e.UserId == userId);
    }
}
