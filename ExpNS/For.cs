using MetacircularEval_CSharp.PairNS;
using static MetacircularEval_CSharp.Lazy.ThunkStatic;

namespace MetacircularEval_CSharp.ExpNS
{
    public class For(Exp initialize, Exp predicate, Exp next, Pair<Exp> body) : Exp
    {
        public override ExpType Type => ExpType.For;

        public Exp Initialize => initialize;

        public Exp Predicate => predicate;

        public Exp Next => next;

        public Pair<Exp> Body => body;

        public override object Eval(Environment env)
        {
            var loop_env = SetupEnv(env);
            Initialize.Eval(loop_env);

            while ((bool)(Predicate.Eval(loop_env)))
            {
                EvalSequence(Body, loop_env);
                Next.Eval(loop_env);
            }

            return "done";
        }

        public override object LazyEval(Environment env)
        {
            var loop_env = SetupEnv(env);
            ActualValue(Initialize, loop_env);

            while ((bool)(ActualValue(Predicate, loop_env)))
            {
                LazyEvalSequence(Body, loop_env);
                ActualValue(Next, loop_env);
            }

            return "done";
        }

        public override Func<Environment, object> Analyze()
        {
            var proc_initial = Initialize.Analyze();
            var proc_predicate = Predicate.Analyze();
            var proc_body = AnalyzeSequence(Body);
            var proc_next = Next.Analyze();

            return env =>
            {
                var loop_env = SetupEnv(env);
                proc_initial(loop_env);

                while ((bool)proc_predicate(loop_env))
                {
                    proc_body(loop_env);
                    proc_next(loop_env);
                }

                return "done";
            };
        }

        public override Func<Environment, object> LazyAnalyze()
        {
            var proc_initial = Initialize.LazyAnalyze();
            var proc_predicate = Predicate.LazyAnalyze();
            var proc_body = LazyAnalyzeSequence(Body);
            var proc_next = Next.LazyAnalyze();

            return env =>
            {
                var loop_env = SetupEnv(env);
                ActualValue(proc_initial, loop_env);

                while ((bool)ActualValue(proc_predicate, loop_env))
                {
                    proc_body(loop_env);
                    ActualValue(proc_next, loop_env);
                }

                return "done";
            };
        }

        public Environment SetupEnv(Environment env) =>

            Initialize is Assignment ?
                env :

            Initialize is Definition ?
                env.ExtendEmpty() :

                throw new Exception("INITIAL clause isn't def or set -- EVAL-FOR");

        public override string ToString() =>

            $"(for {Initialize} " +
                 $"{Predicate} " +
                 $"{Next} " +
                 $"{Body.ToString()[1..^1]})";
    }
}
