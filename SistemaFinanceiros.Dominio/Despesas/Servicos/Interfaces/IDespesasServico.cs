using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.Dominio.Despesas.Entidades;
using SistemaFinanceiros.Dominio.Despesas.Enumeradores;
using SistemaFinanceiros.Dominio.Despesas.Servicos.Comandos;

namespace SistemaFinanceiros.Dominio.Despesas.Servicos.Interfaces
{
    public interface IDespesasServico
    {
        Task<Despesa> ValidarAsync(int id);
        Task<Despesa> InserirAsync(DespesaComando comando);
        Task<Despesa> EditarAsync(int id, DespesaComando comando);
        Task<object> CarregaGraficos(string emailUsuario);
        Task<Despesa> InstanciarAsync(DespesaComando comando);
        
    }
}