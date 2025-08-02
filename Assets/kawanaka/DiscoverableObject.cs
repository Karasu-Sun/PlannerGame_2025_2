using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;

// 発見アイテムハイライト中継器

public class DiscoverableObject : MonoBehaviour
{
    [Header("発見状態")]
    public bool isDiscovered = false;

    [Header("ハイライト＆コライダー")]
    [SerializeField] private Outline outline; // 任意のハイライト制御コンポーネント
    [SerializeField] private Collider targetCollider;

    public void Discover()
    {
        if (isDiscovered) return;

        isDiscovered = true;

        if (outline != null)
            outline.eraseRenderer = false;

        if (targetCollider != null)
            targetCollider.enabled = true;
    }
}