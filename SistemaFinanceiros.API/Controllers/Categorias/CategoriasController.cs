using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiros.Aplicacao.Categorias.Servicos.Interfaces;
using SistemaFinanceiros.DataTransfer.Categorias.Request;
using SistemaFinanceiros.DataTransfer.Categorias.Response;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.API.Controllers.Categorias
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriasAppServico categoriasAppServico;

       public CategoriasController(ICategoriasAppServico categoriasAppServico)
       {
        this.categoriasAppServico = categoriasAppServico;
       }

          /// <summary>
    /// Recupera uma categoria por Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaResponse>> RecuperarAsync(int id)
        {
            var response = await categoriasAppServico.RecuperarAsync(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

          /// <summary>
    /// Lista as categorias com paginação
    /// </summary>  
    /// <param name="request"></param>
    /// <returns></returns>
       [HttpGet("listar-sincrono")]
        public async Task<ActionResult<PaginacaoConsulta<CategoriaResponse>>> ListarAsync([FromQuery] CategoriaListarRequest request)
        {    
            var response = await categoriasAppServico.ListarAsync(request);
   
             return Ok(response);
        }

         /// <summary>
    /// Lista apenas os nomes das categorias
    /// </summary>
    /// <returns></returns>
        [HttpGet("ListarNomesCategorias")]
        public ActionResult<IList<CategoriaNomeResponse>> ListarNomesCategoria()
        {
            var response = categoriasAppServico.ListarNomesCategoria();
            return Ok(response);
        }


            /// <summary>
    /// Adiciona uma nova categoria
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<CategoriaResponse>> Inserir([FromBody] CategoriaInserirRequest request)
        {
            var retorno = await categoriasAppServico.InserirAsync(request);
            return Ok(retorno);
        }


        /// <summary>
    /// Edita uma categoria por Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> EditarAsync(int id, [FromBody] CategoriaEditarRequest request)
        {

            await categoriasAppServico.EditarAsync(id,  request);
            return Ok();
        }


               /// <summary>
    /// Deleta uma categoria por Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> ExcluirAsync(int id)
        {
            await categoriasAppServico.ExcluirAsync(id);
            return Ok();
        }


        /// <summary>
    /// Adiciona um arquivo excel dentro do banco
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
        [HttpPost]
        [Route("excel")]
        public async Task<ActionResult> UploadExcel(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                await categoriasAppServico.UploadExcel(file);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

    }
}