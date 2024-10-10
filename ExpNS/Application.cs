using MetacircularEval_CSharp.PairNS;
using MetacircularEval_CSharp.Procedures;
using static MetacircularEval_CSharp.Lazy.ThunkStatic;
using static MetacircularEval_CSharp.PairNS.Pair;

namespace MetacircularEval_CSharp.ExpNS
{
    public class Application : Exp
    {
        public override ExpType Type => ExpType.Application;

        public Exp Operate { get; }

        public Pair<Exp>? Args { get; }

        public Application(Exp operate)
        {
            Operate = operate;
            Args = null;
        }

        public Application(Exp operate, Pair<Exp> args)
        {
            Operate = operate;
            Args = args;
        }

        public override string ToString() =>

            Args == null ?
                $"({Operate})" :
                $"({Operate} {Args.ToString()[1..^1]})";

        public override object Eval(Environment env)
        {
            var proc = Operate.Eval(env);

            var args =
                    Args == null ?
                        null :
                        Map(Args, x => x.Eval(env));

            return
                proc is EvaluableProcedure p1 ?
                    Apply(p1, args) :

                    proc is Func<Pair<object>, object> p2 ?
                        Primitive(p2, args!) :

                        throw new InvalidCastException($"{proc} is not a procedure");
        }

        public override object LazyEval(Environment env)
        {
            var proc = ActualValue(Operate, env);

            if (proc is EvaluableProcedure p1)
            {
                var args =
                    Args == null ?
                        null :
                        Map(Args, x => Delay(x, env) as object);

                return LazyApply(p1, args);
            }
            else if (proc is Func<Pair<object>, object> p2)
            {
                var args =
                    Args == null ?
                        null :
                        Map(Args, x => ActualValue(x, env));

                return Primitive(p2, args);
            }
            else
            {
                throw new InvalidCastException($"{proc} is not a procedure");
            }
        }

        public static object Apply(EvaluableProcedure proc, Pair<object>? args) =>

            EvalSequence(proc.Body,
                         proc.Environment.Extend(proc.Params,
                                                 proc.ListParam,
                                                 args));


        public static object LazyApply(EvaluableProcedure proc, Pair<object>? args) =>

            LazyEvalSequence(proc.Body,
                             proc.Environment.Extend(proc.Params,
                                                     proc.ListParam,
                                                     args));

        /*
         *  这里要 return 一个函数：
         *
         *      给定一个环境，该函数将：
         *          ①执行一个过程，输入环境，得到可执行过程
         *          ②执行一个过程，输入环境，得到参数列表
         *          ③执行可执行过程：将可执行过程作用于参数列表，返回结果
         *
         *      对于给定的环境：
         *          第①步之后才能知道可执行过程是基本过程还是复合过程，
         *          因此第①步的函数的返回值必须在类型上能够区分
         *          同时第③步的执行函数必须对两种类型有两种重载
         *
         *      第①步获得的可执行过程必须是从环境中直接获得，不能在第①步执行时再进行Analyze，
         *          因为此刻是执行时而不是分析时，执行时不应该再进行分析工作。
         *          因此这个可执行过程必须是在分析Lambda表达式时已经生成好
         *
         *      对于基本过程，执行步骤是把基本过程函数作用于步骤②得到的参数列表
         *
         *      对于复合过程，执行步骤是把复合过程定义的参数和步骤②得到参数列表进行绑定
         *          然后扩展复合过程定义时的环境，加入这些绑定，得到一个临时的扩展环境
         *          再将扩展环境输入可执行过程，直接得到计算结果
         *      
         *      因此：①可执行基本过程的类型是 Func<args list, Object>
         *           ②可执行复合过程的类型是 Func<Environment, Object>
         *           ③分析Lambda表达式必须生成可执行复合过程实体和参数列表实体，以供过程调用的分析阶段使用
         */
        public override Func<Environment, object> Analyze()
        {
            var proc_getter = Operate.Analyze();
            var arg_getters = Map(Args, x => x.Analyze());

            return env =>
            {
                var proc = proc_getter(env);
                var args = Map(arg_getters, x => x(env));

                return
                    proc is ExecutableProcedure p1 ?
                        Execute(p1, args) :

                        proc is Func<Pair<object>, object> p2 ?
                            Primitive(p2, args!) :

                            throw new InvalidCastException($"{proc} is not a procedure");
            };
        }

        public override Func<Environment, object> LazyAnalyze()
        {
            var proc_getter = Operate.LazyAnalyze();
            var arg_getters = Map(Args, x => x.LazyAnalyze());

            return env =>
            {
                var proc = ActualValue(proc_getter, env);

                if (proc is ExecutableProcedure p1)
                {
                    var args = Map(arg_getters, x => Delay(x, env) as object);
                    return Execute(p1, args);
                }
                else if (proc is Func<Pair<object>, object> p2)
                {
                    var args = Map(arg_getters, x => ActualValue(x, env));
                    return Primitive(p2, args);
                }
                else
                {
                    throw new InvalidCastException($"{proc} is not a procedure");
                }
            };
        }

        public static object Execute(ExecutableProcedure proc, Pair<object>? args) =>

            proc.Body(proc.Environment.Extend(proc.Params,
                                              proc.ListParam,
                                              args));

        public static object Primitive(Func<Pair<object>, object> func, Pair<object>? args) => func(args!);
    }
}
