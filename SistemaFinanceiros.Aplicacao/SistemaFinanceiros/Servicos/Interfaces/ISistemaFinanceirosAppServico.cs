using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.DataTransfer.SistemaFinanceiros.Request;
using SistemaFinanceiros.DataTransfer.SistemaFinanceiros.Response;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.Aplicacao.SistemaFinanceiros.Servicos.Interfaces
{
    public interface ISistemaFinanceirosAppServico
    {
        Task<PaginacaoConsulta<SistemaFinanceiroResponse>> ListarAsync(SistemaFinanceiroListarRequest request);
        Task<SistemaFinanceiroResponse> RecuperarAsync(int id);
        Task<SistemaFinanceiroResponse> InserirAsync(SistemaFinanceiroInserirRequest sistemaFinanceiroInserirRequest);
        Task<SistemaFinanceiroResponse> EditarAsync(int id, SistemaFinanceiroEditarRequest sistemaFinanceiroEditarRequest);
        Task ExcluirAsync(int id);
    }
}