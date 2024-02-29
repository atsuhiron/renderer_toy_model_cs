using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;
using RendererToyModelCs.Chromatic;
using RendererToyModelCs.Geom;

namespace RendererToyModelCsTests.Geom
{
    public class LightSurfaceTests
    {
        [Fact]
        public void GetCollisionParticleTest()
        {
            var light = new CLight(Vector<float>.Build.DenseOfArray([0.5f, 0.5f, 0.5f]));
            var surface = new LightSurface(
            [
                Vector<float>.Build.DenseOfArray([-1f, -1f, 0f]),
                Vector<float>.Build.DenseOfArray([-1f, 2f, 0f]),
                Vector<float>.Build.DenseOfArray([2f, -1f, 0f])
            ],
            "light",
            light);

            var pos = Vector<float>.Build.DenseOfArray([-1f, -1f, 1f]);
            var vec = Vector<float>.Build.DenseOfArray([1f, 1f, -1f]);
            var inParticle = new Particle(pos, vec);

            var cParam = CollisionParameter.CreateDefault();
            var num = 100;

            List<IParticle> actual = surface.GetCollisionParticle(inParticle, cParam, num);

            Assert.Single(actual);
            Assert.NotNull(actual.First().Light);
            Assert.Equal(0.5f, actual.First().Light?.Elements[0]);
            Assert.Equal(0.5f, actual.First().Light?.Elements[1]);
            Assert.Equal(0.5f, actual.First().Light?.Elements[2]);
            Assert.Equal(1f / MathF.Sqrt(3), actual.First().Vec[0]);
            Assert.Equal(1f / MathF.Sqrt(3), actual.First().Vec[1]);
            Assert.Equal(-1f / MathF.Sqrt(3), actual.First().Vec[2]);
            Assert.Equal(-1f, actual.First().Pos[0]);
            Assert.Equal(-1f, actual.First().Pos[1]);
            Assert.Equal(1f, actual.First().Pos[2]);
            Assert.Equal(inParticle.Id, actual.First().ParentId);
        }
    }
}
