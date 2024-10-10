using MetacircularEval_CSharp.ExpNS;

namespace MetacircularEval_CSharp.Lazy
{
    public class ThunkStatic
    {
        public static Thunk<Exp> Delay(Exp exp, Environment env)

            => new(exp, env, (exp, env) => exp.LazyEval(env));

        public static Thunk<Func<Environment, object>> Delay(Func<Environment, object> proc, Environment env)

            => new(proc, env, (proc, env) => proc(env));

        public static object ActualValue(Exp exp, Environment env) =>

            ActualValue<Exp>(exp.LazyEval(env));

        public static object ActualValue(Func<Environment, object> proc, Environment env) =>

            ActualValue<Func<Environment, object>>(proc(env));

        public static object ActualValue<T>(Thunk<T> thunk)
        {
            if (thunk.Evaluated)
            {
                return thunk.Value!;
            }
            else
            {
                thunk.Value = ActualValue<T>(thunk.GetValue(thunk.Delayed!, thunk.Env!));
                thunk.Evaluated = true;
                return thunk.Value;
            }
        }

        public static object ActualValue<T>(object obj) =>

            obj is Thunk<T> thunk ?
                ActualValue<T>(thunk) :
                obj;
    }
}
