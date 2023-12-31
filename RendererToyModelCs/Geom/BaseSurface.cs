using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;

namespace RendererToyModelCs.Geom
{
    public abstract class BaseSurface : ISurface
    {
        public abstract string Name { get; init; }
        public abstract SurfaceType SufType { get; }
        public abstract Tuple<Vector<float>, Vector<float>> Basis { get; init; }
        public abstract Tuple<float, float> BasisNorm { get; init; }
        public abstract Vector<float> Origin { get; init; }
        public abstract Vector<float> NormVec { get; init; }
        public abstract string Id { get; init; }
        public abstract List<Vector<float>> Points { get; init; }

        public BaseSurface(List<Vector<float>> points, string name)
        {
            Name = name ?? string.Empty;

            if (points.Count != 3)
            {
                throw new ArgumentException("The size of points must be 3");
            }
            Points = points;
            Id = Guid.NewGuid().ToString();

            Basis = CalcBasis();
            BasisNorm = CalcBasisNorm();
            NormVec = CalcNormVec();
        }

        public abstract Vector<float> CalcRelativeCPoint(in CollisionParameter cParam);
        public abstract List<IParticle> GetCollisionParticle(in IParticle inPaticle, in CollisionParameter cParam, int num);

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
