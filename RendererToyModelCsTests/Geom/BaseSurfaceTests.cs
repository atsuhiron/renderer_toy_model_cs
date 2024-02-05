using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;
using RendererToyModelCs.Geom;

namespace RendererToyModelCsTests.Geom
{
    public class BaseSurfaceTests
    {
        [Fact]
        public void OriginTest()
        {
            BaseSurface surface = new SmoothSurface(
            [
                Vector<float>.Build.DenseOfArray([-1f, -1f, 1f]),
                Vector<float>.Build.DenseOfArray([-1f, 2f, 2f]),
                Vector<float>.Build.DenseOfArray([2f, -1f, -3f])
            ],
            "smooth");

            var actual = surface.Origin;

            var expected = Vector<float>.Build.DenseOfArray([-1f, -1f, 1f]);
            Assert.True(TestUtil.IsNearlyEqual(expected, actual));
        }

        [Fact]
        public void CalcBasisTest()
        {
            BaseSurface surface = new SmoothSurface(
            [
                Vector<float>.Build.DenseOfArray([-1f, -1f, 1f]),
                Vector<float>.Build.DenseOfArray([-1f, 2f, 2f]),
                Vector<float>.Build.DenseOfArray([2f, -1f, -3f])
            ],
            "smooth");

            Tuple<Vector<float>, Vector<float>> actual = surface.Basis;

            var expected1 = Vector<float>.Build.DenseOfArray([0f, 3f, 1f]);
            var expected2 = Vector<float>.Build.DenseOfArray([3f, 0f, -4f]);
            Assert.True(TestUtil.IsNearlyEqual(expected1, actual.Item1));
            Assert.True(TestUtil.IsNearlyEqual(expected2, actual.Item2));
        }

        [Fact]
        public void CalcBasisNormTest()
        {
            BaseSurface surface = new SmoothSurface(
            [
                Vector<float>.Build.DenseOfArray([-1f, -1f, 1f]),
                Vector<float>.Build.DenseOfArray([-1f, 2f, 2f]),
                Vector<float>.Build.DenseOfArray([2f, -1f, -3f])
            ],
            "smooth");

            Tuple<float, float> actual = surface.BasisNorm;

            var expected1 = MathF.Sqrt(10);
            var expected2 = 5f;
            Assert.Equal(expected1, actual.Item1);
            Assert.Equal(expected2, actual.Item2);
        }

        [Fact]
        public void CalcRelativeCPointTest()
        {
            BaseSurface surface = new SmoothSurface(
            [
                Vector<float>.Build.DenseOfArray([-1f, -1f, 1f]),
                Vector<float>.Build.DenseOfArray([-1f, 2f, 2f]),
                Vector<float>.Build.DenseOfArray([2f, -1f, -3f])
            ],
            "smooth");
            var cParam = new CollisionParameter(0.1f, 0.2f, 1f);

            var actual = surface.CalcRelativeCPoint(cParam);
            var expected = Vector<float>.Build.DenseOfArray([0.6f, 0.3f, -0.7f]);
            Assert.True(TestUtil.IsNearlyEqual(expected, actual));
        }
    }
}
