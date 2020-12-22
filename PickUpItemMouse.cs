using CustomPackages.Silicom.Player.Interactions;
using CustomPackages.Silicom.Player.Players.MouseKeyboard;
using Inventory;
using UnityEngine;

public class PickUpItemMouse : MonoBehaviour, IMouseInteract
{
    [SerializeField] private Item item;
    
    public bool DisableInteraction { get; }
    public void LeftClick(MouseController mouseController)
    {
        Inventory.Inventory.Current.Add(item);
    }

    public void RightClick(MouseController mouseController)
    {
        
    }

    public void HoverEnter(MouseController mouseController)
    {
        
    }

    public void HoverStay(MouseController mouseController)
    {
        
    }

    public void HoverExit(MouseController mouseController)
    {
        
    }
}