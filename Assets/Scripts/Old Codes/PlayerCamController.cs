using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCamController : MonoBehaviour
{
    CinemachineVirtualCamera VCam;
    Transform followTransform;
    //ErikaArcher player;

    float rotationStrength = -1f;
    public float maxAimVertRotation;
    public float minAimVertRotation;
    float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        VCam = GetComponent<CinemachineVirtualCamera>();
        //VCam = (CinemachineVirtualCamera)GetComponent<CinemachineBrain>().ActiveVirtualCamera;
        //print(VCam.Follow);
        followTransform = VCam.Follow.transform;
        //player = VCam.Follow.GetComponentInParent<ErikaArcher>();
    }

    // Update is called once per frame
    void Update()
    {
        mouseY = Input.GetAxis("Mouse Y");

        //print(player.mouseYInput);
        followTransform.rotation *= Quaternion.AngleAxis(mouseY * rotationStrength, Vector3.right);
        var angles = followTransform.localEulerAngles;
        angles.z = 0f;
        var angle = angles.x;
        if (angle < minAimVertRotation || angle > 180)
            angles.x = minAimVertRotation;
        else if (angle > maxAimVertRotation)
        {
            angles.x = maxAimVertRotation;
        }
        //print(angles);
        followTransform.localEulerAngles = angles;
    }
}
