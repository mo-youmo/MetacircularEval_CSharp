namespace MetacircularEval_CSharp.Lazy
{
    public class Thunk<T>(T delayed, Environment env, Func<T, Environment, object> get_value)
    {
        public T? Delayed = delayed;

        public Environment? Env = env;

        public object? Value;

        public bool Evaluated = false;

        public Func<T, Environment, object> GetValue = get_value;
    }
}
