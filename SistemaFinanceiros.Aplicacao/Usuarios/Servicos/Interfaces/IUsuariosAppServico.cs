using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.DataTransfer.Usuarios.Request;
using SistemaFinanceiros.DataTransfer.Usuarios.Response;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.Aplicacao.Usuarios.Servicos.Interfaces
{
    public interface IUsuariosAppServico
    {
        Task<PaginacaoConsulta<UsuarioResponse>> ListarAsync(UsuarioListarRequest usuarioListarRequest);
        Task<UsuarioResponse> RecuperarAsync(int id);
        Task<UsuarioResponse> EditarAsync(int id, UsuarioEditarRequest usuarioEditarRequest);
        Task ExcluirAsync(int id); 
    }
}