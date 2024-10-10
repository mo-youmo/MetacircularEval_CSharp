namespace MetacircularEval_CSharp
{
    public static class ProgramTest
    {
        public static List<string> Cases = [];

        static ProgramTest()
        {
            Cases.Add("(print)                                 " + "\r\n" +
                      "(print \"1 2 3\")                       " + "\r\n" +
                      "(print \"1 2 3\" \"4 5 6\")             " + "\r\n" +
                      "(print (cons nil nil))                  " + "\r\n" +
                      "(print (cons nil 1))                    " + "\r\n" +
                      "(print (cons nil (cons 1 2)))           " + "\r\n" +
                      "(print (cons nil (cons 1 nil)))         " + "\r\n" +
                      "(newline)                               " + "\r\n" +
                      "(print (cons 1 nil))                    " + "\r\n" +
                      "(print (cons 1 1))                      " + "\r\n" +
                      "(print (cons 1 (cons 1 2)))             " + "\r\n" +
                      "(print (cons 1 (cons 1 nil)))           " + "\r\n" +
                      "(newline)                               " + "\r\n" +
                      "(print (cons (cons 1 2) nil))           " + "\r\n" +
                      "(print (cons (cons 1 2) 1))             " + "\r\n" +
                      "(print (cons (cons 1 2) (cons 1 2)))    " + "\r\n" +
                      "(print (cons (cons 1 2) (cons 1 nil)))  " + "\r\n" +
                      "(newline)                               " + "\r\n" +
                      "(print (cons (cons 1 nil) nil))         " + "\r\n" +
                      "(print (cons (cons 1 nil) 1))           " + "\r\n" +
                      "(print (cons (cons 1 nil) (cons 1 2)))  " + "\r\n" +
                      "(print (cons (cons 1 nil) (cons 1 nil)))" + "\r\n" +
                      "(newline)                               " + "\r\n" +
                      "(print (list))                          " + "\r\n" +
                      "(newline)                               " + "\r\n" +
                      "(print (list nil))                      " + "\r\n" +
                      "(print (list 1))                        " + "\r\n" +
                      "(print (list (cons 1 2)))               " + "\r\n" +
                      "(print (list (cons 1 nil)))             " + "\r\n" +
                      "(newline)                               " + "\r\n" +
                      "(print (list nil nil))                  " + "\r\n" +
                      "(print (list nil 1))                    " + "\r\n" +
                      "(print (list nil (cons 1 2)))           " + "\r\n" +
                      "(print (list nil (cons 1 nil)))         " + "\r\n" +
                      "(newline)                               " + "\r\n" +
                      "(print (list (cons 1 2) nil))           " + "\r\n" +
                      "(print (list (cons 1 2) 1))             " + "\r\n" +
                      "(print (list (cons 1 2) (cons 1 2)))    " + "\r\n" +
                      "(print (list (cons 1 2) (cons 1 nil)))  " + "\r\n" +
                      "(newline)                               " + "\r\n" +
                      "(print (list (cons 1 nil) nil))         " + "\r\n" +
                      "(print (list (cons 1 nil) 1))           " + "\r\n" +
                      "(print (list (cons 1 nil) (cons 1 2)))  " + "\r\n" +
                      "(print (list (cons 1 nil) (cons 1 nil)))");

            Cases.Add("1");
            Cases.Add("+1");
            Cases.Add("-0.3");
            Cases.Add("1.3e1");
            Cases.Add("-0.3e-1");
            Cases.Add("\"'string'\"");
            Cases.Add("\"'x'\"");
            Cases.Add("\"'x0'\"");

            Cases.Add("(define x +0.9)");
            Cases.Add("(define x (+ 1 3))");
            Cases.Add($"(define f (lambda (x) (* x x)))");
            Cases.Add("(define (f) 3.14159)");
            Cases.Add("(define (f x)                " + "\r\n" +
                      "    (define y (* x x))       " + "\r\n" +
                      "    (* y y))");

            Cases.Add("x");
            Cases.Add("true");
            Cases.Add("false");
            Cases.Add("y");
            Cases.Add("f");

            Cases.Add("(quote 0)");
            Cases.Add("(quote x)");
            Cases.Add("(quote true)");
            Cases.Add("(quote (+ x y))");
            Cases.Add("(quote (define x (+ 3 y)))");

            Cases.Add("(lambda () 0)");
            Cases.Add("(lambda (x) 0)");
            Cases.Add("(lambda (x y) 0)");
            Cases.Add("(lambda (x) (* x x))");
            Cases.Add("(lambda (x y) (* x y))");
            Cases.Add("(lambda (x) (define y (* x x)) (* y y))");

            Cases.Add("(set! x 0.9)");
            Cases.Add("(set! x (+ 1 x))");

            Cases.Add("(begin 1 2 3 x)");
            Cases.Add("(begin (define x 3.14) (+ 2 3 x))");
            Cases.Add("(begin 1 2 3 x)");

            Cases.Add("(if (< x 0) -1 1)");
            Cases.Add("(if (< x 0) (+ 2 3))");

            Cases.Add("(cond)");
            Cases.Add("(cond (else 0))");
            Cases.Add("(cond ((< x 0) -1))");
            Cases.Add("(cond ((< x 0) -1)       " + "\r\n" +
                      "    ((> x 0) 1 2 3)      " + "\r\n" +
                      "    (else 0))");

            Cases.Add("(let () 2)");
            Cases.Add("(let ((x 2)) (+ x 2))");

            Cases.Add("(let* () 2)");
            Cases.Add("(let* ((x 2)) (+ x 2))");
            Cases.Add("(let* ((x 2)             " + "\r\n" +
                      "       (y (+ x 2)))      " + "\r\n" +
                      "      (+ x y))");

            Cases.Add("(and)");
            Cases.Add("(and true)");
            Cases.Add("(and false)");
            Cases.Add("(and (= 1 2))");
            Cases.Add("(and (< 1 0) (= (/ 1 0) 0))");
            Cases.Add("(and (< 1 2) (= (/ 1 0) 0))");
            Cases.Add("(and (< 1 0) (= (/ 0 1) 0))");
            Cases.Add("(and (< 1 2) (= (/ 0 1) 0))");
            Cases.Add("(and (< 1 0) (< (/ 0 1) 0))");
            Cases.Add("(and (< 1 2) (< (/ 0 1) 0))");

            Cases.Add("(or)");
            Cases.Add("(or true)");
            Cases.Add("(or false)");
            Cases.Add("(or (< 1 2))");
            Cases.Add("(or (< 1 0) (= (/ 1 0) 0))");
            Cases.Add("(or (< 1 2) (= (/ 1 0) 0))");
            Cases.Add("(or (< 1 0) (= (/ 0 1) 0))");
            Cases.Add("(or (< 1 2) (= (/ 0 1) 0))");
            Cases.Add("(or (< 1 0) (< (/ 0 1) 0))");
            Cases.Add("(or (< 1 2) (< (/ 0 1) 0))");

            Cases.Add("(define (sum l)                      " + "\r\n" +
                      "    (if (null? l)                    " + "\r\n" +
                      "        0                            " + "\r\n" +
                      "        (+ (car l) (sum (cdr l)))))");

            Cases.Add("(define (f1 . z) (sum z))");
            Cases.Add("(define f2 (lambda z (sum z)))");
            Cases.Add("(define (f3 x y . z) (+ x y (sum z)))");
            Cases.Add("(define f4 (lambda (x y . z) (+ x y (sum z))))");

            Cases.Add("(f1)");
            Cases.Add("(f1 1)");
            Cases.Add("(f1 1 2 3)");

            Cases.Add("(f2)");
            Cases.Add("(f2 1)");
            Cases.Add("(f2 1 2 3)");

            Cases.Add("(f3)");
            Cases.Add("(f3 1)");
            Cases.Add("(f3 1 2)");
            Cases.Add("(f3 1 2 3)");
            Cases.Add("(f3 1 2 3 4)");

            Cases.Add("(f4)");
            Cases.Add("(f4 1)");
            Cases.Add("(f4 1 2)");
            Cases.Add("(f4 1 2 3)");
            Cases.Add("(f4 1 2 3 4)");

            Cases.Add("(define i 256)                                   " + "\r\n" +
                      "(define sum1 0)                                  " + "\r\n" +
                      "(define sum2 0)                                  " + "\r\n" +
                      "(for (set! i 0) (< i 101) (set! i (+ i 1))       " + "\r\n" +
                      "   (set! sum1 (+ sum1 i))                        " + "\r\n" +
                      "   (for (define j 0) (< j 101) (set! j (+ j 1))  " + "\r\n" +
                      "       (set! sum2 (+ sum2 j))))");

            Cases.Add("sum1");
            Cases.Add("sum2");
            Cases.Add("i");

            Cases.Add("(f 4)");
            Cases.Add("(+ 1 2 3 4)");
            Cases.Add("(/ 1 0)");
            Cases.Add("(* 1 2 3 4)");
            Cases.Add("(- 10 5)");
            Cases.Add("(% 10 3)");
            Cases.Add("(< 1 0)");
            Cases.Add("(> 1 0)");
            Cases.Add("(= 1 0)");

            Cases.Add("(define l1 (list 99))");
            Cases.Add("(define l2 (list 99 98))");
            Cases.Add("(define l3 (cons 99 nil))");
            Cases.Add("(define l4 (cons 97 (cons 98 99)))");

            Cases.Add("l1");
            Cases.Add("l2");
            Cases.Add("l3");
            Cases.Add("l4");

            Cases.Add("(car l1)");
            Cases.Add("(car l2)");
            Cases.Add("(cdr l1)");
            Cases.Add("(cdr l2)");
            Cases.Add("(cdr l4)");

            Cases.Add("(define w nil)");
            Cases.Add("(null? x)");
            Cases.Add("(null? w)");

            Cases.Add(@"1.3                                   ");
            Cases.Add(@"""x""                                 ");
            Cases.Add(@"true                                  ");
            Cases.Add(@"(quote (define x 3))                  ");
            Cases.Add(@"(quote x)                             ");
            Cases.Add(@"x                                     ");
            Cases.Add(@"y                                     ");
            Cases.Add(@"(define x 3)                          ");
            Cases.Add(@"(define y (- x 1))                    ");
            Cases.Add(@"(define s '*unassigned*)              ");
            Cases.Add(@"x                                     ");
            Cases.Add(@"y                                     ");
            Cases.Add(@"s                                     ");
            Cases.Add(@"(+ x y)                               ");
            Cases.Add(@"(square x)                            ");
            Cases.Add(@"(define (square x) (* x x))           ");
            Cases.Add(@"(square x)                            ");
            Cases.Add(@"(square (square (+ x y)))             ");
            Cases.Add(@"(and)                                 ");
            Cases.Add(@"(and 0 1)                             ");
            Cases.Add(@"(and (< 1 (+ 2 3)))                   ");
            Cases.Add(@"(and (< 1 (+ 2 3)) (= 1 2))           ");
            Cases.Add(@"(or (< 1 (+ 2 3)) (= 1 2))            ");
            Cases.Add(@"(or (> 1 (+ 2 3)) (= 1 2))            ");
            Cases.Add(@"(or (> (square 3) (+ 2 3)) (= 3 2))   ");
            Cases.Add(@"(or)                                  ");

            Cases.Add("(let ((a 3)                            " + "\r\n" +
                      "      (b (* 2 2))                      " + "\r\n" +
                      "      (square (lambda (x) (* x x))))   " + "\r\n" +
                      "     (define x (square a))             " + "\r\n" +
                      "     (define y (square b))             " + "\r\n" +
                      "     (+ x y))");

            Cases.Add("(let* ((x 3)                           " + "\r\n" +
                      "       (y (+ x 2))                     " + "\r\n" +
                      "       (z (+ x y 5)))                  " + "\r\n" +
                      "      (+ x z))");

            Cases.Add("(define(less a b)                      " + "\r\n" +
                      "    (define (compare a b t f)          " + "\r\n" +
                      "        (if (< a b) t f))              " + "\r\n" +
                      "    (compare a b true false))");

            Cases.Add("(define sum 0)                                   " + "\r\n" +
                      "(define sum_odd 0)                               " + "\r\n" +
                      "(define sum_even 0)                              " + "\r\n" +
                      "                                                 " + "\r\n" +
                      "(define (even x) (= 0 (% x 2)))                  " + "\r\n" +
                      "(define (odd x) (= 1 (% x 2)))                   " + "\r\n" +
                      "                                                 " + "\r\n" +
                      "(for (define i 0) (less i 101) (set! i (+ i 1))  " + "\r\n" +
                      "    (set! sum(+ sum i))                          " + "\r\n" +
                      "    (if (even i) (set! sum_even (+ sum_even i))) " + "\r\n" +
                      "    (if (odd i) (set! sum_odd (+ sum_odd i))))   " + "\r\n" +
                      "                                                 " + "\r\n" +
                      "(print sum)                                      " + "\r\n" +
                      "(print sum_even)                                 " + "\r\n" +
                      "(print sum_odd)");

            Cases.Add("(define i 127)                                   " + "\r\n" +
                      "(define sum 0)                                   " + "\r\n" +
                      "                                                 " + "\r\n" +
                      "(for (set! i 0) (< i 101) (set! i (+ i 1))       " + "\r\n" +
                      "    (set! sum(+ sum i)))                         " + "\r\n" +
                      "                                                 " + "\r\n" +
                      "(print i)                                        " + "\r\n" +
                      "(print sum)");

            Cases.Add("(define(fact n)                                  " + "\r\n" +
                      "    (if (= n 1)                                  " + "\r\n" +
                      "        1                                        " + "\r\n" +
                      "        (* n (fact (- n 1)))))");

            Cases.Add("(for (define i 1) (< i 5) (set! i(+ i 1))        " + "\r\n" +
                      "                                                 " + "\r\n" +
                      "    (print \"***\")                              " + "\r\n" +
                      "    (print(fact i))                              " + "\r\n" +
                      "                                                 " + "\r\n" +
                      "    (for (define j 0) (< j i) (set! j (+ j 1))   " + "\r\n" +
                      "        (print(* i j))))");

            Cases.Add("(define(f x)                     " + "\r\n" +
                      "                                 " + "\r\n" +
                      "    (if (even? x) 'EVEN 'ODD)    " + "\r\n" +
                      "                                 " + "\r\n" +
                      "    (define (even? n)            " + "\r\n" +
                      "        (if (= n 0)              " + "\r\n" +
                      "            true                 " + "\r\n" +
                      "            (odd? (- n 1))))     " + "\r\n" +
                      "                                 " + "\r\n" +
                      "    (define (odd? n)             " + "\r\n" +
                      "        (if (= n 0)              " + "\r\n" +
                      "            false                " + "\r\n" +
                      "            (even? (- n 1)))))");

            Cases.Add(@"(f 0)");
            Cases.Add(@"(f 1)");
            Cases.Add(@"(f 2)");
            Cases.Add(@"(f 3)");

            Cases.Add("(let((a 1))                  " + "\r\n" +
                      "    (define (f x)            " + "\r\n" +
                      "        (define b (+ a x))   " + "\r\n" +
                      "        (define a 5)         " + "\r\n" +
                      "        (+ a b))             " + "\r\n" +
                      "    (f 10))");

            Cases.Add("(fact 5)");

            Cases.Add("(define (unless condition usual-value exceptional-value) " + "\r\n" +
                      "     (let ((predicate condition)                         " + "\r\n" +
                      "           (consequent exceptional-value)                " + "\r\n" +
                      "           (alternative usual-value))                    " + "\r\n" +
                      "          (if predicate consequent alternative)))");

            Cases.Add("(define(fact-by-unless n stack)                              " + "\r\n" +
                      "     (if (> stack 200)                                       " + "\r\n" +
                      "         (error 'StackOverflow)                              " + "\r\n" +
                      "         (unless (= n 1)                                     " + "\r\n" +
                      "                 (* n (fact-by-unless (- n 1) (+ 1 stack)))  " + "\r\n" +
                      "                 1)))");

            Cases.Add("(fact-by-unless 5 0)");

            Cases.Add("(define(for-each proc items)                 " + "\r\n" +
                      "  (if (null? items)                          " + "\r\n" +
                      "    'done                                    " + "\r\n" +
                      "    (begin (proc (car items))                " + "\r\n" +
                      "           (for-each proc (cdr items)))))");

            Cases.Add("(for-each (lambda(x)(print x))               " + "\r\n" +
                      "          (list 5 6 7 8))");

            Cases.Add("(define (p1 x)                               " + "\r\n" +
                      "  (set! x (cons x (list 2)))                 " + "\r\n" +
                      "  x)                                         " + "\r\n" +
                      "                                             " + "\r\n" +
                      "(define (p2 x)                               " + "\r\n" +
                      "  (define (p e)                              " + "\r\n" +
                      "    e                                        " + "\r\n" +
                      "    x)                                       " + "\r\n" +
                      "  (p (set! x(cons x (list 2)))))");

            Cases.Add("(p1 1)");
            Cases.Add("(p2 1)");

            Cases.Add("((lambda (x) (+ x 5)) (- x))");
        }
    }
}
