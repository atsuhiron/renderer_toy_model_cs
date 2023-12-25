using MathNet.Numerics.LinearAlgebra;


namespace RendererToyModelCs.Chromatic
{
    public abstract class BaseChromatic
    {
        public Vector<float> Elements { get; private set; }

        public BaseChromatic(Vector<float> elememts)
        {
            Elements = elememts;
        }

        public static BaseChromatic CreateFromColorCode(string code)
        {
            throw new NotImplementedException();
        }
    }
}
