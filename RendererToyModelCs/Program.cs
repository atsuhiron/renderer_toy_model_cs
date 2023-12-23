using MathNet.Numerics.LinearAlgebra;

namespace RendererToyModelCs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var vec1 = Vector<float>.Build.DenseOfArray(new float[] { 0.0f, -9.1f, 0.2f });
            var vec2 = Vector<float>.Build.DenseOfArray(new float[] { 0.3f, -1.1f, 4.1f });

            Console.WriteLine(vec1);
            Console.WriteLine(vec2);
            Console.WriteLine(vec1 + vec2);
            Console.WriteLine(vec1 - vec2);
            Console.WriteLine(vec1 * vec2);
            Console.WriteLine(vec1 / vec2);
        }
    }
}
