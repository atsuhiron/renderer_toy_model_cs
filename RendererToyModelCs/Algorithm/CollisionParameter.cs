namespace RendererToyModelCs.Algorithm
{
    public record CollisionParameter
    {
        public float CoefA { get; init; }
        public float CoefB { get; init; }
        public float Dist { get; init; }

        public CollisionParameter(float coefA, float coefB, float dist)
        {
            CoefA = coefA;
            CoefB = coefB;
            Dist = dist;
        }

        public static CollisionParameter CreateDefault()
        {
            return new CollisionParameter(-1f, -1f, -1f);
        }
    }
}
