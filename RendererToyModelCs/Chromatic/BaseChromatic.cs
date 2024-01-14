using MathNet.Numerics.LinearAlgebra;


namespace RendererToyModelCs.Chromatic
{
    public abstract class BaseChromatic(Vector<float> elememts)
    {
        public Vector<float> Elements { get; private set; } = elememts;

        protected static Vector<float> ConvertColorCode(string? code)
        {
            ArgumentNullException.ThrowIfNull(code, nameof(code));

            if (!Equals(code[0], '#') || code.Length != 7)
            {
                throw new ArgumentException("This is not color code");
            }

            var intR = Convert.ToInt32(code.Substring(1, 2), 16);
            var intG = Convert.ToInt32(code.Substring(3, 2), 16);
            var intB = Convert.ToInt32(code.Substring(5, 2), 16);

            return Vector<float>.Build.DenseOfArray([intR / 255f, intG / 255f, intB / 255f]);
        }
    }
}
