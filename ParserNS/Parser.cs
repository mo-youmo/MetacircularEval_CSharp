using MetacircularEval_CSharp.ExpNS;
using MetacircularEval_CSharp.Lexical;
using MetacircularEval_CSharp.PairNS;
using static MetacircularEval_CSharp.ExpNS.Exp;
using static MetacircularEval_CSharp.Lexical.Token;
using static MetacircularEval_CSharp.Nil;
using static MetacircularEval_CSharp.PairNS.Pair;

namespace MetacircularEval_CSharp.ParserNS
{
    public class Parser
    {
        private List<Token> tokens = [];

        private Parser(IEnumerator<Token> lex)
        {
            while (lex.MoveNext())
            {
                tokens.Add(lex.Current);
            }
            length = tokens.Count;
        }

        private int cp = 0;
        private int length = 0;

        private bool EOF => cp >= length;
        private Token Current => tokens[cp];

        private Token consume(byte expect) =>

            EOF ?
                throw new ArgumentException($"expect {GetTypeName(expect)}, but already EOF") :

                Current.Type == expect ?
                    tokens[cp++] :

                    throw new ArgumentException($"expect {GetTypeName(expect)}, but given {Current.TypeName}");

        private Token lookahead(int ahead) =>

            cp + ahead < length ?
                tokens[cp + ahead] :

                throw new ArgumentException($"expect a token, but already EOF");

        private bool is_atom(byte token_type) => token_type == NUM || token_type == SYM || token_type == STR;

        private byte t0 => lookahead(0).Type;
        private byte t1 => lookahead(1).Type;
        private byte t2 => lookahead(2).Type;

        private Pair<Exp>? parse()
        {
            var ret = EOF ? null : make_seqs();

            return
                EOF ?
                    ret :
                    throw new ArgumentException($"expect EOF, but given {Current.TypeName}");
        }

        public static Pair<Exp>? Parse(IEnumerator<Token> lex) => new Parser(lex).parse();

        private Pair<Exp> make_seqs()
        {
            Pair<Exp>? rest() =>

                EOF || Current.Type == RPAREN ?
                    null :
                    Cons(make_exp(), rest());

            return Cons(make_exp(), rest());
        }

        public Exp make_exp()
        {
            if (is_atom(t0))
            {
                return make_atom();
            }
            else if (t0 == SQUOTE)
            {
                return make_squote();
            }
            else
            {
                return t1 switch
                {
                    QUOTE => make_quote(),
                    SET => make_assignment(),
                    DEFINE => make_definition(),
                    IF => make_if(),
                    LAMBDA => make_lambda(),
                    BEGIN => make_begin(),
                    COND => make_cond(),
                    AND => make_and(),
                    OR => make_or(),
                    LET => make_let(),
                    LETSTAR => make_letstar(),
                    FOR => make_for(),
                    SYM => make_application(),
                    LPAREN => make_application(),
                    _ => throw new ArgumentException("Unknow Exp Type")
                };
            }
        }

        private Exp make_atom() => t0 switch
        {
            NUM => new SelfEval(double.Parse(consume(NUM).Value)),
            STR => new SelfEval($"{consume(STR).Value}"),
            SYM => new Variable(consume(SYM).Value),
            _ => throw new ArgumentException("Token should be NUM | SYM | STR")
        };

        private Exp make_squote()
        {
            consume(SQUOTE);
            object content = make_content();
            return new Quoted(content);
        }

        private Exp make_quote()
        {
            consume(LPAREN);
            consume(QUOTE);

            object content = make_content();
            var ret = new Quoted(content);

            consume(RPAREN);
            return ret;
        }

        private object make_content()
        {

            if (is_atom(t0))
            {
                return make_atom();
            }
            else if (t0 != LPAREN && t0 != SQUOTE)
            {
                return new Variable(consume(t0).Value);
            }
            else if (t0 == LPAREN && t1 == RPAREN)
            {
                consume(LPAREN);
                consume(RPAREN);
                return nil;
            }
            else
            {
                return make_pair();
            }
        }

        private Pair make_pair()
        {
            if (t0 == SQUOTE)
            {
                consume(SQUOTE);
                var content = make_content();

                return Cons("quote", Cons(content));
            }
            else
            {
                consume(LPAREN);
                var contents = make_contents();

                consume(RPAREN);
                return contents;
            }
        }

        private Pair make_contents()
        {
            var fst = make_content();

            if (t0 == DOT)
            {
                consume(DOT);
                return Cons(fst, make_content());
            }
            else if (t0 == RPAREN)
            {
                return Cons(fst);
            }
            else
            {
                return Cons(fst, make_contents());
            }
        }

        private Exp make_assignment()
        {
            consume(LPAREN);
            consume(SET);

            string variable = consume(SYM).Value;

            var value = make_exp();
            consume(RPAREN);

            return new Assignment(variable, value);
        }

        private Exp make_definition()
        {
            consume(LPAREN);
            consume(DEFINE);

            if (t0 == SYM)
            {
                string variable = consume(SYM).Value;

                var value = make_exp();
                consume(RPAREN);

                return new Definition(variable, value);
            }
            else
            {
                consume(LPAREN);
                string variable = consume(SYM).Value;

                Pair<string>? @params = null;
                string? list_param = null;

                @params = t0 == SYM ? make_params() : null;

                if (t0 == DOT)
                {
                    consume(DOT);
                    list_param = consume(SYM).Value;
                }
                consume(RPAREN);

                var body = make_seqs();
                var value = new Lambda(@params, list_param, body);
                consume(RPAREN);

                return new Definition(variable, value);
            }
        }

