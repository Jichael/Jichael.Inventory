using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory
{
    public class UIItemSlot : MonoBehaviour
    {
        [SerializeField] private Image itemIcon;
        [SerializeField] private TMP_Text itemName;
        [SerializeField] private GameObject border;
        [SerializeField] private GameObject heldIcon;
        
        
        public Button button;
        
        public Item AssignedItem { get; private set; }

        public void AssignItem(Item item)
        {
            AssignedItem = item;
            itemIcon.sprite = AssignedItem.itemSO.icon;
            itemName.text = AssignedItem.itemSO.name;

            if (AssignedItem is HandHeldItem handHeldItem)
            {
                handHeldItem.OnHeldState += OnHeldState;
            }
        }

        private void OnHeldState(bool held)
        {
            heldIcon.SetActive(held);
        }

        public void ResetSlot()
        {
            if (AssignedItem is HandHeldItem handHeldItem)
            {
                handHeldItem.OnHeldState -= OnHeldState;
            }
            
            AssignedItem = null;
            itemIcon.sprite = null;
            itemName.text = "Vide";
            
            Destroy(gameObject);
        }

        public void Select()
        {
            border.SetActive(true);
        }

        public void UnSelect()
        {
            border.SetActive(false);
        }
    }
}