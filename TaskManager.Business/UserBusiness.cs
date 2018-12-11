using ProjectManager.Data.Models.Custom;
using ProjectManager.Data.Repository;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace ProjectManager.Business
{
    public class UserBusiness
    {
        UserRepository userRepository;

        #region GetActiveUserList
        /// <summary>
        /// Method to fetch the user details
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetActiveUserList()
        {
            userRepository = new UserRepository();
            var result = userRepository.GetActiveUserList();
            return result;
        }
        #endregion

        #region AddorUpdate
        /// <summary>
        /// Method to create or update a user
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public string AddOrUpdateUser(object userModel)
        {
            string result = string.Empty;
            userRepository = new UserRepository();
            result = userRepository.AddOrUpdateUser(UserConverter(userModel));
            return result;
        }
        #endregion

        #region DeleteUser
        /// <summary>
        /// Method to end Task 
        /// </summary>
        /// <param name="userModel"></param>
        /// <returns></returns>
        public bool DeleteUser(object userModel)
        {
            userRepository = new UserRepository();
            return userRepository.DeleteUser(UserConverter(userModel));
        }
        #endregion

        #region UserConverter
        /// <summary>
        /// Method to convert the incoming objects to models
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserModel UserConverter(object user)
        {
            UserModel userModel = new UserModel();
            string details = user.ToString();
            JavaScriptSerializer objJavascript = new JavaScriptSerializer();
            var inputModel = objJavascript.DeserializeObject(details);

            if (inputModel != null)
            {
                Dictionary<string, object> dic1 = (Dictionary<string, object>)inputModel;
                object value;

                if (dic1.TryGetValue("FirstName", out value))
                    userModel.FirstName = value != null ? value.ToString() : null;
                if (dic1.TryGetValue("LastName", out value))
                    userModel.LastName = value != null ? value.ToString() : null;
                if (dic1.TryGetValue("EmployeeId", out value))
                    userModel.EmployeeId = value != null ? value.ToString() : null;
                if (dic1.TryGetValue("UserId", out value))
                    userModel.UserId = string.IsNullOrWhiteSpace(value.ToString()) ? 0 : Convert.ToInt16(value);
                if (dic1.TryGetValue("ProejctId", out value))
                    userModel.ProjectId = string.IsNullOrWhiteSpace(value.ToString()) ? 0 : Convert.ToInt16(value);
                if (dic1.TryGetValue("TaskId", out value))
                    userModel.TaskId = string.IsNullOrWhiteSpace(value.ToString()) ? 0 : Convert.ToInt16(value);
                return userModel;
            }

            return userModel;
        }
        #endregion
    }
}
