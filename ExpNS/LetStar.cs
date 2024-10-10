using MetacircularEval_CSharp.PairNS;

namespace MetacircularEval_CSharp.ExpNS
{
    public class LetStar : Exp
    {
        public override ExpType Type => ExpType.LetStar;

        public Pair<LetBinding>? Bindings { get; }

        public Pair<Exp> Body { get; }

        public LetStar(Pair<Exp> body)
        {
            Bindings = null;
            Body = body;
        }

        public LetStar(Pair<LetBinding> bindings, Pair<Exp> body)
        {
            Bindings = bindings;
            Body = body;
        }

        private static Exp nested_let(Pair<LetBinding>? bindings, Pair<Exp> body) =>

            bindings == null ?
                new Let(body) :

                bindings.Snd == null ?
                    new Let(bindings, body) :

                    new Let(new Pair<LetBinding>(bindings.Fst),
                            new Pair<Exp>(nested_let(bindings.Snd, body)));

        public Exp ToLet() => nested_let(Bindings, Body);

        public override string ToString()
        {
            var bindings =
                Bindings == null ?
                    $"()" :
                    $"{Bindings}";

            return $"(let* {bindings} {Body.ToString()[1..^1]})";
        }

        public override object Eval(Environment env) => ToLet().Eval(env);

        public override object LazyEval(Environment env) => ToLet().LazyEval(env);

        public override Func<Environment, object> Analyze() => ToLet().Analyze();

        public override Func<Environment, object> LazyAnalyze() => ToLet().LazyAnalyze();
    }
}
