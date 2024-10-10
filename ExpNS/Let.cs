using MetacircularEval_CSharp.PairNS;
using static MetacircularEval_CSharp.Lazy.ThunkStatic;
using static MetacircularEval_CSharp.PairNS.Pair;

namespace MetacircularEval_CSharp.ExpNS
{
    public partial class Let : Exp
    {
        public override ExpType Type => ExpType.Let;

        public Pair<LetBinding>? Bindings { get; }

        public Pair<Exp> Body { get; }

        public Let(Pair<Exp> body)
        {
            Bindings = null;
            Body = body;
        }

        public Let(Pair<LetBinding> bindings, Pair<Exp> body)
        {
            Bindings = bindings;
            Body = body;
        }

        public override string ToString()
        {
            var bindings =
                Bindings == null ?
                    $"()" :
                    $"{Bindings}";

            return $"(let {bindings} {Body.ToString()[1..^1]})";
        }

        public override object Eval(Environment env) =>

            Bindings == null ?
                EvalSequence(Body, env) :
                EvalSequence(Body, env.Extend(Map(Bindings, b => b.Variable),
                                              null,
                                              Map(Bindings, b => b.Value.Eval(env))));

        public override object LazyEval(Environment env) =>

            Bindings == null ?
                LazyEvalSequence(Body, env) :
                LazyEvalSequence(Body, env.Extend(Map(Bindings, b => b.Variable),
                                              null,
                                              Map(Bindings, b => Delay(b.Value, env) as object)));

        public override Func<Environment, object> Analyze()
        {
            return
                Bindings == null ?
                    AnalyzeSequence(Body) :
                    AnalyzeLetWithBindings();

            Func<Environment, object> AnalyzeLetWithBindings()
            {
                var @params = Map(Bindings, b => b.Variable);
                var val_getters = Map(Bindings, b => b.Value.Analyze());
                var proc_args = (Environment env) => Map(val_getters, x => x(env));
                var proc_body = AnalyzeSequence(Body);

                return env => proc_body(env.Extend(@params,
                                                   null,
                                                   proc_args(env)));
            }
        }

        public override Func<Environment, object> LazyAnalyze()
        {
            return
                Bindings == null ?
                    LazyAnalyzeSequence(Body) :
                    LazyAnalyzeLetWithBindings();

            Func<Environment, object> LazyAnalyzeLetWithBindings()
            {
                var @params = Map(Bindings, b => b.Variable);
                var val_getters = Map(Bindings, b => b.Value.LazyAnalyze());
                var proc_args = (Environment env) => Map(val_getters, x => x(env));
                var proc_body = LazyAnalyzeSequence(Body);

                return env => proc_body(env.Extend(@params,
                                                   null,
                                                   proc_args(env)));
            }
        }
    }
}
