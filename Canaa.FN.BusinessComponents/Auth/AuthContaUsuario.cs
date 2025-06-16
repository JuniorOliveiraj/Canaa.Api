using Canaa.AppHost.utils.Query;
using Canaa.DataContracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Auth
{
    public class AuthContaUsuario : IAuthContaUsuario
    {
        public UserLogin Login(UserLogin userLogin)
        {
            var query = new Query(@"SELECT ID FROM Z_USUARIOS WHERE EMAIL = :EMAIL AND SENHA = :SENHA");
            query.AddParameter(new Parameter("EMAIL", userLogin.Email));
            query.AddParameter(new Parameter("SENHA", userLogin.Senha));

            var result = query.Execute().FirstOrDefault();

            if (result != null && result["ID"] is int id)
                return Autorizado(id, "Login realizado com sucesso.");

            return NaoAutorizado(userLogin, "Email ou senha inválidos.");
        }


        public UserLogin CriarConta(UserLogin UserLogin)
        {
            if (VerificaEmail(UserLogin.Email))
                return new UserLogin { Mensage = "Email já cadastrado", Autorizado = false };

            if (!string.IsNullOrEmpty(UserLogin.NomeUsuario) && VerificaNome(UserLogin.NomeUsuario))
                return new UserLogin { Mensage = string.IsNullOrEmpty(UserLogin.NomeUsuario) ? "Nome de usuario é obrigatorio" : "Já existe um usuário com esse nome", Autorizado = false };


            var query = new Query(@"INSERT INTO Z_USUARIOS (NOME, EMAIL, SENHA, APELIDO) VALUES (:NOME, :EMAIL, :SENHA, :APELIDO) RETURNING ID INTO :ID");
            query.AddParameter(new Parameter("APELIDO", UserLogin.NomeUsuario));
            query.AddParameter(new Parameter("EMAIL", UserLogin.Email));
            query.AddParameter(new Parameter("SENHA", UserLogin.Senha));
            query.AddParameter(new Parameter("NOME", UserLogin.Nome));
            var result = query.Execute().FirstOrDefault();
            int id;

            if (result != null && int.TryParse(result["ID"].ToString(), out id))
                return Autorizado(id, "Conta criada com sucesso.");

            return NaoAutorizado(UserLogin, "Erro ao criar conta. ID não foi retornado.");
        }


        private bool VerificaEmail(string email)
        {
            Query query = new Query(@"SELECT ID FROM Z_USUARIOS WHERE EMAIL = :EMAIL");

            query.AddParameter(new Parameter("EMAIL", email));
            var result = query.Execute().FirstOrDefault();


            if (result != null && result["ID"] is int id)
                return true;

            return false;
        }
        private bool VerificaNome(string nome)
        {
            Query query = new Query(@"SELECT ID FROM Z_USUARIOS WHERE APELIDO = :APELIDO");
            query.AddParameter(new Parameter("APELIDO", nome));
            var result = query.Execute().FirstOrDefault();
            if (result != null && result["ID"] is int id)
                return true;
            return false;
        }



        private UserLogin NaoAutorizado(UserLogin userLogin, string mensagem)
        {
            return new UserLogin
            {
                Email = userLogin.Email,
                Autorizado = false,
                Mensage = mensagem
            };
        }


        private UserLogin Autorizado(int id, string mensagem)
        {
            Query query = new Query(@"SELECT ID, NOME, EMAIL, SENHA FROM Z_USUARIOS WHERE ID = :ID");
            query.AddParameter(new Parameter("ID", id));

            var result = query.Execute().FirstOrDefault();
            if (result == null)
                throw new Exception("Usuário não encontrado.");

            return new UserLogin
            {
                Id = id,
                Nome = result["NOME"]?.ToString() ?? string.Empty, 
                Email = result["EMAIL"]?.ToString() ?? string.Empty, 
                Senha = result["SENHA"]?.ToString() ?? string.Empty, 
                Autorizado = true,
                Mensage = mensagem
            };
        }
    }
}
