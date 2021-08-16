using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SISTEMA DE CAIXA DE DI�LOGO
/// V1.0
/// 
/// ULTIMA ATUALIZA��O EM 14-08-2021
/// DEV: IVAN CORREIA LIMA COQUEIRO
/// 
/// BASEADO NO C�DIGO DE BRACKEYS
/// </summary> 

public class DialogueTrigger : MonoBehaviour
{

    public Dialogue dialogue;

    public Animator NpcAnimator;

    
    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, NpcAnimator);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TriggerDialogue();
        }
            
    }

}
