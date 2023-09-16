using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SistemaFinanceiros.Dominio.Categorias.Entidades;
using SistemaFinanceiros.Dominio.Categorias.Repositorios;
using SistemaFinanceiros.Dominio.Categorias.Servicos;
using SistemaFinanceiros.Dominio.Categorias.Servicos.Comandos;
using SistemaFinanceiros.Dominio.Execoes;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Entidades;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos.Interfaces;
using Xunit;

namespace SistemaFinanceiros.Dominio.Testes.Categorias.Servicos
{
    public class CategoriasServicoTestes
    {
         private readonly CategoriasServico sut;
        private readonly ICategoriasRepositorio categoriasRepositorio;
        private readonly ISistemaFinanceirosServico sistemaFinanceirosServico;
        private readonly Categoria categoriaValido;
        private readonly SistemaFinanceiro sistemaValido;
        public CategoriasServicoTestes()
        {
            categoriasRepositorio = Substitute.For<ICategoriasRepositorio>();
            categoriaValido = Builder<Categoria>.CreateNew().Build();
            sistemaValido = Builder<SistemaFinanceiro>.CreateNew().Build();
            sistemaFinanceirosServico = Substitute.For<ISistemaFinanceirosServico>();
            sut = new CategoriasServico(categoriasRepositorio, sistemaFinanceirosServico);
        }

        public class ValidarMetodo : CategoriasServicoTestes
        {
            [Fact]
            public void Dado_CategoriaNaoEncontrado_Espero_Excecao()
            {
                categoriasRepositorio.RecuperarAsync(1).ReturnsNull();
                sut.Invoking(x => x.ValidarAsync(1)).Should().ThrowExactlyAsync<RegraDeNegocioExcecao>();
            }

            [Fact]
            public async void Dado_CategoriaForEncontrado_Espero_CategoriaValido()
            {
                categoriasRepositorio.RecuperarAsync(1).Returns(categoriaValido);
                var resultado = await sut.ValidarAsync(1);
                resultado.Should().Be(categoriaValido);
            }
        }

        public class InserirMetodo : CategoriasServicoTestes
        {
            [Fact]
            public async void Dado_CategoriaValido_Espero_CategoriaInserido()
            {
                CategoriaComando comando = Builder<CategoriaComando>.CreateNew()
                .With(x => x.Nome, "Empresa").With(x => x.IdSistemaFinanceiro, 1).Build();
                
                Categoria resultado = await sut.InserirAsync(comando);
                categoriasRepositorio.InserirAsync(resultado).Returns(categoriaValido);

                resultado.Should().BeOfType<Categoria>();
                resultado.Nome.Should().Be(comando.Nome);
                resultado.Should().NotBeNull();
            }
        }

        public class EditarMetodo: CategoriasServicoTestes
        {
            [Fact]
            public async void Dado_ParametrosParaEditarCategoria_Espero_CategoriaEditado()
            {
               CategoriaComando comando = Builder<CategoriaComando>.CreateNew()
                .With(x => x.Nome, "Empresa").With(x => x.IdSistemaFinanceiro, 1).Build();

                categoriasRepositorio.RecuperarAsync(1).Returns(categoriaValido);

                Categoria resultado = await sut.EditarAsync(1, comando);

                resultado.Should().NotBeNull();
                resultado.Nome.Should().Be(comando.Nome);
            }
        }

         public class InstanciarMetodo : CategoriasServicoTestes
        {
            [Fact]
            public void Quando_DadosCategoriaForemValidos_Espero_ObjetoInstanciado()
            {
                CategoriaComando comando = Builder<CategoriaComando>.CreateNew()
                    .Build();

                Categoria resultado = sut.Instanciar(comando);

                resultado.Nome.Should().Be(comando.Nome);
            }

        }


    }
}