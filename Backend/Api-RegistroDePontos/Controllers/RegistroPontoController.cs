using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistroDePontosApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace RegistroDePontosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroPontoController : ControllerBase
    {
        private readonly RegistroContext _context;

        public RegistroPontoController(RegistroContext context)
        {
            _context = context;
        }

        // GET: api/RegistroPonto
        [HttpGet,Authorize]
        public async Task<ActionResult<IEnumerable<RegistroPonto>>> GetRegistroPonto()
        {
            return await _context.RegistroPonto.ToListAsync();
        }

        // GET: api/RegistroPonto/5
        [HttpGet("{FuncionarioId}"),Authorize]
        public async Task<ActionResult<RegistroPonto>> GetRegistroPonto(int FuncionarioId)
        {
            var registroPonto = await _context.RegistroPonto .FirstOrDefaultAsync(r => r.FuncionarioId == FuncionarioId);

            if (registroPonto == null)
            {
                return NotFound();
            }

            return registroPonto;
        }



        // PUT: api/RegistroPonto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}"),Authorize]
        public async Task<IActionResult> PutRegistroPonto(int id, RegistroPonto registroPonto)
        {
            if (id != registroPonto.Id)
            {
                return BadRequest();
            }

            _context.Entry(registroPonto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegistroPontoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }



        // POST: api/RegistroPonto
        //Removi anteriormente o post que enviava todos os registros de uma vez só, agora, ele vai registrar de acordo com a rota selecionada
        [HttpPost,Authorize]
        public async Task<ActionResult<RegistroPonto>> PostRegistroPonto()
        {
            var registroPonto = new RegistroPonto();
            _context.RegistroPonto.Add(registroPonto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRegistroPonto), new { id = registroPonto.Id }, registroPonto);
        }
        [HttpPost("{funcionarioId}/entrada"),Authorize]
        public async Task<IActionResult> RegistrarEntrada(int funcionarioId)
        {
                var token = Request.Headers["Authorization"].ToString();

                // Verifique se o token foi enviado corretamente
                if (string.IsNullOrEmpty(token) || !token.StartsWith("Bearer "))
                {
                    return Unauthorized("Token não fornecido.");
                }
                
            var dataAtual = DateTime.Today;
            // Verificar se já existe um registro de ponto para o funcionário no dia atual
            var registroPonto = await _context.RegistroPonto.FirstOrDefaultAsync(r => r.FuncionarioId == funcionarioId && r.Data == dataAtual);

            if (registroPonto == null)
            {
                // Criar novo registro de ponto para o dia atual
                registroPonto = new RegistroPonto
                {
                    FuncionarioId = funcionarioId,
                    Data = dataAtual,
                    PontoDeEntrada = DateTime.Now
                };

                _context.RegistroPonto.Add(registroPonto);
                await _context.SaveChangesAsync();
                return Ok(registroPonto);
            }
            else if (registroPonto.PontoDeEntrada == null)
            {
                // Atualizar ponto de entrada caso ele ainda não tenha sido registrado
                registroPonto.PontoDeEntrada = DateTime.Now;
                await _context.SaveChangesAsync();
                return Ok(registroPonto);
            }
            else
            {
                // Retornar erro caso o ponto de entrada já tenha sido registrado
                return BadRequest("Ponto de entrada já registrado para hoje.");
            }
        }
        [HttpPost("{id}/almoco"),Authorize]
        public async Task<IActionResult> RegistroAlmoço(int id)
        {
            var dataAtual = DateTime.Today;
            
            var registroPonto = await _context.RegistroPonto.FirstOrDefaultAsync(r => r.FuncionarioId == id && r.Data == dataAtual);

            if (registroPonto == null)
            {
                //refatorar depois
                registroPonto = new RegistroPonto
                {
                    FuncionarioId = id,
                    Data = dataAtual,
                    PontoDeAlmoço = DateTime.Now
                };

                _context.RegistroPonto.Add(registroPonto);
                await _context.SaveChangesAsync();
                return Ok(registroPonto);
            }
            else if (registroPonto.PontoDeAlmoço == null)
            {
                
                registroPonto.PontoDeAlmoço = DateTime.Now;
                await _context.SaveChangesAsync();
                return Ok(registroPonto);
            }
            else
            {
                
                return BadRequest("Ponto de Almoço já registrado para hoje.");
            }
        }
        [HttpPost("{id}/retorno"),Authorize]
        public async Task<IActionResult> RegistroRetorno(int id)
        {
            var dataAtual = DateTime.Today;
            // Verificar se já existe um registro de ponto para o funcionário no dia atual
            var registroPonto = await _context.RegistroPonto.FirstOrDefaultAsync(r => r.FuncionarioId == id && r.Data == dataAtual);

            if (registroPonto == null)
            {
                // Criar novo registro de ponto para o dia atual
                registroPonto = new RegistroPonto
                {
                    FuncionarioId = id,
                    Data = dataAtual,
                    PontoDeVoltaAlmoço = DateTime.Now
                };

                _context.RegistroPonto.Add(registroPonto);
                await _context.SaveChangesAsync();
                return Ok(registroPonto);
            }
            else if (registroPonto.PontoDeVoltaAlmoço == null)
            {
                // Atualizar ponto de entrada caso ele ainda não tenha sido registrado
                registroPonto.PontoDeVoltaAlmoço = DateTime.Now;
                await _context.SaveChangesAsync();
                return Ok(registroPonto);
            }
            else
            {
                // Retornar erro caso o ponto de entrada já tenha sido registrado
                return BadRequest("Ponto de Retorno do almoço já registrado para hoje.");
            }
        }
        [HttpPost("{id}/saida"),Authorize]
        public async Task<IActionResult> RegistroSaida(int id)
        {
            var dataAtual = DateTime.Today;
            // Verificar se já existe um registro de ponto para o funcionário no dia atual
            var registroPonto = await _context.RegistroPonto.FirstOrDefaultAsync(r => r.FuncionarioId == id && r.Data == dataAtual);

            if (registroPonto == null)
            {
                // Criar novo registro de ponto para o dia atual
                registroPonto = new RegistroPonto
                {
                    FuncionarioId = id,
                    Data = dataAtual,
                    PontoDeSaída = DateTime.Now
                };

                _context.RegistroPonto.Add(registroPonto);
                await _context.SaveChangesAsync();
                return Ok(registroPonto);
            }
            else if (registroPonto.PontoDeSaída == null)
            {
                // Atualizar ponto de entrada caso ele ainda não tenha sido registrado
                registroPonto.PontoDeSaída = DateTime.Now;
                await _context.SaveChangesAsync();
                return Ok(registroPonto);
            }
            else
            {
                // Retornar erro caso o ponto de entrada já tenha sido registrado
                return BadRequest("Ponto de Saída já registrado para hoje.");
            }
        }


        // DELETE: api/RegistroPonto/5
        [HttpDelete("{id}/entrada"),Authorize]
        public async Task<IActionResult> DeleteEntrada(int id)
        {
            var registroPonto = await _context.RegistroPonto.FindAsync(id);
            if (registroPonto == null)
            {
                return NotFound();
            }
            //esse comando vai zerar ou tornar nullo o ponto registrado anteriormente
            registroPonto.PontoDeEntrada = null;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}/almoco"),Authorize]
        public async Task<IActionResult> DeleteAlmoco(int id)
        {
            var registroPonto = await _context.RegistroPonto.FindAsync(id);
            if(registroPonto == null)
            {
                return NotFound();
            }

            registroPonto.PontoDeAlmoço = null;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}/retorno"),Authorize]
        public async Task<IActionResult> DeleteRetorno(int id)
        {
            var registroPonto = await _context.RegistroPonto.FindAsync(id);
            if(registroPonto == null)
            {
                return NotFound();
            }

            registroPonto.PontoDeVoltaAlmoço = null;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}/saida"),Authorize]
        public async Task<IActionResult> DeleteSaida(int id)
        {
            var registroPonto = await _context.RegistroPonto.FindAsync(id);
            if(registroPonto == null)
            {
                return NotFound();
            }

            registroPonto.PontoDeSaída = null;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RegistroPontoExists(int id)
        {
            return _context.RegistroPonto.Any(e => e.Id == id);
        }
    }
}
