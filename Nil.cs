namespace MetacircularEval_CSharp
{
    /* 
     * 在运行环境中表示 null. 
     * Lisp 对于 nil 有特殊的输出形式："()", 
     * 所以用一个专门的类型来表示 nil，
     * 比用 C# 的 null 直接表示更方便。
     * 
     * 这样 LookupValue 将返回 object 而非 object?。
     * 同时 LookupValue 内部会将 scanKvp 返回 null 解释为没有查找到该变量，
     * 而不是找到了一个 null 变量。
     */
    public class Nil
    {
        private Nil() { }

        public static Nil nil = new();

        public override string ToString() => "()";
    }
}
