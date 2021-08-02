using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MouseLook : MonoBehaviour
{
    public Animator anim;

    public float mouseSensitivity = 50f;

    public Transform playerBody;

    CinemachineVirtualCamera[] vcams;
    CinemachineVirtualCamera curr_vcam;

    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        //vcams = [cam GameObject.FindGameObjectsWithTag("VirtualCam")];
        //foreach (CinemachineVirtualCamera vcam in GameObject.FindGameObjectsWithTag("VirtualCam"))
        //{
        //    print(vcam);
        //}
        //if(vcams != null)
        //{
        //    foreach (CinemachineVirtualCamera cam in vcams)
        //    {
        //        print(cam);
        //    }
            
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


        //xRotation -= mouseY;
        //xRotation = Mathf.Clamp(xRotation, -90f, 90f); //Clamping

        //transform.localRotation = Quaternion.Euler(xRotation,0f,0f);

        //anim.SetFloat("mouseX", Input.GetAxis("Mouse X"));

        //playerBody.Rotate(Vector3.up * mouseX);

    }
}
