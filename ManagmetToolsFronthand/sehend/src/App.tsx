import React from 'react';



import DrawerBlock from './Component/DrawerBlock';

import axios from 'axios';

import Stack from '@mui/material/Stack';


import Button from '@mui/material/Button';
import { Routes, Route } from 'react-router-dom';
import AplicationMainManü from './AplicationMainManü';
import ManagmentToolMainManü from './ManagmentToolMainManü';




function App() {

  const [nickName, setNickName] = React.useState<string>("");

  const [password, setPassword] = React.useState<string>("");

  const [resualt, setResualt] = React.useState<string>("");


  const Conturol = async () => {


    const response = await axios.post('https://localhost:7203/api/UserNickAndPassword/UserNickAndPasswordConturol', {


      NickName: nickName,

      Password: password,


    });

    setResualt(response.data);

    if (resualt === "Kulanıcı Adı Başka Kulanıcı Tarafından Kulanılmaktadır....") {

    } else {
      CreateUserNickAndPassword();
    }



  }

  const CreateUserNickAndPassword = async () => {


    await axios.post('https://localhost:7203/api/UserNickAndPassword/CreateUserNickAndPassword', {


      nickName: nickName,

      password: password,


    });



  }

  return (


    <div>
      <Routes>
        <Route path="/AplicationMainManü" element={<AplicationMainManü></AplicationMainManü>}></Route>

        <Route path="/ManagmentToolMainManü" element={<ManagmentToolMainManü></ManagmentToolMainManü>}></Route>


      </Routes>,


      <DrawerBlock></DrawerBlock>

      <Stack
        direction="row-reverse"
        justifyContent="center"
        alignItems="stretch"
        marginTop='20px'
        spacing={1}
      >
        <label style={{ marginTop: "40px" }}>Nick Name</label>

      </Stack>

      <Stack
        direction="row-reverse"
        justifyContent="center"
        alignItems="stretch"

        marginTop='20px'

        spacing={1}
      >
        <input onChange={(e) => setNickName(e.target.value)} style={{ height: '5px', padding: '12px 20px', marginTop: "100", display: 'inline-block', border: '1px solid #ccc', borderRadius: '4px', boxSizing: 'border-box' }} ></input>
      </Stack>

      <Stack
        direction="row-reverse"
        justifyContent="center"
        alignItems="stretch"

        marginTop="-10px"

        spacing={1}
      >
        <label style={{ marginTop: "40px" }}>Password</label>

      </Stack>

      <Stack
        direction="row-reverse"
        justifyContent="center"
        alignItems="stretch"

        marginTop="10px"

        spacing={1}
      >
        <input onChange={(e) => setPassword(e.target.value)} style={{ height: '5px', padding: '12px 20px', margin: '8px 0', display: 'inline-block', border: '1px solid #ccc', borderRadius: '4px', boxSizing: 'border-box' }} ></input>
      </Stack>
      <Stack
        direction="row-reverse"
        justifyContent="center"
        alignItems="stretch"

        marginTop="10px"

        spacing={1}
      >
        <Button style={{ width: '200px', height: '60px' }} onClick={Conturol}  >Kayıt Ol</Button>

      </Stack>
      <Stack
        direction="row-reverse"
        justifyContent="center"
        alignItems="stretch"

        marginTop="10px"

        spacing={1}
      >
        <label style={{ marginLeft: '40px' }} >{resualt}</label>

      </Stack>

    </div>
  );
}

export default App;
