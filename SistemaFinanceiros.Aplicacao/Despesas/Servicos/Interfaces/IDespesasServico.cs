using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.DataTransfer.Despesas.Request;
using SistemaFinanceiros.DataTransfer.Despesas.Response;
using SistemaFinanceiros.Dominio.Despesas.Repositorios.Consultas;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.Aplicacao.Despesas.Servicos.Interfaces
{
    public interface IDespesasAppServico
    {
        Task<DespesaResponse> RecuperarAsync(int id);
        Task<DespesaResponse> InserirAsync(DespesaInserirRequest despesaInserirRequest);
        Task<DespesaResponse> EditarAsync(int id, DespesaEditarRequest despesaEditarRequest);
        Task ExcluirAsync(int id); 
        Task<IList<DespesaResponse>> ListarDespesasUsuario(string emailUsuario);
        PaginacaoConsulta<DespesaConsultaResponse> ListarDespesasUsuarioNaoPagasMesesAnteriorDapper(string email);
        Task<object> CarregaGraficos(string email);
        Task<IList<DespesaResponse>> ListarDespesasUsuarioNaoPagasMesesAtras(string email);
        Task<PaginacaoConsulta<DespesaResponse>> ListarAsync(DespesaListarRequest request);
        Task<PaginacaoConsulta<DespesaResponse>> ListarDespesas(DespesaListarRequest request);
        Task<List<DespesasResumo>> ConsultaAsync();
        Task<Stream> ExportarExcel(DespesaListarRequest request);

    }
}