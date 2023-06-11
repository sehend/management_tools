using Core.AllSmoInterFace;
using Core.PropertyModel;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Smo
{
    public class AllSmoCode : IAllSmo
    {
        public List<object> DataBaseList(string ConnectionStringName)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionStringName);

            ServerConnection serverConnection = new ServerConnection(sqlConnection);

            Server srv = new Server(serverConnection);

            var objectList = new List<object>();

            try
            {
                for (int i = 0; i < srv.Databases.Count; i++)
                {
                    objectList.Add(srv.Databases[i].Name);
                }

                return objectList;
            }
            catch (Exception)
            {

                return null;
            }






        }
        public List<object> StoredProceduresAllSmo(List<string> ConnectionStringName , string DatabaseName)
        {
            var listString = new List<string>();

            foreach (var item in ConnectionStringName)
            {

                listString.Add(item.ToString());

            }

            SqlConnection sqlConnection = new SqlConnection(listString[0]);

            ServerConnection serverConnection = new ServerConnection(sqlConnection);

            Server srv = new Server(serverConnection);

            Microsoft.SqlServer.Management.Smo.Database BaseDataBase = srv.Databases[DatabaseName];

            var objectList = new List<object>();

            for (int i = 0; i < BaseDataBase.StoredProcedures.Count; i++)
            {
                if (!BaseDataBase.StoredProcedures[i].Schema.Contains("sys"))
                {
                    objectList.Add(new { StoredProceduresName = BaseDataBase.StoredProcedures[i].Name, StoredProceduresSchema = BaseDataBase.StoredProcedures[i].Schema });
                }
               
            }


            return objectList;


        }

        public List<object> ViewsAllSmo(List<string> ConnectionStringName, string DatabaseName)
        {
            var listString = new List<string>();

            foreach (var item in ConnectionStringName)
            {

                listString.Add(item.ToString());

            }

            SqlConnection sqlConnection = new SqlConnection(listString[0]);

            ServerConnection serverConnection = new ServerConnection(sqlConnection);

            Server srv = new Server(serverConnection);

            Microsoft.SqlServer.Management.Smo.Database BaseDataBase = srv.Databases[DatabaseName];

            var objectList = new List<object>();

            for (int i = 0; i < BaseDataBase.Views.Count; i++)
            {
                if (!BaseDataBase.Views[i].Schema.Contains("sys") && !BaseDataBase.Views[i].Schema.Contains("INFORMATION_SCHEMA"))
                {
                    objectList.Add(new { ViewsName = BaseDataBase.Views[i].Name, ViewsSchema = BaseDataBase.Views[i].Schema });
                }

            }


            return objectList;


        }

        public string DatabaseConturol(string ConnectionString, string DatabaseName)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionString);

            ServerConnection serverConnection = new ServerConnection(sqlConnection);

            Server srv = new Server(serverConnection);

            Microsoft.SqlServer.Management.Smo.Database BaseDataBase = srv.Databases[DatabaseName];

            if (BaseDataBase == null)
            {
                return "Bu Database Diger Serverda Yok OluşTurmak İstiyormusun ?";
            }
            else
            {
                return "Başarılı....";
            }
        }



        public List<object> SchemaList(string ConnectionStringName, string DataBaseName)
        {
            SqlConnection sqlConnection = new SqlConnection(ConnectionStringName);

            ServerConnection serverConnection = new ServerConnection(sqlConnection);

            Server srv = new Server(serverConnection);

            Database BaseDataBase = srv.Databases[DataBaseName];




            var objectList = new List<object>();

            Random rnd = new Random();

            try
            {
                for (int i = 0; i < BaseDataBase.Schemas.Count; i++)
                {



                    objectList.Add(new { SchemasName = BaseDataBase.Schemas[i].Name, NodeId = rnd.Next(1000000000) });
                }

                return objectList;
            }
            catch (Exception)
            {

                return null;
            }

        }

        public void CreateDatabaseSmo(string ConnectionStringName, string DataBaseName)
        {
            SqlConnection sqlConnectionOther = new SqlConnection(ConnectionStringName);

            ServerConnection serverConnectionOther = new ServerConnection(sqlConnectionOther);

            Server srvOther = new Server(serverConnectionOther);



            Microsoft.SqlServer.Management.Smo.Database database = new Microsoft.SqlServer.Management.Smo.Database(srvOther, DataBaseName);



            database.Create();

        }
        public List<object> AllDiferentTable(List<string> listString, string DatabaseName)
        {
            SqlConnection sqlConnection = new SqlConnection(listString[0]);

            ServerConnection serverConnection = new ServerConnection(sqlConnection);

            Server srv = new Server(serverConnection);

            Database BaseDataBase = srv.Databases[DatabaseName];

            var BaseTables = BaseDataBase.Tables;

            SqlConnection sqlConnectionOther = new SqlConnection(listString[1]);

            ServerConnection serverConnectionOther = new ServerConnection(sqlConnectionOther);

            Server srvOther = new Server(serverConnectionOther);

            Database OtherDataBase = srvOther.Databases[DatabaseName];

            TableCollection OtherTables = OtherDataBase.Tables;

            Random rnd = new Random();


            var resualt = new List<object>();

            for (int i = 0; i < BaseTables.Count; i++)
            {
                for (int j = 0; j < OtherTables.Count; j++)
                {
                    if (BaseTables[i].Name == OtherTables[j].Name)
                    {
                        var DifferentColumns = new List<object>();

                        var DifferentIndexes = new List<object>();

                        var SameColumns = new List<object>();

                        var AllColumns = new List<object>();

                        var SameIndexes = new List<object>();

                        var AllIndexes = new List<object>();

                        for (int l = 0; l < BaseTables[i].Columns.Count; l++)
                        {
                            AllColumns.Add(BaseTables[i].Columns[l].Name);
                        }

                        for (int k = 0; k < BaseTables[i].Columns.Count; k++)
                        {
                            for (int h = 0; h < OtherTables[j].Columns.Count; h++)
                            {
                                if (BaseTables[i].Columns[k].Name == OtherTables[j].Columns[h].Name)
                                {
                                    SameColumns.Add(BaseTables[i].Columns[k].Name);
                                }
                            }
                        }

                        DifferentColumns = AllColumns.Except(SameColumns).ToList();


                        for (int l = 0; l < BaseTables[i].Indexes.Count; l++)
                        {
                            AllIndexes.Add(BaseTables[i].Indexes[l].Name);
                        }

                        for (int k = 0; k < BaseTables[i].Indexes.Count; k++)
                        {
                            for (int h = 0; h < OtherTables[j].Indexes.Count; h++)
                            {
                                if (BaseTables[i].Indexes[k].Name == OtherTables[j].Indexes[h].Name)
                                {
                                    SameIndexes.Add(BaseTables[i].Indexes[k].Name);
                                }
                            }
                        }

                        DifferentIndexes = AllIndexes.Except(SameIndexes).ToList();

                        if (DifferentColumns.Count > 0 || DifferentIndexes.Count > 0)
                        {
                            resualt.Add(new { TableName = BaseTables[i].Name, Columns = DifferentColumns, Indexes = DifferentIndexes, Schema = BaseTables[i].Schema });
                        }



                    }
                }
            }

            var BaseAllTableName = new List<object>();

            var OtherAllTableName = new List<object>();

            var DifferensesTableName = new List<object>();

            for (int i = 0; i < BaseTables.Count; i++)
            {

                BaseAllTableName.Add(BaseTables[i].Name);

            }

            for (int i = 0; i < OtherTables.Count; i++)
            {

                OtherAllTableName.Add(OtherTables[i].Name);

            }

            DifferensesTableName = BaseAllTableName.Except(OtherAllTableName).ToList();

            for (int i = 0; i < BaseTables.Count; i++)
            {
                for (int k = 0; k < DifferensesTableName.Count; k++)
                {
                    if (BaseTables[i].Name == DifferensesTableName[k].ToString())
                    {
                        var AllColumns = new List<object>();

                        var AllIndexes = new List<object>();

                        for (int l = 0; l < BaseTables[i].Columns.Count; l++)
                        {
                            AllColumns.Add(BaseTables[i].Columns[l].Name);
                        }

                        for (int l = 0; l < BaseTables[i].Indexes.Count; l++)
                        {
                            AllIndexes.Add(BaseTables[i].Indexes[l].Name);
                        }

                        resualt.Add(new { TableName = BaseTables[i].Name, Columns = AllColumns, Indexes = AllIndexes, Schema = BaseTables[i].Schema });
                    }
                }


            }

            return resualt;

        }


    }
}
