using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Chromatic;

namespace RendererToyModelCsTests.Chromatic
{
    public class CLightTests
    {
        public static IEnumerable<object[]> ToColorTestData()
        {
            yield return new float[][] { [1f, 1f, 1f], [0f, 0f, 0f] };
            yield return new float[][] { [0f, 0f, 0f], [1f, 1f, 1f] };
            yield return new float[][] { [1f, 0.5f, 0f], [0f, 0.5f, 1f] };
        }

        [Theory]
        [MemberData(nameof(ToColorTestData))]
        public void ToColorTest(float[] colorElement, float[] expectedElemet)
        {
            var light = new CLight(Vector<float>.Build.DenseOfArray(colorElement));
            var actualElement = light.ToColor().Elements;

            var expectedElement = Vector<float>.Build.DenseOfArray(expectedElemet);
            Assert.Equal(expectedElement, actualElement);
        }
    }
}
