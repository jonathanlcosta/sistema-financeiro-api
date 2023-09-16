using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Entidades;
using SistemaFinanceiros.Dominio.Usuarios.Entidades;
using SistemaFinanceiros.Dominio.Usuarios.Servicos.Comandos;

namespace SistemaFinanceiros.Dominio.Usuarios.Servicos.Interfaces
{
    public interface IUsuariosServico
    {
        Task<Usuario> ValidarAsync(int id);
        Task<Usuario> EditarAsync(int id, UsuarioEditarComando comando);
    }
}