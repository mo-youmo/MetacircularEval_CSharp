using MetacircularEval_CSharp.PairNS;
using static MetacircularEval_CSharp.Lazy.ThunkStatic;
using static MetacircularEval_CSharp.PairNS.Pair;

namespace MetacircularEval_CSharp.ExpNS
{
    public abstract class Exp
    {
        public enum ExpType
        {
            SelfEval,
            Variable,
            Quoted,
            Assignment,
            Definition,
            If,
            Lambda,
            Begin,
            Cond,
            And,
            Or,
            Let,
            LetStar,
            For,
            Application
        }

        public abstract ExpType Type { get; }

        public abstract override string ToString();

        public abstract Func<Environment, object> Analyze();

        public abstract object Eval(Environment env);

        public abstract Func<Environment, object> LazyAnalyze();

        public abstract object LazyEval(Environment env);

        public static object EvalSequence(Pair<Exp> seq, Environment env)
        {
            if (seq.Snd == null)
            {
                return seq.Fst.Eval(env);
            }
            else
            {
                seq.Fst.Eval(env);
                return EvalSequence(seq.Snd, env);
            }
        }

        public static object LazyEvalSequence(Pair<Exp> seq, Environment env)
        {
            if (seq.Snd == null)
            {
                return seq.Fst.LazyEval(env);
            }
            else
            {
                ActualValue(seq.Fst, env);
                return LazyEvalSequence(seq.Snd, env);
            }
        }

        public static Func<Environment, object> AnalyzeSequence(Pair<Exp> seq)
        {
            if (seq.Snd == null)
            {
                return seq.Fst.Analyze();
            }
            else
            {
                var proc1 = seq.Fst.Analyze();
                var proc2 = AnalyzeSequence(seq.Snd);
                return env =>
                {
                    proc1(env);
                    return proc2(env);
                };
            }
        }

        public static Func<Environment, object> LazyAnalyzeSequence(Pair<Exp> seq)
        {
            if (seq.Snd == null)
            {
                return seq.Fst.LazyAnalyze();
            }
            else
            {
                var proc1 = seq.Fst.LazyAnalyze();
                var proc2 = LazyAnalyzeSequence(seq.Snd);
                return env =>
                {
                    ActualValue(proc1, env);
                    return proc2(env);
                };
            }
        }

        public static Pair<Exp> LetDefinitionFirst(Pair<Exp> body) =>

            Append(Filter(body, x => x is Definition),
                   Filter(body, x => x is not Definition))!;

        public static Exp Empty => new SelfEval("");

        public static Exp True => new Variable("true");

        public static Exp False => new Variable("false");

        public static Exp EqualTrue(Exp exp) => new Application(new Variable("="), Cons(exp, Cons(True)));
    }
}
