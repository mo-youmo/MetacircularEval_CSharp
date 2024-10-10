/*
Program ::= Seqs

Seqs ::= Exp
Seqs ::= Exp Seqs

Exp ::= Atom          Exp ::= Quote     
Exp ::= Assignment    Exp ::= Define        Exp ::= If    
Exp ::= Lambda        Exp ::= Begin         Exp ::= Cond      
Exp ::= And           Exp ::= Or            Exp ::= Let
Exp ::= LetStar       Exp ::= For           Exp ::= Application

Atom ::= NUM | SYM | STR

Quote ::= SQUOTE Content                            Content ::= Pair  
Quote ::= (QUOTE Content)                           Content ::= Atom      
                                                    Content ::= ()

Pair ::= 'Content                                   Contents ::= Content . Content
Pair ::= (Contents)                                 Contents ::= Content Contents      
                                   

Assignment ::= (SET SYM Exp)

Define ::= (DEFINE SYM Exp)                         Params ::= SYM              
Define ::= (DEFINE (Func) Seqs)                     Params ::= SYM Params
Define ::= (DEFINE (Func Params) Seqs)               
Define ::= (DEFINE (Func . SYM) Seqs)               Func ::= SYM
Define ::= (DEFINE (Func Params . SYM) Seqs)

If ::= (IF Predicate Consequent)                    Predicate ::= Exp  
If ::= (IF Predicate Consequent Alternative)        Consequent ::= Exp    
                                                    Alternative ::= Exp

Lambda ::= (LAMBDA SYM Seqs)
Lambda ::= (LAMBDA () Seqs)
Lambda ::= (LAMBDA (Params) Seqs)
Lambda ::= (LAMBDA (Params . SYM) Seqs)

Begin ::= (BEGIN)
Begin ::= (BEGIN Seqs)

Cond ::= (COND)                                     ElseClause ::= (ELSE Seqs)                   
Cond ::= (COND ElseClause)                          PredicateClauses ::= PredicateClause
Cond ::= (COND PredicateClauses)                    PredicateClauses ::= PredicateClause PredicateClauses
Cond ::= (COND PredicateClauses ElseClause)         PredicateClause ::= (Exp)
                                                    PredicateClause ::= (Predicate Seqs)

And ::= (AND)                                       Or ::= (OR)
And ::= (AND Seqs)                                  Or ::= (OR Seqs)

Let ::= (LET LetContent)
LetStar ::= (LETSTAR LetContent)

LetContent ::= () Seqs                              Bindings ::= Binding
LetContent ::= (Bindings) Seqs                      Bindings ::= Binding Bindings
                                                    Binding ::= (SYM Exp)

For ::= (FOR Initialize Predicate Next Body)
Initialize ::= Assignment
Initialize ::= Define
Next ::= Exp
Body ::= Seqs

Application ::= (Operate)
Application ::= (Operate ParamExps)

Operate ::= Exp
ParamExps ::= Seqs
*/
