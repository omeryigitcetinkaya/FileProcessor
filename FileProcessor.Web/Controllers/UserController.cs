using FileProcessor.Web.Models;
using FileProcessor.Web.UserService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileProcessor.Web.Controllers
{
    public class UserController : Controller
    {
        UserServiceClient userServiceClient = new UserServiceClient();

        public ActionResult Index(User model)
        {            
            User[] users = userServiceClient.GetFilteredUsers(model.Name, model.Surname, model.Telephone, model.Address);

            List<UserModel> userModels = new List<UserModel>();

            foreach (User user in users)
            {
                userModels.Add(new UserModel()
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    PhoneNumber = user.Telephone,
                    Address = user.Address
                });
            }

            return View(userModels);
        }
    }
}