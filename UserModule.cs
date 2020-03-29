using System;
using System.Collections.Generic;
using System.Text;
using Nancy;
using System.Data.SqlClient;
using System.Linq;
using Newtonsoft.Json;
using System.Linq;

namespace CreateApi_NancyFk
{
    public class UserModule : NancyModule

    {
        public NancyContext Context { get; set; }

        public UserModule()
        {
            Context = new NancyContext();

            // Ci-dessous: exemple du net, https://www.c-sharpcorner.com/article/generating-api-document-in-nancy/ qui ne fonctionne pas.
            // En effet, il semblerait que Nancy ne puisse effectuer toutes les requêtes notamment les requêtes Put et Delete.
            // Les réponses des requêtes Get, Put, Delete et Post on été testées sur le navigateur, Postman et en ligne de commande. Seules les requêtes Get exécutent la commande.
            /*Delete("/{productid}", _ =>
            {
                return Response.AsText("Delete product");
            }, null, "DeleteProductByProductId");*/

            Get("/Users", parameters => GetAllUsers());

            Get("/Users/{UserId}", parameters => GetUser(parameters.UserId));

            //Delete("/Delete/{UserId}", parameters => DeleteUser(parameters.UserId));
            Get("/Delete/{UserId}", parameters => DeleteUser(parameters.UserId));

            //Put("/Users/{UserName}/{Password}", parameters => PutNewUser(parameters.UserName, parameters.Password));
            Get("/Users/{UserName}/{Password}", parameters => PutNewUser(parameters.UserName, parameters.Password));


            //Post("/authentify", parameters => Authentify(parameters.UserName, parameters.Password));  
            Get("/authentify/{UserName}/{Password}", parameters => Authentify(parameters.UserName, parameters.Password));
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

        public string DeleteUser(object userId)
        {
            User user = (from u in Context.Users
                         where u.UserIdNum == Convert.ToInt32(userId)
                         select u).First();
            Context.Users.Remove(user);
            Context.SaveChanges();
            return "Deletion OK";
        }

        public string PutNewUser(object userName, object password)
        {
            int maxId = (from u in Context.Users
                         orderby u.UserIdNum descending
                         select u.UserIdNum).First();
            User user = new User() { UserName = userName.ToString(), Password = password.ToString(), UserIdNum = maxId+1};
            Context.Add(user);
            Context.SaveChanges();
            return "Insertion OK";
        }

        public string Authentify(object userName, object password)
        {
            User user = (from u in Context.Users
                        where u.UserName == userName.ToString()
                        select u).First();
            Authentified ath = new Authentified();
            if (password.ToString() == user.Password)
            {
                ath.Welcome = "Welcome";
                ath.IsAuthentified = true;
            }
            else
            {
                ath.Welcome = "Not welcome";
                ath.IsAuthentified = false;
            }
            string authentification = JsonConvert.SerializeObject(ath);
            return authentification;
        }

    }
}
