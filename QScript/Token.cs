using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEditor.QScript
{
    internal sealed class Token
    {
        public enum TokenType
        {
            Print,
            PrintIn,
            Set,

            Name,
            StringLiteral
        }

        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
