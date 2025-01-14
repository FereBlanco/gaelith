using System;
using NUnit.Framework;
using UnityEngine;

public class FloorTileStyle : MonoBehaviour
{
    Renderer tileRenderer;
    [SerializeField] GameObject model;
    [SerializeField] Material[] materials;

    private void Awake() {
        Assert.IsNotNull(model, "ERROR: model is null");

        if (materials == null || materials.Length == 0) throw new Exception("ERROR: materials array is null or empty");
        foreach (var material in materials)
        {
            Assert.IsNotNull(material, "ERROR: any null material in the materials array");
        }

        tileRenderer = model.GetComponent<Renderer>();
    }

    public void SetMaterial(int num)
    {
        tileRenderer.material = materials[num];
    }
}