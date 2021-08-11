using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class FlashlightController : MonoBehaviour
{
    private StarterAssetsInputs _input;

    public Light spotLight;
    public GameObject glassMesh;
    private Material glassMaterial;

    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        glassMaterial = glassMesh.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        Flashlight();   
    }

    private void Flashlight()
    {
        if (_input.flashlight)
        {
            spotLight.intensity = 3000.0f;
            glassMaterial.SetFloat("_EmissiveExposureWeight", 0f);
        }
        else
        {
            spotLight.intensity = 0f;
            glassMaterial.SetFloat("_EmissiveExposureWeight", 1f);
        }
    }
}
