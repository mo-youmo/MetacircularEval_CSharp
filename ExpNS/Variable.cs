namespace MetacircularEval_CSharp.ExpNS
{
    public class Variable(string sym) : Exp
    {
        public override ExpType Type => ExpType.Variable;

        public string Symbol => sym;

        public override string ToString() => Symbol;

        public override object Eval(Environment env) => env.LookupValue(Symbol);

        public override object LazyEval(Environment env) => env.LookupValue(Symbol);

        public override Func<Environment, object> Analyze() => env => env.LookupValue(Symbol);

        public override Func<Environment, object> LazyAnalyze() => env => env.LookupValue(Symbol);
    }
}
