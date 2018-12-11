using ProjectManager.Data.Models;
using ProjectManager.Data.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManager.Data.Repository
{
    public class TaskRepository
    {
        #region GetParentTask
        /// <summary>
        /// Method to get parent tasks
        /// </summary>
        /// <returns></returns>
        public List<TaskModel> GetParentTask()
        {
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                var parentTasks = (from task in entity.ParentTasks
                                   select new TaskModel()
                                   {
                                       ParentId = task.Parent_ID,
                                       ParentTask = task.Parent_Task
                                   }).ToList();

                return parentTasks;
            }
        }
        #endregion 

        #region GetAllTask
        /// <summary>
        /// Method to get all task
        /// </summary>
        /// <returns></returns>
        public List<TaskModel> GetAllTask()
        {
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                var taskList = (from task in entity.Tasks.Include("ParentTask")
                                join proj in entity.Projects on task.Project_ID equals proj.Project_ID
                                join usr in entity.Users on proj.Project_ID equals usr.Project_ID into allTask
                                from temp in allTask.DefaultIfEmpty()
                                orderby task.Task_ID descending
                                select new TaskModel()
                                {
                                    TaskId = task.Task_ID,
                                    Task = task.Task1,
                                    ParentTask = task.ParentTask.Parent_Task,
                                    Priority = task.Priority,
                                    StartDate = task.Start_Date,
                                    EndDate = task.End_Date,
                                    ParentId = task.ParentTask.Parent_ID,
                                    IsActive = task.Status,
                                    Project = task.Project.Project1,
                                    ProjectId = task.Project.Project_ID,
                                    ManagerId = task.Project.Manager_ID,
                                    UserId = task.Users.Where(x => x.Project_ID == proj.Project_ID).Select(x=>x.User_ID).FirstOrDefault()
                                }).ToList();

                if (taskList != null)
                {
                    foreach (var item in taskList)
                    {
                        if (item.StartDate != null)
                            item.StartDateString = item.StartDate.ToString();
                        if (item.EndDate != null)
                            item.EndDateString = item.EndDate.ToString();
                    }
                }
                return taskList;
            }
        }
        #endregion

        #region AddParent
        /// <summary>
        /// Method to create a new task or update an existing task
        /// </summary>
        /// <param name="taskModel"></param>
        /// <returns></returns>
        public string AddParent(TaskModel taskModel)
        {
            string result = string.Empty;
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                if (taskModel != null)
                {
                    ParentTask parentTask = new ParentTask();
                    parentTask.Parent_Task = taskModel.Task;
                    entity.Entry(parentTask).State = System.Data.Entity.EntityState.Added;
                    entity.SaveChanges();
                    result = "ADD";
                }
            }
            return result;
        }
        #endregion

        #region AddorUpdateTask
        /// <summary>
        /// Method to create a new task or update an existing task
        /// </summary>
        /// <param name="taskModel"></param>
        /// <returns></returns>
        public string AddorUpdateTask(TaskModel taskModel)
        {
            string result = string.Empty;
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                if (taskModel != null)
                {
                    Task addTask = new Task();
                    addTask.Task1 = taskModel.Task;
                    if (taskModel.StartDateString != null && !string.IsNullOrEmpty(taskModel.StartDateString))
                        addTask.Start_Date = Convert.ToDateTime(taskModel.StartDateString);
                    if (taskModel.EndDateString != null && !string.IsNullOrEmpty(taskModel.EndDateString))
                        addTask.End_Date = Convert.ToDateTime(taskModel.EndDateString);
                    addTask.Priority = taskModel.Priority;
                    addTask.Parent_ID = taskModel.ParentId;
                    addTask.Task_ID = taskModel.TaskId;
                    addTask.Project_ID = taskModel.ProjectId;
                    addTask.Status = true;
                    result = addTask.Task_ID == 0 ? "ADD" : "UPDATE";
                    entity.Entry(addTask).State = addTask.Task_ID == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();

                    if (result == "ADD")
                    {
                        int taskId = GetTaskId();
                        if (taskModel.UserId != 0)
                        {
                            User userTask = GetUserById(taskModel.UserId);
                            userTask.Project_ID = taskModel.ProjectId;
                            userTask.Task_ID = taskId;
                            userTask.User_ID = taskModel.UserId;
                            entity.Entry(userTask).State = System.Data.Entity.EntityState.Modified;
                            entity.SaveChanges();
                        }
                    }
                }
            }
            return result;
        }
        #endregion

        #region EndTask
        /// <summary>
        /// Method to end task
        /// </summary>
        /// <param name="taskModel"></param>
        /// <returns></returns>
        public bool EndTask(TaskModel taskModel)
        {
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                if (taskModel != null && taskModel.TaskId != 0)
                {
                    Task endTask = new Task();
                    endTask.Task_ID = taskModel.TaskId;
                    endTask.Task1 = taskModel.Task;
                    if (taskModel.StartDateString != null)
                        endTask.Start_Date = Convert.ToDateTime(taskModel.StartDateString);
                    if (taskModel.EndDateString != null)
                        endTask.End_Date = Convert.ToDateTime(taskModel.EndDateString);
                    endTask.Priority = taskModel.Priority;
                    endTask.Parent_ID = taskModel.ParentId;
                    endTask.Project_ID = taskModel.ProjectId;
                    endTask.Status = false;
                    entity.Entry(endTask).State = System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
                }
                return true;
            }

        }
        #endregion

        #region GetTaskId
        /// <summary>
        /// Method to get latest task id
        /// </summary>
        /// <returns></returns>
        private int GetTaskId()
        {
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                var taskDetails = (from task in entity.Tasks
                                   select task).Max(t => t.Task_ID);

                return taskDetails;
            }
        }
        #endregion

        private User GetUserById(int userId)
        {
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                var userDetails = (from user in entity.Users
                                   where user.User_ID.Equals(userId)
                                   select user).FirstOrDefault();
                return userDetails;
            }
        }
    }
}
