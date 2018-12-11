using System.Collections.Generic;
using System.Web.Http;
using ProjectManager.Business;
using ProjectManager.Data.Models.Custom;

namespace ProjectManager.Controllers
{
    public class ProjectController : ApiController
    {
        ProjectBusiness projectBusiness;

        [Route("api/ProjectManager/Project/All")]
        [HttpGet]
        public List<ProjectModel> GetAllProject()
        {
            projectBusiness = new ProjectBusiness();
            var result = projectBusiness.GetAllProject();
            return result;
        }

        [Route("api/ProjectManager/Project/AddUpdate")]
        [HttpPost]
        public string AddOrUpdateProject(object project)
        {
            string result = string.Empty;
            projectBusiness = new ProjectBusiness();
            result = projectBusiness.AddOrUpdateProject(project);
            return result;
        }

        //[Route("Update")]
        //[HttpPost]
        //public bool UpdateProject(object task)
        //{
        //    projectBusiness = new ProjectBusiness();
        //    return projectBusiness.UpdateProject(task);
        //}

        [Route("api/ProjectManager/Project/Suspend")]
        [HttpPost]
        public bool SuspendProject(object task)
        {
            projectBusiness = new ProjectBusiness();
            return projectBusiness.SuspendProject(task);
        }

    }
}
