using Canaa.AppHost.utils.Query;
using Canaa.DataContracts.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.FN.BusinessComponents.Auth
{
    public class CriarConta : ICriarConta
    {
        public UserLogin Create(UserLogin UserLogin)
        {
            if (VerificaEmail(UserLogin.Email))
                return new UserLogin { Mensage = "Email já cadastrado", Autorizado = false };

            if (!string.IsNullOrEmpty(UserLogin.NomeUsuario) && VerificaNome(UserLogin.NomeUsuario))
                return new UserLogin { Mensage = string.IsNullOrEmpty(UserLogin.NomeUsuario) ? "Nome de usuario é obrigatorio" : "Já existe um usuário com esse nome", Autorizado = false };


            var query = new Query(@"INSERT INTO Z_USUARIOS (NOME, EMAIL, SENHA, APELIDO) VALUES (:NOME, :EMAIL, :SENHA, :APELIDO) RETURNING ID INTO :ID");
            query.AddParameter(new Parameter("APELIDO", UserLogin.NomeUsuario));
            query.AddParameter(new Parameter("EMAIL", UserLogin.Email));
            query.AddParameter(new Parameter("SENHA", UserLogin.Senha));
            query.AddParameter(new Parameter("NOME", UserLogin.Nome
                ));
            var result = query.Execute().FirstOrDefault();

            int id;
            if (result != null && int.TryParse(result["ID"].ToString(), out id))
            {
                return new UserLogin
                {
                    Id = id,
                    NomeUsuario = UserLogin.NomeUsuario,
                    Email = UserLogin.Email,
                    Senha = UserLogin.Senha,
                    Autorizado = true
                };
            }

            throw new Exception("Erro ao criar conta. ID não foi retornado.");
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
    }
}
