import React from 'react';
import Login from './Components/Login.tsx';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import RegistroPonto from "./Components/RegistroPonto.tsx"


function App() {
  return (
      <Router>
        <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/Registro" element={<RegistroPonto />} />
        </Routes>
      </Router>
  );
}

export default App;
