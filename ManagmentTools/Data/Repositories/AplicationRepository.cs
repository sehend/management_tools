using Azure.Core;
using Core.Model;
using Core.PropertyModel;
using Core.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Smo;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Transform;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Data.Repositories
{
    public class AplicationRepository : Repository<Aplication>, IApplicationRepository
    {
        private AppDbContext _appDbContext { get => _context as AppDbContext; }

        public AplicationRepository(AppDbContext context) : base(context)
        {

        }
        public List<Aplication> GetDataWihtNickNameAndPassword(string NickNameOutPut, string PasswordOutPut)
        {

            var userId = _appDbContext.UserNickAndPasswords.Where(x => x.NickName == NickNameOutPut && x.Password == PasswordOutPut).ToList();

            if (userId.Count > 0)
            {
                var data = _appDbContext.Aplications.Where(x => x.IdUserNickAndPassword == userId[0].IdUserNickAndPassword).ToList();

                return data;
            }
            else
            {
                return null;
            }



        }

        public void UpdateData(AplicationUpdateModel aplicationUpdateModel)
        {
            var oldData = _appDbContext.Aplications.Where(x=>x.Id == aplicationUpdateModel.Id).ToList();

            Aplication aplication = new Aplication();

          

            aplication.EnvarimantName = aplicationUpdateModel.EnvarimantName;

            aplication.Line = oldData[0].Line;

            aplication.ConnectionString = aplicationUpdateModel.ConnectionString;

            aplication.IdUserNickAndPassword = oldData[0].IdUserNickAndPassword;

            _appDbContext.Update(aplication);

            _appDbContext.SaveChanges();

           

        }


        public List<string> GetConnectionStringWithEnvarimentName(List<Aplication> aplications , string EnvarimantName)
        {
            

            var BaseDataBase = aplications.Where(x => x.EnvarimantName == EnvarimantName).Select(x => x.Line).ToList();

            int lineBase = Convert.ToInt32(BaseDataBase[0]) - 1;

            var EnvarimantList = aplications.Where(x => x.Line == lineBase).Select(x => x.ConnectionString).ToList();

            return EnvarimantList;
        }

        public List<string> GetConnectionStringBaseAndOtherWithEnvarimentName(List<Aplication> aplications,string EnvarimantName)
        {


            var BaseDataBase = aplications.Where(x => x.EnvarimantName == EnvarimantName).Select(x => x.Line).ToList();

            int lineBase = Convert.ToInt32(BaseDataBase[0]) - 1;

            var BaseConnectionString = aplications.Where(x => x.Line == lineBase).Select(x => x.ConnectionString).ToList();

            var OtherConnectionString = aplications.Where(x => x.EnvarimantName == EnvarimantName).Select(x => x.ConnectionString).ToList();

            List<string> objectlist = new List<string>();

            objectlist.Add(BaseConnectionString[0].ToString());

            objectlist.Add(OtherConnectionString[0].ToString());

            return objectlist;
        }


        public Aplication AplicationCreate(string EnvarimantName, string ConnectionString, string nickName, string Password)
        {
            var userDataUserNickAndPasswords = _appDbContext.UserNickAndPasswords.Where(x => x.NickName == nickName && x.Password == Password).ToList();

            if (userDataUserNickAndPasswords.Count > 0)
            {
                var AllLine = _appDbContext.Aplications.Select(x => x.Line).ToList();

                Aplication Newaplication = new Aplication();

                Newaplication.EnvarimantName = EnvarimantName;

                Newaplication.ConnectionString = ConnectionString;

                Newaplication.Line = AllLine.Count();

                Newaplication.IdUserNickAndPassword = userDataUserNickAndPasswords[0].IdUserNickAndPassword;






                _appDbContext.Aplications.Add(Newaplication);

                _appDbContext.SaveChanges();

                return Newaplication;
            }
            else
            {
                return null;
            }

        }

        public List<string> GetAplicationName(List<Aplication> DataNotClear)
        {

            var data = DataNotClear.Select(x=>x.EnvarimantName).ToList();

            var listStringEnvarimentName = new List<string>();

            for (int i = 0; i < data.Count; i++)
            {
                listStringEnvarimentName.Add(data[i].ToString());
            }

            return listStringEnvarimentName;
        }

       
    }
}
