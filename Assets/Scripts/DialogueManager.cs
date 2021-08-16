using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


/// <summary>
/// SISTEMA DE CAIXA DE DIÁLOGO
/// V1.0
/// 
/// ULTIMA ATUALIZAÇÃO EM 14-08-2021
/// DEV: IVAN CORREIA LIMA COQUEIRO
/// 
/// BASEADO NO CÓDIGO DE BRACKEYS
/// </summary>


//gerencia o comportamento do dialogo
public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    private Queue<string> sentences;                //cria uma qeue (funcionará como string[] array)

    public Animator animator;
    private Animator npcAnimator;

    public PlayerInput playerInput;
    public GameObject player;

    //método de inicio da rotina
    void Start()
    {
        sentences = new Queue<string>();
    }


    private void Update()
    {
        if (animator.GetBool("IsOpen"))
        {
            Cursor.visible = true;
        }
    }

    //inicia o dialogo
    public void StartDialogue(Dialogue dialogue, Animator npcAnimator)
    {

        
        //Debug.Log("Starting Conversation with " + dialogue.name); //fala o nome do cidadão
        animator.SetBool("IsOpen", true);

        playerInput.actions.Disable();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        if (npcAnimator != null)
        {
            this.npcAnimator = npcAnimator;
            npcAnimator.SetBool("HasEnter", true);
        }

        nameText.text = dialogue.NpcName;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();

    }
    
    //ao encerrar uma fala, apaga a fala antiga e inicia uma nova
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();              //chama a função fim de dialogo
            return;
        }
        string sentence = sentences.Dequeue();      // da dequeue nos dados guardados
                                                    // Debug.Log(sentence);                        // mostra a proxima fala

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }
    //info de fim de dialogo
    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);

        if (this.npcAnimator != null)
        {
            npcAnimator.SetBool("HasEnter", false);
        }

        Cursor.lockState = CursorLockMode.Locked;
        playerInput.actions.Enable();
        Cursor.visible = false;
        
        //Debug.Log("End of dialogue.");
    }
}
