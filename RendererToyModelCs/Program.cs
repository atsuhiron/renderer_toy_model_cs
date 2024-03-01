using RendererToyModelCs.IO;
using RendererToyModelCs.WorldObject;

namespace RendererToyModelCs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fileName = "../../../samples/simple_world_qvga.json";

            var dict = JsonReader.ReadFile(fileName);
            var world = Parser.Parse(dict);
            var config = new RenderingConfig(3, 6);

            var renderer = new Renderer(world, config);
            var paricleList = renderer.Render();

            var drawer = new Drawer(world.Camera);
            drawer.Draw(paricleList, "../../../samples/out.bmp");
        }
    }
}
