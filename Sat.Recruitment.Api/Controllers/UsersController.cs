using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Domian;
using Sat.Recruitment.Domian.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public partial class UsersController : ControllerBase
    {
        private readonly List<User> _users = new List<User>();
        public UsersController() { }

        [HttpPost]
        [Route("/create-user")]
        public async Task<Result> CreateUser([FromBody] User userparameter)
        {
            var errors = "";

            Validate.ValidateErrors(userparameter, ref errors);

            if (errors != null && errors != "")
                return new Result()
                {
                    IsSuccess = false,
                    Errors = errors
                };

            Validate.ValidateUser(ref userparameter);

            Validate.GetListUser(_users);

            try
            {
                var isDuplicated = false;
                foreach (var user in _users)
                {
                    if (user.Email == userparameter.Email || user.Phone == userparameter.Phone)
                    {
                        isDuplicated = true;
                    }
                    else if (user.Name == userparameter.Name)
                    {
                        if (user.Address == userparameter.Address)
                        {
                            isDuplicated = true;
                            throw new Exception("User is duplicated");
                        }
                    }
                }

                if (!isDuplicated)
                {
                    Debug.WriteLine("User Created");

                    return new Result()
                    {
                        IsSuccess = true,
                        Errors = "User Created"
                    };
                }
                else
                {
                    Debug.WriteLine("The user is duplicated");

                    return new Result()
                    {
                        IsSuccess = false,
                        Errors = "The user is duplicated"
                    };
                }
            }
            catch
            {
                Debug.WriteLine("The user is duplicated");
                return new Result()
                {
                    IsSuccess = false,
                    Errors = "The user is duplicated"
                };
            }
        }
    }
}