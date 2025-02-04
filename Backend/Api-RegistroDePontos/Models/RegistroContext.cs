using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RegistroDePontosApi.Models
{
    public class RegistroContext : DbContext
    {
        public RegistroContext(DbContextOptions<RegistroContext> options) 
            : base(options)
        { }

        public DbSet<RegistroPonto> RegistroPonto { get; set; } // Tabela de pontos de registro
        public DbSet<LoginViewModel> Usuarios {get; set; }

        // Definir as configurações das tabelas, se necessário
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuração para a tabela de RegistroPonto
            modelBuilder.Entity<RegistroPonto>()
                .ToTable("RegistroPonto");

            modelBuilder.Entity<RegistroPonto>()
                .HasKey(r => r.Id); // Definir a chave primária

            modelBuilder.Entity<RegistroPonto>()
                .Property(r => r.FuncionarioId)
                .IsRequired(); // FuncionarioId é obrigatório

            modelBuilder.Entity<RegistroPonto>()
                .Property(r => r.Data)
                .HasDefaultValueSql("GETDATE()"); // Valor padrão para a Data (data atual)

            modelBuilder.Entity<RegistroPonto>()
                .Property(r => r.PontoDeEntrada)
                .HasColumnType("datetime");

            modelBuilder.Entity<RegistroPonto>()
                .Property(r => r.PontoDeAlmoço)
                .HasColumnType("datetime");

            modelBuilder.Entity<RegistroPonto>()
                .Property(r => r.PontoDeVoltaAlmoço)
                .HasColumnType("datetime");

            modelBuilder.Entity<RegistroPonto>()
                .Property(r => r.PontoDeSaída)
                .HasColumnType("datetime");

            // Relacionamento entre RegistroPonto e Funcionario pode ser configurado, se necessário
            // Por exemplo, se você tiver a tabela de Funcionarios, poderia usar um relacionamento de chave estrangeira.

            
            // Configuração para a tabela de Usuarios
            modelBuilder.Entity<LoginViewModel>()
                .ToTable("Usuarios")
                .HasKey(u => u.Id);

            modelBuilder.Entity<LoginViewModel>()
                .Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<LoginViewModel>()
                .Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}