        private Pair<string> make_params()
        {
            Pair<string>? rest() =>

                t0 == SYM ?
                    Cons(consume(SYM).Value, rest()) :
                    null;

            return Cons(consume(SYM).Value, rest());
        }

        private Exp make_if()
        {
            consume(LPAREN);
            consume(IF);

            var predicate = make_exp();
            var consequent = make_exp();
            var alternative = t0 == RPAREN ? null : make_exp();

            consume(RPAREN);

            return
                alternative == null ?
                    new If(predicate, consequent) :
                    new If(predicate, consequent, alternative);
        }

        private Exp make_lambda()
        {
            Pair<string>? @params = null;
            string? list_param = null;

            consume(LPAREN);
            consume(LAMBDA);

            if (t0 == SYM)
            {
                list_param = consume(SYM).Value;
            }
            else
            {
                consume(LPAREN);
                @params = t0 == SYM ? make_params() : null;

                if (t0 == DOT)
                {
                    consume(DOT);
                    list_param = consume(SYM).Value;
                }
                consume(RPAREN);
            }

            var body = make_seqs();
            consume(RPAREN);

            return new Lambda(@params, list_param, body);
        }

        private Exp make_begin()
        {
            consume(LPAREN);
            consume(BEGIN);

            var actions = t0 == RPAREN ? null : make_seqs();
            consume(RPAREN);

            return
                actions == null ?
                    new Begin() :
                    new Begin(actions);
        }

        private Exp make_cond()
        {
            consume(LPAREN);
            consume(COND);

            var predicate_clauses =
                t0 == RPAREN ?
                    null :
                    t1 == ELSE ?
                        null :
                        make_condition_clauses();

            var @else = t0 == RPAREN ? null : make_else_clause();

            consume(RPAREN);

            return
                predicate_clauses != null && @else != null ? new Cond(predicate_clauses, @else) :
                predicate_clauses != null && @else == null ? new Cond(predicate_clauses) :
                predicate_clauses == null && @else != null ? new Cond(@else) :
                                                             new Cond();
        }

        private Pair<Cond.ConditionClause> make_condition_clauses()
        {
            Pair<Cond.ConditionClause>? rest() =>

                t0 == RPAREN || t1 == ELSE ?
                    null :
                    Cons(make_condition_clause(), rest());

            return Cons(make_condition_clause(), rest());
        }

        private Cond.ConditionClause make_condition_clause()
        {
            consume(LPAREN);

            var exp = make_exp();
            var actions = t0 == RPAREN ? null : make_seqs();

            consume(RPAREN);

            return
                actions == null ?
                    new Cond.ConditionClause(True, Cons(exp)) :
                    new Cond.ConditionClause(exp, actions);
        }

        private Cond.ElseClause make_else_clause()
        {
            consume(LPAREN);
            consume(ELSE);

            var actions = make_seqs();
            consume(RPAREN);

            return new Cond.ElseClause(actions);
        }

        private Exp make_and()
        {
            consume(LPAREN);
            consume(AND);

            var predicates = t0 == RPAREN ? null : make_seqs();
            consume(RPAREN);

            return predicates == null ? new And() : new And(predicates);
        }

        private Exp make_or()
        {
            consume(LPAREN);
            consume(OR);

            var predicates = t0 == RPAREN ? null : make_seqs();
            consume(RPAREN);

            return predicates == null ? new Or() : new Or(predicates);
        }

        private Exp make_let()
        {
            consume(LPAREN);
            consume(LET);

            extract_let(out Pair<LetBinding>? bindings, out Pair<Exp> body);
            consume(RPAREN);

            return bindings == null ? new Let(body) : new Let(bindings, body);
        }

        private Exp make_letstar()
        {
            consume(LPAREN);
            consume(LETSTAR);

            extract_let(out Pair<LetBinding>? bindings, out Pair<Exp> body);
            consume(RPAREN);

            return bindings == null ? new LetStar(body) : new LetStar(bindings, body);
        }

        private void extract_let(out Pair<LetBinding>? bindings, out Pair<Exp> body)
        {
            consume(LPAREN);
            bindings = t0 == RPAREN ? null : make_bindings();

            consume(RPAREN);
            body = make_seqs();
        }

        private Pair<LetBinding> make_bindings()
        {
            Pair<LetBinding>? rest() =>

                t0 == RPAREN ?
                    null :
                    Cons(make_binding(), rest());

            return Cons(make_binding(), rest());
        }

        private LetBinding make_binding()
        {
            consume(LPAREN);
            var variable = consume(SYM).Value;

            var value = make_exp();
            consume(RPAREN);

            return new LetBinding(variable, value);
        }

        private Exp make_for()
        {
            consume(LPAREN);
            consume(FOR);

            var initialize =
                lookahead(1).Type == SET ?
                    make_assignment() :
                    make_definition();

            var predicate = make_exp();
            var next = make_exp();
            var body = make_seqs();

            consume(RPAREN);
            return new For(initialize, predicate, next, body);
        }

        private Exp make_application()
        {
            consume(LPAREN);

            var operate = make_exp();
            var @params = t0 == RPAREN ? null : make_seqs();

            consume(RPAREN);

            return
                @params == null ?
                    new Application(operate) :
                    new Application(operate, @params);
        }
    }
}
