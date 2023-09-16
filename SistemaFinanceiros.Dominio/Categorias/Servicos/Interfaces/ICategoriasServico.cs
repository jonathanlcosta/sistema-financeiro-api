using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.Dominio.Categorias.Entidades;
using SistemaFinanceiros.Dominio.Categorias.Servicos.Comandos;

namespace SistemaFinanceiros.Dominio.Categorias.Servicos.Interfaces
{
    public interface ICategoriasServico
    {
        Task<Categoria> ValidarAsync(int id);
        Task<Categoria> InserirAsync(CategoriaComando comando);
        Task<Categoria> EditarAsync(int id, CategoriaComando comando);
        Task<Categoria> InstanciarAsync(CategoriaComando comando);
    }
}