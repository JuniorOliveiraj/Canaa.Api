using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Canaa.DataContracts.Whatsapp
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Mediatype
    {
        image,
        video
    }

}
