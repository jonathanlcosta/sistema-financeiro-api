using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using SistemaFinanceiros.Aplicacao.Autenticacoes.Servicos.Interfaces;
using SistemaFinanceiros.DataTransfer.Autenticacoes.Request;
using SistemaFinanceiros.DataTransfer.Autenticacoes.Response;
using SistemaFinanceiros.Dominio.Autenticacoes.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.Usuarios.Repositorios;
using SistemaFinanceiros.Dominio.Usuarios.Servicos.Interfaces;

namespace SistemaFinanceiros.Aplicacao.Autenticacoes.Servicos
{
    public class AutenticacoesAppServico : IAutenticacoesAppServico
    {
        private readonly IUsuariosRepositorio usuariosRepositorio;
        private readonly IUsuariosServico usuariosServico;
        private readonly IAutenticacoesServico autenticacoesServico;
        private readonly IMapper mapper;
        public AutenticacoesAppServico(IUsuariosRepositorio usuariosRepositorio,IUsuariosServico usuariosServico,
        IAutenticacoesServico autenticacoesServico, IMapper mapper )
        {
            this.usuariosRepositorio = usuariosRepositorio;
            this.usuariosServico = usuariosServico;
            this.autenticacoesServico = autenticacoesServico;
            this.mapper = mapper;
        }

        public async Task<CadastroResponse> CadastrarAsync(CadastroRequest cadastroRequest)
        {
            var usuario =  autenticacoesServico.ValidarCadastro(cadastroRequest.Email, cadastroRequest.Senha);
            usuario.SetSenhaHash(BCrypt.Net.BCrypt.HashPassword(cadastroRequest.Senha));
            usuario = await usuariosRepositorio.InserirAsync(usuario);
            return mapper.Map<CadastroResponse>(usuario);
        }

        public async Task<LoginResponse> LogarAsync(LoginRequest loginRequest)
        {
           var usuario = await usuariosRepositorio.RecuperaUsuarioPorEmailAsync(loginRequest.Email);
            usuario = autenticacoesServico.ValidarLogin(usuario, loginRequest.Senha);

            string token = autenticacoesServico.GerarToken(usuario);

            var response = new LoginResponse();
            response.Token = token;

            return response;
        }
    }
}