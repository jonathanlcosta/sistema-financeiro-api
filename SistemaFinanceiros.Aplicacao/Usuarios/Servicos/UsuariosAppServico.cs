using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NHibernate;
using SistemaFinanceiros.Aplicacao.Transacoes.Interfaces;
using SistemaFinanceiros.Aplicacao.Usuarios.Servicos.Interfaces;
using SistemaFinanceiros.DataTransfer.Usuarios.Request;
using SistemaFinanceiros.DataTransfer.Usuarios.Response;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.Usuarios.Entidades;
using SistemaFinanceiros.Dominio.Usuarios.Repositorios;
using SistemaFinanceiros.Dominio.Usuarios.Repositorios.Filtros;
using SistemaFinanceiros.Dominio.Usuarios.Servicos.Comandos;
using SistemaFinanceiros.Dominio.Usuarios.Servicos.Interfaces;
using SistemaFinanceiros.Dominio.util;

namespace SistemaFinanceiros.Aplicacao.Usuarios.Servicos
{
    public class UsuariosAppServico : IUsuariosAppServico
    {
        private readonly ISistemaFinanceirosServico sistemaFinanceirosServico;
        private readonly IUsuariosServico usuariosServico;
        private readonly IUsuariosRepositorio usuariosRepositorio;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        
        public UsuariosAppServico(ISistemaFinanceirosServico sistemaFinanceirosServico, IUsuariosServico usuariosServico,
        IUsuariosRepositorio usuariosRepositorio, IUnitOfWork unitOfWork,  IMapper mapper)
        {
            this.sistemaFinanceirosServico = sistemaFinanceirosServico;
            this.usuariosRepositorio = usuariosRepositorio;
            this.usuariosServico = usuariosServico;
            this.mapper = mapper;
           this.unitOfWork = unitOfWork;
        }
        public async Task<UsuarioResponse> EditarAsync(int id, UsuarioEditarRequest request)
        {
            UsuarioEditarComando comando = mapper.Map<UsuarioEditarComando>(request);
            try
            {
                unitOfWork.BeginTransaction();
                Usuario usuario = await usuariosServico.EditarAsync(id, comando);
                unitOfWork.Commit();
                return mapper.Map<UsuarioResponse>(usuario);;
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public async Task ExcluirAsync(int id)
        {
            try
            {
                unitOfWork.BeginTransaction();
                Usuario usuario = await usuariosServico.ValidarAsync(id);
                await usuariosRepositorio.ExcluirAsync(usuario);
                unitOfWork.Commit();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        public async Task<PaginacaoConsulta<UsuarioResponse>> ListarAsync(UsuarioListarRequest request)
        {
            UsuarioListarFiltro filtro = mapper.Map<UsuarioListarFiltro>(request);
            IQueryable<Usuario> query = await usuariosRepositorio.FiltrarAsync(filtro);
            PaginacaoConsulta<Usuario> usuarios = await usuariosRepositorio.ListarAsync(query, request.Qt, request.Pg, request.CpOrd, request.TpOrd);
            return mapper.Map<PaginacaoConsulta<UsuarioResponse>>(usuarios);
        }

        public async Task<UsuarioResponse> RecuperarAsync(int id)
        {
           Usuario usuario = await usuariosServico.ValidarAsync(id);
           return mapper.Map<UsuarioResponse>(usuario);
        }
    }
}