using static MetacircularEval_CSharp.Lazy.ThunkStatic;

namespace MetacircularEval_CSharp.ExpNS
{
    public class If(Exp predicate, Exp consequent, Exp alternative) : Exp
    {
        public override ExpType Type => ExpType.If;

        public Exp Predicate { get; } = predicate;

        public Exp Consequent { get; } = consequent;

        public Exp Alternative { get; } = alternative;

        public If(Exp predicate, Exp consequent) : this(predicate, consequent, Empty) { }

        public override string ToString() =>

            Alternative.ToString() == "" ?
                $"(if {Predicate} {Consequent})" :
                $"(if {Predicate} {Consequent} {Alternative})";

        public override object Eval(Environment env) =>

            (bool)(Predicate.Eval(env)) ? Consequent.Eval(env) : Alternative.Eval(env);

        public override object LazyEval(Environment env) =>

            (bool)ActualValue(Predicate, env) ? Consequent.LazyEval(env) : Alternative.LazyEval(env);

        public override Func<Environment, object> Analyze()
        {
            var predicate = Predicate.Analyze();
            var consequent = Consequent.Analyze();
            var alternative = Alternative.Analyze();

            return env => (bool)predicate(env) ? consequent(env) : alternative(env);
        }

        public override Func<Environment, object> LazyAnalyze()
        {
            var predicate = Predicate.LazyAnalyze();
            var consequent = Consequent.LazyAnalyze();
            var alternative = Alternative.LazyAnalyze();

            return env => (bool)ActualValue(predicate, env) ? consequent(env) : alternative(env);
        }
    }
}
