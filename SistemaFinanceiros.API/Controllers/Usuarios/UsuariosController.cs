using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaFinanceiros.Aplicacao.Usuarios.Servicos.Interfaces;
using SistemaFinanceiros.DataTransfer.Usuarios.Request;
using SistemaFinanceiros.DataTransfer.Usuarios.Response;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.API.Controllers.Usuarios
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuariosAppServico usuariosAppServico;

       public UsuariosController(IUsuariosAppServico usuariosAppServico)
       {
        this.usuariosAppServico = usuariosAppServico;
       }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioResponse>> RecuperarAsync(int id)
        {
            var response = await usuariosAppServico.RecuperarAsync(id);

            if (response == null)
                return NotFound();

            return Ok(response);
        }

       [HttpGet]
        public async Task<ActionResult<PaginacaoConsulta<UsuarioResponse>>> ListarAsync([FromQuery] UsuarioListarRequest request)
        {    var response = await usuariosAppServico.ListarAsync(request);
            return Ok(response);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> EditarAsync(int id, [FromBody] UsuarioEditarRequest request)
        {
            await usuariosAppServico.EditarAsync(id,  request);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> ExcluirAsync(int id)
        {
            await usuariosAppServico.ExcluirAsync(id);
            return Ok();
        }
    }
}