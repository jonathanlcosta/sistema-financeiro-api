using AutoMapper;
using SistemaFinanceiros.Aplicacao.SistemaFinanceiros.Servicos.Interfaces;
using SistemaFinanceiros.Aplicacao.Transacoes.Interfaces;
using SistemaFinanceiros.DataTransfer.SistemaFinanceiros.Request;
using SistemaFinanceiros.DataTransfer.SistemaFinanceiros.Response;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Entidades;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Repositorios;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Repositorios.Filtros;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos.Comandos;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.Aplicacao.SistemaFinanceiros.Servicos
{
    public class SistemaFinanceirosAppServico : ISistemaFinanceirosAppServico
    {
        private readonly ISistemaFinanceirosServico sistemaFinanceirosServico;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ISistemaFinanceirosRepositorio sistemaFinanceirosRepositorio;
        public SistemaFinanceirosAppServico(ISistemaFinanceirosServico sistemaFinanceirosServico,IMapper mapper,
        IUnitOfWork unitOfWork, ISistemaFinanceirosRepositorio sistemaFinanceirosRepositorio)
        {
            this.sistemaFinanceirosServico = sistemaFinanceirosServico;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.sistemaFinanceirosRepositorio = sistemaFinanceirosRepositorio;
        }
        public async Task<SistemaFinanceiroResponse> EditarAsync(int id, SistemaFinanceiroEditarRequest request)
        {
           SistemaFinanceiroComando comando = mapper.Map<SistemaFinanceiroComando>(request);
            try
            {
                unitOfWork.BeginTransaction();
                SistemaFinanceiro sistemaFinanceiro = await sistemaFinanceirosServico.EditarAsync(id, comando);
                unitOfWork.Commit();
                return mapper.Map<SistemaFinanceiroResponse>(sistemaFinanceiro);;
            }
            catch
            {
               unitOfWork.Rollback();
                throw;
            }
        }

        public async Task ExcluirAsync(int id)
        {
            try
            {
                unitOfWork.BeginTransaction();
                SistemaFinanceiro sistemaFinanceiro = await sistemaFinanceirosServico.ValidarAsync(id);
                await sistemaFinanceirosRepositorio.ExcluirAsync(sistemaFinanceiro);
                unitOfWork.Commit();
            }
            catch
            {
               unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<SistemaFinanceiroResponse> InserirAsync(SistemaFinanceiroInserirRequest request)
        {
            SistemaFinanceiroComando comando = mapper.Map<SistemaFinanceiroComando>(request);
            try
            {
                unitOfWork.BeginTransaction();
                SistemaFinanceiro sistemaFinanceiro = await sistemaFinanceirosServico.InserirAsync(comando);
                unitOfWork.Commit();
                return mapper.Map<SistemaFinanceiroResponse>(sistemaFinanceiro);
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<PaginacaoConsulta<SistemaFinanceiroResponse>> ListarAsync(SistemaFinanceiroListarRequest request)
        {
           SistemaFinanceiroListarFiltro filtro = mapper.Map<SistemaFinanceiroListarFiltro>(request);
            IQueryable<SistemaFinanceiro> query = await sistemaFinanceirosRepositorio.FiltrarAsync(filtro);
            PaginacaoConsulta<SistemaFinanceiro> sistemaFinanceiros = await sistemaFinanceirosRepositorio.ListarAsync(query, request.Qt, request.Pg, request.CpOrd, request.TpOrd);
            return mapper.Map<PaginacaoConsulta<SistemaFinanceiroResponse>>(sistemaFinanceiros);
        }

        public async Task<SistemaFinanceiroResponse> RecuperarAsync(int id)
        {
            SistemaFinanceiro sistemaFinanceiro = await sistemaFinanceirosServico.ValidarAsync(id);
            return mapper.Map<SistemaFinanceiroResponse>(sistemaFinanceiro);
        }
    }
}