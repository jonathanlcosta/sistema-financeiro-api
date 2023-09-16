using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.Dominio.Execoes;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Entidades;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.Usuarios.Entidades;
using SistemaFinanceiros.Dominio.Usuarios.Repositorios;
using SistemaFinanceiros.Dominio.Usuarios.Servicos.Comandos;
using SistemaFinanceiros.Dominio.Usuarios.Servicos.Interfaces;

namespace SistemaFinanceiros.Dominio.Usuarios.Servicos
{
    public class UsuariosServico : IUsuariosServico
    {
        private readonly IUsuariosRepositorio usuariosRepositorio;
        private readonly ISistemaFinanceirosServico sistemaFinanceirosServico;
        public UsuariosServico(IUsuariosRepositorio usuariosRepositorio, ISistemaFinanceirosServico sistemaFinanceirosServico)
        {
            this.usuariosRepositorio = usuariosRepositorio;
            this.sistemaFinanceirosServico = sistemaFinanceirosServico;
        }
        public async Task<Usuario> EditarAsync(int id, UsuarioEditarComando comando)
        {
           SistemaFinanceiro sistemaFinanceiro = await sistemaFinanceirosServico.ValidarAsync(comando.idSistemaFinanceiro);
           Usuario usuario = await ValidarAsync(id);
           usuario.SetCpf(comando.CPF);
           usuario.SetEmail(comando.Email);
           usuario.SetAdministrador(comando.Administrador);
           usuario.SetSistemaAtual(comando.SistemaAtual);
           usuario.SetSistemaFinanceiro(sistemaFinanceiro);
           usuario.SetNome(comando.Nome);
           await usuariosRepositorio.EditarAsync(usuario);
           return usuario;
        }

        public async Task<Usuario> ValidarAsync(int id)
        {
           var usuarioResponse = await usuariosRepositorio.RecuperarAsync(id);
            if(usuarioResponse is null)
            throw new RegraDeNegocioExcecao("Usuario não encontrado");

            return usuarioResponse;
        }
    }
}