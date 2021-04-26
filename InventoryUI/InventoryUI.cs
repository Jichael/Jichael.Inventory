using System;
using System.Collections.Generic;
using CustomPackages.Silicom.Localization.Runtime;
using Silicom.UI;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CustomPackages.Silicom.Inventory.InventoryUI
{
    public class InventoryUI : UIInstance
    {
    
        public static InventoryUI Current { get; private set; }
        
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
        private readonly List<UIItemSlot> _itemsUIEquipment = new List<UIItemSlot>();

        protected override void Awake()
        {
            base.Awake();
            Current = this;
            toggleInventoryAction.Enable();
            toggleInventoryAction.performed += OnToggleInventory;
            Inventory.OnItemAdded += OnItemAdded;
            Inventory.OnItemRemoved += OnItemRemoved;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Inventory.OnItemAdded -= OnItemAdded;
            Inventory.OnItemRemoved -= OnItemRemoved;
            
            toggleInventoryAction.Disable();
            toggleInventoryAction.performed -= OnToggleInventory;
        }

        private void OnToggleInventory(InputAction.CallbackContext ctx)
        {
            Toggle();
        }

        private void OnEnable()
        {
            selectedItemContainer.SetActive(false);
            
            if (_itemsUIToolbox.Count > 0)
            {
                bool found = false;
                for (int i = 0; i < _itemsUIToolbox.Count; i++)
                {
                    if (_itemsUIToolbox[i].AssignedItem is HandHeldItem {Held: true})
                    {
                        SetSelectedItem(_itemsUIToolbox[i]);
                        found = true;
                        break;
                    }
                }
                if(!found) SetSelectedItem(_itemsUIToolbox[0]);
            }
            else if (_itemsUIEquipment.Count > 0)
            {
                SetSelectedItem(_itemsUIEquipment[0]);
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
                selectedItemName.text = _selectedItem.AssignedItem.itemSO.Name;
                selectedItemDescription.text = LanguageManager.Instance.RequestValue(_selectedItem.AssignedItem.itemSO.description);
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

            switch (item.category)
            {
                case ItemCategory.HandHeld: case ItemCategory.ToolBox:
                {
                    UIItemSlot itemUI = Instantiate(itemUITemplate, toolBox);
                    itemUI.AssignItem(item);
                    itemUI.button.onClick.AddListener(() => SetSelectedItem(itemUI));
                    _itemsUIToolbox.Add(itemUI);
                    break;
                }
                case ItemCategory.Equipment:
                {
                    UIItemSlot itemUI = Instantiate(itemUITemplate, protections);
                    itemUI.AssignItem(item);
                    itemUI.button.onClick.AddListener(() => SetSelectedItem(itemUI));
                    _itemsUIEquipment.Add(itemUI);
                    break;
                }
                case ItemCategory.Other:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    
        private void OnItemRemoved(Item item)
        {
            UpdateCapacityText();
            switch (item.category)
            {
                case ItemCategory.HandHeld: case ItemCategory.ToolBox:
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

                    break;
                }
                case ItemCategory.Equipment:
                {
                    bool found = false;
                    for (int i = 0; i < _itemsUIEquipment.Count; i++)
                    {
                        if (_itemsUIEquipment[i].AssignedItem != item) continue;
                        _itemsUIEquipment[i].ResetSlot();
                        _itemsUIEquipment.RemoveAt(i);
                        found = true;
                        break;
                    }

                    if (!found)
                    {
                        Debug.LogError("Should not get here !", this);
                    }

                    break;
                }
                case ItemCategory.Other:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateCapacityText()
        {
            capacityText.text = $"{Inventory.Current.CurrentWeight} / {Inventory.Current.MaxWeight}";
        }
    }
}