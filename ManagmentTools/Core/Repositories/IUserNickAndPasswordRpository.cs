using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IUserNickAndPasswordRpository : IRepository<UserNickAndPassword>
    {

        System.Collections.IList UserNickAndPasswordConturol(string NickNameOutPut);

    }
}
