using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;
using RendererToyModelCs.Geom;

namespace RendererToyModelCs.WorldObject
{
    public class Camera(Vector<float> pos, Vector<float> vec, float focal, float fovV, float fovH, int pixelV, int pixelH, CameraMode mode)
    {
        private static readonly Vector<float> s_foreward = Vector<float>.Build.Dense([0f, 1f, 0f]);

        public Vector<float> Pos { get; init; } = pos;
        public Vector<float> Vec { get; init; } = vec;
        public float Focal { get; init; } = focal;
        public float FovV { get; init; } = fovV;
        public float FovH { get; init; } = fovH;
        public int PixelV { get; init; } = pixelV;
        public int PixelH { get; init; } = pixelH;
        public CameraMode Mode { get; init; } = mode;

        public List<IParticle> CreatePixelVec()
        {
            List<Vector<float>> nonRotatedPixelVec = Mode switch
            {
                CameraMode.Plane => CreateNonRotatedPlanePixelVec(),
                CameraMode.Spherical => CreateNonRotatedSphericalPixelVec(),
                _ => throw new ArgumentException($"Not supported camera mode: {Mode}"),
            };

            if (IsForwardDirection())
            {
                return nonRotatedPixelVec
                    .Select(pVec => (IParticle)new Particle(Pos, pVec))
                    .ToList();
            }

            float angle = MathF.Acos(Vec.DotProduct(s_foreward) / (float)Vec.L2Norm());
            Vector<float> axial = LinearAlgebra.Cross(s_foreward, Vec).Normalize(2f);
            return nonRotatedPixelVec
                .Select(pVec => LinearAlgebra.RotateVector(pVec, axial, angle))
                .Select((pVec, index) => (IParticle)new Particle(Pos, pVec, pixelIndex: index))
                .ToList();
        }

        private List<Vector<float>> CreateNonRotatedSphericalPixelVec()
        {
            var halfTheta = FovV / 2;
            var halfPhi = FovH / 2;
            var dTheta = FovV / PixelV;
            var dPhi = FovH / PixelH;
            var pixelVec = new List<Vector<float>>(PixelH * PixelV);

            for (int vi = 0; vi < PixelV; vi++)
            {
                for (int hi = 0; hi < PixelH; hi++)
                {
                    // It can use FMA
                    var phi = dPhi * hi - halfPhi;
                    var theta = dTheta * vi - halfTheta;
                    var vec = Vector<float>.Build.Dense([MathF.Sin(phi), 1f, MathF.Sin(theta)]);
                    pixelVec.Add(vec.Multiply(Focal));
                }
            }
            return pixelVec;
        }

        private List<Vector<float>> CreateNonRotatedPlanePixelVec()
        {
            var halfV = Focal * MathF.Sin(FovV / 2);
            var halfH = Focal * MathF.Sin(FovH / 2);
            var dv = 2 * halfV / PixelV;
            var dh = 2 * halfH / PixelH;
            var pixelVec = new List<Vector<float>>(PixelH * PixelV);

            for (int vi = 0; vi < PixelV; vi++)
            {
                for (int hi = 0; hi < PixelH; hi++)
                {
                    // It can use FMA
                    var x = dh * hi - halfH;
                    var z = dv * vi - halfV;
                    pixelVec.Add(Vector<float>.Build.Dense([x, Focal, z]));
                }
            }
            return pixelVec;
        }

        private bool IsForwardDirection()
        {
            return Vec[0] == 0
                && Vec[1] > 0
                && Vec[2] == 0;
        }
    }
}
