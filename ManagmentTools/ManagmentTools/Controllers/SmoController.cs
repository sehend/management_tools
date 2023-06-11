using Core.AllSmoInterFace;
using Core.Model;
using Core.PropertyModel;
using Core.Services;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Hql.Ast;
using NHibernate.Transform;
using NHibernate.Type;
using Service.Smo;
using System;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ManagmentTools.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmoController : ControllerBase
    {
        private readonly IAplicationService _service;

        private readonly IAllSmo _allSmoCode;

        public SmoController(IAplicationService service, IAllSmo allSmoCode)
        {
            _service = service;

            _allSmoCode = allSmoCode;
        }

        [HttpPost("CreateScriptMain12")]
        public IActionResult CreateScriptMain12(CreateScriptMainModel createScriptMainModel)
        {
            if (createScriptMainModel.Scripsts.Count > 0)
            {
                var AplicationNameGetAll = _service.GetDataWihtNickNameAndPassword(createScriptMainModel.NickName, createScriptMainModel.Password);

                if (AplicationNameGetAll != null)
                {
                    var BaseAndOtherConnectionString = _service.GetConnectionStringBaseAndOtherWithEnvarimentName(AplicationNameGetAll, createScriptMainModel.BaseEnvarimantName);

                    if (BaseAndOtherConnectionString.Count > 0)
                    {

                        var listString = new List<string>();

                        foreach (var item in BaseAndOtherConnectionString)
                        {

                            listString.Add(item.ToString());

                        }
                        SqlConnection sqlConnection = new SqlConnection(listString[1]);

                        ServerConnection serverConnection = new ServerConnection(sqlConnection);

                        Server srv = new Server(serverConnection);

                        Microsoft.SqlServer.Management.Smo.Database BaseDataBase = srv.Databases[createScriptMainModel.DatabaseName];

                        for (int i = 0; i < createScriptMainModel.Scripsts.Count; i++)
                        {
                            try
                            {
                                if (!createScriptMainModel.Scripsts[i].Contains("tablosunda Primary Key'i İki Property Paylaşıyor Script Çıkarılamadı....") && !createScriptMainModel.Scripsts[i].Contains("tablosunda Diger Databasede Null Oldugundan Script Çıkarılamadı...."))
                                {
                                    BaseDataBase.ExecuteNonQuery(createScriptMainModel.Scripsts[i]);
                                }


                            }
                            catch (Exception ex)
                            {

                                var error = ex;
                            }



                        }

                    }
                }
            }

            return Ok();

        }
        [HttpPost("DataBaseNameList")]
        public IActionResult DataBaseNameList(EnviramentNameModel enviramentNameModel)
        {
            var AplicationNameGetAll = _service.GetDataWihtNickNameAndPassword(enviramentNameModel.NickName, enviramentNameModel.Password);

            if (AplicationNameGetAll != null)
            {
                var ConnectionString = _service.GetConnectionStringWithEnvarimentName(AplicationNameGetAll, enviramentNameModel.EnviramentName);

                if (ConnectionString.Count > 0)
                {
                    var objectList = _allSmoCode.DataBaseList(ConnectionString[0]);

                    return Ok(objectList);
                }
            }


            return Ok();
        }

        [HttpPost("SchemaList")]
        public IActionResult SchemaList(SchemaListModel schemaListModel)
        {
            var AplicationNameGetAll = _service.GetDataWihtNickNameAndPassword(schemaListModel.NickName, schemaListModel.Password);

            if (AplicationNameGetAll != null)
            {
                var ConnectionString = _service.GetConnectionStringWithEnvarimentName(AplicationNameGetAll, schemaListModel.EnviramentName);


                if (ConnectionString.Count > 0)
                {
                    var objectList = _allSmoCode.SchemaList(ConnectionString[0], schemaListModel.DatabaseName);



                    return Ok(objectList);
                }
            }

            return Ok();


        }

        [HttpPost("CreateDatabaseConturol")]
        public IActionResult CreateDatabaseConturol(CreateDatabaseConturolModel createDatabaseConturolModel)
        {
            var AplicationNameGetAll = _service.GetDataWihtNickNameAndPassword(createDatabaseConturolModel.NickName, createDatabaseConturolModel.Password);

            if (AplicationNameGetAll != null)
            {
                var ConnectionString = _service.GetConnectionStringBaseAndOtherWithEnvarimentName(AplicationNameGetAll, createDatabaseConturolModel.EnviramentName);

                if (ConnectionString.Count > 0)
                {

                    var conturol = _allSmoCode.DatabaseConturol(ConnectionString[1], createDatabaseConturolModel.DatabaseName);

                    return Ok(conturol);


                }
            }

            return Ok();


        }

        [HttpPost("CreateDatabase")]
        public IActionResult CreateDatabase(CreateDatabaseModel createDatabaseModel)
        {
            var AplicationNameGetAll = _service.GetDataWihtNickNameAndPassword(createDatabaseModel.NickName, createDatabaseModel.Password);

            if (AplicationNameGetAll != null)
            {
                var ConnectionString = _service.GetConnectionStringBaseAndOtherWithEnvarimentName(AplicationNameGetAll, createDatabaseModel.EnviramentName);


                _allSmoCode.CreateDatabaseSmo(ConnectionString[1], createDatabaseModel.DatabaseName);

            }

            return Ok();

        }

        [HttpPost("AllManangmentToolsTablo")]
        public IActionResult AllManangmentToolsTablo(AllManangmentToolsTabloModel allManangmentToolsTabloModel)
        {
            var AplicationNameGetAll = _service.GetDataWihtNickNameAndPassword(allManangmentToolsTabloModel.NickName, allManangmentToolsTabloModel.Password);

            if (AplicationNameGetAll != null)
            {
                var BaseAndOtherConnectionString = _service.GetConnectionStringBaseAndOtherWithEnvarimentName(AplicationNameGetAll, allManangmentToolsTabloModel.EnviramentName);

                var listString = new List<string>();

                foreach (var item in BaseAndOtherConnectionString)
                {

                    listString.Add(item.ToString());

                }

                var objectList = _allSmoCode.AllDiferentTable(listString, allManangmentToolsTabloModel.DatabaseName);

                return Ok(objectList);
            }

            return Ok();
        }

        [HttpPost("StoredProcedures")]
        public IActionResult StoredProceduresAll(StoredProceduresAllModel storedProceduresAllModel)
        {
            var AplicationNameGetAll = _service.GetDataWihtNickNameAndPassword(storedProceduresAllModel.NickName, storedProceduresAllModel.Password);

            if (AplicationNameGetAll != null)
            {
                var BaseAndOtherConnectionString = _service.GetConnectionStringBaseAndOtherWithEnvarimentName(AplicationNameGetAll, storedProceduresAllModel.EnviramentName);



                var resualt = _allSmoCode.StoredProceduresAllSmo(BaseAndOtherConnectionString, storedProceduresAllModel.DatabaseName);

                return Ok(resualt);
            }

            return Ok();
        }

        [HttpPost("Views")]
        public IActionResult ViewsAll(ViewsAllModel viewsAllModel)
        {
            var AplicationNameGetAll = _service.GetDataWihtNickNameAndPassword(viewsAllModel.NickName, viewsAllModel.Password);

            if (AplicationNameGetAll != null)
            {
                var BaseAndOtherConnectionString = _service.GetConnectionStringBaseAndOtherWithEnvarimentName(AplicationNameGetAll, viewsAllModel.EnviramentName);



                var resualt = _allSmoCode.ViewsAllSmo(BaseAndOtherConnectionString, viewsAllModel.DatabaseName);

                return Ok(resualt);
            }

            return Ok();
        }

        [HttpPost("CreateScriptAll")]
        public IActionResult CreateScriptAll(CreateScripAllModel createScripAllModel)
        {
            var AplicationNameGetAll = _service.GetDataWihtNickNameAndPassword(createScripAllModel.NickName, createScripAllModel.Password);

            if (AplicationNameGetAll != null)
            {
                var BaseAndOtherConnectionString = _service.GetConnectionStringBaseAndOtherWithEnvarimentName(AplicationNameGetAll, createScripAllModel.BaseEnvarimantName);


                var listString = new List<string>();

                foreach (var item in BaseAndOtherConnectionString)
                {

                    listString.Add(item.ToString());

                }


                SqlConnection sqlConnection = new SqlConnection(listString[0]);

                ServerConnection serverConnection = new ServerConnection(sqlConnection);

                Server srv = new Server(serverConnection);

                Microsoft.SqlServer.Management.Smo.Database BaseDataBase = srv.Databases[createScripAllModel.DatabaseName];

                var BaseTables = BaseDataBase.Tables;

                SqlConnection sqlConnectionOther = new SqlConnection(listString[1]);

                ServerConnection serverConnectionOther = new ServerConnection(sqlConnectionOther);

                Server srvOther = new Server(serverConnectionOther);

                Microsoft.SqlServer.Management.Smo.Database OtherDataBase = srvOther.Databases[createScripAllModel.DatabaseName];

                var BaseConnectionString = listString[0].Contains("Database");

                if (BaseConnectionString == false)
                {
                    listString[0] += $";Database={createScripAllModel.DatabaseName};";
                }

                System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(listString[0]);

                connection.Open();

                System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("select * from INFORMATION_SCHEMA.KEY_COLUMN_USAGE", connection);

                System.Data.SqlClient.SqlDataReader reader = command.ExecuteReader();


                List<KEY_COLUMN_USAGE> Pk = new List<KEY_COLUMN_USAGE>();

                while (reader.Read())
                {


                    Pk.Add(new KEY_COLUMN_USAGE
                    {
                        TABLE_NAME = reader["TABLE_NAME"].ToString(),

                        CONSTRAINT_NAME = reader["CONSTRAINT_NAME"].ToString(),

                        COLUMN_NAME = reader["COLUMN_NAME"].ToString(),

                    });
                }



                System.Data.SqlClient.SqlConnection connection2 = new System.Data.SqlClient.SqlConnection(listString[0]);

                connection2.Open();

                System.Data.SqlClient.SqlCommand command2 = new System.Data.SqlClient.SqlCommand("select * from INFORMATION_SCHEMA.COLUMNS", connection2);

                System.Data.SqlClient.SqlDataReader reader2 = command2.ExecuteReader();


                List<COLUMNS> cOLUMNs = new List<COLUMNS>();

                while (reader2.Read())
                {


                    cOLUMNs.Add(new COLUMNS
                    {
                        TABLE_NAME = reader2["TABLE_NAME"].ToString(),

                        COLUMN_NAME = reader2["COLUMN_NAME"].ToString(),

                        IS_NULLABLE = reader2["IS_NULLABLE"] as string,

                        DATA_TYPE = reader2["DATA_TYPE"].ToString(),

                        CHARACTER_MAXIMUM_LENGTH = reader2["CHARACTER_MAXIMUM_LENGTH"].ToString(),

                        DATETIME_PRECISION = reader2["DATETIME_PRECISION"].ToString(),

                        NUMERIC_SCALE = reader2["NUMERIC_SCALE"].ToString(),

                        NUMERIC_PRECISION = reader2["NUMERIC_PRECISION"].ToString(),



                    });
                }
                System.Data.SqlClient.SqlConnection connection3 = new System.Data.SqlClient.SqlConnection(listString[0]);

                connection3.Open();

                System.Data.SqlClient.SqlCommand command3 = new System.Data.SqlClient.SqlCommand("select T.name as TableName, I.name as IndexName, AC.Name as ColumnName, I.type_desc as IndexType \r\nfrom sys.tables as T inner join sys.indexes as I on T.[object_id] = I.[object_id]    inner join sys.index_columns as IC on IC.[object_id] = I.[object_id] and IC.[index_id] = I.[index_id]    inner join sys.all_columns as AC on IC.[object_id] = AC.[object_id] and IC.[column_id] = AC.[column_id] order by T.name, I.name", connection3);

                System.Data.SqlClient.SqlDataReader reader3 = command3.ExecuteReader();

                List<index> index = new List<index>();


                while (reader3.Read())
                {

                    index.Add(new index
                    {
                        IndexName = reader3["IndexName"].ToString(),

                        Table_Name = reader3["TableName"].ToString(),

                        Column_Name = reader3["ColumnName"].ToString(),

                        type_desc = reader3["IndexType"].ToString(),

                    });

                }
                var OtherTables = OtherDataBase.Tables;

                var scriptAll = new List<object>();

                var differntTable = new List<string>();


                if (createScripAllModel.TableName != null)
                {
                    var AllBaseTable = new List<string>();

                    var AllOtherTable = new List<string>();

                    for (int i = 0; i < BaseTables.Count; i++)
                    {
                        AllBaseTable.Add(BaseTables[i].Name);
                    }


                    for (int i = 0; i < OtherTables.Count; i++)
                    {
                        AllOtherTable.Add(OtherTables[i].Name);
                    }

                    differntTable = AllBaseTable.Except(AllOtherTable).ToList();

                }

                for (int g = 0; g < createScripAllModel.TableName.Count; g++)
                {


                    for (int k = 0; k < differntTable.Count; k++)
                    {
                        if (createScripAllModel.TableName[g] == differntTable[k].ToString())
                        {
                            var table = BaseTables[differntTable[k].ToString()];



                            Table myFirstSMOTable = new Table(OtherDataBase, createScripAllModel.TableName[g]);


                            for (int f = 0; f < table.Columns.Count; f++)
                            {
                                for (int h = 0; h < createScripAllModel.ColLumnsName.Count; h++)
                                {
                                    if (table.Columns[f].Name == createScripAllModel.ColLumnsName[h].ToString())
                                    {


                                        List<string> ColumnsType = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[h].ToString()).Select(x => x.DATA_TYPE).ToList();

                                        List<string> ColumnsIS_NULLABLE = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[h].ToString()).Select(x => x.IS_NULLABLE).ToList();

                                        List<string> CHARACTER_MAXIMUM_LENGTH = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[h].ToString()).Select(x => x.CHARACTER_MAXIMUM_LENGTH).ToList();

                                        List<string> DATETIME_PRECISION = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[h].ToString()).Select(x => x.DATETIME_PRECISION).ToList();

                                        List<string> NUMERIC_SCALE = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[h].ToString()).Select(x => x.NUMERIC_SCALE).ToList();

                                        List<string> NUMERIC_PRECISION = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[h].ToString()).Select(x => x.NUMERIC_PRECISION).ToList();

                                        Column column = null;

                                        if (ColumnsIS_NULLABLE.Count > 0 && ColumnsType.Count > 0)
                                        {



                                            if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "int")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Int);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "int")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Int);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "bigint")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.BigInt);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "bigint")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.BigInt);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "binary" && CHARACTER_MAXIMUM_LENGTH[0] != "-1")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), Microsoft.SqlServer.Management.Smo.DataType.Binary(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "binary" && CHARACTER_MAXIMUM_LENGTH[0] != "-1")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Binary(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "bit")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Bit);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "bit")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Bit);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "char" && CHARACTER_MAXIMUM_LENGTH[0] != "-1")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Char(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0].ToString())));
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "char" && CHARACTER_MAXIMUM_LENGTH[0] != "-1")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Char(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "date")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Date);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "date")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Date);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "datetime")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.DateTime);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "datetime")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.DateTime);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "datetime2" && DATETIME_PRECISION != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.DateTime2(Int32.Parse(DATETIME_PRECISION[0])));
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "datetime2" && DATETIME_PRECISION != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.DateTime2(Int32.Parse(DATETIME_PRECISION[0])));
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "datetimeoffset" && DATETIME_PRECISION != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.DateTimeOffset(Int32.Parse(DATETIME_PRECISION[0])));
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "datetimeoffset" && DATETIME_PRECISION != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.DateTimeOffset(Int32.Parse(DATETIME_PRECISION[0])));
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "decimal" && DATETIME_PRECISION != null && NUMERIC_SCALE != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Decimal(Int32.Parse(NUMERIC_SCALE[0]), Int32.Parse(NUMERIC_PRECISION[0])));
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "decimal" && DATETIME_PRECISION != null && NUMERIC_SCALE != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Decimal(Int32.Parse(NUMERIC_SCALE[0]), Int32.Parse(NUMERIC_PRECISION[0])));
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "float")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Float);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "float")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Float);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "geography")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Geography);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "geography")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Geography);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "geometry")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Geometry);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "geometry")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Geometry);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "hierarchyid")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.HierarchyId);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "hierarchyid")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.HierarchyId);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "image")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Image);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "image")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Image);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "money")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Money);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "money")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Money);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "nchar" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.NChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "nchar")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.NChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "ntext")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.NText);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "ntext")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.NText);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "numeric" && DATETIME_PRECISION != null && NUMERIC_SCALE != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Numeric(Int32.Parse(NUMERIC_SCALE[0]), Int32.Parse(NUMERIC_PRECISION[0])));
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "numeric" && DATETIME_PRECISION != null && NUMERIC_SCALE != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Numeric(Int32.Parse(NUMERIC_SCALE[0]), Int32.Parse(NUMERIC_PRECISION[0])));
                                                column.Nullable = false;
                                            }

                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "nvarchar" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.NChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "nvarchar" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.NChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                column.Nullable = false;
                                            }

                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "nvarchar" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.NVarCharMax);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "nvarchar" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.NVarCharMax);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "real")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Real);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "real")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Real);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "smalldatetime")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.SmallDateTime);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "smalldatetime")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.SmallDateTime);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "smallint")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.SmallInt);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "smallint")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.SmallInt);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "smallmoney")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.SmallMoney);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "smallmoney")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.SmallMoney);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "text")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Text);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "text")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Text);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "time" && DATETIME_PRECISION[0] != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Time(Int32.Parse(DATETIME_PRECISION[0])));
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "time" && DATETIME_PRECISION[0] != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Time(Int32.Parse(DATETIME_PRECISION[0])));
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "timestamp")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Timestamp);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "timestamp")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.Timestamp);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "tinyint")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.TinyInt);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "tinyint")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.TinyInt);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "uniqueidentifier")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.UniqueIdentifier);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "uniqueidentifier")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.UniqueIdentifier);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "varbinary" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.VarBinary(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "varbinary" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.VarBinary(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "varbinary" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.VarBinaryMax);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "varbinary" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.VarBinaryMax);
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "varchar" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.VarChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "varchar" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.VarChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                column.Nullable = false;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "varchar" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.VarCharMax);
                                                column.Nullable = true;
                                            }
                                            else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "varchar" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                            {
                                                column = new Column(myFirstSMOTable, createScripAllModel.ColLumnsName[h].ToString(), DataType.VarCharMax);
                                                column.Nullable = false;
                                            }

                                            if (column != null)
                                            {
                                                myFirstSMOTable.Columns.Add(column);
                                            }





                                        }


                                    }
                                }
                            }

                            if (myFirstSMOTable.Columns.Count > 0)
                            {
                                var CreateTableWithCollums = myFirstSMOTable.Script();

                                var clear = CreateTableWithCollums[1].Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");

                                scriptAll.Add(clear);
                            }



                            for (int l = 0; l < createScripAllModel.IndexName.Count; l++)
                            {




                                List<string> Column_Name = index.Where(x => x.Table_Name == table.Name && x.IndexName == createScripAllModel.IndexName[l]).Select(x => x.Column_Name).ToList();



                                for (int q = 0; q < table.Indexes.Count; q++)
                                {
                                    if (table.Indexes[q].Name == createScripAllModel.IndexName[l].ToString())
                                    {
                                        var DisColums = createScripAllModel.ColLumnsName.Distinct().ToList();

                                        if (Column_Name.Count == 1)
                                        {



                                            for (int x = 0; x < Column_Name.Count; x++)
                                            {
                                                for (int s = 0; s < DisColums.Count; s++)
                                                {
                                                    if (Column_Name[x].ToString() == DisColums[s].ToString())
                                                    {



                                                        Table myFirstSMOTable34 = new Table(OtherDataBase, createScripAllModel.TableName[g]);

                                                        Microsoft.SqlServer.Management.Smo.Index idx = new Microsoft.SqlServer.Management.Smo.Index(myFirstSMOTable34, createScripAllModel.IndexName[l]);

                                                        List<string> type_desc = index.Where(x => x.Table_Name == table.Name && x.IndexName == createScripAllModel.IndexName[l]).Select(x => x.type_desc).ToList();

                                                        type_desc = type_desc.Distinct().ToList();

                                                        Column column = null;

                                                        column = new Column(myFirstSMOTable34, Column_Name[x].ToString(), DataType.Int);

                                                        myFirstSMOTable34.Columns.Add(column);



                                                        for (int p = 0; p < type_desc.Count; p++)
                                                        {
                                                            if (type_desc[p] == "CLUSTERED")
                                                            {

                                                                myFirstSMOTable34.Indexes.Add(idx);
                                                                idx.IndexedColumns.Add(new IndexedColumn(idx, Column_Name[x]));
                                                                idx.IsClustered = true;
                                                                idx.IsUnique = true;
                                                                idx.IndexKeyType = IndexKeyType.DriPrimaryKey;
                                                            }
                                                            else if (type_desc[p] == "NONCLUSTERED")
                                                            {
                                                                myFirstSMOTable34.Indexes.Add(idx);
                                                                idx.IndexedColumns.Add(new IndexedColumn(idx, Column_Name[x]));
                                                                idx.IsClustered = false;
                                                                idx.IsUnique = false;
                                                                idx.IndexKeyType = IndexKeyType.DriPrimaryKey;
                                                            }
                                                        }
                                                        var sehend3 = idx.Script();

                                                        string allScript = sehend3[0].ToString();

                                                        string clearScrip = allScript.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("(", " ").Replace(")", " ").Replace("SORT_IN_TEMPDB = OFF, ONLINE = OFF", " ").Replace("WITH", " ").Replace($"[{Column_Name[x]}]", " ");

                                                        clearScrip += $" ([{Column_Name[x]}])";

                                                        scriptAll.Add(clearScrip);


                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            scriptAll.Add($"{table.Name} tablosunda Primary Key'i İki Property Paylaşıyor Script Çıkarılamadı....");
                                        }




                                    }
                                }


                            }



                        }
                    }
                }



                for (int i = 0; i < BaseTables.Count; i++)
                {
                    for (int j = 0; j < OtherTables.Count; j++)
                    {
                        if (BaseTables[i].Name == OtherTables[j].Name)
                        {



                            for (int h = 0; h < createScripAllModel.TableName.Count; h++)
                            {


                                var CHARACTER_MAXIMUM_LENGTH_SCRİPT = new List<string>();

                                if (createScripAllModel.TableName[h].ToString() == BaseTables[i].Name)
                                {
                                    Table myFirstSMOTable2 = new Table(OtherDataBase, BaseTables[i].Name);

                                    var table = BaseTables[BaseTables[i].Name];

                                    int sayac = 0;

                                    for (int g = 0; g < createScripAllModel.ColLumnsName.Count; g++)
                                    {
                                        for (int y = 0; y < table.Columns.Count; y++)
                                        {


                                            if (createScripAllModel.ColLumnsName[g].ToString() == table.Columns[y].Name)
                                            {
                                                sayac++;


                                                List<string> ColumnsType = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[g].ToString()).Select(x => x.DATA_TYPE).ToList();

                                                List<string> ColumnsIS_NULLABLE = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[g].ToString()).Select(x => x.IS_NULLABLE).ToList();

                                                List<string> CHARACTER_MAXIMUM_LENGTH = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[g].ToString()).Select(x => x.CHARACTER_MAXIMUM_LENGTH).ToList();

                                                CHARACTER_MAXIMUM_LENGTH_SCRİPT.Add(CHARACTER_MAXIMUM_LENGTH[0].ToString());

                                                List<string> DATETIME_PRECISION = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[g].ToString()).Select(x => x.DATETIME_PRECISION).ToList();

                                                List<string> NUMERIC_SCALE = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[g].ToString()).Select(x => x.NUMERIC_SCALE).ToList();

                                                List<string> NUMERIC_PRECISION = cOLUMNs.Where(x => x.TABLE_NAME == table.Name && x.COLUMN_NAME == createScripAllModel.ColLumnsName[g].ToString()).Select(x => x.NUMERIC_PRECISION).ToList();

                                                Column column = null;

                                                string TruScript = null;

                                                if (ColumnsType.Count > 0 && ColumnsIS_NULLABLE.Count > 0 && CHARACTER_MAXIMUM_LENGTH.Count > 0 && DATETIME_PRECISION.Count > 0 && NUMERIC_SCALE.Count > 0 && NUMERIC_PRECISION.Count > 0)
                                                {



                                                    if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "int")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Int);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "int")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Int);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "bigint")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.BigInt);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "bigint")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.BigInt);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "binary" && CHARACTER_MAXIMUM_LENGTH[0] != "-1")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), Microsoft.SqlServer.Management.Smo.DataType.Binary(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "binary" && CHARACTER_MAXIMUM_LENGTH[0] != "-1")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Binary(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "bit")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Bit);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "bit")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Bit);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "char" && CHARACTER_MAXIMUM_LENGTH[0] != "-1")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Char(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0].ToString())));
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "char" && CHARACTER_MAXIMUM_LENGTH[0] != "-1")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Char(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "date")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Date);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "date")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Date);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "datetime")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.DateTime);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "datetime")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.DateTime);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "datetime2" && DATETIME_PRECISION != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.DateTime2(Int32.Parse(DATETIME_PRECISION[0])));
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "datetime2" && DATETIME_PRECISION != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.DateTime2(Int32.Parse(DATETIME_PRECISION[0])));
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "datetimeoffset" && DATETIME_PRECISION != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.DateTimeOffset(Int32.Parse(DATETIME_PRECISION[0])));
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "datetimeoffset" && DATETIME_PRECISION != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.DateTimeOffset(Int32.Parse(DATETIME_PRECISION[0])));
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "decimal" && DATETIME_PRECISION != null && NUMERIC_SCALE != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Decimal(Int32.Parse(NUMERIC_SCALE[0]), Int32.Parse(NUMERIC_PRECISION[0])));
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "decimal" && DATETIME_PRECISION != null && NUMERIC_SCALE != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Decimal(Int32.Parse(NUMERIC_SCALE[0]), Int32.Parse(NUMERIC_PRECISION[0])));
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "float")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Float);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "float")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Float);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "geography")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Geography);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "geography")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Geography);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "geometry")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Geometry);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "geometry")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Geometry);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "hierarchyid")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.HierarchyId);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "hierarchyid")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.HierarchyId);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "image")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Image);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "image")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Image);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "money")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Money);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "money")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Money);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "nchar" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.NChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "nchar")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.NChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "ntext")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.NText);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "ntext")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.NText);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "numeric" && DATETIME_PRECISION != null && NUMERIC_SCALE != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Numeric(Int32.Parse(NUMERIC_SCALE[0]), Int32.Parse(NUMERIC_PRECISION[0])));
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "numeric" && DATETIME_PRECISION != null && NUMERIC_SCALE != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Numeric(Int32.Parse(NUMERIC_SCALE[0]), Int32.Parse(NUMERIC_PRECISION[0])));
                                                        column.Nullable = false;
                                                    }

                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "nvarchar" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.NChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "nvarchar" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.NChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                        column.Nullable = false;
                                                    }

                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "nvarchar" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.NVarCharMax);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "nvarchar" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.NVarCharMax);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "real")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Real);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "real")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Real);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "smalldatetime")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.SmallDateTime);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "smalldatetime")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.SmallDateTime);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "smallint")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.SmallInt);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "smallint")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.SmallInt);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "smallmoney")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.SmallMoney);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "smallmoney")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.SmallMoney);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "text")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Text);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "text")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Text);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "time" && DATETIME_PRECISION[0] != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Time(Int32.Parse(DATETIME_PRECISION[0])));
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "time" && DATETIME_PRECISION[0] != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Time(Int32.Parse(DATETIME_PRECISION[0])));
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "timestamp")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Timestamp);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "timestamp")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.Timestamp);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "tinyint")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.TinyInt);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "tinyint")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.TinyInt);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "uniqueidentifier")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.UniqueIdentifier);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "uniqueidentifier")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.UniqueIdentifier);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "varbinary" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.VarBinary(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "varbinary" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.VarBinary(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "varbinary" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.VarBinaryMax);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "varbinary" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.VarBinaryMax);
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "varchar" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.VarChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "varchar" && CHARACTER_MAXIMUM_LENGTH[0] != "-1" && CHARACTER_MAXIMUM_LENGTH[0] != null)
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.VarChar(Int32.Parse(CHARACTER_MAXIMUM_LENGTH[0])));
                                                        column.Nullable = false;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "YES" && ColumnsType[0] == "varchar" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.VarCharMax);
                                                        column.Nullable = true;
                                                    }
                                                    else if (ColumnsIS_NULLABLE[0] == "NO" && ColumnsType[0] == "varchar" && CHARACTER_MAXIMUM_LENGTH[0] == "-1")
                                                    {
                                                        column = new Column(myFirstSMOTable2, createScripAllModel.ColLumnsName[g].ToString(), DataType.VarCharMax);
                                                        column.Nullable = false;
                                                    }

                                                    if (column != null)
                                                    {
                                                        myFirstSMOTable2.Columns.Add(column);
                                                    }









                                                }

                                            }



                                        }

                                    }

                                    System.Collections.Specialized.StringCollection CreateTableWithCollums = null;

                                    if (sayac > 0)
                                    {
                                        CreateTableWithCollums = myFirstSMOTable2.Script();
                                    }

                                    if (CreateTableWithCollums != null)
                                    {
                                        var Notclear = CreateTableWithCollums[1].Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("(", " ").Replace(")", " ").Replace("CREATE ", "ALTER ");

                                        var clear = Notclear.Split("]").ToList();

                                        string ClearScript = null;

                                        for (int u = 0; u < clear.Count; u++)
                                        {


                                            if (u == 1)
                                            {
                                                clear[u] += "] ADD";

                                                ClearScript += clear[u];
                                            }
                                            else if (u == (clear.Count - 1))
                                            {
                                                ClearScript += clear[u];
                                            }
                                            else
                                            {
                                                clear[u] += "] ";

                                                ClearScript += clear[u];

                                            }

                                        }

                                        if (ClearScript.Contains("max"))
                                        {
                                            ClearScript = ClearScript.Replace("max", "(max)");
                                        }

                                        CHARACTER_MAXIMUM_LENGTH_SCRİPT = CHARACTER_MAXIMUM_LENGTH_SCRİPT.Distinct().ToList();

                                        if (CHARACTER_MAXIMUM_LENGTH_SCRİPT.Count > 0)
                                        {
                                            for (int w = 0; w < CHARACTER_MAXIMUM_LENGTH_SCRİPT.Count; w++)
                                            {
                                                if (CHARACTER_MAXIMUM_LENGTH_SCRİPT[w] != "")
                                                {
                                                    if (CHARACTER_MAXIMUM_LENGTH_SCRİPT[w] != "-1")
                                                    {
                                                        if (ClearScript.Contains($"{CHARACTER_MAXIMUM_LENGTH_SCRİPT[w].ToString()}"))
                                                        {
                                                            var CHARACTER_MAXIMUM_LENGTH = CHARACTER_MAXIMUM_LENGTH_SCRİPT[w].ToString();

                                                            ClearScript = ClearScript.Replace($"{CHARACTER_MAXIMUM_LENGTH}", $"({CHARACTER_MAXIMUM_LENGTH})");
                                                        }
                                                    }
                                                }
                                            }
                                        }


                                        scriptAll.Add(ClearScript);

                                    }




                                    for (int ç = 0; ç < createScripAllModel.IndexName.Count; ç++)
                                    {

                                        int check = 0;


                                        List<string> Column_Name = index.Where(x => x.Table_Name == table.Name && x.IndexName == createScripAllModel.IndexName[ç]).Select(x => x.Column_Name).ToList();

                                        Table myFirstSMOTable34 = new Table(OtherDataBase, table.Name);



                                        List<string> type_desc = index.Where(x => x.Table_Name == table.Name && x.IndexName == createScripAllModel.IndexName[ç]).Select(x => x.type_desc).ToList();


                                        for (int n = 0; n < table.Indexes.Count; n++)
                                        {
                                            if (table.Indexes[n].Name == createScripAllModel.IndexName[ç].ToString())
                                            {

                                                var MainTable = OtherTables[BaseTables[i].Name];

                                                List<string> ColumnsIS_NULLABLE = cOLUMNs.Where(x => x.TABLE_NAME == MainTable.Name && x.COLUMN_NAME == Column_Name[0].ToString()).Select(x => x.IS_NULLABLE).ToList();

                                                for (int q = 0; q < ColumnsIS_NULLABLE.Count; q++)
                                                {
                                                    for (int y = 0; y < createScripAllModel.ColLumnsName.Count; y++)
                                                    {
                                                        if (ColumnsIS_NULLABLE[q].ToString() == createScripAllModel.ColLumnsName[y].ToString())
                                                        {


                                                            if (ColumnsIS_NULLABLE[0] == "NO")
                                                            {
                                                                for (int o = 0; o < Column_Name.Count; o++)
                                                                {


                                                                    Column column = null;

                                                                    column = new Column(myFirstSMOTable34, Column_Name[o].ToString(), DataType.Int);

                                                                    myFirstSMOTable34.Columns.Add(column);

                                                                    Microsoft.SqlServer.Management.Smo.Index id2x = new Microsoft.SqlServer.Management.Smo.Index(myFirstSMOTable34, createScripAllModel.IndexName[ç]);

                                                                    for (int p = 0; p < type_desc.Count; p++)
                                                                    {
                                                                        if (type_desc[p] == "CLUSTERED")
                                                                        {

                                                                            myFirstSMOTable34.Indexes.Add(id2x);
                                                                            id2x.IndexedColumns.Add(new IndexedColumn(id2x, Column_Name[o].ToString()));
                                                                            id2x.IsClustered = true;
                                                                            id2x.IsUnique = true;
                                                                            id2x.IndexKeyType = IndexKeyType.DriPrimaryKey;
                                                                        }
                                                                        else if (type_desc[p] == "NONCLUSTERED")
                                                                        {
                                                                            myFirstSMOTable34.Indexes.Add(id2x);
                                                                            id2x.IndexedColumns.Add(new IndexedColumn(id2x, Column_Name[o].ToString()));
                                                                            id2x.IsClustered = false;
                                                                            id2x.IsUnique = false;
                                                                            id2x.IndexKeyType = IndexKeyType.DriPrimaryKey;
                                                                        }
                                                                    }
                                                                    var sehend3 = id2x.Script();

                                                                    string allScript = sehend3[0].ToString();

                                                                    string clearScrip = allScript.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("(", " ").Replace(")", " ").Replace("SORT_IN_TEMPDB = OFF, ONLINE = OFF", " ").Replace("WITH", " ").Replace($"[{Column_Name[o]}]", " ");

                                                                    clearScrip += $" ([{Column_Name[o]}])";

                                                                    scriptAll.Add(clearScrip);

                                                                    check = 12;
                                                                }
                                                            }

                                                        }
                                                    }
                                                }

                                                if (check == 0)
                                                {
                                                    for (int t = 0; t < createScripAllModel.ColLumnsName.Count; t++)
                                                    {
                                                        for (int ğ = 0; ğ < Column_Name.Count; ğ++)
                                                        {
                                                            if (createScripAllModel.ColLumnsName[t].ToString() == Column_Name[ğ].ToString())
                                                            {
                                                                for (int o = 0; o < Column_Name.Count; o++)
                                                                {


                                                                    Column column = null;

                                                                    column = new Column(myFirstSMOTable34, Column_Name[o].ToString(), DataType.Int);

                                                                    myFirstSMOTable34.Columns.Add(column);

                                                                    Microsoft.SqlServer.Management.Smo.Index id2x = new Microsoft.SqlServer.Management.Smo.Index(myFirstSMOTable34, createScripAllModel.IndexName[ç]);

                                                                    for (int p = 0; p < type_desc.Count; p++)
                                                                    {
                                                                        if (type_desc[p] == "CLUSTERED")
                                                                        {

                                                                            myFirstSMOTable34.Indexes.Add(id2x);
                                                                            id2x.IndexedColumns.Add(new IndexedColumn(id2x, Column_Name[o].ToString()));
                                                                            id2x.IsClustered = true;
                                                                            id2x.IsUnique = true;
                                                                            id2x.IndexKeyType = IndexKeyType.DriPrimaryKey;
                                                                        }
                                                                        else if (type_desc[p] == "NONCLUSTERED")
                                                                        {
                                                                            myFirstSMOTable34.Indexes.Add(id2x);
                                                                            id2x.IndexedColumns.Add(new IndexedColumn(id2x, Column_Name[o].ToString()));
                                                                            id2x.IsClustered = false;
                                                                            id2x.IsUnique = false;
                                                                            id2x.IndexKeyType = IndexKeyType.DriPrimaryKey;
                                                                        }
                                                                    }
                                                                    var sehend3 = id2x.Script();

                                                                    string allScript = sehend3[0].ToString();

                                                                    string clearScrip = allScript.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("(", " ").Replace(")", " ").Replace("SORT_IN_TEMPDB = OFF, ONLINE = OFF", " ").Replace("WITH", " ").Replace($"[{Column_Name[o]}]", " ");

                                                                    clearScrip += $" ([{Column_Name[o]}])";

                                                                    scriptAll.Add(clearScrip);

                                                                    check = 12;

                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (check == 0)
                                                {
                                                    var ColumnName = index.Where(x => x.IndexName == createScripAllModel.IndexName[ç]).Select(x => x.Column_Name).ToList();

                                                    var BaseAndOtherConnectionString2 = _service.GetConnectionStringBaseAndOtherWithEnvarimentName(AplicationNameGetAll, createScripAllModel.BaseEnvarimantName);


                                                    var listString2 = new List<string>();

                                                    foreach (var item in BaseAndOtherConnectionString2)
                                                    {

                                                        listString2.Add(item.ToString());

                                                    }
                                                    System.Data.SqlClient.SqlConnection connection6 = new System.Data.SqlClient.SqlConnection(listString2[1]);

                                                    connection6.Open();

                                                    System.Data.SqlClient.SqlCommand command6 = new System.Data.SqlClient.SqlCommand("select * from INFORMATION_SCHEMA.COLUMNS", connection6);

                                                    System.Data.SqlClient.SqlDataReader reader6 = command6.ExecuteReader();


                                                    List<COLUMNS> cOLUMNs6 = new List<COLUMNS>();

                                                    while (reader6.Read())
                                                    {


                                                        cOLUMNs.Add(new COLUMNS
                                                        {
                                                            TABLE_NAME = reader6["TABLE_NAME"].ToString(),

                                                            COLUMN_NAME = reader6["COLUMN_NAME"].ToString(),

                                                            IS_NULLABLE = reader6["IS_NULLABLE"] as string,

                                                            DATA_TYPE = reader6["DATA_TYPE"].ToString(),

                                                            CHARACTER_MAXIMUM_LENGTH = reader6["CHARACTER_MAXIMUM_LENGTH"].ToString(),

                                                            DATETIME_PRECISION = reader6["DATETIME_PRECISION"].ToString(),

                                                            NUMERIC_SCALE = reader6["NUMERIC_SCALE"].ToString(),

                                                            NUMERIC_PRECISION = reader6["NUMERIC_PRECISION"].ToString(),



                                                        });
                                                    }

                                                    var OtherTableMain2 = OtherTables[table.Name];

                                                    List<string> IsNull = cOLUMNs.Where(x => x.TABLE_NAME == OtherTableMain2.Name && x.COLUMN_NAME == ColumnName[0].ToString()).Select(x => x.IS_NULLABLE).ToList();

                                                    if (IsNull[0] == "YES")
                                                    {
                                                        scriptAll.Add($"{table.Name} tablosunda Diger Databasede Null Oldugundan Script Çıkarılamadı....");
                                                    }
                                                    else
                                                    {

                                                        Column column = null;

                                                        column = new Column(myFirstSMOTable34, ColumnName[0].ToString(), DataType.Int);

                                                        myFirstSMOTable34.Columns.Add(column);

                                                        Microsoft.SqlServer.Management.Smo.Index id2x = new Microsoft.SqlServer.Management.Smo.Index(myFirstSMOTable34, createScripAllModel.IndexName[ç]);

                                                        for (int p = 0; p < type_desc.Count; p++)
                                                        {
                                                            if (type_desc[p] == "CLUSTERED")
                                                            {

                                                                myFirstSMOTable34.Indexes.Add(id2x);
                                                                id2x.IndexedColumns.Add(new IndexedColumn(id2x, ColumnName[0].ToString()));
                                                                id2x.IsClustered = true;
                                                                id2x.IsUnique = true;
                                                                id2x.IndexKeyType = IndexKeyType.DriPrimaryKey;
                                                            }
                                                            else if (type_desc[p] == "NONCLUSTERED")
                                                            {
                                                                myFirstSMOTable34.Indexes.Add(id2x);
                                                                id2x.IndexedColumns.Add(new IndexedColumn(id2x, ColumnName[0].ToString()));
                                                                id2x.IsClustered = false;
                                                                id2x.IsUnique = false;
                                                                id2x.IndexKeyType = IndexKeyType.DriPrimaryKey;
                                                            }
                                                        }
                                                        var sehend3 = id2x.Script();

                                                        string allScript = sehend3[0].ToString();

                                                        string clearScrip = allScript.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ").Replace("(", " ").Replace(")", " ").Replace("SORT_IN_TEMPDB = OFF, ONLINE = OFF", " ").Replace("WITH", " ").Replace($"[{ColumnName[0]}]", " ");

                                                        clearScrip += $" ([{ColumnName[0]}])";

                                                        scriptAll.Add(clearScrip);

                                                        check = 12;
                                                    }
                                                }




                                            }
                                        }


                                    }

                                }


                            }



                        }
                    }
                }




                if (createScripAllModel.Views != null)
                {
                    for (int i = 0; i < BaseDataBase.Views.Count; i++)
                    {
                        for (int j = 0; j < createScripAllModel.Views.Count; j++)
                        {
                            if (BaseDataBase.Views[i].Name == createScripAllModel.Views[j])
                            {
                                View myview = new View(OtherDataBase, createScripAllModel.Views[i].ToString());

                                myview.TextHeader = BaseDataBase.Views[i].TextHeader;

                                myview.TextBody = BaseDataBase.Views[i].TextBody;

                                var ViewScript = myview.Script();

                                scriptAll.Add(ViewScript[0].ToString());

                            }

                        }
                    }

                }


                if (createScripAllModel.StoredProcedures != null)
                {
                    for (int i = 0; i < BaseDataBase.StoredProcedures.Count; i++)
                    {
                        for (int j = 0; j < createScripAllModel.StoredProcedures.Count; j++)
                        {
                            if (BaseDataBase.StoredProcedures[i].Name == createScripAllModel.StoredProcedures[j])
                            {
                                StoredProcedure sp = new StoredProcedure(OtherDataBase, createScripAllModel.StoredProcedures[i].ToString());

                                sp.TextBody = BaseDataBase.StoredProcedures[i].TextBody;

                                sp.TextMode = false;

                                sp.AnsiNullsStatus = false;

                                sp.QuotedIdentifierStatus = false;

                                var StoredProceduresScript = sp.Script();

                                var StoredProceduresScriptDemo = StoredProceduresScript[2].ToString();

                                StoredProceduresScriptDemo = StoredProceduresScriptDemo.Replace("\r", " ").Replace("\n", " ");
                           


                                scriptAll.Add(StoredProceduresScriptDemo);
                            }

                        }
                    }

                }








                return Ok(scriptAll);
            }

            return Ok();
        }
    }
}


