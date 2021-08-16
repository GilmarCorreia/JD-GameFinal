using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// CÓDIGO PARA A HEALTHBAR DO MONSTRO
/// 
/// FUNÇÃO: ELA DESCREVE O COMPORTAMENTO DA VIDA DO MONSTRO E QUANDO ELE RECEBE DANO
/// 
/// DEV: IVAN CORREIA LIMA COQUEIRO
/// ULTIMA MUDANÇA EM 11 DE AGOSTO DE 2021
///  VERSÃO 1.0
///  
/// </summary>

public class HealthbarScript : MonoBehaviour
{
    //adiciona um botão de correr (slider)
    public Slider slider;

    //deixa a saude do monstro no maximo (no inicio)
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

   //mostra a saude do monstro
   public void SetHealth(int health)
    {
        slider.value = health;
    }

}
