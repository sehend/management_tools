using Core.Model;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class UserNickAndPasswordRepository : Repository<UserNickAndPassword>, IUserNickAndPasswordRpository
    {
        private AppDbContext _appDbContext { get => _context as AppDbContext; }

        public UserNickAndPasswordRepository(AppDbContext context) : base(context)
        {

        }

        public System.Collections.IList UserNickAndPasswordConturol(string NickNameOutPut)
        {

            var userNicName = _appDbContext.UserNickAndPasswords.Where(x => x.NickName == NickNameOutPut).Select(x=>x.NickName).ToList();



            return userNicName;

        }
    }
}    
