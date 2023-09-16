using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistemaFinanceiros.DataTransfer.Autenticacoes.Request;
using SistemaFinanceiros.DataTransfer.Autenticacoes.Response;

namespace SistemaFinanceiros.Aplicacao.Autenticacoes.Servicos.Interfaces
{
    public interface IAutenticacoesAppServico
    {
        Task<LoginResponse> LogarAsync(LoginRequest loginRequest);
        Task<CadastroResponse> CadastrarAsync(CadastroRequest cadastroRequest);
    }
}