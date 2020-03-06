using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour {

    public GameObject GraphicsObject;
    public GameObject GraphicsObject2;
    public GameObject GraphicsClone { get; set; }
    public GameObject GraphicsClone2 { get; set; }
    public Vector3 PreviousOffsetFromPortal { get; set; }

    public Material[] OriginalMaterials { get; set; }
    public Material[] OriginalMaterials2 { get; set; }
    public Material[] CloneMaterials { get; set; }
    public Material[] CloneMaterials2 { get; set; }

    public virtual void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        transform.position = pos;
        transform.rotation = rot;
    }

    // Called when first touches portal
    public virtual void EnterPortalThreshold () {
        if (GraphicsClone == null) {
            GraphicsClone = Instantiate (GraphicsObject);
            GraphicsClone.transform.parent = GraphicsObject.transform.parent;
            GraphicsClone.transform.localScale = GraphicsObject.transform.localScale;
            OriginalMaterials = GetMaterials(GraphicsObject);
            CloneMaterials = GetMaterials (GraphicsClone);
        } else {
            GraphicsClone.SetActive (true);
        }

        if (GraphicsObject2)
        {
            if (GraphicsClone2 == null)
            {
                GraphicsClone2 = Instantiate(GraphicsObject2);
                GraphicsClone2.transform.parent = GraphicsObject2.transform.parent;
                GraphicsClone2.transform.localScale = GraphicsObject2.transform.localScale;
                OriginalMaterials2 = GetMaterials(GraphicsObject2);
                CloneMaterials2 = GetMaterials(GraphicsClone2);
            }
            else
            {
                GraphicsClone2.SetActive(true);
            }
        }
    }

    // Called once no longer touching portal (excluding when teleporting)
    public virtual void ExitPortalThreshold () {
        GraphicsClone.SetActive (false);
        if (GraphicsObject2)
            GraphicsClone2.SetActive(false);
        // Disable slicing
        for (int i = 0; i < OriginalMaterials.Length; i++) {
            OriginalMaterials[i].SetVector ("sliceNormal", Vector3.zero);
        }

        if (GraphicsObject2)
        {
            for (int i2 = 0; i2 < OriginalMaterials2.Length; i2++)
            {
                OriginalMaterials2[i2].SetVector("sliceNormal", Vector3.zero);
            }
        }
    }

    public void SetSliceOffsetDst (float dst, bool clone) {
        for (int i = 0; i < OriginalMaterials.Length; i++) {
            if (clone) {
                CloneMaterials[i].SetFloat ("sliceOffsetDst", dst);
            } else {
                OriginalMaterials[i].SetFloat ("sliceOffsetDst", dst);
            }

        }

        if (GraphicsObject2)
        {
            for (int i2 = 0; i2 < OriginalMaterials2.Length; i2++)
            {
                if (clone) {
                    CloneMaterials2[i2].SetFloat("sliceOffsetDst", dst);
                } else {
                    OriginalMaterials2[i2].SetFloat("sliceOffsetDst", dst);
                }
            }
        }
    }

    Material[] GetMaterials (GameObject g) {
        var renderers = g.GetComponentsInChildren<MeshRenderer> ();
        var matList = new List<Material> ();
        foreach (var renderer in renderers) {
            foreach (var mat in renderer.materials) {
                matList.Add (mat);
            }
        }
        return matList.ToArray ();
    }
}