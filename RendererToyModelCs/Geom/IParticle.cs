using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Chromatic;

namespace RendererToyModelCs.Geom
{
    public interface IParticle
    {
        Vector<float> Pos { get; init; }
        Vector<float> Vec { get; init; }
        float Intensity { get; init; }
        string Id { get; init; }
        bool IsTerminated { get; init; }
        string ParentId { get; init; }
        int Generation { get; init; }
        CLight Light { get; init; }
        string LastCollidedSurfaceId { get; init; }
    }
}
