using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiros.Aplicacao.SistemaFinanceiros.Servicos.Interfaces;
using SistemaFinanceiros.DataTransfer.SistemaFinanceiros.Request;
using SistemaFinanceiros.DataTransfer.SistemaFinanceiros.Response;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.API.Controllers.SistemaFinanceiros
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SistemaFinanceirosController : ControllerBase
    {
        private readonly ISistemaFinanceirosAppServico sistemaFinanceirosAppServico;

       public SistemaFinanceirosController(ISistemaFinanceirosAppServico sistemaFinanceirosAppServico)
       {
        this.sistemaFinanceirosAppServico = sistemaFinanceirosAppServico;
       }

        [HttpGet("{id}")]
        public async Task<ActionResult<SistemaFinanceiroResponse>> RecuperarAsync(int id)
        {
            var response = await sistemaFinanceirosAppServico.RecuperarAsync(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

       [HttpGet]
        public async Task<ActionResult<PaginacaoConsulta<SistemaFinanceiroResponse>>> ListarAsync([FromQuery] SistemaFinanceiroListarRequest request)
        {    var response = await sistemaFinanceirosAppServico.ListarAsync(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<SistemaFinanceiroResponse>> InserirAsync([FromBody] SistemaFinanceiroInserirRequest request)
        {
            var retorno = await sistemaFinanceirosAppServico.InserirAsync(request);
            return Ok(retorno);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EditarAsync(int id, [FromBody] SistemaFinanceiroEditarRequest request)
        {
            await sistemaFinanceirosAppServico.EditarAsync(id,  request);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> ExcluirAsync(int id)
        {
            await sistemaFinanceirosAppServico.ExcluirAsync(id);
            return Ok();
        }
    }
}