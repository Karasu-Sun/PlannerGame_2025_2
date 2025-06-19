using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kawanaka
{
    public class NoiseEmitter : MonoBehaviour
    {
        public static void EmitNoise(Vector3 position, float radius)
        {
            Collider[] colliders = Physics.OverlapSphere(position, radius);
            foreach (Collider col in colliders)
            {
                INoiseListener listener = col.GetComponent<INoiseListener>();
                if (listener != null)
                {
                    listener.OnHearNoise(position, radius);
                }
            }
        }
    }
}