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


[System.Serializable]

//Cria variaveis para o dialogo, sendo essas o nome e as falas
public class Dialogue
{
    public string NpcName;

    [TextArea(3,10)] 
    public string[] sentences;


}
