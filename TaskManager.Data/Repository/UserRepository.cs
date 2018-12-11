using ProjectManager.Data.Models;
using ProjectManager.Data.Models.Custom;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManager.Data.Repository
{
    public class UserRepository
    {
        #region GetActiveUserList
        /// <summary>
        /// Method to get active user list
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetActiveUserList()
        {
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                var userList = (from user in entity.Users
                                where user.IsActive == true
                                orderby user.User_ID descending
                                select new UserModel()
                                {
                                    FirstName = user.First_Name,
                                    LastName = user.Last_Name,
                                    EmployeeId = user.Employee_ID,
                                    UserId = user.User_ID
                                }).ToList();

                return userList;
            }
        }
        #endregion 

        #region AddOrUpdateUser
        /// <summary>
        /// Method to create or update a user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public string AddOrUpdateUser(UserModel userModel)
        {
            string result = string.Empty;
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                if (userModel != null)
                {
                    User addUser = new User();
                    addUser.First_Name = userModel.FirstName;
                    addUser.Last_Name = userModel.LastName;
                    addUser.Employee_ID = userModel.EmployeeId;
                    addUser.IsActive = true;
                    addUser.User_ID = userModel.UserId;
                    result = addUser.User_ID == 0 ? "ADD" : "UPDATE";
                    entity.Entry(addUser).State = addUser.User_ID == 0 ? System.Data.Entity.EntityState.Added : System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
                }
            }
            return result;
        }
        #endregion

        #region DeleteUser
        /// <summary>
        /// Method to delete user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public bool DeleteUser(UserModel userModel)
        {
            using (ProjectManagerEntities entity = new ProjectManagerEntities())
            {
                if (userModel != null && userModel.UserId != 0)
                {
                    User deleteUser = new User();
                    deleteUser.User_ID = userModel.UserId;
                    deleteUser.First_Name = userModel.FirstName;
                    deleteUser.Last_Name = userModel.LastName;
                    deleteUser.Employee_ID = userModel.EmployeeId;
                    deleteUser.IsActive = false;
                    entity.Entry(deleteUser).State = System.Data.Entity.EntityState.Modified;
                    entity.SaveChanges();
                }
                return true;
            }

        }
        #endregion
    }
}
