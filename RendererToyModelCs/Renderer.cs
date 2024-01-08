using RendererToyModelCs.Algorithm;
using RendererToyModelCs.Chromatic;
using RendererToyModelCs.Extension;
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

        private IParticle InverseTraceChild(in List<IParticle> children, in IParticle parent, in Dictionary<string, ISurface> surfaceMap)
        {
            List<CLight> lights = children.Select(c => c.Light ?? CLight.CreateDark()).ToList();
            List<float> itst = children.Select(c => c.Intensity).ToList();

            var (newLight, intensity) = CLight.AddLights(lights, itst);
            List<string> sufIds = children
                .Select(c => c.LastCollidedSurfaceId)
                .WhereNotNull()
                .ToList();

            if (sufIds.Count > 0)
            {
                var found = surfaceMap.TryGetValue(sufIds.First(), out var suf);
                if (found && suf is RoughSurface roughSuf)
                {
                    newLight = newLight.AddColor(roughSuf.Color);
                }
            }

            return Particle.CreateInverseTraceParticle(parent, newLight, intensity);
        }

        private List<IParticle> InverseTrace(List<IParticle> children, List<IParticle> parent, Dictionary<string, ISurface> surfaceMap)
        {
            IEnumerable<string> parentIds = parent.Select(p => p.Id);
            Dictionary<string, List<IParticle>> familyTree = parentIds.ToDictionary(pId => pId, pId => new List<IParticle>());

            foreach (var child in children)
                familyTree[child.ParentId].Add(child);

            return parentIds.Select((string pId, int index) =>
            {
                var family = familyTree[pId];
                IParticle itp;
                if (family.Count == 0)
                    itp = parent[index];
                else
                    itp = InverseTraceChild(children, parent[index], surfaceMap);
                return itp;
            }).ToList();
        }
    }
}
