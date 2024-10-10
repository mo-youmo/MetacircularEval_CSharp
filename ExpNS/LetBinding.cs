namespace MetacircularEval_CSharp.ExpNS
{
    public class LetBinding(string variable, Exp value)
    {
        public string Variable => variable;

        public Exp Value => value;

        public override string ToString() => $"({Variable} {Value})";
    }
}
