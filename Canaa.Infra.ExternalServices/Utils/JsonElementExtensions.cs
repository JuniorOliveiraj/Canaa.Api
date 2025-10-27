using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Canaa.Infra.ExternalServices.Utils
{
    public static class JsonElementExtensions
    {
        public static string? GetPropertyOrDefault(this JsonElement element, string propertyName)
        {
            if (element.TryGetProperty(propertyName, out JsonElement value))
            {
                return value.ValueKind == JsonValueKind.String ? value.GetString() : null;
            }
            return null;
        }
    }
}
