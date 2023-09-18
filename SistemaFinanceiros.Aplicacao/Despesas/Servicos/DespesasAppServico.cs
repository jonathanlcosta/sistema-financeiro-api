using AutoMapper;
using NHibernate;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SistemaFinanceiros.Aplicacao.Despesas.Servicos.Interfaces;
using SistemaFinanceiros.Aplicacao.Transacoes.Interfaces;
using SistemaFinanceiros.DataTransfer.Despesas.Request;
using SistemaFinanceiros.DataTransfer.Despesas.Response;
using SistemaFinanceiros.Dominio.Categorias.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.Despesas.Entidades;
using SistemaFinanceiros.Dominio.Despesas.Repositorios;
using SistemaFinanceiros.Dominio.Despesas.Repositorios.Consultas;
using SistemaFinanceiros.Dominio.Despesas.Repositorios.Filtros;
using SistemaFinanceiros.Dominio.Despesas.Servicos.Comandos;
using SistemaFinanceiros.Dominio.Despesas.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.Aplicacao.Despesas.Servicos
{
    public class DespesasAppServico : IDespesasAppServico
    {
        private readonly IDespesasRepositorio despesasRepositorio;
        private readonly IDespesasServico despesasServico;
        private readonly ICategoriasServico categoriasServico;
        private readonly IDespesasConsultasRepositorio despesasConsultasRepositorio;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public DespesasAppServico(IDespesasRepositorio despesasRepositorio, IDespesasServico despesasServico, ICategoriasServico categoriasServico,
        IMapper mapper,  IUnitOfWork unitOfWork, IDespesasConsultasRepositorio despesasConsultasRepositorio)
        {
            this.categoriasServico = categoriasServico;
            this.despesasServico = despesasServico;
            this.despesasRepositorio = despesasRepositorio;
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.despesasConsultasRepositorio = despesasConsultasRepositorio;
        }

        public async Task<object> CarregaGraficos(string email)
        {
            return await despesasServico.CarregaGraficos(email);
        }

        public async Task<DespesaResponse> EditarAsync(int id, DespesaEditarRequest request)
        {
             DespesaComando comando = mapper.Map<DespesaComando>(request);
            try
            {
                unitOfWork.BeginTransaction();
                Despesa despesa = await despesasServico.EditarAsync(id, comando);
                unitOfWork.Commit();
                return mapper.Map<DespesaResponse>(despesa);;
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
                Despesa despesa = await despesasServico.ValidarAsync(id);
                await despesasRepositorio.ExcluirAsync(despesa);
                unitOfWork.Commit();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<DespesaResponse> InserirAsync(DespesaInserirRequest request)
        {
            DespesaComando comando = mapper.Map<DespesaComando>(request);
             
            try
            {
                unitOfWork.BeginTransaction();
                Despesa despesa = await despesasServico.InserirAsync(comando);
                unitOfWork.Commit();
                return mapper.Map<DespesaResponse>(despesa);
            }
            catch
            {
               unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<PaginacaoConsulta<DespesaResponse>> ListarAsync(DespesaListarRequest request)
        {
            DespesaListarFiltro filtro = mapper.Map<DespesaListarFiltro>(request);
            IQueryable<Despesa> query = await despesasRepositorio.Filtrar(filtro);
            PaginacaoConsulta<Despesa> despesas = despesasRepositorio.Listar(query, request.Qt, request.Pg, request.CpOrd, request.TpOrd);
            PaginacaoConsulta<DespesaResponse> response;
            response = mapper.Map<PaginacaoConsulta<DespesaResponse>>(despesas);
            return response;
        }

        public async Task<PaginacaoConsulta<DespesaResponse>> ListarDespesas(DespesaListarRequest request)
        {

            DespesaListarFiltro filtro = mapper.Map<DespesaListarFiltro>(request);
            IQueryable<Despesa> query = await despesasRepositorio.FiltrarDespesasAtrasadas(filtro);
            PaginacaoConsulta<Despesa> despesas = despesasRepositorio.Listar(query, request.Qt, request.Pg, request.CpOrd, request.TpOrd);
            return mapper.Map<PaginacaoConsulta<DespesaResponse>>(despesas);
        }


        public async Task<IList<DespesaResponse>> ListarDespesasUsuario(string emailUsuario)
        {

            IList<Despesa> despesas = await despesasRepositorio.ListarDespesasUsuario(emailUsuario);
            return mapper.Map<IList<DespesaResponse>>(despesas);
        }

        public PaginacaoConsulta<DespesaConsultaResponse> ListarDespesasUsuarioNaoPagasMesesAnteriorDapper(string email)
        {

        var despesas = despesasConsultasRepositorio.ListarDespesasUsuarioNaoPagasMesesAnterior(1, 100, email);
        var response = mapper.Map<PaginacaoConsulta<DespesaConsultaResponse>>(despesas);
         return response;

        }

        public async Task<IList<DespesaResponse>> ListarDespesasUsuarioNaoPagasMesesAtras(string email)
        {
            IList<Despesa> despesas = await despesasRepositorio.ListarDespesasUsuarioNaoPagasMesesAnterior(email);
            return mapper.Map<IList<DespesaResponse>>(despesas);
        }

        public async Task<DespesaResponse> RecuperarAsync(int id)
        {
            Despesa despesa = await despesasServico.ValidarAsync(id);
            return mapper.Map<DespesaResponse>(despesa);
        }

        public async Task<List<DespesasResumo>> ConsultaAsync()
        {
            var query = await despesasRepositorio.QueryAsync();
            var despesas = query.Select(x => new DespesasResumo
            {
                NomeDespesa = x.Nome,
                NomeSistema = x.Categoria.SistemaFinanceiro.Nome,
                NomeCategoria = x.Categoria.Nome,
                NomeUsuario = x.Usuario.Nome
            }).ToList();
            return despesas;
        }

        public async Task<Stream> ExportarExcel(DespesaListarRequest request)
        {
            IWorkbook planilha = new XSSFWorkbook();
            ISheet folha = planilha.CreateSheet("Despesas");
            IRow cabecalho = folha.CreateRow(0);

            cabecalho.CreateCell(0).SetCellValue("NomeDespesa");
            cabecalho.CreateCell(1).SetCellValue("NomeCategoria");
            cabecalho.CreateCell(2).SetCellValue("NomeSistema");
            cabecalho.CreateCell(3).SetCellValue("NomeUsuario");

            var filtro = mapper.Map<DespesaListarFiltro>(request);

            IQueryable<Despesa> query = await despesasRepositorio.Filtrar(filtro);

            var queryProjecao = query.Select(x => new
            {
                NomeDespesa = x.Nome,
                NomeSistema = x.Categoria.SistemaFinanceiro.Nome,
                NomeCategoria = x.Categoria.Nome,
                NomeUsuario = x.Usuario.Nome
            });

            int indiceDeLinha = 1;

            foreach (var despesa in queryProjecao)
            {
                IRow linha = folha.CreateRow(indiceDeLinha);
                linha.CreateCell(0).SetCellValue(despesa.NomeDespesa);
                linha.CreateCell(1).SetCellValue(despesa.NomeSistema);
                linha.CreateCell(2).SetCellValue(despesa.NomeCategoria);
                linha.CreateCell(3).SetCellValue(despesa.NomeUsuario);
                indiceDeLinha++;
            }

            for (int coluna = 0; coluna <= cabecalho.LastCellNum - 1; coluna++)
            {
                folha.AutoSizeColumn(coluna);
            }

            var memoryStream = new MemoryStream();
            planilha.Write(memoryStream, false);
            var bytes = memoryStream.ToArray();
            File.WriteAllBytes($@"C:\\Users\nickc\Downloads_{DateTime.Now:ddMMyyyyHHmmss}.xlsx", bytes);

            return new MemoryStream(bytes);
        }
    }
}