using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace CreateApi_NancyFk
{
    class DatabasePopulator
    {
        public NancyContext Context;

        public DatabasePopulator()
        {
            Context = new NancyContext();
        }

        public void Populate(int numberOfUsers)
        {
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();

            for (int i=1; i<numberOfUsers; i++)
            {
                User user = new User() { Password = "123", UserName = $"User{i}", UserIdNum = i };
                Context.Add(user);
            }
            Context.SaveChanges();
        }
    }
}
