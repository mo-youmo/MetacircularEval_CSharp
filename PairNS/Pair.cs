using System.Collections;
using static MetacircularEval_CSharp.Nil;

namespace MetacircularEval_CSharp.PairNS
{
    public class Pair(object a, object b)
    {
        public object Fst { get; set; } = a;
        public object Snd { get; set; } = b;

        public override string ToString() =>

            Snd == nil ?
                $"({Fst})" :

                Snd is not Pair ?
                    $"({Fst} . {Snd})" :

                    Fst.ToString() == "quote" && (Snd as Pair)!.Snd == nil ?
                        $"'{(Snd as Pair)!.Fst}" :

                        $"({Fst} {Snd.ToString()?[1..^1] ?? ""})";

        public static Pair Cons(object fst, object snd) => new(fst, snd);

        public static Pair<T> Cons<T>(T fst, Pair<T>? snd = null) where T : notnull => new(fst, snd);

        public static Pair<T>? Filter<T>(Pair<T>? p, Predicate<T> predicate) where T : notnull =>

            p == null ?
                null :

                predicate(p.Fst) ?
                    Cons(p.Fst, Filter(p.Snd, predicate)) :

                    Filter(p.Snd, predicate);

        public static Pair<T2>? Map<T1, T2>(Pair<T1>? p, Func<T1, T2> mapper)

            where T1 : notnull
            where T2 : notnull =>

            p == null ?
                null :
                Cons(mapper(p.Fst), Map(p.Snd, mapper));

        public static Pair<T>? Append<T>(Pair<T>? p1, Pair<T>? p2) where T : notnull =>

            p1 == null ?
                p2 :
                Cons(p1.Fst, Append(p1.Snd, p2));

    }

    public class Pair<T> : Pair, IEnumerable<T> where T : notnull
    {
        public new T Fst
        {
            get => (T)(base.Fst);
            set => base.Fst = value;
        }

        public new Pair<T>? Snd
        {
            get => base.Snd == nil ? null : (Pair<T>)(base.Snd);
            set => base.Snd = value == null ? nil : value;
        }

        public Pair(T fst, Pair<T>? snd = null) : base(fst, snd == null ? nil : snd) { }

        public IEnumerator<T> GetEnumerator() => new PairEnumerator<T>(this);

        IEnumerator IEnumerable.GetEnumerator() => new PairEnumerator<T>(this);
    }

    public class PairEnumerator<T>(Pair<T> p) : IEnumerator<T> where T : notnull
    {
        private readonly Pair<T> p = p;

        private Pair<T>? pt = p;

        public T Current { get; private set; } = default!;

        object? IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (pt != null)
            {
                Current = pt.Fst!;
                pt = pt.Snd;
                return true;
            }
            else
            {
                Current = default!;
                return false;
            }
        }

        public void Reset()
        {
            pt = p;
            Current = default!;
        }

        public void Dispose() { /* do nothing */}
    }
}
