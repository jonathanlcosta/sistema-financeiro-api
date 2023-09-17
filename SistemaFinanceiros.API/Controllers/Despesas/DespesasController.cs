using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiros.Aplicacao.Despesas.Servicos.Interfaces;
using SistemaFinanceiros.DataTransfer.Despesas.Request;
using SistemaFinanceiros.DataTransfer.Despesas.Response;
using SistemaFinanceiros.Dominio.Despesas.Repositorios.Consultas;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.API.Controllers.Despesas
{
    [ApiController]
    [Route("api/[controller]")]
    public class DespesasController : ControllerBase
    {
        private readonly IDespesasAppServico despesasAppServico;
        public DespesasController(IDespesasAppServico despesasAppServico)
        {
            this.despesasAppServico = despesasAppServico;
        }

        [HttpGet("despesas/despesasUsuario")]
        public async Task<ActionResult<IList<DespesaResponse>>> ListarDespesasUsuario(string emailUsuario)
        {
            var response = await despesasAppServico.ListarDespesasUsuario(emailUsuario);
            return Ok(response);      
            
        }

       [HttpGet("despesas/usuario-nao-pagas-anteriorDapper")]
        public ActionResult<IList<DespesaConsultaResponse>> ListarDespesasUsuarioNaoPagasMesesAnterior(string email)
        {
            var response = despesasAppServico.ListarDespesasUsuarioNaoPagasMesesAnteriorDapper(email);
            return Ok(response);      
        }

        [HttpGet("despesas/usuario-nao-pagas-atras")]
        public async Task<ActionResult<IList<DespesaResponse>>> ListarDespesasUsuarioNaoPagasMesesAtras(string emailUsuario)
        {
            var response = await despesasAppServico.ListarDespesasUsuarioNaoPagasMesesAtras(emailUsuario);
            return Ok(response);      
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DespesaResponse>> Recuperar(int id)
        {
            var response = await despesasAppServico.RecuperarAsync(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("CarregaGraficos")]
        public async Task<object> CarregaGraficos(string emailUsuario)
        {
            return  await despesasAppServico.CarregaGraficos(emailUsuario);
        }

        [HttpPost]
        public async Task<ActionResult<DespesaResponse>> InserirAsync([FromBody] DespesaInserirRequest request)
        {
            var retorno = await despesasAppServico.InserirAsync(request);
            return Ok(retorno);
        }

        [HttpGet()]
        public async Task<ActionResult<PaginacaoConsulta<DespesaResponse>>> ListarDespesas([FromQuery] DespesaListarRequest despesaListarRequest)
        {    var response = await despesasAppServico.ListarAsync(despesaListarRequest);
            return Ok(response);
        }

        [HttpGet("ListarDespesasNaoPagasMesesAtras")]
        public async Task<ActionResult<PaginacaoConsulta<DespesaResponse>>> ListarDespesasNaoPagasMesesAtras([FromQuery] DespesaListarRequest despesaListarRequest)
        {    var response = await despesasAppServico.ListarDespesas(despesaListarRequest);
            return Ok(response);
        }

        [HttpGet("Consulta")]
        public async Task<ActionResult<IList<DespesasResumo>>> ConsultaAsync()
        {
            var response = await despesasAppServico.ConsultaAsync();
            return Ok(response);
        }

         [HttpGet]
        [Route("excel")]
        public async Task<ActionResult> Exportar([FromQuery] DespesaListarRequest request)
        {
            var planilha = await despesasAppServico.ExportarExcel(request);
            FileStreamResult result = new FileStreamResult(planilha, "application/octet-stream")
            {
                FileDownloadName = "Relatório de Despesas.xlsx"
            };
            return result;
        }
    }
}