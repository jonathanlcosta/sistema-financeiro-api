using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.Dominio.Despesas.Entidades;
using SistemaFinanceiros.Dominio.Despesas.Repositorios.Filtros;
using SistemaFinanceiros.Dominio.Genericos;

namespace SistemaFinanceiros.Dominio.Despesas.Repositorios
{
    public interface IDespesasRepositorio : IGenericoRepositorio<Despesa>
    {
        Task<IList<Despesa>> ListarDespesasUsuarioNaoPagasMesesAnterior(string email);
        Task<IList<Despesa>> ListarDespesasUsuario(string email);
        Task<IQueryable<Despesa>> Filtrar(DespesaListarFiltro filtro);
        Task<IQueryable<Despesa>> FiltrarDespesasAtrasadas(DespesaListarFiltro filtro);


    }
}