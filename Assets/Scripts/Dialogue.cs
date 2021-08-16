using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SISTEMA DE CAIXA DE DIÁLOGO
/// V1.0
/// 
/// ULTIMA ATUALIZAÇÃO EM 14-08-2021
/// DEV: IVAN CORREIA LIMA COQUEIRO
/// 
/// BASEADO NO CÓDIGO DE BRACKEYS
/// </summary>


[System.Serializable]

//Cria variaveis para o dialogo, sendo essas o nome e as falas
public class Dialogue
{
    public string NpcName;

    [TextArea(3,10)] 
    public string[] sentences;


}
