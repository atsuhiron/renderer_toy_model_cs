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
            Assert.True(TestUtil.IsNearlyEqual(expected, actual));
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

        public static IEnumerable<object[]> RotateVectorTestData()
        {
            yield return new float[][] { [1f, 0f, 0f], [1f, 0f, 0f], [1f, 0f, 0f] };   // xx
            yield return new float[][] { [1f, 0f, 0f], [0f, 1f, 0f], [0f, 0f, -1f] };  // xy
            yield return new float[][] { [1f, 0f, 0f], [0f, 0f, 1f], [0f, 1f, 0f] };   // xz
            yield return new float[][] { [0f, 1f, 0f], [1f, 0f, 0f], [0f, 0f, 1f] };   // yx
            yield return new float[][] { [0f, 1f, 0f], [0f, 1f, 0f], [0f, 1f, 0f] };   // yy
            yield return new float[][] { [0f, 1f, 0f], [0f, 0f, 1f], [-1f, 0f, 0f] };  // yz
            yield return new float[][] { [0f, 0f, 1f], [1f, 0f, 0f], [0f, -1f, 0f] };  // zx
            yield return new float[][] { [0f, 0f, 1f], [0f, 1f, 0f], [1f, 0f, 0f] };   // zy
            yield return new float[][] { [0f, 0f, 1f], [0f, 0f, 1f], [0f, 0f, 1f] };   // zz
        }

        [Theory]
        [MemberData(nameof(RotateVectorTestData))]
        public void RotateVectorTest(float[] vecEle, float[] axialEle, float[] expectedEle)
        {
            var vec = Vector<float>.Build.DenseOfArray(vecEle);
            var axis = Vector<float>.Build.DenseOfArray(axialEle);
            var expected = Vector<float>.Build.DenseOfArray(expectedEle);

            var actual = LinearAlgebra.RotateVector(vec, axis, MathF.PI/2);
            Assert.True(TestUtil.IsNearlyEqual(expected, actual));
        }

        public static IEnumerable<object[]> CalcMainOutVecTestData()
        {
            // surface_point1~3, particle_point, particle_vec, expected_vec
            yield return new float[][]
            {
                [0f, 0f, 0f], [1f, 0f, 0f], [0f, 1f, 0f],
                [0.25f, 0.25f, 1.25f], [0f, 0f, -1f], [0f, 0f, 1f]
            };
            yield return new float[][]
            {
                [0f, 0f, 0f], [1f, 0f, 0f], [0f, 1f, 0f],
                [0.25f, 0f, 1f], [0.70710678f, 0f, -0.70710678f], [0.70710678f, 0f, 0.707106781f]
            };
            yield return new float[][]
            {
                [0f, 0f, 0f], [1f, 0f, 0f], [0f, -10f, -10f],
                [0.25f, -0.25f, 1.25f], [0f, 0f, -1f], [0f, -1f, 0f]
            };
            yield return new float[][]
            {
                [0f, 0f, 0f], [0f, -10f, -10f], [1f, 0f, 0f],
                [0.25f, -0.25f, 1.25f], [0f, 0f, -1f], [0f, -1f, 0f]
            };
        }

        [Theory]
        [MemberData(nameof(CalcMainOutVecTestData))]
        public void CalcMainOutVecTest(float[] sufpoint1, float[] sufpoint2, float[] sufpoint3, float[] partPoint, float[] partVec, float[] expectedVec)
        {
            var sufPoints = new List<Vector<float>>()
            {
                Vector<float>.Build.DenseOfArray(sufpoint1),
                Vector<float>.Build.DenseOfArray(sufpoint2),
                Vector<float>.Build.DenseOfArray(sufpoint3)
            };
            var suf = new SmoothSurface(sufPoints, string.Empty);

            var part = new Particle(Vector<float>.Build.DenseOfArray(partPoint), Vector<float>.Build.DenseOfArray(partVec));

            var expected = Vector<float>.Build.DenseOfArray(expectedVec);

            var actual = LinearAlgebra.CalcMainOutVec(suf, part);
            Assert.True(TestUtil.IsNearlyEqual(expected, actual));
        }

        [Fact]
        public void FindCollisionSurfaceTest_Empty()
        {
            var pos = Vector<float>.Build.DenseOfArray([0f, 0f, 1f]);
            var vec = Vector<float>.Build.DenseOfArray([0f, 0f, -1f]);
            var particle = new Particle(pos, vec);
            List<ISurface> surfaces = [];

            CollisionResult ret = LinearAlgebra.FindCollisionSurface(particle, surfaces);

            Assert.Equal(-1f, ret.CollisionParame.CoefA);
            Assert.Equal(-1f, ret.CollisionParame.CoefB);
            Assert.Equal(-1f, ret.CollisionParame.Dist);
            Assert.Null(ret.CollidedSurface);
        }

        [Fact]
        public void FindCollisionSurfaceTest_OneCollision()
        {
            var pos = Vector<float>.Build.DenseOfArray([0f, 0f, 1f]);
            var vec = Vector<float>.Build.DenseOfArray([0f, 0f, -1f]);
            var particle = new Particle(pos, vec);
            var surfaces = new List<ISurface>
            {
                new SmoothSurface(
                [
                    Vector<float>.Build.DenseOfArray([-1f, -1f, 0f]),
                    Vector<float>.Build.DenseOfArray([-1f, 2f, 0f]),
                    Vector<float>.Build.DenseOfArray([2f, -1f, 0f])
                ],
                "collision!")
            };

            CollisionResult ret = LinearAlgebra.FindCollisionSurface(particle, surfaces);

            Assert.Equal(1f, ret.CollisionParame.Dist);
            Assert.Equal("collision!", ret.CollidedSurface?.Name);
        }
    }
}
