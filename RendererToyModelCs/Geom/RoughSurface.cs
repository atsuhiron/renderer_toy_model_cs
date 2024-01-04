using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;
using RendererToyModelCs.Chromatic;

namespace RendererToyModelCs.Geom
{
    public class RoughSurface(List<Vector<float>> points, string? name, CColor color) : BaseSurface(points, name)
    {
        public override SurfaceType SufType => SurfaceType.Rough;

        public CColor Color { get; init; } = color;

        public override List<IParticle> GetCollisionParticle(in IParticle inParticle, in CollisionParameter cParam, int num)
        {
            var relCPoint = CalcRelativeCPoint(cParam);
            var cPoint = relCPoint + Origin;

            var normalizedNorm = NormVec.Normalize(2.0);
            if (normalizedNorm.DotProduct(inParticle.Vec) > 0)
            {
                normalizedNorm.Multiply(-1f);
            }
            var cPointToOriginVec = relCPoint.Normalize(2.0).Multiply(-1f);

            List<float> phi = GenerateRandomPhi(num);
            List<float> theta = GenerateRandomTheta(num);
            var childItst = inParticle.Intensity / num;
            var parentId = inParticle.Id;

            return Enumerable.Range(0, num)
                .Select(index =>
                    {
                        var zenithRotateAxialVec = LinearAlgebra.RotateVector(cPointToOriginVec, normalizedNorm, phi[index]);
                        var outVec = LinearAlgebra.RotateVector(normalizedNorm, zenithRotateAxialVec, theta[index]);
                        return (IParticle)new Particle(cPoint, outVec, parentId, childItst, null, Id, false, null);
                    }
                )
                .ToList();
        }

        private static List<float> GenerateRandomPhi(int size)
        {
            return RandomGenerator.GenrateRandom(size).Select(x => 2 * MathF.PI * x).ToList();
        }

        private static List<float> GenerateRandomTheta(int size)
        {
            var samples = new List<float>(size);
            var coef = MathF.PI / 2;
            while (samples.Count < size)
            {
                while (true)
                {
                    var r = RandomGenerator.GenrateRandom(2);
                    var angle = r[0] * coef;
                    var value = r[1];

                    if (angle > value)
                    {
                        samples.Add(angle);
                        break;
                    }
                }
            }

            return samples;
        }
    }
}
