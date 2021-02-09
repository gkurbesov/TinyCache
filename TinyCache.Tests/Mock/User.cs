using System;
using System.Collections.Generic;
using System.Text;

namespace TinyCache.Tests.Mock
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }

        public User(int id, string name, string email, int age)
        {
            Id = id;
            Name = name;
            Email = email;
            Age = age;
        }
    }
}
