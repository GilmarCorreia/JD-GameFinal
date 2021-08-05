using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamController : MonoBehaviour
{
    [System.Serializable]
    public class CameraSettings
    {
        [Header("Camera Move Settings")]
        public float speed = 5;
        public float moveSpeed = 5;
        public float rotationSpeed = 5;
        public float orignal_fov = 70;
        public float new_fov = 20;
        public float mouseXSense = 5;
        public float mouseYSense = 5;
        public float maxClampAngle = 90;
        public float minClampAngle = -30;
    }
    [SerializeField]
    public CameraSettings camSettings;

    [System.Serializable]
    public class CameraInputSettings
    {
        public string MouseXAxis = "Mouse X";
        public string MouseYAxis = "Mouse Y";
        public string AimingInput = "Fire2";
    }
    [SerializeField]
    public CameraInputSettings camInputSettings;

    public InputSystem inputSys;
    public CinemachineStateDrivenCamera StateDrivenCam;
    public CinemachineVirtualCamera VCam;
    Animator camAnim;
    Transform center;
    Transform target;
    Camera mainCam;
    float cameraXrotation = 0;
    float cameraYrotation = 0;
    public InputSystem input;

    // Start is called before the first frame update

    void Start()
    {
        mainCam = Camera.main;
        FindPlayer();
        input = GetComponent<InputSystem>();
        camAnim = StateDrivenCam.GetComponent<Animator>();
        //VCam = GetComponent<CinemachineVirtualCamera>();
        //print(VCam.Follow);
        center = VCam.Follow.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!target)
            return;

        if (!Application.isPlaying)
            return;

        if (Input.GetAxis(input.input.forwardInput) == 0 && Input.GetAxis(input.input.strafeInput) == 0 || inputSys.isAiming)
        {
            RotateCamera();
            //RotateCameraHor();
            //RotateCameraVert();
        }
        else 
        {
            center.transform.rotation = Quaternion.RotateTowards(center.transform.rotation, target.transform.rotation, 200f * Time.deltaTime);
            cameraYrotation = 0;
            cameraXrotation = 0;
        }
        ZoomCamera();
    }

    private void LateUpdate()
    {
        //if (target)
        //    FollowPlayer();
        //else
        //    FindPlayer();
    }


    public void FindPlayer()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void FollowPlayer()
    {
        Vector3 moveVector = Vector3.Slerp(transform.position,target.transform.position,camSettings.moveSpeed * Time.deltaTime);
        transform.position = moveVector;
    }

    public void RotateCamera()
    {
        cameraYrotation += Input.GetAxis(camInputSettings.MouseXAxis) * camSettings.mouseXSense;
        cameraXrotation -= Input.GetAxis(camInputSettings.MouseYAxis) * camSettings.mouseYSense;
        cameraXrotation = Mathf.Clamp(cameraXrotation,camSettings.minClampAngle,camSettings.maxClampAngle);
        cameraYrotation = Mathf.Repeat(cameraYrotation,360);
        Vector3 rotatingAngle = new Vector3(cameraXrotation,cameraYrotation,0);
        Quaternion rotation = Quaternion.Slerp(center.transform.localRotation, Quaternion.Euler(rotatingAngle), camSettings.rotationSpeed * Time.deltaTime);
        center.transform.localRotation = rotation;
    }

    public void ZoomCamera()
    {
        if (Input.GetButton(camInputSettings.AimingInput) &&inputSys.armed)
        {
            camAnim.Play("AimCam");
        }
        else
        {
            camAnim.Play("PlayerCam");
        }
    }
}
