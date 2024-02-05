using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;

namespace RendererToyModelCs.Geom
{
    public abstract class BaseSurface : ISurface
    {
        public string Name { get; init; }
        public abstract SurfaceType SufType { get; }
        public Tuple<Vector<float>, Vector<float>> Basis { get; init; }
        public Tuple<float, float> BasisNorm { get; init; }
        public Vector<float> Origin { get; init; }
        public Vector<float> NormVec { get; init; }
        public string Id { get; init; }
        public List<Vector<float>> Points { get; init; }

        public BaseSurface(List<Vector<float>> points, string? name)
        {
            Name = name ?? string.Empty;

            if (points.Count != 3)
            {
                throw new ArgumentException("The size of points must be 3");
            }
            Points = points;
            Id = Guid.NewGuid().ToString();

            Origin = points[0];
            Basis = CalcBasis();
            BasisNorm = CalcBasisNorm();
            NormVec = CalcNormVec();
        }

        public Vector<float> CalcRelativeCPoint(in CollisionParameter cParam)
        {
            return Basis.Item1.Multiply(cParam.CoefA) + Basis.Item2.Multiply(cParam.CoefB);
        }

        public abstract List<IParticle> GetCollisionParticle(in IParticle inParticle, in CollisionParameter cParam, int num);

        private Tuple<Vector<float>, Vector<float>> CalcBasis()
        {
            return new Tuple<Vector<float>, Vector<float>>(
                Points.ElementAt(1) - Points.ElementAt(0),
                Points.ElementAt(2) - Points.ElementAt(0)
            );
        }

        private Tuple<float, float> CalcBasisNorm()
        {
            return new Tuple<float, float>(
                (float)Basis.Item1.L2Norm(),
                (float)Basis.Item2.L2Norm()
            );
        }

        private Vector<float> CalcNormVec() => LinearAlgebra.Cross(Basis.Item1, Basis.Item2);
    }
}
