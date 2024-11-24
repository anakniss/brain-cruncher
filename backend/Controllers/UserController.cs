using backend.Entities;
using backend.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
  [HttpGet]
  public async Task<IActionResult> GetAllUsers()
  {
    var users = new List<User>
    {
      new User()
      {
        Id = 1,
        Name = "Ana",
        Email = "ackniss@gmail.com",
        Password = "123",
        Role = Role.Admin
      }
    };

    return Ok(users);
  }
}