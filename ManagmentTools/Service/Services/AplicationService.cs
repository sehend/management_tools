using Core.Model;
using Core.PropertyModel;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWork;
using Data.Repositories;
using Data.UnitOfWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class AplicationService : Service<Aplication>, IAplicationService
    {
        public AplicationService(IUnitOfWork unitOfWork, IRepository<Aplication> repository) : base(unitOfWork, repository)
        {
        }

        public Aplication AplicationCreate(string EnvarimantName, string ConnectionString, string nickName, string Password)
        {
            return _unitOfWork.aplication.AplicationCreate(EnvarimantName, ConnectionString, nickName, Password);
        }

        public List<string> GetAplicationName(List<Aplication> DataNotClear)
        {
            return _unitOfWork.aplication.GetAplicationName(DataNotClear);
        }

        public List<string> GetConnectionStringBaseAndOtherWithEnvarimentName(List<Aplication> aplications,string EnvarimantName)
        {
            return _unitOfWork.aplication.GetConnectionStringBaseAndOtherWithEnvarimentName(aplications,EnvarimantName);
        }

        public List<string> GetConnectionStringWithEnvarimentName(List<Aplication> aplications, string Name)
        {
           return _unitOfWork.aplication.GetConnectionStringWithEnvarimentName(aplications,Name);
        }

        public void UpdateData(AplicationUpdateModel aplicationUpdateModel)
        {
            _unitOfWork.aplication.UpdateData(aplicationUpdateModel);
        }

        public List<Aplication> GetDataWihtNickNameAndPassword(string NickNameOutPut, string PasswordOutPut)
        {
            return _unitOfWork.aplication.GetDataWihtNickNameAndPassword(NickNameOutPut, PasswordOutPut);
        }

      
    }
}
