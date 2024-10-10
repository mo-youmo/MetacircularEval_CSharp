using MetacircularEval_CSharp.ExpNS;
using MetacircularEval_CSharp.Lexical;
using MetacircularEval_CSharp.PairNS;
using MetacircularEval_CSharp.ParserNS;
using static MetacircularEval_CSharp.ExpNS.Exp;
using static MetacircularEval_CSharp.Lazy.ThunkStatic;

namespace MetacircularEval_CSharp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Func<Pair<Exp>, Func<Environment, object>>

                Eval = program => env => EvalSequence(program, env);

            Func<Pair<Exp>, Func<Environment, object>>

                LazyEval = program => env => ActualValue<Exp>(LazyEvalSequence(program, env));

            Func<Pair<Exp>, Func<Environment, object>>

                Analyze = program => env => AnalyzeSequence(program)(env);

            Func<Pair<Exp>, Func<Environment, object>>

                LazyAnalyze = program => env => ActualValue<Func<Environment,object>>(LazyAnalyzeSequence(program)(env));

            LexerUnitTest.Test();
            
            ParserUnitTest.Test();
            
            PairUnitTest.Test();
            
            RunnerTest(Eval,nameof(Eval));
            
            RunnerTest(LazyEval, nameof(LazyEval));
            
            RunnerTest(Analyze, nameof(Analyze));
            
            RunnerTest(LazyAnalyze, nameof(LazyAnalyze));
        }

        private static void RunnerTest(Func<Pair<Exp>, Func<Environment, object>> runner,string runner_name)
        {
            Environment env = Environment.MakeGlobalEnvironment();

            List<string> Cases = ProgramTest.Cases;

            for (int i = 0; i < Cases.Count; i++)
            {
                var codes = Cases[i];

                try
                {
                    Console.WriteLine($"{runner_name}:{i}".PadLeft(65, '-'));
                    Console.WriteLine(codes);

                    var program = Parser.Parse(Lexer.Tokenize(codes));

                    if (program != null)
                    {
                        var ret = runner(program)(env);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine(": " + ret);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    foreach (var line in ex.Message.Split("\r\n"))
                    {
                        Console.WriteLine(": " + line);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
