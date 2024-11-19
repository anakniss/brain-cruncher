using backend.Entities;
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
        // UserExams = new List<UserExam>();
        Password = "123",
        Role = 0
      }
    };

    return Ok(users);
  }
}