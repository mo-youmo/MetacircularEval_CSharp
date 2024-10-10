using MetacircularEval_CSharp.PairNS;

namespace MetacircularEval_CSharp.ExpNS
{
    public class Begin : Exp
    {
        public override ExpType Type => ExpType.Begin;

        public Pair<Exp>? Actions { get; }

        public Begin() => Actions = null;

        public Begin(Pair<Exp> actions) => Actions = actions;

        public override string ToString() =>

            Actions == null ?
                $"(begin)" :
                $"(begin {Actions.ToString()[1..^1]})";

        public override object Eval(Environment env) =>

            Actions == null ?
                Empty :
                EvalSequence(Actions, env);

        public override object LazyEval(Environment env) =>

            Actions == null ?
                Empty :
                LazyEvalSequence(Actions, env);

        public override Func<Environment, object> Analyze()
        {
            if (Actions == null)
            {
                return env => Empty;
            }
            else
            {
                var proc = AnalyzeSequence(Actions);
                return env => proc(env);
            }
        }

        public override Func<Environment, object> LazyAnalyze()
        {
            if (Actions == null)
            {
                return env => Empty;
            }
            else
            {
                var proc = LazyAnalyzeSequence(Actions);
                return env => proc(env);
            }
        }
    }
}
