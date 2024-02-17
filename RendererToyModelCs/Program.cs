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

            var je = JsonReader.ReadFileStr(fileName);
            var json = JsonSerializer.Deserialize<World>(je);
            Console.WriteLine(json?.ToString() ?? "NULL");
        }
    }
}
