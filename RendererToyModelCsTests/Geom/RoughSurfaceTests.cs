using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;
using RendererToyModelCs.Chromatic;
using RendererToyModelCs.Geom;

namespace RendererToyModelCsTests.Geom
{
    public class RoughSurfaceTests
    {
        [Fact]
        public void GetCollisionParticleTest()
        {
            var color = new CColor(Vector<float>.Build.DenseOfArray([0.5f, 0.5f, 0.5f]));
            var surface = new RoughSurface(
            [
                Vector<float>.Build.DenseOfArray([-1f, -1f, 0f]),
                Vector<float>.Build.DenseOfArray([-1f, 2f, 0f]),
                Vector<float>.Build.DenseOfArray([2f, -1f, 0f])
            ],
            "rough",
            color);

            var pos = Vector<float>.Build.DenseOfArray([-1f, -1f, 1f]);
            var vec = Vector<float>.Build.DenseOfArray([1f, 1f, -1f]);
            var inParticle = new Particle(pos, vec);

            var cParam = LinearAlgebra.CalcCollisionParam(surface, inParticle);
            var num = 100;

            List<IParticle> actual = surface.GetCollisionParticle(inParticle, cParam, num);

            Assert.Equal(100, actual.Count);

            Assert.True(actual.All(part => part.Pos[0] == 0f));
            Assert.True(actual.All(part => part.Pos[1] == 0f));
            Assert.True(actual.All(part => part.Pos[2] == 0f));

            var scatter1stOrthant = actual.Where(part => part.Vec[0] > 0 && part.Vec[1] > 0);
            var scatter2ndOrthant = actual.Where(part => part.Vec[0] < 0 && part.Vec[1] > 0);
            var scatter3rdOrthant = actual.Where(part => part.Vec[0] < 0 && part.Vec[1] < 0);
            var scatter4thOrthant = actual.Where(part => part.Vec[0] < 0 && part.Vec[1] < 0);

            Assert.NotEmpty(scatter1stOrthant);
            Assert.NotEmpty(scatter2ndOrthant);
            Assert.NotEmpty(scatter3rdOrthant);
            Assert.NotEmpty(scatter4thOrthant);
        }
    }
}
