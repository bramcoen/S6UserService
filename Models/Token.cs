using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Token
    {
        public Token(string value, DateTime validUntil)
        {
            Value = value;
            ValidUntil = validUntil;
        }
        public string Value { get; init; }
        public DateTime ValidUntil { get; init; }
    }
}
