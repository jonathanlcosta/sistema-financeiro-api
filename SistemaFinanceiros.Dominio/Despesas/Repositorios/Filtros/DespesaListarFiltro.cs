using SistemaFinanceiros.Dominio.util.Filtros;
using SistemaFinanceiros.Dominio.util.Filtros.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaFinanceiros.Dominio.Despesas.Repositorios.Filtros
{
    public class DespesaListarFiltro : PaginacaoFiltro
    {
    public string Nome { get; set; }
    public string emailUsuario { get; set; }

        public DespesaListarFiltro() : base(cpOrd: "Nome", tpOrd: TipoOrdenacaoEnum.Asc)
        {

        }
    }

}