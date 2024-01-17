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

        [Fact]
        public void AddLightsTest_Empty()
        {
            var lights = new List<CLight>();
            var itst = new List<float>();

            (CLight syntheLight, float syntheInts) = CLight.AddLights(lights, itst);
            
            Assert.Equal(0f, syntheInts);

            var expectedLightElement = Vector<float>.Build.Dense(3, 0f);
            Assert.Equal(expectedLightElement, syntheLight.Elements);
        }

        [Fact]
        public void AddLightsTest_Single()
        {
            var lights = new List<CLight>() { new(Vector<float>.Build.Dense(3, 0.25f)) };
            var itst = new List<float>() { 0.5f };

            (CLight syntheLight, float syntheInts) = CLight.AddLights(lights, itst);
            
            Assert.Equal(0.5f, syntheInts);

            var expectedLightElement = Vector<float>.Build.Dense(3, 0.25f);
            Assert.Equal(expectedLightElement, syntheLight.Elements);
        }

        [Fact]
        public void AddLightsTest_Double()
        {
            var lights = new List<CLight>()
            {
                new(Vector<float>.Build.Dense(3, 0.25f)),
                new(Vector<float>.Build.Dense(3, 0.5f))
            };
            var itst = new List<float>()
            {
                0.25f,
                0.125f
            };

            (CLight syntheLight, float syntheInts) = CLight.AddLights(lights, itst);

            Assert.Equal(0.375f, syntheInts);

            var expectedLightElement = Vector<float>.Build.Dense(3, 0.75f);
            Assert.Equal(expectedLightElement, syntheLight.Elements);
        }

        [Fact]
        public void AddLightsTest_ContainDark()
        {
            var lights = new List<CLight>()
            {
                new(Vector<float>.Build.Dense(3, 0.25f)),
                new(Vector<float>.Build.Dense(3, 0.5f)),
                new(Vector<float>.Build.Dense(3, 1f))  // Dark
            };
            var itst = new List<float>()
            {
                0.25f,
                0.25f,
                0.25f
            };

            (CLight syntheLight, float syntheInts) = CLight.AddLights(lights, itst);

            Assert.Equal(0.5f, syntheInts);

            var expectedLightElement = Vector<float>.Build.Dense(3, 0.6875f);
            Assert.Equal(expectedLightElement, syntheLight.Elements);
        }

        [Fact]
        public void AddLightsTest_DifferentLength()
        {
            var lights = new List<CLight>()
            {
                new(Vector<float>.Build.Dense(3, 0.25f)),
                new(Vector<float>.Build.Dense(3, 0.5f))
            };
            var itst = new List<float>()
            {
                0.25f,
                0.25f,
                0.25f
            };

            var excep = Assert.Throws<ArgumentException>(() => CLight.AddLights(lights, itst));
            Assert.Contains("must have same length", excep.Message);
        }

        [Fact]
        public void CreateDarkTest()
        {
            var expected = Vector<float>.Build.Dense(3, 1f);
            var actualLight = CLight.CreateDark();

            Assert.Equal(expected, actualLight.Elements);
        }
    }
}
