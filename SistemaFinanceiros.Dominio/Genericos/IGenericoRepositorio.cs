using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.Dominio.util;
using SistemaFinanceiros.Dominio.util.Filtros.Enumeradores;

namespace SistemaFinanceiros.Dominio.Genericos
{
    public interface IGenericoRepositorio<T> where T: class
    {
        T Recuperar(int codigo);

        T Inserir(T entidade);

        T Editar(T entidade);

        void Excluir(T entidade);
        Task InserirAsync(IEnumerable<T> entidades);

         PaginacaoConsulta<T> Listar(IQueryable<T> query, int qt, int pg, string cpOrd, TipoOrdenacaoEnum tpOrd);

        IQueryable<T> Query();

        IList<T> QueryList();

        Task<T> RecuperarAsync(int id);

        Task<T> InserirAsync(T entidade);

        Task<T> EditarAsync(T entidade);
        Task ExcluirAsync(T entidade);

        Task<PaginacaoConsulta<T>> ListarAsync(IQueryable<T> query, int qt, int pg, string cpOrd, TipoOrdenacaoEnum tpOrd);
    }
}