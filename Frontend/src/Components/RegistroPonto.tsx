import React, { useState, useEffect } from 'react';
import axios from 'axios';
import {jwtDecode} from 'jwt-decode';
import '../Styles/Registroponto.css';
import { ImEnter } from "react-icons/im";
import { GiExitDoor } from "react-icons/gi";
import { IoFastFoodSharp } from "react-icons/io5";
import { FaPersonWalkingArrowLoopLeft } from "react-icons/fa6";
import { IoMdTimer } from "react-icons/io";
import Nav from './Nav.tsx';
import Login from './Login.tsx';




function RegistroPonto() {
  const [funcionarioId, setFuncionarioId] = useState(2); // Substitua com o ID real do funcionário
  const [registroPontos, setRegistroPontos] = useState({});
  const [error, setError] = useState(null);

  interface JwtPayload {
    id: number; // Ou string, dependendo do tipo do ID
  }

  // Função para decodificar o token e obter o ID do funcionário
  function obterFuncionarioId(): number | null {
    try {
      const token = localStorage.getItem('token');
      if (!token) throw new Error('Token de autenticação não encontrado');
  
      // Decodifique o token e informe o tipo do payload
      const decoded = jwtDecode<JwtPayload>(token);
  
      return decoded.id; // Certifique-se de que o ID está presente no token
    } catch (err) {
      console.error('Erro ao decodificar o token:', err);
      return null;
    }
  }

  // Função para registrar pontos
  const registrarPonto = async (tipo) => {
    try {
      // Obtém o token do localStorage
      const token = localStorage.getItem('token');
      
      // Se não houver token, exibe um erro
      if (!token) {
        setError('Token de autenticação não encontrado');
        return;
      }

      // Inclui o token no cabeçalho da requisição
      const response = await axios.post(`https://localhost:5001/api/RegistroPonto/${funcionarioId}/${tipo}`, {}, {
        headers: {
          "Content-Type": "application/json",
          'Authorization': `Bearer ${token}`,
        }
      });

      setRegistroPontos(response.data);
      alert(`${tipo} registrado com sucesso!`);
    } catch (err) {
      setError(`Erro ao registrar ponto: ${err.response?.data || err.message}`);
    }
  };

  // Função para excluir pontos
  const excluirPonto = async (tipo) => {
    try {
      const token = localStorage.getItem('token');
      
      if (!token) {
        setError('Token de autenticação não encontrado');
        return;
      }

      await axios.delete(`https://localhost:5001/api/RegistroPonto/${funcionarioId}/${tipo}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
        }
      });

      setRegistroPontos((prevState) => ({
        ...prevState,
        [tipo]: null,
      }));
      alert(`${tipo} excluído com sucesso!`);
    } catch (err) {
      setError(`Erro ao excluir ponto: ${err.response?.data || err.message}`);
    }
  };

  // Função para buscar os pontos registrados do funcionário
  const fetchPontos = async () => {
    try {
      const token = localStorage.getItem('token');
      
      if (!token) {
        setError('Token de autenticação não encontrado');
        return;
      }

      const response = await axios.get(`https://localhost:5001/api/RegistroPonto/${funcionarioId}`, {
        headers: {
          'Authorization': `Bearer ${token}`,
        }
      });

      setRegistroPontos(response.data);
    } catch (err) {
      setError(`Erro ao buscar registros: ${err.response?.data || err.message}`);
    }
  };

  // Carrega os pontos ao iniciar
  useEffect(() => {
    fetchPontos();
  }, [funcionarioId]);

  return (
    <div>
      <Nav />
      <div className="RegistroBox">
        {error && <p style={{ color: 'black' }} className="erro">{error}</p>}
        <div className="buttonBox">
          <div>
            <button className="entrada" onClick={() => registrarPonto('entrada')}>Registrar Entrada <ImEnter size={"100px"}/>
            </button>
            <button className="almoco" onClick={() => registrarPonto('almoco')}>Registrar Almoço
            <IoFastFoodSharp size={"100px"}/>
            </button>
          </div>
          <div>
            <button className="retorno" onClick={() => registrarPonto('retorno')}>Registrar Retorno do Almoço <FaPersonWalkingArrowLoopLeft size={"100px"}/>
            </button>
            <button className="saida" onClick={() => registrarPonto('saida')}>Registrar Saída  <GiExitDoor size={"100px"}/></button>
          </div>
        </div>
      </div>
    </div>
  );
}

export default RegistroPonto;