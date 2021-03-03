using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PPB.BL.UnitOfWork;
using PPB.Client.Models;
using PPB.DAL.Concrete;
using PPB.Server.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PPB.Server.Controllers
{
    [ApiController]

    public class UserController : ControllerBase
    {

        private readonly AppSettings _appSettings;
        UnitOfWork _unitOfWork = new UnitOfWork();
        public UserController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

        }

        [HttpPost]
        [Route("create-user")]
        public IActionResult CreateUser(string UserName, string Password,string NameSurname)
        {

            if (UserName.Trim().Length >= 4 && UserName.Trim().Length <= 20)
            {
                if (!_unitOfWork.Users.Any(x => x.UserName == UserName))
                {
                    _unitOfWork.Users.Insert(new Users
                    {
                        UserName = UserName,
                        Password = Password,
                        NameSurname= NameSurname
                    });
                    _unitOfWork.Save();
                    return Ok("Registration Successful");
                }
                else
                {
                    return Ok("This username is already exist.");
                }
            }
            else
            {
                return Ok("Your username must be between 8-20 characters.");

            }
        }
        [HttpPost]
        [Route("login-user")]
        public IActionResult LoginUser(string UserName, string Password)
        {
             if (_unitOfWork.Users.Any(x => x.UserName == UserName && x.Password == Password))
            {
                //Kart paketlerini oluştur

               
                if (!_unitOfWork.PlayerLobby.Any(x => x.UserName == UserName))
                {
                    _unitOfWork.PlayerLobby.Insert(new PlayerLobby
                    {
                        UserName = UserName
                    });
                }
                _unitOfWork.BattleLogs.RemoveRange(_unitOfWork.BattleLogs.Where(x => x.UserName == UserName));
                _unitOfWork.Save();
                 if (_unitOfWork.PlayerLobby.GetList().Count() >= 0)
                {
                    JwtSecurityTokenHandler tokenHandler;
                    SecurityToken token;
                    CreateCheckToken(UserName, out tokenHandler, out token);
                    UserAuthentication userAuthentication = new UserAuthentication
                    {
                        Auth = true,
                        Response = "Your login is successfully.Please Copy Token: " + tokenHandler.WriteToken(token),
                    };
               
                    return Ok(JsonConvert.SerializeObject(userAuthentication));
                }
                else
                {

                    if (_unitOfWork.PlayerLobby.GetList().Count() == 1)
                    {
                        JwtSecurityTokenHandler tokenHandler;
                        SecurityToken token;
                        CreateCheckToken(UserName, out tokenHandler, out token);
                        UserAuthentication userAuthentication = new UserAuthentication
                        {
                            Auth = true,
                            Response = "Your login is successfully.Please Copy Token: " + tokenHandler.WriteToken(token)
                        };
                        return Ok(JsonConvert.SerializeObject(userAuthentication));

                    }
                    else
                    {
                        JwtSecurityTokenHandler tokenHandler;
                        SecurityToken token;
                        CreateCheckToken(UserName, out tokenHandler, out token);
                        return Ok("Other player is ready ! Press Enter and Start Game Please Copy Token: " + tokenHandler.WriteToken(token));
                    }


                }

            }
            else
            {
                return Ok("Your username or password incorrect.");
            }
        }

        private void CreateCheckToken(string UserName, out JwtSecurityTokenHandler tokenHandler, out SecurityToken token)
        {
            tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                              new Claim(System.Security.Claims.ClaimTypes.Name, UserName)

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            token = tokenHandler.CreateToken(tokenDescriptor);
        }

        [HttpDelete]
        [Route("delete-user")]
        public IActionResult DeleteUser(string UserName, string Password)
        {

            if (_unitOfWork.Users.Any(x => x.UserName == UserName && x.Password == Password))
            {
                _unitOfWork.Users.Delete(_unitOfWork.Users.FirstOrDefault(x => x.UserName == UserName));
                _unitOfWork.Save();
                return Ok("User deletion is complete.");
            }
            else
            {
                return BadRequest("Check username or password and try again.");
            }

        }

        [HttpPut]
        [Route("update-user")]
        public IActionResult UpdateUser(string UserName, string Password, string newUserName, string newPassword)
        {


            if (_unitOfWork.Users.Any(x => x.UserName == UserName && x.Password == Password))
            {
                Users user = _unitOfWork.Users.Find(x => x.UserName == UserName && x.Password == Password);
                user.Password = newPassword;
                user.UserName = newUserName;
                _unitOfWork.Users.Update(user);
                _unitOfWork.Save();
                return Ok("User update process is complete.");
            }
            else
            {
                return BadRequest("Check the information you entered.");
            }
        }

    }
}
