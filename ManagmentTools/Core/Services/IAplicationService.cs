using Core.Model;
using Core.PropertyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IAplicationService : IService<Aplication>
    {
        List<string> GetConnectionStringWithEnvarimentName(List<Aplication> aplications, string Name);

        List<string> GetConnectionStringBaseAndOtherWithEnvarimentName(List<Aplication> aplications,string EnvarimantName);

        Aplication AplicationCreate(string EnvarimantName, string ConnectionString, string nickName, string Password);

        List<string> GetAplicationName(List<Aplication> DataNotClear);

        List<Aplication> GetDataWihtNickNameAndPassword(string NickNameOutPut, string PasswordOutPut);

        void UpdateData(AplicationUpdateModel aplicationUpdateModel);
    }
}
