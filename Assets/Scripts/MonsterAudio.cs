using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAudio : MonoBehaviour
{
    public AudioSource MSource;
    public AudioSource MRugido;
    public AudioSource Mataque;
    public AudioSource Mmorre;
    public AudioSource Mmaos;
    public AudioSource Mboca;

    public AudioClip MSourceclip;
    public AudioClip MRugidoclip;
    public AudioClip Mataqueclip;
    public AudioClip Mmorreclip;
    public AudioClip Mmaosclip;
    public AudioClip Mbocaclip;


    public void MAndar()
    {
        MSource.clip = MSourceclip;
        MSource.Play();
    }

    public void MRugir()
    {
        MRugido.clip = MRugidoclip;
        MRugido.Play();
    }

    public void Mmorrer()
    {
        Mmorre.clip = Mmorreclip;
        Mmorre.Play();
    }

    public void Matacar()
    {
        Mataque.clip = Mataqueclip;
        Mataque.Play();
    }
    public void MmaosOuvir()
    {   
        Mmaos.clip = Mmaosclip;
        Mmaos.Play();
    }
    public void MbocaOuvir()
    {
        Mboca.clip = Mbocaclip;
        Mboca.Play();
    }
}
