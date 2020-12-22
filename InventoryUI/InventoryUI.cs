using System;
using System.Collections.Generic;
using CustomPackages.Silicom.Player.CursorSystem;
using CustomPackages.Silicom.Player.Players;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inventory
{
    public class InventoryUI : MonoBehaviour
    {

        [SerializeField] private InputAction toggleInventoryAction;

        [SerializeField] private Transform toolBox;
        [SerializeField] private Transform protections;

        [SerializeField] private TMP_Text capacityText;
        

        [SerializeField] private UIItemSlot itemUITemplate;

        [SerializeField] private GameObject selectedItemContainer;
        [SerializeField] private TMP_Text selectedItemName;
        [SerializeField] private Image selectedItemIcon;
        [SerializeField] private TMP_Text selectedItemDescription;
        [SerializeField] private TMP_Text selectedItemWeight;
        private UIItemSlot _selectedItem;

        private readonly List<UIItemSlot> _itemsUIToolbox = new List<UIItemSlot>();
        private readonly List<UIItemSlot> _itemsUIProtections = new List<UIItemSlot>();

        private void Awake()
        {
            toggleInventoryAction.Enable();
            toggleInventoryAction.performed += OnToggleInventory;
            Inventory.OnItemAdded += OnItemAdded;
            Inventory.OnItemRemoved += OnItemRemoved;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            Inventory.OnItemAdded -= OnItemAdded;
            Inventory.OnItemRemoved -= OnItemRemoved;
            
            toggleInventoryAction.Disable();
            toggleInventoryAction.performed -= OnToggleInventory;
        }

        private void OnToggleInventory(InputAction.CallbackContext ctx)
        {
            ToggleInventory();
        }

        public void ToggleInventory()
        {
            bool opened = !gameObject.activeSelf;
            gameObject.SetActive(opened);
            
            // TODO : rework
            CursorManager.Instance.SetLockState(!opened);
            if (opened)
            {
                InputsHandler.Instance.StopInputProcessing();
            }
            else
            {
                InputsHandler.Instance.StartInputProcessing();
            }
        }

        private void OnEnable()
        {
            selectedItemContainer.SetActive(false);
            
            if (_itemsUIToolbox.Count > 0)
            {
                bool found = false;
                for (int i = 0; i < _itemsUIToolbox.Count; i++)
                {
                    if (((HandHeldItem) _itemsUIToolbox[i].AssignedItem).Held)
                    {
                        SetSelectedItem(_itemsUIToolbox[i]);
                        found = true;
                        break;
                    }
                }
                if(!found) SetSelectedItem(_itemsUIToolbox[0]);
            }
            else if (_itemsUIProtections.Count > 0)
            {
                SetSelectedItem(_itemsUIProtections[0]);
            }
        }

        public void DropButton()
        {
            Inventory.Current.Remove(_selectedItem.AssignedItem);
            SetSelectedItem(null);
        }

        private void SetSelectedItem(UIItemSlot item)
        {
            if(_selectedItem) _selectedItem.UnSelect();
            
            _selectedItem = item;

            if (_selectedItem)
            {
                selectedItemName.text = _selectedItem.AssignedItem.itemSO.name;
                selectedItemDescription.text = _selectedItem.AssignedItem.itemSO.description;
                selectedItemIcon.sprite = _selectedItem.AssignedItem.itemSO.icon;
                selectedItemWeight.text = $"Weight : {_selectedItem.AssignedItem.itemSO.weight.ToString()}";
                item.Select();
                selectedItemContainer.SetActive(true);
            }
            else
            {
                selectedItemContainer.SetActive(false);
            }
        }

        private void OnItemAdded(Item item)
        {
            UpdateCapacityText();
            if (item.itemSO.category == ItemCategory.Other)
            {
                
            }
            else if (item.itemSO.category == ItemCategory.HandHeld)
            {
                UIItemSlot itemUI = Instantiate(itemUITemplate, toolBox);
                itemUI.AssignItem(item);
                itemUI.button.onClick.AddListener(() => SetSelectedItem(itemUI));
                _itemsUIToolbox.Add(itemUI);
            }
            else if (item.itemSO.category == ItemCategory.Protection)
            {
                UIItemSlot itemUI = Instantiate(itemUITemplate, protections);
                itemUI.AssignItem(item);
                itemUI.button.onClick.AddListener(() => SetSelectedItem(itemUI));
                _itemsUIProtections.Add(itemUI);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    
        private void OnItemRemoved(Item item)
        {
            UpdateCapacityText();
            if (item.itemSO.category == ItemCategory.Other)
            {
                
            }
            else if (item.itemSO.category == ItemCategory.HandHeld)
            {
                bool found = false;
                for (int i = 0; i < _itemsUIToolbox.Count; i++)
                {
                    if (_itemsUIToolbox[i].AssignedItem != item) continue;
                    _itemsUIToolbox[i].ResetSlot();
                    _itemsUIToolbox.RemoveAt(i);
                    found = true;
                    break;
                }

                if (!found)
                {
                    Debug.LogError("Should not get here !", this);
                }
                
            }
            else if (item.itemSO.category == ItemCategory.Protection)
            {
                bool found = false;
                for (int i = 0; i < _itemsUIProtections.Count; i++)
                {
                    if (_itemsUIProtections[i].AssignedItem != item) continue;
                    _itemsUIProtections[i].ResetSlot();
                    _itemsUIProtections.RemoveAt(i);
                    found = true;
                    break;
                }

                if (!found)
                {
                    Debug.LogError("Should not get here !", this);
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateCapacityText()
        {
            capacityText.text = $"{Inventory.Current.CurrentWeight} / {Inventory.Current.MaxWeight}";
        }
    }
}