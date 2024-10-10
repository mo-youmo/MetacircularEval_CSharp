using MetacircularEval_CSharp.PairNS;
using MetacircularEval_CSharp.Procedures;

namespace MetacircularEval_CSharp.ExpNS
{
    public class Lambda(Pair<Exp> body) : Exp
    {
        public override ExpType Type => ExpType.Lambda;

        public Pair<string>? Params { get; } = null;

        public string? ListParam { get; } = null;

        public Pair<Exp> Body { get; } = LetDefinitionFirst(body);

        public Lambda(Pair<string>? @params, string? list_param, Pair<Exp> body) : this(body)
        {
            Params = @params;
            ListParam = list_param;
        }

        public override string ToString()
        {
            var @params =
                Params != null && ListParam != null ?
                    $"({Params.ToString()[1..^1]} . {ListParam})" :

                    Params != null && ListParam == null ?
                        $"{Params}" :

                        Params == null && ListParam != null ?
                            ListParam :

                            $"()";

            return $"(λ {@params} {Body.ToString()[1..^1]})";
        }

        public override object Eval(Environment env) =>

            new EvaluableProcedure(Params,
                                   ListParam,
                                   Body,
                                   env);

        public override object LazyEval(Environment env) =>

            new EvaluableProcedure(Params,
                                   ListParam,
                                   Body,
                                   env);

        public override Func<Environment, object> Analyze() =>

            env => new ExecutableProcedure(Params,
                                           ListParam,
                                           AnalyzeSequence(Body),
                                           env);

        public override Func<Environment, object> LazyAnalyze() =>

            env => new ExecutableProcedure(Params,
                                           ListParam,
                                           LazyAnalyzeSequence(Body),
                                           env);
    }
}
