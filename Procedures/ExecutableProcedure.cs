using MetacircularEval_CSharp.PairNS;

namespace MetacircularEval_CSharp.Procedures
{
    public class ExecutableProcedure(Pair<string>? @params, string? list_param, Func<Environment, object> body, Environment env)
    {
        public Pair<string>? Params { get; } = @params;

        public string? ListParam { get; } = list_param;

        public Func<Environment, object> Body { get; } = body;

        public Environment Environment { get; } = env;
    }
}
