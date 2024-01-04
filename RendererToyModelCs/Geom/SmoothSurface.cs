using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;

namespace RendererToyModelCs.Geom
{
    public class SmoothSurface(List<Vector<float>> points, string? name) : BaseSurface(points, name)
    {
        public override SurfaceType SufType => SurfaceType.Smooth;

        public override List<IParticle> GetCollisionParticle(in IParticle inParticle, in CollisionParameter cParam, int num)
        {
            var cPoint = CalcRelativeCPoint(cParam) + Origin;
            var outVec = LinearAlgebra.CalcMainOutVec(this, inParticle);

            return
            [
                new Particle(cPoint, outVec, inParticle.Id, inParticle.Intensity, null, Id, false, null)
            ];
        }
    }
}
