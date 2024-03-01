using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Chromatic;

namespace RendererToyModelCs.Geom
{
    public record Particle : IParticle
    {
        public Vector<float> Pos { get; init; }
        public Vector<float> Vec { get; init; }
        public float Intensity { get; init; }
        public string Id { get; init; }
        public bool IsTerminated { get; init; }
        public string ParentId { get; init; }
        public int Generation { get; init; }
        public CLight? Light { get; init; }
        public string? LastCollidedSurfaceId { get; init; }
        public int? PixelIndex { get; init; }

        public Particle(Vector<float> pos, Vector<float> vec,
            string parentId="root", float intensity=1f, CLight? light=null, string? lastCollidedSurfaceId=null,
            bool isTerminated=false, string? uuid=null, int? pixelIndex=null)
        {
            Pos = pos;
            Vec = vec;
            Intensity = intensity;
            Id = uuid ?? Guid.NewGuid().ToString();
            IsTerminated = isTerminated;
            ParentId = parentId;
            Light = light;
            LastCollidedSurfaceId = lastCollidedSurfaceId;
            PixelIndex = pixelIndex;
        }

        private static string GenUuid() => Guid.NewGuid().ToString();


        public static IParticle CreateTerminatedParticle(IParticle source, CLight light)
        {
            return new Particle(
                source.Pos, 
                source.Vec.Normalize(2), 
                source.Id,
                source.Intensity, 
                light, 
                string.Empty, 
                true, 
                GenUuid());
        }

        public static IParticle CreateInverseTraceParticle(IParticle source, CLight light, float itst)
        {
            return new Particle(
                source.Pos,
                source.Vec,
                source.ParentId,
                itst,
                light,
                string.Empty,
                false,
                source.Id);
        }
    }
}
