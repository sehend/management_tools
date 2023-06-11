import './App.css';
import DrawerBlock from './Component/DrawerBlock';

import Stack from '@mui/material/Stack';

import axios from 'axios';
import React from 'react';

import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Modal from '@mui/material/Modal';

import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Paper from '@mui/material/Paper';



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

const style2 = {
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

function AplicationMainManü() {

  const [open, setOpen] = React.useState(false);

  const handleOpen = () => setOpen(true);

  const handleClose = () => { setOpen(false); }

  const aplicationConturol = React.useRef<string>('');

  const [envarimantName, setEnvarimantName] = React.useState<any>([]);

  const [connectionString, setConnectionString] = React.useState<any>([]);

  const [nickName, setNickName] = React.useState<any>([]);

  const [password, setPassword] = React.useState<any>([]);

  const [aplicationListManu2, setAplicationListManu2] = React.useState<any>([]);

  const [open2, setOpen2] = React.useState(false);

  const idValue2 = React.useRef<number>();

  const lineValue2 = React.useRef<number>();

  const aplicationConturol2 = React.useRef<string>('');

  const [envarimantName2, setEnvarimantName2] = React.useState<any>([]);

  const [connectionString2, setConnectionString2] = React.useState<any>([]);

  const handleOpen2 = (id: number, line: number) => { setOpen2(true); idValue2.current = id; lineValue2.current = line; };

  const handleClose2 = () => { setOpen2(false); }

  const newPage2 = () => {


    window.location.reload();

  }

  const newPage = () => {


    window.location.reload();

  }
  const handlerEnvarimantName = (event: React.ChangeEvent<HTMLInputElement>) => {

    setEnvarimantName(event.target.value);

  }

  const handlerConnectionString = (event: React.ChangeEvent<HTMLInputElement>) => {

    setConnectionString(event.target.value);

  }

  const AplicationConturol = async () => {



    const response = await axios.post('https://localhost:7203/api/Aplication/AplicationConnectionStringConturol', {

      ConnectionString: connectionString


    });

    const data = response.data;

    aplicationConturol.current = data;

    if (aplicationConturol.current === "Server Parametresi ConnectionString'te Bulunmalıdır.....") {

    }
    if (aplicationConturol.current === "Database Parametresi ConnectionString De Bulunamaz....") {

    }
    if (aplicationConturol.current === "Catalog Parametresi ConnectionString De Bulunamaz....") {

    }
    if (aplicationConturol.current === "Başarılı....") {


      AplicationAdd();


    }



  }
  const AplicationAdd = async () => {





    if (connectionString != null && envarimantName != null) {

      await axios.post('https://localhost:7203/api/Aplication', {

        connectionString: connectionString,

        nickName: nickName,

        password: password,

        envarimantName: envarimantName

      });

      newPage();
      
    }




  }

  const handlerEnvarimantName2 = (event: React.ChangeEvent<HTMLInputElement>) => {

    setEnvarimantName2(event.target.value);

  }

  const handlerConnectionString2 = (event: React.ChangeEvent<HTMLInputElement>) => {

    setConnectionString2(event.target.value);

  }

  const AplicationConturol2 = async () => {

    debugger;

    const response = await axios.post('https://localhost:7203/api/Aplication/AplicationConnectionStringConturol', {

      ConnectionString: connectionString2


    });

    const data = response.data;

    aplicationConturol2.current = data;

    if (aplicationConturol2.current === "Server Parametresi ConnectionString'te Bulunmalıdır.....") {

    }
    if (aplicationConturol2.current === "Database Parametresi ConnectionString De Bulunamaz....") {

    }
    if (aplicationConturol2.current === "Catalog Parametresi ConnectionString De Bulunamaz....") {

    }
    if (aplicationConturol2.current === "Başarılı....") {


      handlerUpdate2();


    }

    console.log(aplicationConturol2);

  }


  const handlerDelete2 = async (id: number) => {

    await axios.delete(`https://localhost:7203/api/Aplication/${id}`);

    const aplicationListManuClear = aplicationListManu2.filter((aplicationListManu: any) => {

      return aplicationListManu.id !== id;

    });

    setAplicationListManu2(aplicationListManuClear);

  }

  const handlerUpdate2 = async () => {

    await axios.put(`https://localhost:7203/api/Aplication/`, {

      id: idValue2.current,

      line: lineValue2.current,

      envarimantName: envarimantName2,

      nickName: nickName,

      password: password,

      connectionString: connectionString2

    });

    newPage2();

  }



  const AplicationListManu2 = async () => {

    const response = await axios.post('https://localhost:7203/api/Aplication/GetAll', {

      nickName: nickName,

      password: password

    });

    setAplicationListManu2(response.data);

    console.log(response.data);


    debugger;
  }





  return (

    <div>
      <Stack
        direction="row-reverse"
        alignItems="stretch"
        spacing={2}
      >
        <DrawerBlock></DrawerBlock>
        <Stack spacing={3}>


          <div style={{ marginTop: `62px` }} >
            <Button  onClick={handleOpen}>Ekle</Button>


            <Modal
              open={open}
            >

              <Box sx={style}>
                <Stack spacing={2}

                  justifyContent="center"

                  alignItems="stretch"
                >
                  <TextField id="outlined-basic" onChange={handlerEnvarimantName} label="EnvarimantName" variant="outlined" />

                  <TextField id="outlined-basic" onChange={handlerConnectionString} label="ConnectionString" variant="outlined" />

                  <label>{aplicationConturol.current}</label>

                  <Button onClick={AplicationConturol} variant="outlined">Ekle</Button>

                  <Button onClick={handleClose} variant="outlined" color="error">
                    Kapat
                  </Button>

                </Stack>
              </Box>

            </Modal>


            <label style={{marginLeft:"550px"}}>NickName : </label>

            <input onChange={(e) => setNickName(e.target.value)} style={{ height: '5px', padding: '12px 20px', margin: '8px 0', display: 'inline-block', border: '1px solid #ccc', borderRadius: '4px', boxSizing: 'border-box' }} ></input>

            <label style={{marginLeft:"20px"}}>Pasword : </label>

            <input onChange={(e) => setPassword(e.target.value)} style={{ height: '5px', padding: '12px 20px', margin: '8px 0', display: 'inline-block', border: '1px solid #ccc', borderRadius: '4px', boxSizing: 'border-box' }} ></input>

            <Button onClick={AplicationListManu2}>Giriş Yap</Button>


          </div>

          <Stack
            direction="row-reverse"
            justifyContent="center"
            alignItems="stretch"



            spacing={1}
          >



            <TableContainer  component={Paper}>
              <Table sx={{ minWidth: 1250 }} aria-label="simple table">
                <TableHead>
                  <TableRow>
                    <TableCell align="left">EnvarimantName</TableCell>
                    <TableCell align="left">Line</TableCell>
                    <TableCell align="left">ConnectionString</TableCell>
                    <TableCell align="left">Delete</TableCell>
                    <TableCell align="left">Update</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>


                  {aplicationListManu2 && aplicationListManu2?.map((row: any, index: number) => (
                    <TableRow
                      key={index}
                      sx={{ '&:last-child td, &:last-child th': { border: 0 } }}
                    >
                      <TableCell align="left">{row.envarimantName}</TableCell>
                      <TableCell align="left">{row.line}</TableCell>
                      <TableCell style={{ height: "70px", whiteSpace: "nowrap", textOverflow: "ellipsis", width: "800px", display: "block", overflow: "hidden" }} align="left">{row.connectionString}</TableCell>
                      <TableCell align="left"><Button onClick={() => handlerDelete2(row.id)} variant="outlined" color="error">Delete</Button></TableCell>
                      <TableCell align="left"><Button onClick={() => handleOpen2(row.id, row.line)} variant="outlined">Update</Button></TableCell>
                      <Modal
                        open={open2}
                      >
                        <Box sx={style2}>
                          <Stack spacing={2}

                            justifyContent="center"

                            alignItems="stretch"
                          >
                            <TextField id="outlined-basic" onChange={handlerEnvarimantName2} label="EnvarimantName" variant="outlined" />

                            <TextField id="outlined-basic" onChange={handlerConnectionString2} label="ConnectionString" variant="outlined" />

                            <label>{aplicationConturol2.current}</label>

                            <Button onClick={AplicationConturol2} variant="outlined">Update</Button>

                            <Button onClick={handleClose2} variant="outlined" color="error">
                              Kapat
                            </Button>

                          </Stack>
                        </Box>

                      </Modal>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>

          </Stack>

        </Stack>
      </Stack>



    </div>

  );
}

export default AplicationMainManü;