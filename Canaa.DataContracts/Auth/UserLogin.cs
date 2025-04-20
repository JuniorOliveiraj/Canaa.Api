using System.Runtime.Serialization;

namespace Canaa.DataContracts.Auth
{
    
    [DataContract]
    public class UserLogin
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Senha { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public bool Autorizado { get; set; }
    }
}
