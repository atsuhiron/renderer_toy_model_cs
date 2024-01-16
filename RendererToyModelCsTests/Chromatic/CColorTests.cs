using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Chromatic;

namespace RendererToyModelCsTests.Chromatic
{
    public class CColorTests
    {
        public static IEnumerable<object[]> ToLightTestData()
        {
            yield return new float[][] { [1f, 1f, 1f], [0f, 0f, 0f] };
            yield return new float[][] { [0f, 0f, 0f], [1f, 1f, 1f] };
            yield return new float[][] { [1f, 0.5f, 0f], [0f, 0.5f, 1f] };
        }

        [Theory]
        [MemberData(nameof(ToLightTestData))]
        public void ToLightTest(float[] colorElement, float[] expectedElemet)
        {
            var color = new CColor(Vector<float>.Build.DenseOfArray(colorElement));
            var actualElement = color.ToLight().Elements;

            var expectedElement = Vector<float>.Build.DenseOfArray(expectedElemet);
            Assert.Equal(expectedElement, actualElement);
        }
    }
}
