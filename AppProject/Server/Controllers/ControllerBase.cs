using Microsoft.AspNetCore.Mvc;

namespace AppProject.Server.Controllers;

[ApiController]
[Route("FirstApp/api/[controller]")]
[Route("SecondApp/api/[controller]")]
public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{}
