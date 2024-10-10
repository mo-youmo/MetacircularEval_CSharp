using MetacircularEval_CSharp.PairNS;
using static MetacircularEval_CSharp.Nil;
using static MetacircularEval_CSharp.PairNS.Pair;

namespace MetacircularEval_CSharp
{
    public partial class Environment
    {
        private Dictionary<string, object> kvps = new Dictionary<string, object>();

        private Environment? next = null;

        public object LookupValue(string var)
        {
            object? scanKvps(string var, Func<object> found, Func<object?> notFound) =>
                kvps.ContainsKey(var) ?
                    found() :
                    notFound();

            object? ret =
                 scanKvps(var,
                     () => kvps[var],
                     () => next?.LookupValue(var) ?? null);

            return
                ret == null ?
                    throw new Exception($"ERROR: Unbound Variable - {var}") :
                    ret;
        }

        public object SetValue(string var, object val)
        {
            if (kvps.ContainsKey(var))
            {
                kvps[var] = val;
            }
            else if (next != null)
            {
                return next.SetValue(var, val);
            }
            else
            {
                throw new Exception($"ERROR: Unbound Variable - {var}");
            }
            return "done";
        }

        public object DefineVariable(string var, object val)
        {
            if (kvps.ContainsKey(var))
            {
                kvps[var] = val;
            }
            else
            {
                kvps.Add(var, val);
            }
            return "done";
        }

        public Environment Extend(Pair<KeyValuePair<string, object>> kvps)
        {
            var dic =
                kvps == null ?
                    new Dictionary<string, object>() :
                    kvps.ToDictionary(kvps => kvps.Key,
                                      kvps => kvps.Value);

            return new Environment { kvps = dic, next = this };
        }

        public Environment Extend(Pair<string>? @params, string? list_param, Pair<object>? args)
        {
            var kvp = merge(@params, list_param, args);

            return
                kvp == null ?
                    ExtendEmpty() :
                    Extend(kvp);
        }

        private Pair<KeyValuePair<string, object>>? merge(Pair<string>? @params, string? list_param, Pair<object>? args) =>

            @params != null && args != null ?
                Cons(new KeyValuePair<string, object>(@params.Fst, args.Fst),
                     merge(@params.Snd, list_param, args.Snd)) :

            @params == null && list_param != null ?
                Cons(new KeyValuePair<string, object>(list_param, args == null ? nil : args)) :

            @params == null && list_param == null && args == null ?
                null :

                throw new ArgumentException(
                    $"number of arguments does not match the given number: " + "\r\n" +
                    $"@params=[{@params}]; list_param=[{list_param}]; args=[{args}]");

        public Environment ExtendEmpty() => new Environment { kvps = [], next = this };

        public static Environment MakeGlobalEnvironment()
        {
            Func<Pair<object>?, object> list = p => p == null ? nil : p;
            Func<Pair<object>, object> @null = p => p.Fst == null || p.Fst == nil;
            Func<Pair<object>, object> cons = p =>
            {
                object a = p.Fst;
                object b = p.Snd!.Fst;
                return b == nil ? Cons(a) : Cons(a, b);
            };
            Func<Pair<object>, object> car = p =>
            {
                var arg = p.Fst;
                return (arg as Pair)?.Fst ?? nil;
            };
            Func<Pair<object>, object> cdr = p =>
            {
                var arg = p.Fst;
                var ret = (arg as Pair)?.Snd;
                return ret == null ? nil : ret;
            };
            Func<Pair<object>, object> add = p => p.Aggregate(0d, (x, y) => (double)x + (double)y);
            Func<Pair<object>, object> mul = p => p.Aggregate(1d, (x, y) => (double)x * (double)y);
            Func<Pair<object>, object> sub = p => p.Snd == null ? -(double)p.Fst : (double)p.Fst - (double)p.Snd.Fst;
            Func<Pair<object>, object> div = p => (double)p.Fst / (double)p.Snd!.Fst;
            Func<Pair<object>, object> mod = p => (double)p.Fst % (double)p.Snd!.Fst;
            Func<Pair<object>, object> eq = p =>
            {
                var a = p.Fst; var b = p.Snd!.Fst;
                if (a is bool x && b is bool y) return x == y;
                if (a is string j && b is string k) return j == k;
                if (a is double m && b is double n) return m == n;
                return a.Equals(b);
            };
            Func<Pair<object>, object> gt = p => (double)p.Fst > (double)p.Snd!.Fst;
            Func<Pair<object>, object> lt = p => (double)p.Fst < (double)p.Snd!.Fst;
            Func<Pair<object>, object> display = p => { Console.Write(p.Fst); return default!; };
            Func<Pair<object>, object> newline = p => { Console.WriteLine(); return default!; };
            Func<Pair<object>, object> error = p => { throw new Exception(p.Fst.ToString()); };

            Func<Pair<object>?, object> print = p =>
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;

                var node = p;
                while (node != null)
                {
                    var content = node.Fst.ToString();
                    if (content != null)
                    {
                        foreach (var line in content.Split("\r\n"))
                        {
                            Console.WriteLine(": " + line);
                        }
                    }
                    node = node.Snd;
                }
                Console.ForegroundColor = color;
                return default!;
            };

            var env = new Environment();

            env.DefineVariable("true", true);
            env.DefineVariable("false", false);
            env.DefineVariable("#t", true);
            env.DefineVariable("#f", false);
            env.DefineVariable("nil", nil);
            env.DefineVariable("list", list);
            env.DefineVariable("null?", @null);
            env.DefineVariable("cons", cons);
            env.DefineVariable("car", car);
            env.DefineVariable("cdr", cdr);
            env.DefineVariable("+", add);
            env.DefineVariable("-", sub);
            env.DefineVariable("*", mul);
            env.DefineVariable("/", div);
            env.DefineVariable("%", mod);
            env.DefineVariable("=", eq);
            env.DefineVariable(">", gt);
            env.DefineVariable("<", lt);
            env.DefineVariable("display", display);
            env.DefineVariable("newline", newline);
            env.DefineVariable("error", error);
            env.DefineVariable("print", print);

            return env;
        }
    }
}
