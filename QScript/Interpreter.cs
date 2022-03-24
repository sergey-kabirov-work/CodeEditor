using System;
using System.Collections.Generic;
using System.Windows;

namespace CodeEditor.QScript
{
    internal class Interpreter
    {
        // Scanner: converts text to array of words
        private static string[] Scan(string text)
        {
            List<string> words = new List<string>();

            string current = "";
            int i = 0;
            foreach (char ch in text)
            {
                // add word
                if (char.IsWhiteSpace(ch)  && current != string.Empty)
                {
                    words.Add(current);
                    current = "";
                }
                else if(i + 1 == text.Length && current != string.Empty)
                {
                    words.Add(current + ch);
                    current = "";
                }
                else if (!char.IsWhiteSpace(ch))
                    current += ch;

                i++;
            }

            return words.ToArray();
        }

        // Lexer: converts words to tokens
        private static Token[] Lex(string[] words)
        {
            List<Token> tokens = new List<Token>();

            foreach (string word in words)
            {
                //print command
                if (word == "PRINT")
                    tokens.Add(new Token(Token.TokenType.Print, word));
                //set command
                if (word == "SET")
                    tokens.Add(new Token(Token.TokenType.Set, word));
                //string literal
                else if (word.Length >= 2 && word[0] == '\'' && word[word.Length - 1] == '\'')
                    tokens.Add(new Token(Token.TokenType.StringLiteral, word.Replace("'", "")));
                // name
                else if(!String.IsNullOrEmpty(word))
                    tokens.Add(new Token(Token.TokenType.Name, word));
                //unexpected token (not whitespace)
                else
                    Console.WriteLine("Unexpected token: " + word);
            }

            return tokens.ToArray();
        }

        // Parser: converts tokens to AST
        private static ProgramAst? Parse(Token[] tokens)
        {
            ProgramAst program = new ProgramAst();

            if (tokens.Length == 0)
                return null;

            int cursor;
            for (cursor = 0; cursor < tokens.Length; cursor++)
            {
                Token current = tokens[cursor];

                // Print statement
                if (current.Type == Token.TokenType.Print)
                {
                    // next is exists ?
                    if (cursor + 1 < tokens.Length)
                    {
                        Token printVal = tokens[cursor + 1];
                        // next is string ?
                        if (printVal.Type == Token.TokenType.StringLiteral)
                        {
                            cursor++;
                            program.Statements.Add(new PrintAst(printVal));
                        }
                        else
                            Console.WriteLine("Expected string after print, but got token: " + current.Type);
                    }
                }
                // Set statement
                else if(current.Type == Token.TokenType.Set)
                {
                    // next 2 tokens is exists ?
                    if (cursor + 2 < tokens.Length)
                    {
                        Token setName = tokens[cursor + 1];
                        Token setVal = tokens[cursor + 2];

                        // next is word
                        if (setName.Type == Token.TokenType.Name && setVal.Type == Token.TokenType.StringLiteral)
                        {
                            cursor += 2;
                            program.Statements.Add(new SetAst(setName, setVal));
                        }
                        else
                            Console.WriteLine("Expected name of variable and value, but got token: " + current.Type);
                    }
                }
                else
                    Console.WriteLine("Unexpected token: '" + current.Type + "'");
            }

            return program;
        }

        public static void Run(string code)
        {
            string[] words = Scan(code);
            Token[] tokens = Lex(words);
            ProgramAst? program = Parse(tokens);

            if (program != null)
            {
                foreach (Token token in tokens)
                    Console.WriteLine($"Token: [{token.Type}]");

                //launch
                GlobalStorage.Reset();
                program.Eval();
            }
            else
                MessageBox.Show("Error in syntax", "QScript");
        }
    }
}