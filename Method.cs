using System;
using System.Reflection;

class test {
    public static void Main(string[] args) {
        var targetMethod = typeof(hoge).GetMethod ("Echo", BindingFlags.Instance | BindingFlags.Public);
        var replacedMethod = typeof(test).GetMethod ("Echo", BindingFlags.Instance | BindingFlags.Public);
        var targetStaticMethod = typeof(hoge).GetMethod ("StaticEcho", BindingFlags.Static | BindingFlags.Public);
        var replacedStaticMethod = typeof(test).GetMethod ("StaticEcho", BindingFlags.Static | BindingFlags.Public);

        try {
            MethodUtil.ExchangeFunctionPointer (targetMethod, replacedMethod);

            hoge _hoge = new hoge ();
            _hoge.Echo ();

            MethodUtil.ExchangeFunctionPointer (targetStaticMethod, replacedStaticMethod);
            hoge.StaticEcho();
        } catch (Exception e) {
            Console.WriteLine (e.ToString());
        }
    }

    public void Echo() {
        Console.WriteLine ("test Echoooo");
    }

    public static void StaticEcho() {
        Console.WriteLine ("test static Echoooo");
    }
}

class hoge {
    public void Echo() {
        Console.WriteLine ("echoooo");
    }

    public static void StaticEcho() {
        Console.WriteLine ("static echoooo");
    }
}
    
public static class MethodUtil
{
    public static void ExchangeFunctionPointer(MethodInfo method0, MethodInfo method1)
    {
        unsafe
        {
            var functionPointer0 = method0.MethodHandle.Value.ToPointer();
            var functionPointer1 = method1.MethodHandle.Value.ToPointer();
            var tmpPointer = *((int*)new IntPtr(((int*)functionPointer0 + 1)).ToPointer());
            *((int*)new IntPtr(((int*)functionPointer0 + 1)).ToPointer()) = *((int*)new IntPtr(((int*)functionPointer1 + 1)).ToPointer());
            *((int*)new IntPtr(((int*)functionPointer1 + 1)).ToPointer()) = tmpPointer;
        }
    }
}
