using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Random;
using RendererToyModelCs.Algorithm;
using RendererToyModelCs.Geom;
using RendererToyModelCs.IO;
using RendererToyModelCs.WorldObject;
using System;
using System.Text.Json;

namespace RendererToyModelCs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileName = "../../../samples/simple_world.json";

            var dict = JsonReader.ReadFile(fileName);
            Console.WriteLine(dict?.ToString() ?? "NULL");
        }
    }
}
