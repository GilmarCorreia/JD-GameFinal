using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePiece : MonoBehaviour
{
    public bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        clicked = true;
        print("cliquei");
    }
}
