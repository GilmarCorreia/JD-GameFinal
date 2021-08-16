using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinerSound : MonoBehaviour
{
    private AudioSource minerSource;
    // Start is called before the first frame update
    void Start()
    {
        minerSource = GetComponent<AudioSource>();
    }

    public void HitRock()
    {
        minerSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
