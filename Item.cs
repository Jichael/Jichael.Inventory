using UnityEngine;

namespace Inventory
{
    public class Item : MonoBehaviour
    {
        public ItemSO itemSO;
        public virtual void PickUp() { }

        public virtual void Drop() { }
    }
}