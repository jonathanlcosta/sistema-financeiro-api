using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SistemaFinanceiros.DataTransfer.Categorias.Request;
using SistemaFinanceiros.DataTransfer.Categorias.Response;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.Aplicacao.Categorias.Servicos.Interfaces
{
    public interface ICategoriasAppServico
    {
        Task<PaginacaoConsulta<CategoriaResponse>> ListarAsync(CategoriaListarRequest request);
        Task<CategoriaResponse> RecuperarAsync(int id);
        Task<CategoriaResponse> InserirAsync(CategoriaInserirRequest categoriaInserirRequest);
        Task<CategoriaResponse> EditarAsync(int id, CategoriaEditarRequest categoriaEditarRequest);
        Task ExcluirAsync(int id); 
        Task UploadExcel(IFormFile file);
        IList<CategoriaNomeResponse> ListarNomesCategoria();

    }
}