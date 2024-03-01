using MathNet.Numerics.LinearAlgebra;

namespace RendererToyModelCs.Chromatic
{
    public class CColor(Vector<float> elememts) : BaseChromatic(elememts)
    {
        public CLight ToLight()
        {
            return new CLight(1 - Elements);
        }

        public bool IsDark() => Elements.Sum() <= 0;

        public int ToRGBCode()
        {
            var red = (int)(Elements[0] * 255);
            var gre = (int)(Elements[1] * 255);
            var blu = (int)(Elements[2] * 255);

            return (red << 16) + (gre << 8) + blu;
        }

        public static CColor CreateFromColorCode(string? code)
        {
            return new CColor(ConvertColorCode(code));
        }
    }
}
