using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLights : MonoBehaviour
{
    // Start is called before the first frame update

    private AudioSource generator;

    private bool audioStarted = false;

    public Light[] lights = new Light[10];

    private void Awake()
    {
        foreach (Light lightObject in lights)
        {
            lightObject.intensity = 0f;
        }
    }
    void Start()
    {
        generator = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            if (!audioStarted) { 
                generator.Play();
                audioStarted = true;
            }   

            foreach (Light lightObject in lights)
            {
                lightObject.intensity = 300f;
            }
        }
    }
 
    // Update is called once per frame
    void Update()
    {
    }
}
