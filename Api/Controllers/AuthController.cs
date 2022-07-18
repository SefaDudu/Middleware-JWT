using Api.CustomAttribute;
using Api.Model;
using Api.VM;
using Business.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        Context Context = new Context();
        private readonly IConfiguration _configuration;
        private readonly Token _token;

        public AuthController(IConfiguration configuration, Token token)
        {
            _configuration = configuration;
            _token = token;

        }
        [HttpPost("Login")]
        public string Login(LoginVM model)
        {
            var hashedPassword = Hash.HashString(model.Password);
            var user = Context.Users.FirstOrDefault(x => x.UserName == model.Email && x.Password == model.Password);

            var tokenSecret = _configuration.GetSection("JWTOptions").GetSection("Token").Value;
            var token = _token.CreateJWT(user, tokenSecret);
            return token;
        }


        [CustomAuthorize]
        [HttpPost("Logout")]
        public bool Logout()
        {
            var getUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var token = Context.UserTokens.Where(x => x.UserId == Convert.ToInt64(getUserId));
            foreach (var a in token)
            {
                Context.UserTokens.Remove(a);
            }
            Context.SaveChanges();
            return true;
        }

        //[HttpPost("Register")]
        //public ResultDto Register(RegisterVM model)
        //{
        //    var result = _userService.Register(model);
        //    return result;

        //}
        //         try
        //            {
        //                var checkEmail = _userDal.Get(x => x.Email == model.Email);
        //                if (checkEmail != null)
        //                {
        //                    return new ResultDto()
        //        {
        //            IsSuccess = false, Message = "Mail adresi mevcut"
        //                    };
        //    }

        //    var user = new User
        //    {
        //        Email = model.Email,
        //        Password = Hash.HashString(model.Password),
        //        BirthDate = model.BirthDate,
        //        Code = Guid.NewGuid(),
        //        CreatedDate = DateTime.Now,
        //        FirstName = model.FirstName,
        //        LastName = model.LastName,
        //        IsActive = true,
        //        PhoneNumber = model.PhoneNumber,
        //        Photo = model.Photo,

        //    };
        //    _userDal.Add(user);
        //                return new ResultDto()
        //    {
        //        IsSuccess = true,Message = "Kayıt işlemi başarı ile gerçekleştirildi"
        //                };
        //}
        //            catch (Exception e)
        //{
        //    return new ResultDto()
        //    {
        //        IsSuccess = false,
        //        Message = e.Message
        //    };
        //}

    }
}
