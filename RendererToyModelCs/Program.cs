using RendererToyModelCs.IO;
using RendererToyModelCs.WorldObject;

namespace RendererToyModelCs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileName = "../../../samples/simple_world.json";

            var dict = JsonReader.ReadFile(fileName);
            var world = Parser.Parse(dict);
            var config = new RenderingConfig(2, 5);

            var renderer = new Renderer(world, config);
            var paricleList = renderer.Render();
        }
    }
}
