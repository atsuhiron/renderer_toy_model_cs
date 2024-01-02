using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;

namespace RendererToyModelCs.Geom
{
    public class SmoothSurface(List<Vector<float>> points, string name) : BaseSurface(points, name)
    {
        public override SurfaceType SufType => SurfaceType.Smooth;

        public override List<IParticle> GetCollisionParticle(in IParticle inPaticle, in CollisionParameter cParam, int num)
        {
            var cPoint = CalcRelativeCPoint(cParam) + Origin;
            var outVec = LinearAlgebra.CalcMainOutVec(this, inPaticle);

            return new List<IParticle>()
            {
                new Particle(cPoint, outVec, inPaticle.Id, inPaticle.Intensity, null, Id, false, null)
            };
        }
    }
}
