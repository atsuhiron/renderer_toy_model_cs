using MathNet.Numerics.LinearAlgebra;

namespace RendererToyModelCs.Chromatic
{
    public class CLight(Vector<float> elememts) : BaseChromatic(elememts)
    {
        public CColor ToColor()
        {
            return new CColor(1 - Elements);
        }

        public CLight AddColor(in CColor color)
        {
            var elements = ToColor().Elements.PointwiseMultiply(color.Elements);
            return new CLight(elements);
        }

        public static (CLight, float) AddLights(in List<CLight> lights, in List<float> intensities)
        {
            if (lights.Count != intensities.Count)
            {
                throw new ArgumentException($"{nameof(lights)} and {nameof(intensities)} must have same length");
            }
            if (lights.Count <= 1)
            {
                return (lights.FirstOrDefault(new CLight(Vector<float>.Build.Dense(3, 0f))), intensities.FirstOrDefault(0f));
            }

            IEnumerable<CColor> colors = lights.Zip(intensities)
                .Select((lightInst) => new CColor((1 - lightInst.First.Elements).Multiply(lightInst.Second)));
            IEnumerable<float> darkMask = colors.Select(col => col.IsDark() ? 0f : 1f);

            float maskedIntensity = darkMask.Zip(intensities)
                .Select(maskInst => maskInst.First * maskInst.Second)
                .Sum();
            
            var sumVec = Vector<float>.Build.Dense(3, 0f);
            foreach (var color in colors)
            {
                sumVec += color.Elements;
            }

            var newLight = new CLight(1 - sumVec);
            return (newLight, maskedIntensity);
        }

        public static CLight CreateDark() => new(Vector<float>.Build.Dense([1f, 1f, 1f]));

        public static CLight CreateFromColorCode(string? code)
        {
            return new CLight(1 - ConvertColorCode(code));
        }
    }
}
