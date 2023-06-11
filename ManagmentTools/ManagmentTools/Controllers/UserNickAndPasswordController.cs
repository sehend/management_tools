using Core.Model;
using Core.PropertyModel;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagmentTools.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNickAndPasswordController : ControllerBase
    {
        private readonly IUserNickAndPasswordService _service;

        public UserNickAndPasswordController(IUserNickAndPasswordService service)
        {
            _service = service;
        }

        [HttpPost("CreateUserNickAndPassword")]
        public async Task<IActionResult> CreateUserNickAndPasswordMain(UserNickAndPassword userNickAndPassword)
        {

            if (userNickAndPassword.Password != "" && userNickAndPassword.NickName != "")
            {
                await _service.AddAsync(userNickAndPassword);


                return Ok();
            }



            return Ok();

        }

        [HttpPost("UserNickAndPasswordConturol")]
        public IActionResult UserNickAndPasswordConturolMain(UserNickAndPasswordConturolModel   userNickAndPasswordConturolModel)
        {
            var conturol = _service.UserNickAndPasswordConturol(userNickAndPasswordConturolModel.NickName);

            if (conturol.Count > 0)
            {
                return Ok("Kulanıcı Adı Başka Kulanıcı Tarafından Kulanılmaktadır....");
              
            }
            else
            {
                return Ok("Başarılı....");
            }



        }
    }
}
