using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtDebug
{
    //Draws just the box at where it is currently hitting.
    public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color)
    {
        origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
        DrawBox(origin, halfExtents, orientation, color);
    }

    //Draws the full box from start of cast to its end distance. Can also pass in hitInfoDistance instead of full distance
    public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color)
    {
        direction.Normalize();
        Box bottomBox = new Box(origin, halfExtents, orientation);
        Box topBox = new Box(origin + (direction * distance), halfExtents, orientation);

        Debug.DrawLine(bottomBox.BackBottomLeft, topBox.BackBottomLeft, color);
        Debug.DrawLine(bottomBox.BackBottomRight, topBox.BackBottomRight, color);
        Debug.DrawLine(bottomBox.BackTopLeft, topBox.BackTopLeft, color);
        Debug.DrawLine(bottomBox.BackTopRight, topBox.BackTopRight, color);
        Debug.DrawLine(bottomBox.FrontTopLeft, topBox.FrontTopLeft, color);
        Debug.DrawLine(bottomBox.FrontTopRight, topBox.FrontTopRight, color);
        Debug.DrawLine(bottomBox.FrontBottomLeft, topBox.FrontBottomLeft, color);
        Debug.DrawLine(bottomBox.FrontBottomRight, topBox.FrontBottomRight, color);

        DrawBox(bottomBox, color);
        DrawBox(topBox, color);
    }

    public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
    {
        DrawBox(new Box(origin, halfExtents, orientation), color);
    }
    public static void DrawBox(Box box, Color color)
    {
        Debug.DrawLine(box.FrontTopLeft, box.FrontTopRight, color);
        Debug.DrawLine(box.FrontTopRight, box.FrontBottomRight, color);
        Debug.DrawLine(box.FrontBottomRight, box.FrontBottomLeft, color);
        Debug.DrawLine(box.FrontBottomLeft, box.FrontTopLeft, color);

        Debug.DrawLine(box.BackTopLeft, box.BackTopRight, color);
        Debug.DrawLine(box.BackTopRight, box.BackBottomRight, color);
        Debug.DrawLine(box.BackBottomRight, box.BackBottomLeft, color);
        Debug.DrawLine(box.BackBottomLeft, box.BackTopLeft, color);

        Debug.DrawLine(box.FrontTopLeft, box.BackTopLeft, color);
        Debug.DrawLine(box.FrontTopRight, box.BackTopRight, color);
        Debug.DrawLine(box.FrontBottomRight, box.BackBottomRight, color);
        Debug.DrawLine(box.FrontBottomLeft, box.BackBottomLeft, color);
    }

    public struct Box
    {
        public Vector3 LocalFrontTopLeft { get; private set; }
        public Vector3 LocalFrontTopRight { get; private set; }
        public Vector3 LocalFrontBottomLeft { get; private set; }
        public Vector3 LocalFrontBottomRight { get; private set; }
        public Vector3 LocalBackTopLeft { get { return -LocalFrontBottomRight; } }
        public Vector3 LocalBackTopRight { get { return -LocalFrontBottomLeft; } }
        public Vector3 LocalBackBottomLeft { get { return -LocalFrontTopRight; } }
        public Vector3 LocalBackBottomRight { get { return -LocalFrontTopLeft; } }

        public Vector3 FrontTopLeft { get { return LocalFrontTopLeft + Origin; } }
        public Vector3 FrontTopRight { get { return LocalFrontTopRight + Origin; } }
        public Vector3 FrontBottomLeft { get { return LocalFrontBottomLeft + Origin; } }
        public Vector3 FrontBottomRight { get { return LocalFrontBottomRight + Origin; } }
        public Vector3 BackTopLeft { get { return LocalBackTopLeft + Origin; } }
        public Vector3 BackTopRight { get { return LocalBackTopRight + Origin; } }
        public Vector3 BackBottomLeft { get { return LocalBackBottomLeft + Origin; } }
        public Vector3 BackBottomRight { get { return LocalBackBottomRight + Origin; } }

        public Vector3 Origin { get; private set; }

        public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
        {
            Rotate(orientation);
        }
        public Box(Vector3 origin, Vector3 halfExtents)
        {
            this.LocalFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
            this.LocalFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
            this.LocalFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
            this.LocalFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

            this.Origin = origin;
        }


        public void Rotate(Quaternion orientation)
        {
            LocalFrontTopLeft = RotatePointAroundPivot(LocalFrontTopLeft, Vector3.zero, orientation);
            LocalFrontTopRight = RotatePointAroundPivot(LocalFrontTopRight, Vector3.zero, orientation);
            LocalFrontBottomLeft = RotatePointAroundPivot(LocalFrontBottomLeft, Vector3.zero, orientation);
            LocalFrontBottomRight = RotatePointAroundPivot(LocalFrontBottomRight, Vector3.zero, orientation);
        }
    }

    //This should work for all cast types
    static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance)
    {
        return origin + (direction.normalized * hitInfoDistance);
    }

    static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
    {
        Vector3 direction = point - pivot;
        return pivot + rotation * direction;
    }
}


