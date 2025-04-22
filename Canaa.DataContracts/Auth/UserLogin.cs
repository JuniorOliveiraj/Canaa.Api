using System.Runtime.Serialization;
using System.Runtime.Serialization.DataContracts;

namespace Canaa.DataContracts.Auth
{
    
    [DataContract]
    public class UserLogin
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Nome { get; set; }
        [DataMember]
        public string Senha { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string NomeUsuario { get; set; }

        [DataMember]
        public bool Autorizado { get; set; }

        [DataMember]
        public string Mensage { get; set; }
    }
}
