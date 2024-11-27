using backend.Data;
using backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using backend.Auth;

namespace backend.Controllers;


[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  private readonly DataContext _context;
  private readonly Authenticator _passwordHasher;
public UserController(DataContext context, Authenticator passwordHasher)
{
    _context = context;
    _passwordHasher = passwordHasher;
}
  
  [HttpGet("GetAllUsers")]
  public async Task<ActionResult<List<User>>> GetAllUsers()
  {
    var users = await _context.Users.ToListAsync();

    return Ok(users);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<User>> GetUser(int id)
  {
    var user = await _context.Users.FindAsync(id);
    if (user is null)
      return NotFound("User not found.");

    return Ok(user);
  }
  
  [HttpPost]
  public async Task<ActionResult<object>> CreateUser(User user)
  {
    user.Password = _passwordHasher.HashPassword(user.Password);//hashing password before inserting to db
    _context.Users.Add(user);
    await _context.SaveChangesAsync();

    return Ok(new
    {
      Message = "User created successfully.",
      User = user
    });
  }

  [HttpPut]
  public async Task<ActionResult<List<User>>> UpdateUser(User user)
  {
    var dbUser = await _context.Users.FindAsync(user.Id);
    if (dbUser is null)
      return NotFound("User not found.");

    dbUser.FirstName = user.FirstName;
    dbUser.LastName = user.LastName;
    dbUser.Username = user.Username;
    dbUser.Email = user.Email;
    dbUser.Role = user.Role;

    // Re-hash password if user updated it
    if (!string.IsNullOrEmpty(user.Password))
    {
      dbUser.Password = _passwordHasher.HashPassword(user.Password);//rehashing password before reinsterting to db
    }

    await _context.SaveChangesAsync();
    
    return Ok(new
    {
      Message = "User updated successfully.",
      User = user
    });
  }
  
  [HttpDelete]
  public async Task<ActionResult<List<User>>> DeleteUser(int id)
  {
    var dbUser = await _context.Users.FindAsync(id);
    if (dbUser is null)
      return NotFound("User not found.");

    _context.Users.Remove(dbUser);
    await _context.SaveChangesAsync();
    
    return Ok(new
    {
      Message = "User deleted successfully.",
      Id = id
    });
  }

  //made login method
  [HttpPost("Login")]
  public async Task<ActionResult<object>> Login([FromBody] User loginRequest)
  {
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);
      if (user == null)
          return Unauthorized("Invalid email.");

      bool isPasswordValid = _passwordHasher.MatchHashedPassword(user.Password, loginRequest.Password);
      if (!isPasswordValid)
          return Unauthorized("Invalid password.");

      return Ok(new
      {
          Message = "Login successful.",
          User = new
          {
              user.Id,
              user.FirstName,
              user.LastName,
              user.Username,
              user.Email,
              user.Role
          }
      });
  }
}