using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using SistemaFinanceiros.Dominio.Categorias.Entidades;
using SistemaFinanceiros.Dominio.Despesas.Entidades;
using SistemaFinanceiros.Dominio.Despesas.Repositorios;
using SistemaFinanceiros.Dominio.Despesas.Repositorios.Consultas;
using SistemaFinanceiros.Dominio.Despesas.Repositorios.Filtros;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Entidades;
using SistemaFinanceiros.Dominio.Usuarios.Entidades;
using SistemaFinanceiros.Dominio.util;
using SistemaFinanceiros.Infra.Genericos;

namespace SistemaFinanceiros.Infra.Despesas
{
    public class DespesasRepositorio : GenericoRepositorio<Despesa>, IDespesasRepositorio
    { 
        public DespesasRepositorio(ISession session) : base (session)
        {
            
        }

        public async Task<IQueryable<Despesa>> Filtrar(DespesaListarFiltro filtro)
        {
            IQueryable<Despesa> query = await QueryAsync();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                 query = query.Where(d => d.Nome.Contains(filtro.Nome));
            }

            if (!string.IsNullOrEmpty(filtro.emailUsuario))
            {
                 query = query.Where(d => d.Usuario.Email == filtro.emailUsuario);
            }

            return query;
        }

        public async Task<IQueryable<Despesa>> FiltrarDespesasAtrasadas(DespesaListarFiltro filtro)
        {
             IQueryable<Despesa> query = await QueryAsync();

            if (!string.IsNullOrEmpty(filtro.Nome))
            {
                 query = query.Where(d => d.Nome.Contains(filtro.Nome));
            }

            if (!string.IsNullOrEmpty(filtro.emailUsuario))
            {
                 query = query.Where(d => d.Usuario.Email == filtro.emailUsuario && 
                            d.Pago == false &&
                            d.DataVencimento.Year == DateTime.Now.Year &&
                            d.DataVencimento.Month == DateTime.Now.AddMonths(-1).Month);
            }

            return query;
        }

        public async Task<IList<Despesa>> ListarDespesasUsuario(string email)
        {
            IList<Despesa> despesas = await session.Query<Despesa>().Where(d => d.Usuario.Email == email).ToListAsync();
            return despesas;
        }

       public async Task<IList<Despesa>> ListarDespesasUsuarioNaoPagasMesesAnterior(string email)
        {
            IList<Despesa> despesas = await session.Query<Despesa>().Where(d => d.Usuario.Email == email && 
                            !d.Pago &&
                            d.DataVencimento.Year == DateTime.Now.Year &&
                            d.DataVencimento.Month == DateTime.Now.AddMonths(-1).Month).ToListAsync();

            return despesas;
        }

    }
}