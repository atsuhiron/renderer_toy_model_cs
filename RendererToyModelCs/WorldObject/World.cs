using RendererToyModelCs.Geom;

namespace RendererToyModelCs.WorldObject
{
    public class World(List<ISurface> surfaces, Camera camera)
    {
        public List<ISurface> Surfaces { get; init; } = surfaces;
        public Camera Camera { get; init; } = camera;
    }
}
