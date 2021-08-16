using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelmaSounds : MonoBehaviour
{

    public AudioSource footSource;
    public AudioSource faceSource;

    public AudioClip walkSound;
    public AudioClip runSound;
    public AudioClip diveSound;
    

    public void WalkSound()
    {
        footSource.clip = walkSound;
        footSource.Play();
    }

    public void RunSound()
    {
        footSource.clip = runSound;
        footSource.Play();
    }

    public void DiveSound()
    {
        faceSource.clip = diveSound;
        faceSource.Play();
    }
}
