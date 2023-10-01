using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SistemaFinanceiros.Dominio.Categorias.Entidades;
using SistemaFinanceiros.Dominio.Categorias.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.Despesas.Entidades;
using SistemaFinanceiros.Dominio.Despesas.Enumeradores;
using SistemaFinanceiros.Dominio.Despesas.Repositorios;
using SistemaFinanceiros.Dominio.Despesas.Servicos.Comandos;
using SistemaFinanceiros.Dominio.Despesas.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.Execoes;
using SistemaFinanceiros.Dominio.Mensageria.Interfaces;
using SistemaFinanceiros.Dominio.Usuarios.Entidades;
using SistemaFinanceiros.Dominio.Usuarios.Servicos.Interfaces;

namespace SistemaFinanceiros.Dominio.Despesas.Servicos
{
    public class DespesasServico : IDespesasServico
    {
        private readonly IDespesasRepositorio despesasRepositorio;
        private readonly ICategoriasServico categoriasServico;
        private readonly IUsuariosServico usuariosServico;
        private readonly IMensageriaServico mensageriaServico;
        private const string QUEUE_NAME = "Despesas";

        public DespesasServico(IDespesasRepositorio despesasRepositorio, ICategoriasServico categoriasServico,
        IUsuariosServico usuariosServico, IMensageriaServico mensageriaServico)
        {
            this.despesasRepositorio = despesasRepositorio;
            this.categoriasServico = categoriasServico;
            this.usuariosServico = usuariosServico;
            this.mensageriaServico = mensageriaServico;
        }

        public async Task<object> CarregaGraficos(string email)
        {
            var despesasUsuario =  await despesasRepositorio.ListarDespesasUsuario(email);
            var despesasAnterior =  await despesasRepositorio.ListarDespesasUsuarioNaoPagasMesesAnterior(email);

            var despesas_naoPagasMesesAnteriores = despesasAnterior.Any() ?
                despesasAnterior.ToList().Sum(x => x.Valor) : 0;

            var despesas_pagas = despesasUsuario.Where(d => d.Pago && d.TipoDespesa == EnumTipoDespesa.Contas)
                .Sum(x => x.Valor);

            var despesas_pendentes = despesasUsuario.Where(d => !d.Pago && d.TipoDespesa == EnumTipoDespesa.Contas)
                .Sum(x => x.Valor);

            var investimentos = despesasUsuario.Where(d => d.TipoDespesa == EnumTipoDespesa.Investimento)
                .Sum(x => x.Valor);

            return new
            {
                sucesso = "OK",
                despesas_pagas = despesas_pagas,
                despesas_pendentes = despesas_pendentes,
                despesas_naoPagasMesesAnteriores = despesas_naoPagasMesesAnteriores,
                investimentos = investimentos
            };
        }

        public void EnviarNotifificacaoDespesaAtrasada(Despesa despesa)
        {
            string despesaJson = JsonSerializer.Serialize(despesa);
            byte[] despesaBytes = Encoding.UTF8.GetBytes(despesaJson);
            mensageriaServico.Publish(QUEUE_NAME, despesaBytes);
        }

        public void VerificarDespesaAtrasada(Despesa despesa)
        {
            if(despesa.DespesaAtrasada is true)
            EnviarNotifificacaoDespesaAtrasada(despesa);
        }

        public async Task<Despesa> EditarAsync(int id, DespesaComando comando)
        {
            Categoria categoria = await categoriasServico.ValidarAsync(comando.IdCategoria);
            Usuario usuario = await usuariosServico.ValidarAsync(comando.IdUsuario);
            Despesa despesa = await ValidarAsync(id);
            despesa.SetNome(comando.Nome);
            despesa.SetValor(comando.Valor);
            despesa.SetTipoDespesa(comando.TipoDespesa);
            despesa.SetDataVencimento(comando.DataVencimento);
            despesa.SetPago(comando.Pago);
            despesa.SetDespesaAtrasada(comando.DespesaAtrasada);
            despesa.SetCategoria(categoria);
            despesa.SetUsuario(usuario);
            despesasRepositorio.Editar(despesa);
            return despesa;

        }

        public async Task<Despesa> InserirAsync(DespesaComando comando)
        {
            Despesa despesa = await InstanciarAsync(comando);
            VerificarDespesaAtrasada(despesa);
            await despesasRepositorio.InserirAsync(despesa);
            return despesa;
        }

        public async Task<Despesa> InstanciarAsync(DespesaComando comando)
        {
            Categoria categoria =  await categoriasServico.ValidarAsync(comando.IdCategoria);
            Usuario usuario = await usuariosServico.ValidarAsync(comando.IdUsuario);
            Despesa despesa = new(comando.Nome, comando.Valor, comando.TipoDespesa, comando.DataVencimento, comando.Pago, comando.DespesaAtrasada,
            categoria, usuario);

            return despesa;
        }

        public async Task<Despesa> ValidarAsync(int id)
        {
            Despesa despesa = await despesasRepositorio.RecuperarAsync(id);
            if(despesa is null)
            {
                 throw new RegraDeNegocioExcecao("Despesa não encontrada");
            }
            return despesa;
        }
    }
}