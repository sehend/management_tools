using Core.Repositories;
using Core.UnitOfWork;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;



        private AplicationRepository _aplicationRepository;

        private UserNickAndPasswordRepository  _userNickAndPasswordRepository;

        public IApplicationRepository aplication => _aplicationRepository = _aplicationRepository ?? new AplicationRepository(_context);

        public IUserNickAndPasswordRpository  userNickAndPasswordRpository => _userNickAndPasswordRepository = _userNickAndPasswordRepository ?? new UserNickAndPasswordRepository(_context);

        public UnitOfWork(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

      

       

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
