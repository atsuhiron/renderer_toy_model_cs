using System.Reflection;
using MathNet.Numerics.LinearAlgebra;

namespace RendererToyModelCsTests
{
    public static class TestUtil
    {
        // 参考文献: https://qiita.com/fcijpbgiec-ib/items/b713d7bb1c045a360cf1

        public static T? InvokeStaticNonPublicMethod<T>(Type classType, string methodName, object?[] args)
        {
            var method = classType.GetMethod(methodName, BindingFlags.Static | BindingFlags.NonPublic);
            ArgumentNullException.ThrowIfNull(method);

            object? ret = method.Invoke(methodName, args);
            if (ret == null)
                return default;
            return (T)ret;
        }

        private static readonly float s_tol = 2e-7f;

        public static bool IsNearlyEqual(Vector<float> vec1, Vector<float> vec2)
        {
            var diff = (vec1 - vec2).PointwiseAbs();
            return diff.ToList().All(x => x < s_tol);
        }
    }
}
