using MetacircularEval_CSharp.PairNS;

namespace MetacircularEval_CSharp.ExpNS
{
    public class And : Exp
    {
        public override ExpType Type => ExpType.And;

        public Pair<Exp>? Predicates { get; }

        public And() => Predicates = null;

        public And(Pair<Exp> predicates) => Predicates = predicates;

        public static Exp ExpandAnd(Pair<Exp>? predicates) =>

            predicates == null ?
                True :

                predicates.Snd == null ?
                    EqualTrue(predicates.Fst) :

                    new If(EqualTrue(predicates.Fst),
                           ExpandAnd(predicates.Snd),
                           False);

        public Exp ToIf() => ExpandAnd(Predicates);

        public override string ToString() =>

            Predicates == null ?
                $"(and)" :
                $"(and {Predicates.ToString()[1..^1]})";

        public override object Eval(Environment env) => ToIf().Eval(env);

        public override object LazyEval(Environment env) => ToIf().LazyEval(env);

        public override Func<Environment, object> Analyze() => ToIf().Analyze();

        public override Func<Environment, object> LazyAnalyze() => ToIf().LazyAnalyze();
    }
}
