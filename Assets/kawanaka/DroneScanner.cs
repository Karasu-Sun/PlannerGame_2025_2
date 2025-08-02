using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ドローンによるアイテムの発見

public class DroneScanner : MonoBehaviour
{
    [SerializeField] private Camera droneCamera;
    [SerializeField] private float scanInterval = 1f;

    private void Start()
    {
        InvokeRepeating(nameof(ScanVisibleObjects), 0f, scanInterval);
    }

    void ScanVisibleObjects()
    {
        Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(droneCamera);
        foreach (DiscoverableObject obj in FindObjectsOfType<DiscoverableObject>())
        {
            if (GeometryUtility.TestPlanesAABB(frustumPlanes, obj.GetComponent<Collider>().bounds))
            {
                obj.Discover();
            }
        }
    }
}