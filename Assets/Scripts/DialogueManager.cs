using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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


    //método de inicio da rotina
    void Start()
    {
        sentences = new Queue<string>();
    }

    //inicia o dialogo
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting Conversation with " + dialogue.name); //fala o nome do cidadão
        nameText.text = dialogue.name;

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
        dialogueText.text = sentence;
    }

    //info de fim de dialogo
    void EndDialogue()
    {
        Debug.Log("End of dialogue.");
    }
}
