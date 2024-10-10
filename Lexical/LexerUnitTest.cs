namespace MetacircularEval_CSharp.Lexical
{
    public static class LexerUnitTest
    {
        private static Dictionary<byte, string> token_names = [];

        private static List<string> cases = [];

        static LexerUnitTest()
        {
            var fields =
                typeof(Token)
                    .GetFields()
                    .Where(p => p.IsPublic &&
                                p.IsStatic &&
                                p.FieldType == typeof(byte));

            foreach (var field in fields)
            {
                token_names.Add((byte)field.GetValue(null)!, field.Name);
            }

            cases.Add("");
            cases.Add(" ");
            cases.Add("\t");
            cases.Add("\r");
            cases.Add("\n");
            cases.Add("\r\n");
            cases.Add("(");
            cases.Add(")");
            cases.Add("\"");
            cases.Add("\"s");
            cases.Add("\"str\"");
            cases.Add("+");
            cases.Add("-");
            cases.Add("0");
            cases.Add("1");
            cases.Add("01");
            cases.Add("1.");
            cases.Add("01.");
            cases.Add("+1");
            cases.Add("+.");
            cases.Add("+.9");
            cases.Add("+.-");
            cases.Add(".");
            cases.Add(".0");
            cases.Add("..");
            cases.Add(".(");
            cases.Add(".)");
            cases.Add(". ");
            cases.Add(".'");
            cases.Add(".\"\"");
            cases.Add("5e");
            cases.Add("5E");
            cases.Add("6.e");
            cases.Add("6.1E");
            cases.Add("+6.1Ew");
            cases.Add("6.1E0");
            cases.Add("6.1E1");
            cases.Add("6.1E+");
            cases.Add("6.1E+0");
            cases.Add("6.1E+00");
            cases.Add("6.1E+00 ");
            cases.Add("6.1E+00w");
            cases.Add("6.1E+w");
            cases.Add("6.1E+ ");
        }

        public static void Test()
        {
            for (int i = 0; i < cases.Count; i++)
            {
                Console.WriteLine($"{nameof(LexerUnitTest)}:{i}".PadLeft(65, '-'));
                TestTokenize(cases[i]);
            }
        }

        public static void TestTokenize(string code)
        {
            try
            {
                Console.WriteLine(code);

                var lex = Lexer.Tokenize(code);

                Console.ForegroundColor = ConsoleColor.Green;

                while (lex.MoveNext())
                {
                    var token = lex.Current;
                    Console.WriteLine(token_names[token.Type].PadLeft(10, ' ') + " " + token.Value);
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(e.Message);

                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
