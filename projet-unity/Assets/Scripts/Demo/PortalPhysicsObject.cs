using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class PortalPhysicsObject : PortalTraveller {

    new Rigidbody rigidbody;
    public Color[] colors;
    static int i;
    static int i2;

    void Awake () {
        rigidbody = GetComponent<Rigidbody> ();

        if(GraphicsObject.GetComponent<MeshRenderer>())
            GraphicsObject.GetComponent<MeshRenderer>().material.color = colors[i];

        if (GraphicsObject.GetComponent<SkinnedMeshRenderer>())
            GraphicsObject.GetComponent<SkinnedMeshRenderer>().material.color = colors[i];

        i++;
        if (i > colors.Length - 1)
        {
            i = 0;
        }

        if (GraphicsObject2)
        {
            if (GraphicsObject2.GetComponent<MeshRenderer>())
                GraphicsObject2.GetComponent<MeshRenderer>().material.color = colors[i2];

            if (GraphicsObject2.GetComponent<SkinnedMeshRenderer>())
                GraphicsObject2.GetComponent<SkinnedMeshRenderer>().material.color = colors[i2];

            i2++;
            if (i2 > colors.Length - 1)
            {
                i2 = 0;
            }
        }
    }

    public override void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot) {
        base.Teleport (fromPortal, toPortal, pos, rot);
        rigidbody.velocity = toPortal.TransformVector (fromPortal.InverseTransformVector (rigidbody.velocity));
        rigidbody.angularVelocity = toPortal.TransformVector (fromPortal.InverseTransformVector (rigidbody.angularVelocity));
    }
}