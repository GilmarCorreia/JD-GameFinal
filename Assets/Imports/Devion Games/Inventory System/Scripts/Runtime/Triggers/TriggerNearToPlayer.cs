using DevionGames.UIWidgets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Trigger implementado para avisar ao player se ele está perto ou nao te um item, permitindo ou nao a animação para pega-lo
namespace DevionGames.InventorySystem
{
    public class TriggerNearToPlayer :  MonoBehaviour, ITriggerCameInRange, ITriggerWentOutOfRange
    {
        public void OnCameInRange(GameObject player)
        {
            player.SendMessage("UpdatePickItemFlag", true);
            player.SendMessage("UpdateNearGameObject", gameObject);
            Debug.Log("Came in range event");
        }
        
        public void OnWentOutOfRange(GameObject player)
        {
            player.SendMessage("UpdatePickItemFlag", false);
            Debug.Log("Went out of range event");
        }
        
        
        
    }
}