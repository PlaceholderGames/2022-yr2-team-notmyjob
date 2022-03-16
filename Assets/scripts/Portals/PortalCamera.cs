using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform playerCamera;

    public Transform portalTarget;
    public Transform currentPortal;

    // Update is called once per frame
    void Update()
    {
        Vector3 playerOffsetFromPortal = playerCamera.position - currentPortal.position;
        transform.position = portalTarget.position + playerOffsetFromPortal;

        float angularDifferenceBetweenPortals = Quaternion.Angle(portalTarget.rotation, currentPortal.rotation);
        Quaternion portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortals, Vector3.up);

        Vector3 newCameraDirection = (portalRotationalDifference * playerCamera.forward);
        transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
    }
}
