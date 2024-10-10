using MetacircularEval_CSharp.PairNS;

namespace MetacircularEval_CSharp.ExpNS
{
    public class Cond : Exp
    {
        public override ExpType Type => ExpType.Cond;

        public Pair<ConditionClause>? Conditions { get; } = null;

        public ElseClause? Else { get; } = null;

        public Cond() { }

        public Cond(ElseClause @else) => Else = @else;

        public Cond(Pair<ConditionClause> conditions) => Conditions = conditions;

        public Cond(Pair<ConditionClause> conditions, ElseClause @else)
        {
            Conditions = conditions;
            Else = @else;
        }

        public static Exp ExpandClauses(Pair<ConditionClause>? conditions, ElseClause? @else)
        {
            static Exp toExp(Pair<Exp> actions) =>

                actions.Snd == null ?
                    actions.Fst :
                    new Begin(actions);

            if (conditions == null)
            {
                return @else == null ? Empty : toExp(@else.Actions);
            }
            else
            {
                var clause = conditions.Fst;
                var condition = clause.Condition;
                var consequent = toExp(clause.Actions);
                var alternative = ExpandClauses(conditions.Snd, @else);

                return new If(condition, consequent, alternative);
            }
        }

        public Exp ToIf() => ExpandClauses(Conditions, Else);

        public override string ToString()
        {
            var clauses =
                Conditions == null ?
                    "" :
                    Conditions.ToString()[1..^1];

            clauses += (Else == null ?
                            "" :
                            Else.ToString());

            return clauses == "" ? "(cond)" : $"(cond {clauses})";
        }

        public override object Eval(Environment env) => ToIf().Eval(env);

        public override object LazyEval(Environment env) => ToIf().LazyEval(env);

        public override Func<Environment, object> Analyze() => ToIf().Analyze();

        public override Func<Environment, object> LazyAnalyze() => ToIf().LazyAnalyze();

        public abstract class Clause(Pair<Exp> actions)
        {
            public Pair<Exp> Actions { get; } = actions;
        }

        public class ConditionClause(Exp condition, Pair<Exp> actions) : Clause(actions)
        {
            public Exp Condition { get; } = condition;

            public override string ToString() => $"({Condition} {Actions.ToString()[1..^1]})";
        }

        public class ElseClause(Pair<Exp> actions) : Clause(actions)
        {
            public override string ToString() => $"(else {Actions.ToString()[1..^1]})";
        }
    }
}
