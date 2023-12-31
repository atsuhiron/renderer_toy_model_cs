using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;

namespace RendererToyModelCs.Geom
{
    public interface ISurface
    {
        string Name { get; init; }
        SurfaceType SufType { get; }
        Tuple<Vector<float>, Vector<float>> Basis { get; init; }
        Tuple<float, float> BasisNorm { get; init; }
        Vector<float> Origin { get; init; }
        Vector<float> NormVec {  get; init; }
        string Id { get; init; }
        List<Vector<float>> Points { get; init; }

        Vector<float> CalcRelativeCPoint(in CollisionParameter cParam);

        List<IParticle> GetCollisionParticle(in IParticle inPaticle, in CollisionParameter cParam, int num);
    }
}
