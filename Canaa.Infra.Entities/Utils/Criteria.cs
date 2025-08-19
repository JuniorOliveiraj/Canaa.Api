using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Canaa.Infra.Entities.Utils
{
    public enum ComparisonOperator
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        Contains,
        StartsWith
    }

    public class Criteria
    {
        public string PropertyName { get; set; }
        public ComparisonOperator Operator { get; set; }
        public object Value { get; set; }

        // Construtor para igualdade (o seu caso original)
        public Criteria(string propertyName, object value)
            : this(propertyName, ComparisonOperator.Equals, value) { }

        // Construtor para outros operadores
        public Criteria(string propertyName, ComparisonOperator op, object value)
        {
            PropertyName = propertyName;
            Operator = op;
            Value = value;
        }
    }
}
