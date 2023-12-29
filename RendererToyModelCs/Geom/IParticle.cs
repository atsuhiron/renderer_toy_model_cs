using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Chromatic;

namespace RendererToyModelCs.Geom
{
    public interface IParticle
    {
        Vector<float> GetPos();
        Vector<float> GetVec();
        float GetIntensity();
        Guid GetUuid();
        string GetId();
        string GetParentId();
        int GetGeneration();
        CLight GetLight();
        string getlstCollidedSurfaceId();

        //static IParticle CreateInverseTraceParticle();
        //static IParticle CreateTerminatedParticle();
    }
}
