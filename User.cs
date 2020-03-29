using System;
using System.Collections.Generic;
using System.Text;

namespace CreateApi_NancyFk
{
    public class User
    {
        public Guid UserId { get; set; }
        public int UserIdNum { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }

        public override string ToString()
        {
            return $"UserId = {UserIdNum}, UserName = {UserName}";
        }
    }
}
