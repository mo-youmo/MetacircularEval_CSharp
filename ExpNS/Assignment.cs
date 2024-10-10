namespace MetacircularEval_CSharp.ExpNS
{
    public class Assignment(string variable, Exp value) : Exp
    {
        public override ExpType Type => ExpType.Assignment;

        public string Variable { get; } = variable;

        public Exp Value { get; } = value;

        public override string ToString() => $"(set! {Variable} {Value})";

        public override object Eval(Environment env) =>

            env.SetValue(Variable,
                         Value.Eval(env));

        public override object LazyEval(Environment env) =>

            env.SetValue(Variable,
                         Value.LazyEval(env));

        public override Func<Environment, object> Analyze()
        {
            var val_getter = Value.Analyze();

            return env => env.SetValue(Variable,
                                       val_getter(env));
        }

        public override Func<Environment, object> LazyAnalyze()
        {
            var val_getter = Value.LazyAnalyze();

            return env => env.SetValue(Variable,
                                       val_getter(env));
        }
    }
}
