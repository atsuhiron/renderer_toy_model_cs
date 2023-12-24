using System;
using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;

namespace RendererToyModelCs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var vec1 = Vector<float>.Build.DenseOfArray([0.0f, -9.1f, 0.2f]);
            var vec2 = Vector<float>.Build.DenseOfArray([0.3f, -1.1f, 4.1f]);

            Console.WriteLine(vec1);
            Console.WriteLine(vec2);
            Console.WriteLine(vec1 + vec2);
            Console.WriteLine(vec1 - vec2);
            Console.WriteLine(vec1 * vec2);
            Console.WriteLine(vec1 / vec2);

            var vec3 = Vector<float>.Build.Dense(3, 1f);
            Console.WriteLine(vec3);

            var norm1 = Vector<float>.Build.DenseOfArray([1f, 0f, 0f]);
            var norm2 = Vector<float>.Build.DenseOfArray([0f, 1f, 0f]);
            var norm3 = Vector<float>.Build.DenseOfArray([0f, 0f, 1f]);

            Console.WriteLine(LinearAlgebra.RotateVector(vec3, norm1, MathF.PI / 4));
            Console.WriteLine(LinearAlgebra.RotateVector(vec3, norm2, MathF.PI / 4));
            Console.WriteLine(LinearAlgebra.RotateVector(vec3, norm3, MathF.PI / 4));
        }
    }
}
