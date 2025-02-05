import React, { useState } from "react";
import "../Styles/Login.css";
import RelogioBanner from "../assets/relogio.jpg";
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import RegistroPonto from "./RegistroPonto";
import { useNavigate } from "react-router-dom"; //
import { IoMdTimer } from "react-icons/io";



function Login() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const navigate = useNavigate();

  const handleLogin = async () => {
    const response = await fetch("https://localhost:5001/api/Authentication/Login", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        username: username,
        passwordHash: password, // Deve ser o mesmo campo do LoginViewModel no backend
      }),
    });

    if (response.ok) {
      const data = await response.json();
      localStorage.setItem("token", data.token); // Salva o token no localStorage
      alert("Login bem-sucedido!");
      navigate("/Registro");
    } else {
      const message = await response.text();
      setError(message);
    }
  };

  return (
    <div>
      <div className="BackgroundLogin">
        <div className="BoxLogin">
          <div className="InputsELogin">
            <h2>HORA CERTA <IoMdTimer />
            </h2>
            <p>Bem-vindo usuário! </p>
            <div className="InputsBox">
              <input
                type="text"
                placeholder="Usuário"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
              <input
                type="password"
                placeholder="Senha"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
              <button onClick={handleLogin}>Entrar</button>
              {error && <p style={{ color: "red" }}>{error}</p>}
            </div>
          </div>

          <div>
            <img src={RelogioBanner} alt="" width="52%" />
          </div>
        </div>
      </div>
    </div>
  );
}

export default Login;
