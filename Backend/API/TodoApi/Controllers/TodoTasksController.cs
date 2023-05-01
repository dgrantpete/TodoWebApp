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

        // GET: api/tasks
        [HttpGet]
        public ActionResult<IEnumerable<TaskDTO>> GetTodoTasks(int userId)
        {
            if (_context.TodoTasks == null)
            {
                return NotFound();
            }

            IEnumerable<TaskDTO> todoTasks = GetTasksByUserID(userId)
                .Select(taskModel => _mapper.Map<TaskDTO>(taskModel));

            return Ok(todoTasks);
        }

        // GET: api/tasks/5
        [HttpGet("{id}")]
        public ActionResult<TaskDTO> GetTodoTask(int id, int userId)
        {
            if (_context.TodoTasks == null)
            {
                return NotFound();
            }

            TodoTask? todoTask = GetTasksByUserID(userId)
                .FirstOrDefault(task => task.Id == id);

            if (todoTask == null)
            {
                return NotFound($"Task ID {id} not found under user ID {userId}.");
            }

            return Ok(todoTask);
        }

        // PUT: api/tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutTodoTask(int id, int userId, TaskDTO taskDTO)
        {
            if (id != taskDTO.Id)
            {
                return BadRequest($"ID in URL ({id}) does not match ID in body ({taskDTO.Id}).");
            }

            TodoTask? task = GetTasksByUserID(userId)
                .FirstOrDefault(task => id == task.Id);

            if (task == null)
            {
                return NotFound($"Task ID {id} not found under user ID {userId}.");
            }

            _mapper.Map(taskDTO, task);

            return NoContent();
        }

        // POST: api/tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TaskDTO>> PostTodoTask(int userId, CreateTaskDTO createTaskDTO)
        {
            if (_context.TodoTasks == null)
            {
                return Problem("Entity set 'TodoContext.TodoTasks'  is null.");
            }

            TodoTask task = _mapper.Map<TodoTask>(createTaskDTO);

            task.UserId = userId;

            _context.TodoTasks.Add(task);

            await _context.SaveChangesAsync();

            TaskDTO taskDTO = _mapper.Map<TaskDTO>(task);

            return CreatedAtAction(nameof(GetTodoTask), new { id = taskDTO.Id, userId = task.UserId }, taskDTO);
        }

        // DELETE: api/tasks/5
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

        private bool TodoTaskExists(int id) => (_context.TodoTasks?.Any(e => e.Id == id)).GetValueOrDefault();

        private IQueryable<TodoTask> GetTasksByUserID(int userId) => _context.TodoTasks.Where(e => e.UserId == userId);
    }
}
