using RendererToyModelCs.Algorithm;
using RendererToyModelCs.Geom;
using RendererToyModelCs.WorldObject;

namespace RendererToyModelCs
{
    public class Renderer(World world, RenderingConfig config)
    {
        public World WorldGeom { get; init; } = world;

        public RenderingConfig Config { get; init; } = config;

        private List<IParticle> TraceParticle(in IParticle part, in List<ISurface> surfaces)
        {
            if (part.Generation > Config.MaxGen) return [];
            if (part.IsTerminated) return [];

            var colRes = LinearAlgebra.FindCollisionSurface(part, surfaces);
            if (colRes.GoThroughWorld())
                return [];
            return colRes.CollidedSurface?.GetCollisionParticle(part, colRes.CollisionParame, Config.RoughSurfaceChildNum) ?? [];
        }

        private List<IParticle> TraceParticles(in List<IParticle> particles, List<ISurface> surfaces)
        {
            return particles.Select(part => TraceParticle(part, surfaces)).SelectMany(fromOnePart => fromOnePart).ToList();
        }
    }
}
