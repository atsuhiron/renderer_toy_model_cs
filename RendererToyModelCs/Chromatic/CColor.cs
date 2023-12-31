﻿using MathNet.Numerics.LinearAlgebra;

namespace RendererToyModelCs.Chromatic
{
    public class CColor(Vector<float> elememts) : BaseChromatic(elememts)
    {
        public CLight ToLight()
        {
            return new CLight(1 - Elements);
        }

        public bool IsDark() => Elements.Sum() <= 0;
    }
}
