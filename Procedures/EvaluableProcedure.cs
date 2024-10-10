using MetacircularEval_CSharp.ExpNS;
using MetacircularEval_CSharp.PairNS;

namespace MetacircularEval_CSharp.Procedures
{
    public class EvaluableProcedure(Pair<string>? @params, string? list_param, Pair<Exp> body, Environment env)
    {
        public Pair<string>? Params { get; } = @params;

        public string? ListParam { get; } = list_param;

        public Pair<Exp> Body { get; } = body;

        public Environment Environment { get; } = env;
    }
}
