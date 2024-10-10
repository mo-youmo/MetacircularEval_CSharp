using System.Collections;
using System.Text;
using static MetacircularEval_CSharp.Lexical.Token;

namespace MetacircularEval_CSharp.Lexical
{
    public class Lexer : IEnumerator<Token>
    {
        private string text;
        private int length;
        private int cp = 0;

        private char current_char => text[cp];
        private bool EOF => cp >= length;

        private void skip_char() => cp++;
        private void push_char() => buffer.Append(text[cp++]);

        private bool is_digital(char c) => c >= '0' && c <= '9';
        private bool is_sign(char c) => c == '+' || c == '-';
        private bool is_exponent(char c) => c == 'e' || c == 'E';
        private bool is_lparen(char c) => c == '(';
        private bool is_rparen(char c) => c == ')';
        private bool is_squote(char c) => c == '\'';
        private bool is_dquote(char c) => c == '"';
        private bool is_dot(char c) => c == '.';
        private bool is_blank(char c) => c == ' ' || c == '\r' || c == '\n' || c == '\t';
        private bool is_stopper(char c) => is_blank(c) || is_dquote(c) || is_squote(c) || is_lparen(c) || is_rparen(c);

        private StringBuilder buffer = new StringBuilder();
        private Action? advance = null;
        private bool @break = false;

        private Lexer(string input) => (text, length) = (input, input.Length);

        public static IEnumerator<Token> Tokenize(string input) => new Lexer(input);

        private void return_token(Token token)
        {
            Current = token;
            @break = true;
        }

        public Token Current { get; private set; } = default!;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            buffer.Clear();
            advance = advance_start;
            @break = false;
            Current = default!;
            while (!@break) advance();
            return Current != default;
        }

        public void Reset()
        {
            Current = default!;
            cp = 0;
        }

        public void Dispose() { /* do nothing */ }

        private void advance_start()
        {
            if (EOF) @break = true;

            else if (is_blank(current_char))
            {
                skip_char();
            }
            else if (is_lparen(current_char))
            {
                push_char();
                return_token(new Token(LPAREN, buffer.ToString()));
            }
            else if (is_rparen(current_char))
            {
                push_char();
                return_token(new Token(RPAREN, buffer.ToString()));
            }
            else if (is_squote(current_char))
            {
                push_char();
                return_token(new Token(SQUOTE, buffer.ToString()));
            }
            else if (is_dquote(current_char))
            {
                skip_char();
                advance = advance_str;
            }
            else if (is_dot(current_char))
            {
                push_char();
                advance = advance_dot;
            }
            else if (is_sign(current_char))
            {
                push_char();
                advance = advance_sign;
            }
            else if (is_digital(current_char))
            {
                push_char();
                advance = advance_int;
            }
            else
            {
                push_char();
                advance = advance_sym;
            }
        }

        private void advance_str()
        {
            if (EOF) throw new InvalidDataException("missing \"");

            if (is_dquote(current_char))
            {
                skip_char();
                return_token(new Token(STR, buffer.ToString()));
            }
            else
            {
                push_char();
            }
        }

        private void advance_dot()
        {
            if (EOF || is_stopper(current_char))
            {
                return_token(new Token(DOT, buffer.ToString()));
            }
            else if (is_digital(current_char))
            {
                push_char();
                advance = advance_float;
            }
            else
            {
                push_char();
                advance = advance_sym;
            }
        }

        private void advance_sign()
        {
            if (EOF || is_stopper(current_char))
            {
                return_token(new Token(SYM, buffer.ToString()));
            }
            else if (is_digital(current_char))
            {
                push_char();
                advance = advance_int;
            }
            else if (is_dot(current_char))
            {
                push_char();
                advance = advance_sign_dot;
            }
            else
            {
                push_char();
                advance = advance_sym;
            }
        }

        private void advance_int()
        {
            if (EOF || is_stopper(current_char))
            {
                return_token(new Token(NUM, buffer.ToString()));
            }
            else if (is_digital(current_char))
            {
                push_char();
            }
            else if (is_dot(current_char))
            {
                push_char();
                advance = advance_float;
            }
            else if (is_exponent(current_char))
            {
                push_char();
                advance = advance_exponent;
            }
            else
            {
                push_char();
                advance = advance_sym;
            }
        }

        private void advance_float()
        {
            if (EOF || is_stopper(current_char))
            {
                return_token(new Token(NUM, buffer.ToString()));
            }
            else if (is_digital(current_char))
            {
                push_char();
            }
            else if (is_exponent(current_char))
            {
                push_char();
                advance = advance_exponent;
            }
            else
            {
                push_char();
                advance = advance_sym;
            }
        }

        private void advance_exponent()
        {
            if (EOF || is_stopper(current_char))
            {
                return_token(new Token(SYM, buffer.ToString()));
            }
            else if (is_digital(current_char))
            {
                push_char();
                advance = advance_exponent_int;
            }
            else if (is_sign(current_char))
            {
                push_char();
                advance = advance_exponent_sign;
            }
            else
            {
                push_char();
                advance = advance_sym;
            }
        }

        private void advance_exponent_sign()
        {
            if (EOF || is_stopper(current_char))
            {
                return_token(new Token(SYM, buffer.ToString()));
            }
            else if (is_digital(current_char))
            {
                push_char();
                advance = advance_exponent_int;
            }
            else
            {
                push_char();
                advance = advance_sym;
            }
        }

        private void advance_exponent_int()
        {
            if (EOF || is_stopper(current_char))
            {
                return_token(new Token(NUM, buffer.ToString()));
            }
            else if (is_digital(current_char))
            {
                push_char();
            }
            else
            {
                push_char();
                advance = advance_sym;
            }
        }

        private void advance_sym()
        {
            if (EOF || is_stopper(current_char))
            {
                return_token(new Token(SYM, buffer.ToString()));
            }
            else
            {
                push_char();
            }
        }

        private void advance_sign_dot()
        {
            if (EOF || is_stopper(current_char))
            {
                return_token(new Token(SYM, buffer.ToString()));
            }
            else if (is_digital(current_char))
            {
                push_char();
                advance = advance_float;
            }
            else
            {
                push_char();
                advance = advance_sym;
            }
        }
    }
}
