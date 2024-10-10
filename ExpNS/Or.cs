using MetacircularEval_CSharp.PairNS;

namespace MetacircularEval_CSharp.ExpNS
{
    public class Or : Exp
    {
        public Pair<Exp>? Predicates { get; }

        public Or() => Predicates = null;

        public Or(Pair<Exp> predicates) => Predicates = predicates;

        public override ExpType Type => ExpType.Or;

        public static Exp ExpandOr(Pair<Exp>? predicates) =>

            predicates == null ?
                False :

                predicates.Snd == null ?
                    EqualTrue(predicates.Fst) :

                    new If(EqualTrue(predicates.Fst),
                           True,
                           ExpandOr(predicates.Snd));

        public Exp ToIf() => ExpandOr(Predicates);

        public override string ToString() =>

            Predicates == null ?
                $"(or)" :
                $"(or {Predicates.ToString()[1..^1]})";


        public override object Eval(Environment env) => ToIf().Eval(env);

        public override object LazyEval(Environment env) => ToIf().LazyEval(env);

        public override Func<Environment, object> Analyze() => ToIf().Analyze();

        public override Func<Environment, object> LazyAnalyze() => ToIf().LazyAnalyze();
    }
}
