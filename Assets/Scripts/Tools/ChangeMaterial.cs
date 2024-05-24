using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    [SerializeField] MaterialChange[] materialChanges;

    [SerializeField] MeshRenderer[] meshRenderers;

    public void ChangeMaterials(int index)
    {
        if (index < 0 || index >= materialChanges.Length)
        {
            Debug.LogError("Index out of range");
            return;
        }

        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = materialChanges[index].Material;
        }

    }

    public void ChangeMaterials(string name)
    {
        var material = materialChanges.FirstOrDefault(x => x.Name == name);
        if (material == null)
        {
            Debug.LogError("Material " + name + " not found");
            return;
        }

        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = material.Material;
        }
    }

    public void ChangeM(Material m) { }

    [Serializable]
    class MaterialChange
    {
        public string Name;
        public Material Material;
    }
}
