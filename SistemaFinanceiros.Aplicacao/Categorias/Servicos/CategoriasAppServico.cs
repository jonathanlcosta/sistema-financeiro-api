using AutoMapper;
using Microsoft.AspNetCore.Http;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SistemaFinanceiros.Aplicacao.Categorias.Servicos.Interfaces;
using SistemaFinanceiros.Aplicacao.Transacoes.Interfaces;
using SistemaFinanceiros.DataTransfer.Categorias.Request;
using SistemaFinanceiros.DataTransfer.Categorias.Response;
using SistemaFinanceiros.Dominio.Categorias.Entidades;
using SistemaFinanceiros.Dominio.Categorias.Repositorios;
using SistemaFinanceiros.Dominio.Categorias.Repositorios.Filtros;
using SistemaFinanceiros.Dominio.Categorias.Servicos.Comandos;
using SistemaFinanceiros.Dominio.Categorias.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Entidades;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.Aplicacao.Categorias.Servicos
{
    public class CategoriasAppServico : ICategoriasAppServico
    {
        private readonly ICategoriasServico categoriasServico;
        private readonly ICategoriasRepositorio categoriasRepositorio;
        private readonly ISistemaFinanceirosServico sistemaFinanceirosServico;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        public CategoriasAppServico(ICategoriasServico categoriasServico, ICategoriasRepositorio categoriasRepositorio,
         ISistemaFinanceirosServico sistemaFinanceirosServico, IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.categoriasRepositorio = categoriasRepositorio;
            this.categoriasServico = categoriasServico;
            this.sistemaFinanceirosServico = sistemaFinanceirosServico;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<CategoriaResponse> EditarAsync(int id, CategoriaEditarRequest request)
        {
            CategoriaComando comando = mapper.Map<CategoriaComando>(request);
            try
            {
                unitOfWork.BeginTransaction();
                Categoria categoria = await categoriasServico.EditarAsync(id, comando);
                unitOfWork.Commit();
                return mapper.Map<CategoriaResponse>(categoria);;
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
                Categoria categoria = await categoriasServico.ValidarAsync(id);
                await categoriasRepositorio.ExcluirAsync(categoria);
                unitOfWork.Commit();
            }
            catch
            {
               unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<CategoriaResponse> InserirAsync(CategoriaInserirRequest request)
        {
            CategoriaComando comando = mapper.Map<CategoriaComando>(request);
            try
            {
                unitOfWork.BeginTransaction();
                Categoria categoria = await categoriasServico.InserirAsync(comando);
                unitOfWork.Commit();
                return mapper.Map<CategoriaResponse>(categoria);
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<PaginacaoConsulta<CategoriaResponse>> ListarAsync(CategoriaListarRequest request)
        {
            CategoriaListarFiltro filtro = mapper.Map<CategoriaListarFiltro>(request);
            IQueryable<Categoria> query = await categoriasRepositorio.Filtrar(filtro);

            PaginacaoConsulta<Categoria> categorias = categoriasRepositorio.Listar(query, request.Qt, request.Pg, request.CpOrd, request.TpOrd);
            return mapper.Map<PaginacaoConsulta<CategoriaResponse>>(categorias);
        }

        public async Task<CategoriaResponse> RecuperarAsync(int id)
        {
            Categoria categoria = await categoriasServico.ValidarAsync(id);
            return mapper.Map<CategoriaResponse>(categoria);
        }

        public async Task UploadExcel(IFormFile file)
{
    try
    {
        unitOfWork.BeginTransaction();

        var planilha = new XSSFWorkbook(file.OpenReadStream());
        var folha = planilha.GetSheetAt(0);
        List<Categoria> categorias = new List<Categoria>();

        for (int row = 1; row <= folha.LastRowNum; row++)
        {
            if (folha.GetRow(row) != null)
            {
                IRow linha = folha.GetRow(row);
                string nome = linha.GetCell(0).StringCellValue;
                Categoria categoria = new Categoria(nome);
                categorias.Add(categoria);
            }
        }

        await categoriasRepositorio.InserirAsync(categorias);
        unitOfWork.Commit();
    }
    catch
    {
        unitOfWork.Rollback();
        throw;
    }
}


        public IList<CategoriaNomeResponse> ListarNomesCategoria()
        {
             IList<Categoria> categorias = categoriasRepositorio.ListarNomesCategoria();
            var response = mapper.Map<IList<CategoriaNomeResponse>>(categorias);
            return response;
        }
    }
    }
