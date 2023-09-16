using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.Dominio.Genericos;
using SistemaFinanceiros.Dominio.Usuarios.Entidades;
using SistemaFinanceiros.Dominio.Usuarios.Repositorios.Filtros;

namespace SistemaFinanceiros.Dominio.Usuarios.Repositorios
{
    public interface IUsuariosRepositorio : IGenericoRepositorio<Usuario>
    {
        Task<Usuario> RecuperaUsuarioPorEmailAsync(string email);
        Task<IQueryable<Usuario>> FiltrarAsync(UsuarioListarFiltro filtro);

    }
}