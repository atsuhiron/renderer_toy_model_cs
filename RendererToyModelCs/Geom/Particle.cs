using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Chromatic;

namespace RendererToyModelCs.Geom
{
    public record Particle : IParticle
    {
        public Particle(Vector<float> pos, Vector<float> vec, float intensity)
        {

        }

        public int GetGeneration()
        {
            throw new NotImplementedException();
        }

        public string GetId()
        {
            throw new NotImplementedException();
        }

        public float GetIntensity()
        {
            throw new NotImplementedException();
        }

        public CLight GetLight()
        {
            throw new NotImplementedException();
        }

        public string getlstCollidedSurfaceId()
        {
            throw new NotImplementedException();
        }

        public string GetParentId()
        {
            throw new NotImplementedException();
        }

        public Vector<float> GetPos()
        {
            throw new NotImplementedException();
        }

        public Guid GetUuid()
        {
            throw new NotImplementedException();
        }

        public Vector<float> GetVec()
        {
            throw new NotImplementedException();
        }
    }
}
