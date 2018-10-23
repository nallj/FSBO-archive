using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using System;
//using WebApi.Services;
//using WebApi.Dtos;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
//using WebApi.Helpers;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
//using WebApi.Entities;
using Microsoft.AspNetCore.Authorization;

using FSBO.WebServices.Models;
using FSBO.WebServices.Services;
using Dto = FSBO.WebServices.Models.Dto;
using System.Threading.Tasks;

namespace FSBO.WebServices.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IMapper _mapper;
        private readonly WebServicesSettings WebServicesSettings;
        private IUserService UserSvc;

        public UserController(IUserService userSvc, IMapper mapper, IOptions<WebServicesSettings> appSettings)
        {
            UserSvc = userSvc;

            _mapper = mapper;
            WebServicesSettings = appSettings.Value;
        }


        //[HttpGet("Subscribers")]
        //public IEnumerable<Subscriber> GetSubscribers()
        //{
        //    var serviceResponse = UserSvc.GetSubscribers();
        //    return serviceResponse;
        //}

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]Dto.User userDto)
        {
            var user = UserSvc.Authenticate(userDto.Username, userDto.Password);

            if (user == null)
                return Unauthorized();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(WebServicesSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info (without password) and token to store client side
            return Ok(new
            {
                Id = user.UserId,
                Username = user.Username,
                Email = user.Email,
                Token = tokenString
            });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]Dto.RegistrationData registrationDto)
        {
            // map dto to entity
            //var user = _mapper.Map<DAL.User>(registrationDto);

            try
            {
                // save 
                await UserSvc.Create(registrationDto);
                return Ok();
            }
            catch (Exception ex) //(AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = UserSvc.GetAll();
            var userDtos = _mapper.Map<IList<Dto.User>>(users);
            return Ok(userDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = UserSvc.GetById(id);
            var userDto = _mapper.Map<Dto.User>(user);
            return Ok(userDto);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]Dto.User userDto)
        {
            // map dto to entity and set id
            var user = _mapper.Map<DAL.User>(userDto);
            user.UserId = id;

            try
            {
                // save 
                UserSvc.Update(user, userDto.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            UserSvc.Delete(id);
            return Ok();
        }

    }
}