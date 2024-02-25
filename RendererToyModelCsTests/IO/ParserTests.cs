using MathNet.Numerics.LinearAlgebra;
using Microsoft.CSharp.RuntimeBinder;
using RendererToyModelCs.Geom;
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

        [Fact]
        public void ParseSurfaceTest_Rough()
        {
            var sufDict = new Dictionary<string, dynamic?>()
            {
                { "surface_type", "rough" },
                { "point1", new List<object>() { 1f, 2f, 3f } },
                { "point2", new List<object>() { 4f, 5f, 6f } },
                { "point3", new List<object>() { 7f, 8f, 9f } },
                { "color", "#FFFFFF" },
                { "name", "from dict" }
            };

            ISurface? suf = TestUtil.InvokeStaticNonPublicMethod<ISurface>(typeof(Parser), "ParseSurface", [sufDict]);
            Assert.NotNull(suf);
            Assert.True(suf is RoughSurface);

            RoughSurface rough = (RoughSurface) suf;
            Assert.Equal("from dict", rough.Name);
            Assert.True(TestUtil.IsNearlyEqual(Vector<float>.Build.DenseOfArray([1f, 1f, 1f]), rough.Color.Elements));
            Assert.True(TestUtil.IsNearlyEqual(Vector<float>.Build.DenseOfArray([1f, 2f, 3f]), rough.Points[0]));
            Assert.True(TestUtil.IsNearlyEqual(Vector<float>.Build.DenseOfArray([4f, 5f, 6f]), rough.Points[1]));
            Assert.True(TestUtil.IsNearlyEqual(Vector<float>.Build.DenseOfArray([7f, 8f, 9f]), rough.Points[2]));
        }

        [Fact]
        public void ParseSurfaceTest_Smooth()
        {
            var sufDict = new Dictionary<string, dynamic?>()
            {
                { "surface_type", "smooth" },
                { "point1", new List<object>() { 1f, 2f, 3f } },
                { "point2", new List<object>() { 4f, 5f, 6f } },
                { "point3", new List<object>() { 7f, 8f, 9f } },
                { "name", "from dict" }
            };

            ISurface? suf = TestUtil.InvokeStaticNonPublicMethod<ISurface>(typeof(Parser), "ParseSurface", [sufDict]);
            Assert.NotNull(suf);
            Assert.True(suf is SmoothSurface);

            SmoothSurface smooth = (SmoothSurface)suf;
            Assert.Equal("from dict", smooth.Name);
            Assert.True(TestUtil.IsNearlyEqual(Vector<float>.Build.DenseOfArray([1f, 2f, 3f]), smooth.Points[0]));
            Assert.True(TestUtil.IsNearlyEqual(Vector<float>.Build.DenseOfArray([4f, 5f, 6f]), smooth.Points[1]));
            Assert.True(TestUtil.IsNearlyEqual(Vector<float>.Build.DenseOfArray([7f, 8f, 9f]), smooth.Points[2]));
        }

        [Fact]
        public void ParseSurfaceTest_Light()
        {
            var sufDict = new Dictionary<string, dynamic?>()
            {
                { "surface_type", "light" },
                { "point1", new List<object>() { 1f, 2f, 3f } },
                { "point2", new List<object>() { 4f, 5f, 6f } },
                { "point3", new List<object>() { 7f, 8f, 9f } },
                { "light", "#FFFFFF" },
                { "name", "from dict" }
            };

            ISurface? suf = TestUtil.InvokeStaticNonPublicMethod<ISurface>(typeof(Parser), "ParseSurface", [sufDict]);
            Assert.NotNull(suf);
            Assert.True(suf is LightSurface);

            LightSurface light = (LightSurface)suf;
            Assert.Equal("from dict", light.Name);
            Assert.True(TestUtil.IsNearlyEqual(Vector<float>.Build.DenseOfArray([0f, 0f, 0f]), light.Light.Elements));
            Assert.True(TestUtil.IsNearlyEqual(Vector<float>.Build.DenseOfArray([1f, 2f, 3f]), light.Points[0]));
            Assert.True(TestUtil.IsNearlyEqual(Vector<float>.Build.DenseOfArray([4f, 5f, 6f]), light.Points[1]));
            Assert.True(TestUtil.IsNearlyEqual(Vector<float>.Build.DenseOfArray([7f, 8f, 9f]), light.Points[2]));
        }
    }
}
