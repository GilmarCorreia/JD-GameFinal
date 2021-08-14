using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// SISTEMA DE CAIXA DE DI�LOGO
/// V1.0
/// 
/// ULTIMA ATUALIZA��O EM 14-08-2021
/// DEV: IVAN CORREIA LIMA COQUEIRO
/// 
/// BASEADO NO C�DIGO DE BRACKEYS
/// </summary>


//gerencia o comportamento do dialogo
public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;

    private Queue<string> sentences;                //cria uma qeue (funcionar� como string[] array)


    //m�todo de inicio da rotina
    void Start()
    {
        sentences = new Queue<string>();
    }

    //inicia o dialogo
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting Conversation with " + dialogue.name); //fala o nome do cidad�o
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
            EndDialogue();              //chama a fun��o fim de dialogo
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
