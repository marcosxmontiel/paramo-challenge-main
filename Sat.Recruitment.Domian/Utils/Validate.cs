using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sat.Recruitment.Domian.Utils
{
    public static class Validate
    {
        public static void ValidateErrors(User user, ref string errors)
        {
            if (user.Name == null)
                //Validate if Name is null
                errors = "The name is required";
            if (user.Email == null)
                //Validate if Email is null
                errors = errors + " The email is required";
            if (user.Address == null)
                //Validate if Address is null
                errors = errors + " The address is required";
            if (user.Phone == null)
                //Validate if Phone is null
                errors = errors + " The phone is required";
        }

        public static void ValidateUser(ref User userparameter)
        {
            if (userparameter.UserType == "Normal")
            {
                if (userparameter.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.12);
                    //If new user is normal and has more than USD100
                    var gif = userparameter.Money * percentage;
                    userparameter.Money = userparameter.Money + gif;
                }
                if (userparameter.Money < 100)
                {
                    if (userparameter.Money > 10)
                    {
                        var percentage = Convert.ToDecimal(0.8);
                        var gif = userparameter.Money * percentage;
                        userparameter.Money = userparameter.Money + gif;
                    }
                }
            }
            if (userparameter.UserType == "SuperUser")
            {
                if (userparameter.Money > 100)
                {
                    var percentage = Convert.ToDecimal(0.20);
                    var gif = userparameter.Money * percentage;
                    userparameter.Money = userparameter.Money + gif;
                }
            }
            if (userparameter.UserType == "Premium")
            {
                if (userparameter.Money > 100)
                {
                    var gif = userparameter.Money * 2;
                    userparameter.Money = userparameter.Money + gif;
                }
            }

            //Normalize email
            var aux = userparameter.Email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", "") : aux[0].Replace(".", "").Remove(atIndex);

            userparameter.Email = string.Join("@", new string[] { aux[0], aux[1] });
        }

        public static StreamReader ReadUsersFromFile(string filename)
        {
            //var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
            var path = Directory.GetCurrentDirectory() + "/Files/" + filename;

            FileStream fileStream = new FileStream(path, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);
            return reader;
        }

        public static void GetListUser(List<User> userlst)
        {
            var reader = ReadUsersFromFile("Users.txt");

            while (reader.Peek() >= 0)
            {
                var line = reader.ReadLineAsync().Result;
                var user = new User
                {
                    Name = line.Split(',')[0].ToString(),
                    Email = line.Split(',')[1].ToString(),
                    Phone = line.Split(',')[2].ToString(),
                    Address = line.Split(',')[3].ToString(),
                    UserType = line.Split(',')[4].ToString(),
                    Money = decimal.Parse(line.Split(',')[5].ToString()),
                };
                userlst.Add(user);
            }
            reader.Close();
        }
    }
}