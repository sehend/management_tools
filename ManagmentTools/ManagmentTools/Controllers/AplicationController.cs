using Core.Model;
using Core.PropertyModel;
using Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ManagmentTools.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AplicationController : ControllerBase
    {
        private readonly IAplicationService _service;

        public AplicationController(IAplicationService service)
        {
            _service = service;
        }

        [HttpPost("GetAll")]
        public IActionResult AplicationGetAll(AplicationGetAllModel aplicationGetAllModel)
        {

            var AplicationGetAll = _service.GetDataWihtNickNameAndPassword(aplicationGetAllModel.NickName, aplicationGetAllModel.Password);

            if (AplicationGetAll != null)
            {
                return Ok(AplicationGetAll);
            }
            else
            {
                return Ok();
            }


        }

        [HttpPost("GetAplicationName")]
        public IActionResult GetAplicationName(GetAplicationNameModel getAplicationNameModel)
        {

            var AplicationNameGetAll = _service.GetDataWihtNickNameAndPassword(getAplicationNameModel.NickName, getAplicationNameModel.Password);

            if (AplicationNameGetAll != null)
            {
                List<string> resualt = _service.GetAplicationName(AplicationNameGetAll);

                resualt.Remove(resualt[0]);

                return Ok(resualt);
            }
            else
            {
                return Ok();
            }



        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> AplicationDelete(int id)
        {
            var AplicationGetById = await _service.GetByIdAsync(id);

            _service.Remove(AplicationGetById);


            return NoContent();
        }

        [HttpPut]
        public IActionResult AplicationUpdate(AplicationUpdateModel aplicationUpdateModel)
        {

            _service.UpdateData(aplicationUpdateModel);




            return NoContent();
        }

        [HttpPost("AplicationConnectionStringConturol")]
        public IActionResult AplicationConnectionStringConturol(AplicationConnectionStringConturolModel aplicationConnectionStringConturolModel)
        {

            bool conturolServer = false;

            bool conturolServer2 = false;

            bool conturolDatabase = false;

            bool conturolSDatabase2 = false;

            bool conturolSCatalog = false;

            bool conturolSCatalog2 = false;


            if (!aplicationConnectionStringConturolModel.ConnectionString.Contains("Server ="))
            {
                conturolServer = true;
            }

            if (!aplicationConnectionStringConturolModel.ConnectionString.Contains("Server="))
            {
                conturolServer2 = true;
            }

            if (conturolServer == true && conturolServer2 == true)
            {
                return Ok("Server Parametresi ConnectionString'te Bulunmalıdır.....");
            }

            if (aplicationConnectionStringConturolModel.ConnectionString.Contains("Database ="))
            {
                conturolDatabase = true;
            }

            if (aplicationConnectionStringConturolModel.ConnectionString.Contains("Database="))
            {
                conturolSDatabase2 = true;
            }

            if (conturolDatabase == true || conturolSDatabase2 == true)
            {
                return Ok("Database Parametresi ConnectionString De Bulunamaz....");
            }


            if (aplicationConnectionStringConturolModel.ConnectionString.Contains("Catalog ="))
            {
                conturolSCatalog = true;
            }

            if (aplicationConnectionStringConturolModel.ConnectionString.Contains("Catalog="))
            {
                conturolSCatalog2 = true;
            }

            if (conturolSCatalog == true || conturolSCatalog2 == true)
            {
                return Ok("Catalog Parametresi ConnectionString De Bulunamaz....");
            }







            return Ok("Başarılı....");

        }

        [HttpPost]
        public IActionResult AplicationCreate(AplicationCreateModel aplicationCreateModel)
        {
            if (aplicationCreateModel.EnvarimantName != "" && aplicationCreateModel.ConnectionString != "" && aplicationCreateModel.NickName != "" && aplicationCreateModel.Password != "")
            {
                var AplicationAll = _service.AplicationCreate(aplicationCreateModel.EnvarimantName, aplicationCreateModel.ConnectionString, aplicationCreateModel.NickName, aplicationCreateModel.Password);

                return Ok(AplicationAll);
            }
            else
            {
                return Ok();
            }

        }


    }
}
