using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Geom;

namespace RendererToyModelCs.Algorithm
{
    public static class LinearAlgebra
    {
        public static Vector<float> Cross(Vector<float> left, Vector<float> right)
        {
            var x = left[1] * right[2] - left[2] * right[1];
            var y = -left[0] * right[2] + left[2] * right[0];
            var z = left[0] * right[1] - left[1] * right[0];
            return Vector<float>.Build.Dense([x, y, z]);
        }

        public static CollisionParameter CalcCollisionParam(in ISurface suf, in IParticle part)
        {
            var arr = new float[]
            {
                suf.Basis.Item1[0], suf.Basis.Item1[1], suf.Basis.Item1[2],
                suf.Basis.Item2[0], suf.Basis.Item2[1], suf.Basis.Item2[2],
                -part.Vec[0], -part.Vec[1], -part.Vec[2]
            };
            var matA = Matrix<float>.Build.Dense(3, 3, arr);

            if (matA.Rank() < 3)
            {
                return new CollisionParameter(-1f, -1f, -1f);
            }

            var vecB = part.Pos - suf.Origin;
            var root = matA.Solve(vecB);
            return new CollisionParameter(root[0], root[1], root[2] / (float)part.Vec.Norm(2));
        }

        public static bool DoCollide(CollisionParameter cParam, Tuple<float, float> basisNorm)
        {
            if (cParam.CoefA < 0 || cParam.CoefB < 0)
            {
                return false;
            }

            float a = cParam.CoefA / basisNorm.Item1;
            float b = cParam.CoefB / basisNorm.Item2;
            
            if ((a + b) > 0.5)
            {
                return false;
            }
            return true;
        }

        public static Vector<float> RotateVector(in Vector<float> vector, in Vector<float> normalizedAxial, float radian)
        {
            float mcos = 1 - MathF.Cos(radian);
            float cos = MathF.Cos(radian);
            float sin = MathF.Sin(radian);
            var nx = normalizedAxial.ElementAt(0);
            var ny = normalizedAxial.ElementAt(1);
            var nz = normalizedAxial.ElementAt(2);

            Matrix<float> r = Matrix<float>.Build.Dense(3, 3, 
                [
                    nx * nx * mcos + cos,
                    nx * ny * mcos + nz * sin,
                    nz * nx * mcos - ny * sin,
                    
                    nx * ny * mcos - nz * sin,
                    ny * ny * mcos + cos,
                    ny * nz * mcos + nx * sin,
                    
                    nz * nx * mcos + ny * sin,
                    ny * nz * mcos - nx * sin,
                    nz * nz * mcos + cos,
                ]);

            return r * vector;
        }

        public static Vector<float> CalcMainOutVec(in ISurface suf, in IParticle part)
        {
            var norm = suf.NormVec.Normalize(2f);
            var inVec = part.Vec.Multiply(-1f);
            if (norm.DotProduct(inVec) > 0)
            {
                norm = norm.Multiply(-1f);
            }

            return RotateVector(inVec, norm, MathF.PI);
        }

        public static CollisionResult FindCollisionSurface(in IParticle part, in List<ISurface> surfaces)
        {
            var collisions = new List<CollisionResult>();
            
            foreach (var suf in surfaces)
            {
                if (part.LastCollidedSurfaceId == suf.Id) 
                    continue;

                var cParam = CalcCollisionParam(suf, part);
                if (DoCollide(cParam, suf.BasisNorm))
                    collisions.Add(new CollisionResult(cParam, suf));
            }

            if (collisions.Count == 0)
                return CollisionResult.CreateDefault();
            return collisions
                .Where(colRes => colRes.CollisionParame.Dist >= 0)
                .MinBy(colRes => colRes.CollisionParame.Dist) ?? CollisionResult.CreateDefault();
        }
    }
}
