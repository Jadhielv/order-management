import React from 'react';
import { CssBaseline, ThemeProvider, createTheme } from '@mui/material';
import OrderList from './components/OrderList';

const theme = createTheme();

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <OrderList />
    </ThemeProvider>
  );
}

export default App;