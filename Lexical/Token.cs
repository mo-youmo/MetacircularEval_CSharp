namespace MetacircularEval_CSharp.Lexical
{
    public class Token
    {
        public const byte NUM = 0;
        public const byte SYM = 1;
        public const byte DOT = 2;
        public const byte STR = 3;
        public const byte LPAREN = 4;
        public const byte RPAREN = 5;
        public const byte SQUOTE = 6;

        public const byte SET = 7;
        public const byte DEFINE = 8;
        public const byte IF = 9;
        public const byte LAMBDA = 10;
        public const byte BEGIN = 11;
        public const byte COND = 12;
        public const byte AND = 13;
        public const byte OR = 14;
        public const byte LET = 15;
        public const byte LETSTAR = 16;
        public const byte FOR = 17;
        public const byte QUOTE = 18;
        public const byte ELSE = 19;

        private static readonly Dictionary<string, byte> reserved = [];

        private static readonly Dictionary<byte, string> token_names = [];

        static Token()
        {
            reserved.Add("quote", QUOTE);
            reserved.Add("set!", SET);
            reserved.Add("define", DEFINE);
            reserved.Add("if", IF);
            reserved.Add("lambda", LAMBDA);
            reserved.Add("begin", BEGIN);
            reserved.Add("cond", COND);
            reserved.Add("and", AND);
            reserved.Add("or", OR);
            reserved.Add("let", LET);
            reserved.Add("let*", LETSTAR);
            reserved.Add("for", FOR);
            reserved.Add("else", ELSE);

            var fields = typeof(Token)
                .GetFields()
                .Where(p => p.IsPublic &&
                            p.IsStatic &&
                            p.FieldType == typeof(byte));

            foreach (var field in fields)
            {
                token_names.Add((byte)field.GetValue(null)!, field.Name);
            }
        }

        public byte Type;

        public string Value;

        public string TypeName => GetTypeName(Type);

        public Token(byte type, string value)
        {
            Type = type == SYM && reserved.ContainsKey(value) ? reserved[value] : type;
            Value = value;
        }

        public static string GetTypeName(byte type) => token_names[type];
    }
}
