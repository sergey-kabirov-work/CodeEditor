using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CodeEditor.QScript
{
    internal abstract class ASTNode
    {
        public abstract void Eval();
    }

    internal sealed class ProgramAst : ASTNode
    {
        public List<ASTNode> Statements { get; set; }

        public ProgramAst()
        {
            Statements = new List<ASTNode>();
        }

        public override void Eval()
        {
            foreach (ASTNode statement in Statements)
                statement.Eval();
        }
    }

    internal sealed class PrintAst : ASTNode
    {
        public Token Value { get; }

        public PrintAst(Token value)
        {
            Value = value;
        }

        public override void Eval()
        {
            MessageBox.Show(Value.Value.Replace("\\w", " "), "QScript");
        }
    }

    internal sealed class SetAst : ASTNode
    {
        public Token Name { get; }
        public Token Value { get; }

        public SetAst(Token name, Token val)
        {
            Name = name;
            Value = val;
        }

        public override void Eval()
        {
            GlobalStorage.Set(Name.Value, Value.Value);
        }
    }
}
