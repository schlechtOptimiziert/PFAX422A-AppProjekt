using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MobileClient.Server.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{ }
