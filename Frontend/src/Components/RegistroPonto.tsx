import React, { useState, useEffect } from 'react';
import axios from 'axios';

function RegistroPonto() {
  const [funcionarioId, setFuncionarioId] = useState(1); // Substitua com o ID real do funcionário
  const [registroPontos, setRegistroPontos] = useState({});
  const [error, setError] = useState(null);

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
      <h2>Registro de Ponto</h2>

      {error && <p style={{ color: 'red' }}>{error}</p>}

      <div>
        <button onClick={() => registrarPonto('entrada')}>Registrar Entrada</button>
        <button onClick={() => registrarPonto('almoco')}>Registrar Almoço</button>
        <button onClick={() => registrarPonto('retorno')}>Registrar Retorno do Almoço</button>
        <button onClick={() => registrarPonto('saida')}>Registrar Saída</button>
      </div>
      
      <h3>Registros de Ponto</h3>
      <ul>
        {['entrada', 'almoco', 'retorno', 'saida'].map((tipo) => (
          <li key={tipo}>
            {tipo.charAt(0).toUpperCase() + tipo.slice(1)}: 
            {registroPontos[tipo] ? new Date(registroPontos[tipo]).toLocaleString() : 'Não registrado'}
            <button onClick={() => excluirPonto(tipo)}>Excluir</button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default RegistroPonto;
