using System.Text.Json.Serialization;
using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;

namespace RendererToyModelCs.Geom
{
    [JsonDerivedType(typeof(ISurface), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(RoughSurface), typeDiscriminator: "rough")]
    [JsonDerivedType(typeof(SmoothSurface), typeDiscriminator: "smooth")]
    [JsonDerivedType(typeof(LightSurface), typeDiscriminator: "light")]
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

        List<IParticle> GetCollisionParticle(in IParticle inParticle, in CollisionParameter cParam, int num);
    }
}
