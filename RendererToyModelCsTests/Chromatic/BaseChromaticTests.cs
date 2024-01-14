using System.Reflection;
using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Chromatic;

namespace RendererToyModelCsTests.Chromatic
{
    public class BaseChromaticTests
    {
        [Fact]
        public void InvalidColorCode1()
        {
            string? code = null;

            // Reflection で呼んでいるので、欲しい例外は TargetInvocationException の InnerException にある
            var excep = Assert.Throws<TargetInvocationException>(() =>
            {
                return TestUtil.InvokeStaticNonPublicMethod<Vector<float>>(typeof(BaseChromatic), "ConvertColorCode", [code]);
            });

            Assert.True(excep?.InnerException?.Message.Contains("Value cannot be null") ?? false);
        }

        [Fact]
        public void InvalidColorCode2()
        {
            string? code = "not color code";
            var excep = Assert.Throws<TargetInvocationException>(() =>
            {
                return TestUtil.InvokeStaticNonPublicMethod<Vector<float>>(typeof(BaseChromatic), "ConvertColorCode", [code]);
            });

            Assert.True(excep?.InnerException?.Message.Contains("This is not color code") ?? false);
        }

        [Fact]
        public void InvalidColorCode3()
        {
            string? code = "#123";
            var excep = Assert.Throws<TargetInvocationException>(() =>
            {
                return TestUtil.InvokeStaticNonPublicMethod<Vector<float>>(typeof(BaseChromatic), "ConvertColorCode", [code]);
            });

            Assert.True(excep?.InnerException?.Message.Contains("This is not color code") ?? false);
        }

        public static IEnumerable<object[]> ConvertColorCodeTestData()
        {
            yield return new object[] { "#000000", Vector<float>.Build.DenseOfArray([0f, 0f, 0f]) };
            yield return new object[] { "#ffFFff", Vector<float>.Build.DenseOfArray([1f, 1f, 1f]) };
            yield return new object[] { "#333333", Vector<float>.Build.DenseOfArray([0.2f, 0.2f, 0.2f]) };
        }

        [Theory]
        [MemberData(nameof(ConvertColorCodeTestData))]
        public void ConvertColorCodeTest(string code, Vector<float> expected)
        {
            Vector<float>? actual = TestUtil.InvokeStaticNonPublicMethod<Vector<float>>(typeof(BaseChromatic), "ConvertColorCode", [code]);

            Assert.NotNull(actual);
            Assert.Equal(expected, actual);
        }
    }
}
