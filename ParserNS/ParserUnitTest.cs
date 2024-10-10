using MetacircularEval_CSharp.Lexical;

namespace MetacircularEval_CSharp.ParserNS
{
    public class ParserUnitTest
    {
        private static List<string> cases = [];

        static ParserUnitTest()
        {
            cases.Add("1");
            cases.Add("x");
            cases.Add("\"y\"");
            cases.Add("'x");
            cases.Add("'()");
            cases.Add("''x");
            cases.Add("''()");
            cases.Add("'''3");
            cases.Add("'(())");
            cases.Add("'(x)");
            cases.Add("'('x)");
            cases.Add("'(() 3)");
            cases.Add("'(''x (x y))");
            cases.Add("'(x . (y))");
            cases.Add("(quote x)");
            cases.Add("(quote ())");
            cases.Add("(quote 'x)");
            cases.Add("(quote '())");
            cases.Add("(quote ''3)");
            cases.Add("(quote (()))");
            cases.Add("(quote (x))");
            cases.Add("(quote ('x))");
            cases.Add("(quote (quote x))");
            cases.Add("(quote (() 3))");
            cases.Add("(quote (''x (x y)))");
            cases.Add("(quote ('(quote x) . (x . y)))");
            cases.Add("(set! x 3)");
            cases.Add("(define x 1)");
            cases.Add("(define (f) 3.14159)");
            cases.Add("(define (f x) (* x x))");
            cases.Add("(define (f1 . z) (sum z))");
            cases.Add("(define f2 (lambda z (sum z)))");
            cases.Add("(define (f3 x y . z) (+ x y (sum z)))");
            cases.Add("(define f4 (lambda (x y . z) (+ x y (sum z))))");
            cases.Add("(if (< x 0) -1 1)");
            cases.Add("(if (< x 0) -1)");
            cases.Add("(lambda () 1 2 3)");
            cases.Add("(lambda (x) (* x x))");
            cases.Add("(lambda (x y) (* x y))");
            cases.Add("(begin 1 2 3)");
            cases.Add("(begin)");
            cases.Add("(cond)");
            cases.Add("(cond (else 1 2 3))");
            cases.Add("(cond (1))");
            cases.Add("(cond ((< x 0) 1))");
            cases.Add("(cond ((< x 0) 1 2) (1))");
            cases.Add("(cond ((< x 0) 1 2 3) (1) (else 1 2 3))");
            cases.Add("(and)");
            cases.Add("(and 1)");
            cases.Add("(and 1 (< 1 0))");
            cases.Add("(or)");
            cases.Add("(or 1)");
            cases.Add("(or (> 2 0) 1)");
            cases.Add("(let () 0)");
            cases.Add("(let () 1 2 3)");
            cases.Add("(let ((a 0)) a)");
            cases.Add("(let ((a 0)) a 0)");
            cases.Add("(let ((a 0) (b 1)) (+ a b))");
            cases.Add("(let ((a 0) (b 1)) 1 (+ a b))");
            cases.Add("(let* () 0)");
            cases.Add("(let* () 1 2 3)");
            cases.Add("(let* ((a 0)) a)");
            cases.Add("(let* ((a 0)) a 0)");
            cases.Add("(let* ((a 0) (b 1)) (+ a b))");
            cases.Add("(let* ((a 0) (b 1)) 1 (+ a b))");
            cases.Add("(for (define j 0) (< j 101) (set! j (+ j 1)) (set! sum2 (+ sum2 j)))");
            cases.Add("(for (set! j 0) (< j 101) (set! j (+ j 1)) (set! sum2 (+ sum2 j)))");
            cases.Add("(f)");
            cases.Add("(f 0)");
            cases.Add("(f 0 (f 0))");
            cases.Add("(+)");
        }

        public static void Test()
        {
            for (int i = 0; i < cases.Count; i++)
            {
                var codes = cases[i];

                Console.WriteLine($"{nameof(ParserUnitTest)}:{i}".PadLeft(65, '-'));

                LexerUnitTest.TestTokenize(codes);

                TestParser(codes);
            }
        }

        public static void TestParser(string codes)
        {
            try
            {
                var lex = Lexer.Tokenize(codes);

                var exps = Parser.Parse(lex);

                if (exps != null)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;

                    foreach (var exp in exps)
                    {
                        Console.WriteLine($"{exp.GetType().Name}: {exp}");
                    }
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(e.Message);

                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
