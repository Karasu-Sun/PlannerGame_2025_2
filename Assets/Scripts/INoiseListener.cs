using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public interface INoiseListener
    {
        void OnHearNoise(Vector3 sourcePosition, float radius);
    }
}