using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] int indexMaterial = 0;
    [SerializeField] Vector2 direction;
    [SerializeField] float scrollSpeed = 1f;
    Material material;
    private void Start()
    {
        if (meshRenderer.materials.Length > indexMaterial)
            material = meshRenderer.materials[indexMaterial];
    }

    private void Update()
    {
        if (material == null)
            return;

        float offset = Time.time * scrollSpeed;
        material.mainTextureOffset = direction * offset;
    }
}
