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

        public static IEnumerable<object[]> ToRGBCodeTestData()
        {
            yield return new object[] { new float[] { 0f, 0f, 0f }, 0 };
            yield return new object[] { new float[] { 0f, 0f, 1f }, 255 };
            yield return new object[] { new float[] { 1f, 0f, 0f }, 16711680 };
            yield return new object[] { new float[] { 1f, 1f, 1f }, 16777215 };
        }

        [Theory]
        [MemberData(nameof(ToRGBCodeTestData))]
        public void ToRGBCodeTest(float[] colorElement, int expectedCode)
        {
            var color = new CColor(Vector<float>.Build.DenseOfArray(colorElement));

            var actualCode = color.ToRGBCode();
            Assert.Equal(expectedCode, actualCode);
        }
    }
}