public class Portal : MonoBehaviour {
    [Header ("Main Settings")]
    public Portal linkedPortal;
    public MeshRenderer screen;
    public int recursionLimit = 5;

    [Header ("Advanced Settings")]
    public float nearClipOffset = 0.05f;
    public float nearClipLimit = 0.2f;

    [Header("Corners")]
    public GameObject topleft;
    public GameObject topright;
    public GameObject bottomleft;
    public GameObject bottomright;


    // Private variables
    RenderTexture viewTexture;
    public Camera portalCam;
    Camera playerCam;
    List<PortalTraveller> trackedTravellers;
    MeshFilter screenMeshFilter;

    

    void Awake () {
        playerCam = Camera.main;
        portalCam = GetComponentInChildren<Camera> ();
        portalCam.enabled = false;
        trackedTravellers = new List<PortalTraveller> ();
        screenMeshFilter = screen.GetComponent<MeshFilter> ();
        screen.material.SetInt ("active", 1);
    }

    void LateUpdate () {
        HandleTravellers ();
    }

    void HandleTravellers() {

        for (int i = 0; i < trackedTravellers.Count; i++) {
            PortalTraveller traveller = trackedTravellers[i];
            if (traveller == null) // debugdestroy
                continue;

            if (traveller.GraphicsObject2 == null) // model 2 not here
            {
                Transform travellerT = traveller.transform;
                Transform model1T = traveller.GraphicsObject.transform;
                var t = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;
                var m1 = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * model1T.localToWorldMatrix;

                Vector3 offsetFromPortal = travellerT.position - transform.position;
                int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, transform.forward));
                int portalSideOld = System.Math.Sign(Vector3.Dot(traveller.PreviousOffsetFromPortal, transform.forward));
                // Teleport the traveller if it has crossed from one side of the portal to the other
                if (portalSide != portalSideOld)
                {
                    var positionOld = model1T.position;
                    var rotOld = model1T.rotation;
                    if (traveller.GetComponent<PhysicsObject>())
                    {
                        traveller.GetComponent<PhysicsObject>().Drop();
                        /*if (traveller.GetComponent<PhysicsObject>().m_Held)
                        {
                            GameObject mtraveller = traveller.GetComponent<PhysicsObject>().playercontroller.GetComponent<FPSWeapon>().m_HandTransform.gameObject;
                            Transform mtravellerT = mtraveller.transform;
                            var mt = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * mtravellerT.localToWorldMatrix;

                            var mpositionOld = model1T.position;
                            var mrotOld = model1T.rotation;

                            traveller.Teleport(transform, linkedPortal.transform, mt.GetColumn(3), mt.rotation);
                            traveller.GraphicsClone.transform.SetPositionAndRotation(mpositionOld, mrotOld);
                        }*/
                    }
                    /*else
                    {
                        traveller.Teleport(transform, linkedPortal.transform, t.GetColumn(3), t.rotation);
                        traveller.GraphicsClone.transform.SetPositionAndRotation(positionOld, rotOld);
                    }*/
                    traveller.Teleport(transform, linkedPortal.transform, t.GetColumn(3), t.rotation);
                    traveller.GraphicsClone.transform.SetPositionAndRotation(positionOld, rotOld);
                    // Can't rely on OnTriggerEnter/Exit to be called next frame since it depends on when FixedUpdate runs
                    linkedPortal.OnTravellerEnterPortal(traveller);
                    trackedTravellers.RemoveAt(i);
                    i--;

                }
                else
                {
                    traveller.GraphicsClone.transform.SetPositionAndRotation(m1.GetColumn(3), m1.rotation);
                    traveller.PreviousOffsetFromPortal = offsetFromPortal;
                }
            } else // model 2 is here
            {
                Transform travellerT = traveller.transform;
                Transform model1T = traveller.GraphicsObject.transform;
                Transform model2T = traveller.GraphicsObject2.transform;
                var t = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * travellerT.localToWorldMatrix;
                var m1 = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * model1T.localToWorldMatrix;
                var m2 = linkedPortal.transform.localToWorldMatrix * transform.worldToLocalMatrix * model2T.localToWorldMatrix;

                Vector3 offsetFromPortal = travellerT.position - transform.position;
                int portalSide = System.Math.Sign(Vector3.Dot(offsetFromPortal, transform.forward));
                int portalSideOld = System.Math.Sign(Vector3.Dot(traveller.PreviousOffsetFromPortal, transform.forward));
                // Teleport the traveller if it has crossed from one side of the portal to the other
                if (portalSide != portalSideOld)
                {
                    var positionOld = model1T.position;
                    var rotOld = model1T.rotation;
                    var positionOld2 = model2T.position;
                    var rotOld2 = model2T.rotation;
                    if (traveller.GetComponent<PhysicsObject>())
                        traveller.GetComponent<PhysicsObject>().Drop();
                    traveller.Teleport(transform, linkedPortal.transform, t.GetColumn(3), t.rotation);
                    traveller.GraphicsClone.transform.SetPositionAndRotation(positionOld, rotOld);
                    traveller.GraphicsClone2.transform.SetPositionAndRotation(positionOld2, rotOld2);
                    // Can't rely on OnTriggerEnter/Exit to be called next frame since it depends on when FixedUpdate runs
                    linkedPortal.OnTravellerEnterPortal(traveller);
                    trackedTravellers.RemoveAt(i);
                    i--;

                }
                else
                {
                    traveller.GraphicsClone.transform.SetPositionAndRotation(m1.GetColumn(3), m1.rotation);
                    traveller.GraphicsClone2.transform.SetPositionAndRotation(m2.GetColumn(3), m2.rotation);
                    traveller.PreviousOffsetFromPortal = offsetFromPortal;
                }
            }
        }
    }

    // Called before any portal cameras are rendered for the current frame
    public void PrePortalRender() {
        foreach (var traveller in trackedTravellers) {
            UpdateSliceParams(traveller);
        }
    }

    // Manually render the camera attached to this portal
    // Called after PrePortalRender, and before PostPortalRender
    public void Render () {

        // Skip rendering the view from this portal if player is not looking at the linked portal
        if (!CameraUtility.VisibleFromCamera (linkedPortal.screen, playerCam)) {
            return;
        }

        CreateViewTexture ();

        var localToWorldMatrix = playerCam.transform.localToWorldMatrix;
        var renderPositions = new Vector3[recursionLimit];
        var renderRotations = new Quaternion[recursionLimit];

        int startIndex = 0;
        portalCam.projectionMatrix = playerCam.projectionMatrix;
        for (int i = 0; i < recursionLimit; i++) {
            if (i > 0) {
                // No need for recursive rendering if linked portal is not visible through this portal
                if (!CameraUtility.BoundsOverlap (screenMeshFilter, linkedPortal.screenMeshFilter, portalCam)) {
                    break;
                }
            }
            localToWorldMatrix = transform.localToWorldMatrix * linkedPortal.transform.worldToLocalMatrix * localToWorldMatrix;
            int renderOrderIndex = recursionLimit - i - 1;
            renderPositions[renderOrderIndex] = localToWorldMatrix.GetColumn (3);
            renderRotations[renderOrderIndex] = localToWorldMatrix.rotation;

            portalCam.transform.SetPositionAndRotation (renderPositions[renderOrderIndex], renderRotations[renderOrderIndex]);
            startIndex = renderOrderIndex;
        }

        // Hide screen so that camera can see through portal
        screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        linkedPortal.screen.material.SetInt ("active", 0);

        for (int i = startIndex; i < recursionLimit; i++) {
            portalCam.transform.SetPositionAndRotation (renderPositions[i], renderRotations[i]);
            SetNearClipPlane ();
            HandleClipping ();
            portalCam.Render ();

            if (i == startIndex) {
                linkedPortal.screen.material.SetInt ("active", 1);
            }
        }

        // Unhide objects hidden at start of render
        screen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }

    void HandleClipping () {
        // There are two main graphical issues when slicing travellers
        // 1. Tiny sliver of mesh drawn on backside of portal
        //    Ideally the oblique clip plane would sort this out, but even with 0 offset, tiny sliver still visible
        // 2. Tiny seam between the sliced mesh, and the rest of the model drawn onto the portal screen
        // This function tries to address these issues by modifying the slice parameters when rendering the view from the portal
        // Would be great if this could be fixed more elegantly, but this is the best I can figure out for now
        const float hideDst = -1000;
        const float showDst = 1000;
        float screenThickness = linkedPortal.ProtectScreenFromClipping (portalCam.transform.position);

        foreach (var traveller in trackedTravellers) {
            if (traveller == null) // debugdestroy
            {
                continue;
            }
            if (SameSideOfPortal (traveller.transform.position, PortalCamPos)) {
                // Addresses issue 1
                traveller.SetSliceOffsetDst (hideDst, false);
            } else {
                // Addresses issue 2
                traveller.SetSliceOffsetDst (showDst, false);
            }

            // Ensure clone is properly sliced, in case it's visible through this portal:
            int cloneSideOfLinkedPortal = -SideOfPortal (traveller.transform.position);
            bool camSameSideAsClone = linkedPortal.SideOfPortal (PortalCamPos) == cloneSideOfLinkedPortal;
            if (camSameSideAsClone) {
                traveller.SetSliceOffsetDst (screenThickness, true);
            } else {
                traveller.SetSliceOffsetDst (-screenThickness, true);
            }
        }

        foreach (var linkedTraveller in linkedPortal.trackedTravellers) {
            if (linkedTraveller == null) // debugdestroy
            {
                continue;
            }
            var travellerPos = linkedTraveller.GraphicsObject.transform.position;
            //var clonePos = linkedTraveller.graphicsClone.transform.position;
            //var clonePos2 = linkedTraveller.graphicsClone2.transform.position;
            // Handle clone of linked portal coming through this portal:
            bool cloneOnSameSideAsCam = linkedPortal.SideOfPortal (travellerPos) != SideOfPortal (PortalCamPos);
            if (cloneOnSameSideAsCam) {
                // Addresses issue 1
                linkedTraveller.SetSliceOffsetDst (hideDst, true);
            } else {
                // Addresses issue 2
                linkedTraveller.SetSliceOffsetDst (showDst, true);
            }

            // Ensure traveller of linked portal is properly sliced, in case it's visible through this portal:
            bool camSameSideAsTraveller = linkedPortal.SameSideOfPortal (linkedTraveller.transform.position, PortalCamPos);
            if (camSameSideAsTraveller) {
                linkedTraveller.SetSliceOffsetDst (screenThickness, false);
            } else {
                linkedTraveller.SetSliceOffsetDst (-screenThickness, false);
            }
        }
    }

    // Called once all portals have been rendered, but before the player camera renders
    public void PostPortalRender () {
        foreach (var traveller in trackedTravellers) {
            UpdateSliceParams (traveller);
        }
        ProtectScreenFromClipping (playerCam.transform.position);
    }
    void CreateViewTexture () {
        if (viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height) {
            if (viewTexture != null) {
                viewTexture.Release ();
            }
            viewTexture = new RenderTexture (Screen.width, Screen.height, 0);
            // Render the view from the portal camera to the view texture
            portalCam.targetTexture = viewTexture;
            // Display the view texture on the screen of the linked portal
            linkedPortal.screen.material.SetTexture ("_MainTex", viewTexture);
        }
    }

    // Sets the thickness of the portal screen so as not to clip with camera near plane when player goes through
    float ProtectScreenFromClipping (Vector3 viewPoint) {
        float halfHeight = playerCam.nearClipPlane * Mathf.Tan (playerCam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * playerCam.aspect;
        float dstToNearClipPlaneCorner = new Vector3 (halfWidth, halfHeight, playerCam.nearClipPlane).magnitude;
        float screenThickness = dstToNearClipPlaneCorner;

        Transform screenT = screen.transform;
        bool camFacingSameDirAsPortal = Vector3.Dot (transform.forward, transform.position - viewPoint) > 0;
        screenT.localScale = new Vector3 (screenT.localScale.x, screenT.localScale.y, screenThickness);
        screenT.localPosition = Vector3.forward * screenThickness * ((camFacingSameDirAsPortal) ? 0.5f : -0.5f);
        return screenThickness;
    }

    void UpdateSliceParams (PortalTraveller traveller) {
        if (traveller == null)
            return;
        // Calculate slice normal
        int side = SideOfPortal (traveller.transform.position);
        Vector3 sliceNormal = transform.forward * -side;
        Vector3 cloneSliceNormal = linkedPortal.transform.forward * side;

        // Calculate slice centre
        Vector3 slicePos = transform.position;
        Vector3 cloneSlicePos = linkedPortal.transform.position;

        // Adjust slice offset so that when player standing on other side of portal to the object, the slice doesn't clip through
        float sliceOffsetDst = 0;
        float cloneSliceOffsetDst = 0;
        float screenThickness = screen.transform.localScale.z;

        bool playerSameSideAsTraveller = SameSideOfPortal (playerCam.transform.position, traveller.transform.position);
        if (!playerSameSideAsTraveller) {
            sliceOffsetDst = -screenThickness;
        }
        bool playerSameSideAsCloneAppearing = side != linkedPortal.SideOfPortal (playerCam.transform.position);
        if (!playerSameSideAsCloneAppearing) {
            cloneSliceOffsetDst = -screenThickness;
        }

        // Apply parameters
        for (int i = 0; i < traveller.OriginalMaterials.Length; i++) {
            traveller.OriginalMaterials[i].SetVector ("sliceCentre", slicePos);
            traveller.OriginalMaterials[i].SetVector ("sliceNormal", sliceNormal);
            traveller.OriginalMaterials[i].SetFloat ("sliceOffsetDst", sliceOffsetDst);

            traveller.CloneMaterials[i].SetVector ("sliceCentre", cloneSlicePos);
            traveller.CloneMaterials[i].SetVector ("sliceNormal", cloneSliceNormal);
            traveller.CloneMaterials[i].SetFloat ("sliceOffsetDst", cloneSliceOffsetDst);

        }
        if (traveller.GraphicsObject2 != null)
        {
            for (int i2 = 0; i2 < traveller.OriginalMaterials2.Length; i2++)
            {
                traveller.OriginalMaterials2[i2].SetVector("sliceCentre", slicePos);
                traveller.OriginalMaterials2[i2].SetVector("sliceNormal", sliceNormal);
                traveller.OriginalMaterials2[i2].SetFloat("sliceOffsetDst", sliceOffsetDst);

                traveller.CloneMaterials2[i2].SetVector("sliceCentre", cloneSlicePos);
                traveller.CloneMaterials2[i2].SetVector("sliceNormal", cloneSliceNormal);
                traveller.CloneMaterials2[i2].SetFloat("sliceOffsetDst", cloneSliceOffsetDst);

            }
        }
    }

    // Use custom projection matrix to align portal camera's near clip plane with the surface of the portal
    // Note that this affects precision of the depth buffer, which can cause issues with effects like screenspace AO
    void SetNearClipPlane () {
        // Learning resource:
        // http://www.terathon.com/lengyel/Lengyel-Oblique.pdf
        Transform clipPlane = transform;
        int dot = System.Math.Sign (Vector3.Dot (clipPlane.forward, transform.position - portalCam.transform.position));

        Vector3 camSpacePos = portalCam.worldToCameraMatrix.MultiplyPoint (clipPlane.position);
        Vector3 camSpaceNormal = portalCam.worldToCameraMatrix.MultiplyVector (clipPlane.forward) * dot;
        float camSpaceDst = -Vector3.Dot (camSpacePos, camSpaceNormal) + nearClipOffset;

        // Don't use oblique clip plane if very close to portal as it seems this can cause some visual artifacts
        if (Mathf.Abs (camSpaceDst) > nearClipLimit) {
            Vector4 clipPlaneCameraSpace = new Vector4 (camSpaceNormal.x, camSpaceNormal.y, camSpaceNormal.z, camSpaceDst);

            // Update projection based on new clip plane
            // Calculate matrix with player cam so that player camera settings (fov, etc) are used
            portalCam.projectionMatrix = playerCam.CalculateObliqueMatrix (clipPlaneCameraSpace);
        } else {
            portalCam.projectionMatrix = playerCam.projectionMatrix;
        }
    }

    void OnTravellerEnterPortal (PortalTraveller traveller) {
        if (!trackedTravellers.Contains (traveller)) {
            traveller.EnterPortalThreshold ();
            traveller.PreviousOffsetFromPortal = traveller.transform.position - transform.position;
            trackedTravellers.Add (traveller);
        }
    }

    void OnTriggerEnter (Collider other) {
        var traveller = other.GetComponent<PortalTraveller>();
        if (traveller) {
            OnTravellerEnterPortal (traveller);
        }
        if (other.gameObject.layer != 12)
            other.gameObject.layer = 9;
    }

    public Vector3 box_direction = Vector3.forward * 10;
    readonly List<GameObject> colored = new List<GameObject>();

    void Update()
    {
        CheckPath(transform.position, transform.position + box_direction);
    }

    private void CheckPath(Vector3 position, Vector3 target)
    {
        Quaternion rotation = Quaternion.LookRotation(target - position);
        Vector3 direction = target - position;
        float distance = Vector3.Distance(position, target);

        Vector3 positionz = Vector3.Lerp(position, target, 0.5f);
        Vector3 halfExtents = new Vector3(1.6f, 2.6f, (target - position).magnitude) / 2;
        ExtDebug.DrawBox(positionz, halfExtents, rotation, Color.green);

        // Debug.DrawRay(position, direction, result ? Color.green : Color.red);

        RaycastHit[] rhit = Physics.BoxCastAll(positionz, halfExtents, direction, rotation, distance);
        
        foreach (GameObject cube in colored)
        {
            if (!cube) // not valid
            {
                colored.Remove(cube);
                continue;
            }

            if (cube.layer != 12)
            {
                if (cube.GetComponent<Renderer>() && cube.tag == "Moonrock")
                {
                    Renderer rendererToEdit = cube.GetComponent<Renderer>();
                    foreach (Material mat in rendererToEdit.materials)
                    {
                        mat.color = Color.green;
                        cube.layer = 0;
                    }
                }
                else if (cube.tag == "Untagged")
                {
                    cube.layer = 0;
                }
            }
        }
        colored.Clear();
        foreach (RaycastHit uhit in rhit) {
            GameObject cube = uhit.collider.gameObject;
            if (cube.layer != 12)
            {
                if (cube.GetComponent<Renderer>() && cube.tag == "Moonrock")
                {
                    Renderer rendererToEdit = cube.GetComponent<Renderer>();
                    foreach (Material mat in rendererToEdit.materials)
                    {
                        mat.color = Color.red;
                        cube.layer = 8;
                    }
                    colored.Add(cube);
                }
                else if (cube.tag == "Untagged")
                {
                    cube.layer = 8;
                    colored.Add(cube);
                }
            }
        }
    }

    void OnTriggerExit (Collider other) {
        var traveller = other.GetComponent<PortalTraveller> ();
        if (traveller && trackedTravellers.Contains (traveller)) {
            traveller.ExitPortalThreshold ();
            trackedTravellers.Remove (traveller);
        }
        if (other.gameObject.layer != 12)
            other.gameObject.layer = 10;
    }

    /*
     ** Some helper/convenience stuff:
     */

    int SideOfPortal (Vector3 pos) {
        return System.Math.Sign (Vector3.Dot (pos - transform.position, transform.forward));
    }

    bool SameSideOfPortal (Vector3 posA, Vector3 posB) {
        return SideOfPortal (posA) == SideOfPortal (posB);
    }

    Vector3 PortalCamPos {
        get {
            return portalCam.transform.position;
        }
    }

    void OnValidate () {
        if (linkedPortal != null) {
            linkedPortal.linkedPortal = this;
        }
    }
}