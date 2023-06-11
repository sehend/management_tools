using Core.Model;
using Core.Repositories;
using Core.Services;
using Core.UnitOfWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class UserNickAndPasswordService : Service<UserNickAndPassword>, IUserNickAndPasswordService
    {
        public UserNickAndPasswordService(IUnitOfWork unitOfWork, IRepository<UserNickAndPassword> repository) : base(unitOfWork, repository)
        {
        }

        public IList UserNickAndPasswordConturol(string NickNameOutPut)
        {
            return _unitOfWork.userNickAndPasswordRpository.UserNickAndPasswordConturol(NickNameOutPut);
        }
    }
}
