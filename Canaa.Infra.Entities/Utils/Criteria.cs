using System;
using System.Collections.Generic;

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

        public Criteria(string propertyName, object value)
            : this(propertyName, ComparisonOperator.Equals, value) { }

        public Criteria(string propertyName, ComparisonOperator op, object value)
        {
            PropertyName = propertyName;
            Operator = op;
            Value = value;
        }
    }

    public class CriteriaGroup
    {
        private readonly List<Criteria> _criterias = new();

        public CriteriaGroup(string propertyName, object value)
        {
            _criterias.Add(new Criteria(propertyName, value));
        }

        public CriteriaGroup AddParameter(string propertyName, object value)
        {
            _criterias.Add(new Criteria(propertyName, value));
            return this;
        }

        public CriteriaGroup AddParameter(string propertyName, ComparisonOperator op, object value)
        {
            _criterias.Add(new Criteria(propertyName, op, value));
            return this;
        }

        public Criteria[] ToArray() => _criterias.ToArray();
    }
}
