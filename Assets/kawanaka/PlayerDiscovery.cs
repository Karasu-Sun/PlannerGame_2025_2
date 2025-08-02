using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// プレイヤー視覚によるアイテムの発見

public class PlayerDiscovery : MonoBehaviour
{
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float discoverDistance = 10f;
    [SerializeField] private LayerMask discoverableMask;

    private void Update()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, discoverDistance, discoverableMask))
        {
            DiscoverableObject discoverable = hit.collider.GetComponent<DiscoverableObject>();
            if (discoverable != null)
            {
                discoverable.Discover();
            }
        }
    }
}