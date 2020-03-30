using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using System.Data.SqlClient;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net;

namespace CreateApi_NancyFk
{
    public class UserModule : NancyModule

    {
        public NancyContext Context { get; set; }

        public UserModule()
        {
            Context = new NancyContext();

            Get("/Users", parameters => GetAllUsers());

            Get("/Users/{UserId}", parameters => GetUser(parameters.UserId));

            Delete("/Delete/{UserId}", parameters =>
            {
                int id = parameters.UserId;
                bool isUserRecorded = Context.Users.ToList().Where(x => x.UserIdNum == id).Any();
                
                if (isUserRecorded)
                {
                    User userToRemove = (from u in Context.Users
                                         where u.UserIdNum == id
                                         select u).First();
                    Context.Users.Remove(userToRemove);
                    Context.SaveChanges();
                    var response = Response.AsJson(Nancy.HttpStatusCode.OK);
                    return response;
                }
                else
                {
                    var response = Response.AsJson(Nancy.HttpStatusCode.NotFound);
                    return response;
                }
            });

            Put("/Users/{UserName}/{Password}", parameters =>
            {
                string userName = parameters.UserName;
                string password = parameters.Password;
                var maxContextId = (from i in Context.Users
                                    orderby i.UserIdNum descending
                                    select i.UserIdNum).First();

                User user = new User() { UserName = userName, Password = password, UserIdNum = maxContextId + 1 };
                Context.Add(user);
                Context.SaveChanges();
                var response = Response.AsJson(Nancy.HttpStatusCode.OK);
                return response;
            });
            
            Post("/authentify/{UserName}/{Password}", parameters =>
            {
                string userName = parameters.UserName;
                string password = parameters.Password;
                var user = (from i in Context.Users
                            where i.UserName == userName
                            select i).First();

                if(password == user.Password)
                {
                    var response = Response.AsJson(Nancy.HttpStatusCode.OK);
                    return response;
                }
                else
                {
                    var response = Response.AsJson(Nancy.HttpStatusCode.Forbidden);
                    return response;
                }
            });  
        }

        private dynamic ServeHome(object manyParameters)
        {
            return View["Home.sshtml", GetUser(manyParameters)];
        }

        private string GetAllUsers()
        {
            var users = from u in Context.Users
                        select u;
            var usersNames = JsonConvert.SerializeObject(users);
            return usersNames;
        }

        private string GetUser(object userId)
        {
            User user = (from u in Context.Users
                        where u.UserIdNum == Convert.ToInt32(userId)
                        select u).First();
            string user1 = JsonConvert.SerializeObject(user);
            return user1;
        }
    }
}
