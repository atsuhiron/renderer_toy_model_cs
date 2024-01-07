using RendererToyModelCs.Geom;

namespace RendererToyModelCs.Algorithm
{
    public record CollisionResult
    {
        public CollisionParameter CollisionParame {  get; init; }

        public ISurface? CollidedSurface { get; init; }

        public CollisionResult(CollisionParameter param, ISurface? surface)
        {
            CollisionParame = param;
            CollidedSurface = surface;
        }

        public bool GoThroughWorld() => CollisionParame == null;

        public static CollisionResult CreateDefault()
        {
            return new CollisionResult(CollisionParameter.CreateDefault(), null);
        }
    }
}
