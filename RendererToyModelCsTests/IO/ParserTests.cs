using MathNet.Numerics.LinearAlgebra;
using Microsoft.CSharp.RuntimeBinder;
using RendererToyModelCs.IO;
using System.Reflection;

namespace RendererToyModelCsTests.IO
{
    public class ParserTests
    {
        public static IEnumerable<object[]> ToFloatTestData()
        {
            yield return new object[] { 1f, 1f };  // float
            yield return new object[] { 2d, 2f };  // double
            yield return new object[] { 3m, 3f };  // decimal
        }

        [Theory]
        [MemberData(nameof(ToFloatTestData))]
        public void ToFloatTest(object numeric, object expected)
        {
            float actual = TestUtil.InvokeStaticNonPublicMethod<float>(typeof(Parser), "ToFloat", [numeric]);
            float expectedFloat  = (float)expected;
            Assert.Equal(expectedFloat, actual);
        }

        [Fact]
        public void ToFloatNTest_NotNumeric()
        {
            var excep = Assert.Throws<TargetInvocationException>(() =>
            {
                return TestUtil.InvokeStaticNonPublicMethod<float>(typeof(Parser), "ToFloat", [ "Not numeric value" ]);
            });

            Assert.True(excep?.InnerException?.Message.Contains("Not supported type") ?? false);
        }

        [Fact]
        public void ParseVecTest_NullArgument()
        {
            object? nullObj = null;
            var excep = Assert.Throws<TargetInvocationException>(() =>
            {
                return TestUtil.InvokeStaticNonPublicMethod<Vector<float>>(typeof(Parser), "ParseVec", [nullObj, "TestVec"]);
            });

            Assert.True(excep?.InnerException?.Message.Contains("Value cannot be null") ?? false);
        }

        [Fact]
        public void ParseVecTest_NotSupportedType()
        {
            string arg = "Not supported Type";
            var excep = Assert.Throws<TargetInvocationException>(() =>
            {
                return TestUtil.InvokeStaticNonPublicMethod<Vector<float>>(typeof(Parser), "ParseVec", [arg, "TestVec"]);
            });

            Exception? expected = excep?.InnerException;
            Assert.True(expected is RuntimeBinderException);
        }

        [Fact]
        public void ParseVecTest_InvalidLength()
        {
            var tooShort = new List<object> { 1f };
            var excep = Assert.Throws<TargetInvocationException>(() =>
            {
                return TestUtil.InvokeStaticNonPublicMethod<Vector<float>>(typeof(Parser), "ParseVec", [tooShort, "TestVec"]);
            });

            Exception? expected = excep?.InnerException;
            Assert.True(expected is ArgumentException);
        }

        [Fact]
        public void ParseVecTest_Nnormal()
        {
            var vec = new List<object> { 1f, 0.5f, -0.25f };
            var actual = TestUtil.InvokeStaticNonPublicMethod<Vector<float>>(typeof(Parser), "ParseVec", [vec, "TestVec"]);
            Assert.NotNull(actual);

            var expected = Vector<float>.Build.DenseOfArray([1f, 0.5f, -0.25f]);

            Assert.True(TestUtil.IsNearlyEqual(actual, expected));
        }
    }
}
