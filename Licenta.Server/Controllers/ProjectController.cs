using Licenta.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace Licenta.Server.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectService _projectService;
        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }

    }
}
