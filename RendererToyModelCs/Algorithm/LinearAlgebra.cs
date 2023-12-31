using MathNet.Numerics.LinearAlgebra;

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

        public static CollisionParameter CalcCollisionParam()
        {
            return null;
        }

        public static bool DoCollide(CollisionParameter cParam, Tuple<float, float> basisNorm)
        {
            if (cParam.CoefA < 0 || cParam.CoefB < 0)
            {
                return false;
            }

            float a = cParam.CoefA / basisNorm.Item1;
            float b = cParam.CoefA / basisNorm.Item2;
            
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
    }
}
