using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.DTOs;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class TodoUsersController : ControllerBase
    {
        private readonly PasswordHasher<TodoUser> passwordHasher = new();

        private readonly TodoContext _context;
        private readonly IMapper _mapper;

        public TodoUsersController(TodoContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetTodoUsers()
        {
            List<UserDTO> userDTOs = await _context.TodoUsers
                .Select(user => _mapper.Map<UserDTO>(user))
                .ToListAsync();

            return Ok(userDTOs);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetTodoUser(int id)
        {
            TodoUser? todoUser = await _context.TodoUsers.FindAsync(id);

            if (todoUser == null)
            {
                return NotFound($"User ID {id} not found.");
            }

            return _mapper.Map<UserDTO>(todoUser);
        }

        // PUT: api/users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoUser(int id, UserDTO userDTO)
        {
            if (id != userDTO.Id)
            {
                return BadRequest($"ID in URL ({id}) does not match ID in body ({userDTO.Id}).");
            }

            TodoUser? user = await _context.TodoUsers.FindAsync(id);

            if (user == null)
            {
                return NotFound($"User ID {id} not found.");
            }

            if (!await EmailUnique(userDTO.Email, id))
            {
                return BadRequest("Email address already associated with an existing account.");
            }

            _mapper.Map(userDTO, user);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDTO>> CreateAccount(CreateAccountDTO createAccountDTO)
        {
            if (!await EmailUnique(createAccountDTO.Email))
            {
                return BadRequest("Email address already associated with an existing account.");
            }

            TodoUser newUser = _mapper.Map<TodoUser>(createAccountDTO);

            newUser.PasswordHash = passwordHasher.HashPassword(newUser, createAccountDTO.Password);

            _context.TodoUsers.Add(newUser);

            await _context.SaveChangesAsync();

            UserDTO userDTO = _mapper.Map<UserDTO>(newUser);

            return CreatedAtAction(nameof(GetTodoUser), new { id = userDTO.Id }, userDTO);
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoUser(int id)
        {
            TodoUser? todoUser = await _context.TodoUsers.FindAsync(id);

            if (todoUser == null)
            {
                return NotFound($"User ID {id} not found.");
            }

            _context.TodoUsers.Remove(todoUser);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private Task<bool> TodoUserExists(int id) => _context.TodoUsers.AnyAsync(e => e.Id == id);

        private Task<bool> EmailUnique(string email) => _context.TodoUsers.AllAsync(e => e.Email != email);

        private Task<bool> EmailUnique(string email, int id) => _context.TodoUsers.AllAsync(e => e.Id != id || e.Email != email);
    }
}
