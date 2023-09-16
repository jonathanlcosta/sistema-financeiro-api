using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.Dominio.Execoes;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Entidades;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Repositorios;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos.Comandos;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos.Interfaces;

namespace SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos
{
    public class SistemaFinanceirosServico : ISistemaFinanceirosServico
    {
        private readonly ISistemaFinanceirosRepositorio sistemaFinanceirosRepositorio;
        public SistemaFinanceirosServico(ISistemaFinanceirosRepositorio sistemaFinanceirosRepositorio)
        {
            this.sistemaFinanceirosRepositorio = sistemaFinanceirosRepositorio;
        }
        public async Task<SistemaFinanceiro> EditarAsync(int id, SistemaFinanceiroComando comando)
        {
            SistemaFinanceiro sistemaFinanceiro = await ValidarAsync(id);
            sistemaFinanceiro.SetNome(comando.Nome);
            await sistemaFinanceirosRepositorio.EditarAsync(sistemaFinanceiro);
            return sistemaFinanceiro;
        }

        public async Task<SistemaFinanceiro> InserirAsync(SistemaFinanceiroComando comando)
        {
        SistemaFinanceiro sistemaFinanceiro = Instanciar(comando);
        await sistemaFinanceirosRepositorio.InserirAsync(sistemaFinanceiro);
        return sistemaFinanceiro;
        }

        public SistemaFinanceiro Instanciar(SistemaFinanceiroComando comando)
        {
             SistemaFinanceiro sistemaFinanceiro = new(comando.Nome);
             return sistemaFinanceiro;
        }

        public async Task<SistemaFinanceiro> ValidarAsync(int id)
        {
            var sistemaFinanceiro = await this.sistemaFinanceirosRepositorio.RecuperarAsync(id);
            if(sistemaFinanceiro is null)
            throw new RegraDeNegocioExcecao("Sistema Financeiro não encontrado");

            return sistemaFinanceiro;
        }
    }
}