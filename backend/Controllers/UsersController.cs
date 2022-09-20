using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using prid_tuto.Models;

namespace prid_tuto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MsnContext _context;
        private readonly IMapper _mapper;

        public UsersController(MsnContext context , IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
       [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAll()
        {
            return _mapper.Map<List<UserDTO>>(await _context.Users.ToListAsync()); 
        }

        [HttpGet("{pseudo}")]
        public async Task<ActionResult<User>> GetOne (string pseudo)
        {
            var user = await _context.Users.FindAsync(pseudo);
            if (user == null)
                return NotFound();

            return user;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostMember(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOne), new { pseudo = user.Pseudo }, user);
        }

        [HttpPut]
        public async Task<IActionResult> PutUser(User user)
        {
            var exists = await _context.Members.CountAsync(m => m.Pseudo == user.Pseudo) > 0;
           
            if (!exists)
                return NotFound();

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}