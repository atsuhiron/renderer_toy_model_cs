namespace RendererToyModelCs.Algorithm
{
    public record CollisionParameter(float CoefA, float CoefB, float Dist)
    {
        public static CollisionParameter CreateDefault()
        {
            return new CollisionParameter(-1f, -1f, -1f);
        }
    }
}
