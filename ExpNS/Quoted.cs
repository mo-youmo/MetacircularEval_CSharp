namespace MetacircularEval_CSharp.ExpNS
{
    public class Quoted(object content) : Exp
    {
        public override ExpType Type => ExpType.Quoted;

        public object Content => content;

        public override string ToString() => $"'{Content}";

        public override object Eval(Environment env) => Content;

        public override object LazyEval(Environment env) => Content;

        public override Func<Environment, object> Analyze() => env => Content;

        public override Func<Environment, object> LazyAnalyze() => env => Content;
    }
}
