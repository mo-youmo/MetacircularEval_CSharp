namespace MetacircularEval_CSharp.ExpNS
{
    public class Definition(string variable, Exp value) : Exp
    {
        public override ExpType Type => ExpType.Definition;

        public string Variable => variable;

        public Exp Value => value;

        public override string ToString() => $"(define {Variable} {Value})";

        public override object Eval(Environment env) =>

            env.DefineVariable(Variable,
                               Value.Eval(env));

        public override object LazyEval(Environment env) =>
        
            env.DefineVariable(Variable,
                               Value.LazyEval(env));

        public override Func<Environment, object> Analyze()
        {
            var val_getter = Value.Analyze();

            return env => env.DefineVariable(Variable,
                                             val_getter(env));
        }

        public override Func<Environment, object> LazyAnalyze()
        {
            var val_getter = Value.LazyAnalyze();

            return env => env.DefineVariable(Variable,
                                             val_getter(env));
        }
    }
}
