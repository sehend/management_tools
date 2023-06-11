import CssBaseline from '@mui/material/CssBaseline';
import Divider from '@mui/material/Divider';
import Drawer from '@mui/material/Drawer';

import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';

import Toolbar from '@mui/material/Toolbar';

import { Link } from 'react-router-dom';




const drawerWidth = 240;

function DrawerBlock() {

  return (
    <div>


      <Drawer
        sx={{
          width: drawerWidth,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: drawerWidth,
            boxSizing: 'border-box',
          },
        }}
        variant="permanent"
        anchor="left"
      >
        <div>


          <Toolbar />
          <Divider />
          <List>

            <ListItem key={1} >
              <ListItemButton>
                <ListItemIcon>
             <Link to="/AplicationMainManü" >Aplication</Link>

                </ListItemIcon>
                <ListItemText />
              </ListItemButton>
            </ListItem>
            <Divider />
            <CssBaseline />
            <ListItem key={2} >
              <ListItemButton>
                <ListItemIcon>
                <Link to="/ManagmentToolMainManü" >ManagmentToolMainManü</Link>
                </ListItemIcon>
                <ListItemText />
              </ListItemButton>
            </ListItem>

          </List>
          <Divider />

        </div>
      </Drawer>

    </div>

  );
}
export default DrawerBlock