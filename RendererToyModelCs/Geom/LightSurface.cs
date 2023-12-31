﻿using MathNet.Numerics.LinearAlgebra;
using RendererToyModelCs.Algorithm;
using RendererToyModelCs.Chromatic;

namespace RendererToyModelCs.Geom
{
    public class LightSurface(List<Vector<float>> points, string? name, CLight light) : BaseSurface(points, name)
    {
        public override SurfaceType SufType => SurfaceType.Light;

        public CLight Light { get; init; } = light;

        public override List<IParticle> GetCollisionParticle(in IParticle inParticle, in CollisionParameter cParam, int num)
        {
            return
            [
                Particle.CreateTerminatedParticle(inParticle, Light)
            ];
        }
    }
}
