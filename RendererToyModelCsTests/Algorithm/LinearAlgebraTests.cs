using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;
using RendererToyModelCs.Chromatic;
using RendererToyModelCs.Geom;

namespace RendererToyModelCsTests.Algorithm
{
    public class LinearAlgebraTests
    {
        public static IEnumerable<object[]> CrossTestData()
        {
            yield return new float[][] { [2f, 0f, 0f], [0f, 1f, 0f], [0f, 0f, 2f] };
            yield return new float[][] { [0f, 3f, 0f], [4f, 0f, 0f], [0f, 0f, -12f] };
        }

        [Theory]
        [MemberData(nameof(CrossTestData))]
        public void CrossTest(float[] vec1Ele, float[] vec2Ele, float[] expectedEle)
        {
            var vec1 = Vector<float>.Build.Dense(vec1Ele);
            var vec2 = Vector<float>.Build.Dense(vec2Ele);
            var expected = Vector<float>.Build.Dense(expectedEle);
            
            var actual = LinearAlgebra.Cross(vec1, vec2);
            Assert.Equal(expected, actual);
        }

        private static readonly float[] s_suf1Point1 = [0f, 0f, 0f];
        private static readonly float[] s_suf1Point2 = [1f, 0f, 0f];
        private static readonly float[] s_suf1Point3 = [0f, 1f, 0f];
        private static readonly float[] s_part1Pos = [0.25f, 0.25f, 1f];
        private static readonly float[] s_part1Vec = [0f, 0f, -1f];

        [Fact]
        public void CalcCollisionParameterTest_NormalCollsion()
        {
            var sufVecElements = new List<float[]>() { s_suf1Point1, s_suf1Point2, s_suf1Point3 }
            .Select(elem => Vector<float>.Build.DenseOfArray(elem))
            .ToList();
            ISurface surface = new RoughSurface(sufVecElements, string.Empty, new CColor(Vector<float>.Build.Dense(3, 1f)));
            IParticle particle = new Particle(Vector<float>.Build.DenseOfArray(s_part1Pos), Vector<float>.Build.DenseOfArray(s_part1Vec));

            var expected = new CollisionParameter(0.25f, 0.25f, 1f);

            var actual = LinearAlgebra.CalcCollisionParam(surface, particle);
            Assert.Equal(expected, actual);
        }

        private static readonly float[] s_part2Pos = [0f, 0f, 1f];
        private static readonly float[] s_part2Vec = [1f, 0f, 0f];

        [Fact]
        public void CalcCollisionParameterTest_NotCollsion()
        {
            var sufVecElements = new List<float[]>() { s_suf1Point1, s_suf1Point2, s_suf1Point3 }
            .Select(elem => Vector<float>.Build.DenseOfArray(elem))
            .ToList();
            ISurface surface = new RoughSurface(sufVecElements, string.Empty, new CColor(Vector<float>.Build.Dense(3, 1f)));
            IParticle particle = new Particle(Vector<float>.Build.DenseOfArray(s_part2Pos), Vector<float>.Build.DenseOfArray(s_part2Vec));

            var expected = new CollisionParameter(-1f, -1f, -1f);

            var actual = LinearAlgebra.CalcCollisionParam(surface, particle);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(-1.0f, 0.4f, 2.0f, 1f, 1f, false)]
        [InlineData(1.0f, -0.4f, 2.0f, 1f, 1f, false)]
        [InlineData(0.6f, 0.5f, -2.0f, 2f, 2f, false)]
        [InlineData(0.5f, 0.6f, -2.0f, 2f, 2f, false)]
        [InlineData(0.5f, 0.5f, -2.0f, 2f, 2f, true)]
        [InlineData(0.1f, 0.1f, -2.0f, 2f, 2f, true)]
        public void DoCollideTest(float cParamA, float cParamB, float cParamD, float basis1, float basis2, bool expected)
        {
            var cParam = new CollisionParameter(cParamA, cParamB, cParamD);
            var basisNorm = Tuple.Create(basis1, basis2);

            bool actual = LinearAlgebra.DoCollide(cParam, basisNorm);
            Assert.Equal(expected, actual);
        }
    }
}
