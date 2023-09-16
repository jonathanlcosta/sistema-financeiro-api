using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SistemaFinanceiros.Dominio.Execoes;
using SistemaFinanceiros.Dominio.SistemaFinanceiros.Entidades;

namespace SistemaFinanceiros.Dominio.Usuarios.Entidades
{
    public class Usuario
    {
        public virtual int Id { get; protected set; }
        public virtual string CPF { get; protected set; }
        public virtual string Nome { get; protected set; }
        public virtual string Email { get; protected set; }
         public virtual string Senha { get; protected set; }
        public virtual bool Administrador { get; protected set; }
        public virtual bool SistemaAtual { get; protected set; }
        public virtual SistemaFinanceiro SistemaFinanceiro { get; protected set; }

        public Usuario(string cpf, string nome, string email,string senha, bool administrador, bool sistemaAtual, SistemaFinanceiro sistemaFinanceiro)
        {
            SetCpf(cpf);
            SetEmail(email);
            SetSenha(senha);
            SetSistemaAtual(sistemaAtual);
            SetAdministrador(administrador);
            SetSistemaFinanceiro(sistemaFinanceiro);
            SetNome(nome);
            
        }

        protected Usuario()
        {
            
        }

        public Usuario(string email, string senha)
        {
            SetEmail(email);
            SetSenha(senha);
        }

        public virtual void SetCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                throw new AtributoObrigatorioExcecao("CPF");
            if (cpf.Length != 11)
                throw new TamanhoDeAtributoInvalidoExcecao("CPF", 11, 11);
            CPF = cpf.Replace(".", "").Replace("/", "").Replace("-", "");
        }

        public virtual void SetEmail(string email)
        {
             string pattern = @"^[a-zA-Z0-9]+@\S+\.com(\.\w+)?$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(email))
            {
                throw new AtributoInvalidoExcecao("Email");
            }
            Email = email;
        }

        public virtual void SetNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new AtributoObrigatorioExcecao("Nome");
            if (nome.Length > 100)
                throw new TamanhoDeAtributoInvalidoExcecao("Nome");
            Nome = nome;
        }

        public virtual void SetSenha(string senha)
        {
            if (string.IsNullOrEmpty(senha) || string.IsNullOrWhiteSpace(senha))
              {
                  throw new AtributoObrigatorioExcecao("Senha");
              }
              if (senha.Length < 6 || senha.Length > 20)
              {
                  throw new TamanhoDeAtributoInvalidoExcecao("Senha", 6, 20);
              }
              // Verifica se a senha possui pelo menos uma letra maiúscula, uma letra minúscula,
              if (!senha.Any(c => char.IsUpper(c)))
              {
                  throw new AtributoInvalidoExcecao("Senha");
              }
              if (!senha.Any(c => char.IsLower(c)))
              {
                  throw new AtributoInvalidoExcecao("Senha");
              }
              // um caractere especial e um número
              if (!senha.Any(c => char.IsSymbol(c) || char.IsPunctuation(c)))
              {
                  throw new AtributoInvalidoExcecao("Senha");
              }
              if (!senha.Any(c => char.IsNumber(c)))
              {
                  throw new AtributoInvalidoExcecao("Senha");
              }
            Senha = senha;
        }

        public virtual void SetSenhaHash(string senhaHash){
            Senha = senhaHash;
        }

        public virtual void SetAdministrador(bool administrador)
        {
            Administrador = administrador;
        }

        public virtual void SetSistemaAtual(bool sistemaAtual)
        {
            SistemaAtual = sistemaAtual;
        }

        public virtual void SetSistemaFinanceiro(SistemaFinanceiro sistema)
        {
            SistemaFinanceiro = sistema ?? throw new AtributoObrigatorioExcecao("Sistema");
        }
    }
}