namespace RendererToyModelCs.WorldObject
{
    public record RenderingConfig
    {
        public int MaxGen {  get; init; }
        public int RoughSurfaceChildNum { get; init; }

        public RenderingConfig(int maxGen, int roughSurfaceChildNum)
        {
            MaxGen = maxGen;
            RoughSurfaceChildNum = roughSurfaceChildNum;
        }
    }
}
