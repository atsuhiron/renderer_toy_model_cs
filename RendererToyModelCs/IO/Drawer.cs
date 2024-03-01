using System.Drawing;
using System.Drawing.Imaging;
using RendererToyModelCs.WorldObject;
using RendererToyModelCs.Geom;

namespace RendererToyModelCs.IO
{
    public class Drawer(Camera camera)
    {
        private readonly Camera _camera = camera;

        public void Draw(List<IParticle> particles, string imgPath)
        {
            var bmp = new Bitmap(_camera.PixelH, _camera.PixelV);

            foreach (var (particle, pi) in particles.OrderBy(part => part.PixelIndex).Select((part, pIndex) => (part, pIndex)))
            {
                int x = pi % _camera.PixelH;
                int y = pi / _camera.PixelH;
                int colorCode = particle.Light?.ToColor().ToRGBCode() ?? 0;
                var col = Color.FromArgb(colorCode);
                bmp.SetPixel(x, y, col);
            }

            bmp.Save(imgPath, ImageFormat.Bmp);
        }
    }
}
