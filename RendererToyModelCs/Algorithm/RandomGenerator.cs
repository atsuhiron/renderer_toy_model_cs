using MathNet.Numerics.Random;

namespace RendererToyModelCs.Algorithm
{
    public static class RandomGenerator
    {
        private static int s_seed;

        static RandomGenerator()
        {
            s_seed = 8492;
        }

        public static List<float> GenrateRandom(int size)
        {
            if (size == 0) return [];
            var randoms = SystemRandomSource.Doubles(length: size, seed: s_seed).Select(x => (float)x).ToList();
            s_seed = (int)(randoms.Last() * 1000);
            return randoms;
        }
    }
}
