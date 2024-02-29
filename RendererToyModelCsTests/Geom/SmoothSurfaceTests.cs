using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;
using RendererToyModelCs.Geom;

namespace RendererToyModelCsTests.Geom
{
    public class SmoothSurfaceTests
    {
        [Fact]
        public void GetCollisionParticleTest()
        {
            var surface = new SmoothSurface(
            [
                Vector<float>.Build.DenseOfArray([-1f, -1f, 0f]),
                Vector<float>.Build.DenseOfArray([-1f, 2f, 0f]),
                Vector<float>.Build.DenseOfArray([2f, -1f, 0f])
            ],
            "smooth");

            var pos = Vector<float>.Build.DenseOfArray([-1f, -1f, 1f]);
            var vec = Vector<float>.Build.DenseOfArray([1f, 1f, -1f]);
            var inParticle = new Particle(pos, vec);

            var cParam = LinearAlgebra.CalcCollisionParam(surface, inParticle);
            var num = 100;

            List<IParticle> actual = surface.GetCollisionParticle(inParticle, cParam, num);

            Assert.Single(actual);

            Assert.True(actual.All(part => part.Pos[0] == 0f));
            Assert.True(actual.All(part => part.Pos[1] == 0f));
            Assert.True(actual.All(part => part.Pos[2] == 0f));

            var scatter1stOrthant = actual.Where(part => part.Vec[0] > 0 && part.Vec[1] > 0);
            var scatter2ndOrthant = actual.Where(part => part.Vec[0] < 0 && part.Vec[1] > 0);
            var scatter3rdOrthant = actual.Where(part => part.Vec[0] < 0 && part.Vec[1] < 0);
            var scatter4thOrthant = actual.Where(part => part.Vec[0] < 0 && part.Vec[1] < 0);

            Assert.Single(scatter1stOrthant);
            Assert.Empty(scatter2ndOrthant);
            Assert.Empty(scatter3rdOrthant);
            Assert.Empty(scatter4thOrthant);

            HashSet<string> parentIdSet = actual.Select(part => part.ParentId).ToHashSet();
            Assert.Single(parentIdSet);
            Assert.Equal(parentIdSet.First(), inParticle.Id);
        }
    }
}
