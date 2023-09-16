using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Entidades;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos.Comandos;

namespace SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos.Interfaces
{
    public interface ISistemaFinanceirosServico
    {
        Task<SistemaFinanceiro> ValidarAsync(int id);
        Task<SistemaFinanceiro> InserirAsync(SistemaFinanceiroComando comando);
        Task<SistemaFinanceiro> EditarAsync(int id, SistemaFinanceiroComando comando);
    }
}