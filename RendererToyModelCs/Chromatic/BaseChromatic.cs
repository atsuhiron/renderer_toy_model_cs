using MathNet.Numerics.LinearAlgebra;


namespace RendererToyModelCs.Chromatic
{
    public abstract class BaseChromatic(Vector<float> elememts)
    {
        public Vector<float> Elements { get; private set; } = elememts;

        public static BaseChromatic CreateFromColorCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}
