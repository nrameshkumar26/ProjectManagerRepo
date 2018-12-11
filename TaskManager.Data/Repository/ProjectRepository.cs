using ProjectManager.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ProjectManager.Data.Models.Custom;

namespace ProjectManager.Data.Repository
{
    public class ProjectRepository
    {
        #region GetAllProject
        /// <summary>
        /// Method to get all project list
        /// </summary>
        /// <returns></returns>
        public List<ProjectModel> GetAllProject()
        {
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                var projectList = (from project in entity.Projects
                                   join task in entity.Tasks on project.Project_ID equals task.Project_ID into allPro
                                   from temp in allPro.DefaultIfEmpty()
                                   where project.Status == true
                                   //orderby project.Project_ID descending
                                   select new ProjectModel()
                                   {
                                       ProjectId = project.Project_ID,
                                       Project = project.Project1,
                                       Priority = project.Priority,
                                       StartDate = project.Start_Date,
                                       EndDate = project.End_Date,
                                       IsActive = project.Status,
                                       NoOfTasks = project.Tasks.Count(),
                                       ManagerId = project.Manager_ID,
                                       CompletedTasks = project.Tasks.Where(x => x.Status == false).Count()
                                   }).Distinct().ToList();

                return projectList.OrderByDescending(x => x.ProjectId).ToList();
            }
        }
        #endregion 

        #region AddOrUpdateProject
        /// <summary>
        /// Method to create new project or update an existing project
        /// </summary>
        /// <param name="projectModel"></param>
        /// <returns></returns>
        public string AddOrUpdateProject(ProjectModel projectModel)
        {
            string result = string.Empty;
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                if (projectModel != null)
                {
                    Project addProject = new Project();
                    addProject.Project_ID = projectModel.ProjectId;
                    addProject.Project1 = projectModel.Project;
                    if (projectModel.StartDateString != null && !string.IsNullOrEmpty(projectModel.StartDateString))
                        addProject.Start_Date = Convert.ToDateTime(projectModel.StartDateString);
                    if (projectModel.EndDateString != null && !string.IsNullOrEmpty(projectModel.EndDateString))
                        addProject.End_Date = Convert.ToDateTime(projectModel.EndDateString);
                    addProject.Priority = projectModel.Priority;
                    addProject.Manager_ID = projectModel.ManagerId;
                    addProject.Status = true;
                    result = addProject.Project_ID == 0 ? "ADD" : "UPDATE";
                    entity.Entry(addProject).State = addProject.Project_ID == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
                }
            }
            return result;
        }
        #endregion

        #region SuspendProject
        /// <summary>
        /// Method to suspend project
        /// </summary>
        /// <param name="projectModel"></param>
        /// <returns></returns>
        public bool SuspendProject(ProjectModel projectModel)
        {
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                if (projectModel != null && projectModel.ProjectId != 0)
                {
                    Project suspendProject = new Project();
                    suspendProject.Project1 = projectModel.Project;
                    suspendProject.Project_ID = projectModel.ProjectId;
                    if (projectModel.StartDateString != null)
                        suspendProject.Start_Date = Convert.ToDateTime(projectModel.StartDateString);
                    if (projectModel.EndDateString != null)
                        suspendProject.End_Date = Convert.ToDateTime(projectModel.EndDateString);
                    suspendProject.Priority = projectModel.Priority;
                    suspendProject.Status = false;
                    entity.Entry(suspendProject).State = System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
                }
                return true;
            }

        }
        #endregion
    }
}
