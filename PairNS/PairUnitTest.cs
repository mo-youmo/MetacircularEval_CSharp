using static MetacircularEval_CSharp.Nil;
using static MetacircularEval_CSharp.PairNS.Pair;

namespace MetacircularEval_CSharp.PairNS
{
    public static class PairUnitTest
    {
        private static List<Pair> obj_pairs = [];
        private static List<Pair<int>> int_pairs = [];

        static PairUnitTest()
        {
            obj_pairs.Add(Cons(1, 2));
            obj_pairs.Add(Cons(1));
            obj_pairs.Add(Cons(nil, 2));
            obj_pairs.Add(Cons(1, nil));
            obj_pairs.Add(Cons(1, Cons(2, Cons(3))));
            obj_pairs.Add(Cons(1, Cons(2, Cons(3))));

            int_pairs.Add(Cons(1));
            int_pairs.Add(Cons(1, Cons(2, Cons(3))));
            int_pairs.Add(Cons(1, Cons(2)));
            int_pairs[1].Snd!.Snd = null;
            int_pairs[2].Snd!.Snd = Cons(3);
        }

        public static void Test()
        {
            for (int i = 0; i < obj_pairs.Count; i++)
            {
                Console.WriteLine($"obj {i} : {obj_pairs[i]}");
            }

            Console.WriteLine();

            for (int i = 0; i < int_pairs.Count; i++)
            {
                Console.WriteLine($"int {i} : {int_pairs[i]}");

                var iter = int_pairs[i].GetEnumerator();

                while (iter.MoveNext())
                {
                    Console.WriteLine($"\t\t{iter.Current}");
                }
            }
        }
    }
}
