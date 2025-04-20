using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.DataContracts.Auth
{
 
    [DataContract]
    public class UsuarioLogado
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Senha { get; set; }

        [DataMember]
        public string Email { get; set; }

    }
}
