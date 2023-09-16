using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using SistemaFinanceiros.Dominio.Categorias.Entidades;
using SistemaFinanceiros.Dominio.Categorias.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.Despesas.Entidades;
using SistemaFinanceiros.Dominio.Despesas.Repositorios;
using SistemaFinanceiros.Dominio.Despesas.Servicos;
using SistemaFinanceiros.Dominio.Despesas.Servicos.Comandos;
using SistemaFinanceiros.Dominio.Execoes;
using SistemaFinanceiros.Dominio.Usuarios.Entidades;
using SistemaFinanceiros.Dominio.Usuarios.Servicos.Interfaces;
using Xunit;

namespace SistemaFinanceiros.Dominio.Testes.Despesas.Servicos
{
    public class DespesasServicoTestes
    {
        private readonly DespesasServico sut;
        private readonly IDespesasRepositorio despesasRepositorio;
        private readonly IUsuariosServico usuariosServico;
        private readonly ICategoriasServico categoriasServico;
        private readonly Despesa despesaValido;
        private readonly Usuario usuarioValido;
        private readonly Categoria categoriaValido;
        public DespesasServicoTestes()
        {
            despesasRepositorio = Substitute.For<IDespesasRepositorio>();
            usuarioValido = Builder<Usuario>.CreateNew().Build();
            categoriaValido = Builder<Categoria>.CreateNew().Build();
            usuariosServico = Substitute.For<IUsuariosServico>();
            categoriasServico = Substitute.For<ICategoriasServico>();
            despesaValido = Builder<Despesa>.CreateNew().Build();
            sut = new(despesasRepositorio, categoriasServico, usuariosServico);
        }

        public class EditarDespesa : DespesasServicoTestes
        {
            [Fact]
            public async void Quando_DadosDespesaForemValidos_Espero_ObjetoEditado()
            {
                DespesaComando comando = Builder<DespesaComando>.CreateNew().
                Build();
                despesasRepositorio.RecuperarAsync(1).Returns(despesaValido);

                Despesa resultado =await sut.EditarAsync(1, comando);
                resultado.Nome.Should().Be(comando.Nome);
                resultado.TipoDespesa.Should().Be(comando.TipoDespesa);
                resultado.DespesaAtrasada.Should().Be(comando.DespesaAtrasada);
                resultado.Valor.Should().Be(comando.Valor);
                resultado.DataVencimento.Should().Be(comando.DataVencimento);
                resultado.Pago.Should().Be(comando.Pago);
                despesasRepositorio.EditarAsync(resultado).Returns(despesaValido);
            }

        }

        public class ValidarMetodo : DespesasServicoTestes
        {
            [Fact]
            public void Dado_DespesaNaoEncontrado_Espero_Excecao()
            {
                despesasRepositorio.RecuperarAsync(2).ReturnsNull();


                sut.Invoking(x => x.ValidarAsync(2)).Should().ThrowAsync<RegraDeNegocioExcecao>();
            }

            [Fact]
            public async void Dado_DespesaForEncontrado_Espero_DespesaValido()
            {
                despesasRepositorio.RecuperarAsync(1).Returns(despesaValido);
                var resultado = await sut.ValidarAsync(1);
                resultado.Should().Be(despesaValido);
            }
        }

        public class CarregarGraficos: DespesasServicoTestes
        {
            [Fact]
            public void Quando_EmailForPassado_Espero_GraficosCarregados()
            {
                string email = "leonardo@gmail.com";
                var despesasUsuario = Builder<Despesa>.CreateListOfSize(3).Build().ToList();
                var despesasAnterior = Builder<Despesa>.CreateListOfSize(2).Build().ToList();
               despesasRepositorio.ListarDespesasUsuario(email).Returns(despesasUsuario);
              despesasRepositorio.ListarDespesasUsuarioNaoPagasMesesAnterior(email).Returns(despesasAnterior);

                var resultado = sut.CarregaGraficos(email);

                despesasRepositorio.Received(1).ListarDespesasUsuario(email);
                despesasRepositorio.Received(1).ListarDespesasUsuarioNaoPagasMesesAnterior(email);

                Assert.NotNull(resultado);
                
            }
        }

        public class InserirMetodo: DespesasServicoTestes
        {
            [Fact]
            public async void Quando_DadosDespesaForemValidos_Espero_ObjetoInserido()
            {
                DespesaComando comando = Builder<DespesaComando>.CreateNew().
                Build();

                usuariosServico.ValidarAsync(comando.IdUsuario).Returns(usuarioValido);
                categoriasServico.ValidarAsync(comando.IdCategoria).Returns(categoriaValido);

                Despesa resultado = await sut.InserirAsync(comando);
                resultado.Nome.Should().Be(comando.Nome);
                resultado.TipoDespesa.Should().Be(comando.TipoDespesa);
                resultado.DespesaAtrasada.Should().Be(comando.DespesaAtrasada);
                resultado.Valor.Should().Be(comando.Valor);
                resultado.DataVencimento.Should().Be(comando.DataVencimento);
                resultado.Pago.Should().Be(comando.Pago);
                despesasRepositorio.InserirAsync(resultado).Returns(despesaValido);
            }
        }

        public class InstanciarMetodo: DespesasServicoTestes
        {
            [Fact]
            public async void Quando_DadosDespesaForemValidos_Espero_ObjetoInstanciado()
            {
                DespesaComando comando = Builder<DespesaComando>.CreateNew().
                Build();

                Despesa resultado = await sut.InstanciarAsync(comando);
                resultado.Nome.Should().Be(comando.Nome);
                resultado.TipoDespesa.Should().Be(comando.TipoDespesa);
                resultado.DespesaAtrasada.Should().Be(comando.DespesaAtrasada);
                resultado.Valor.Should().Be(comando.Valor);
                resultado.DataVencimento.Should().Be(comando.DataVencimento);
                resultado.Pago.Should().Be(comando.Pago);

            }
        }

    }
}