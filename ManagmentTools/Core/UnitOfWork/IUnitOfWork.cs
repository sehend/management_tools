using Core.Model;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        IApplicationRepository aplication { get; }

        IUserNickAndPasswordRpository  userNickAndPasswordRpository { get; }

        Task CommitAsync();


        void Commit();

    }
}
