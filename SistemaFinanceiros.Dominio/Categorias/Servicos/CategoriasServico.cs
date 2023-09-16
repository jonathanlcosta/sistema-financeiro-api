using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.Dominio.Categorias.Entidades;
using SistemaFinanceiros.Dominio.Categorias.Repositorios;
using SistemaFinanceiros.Dominio.Categorias.Servicos.Comandos;
using SistemaFinanceiros.Dominio.Categorias.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.Execoes;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Entidades;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos.Interfaces;

namespace SistemaFinanceiros.Dominio.Categorias.Servicos
{
    public class CategoriasServico : ICategoriasServico
    {
        private readonly ICategoriasRepositorio categoriasRepositorio;
        private readonly ISistemaFinanceirosServico sistemaFinanceirosServico;
        public CategoriasServico(ICategoriasRepositorio categoriasRepositorio, ISistemaFinanceirosServico sistemaFinanceirosServico)
        {
            this.categoriasRepositorio = categoriasRepositorio;
            this.sistemaFinanceirosServico = sistemaFinanceirosServico;
        }
        public async Task<Categoria> EditarAsync(int id, CategoriaComando comando)
        {
            SistemaFinanceiro sistemaFinanceiro = await sistemaFinanceirosServico.ValidarAsync(comando.IdSistemaFinanceiro);
            Categoria categoria = await ValidarAsync(id);
            categoria.SetNome(comando.Nome);
            categoria.SetSistema(sistemaFinanceiro);
            await categoriasRepositorio.EditarAsync(categoria);
            return categoria;
        }

        public async Task<Categoria> InserirAsync(CategoriaComando comando)
        {
            Categoria categoria = await InstanciarAsync(comando);
            await categoriasRepositorio.InserirAsync(categoria);
            return categoria;
        }

         public async Task<Categoria> InstanciarAsync(CategoriaComando comando)
        {
            SistemaFinanceiro sistemaFinanceiro = await sistemaFinanceirosServico.ValidarAsync(comando.IdSistemaFinanceiro);
            Categoria categoria = new(comando.Nome, sistemaFinanceiro);
            return categoria;
        }

        public async Task<Categoria> ValidarAsync(int id)
        {
            var categoriaResponse = await categoriasRepositorio.RecuperarAsync(id);
            if(categoriaResponse is null){
                throw new RegraDeNegocioExcecao("Categoria não encontrada");
            }
           return categoriaResponse;

        }
    }
}