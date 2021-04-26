using System;
using UnityEngine;

namespace CustomPackages.Silicom.Inventory
{
    public class Item : MonoBehaviour
    {
        public ItemSO itemSO;
        public ItemCategory category;
        
        public bool PickedUp { get; private set; }

        public virtual void PickUp()
        {
            PickedUp = true;
            OnPickUp?.Invoke(this);
        }

        public virtual void Drop()
        {
            PickedUp = false;
            OnDrop?.Invoke(this);
        }

        public event Action<Item> OnPickUp;
        public event Action<Item> OnDrop;
    }

    public enum ItemCategory
    {
        HandHeld,
        Equipment,
        ToolBox,
        Other
    }
}