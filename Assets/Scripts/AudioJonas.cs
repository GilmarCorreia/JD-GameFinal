using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioJonas : MonoBehaviour
{
    public AudioSource jonasSource;
    public AudioSource jonasAndar;
    public AudioSource jonasArco;
    public AudioSource jonasPuxa;
    public AudioSource jonasRola;

    public AudioClip jonasSourceClip;
    public AudioClip jonasAndarClip;
    public AudioClip jonasArcoClip;
    public AudioClip jonasPuxaClip;
    public AudioClip jonasRolaClip;

    public void AudioPulo()
    {
        jonasSource.clip = jonasSourceClip;
        jonasSource.Play();
    }

    public void AtiraFlecha()
    {
        jonasArco.clip = jonasArcoClip;
        jonasArco.Play();
    }

    public void PuxaArco()
    {
        jonasPuxa.clip = jonasPuxaClip;
        jonasPuxa.Play();
    }

    public void RolaMuito()
    {
        jonasRola.clip = jonasRolaClip;
        jonasRola.Play();
    }

    public void AndarNaTerra()
    {
        jonasAndar.clip = jonasAndarClip;
        jonasAndar.Play();
    }

}
