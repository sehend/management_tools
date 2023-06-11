import React from 'react';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Button from '@mui/material/Button';
import axios from 'axios';
import Select, { SelectChangeEvent } from '@mui/material/Select';
import TreeView from '@mui/lab/TreeView';
import TreeItem from '@mui/lab/TreeItem';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import Paper from '@mui/material/Paper';
import Modal from '@mui/material/Modal';
import Stack from '@mui/material/Stack';
import Box from '@mui/material/Box';
import Checkbox from "@mui/material/Checkbox";
import { FormControlLabel } from '@mui/material';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';


const style = {
  position: 'absolute' as 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 400,
  height: 400,
  bgcolor: 'background.paper',
  border: '2px solid #000',
  boxShadow: 24,
  p: 4,
};


function ManagmentToolMainManü() {

  const [envarimantLİst, setEnvarimantLİst] = React.useState<string[]>([]);

  const [dataBaseNameList, setDataBaseNameList] = React.useState<any>([]);

  const [envarimant, setEnvarimant] = React.useState('');

  const [databaseName, setdatabaseName] = React.useState('');

  const [schemaList, setSchemaList] = React.useState<any>([]);

  const [allTable, setAllTable] = React.useState<any>([]);

  const [storedProcedures, setStoredProcedures] = React.useState<any>([]);

  const [views, setViews] = React.useState<any>([]);




  const [scriptShow12, setScriptShow12] = React.useState<string[]>([""]);

  const [nickName, setNickName] = React.useState<any>([]);

  const [password, setPassword] = React.useState<any>([]);

  const dataBaseConturol = React.useRef('');

  const [tableAll, setTableAll] = React.useState<string[]>([]);

  const [viewsAll, setviewsAll] = React.useState<string[]>([]);

  const [indexAll, setIndexAll] = React.useState<string[]>([]);

  const [columAll, setColumAll] = React.useState<string[]>([]);

  const [storedProceduresAll, setStoredProceduresAll] = React.useState<string[]>([]);

  const [open, setOpen] = React.useState(false);

  const handleOpen = () => setOpen(true);

  const handleClose = () => { setOpen(false); }

  const newPage = () => {


    window.location.reload();

  }


  const GetAplicationName = async () => {


    const response = await axios.post('https://localhost:7203/api/Aplication/GetAplicationName', {


      NickName: nickName,

      Password: password,


    });

    setEnvarimantLİst(response.data);





  }






  const DataBaseNameList = async () => {





    if (envarimant !== "") {
      console.log(envarimant);
      const response = await axios.post('https://localhost:7203/api/Smo/DataBaseNameList', {

        enviramentName: envarimant,

        NickName: nickName,

        Password: password,

      });

      console.log(response.data);

      setDataBaseNameList(response.data);

    }




  };
  const DataBaseConturol = async () => {





    if (envarimant !== "" && databaseName !== "") {

      const response = await axios.post('https://localhost:7203/api/Smo/CreateDatabaseConturol', {

        EnviramentName: envarimant,

        DatabaseName: databaseName,

        NickName: nickName,

        Password: password,

      });


      dataBaseConturol.current = response.data;

      console.log(dataBaseConturol.current);
      if (dataBaseConturol.current === "Başarılı....") {

        SchemaList();

      } else {

        handleOpen();



      }


    }
    else {



    }




  }

  const CreateDatabase = async () => {





    if (envarimant !== "" && databaseName !== "") {

      await axios.post('https://localhost:7203/api/Smo/CreateDatabase', {

        EnviramentName: envarimant,

        DatabaseName: databaseName,

        NickName: nickName,

        Password: password,

      });
      handleClose();




    }
    else {


    }
  }
  const SchemaList = async () => {

    const response = await axios.post('https://localhost:7203/api/Smo/SchemaList', {

      EnviramentName: envarimant,

      DatabaseName: databaseName,

      NickName: nickName,

      Password: password,

    });

    setSchemaList(response.data);

    AllTable();


  }

  const Views24 = async () => {


    const response = await axios.post('https://localhost:7203/api/Smo/Views', {

      EnviramentName: envarimant,

      DatabaseName: databaseName,

      NickName: nickName,

      Password: password,

    });

    setViews(response.data);

    console.log(response.data);

  }

  const StoredProcedures24 = async () => {


    const response = await axios.post('https://localhost:7203/api/Smo/StoredProcedures', {

      EnviramentName: envarimant,

      DatabaseName: databaseName,

      NickName: nickName,

      Password: password,

    });

    setStoredProcedures(response.data);



  }

  const AllTable = async () => {

    const response = await axios.post('https://localhost:7203/api/Smo/AllManangmentToolsTablo', {

      enviramentName: envarimant,

      databaseName: databaseName,

      NickName: nickName,

      Password: password,

    });

    setAllTable(response.data);

    StoredProcedures24();

    Views24();


  }

  const handleChange = (event: SelectChangeEvent) => {



    setEnvarimant(event.target.value);

    console.log(event.target.value);


  };

  const handleChange2 = (event: SelectChangeEvent) => {



    setdatabaseName(event.target.value);


  };

  const handlerIndexChange = (event: React.ChangeEvent<HTMLInputElement>) => {

    const index = indexAll.indexOf(event.target.value);

    if (index === -1) {

      setIndexAll([...indexAll, event.target.value]);
    } else {

      setIndexAll(indexAll.filter((item) => item !== event.target.value));

    }

  }

  const handlerProceduresNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {

    const index = storedProceduresAll.indexOf(event.target.value);

    if (index === -1) {

      setStoredProceduresAll([...storedProceduresAll, event.target.value]);
    } else {

      setStoredProceduresAll(storedProceduresAll.filter((item) => item !== event.target.value));

    }

  }

  const handlerViewsNameChange = (event: React.ChangeEvent<HTMLInputElement>) => {

    const index = viewsAll.indexOf(event.target.value);

    if (index === -1) {

      setviewsAll([...viewsAll, event.target.value]);
    } else {

      setviewsAll(viewsAll.filter((item) => item !== event.target.value));

    }

  }

  const handlerTableChange = (event: React.ChangeEvent<HTMLInputElement>) => {

    const index = tableAll.indexOf(event.target.value);

    if (index === -1) {

      setTableAll([...tableAll, event.target.value]);
    } else {

      setTableAll(tableAll.filter((item) => item !== event.target.value));

    }

  }

  const handlerColumsChange = (event: React.ChangeEvent<HTMLInputElement>) => {

    const index = columAll.indexOf(event.target.value);

    if (index === -1) {

      setColumAll([...columAll, event.target.value]);

    } else {

      setColumAll(columAll.filter((item) => item !== event.target.value));

    }

  }



  const ScriptAll = async () => {

    for (var i = scriptShow12.length; i > 0; i--) {
 
      scriptShow12.pop();
      
     }

    const response = await axios.post('https://localhost:7203/api/Smo/CreateScriptAll', {

      baseEnvarimantName: envarimant,

      databaseName: databaseName,

      tableName: [...tableAll],

      colLumnsName: [...columAll],

      indexName: [...indexAll],

      Views: [...viewsAll],

      StoredProcedures: [...storedProceduresAll],

      NickName: nickName,

      Password: password,

    });

    setScriptShow12(response.data);

   


  }
  const CreateScript = async () => {
    
    if (scriptShow12[0] !== "") {

      await axios.post('https://localhost:7203/api/Smo/CreateScriptMain12', {

        scripsts: [...scriptShow12],

        baseEnvarimantName: envarimant,

        databaseName: databaseName,





        NickName: nickName,

        Password: password,

      });

      newPage();
    }

  }
  return (

    <div>



      <label style={{ marginLeft: "550px" }}>NickName : </label>

      <input onChange={(e) => setNickName(e.target.value)} style={{ height: '5px', padding: '12px 20px', margin: '8px 0', display: 'inline-block', border: '1px solid #ccc', borderRadius: '4px', boxSizing: 'border-box' }} ></input>

      <label style={{ marginLeft: "20px" }}>Pasword : </label>

      <input onChange={(e) => setPassword(e.target.value)} style={{ height: '5px', padding: '12px 20px', margin: '8px 0', display: 'inline-block', border: '1px solid #ccc', borderRadius: '4px', boxSizing: 'border-box' }} ></input>

      <Button onClick={GetAplicationName}>Giriş Yap</Button>

      <InputLabel style={{ fontSize: '20px', marginTop: '50px', marginLeft: '300px' }}>Ortamı Seç :</InputLabel>



      <FormControl style={{ marginTop: '10px', width: '400px', marginLeft: '300px' }} variant="standard" sx={{ m: 1, minWidth: 120 }}>

        <Select
          labelId="demo-simple-select-standard-label"
          id="demo-simple-select-standard"
          value={envarimant || ''}


          onChange={handleChange}
        >

          <InputLabel>Ortamı Seç</InputLabel>

          {envarimantLİst && envarimantLİst?.map((envarimantList: any, index: number) => (



            <MenuItem key={index + 100} value={envarimantList}>{envarimantList}</MenuItem>

          ))}
        </Select>

      </FormControl>


      <InputLabel style={{ fontSize: '20px', marginTop: '-85px', marginLeft: '1000px' }}>DataBase Seç : </InputLabel>

      <div onClick={DataBaseNameList}>
        <FormControl style={{ marginTop: '15px', width: '400px', marginLeft: '1000px' }} variant="standard" sx={{ m: 1, minWidth: 120 }}>

          <Select
            labelId="demo-simple-select-standard-label"
            id="demo-simple-select-standard"
            value={databaseName}


            onChange={handleChange2}
          >



            {dataBaseNameList && dataBaseNameList?.map((dataBaseNameList: any, index: number) => (



              <MenuItem key={index + 10} value={dataBaseNameList}>{dataBaseNameList}</MenuItem>

            ))}
          </Select>

        </FormControl>
      </div>



      <Button onClick={DataBaseConturol} style={{ marginTop: '-100px', width: '200px', height: '60px', marginLeft: '1450px' }} variant="contained">Show the differences</Button >
      <Modal
        open={open}
      >

        <Box sx={style}>
          <Stack spacing={2}

            justifyContent="center"

            alignItems="stretch"
          >


            <label>Database Diger Serverda Yok OluşTurmak İstiyormusun ?</label>

            <Button onClick={CreateDatabase} variant="outlined">DataBase'i Oluştur</Button>

            <Button onClick={handleClose} variant="outlined" color="error">
              Kapat
            </Button>

          </Stack>
        </Box>

      </Modal>

      <TreeView
        aria-label="file system navigator"
        defaultCollapseIcon={<ExpandMoreIcon />}
        defaultExpandIcon={<ChevronRightIcon />}
        sx={{ height: '650px', marginLeft: '300px', flexGrow: 1, maxWidth: 400, overflowY: 'auto', border: 'solid 1px' }}
      >
        <TreeItem nodeId="1" label="Schemas">

          {schemaList?.map((schemaListIn: any, index: number) => (


            <TreeItem key={index} nodeId={schemaListIn.nodeId.toString()} label={schemaListIn.schemasName}>

              {allTable && allTable?.map((allTableIn: any, index1: number) => (

                schemaListIn.schemasName === allTableIn.schema && (

                  <div> <TreeItem key={index1 + 100} nodeId={(index1 + 10000).toString()} label={<FormControlLabel value={allTableIn.tableName} control={<Checkbox onChange={handlerTableChange}></Checkbox>} label={allTableIn.tableName}></FormControlLabel>}>

                    <TreeItem key={index1 + 1243} nodeId={(index1 * 11).toString()} label='Columns'>
                      {
                        allTableIn.columns && allTableIn.columns?.map((columnsIn: any, index2: number) => (
                          columnsIn.length !== 0 && (
                            <TreeItem key={index2 + 10000} nodeId={(index2 + 1000000).toString()} label={<FormControlLabel value={columnsIn} control={<Checkbox onChange={handlerColumsChange}></Checkbox>} label={columnsIn}></FormControlLabel>}></TreeItem>

                          )
                        ))}

                    </TreeItem>

                    <TreeItem key={index1 + 1456} nodeId={(index1 + 22).toString()} label='Indexes'>
                      {
                        allTableIn.indexes && allTableIn.indexes.map((indexesIn: string, index3: number) => (
                          indexesIn.length !== 0 && (
                            <TreeItem key={index3 + 10000000000} nodeId={(index3 + 1000000000000).toString()} label={<FormControlLabel value={indexesIn} control={<Checkbox onChange={handlerIndexChange}></Checkbox>} label={indexesIn}></FormControlLabel>} />
                          )
                        ))}

                    </TreeItem>
                  </TreeItem>


                  </div>




                )

              ))}





              <TreeItem key={index + 123123} nodeId={(index + 121221321).toString()} label='StoredProcedures'>
                {
                  storedProcedures.map((storedProceduresIn: any, index3: number) => (

                    schemaListIn.schemasName === storedProceduresIn.storedProceduresSchema && (

                      <TreeItem key={index3 + 1000000000000} nodeId={(index3 + 100000000000000).toString()} label={<FormControlLabel value={storedProceduresIn.storedProceduresName} control={<Checkbox onChange={handlerProceduresNameChange}></Checkbox>} label={storedProceduresIn.storedProceduresName}></FormControlLabel>} />
                    )
                  ))}

              </TreeItem>



              <TreeItem key={index + 12312300} nodeId={(index + 12122132100).toString()} label='Views'>
                {
                  views.map((viewsIn: any, index3: number) => (

                    schemaListIn.schemasName === viewsIn.viewsSchema && (

                      <TreeItem key={index3 + 100000000000000} nodeId={(index3 + 10000000000000000).toString()} label={<FormControlLabel value={viewsIn.viewsName} control={<Checkbox onChange={handlerViewsNameChange}></Checkbox>} label={viewsIn.viewsName}></FormControlLabel>} />
                    )
                  ))}

              </TreeItem>




            </TreeItem>


          ))}



        </TreeItem>

      </TreeView>


      <Button style={{ marginTop: '-800px', width: '200px', height: '60px', marginLeft: '750px' }} variant="outlined" onClick={ScriptAll}> Script Çıkar </Button>

      <Box
        sx={{
          display: 'flex',
          flexWrap: 'wrap',
          '& > :not(style)': {
            m: 1,
            width: '650px',
            height: '650px',
            marginLeft: '1000px',
            marginTop: '-670px'
          },
        }}
      >

        <Paper style={{ overflow: 'scroll' }} elevation={7} >


          {scriptShow12 && scriptShow12?.map((scriptShowList: any, index: number) => (

            <ListItem key={index} disablePadding>

              <ListItemText primary={scriptShowList} />

            </ListItem>



          ))}

        </Paper>

      </Box>

      <Button style={{ marginTop: '-1000px', width: '200px', height: '60px', marginLeft: '750px' }} onClick={CreateScript} variant="outlined" >Sql'e Gönder</Button>

    </div>

  );
}

export default ManagmentToolMainManü;