namespace MetacircularEval_CSharp.ExpNS
{
    public class SelfEval(object obj) : Exp
    {
        public override ExpType Type => ExpType.SelfEval;

        public object Value => obj;

        public override string ToString() => $"{Value}";

        public override object Eval(Environment env) => Value;

        public override object LazyEval(Environment env) => Value;

        public override Func<Environment, object> Analyze() => env => Value;

        public override Func<Environment, object> LazyAnalyze() => env => Value;
    }
}
