using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Inventory
{
    public class HandHeldItemUI : MonoBehaviour
    {
        public static HandHeldItemUI Instance { get; private set; }

        public Transform hudAnchorPoint;
        
        [SerializeField] private InputAction nextItemAction;
        [SerializeField] private InputAction previousItemAction;

        [SerializeField] private GameObject slideShow;

        [SerializeField] private GameObject previousItem;
        [SerializeField] private TMP_Text previousItemName;
        [SerializeField] private Image previousItemIcon;
        
        [SerializeField] private TMP_Text currentItemName;
        [SerializeField] private Image currentItemIcon;
        
        [SerializeField] private GameObject nextItem;
        [SerializeField] private TMP_Text nextItemName;
        [SerializeField] private Image nextItemIcon;

        [SerializeField] private List<HandHeldItem> handHeldItems;

        private int _currentIndex;

        private Coroutine _showSlideCo;

        private void Awake()
        {
            Instance = this;
            nextItemAction.Enable();
            nextItemAction.performed += NextItem;
            previousItemAction.Enable();
            previousItemAction.performed += PreviousItem;
            Inventory.OnItemAdded += OnItemAdded;
            Inventory.OnItemRemoved += OnItemRemoved;
            SetHeldItem(_currentIndex);
        }

        private void OnDestroy()
        {
            nextItemAction.Disable();
            nextItemAction.performed -= NextItem;
            previousItemAction.Disable();
            previousItemAction.performed -= PreviousItem;
            Inventory.OnItemAdded -= OnItemAdded;
            Inventory.OnItemRemoved -= OnItemRemoved;
        }

        private IEnumerator ShowSlideCo()
        {
            slideShow.SetActive(true);

            yield return new WaitForSeconds(2);
            
            slideShow.SetActive(false);
        }


        private void PreviousItem(InputAction.CallbackContext ctx)
        {
            if (ctx.ReadValue<float>() < 0) return;
            
            if(_showSlideCo != null) StopCoroutine(_showSlideCo);
            _showSlideCo = StartCoroutine(ShowSlideCo());
            
            handHeldItems[_currentIndex].Release();
            
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = handHeldItems.Count - 1;
            }
            
            handHeldItems[_currentIndex].Grab();
            SetHeldItem(_currentIndex);
        }

        private void NextItem(InputAction.CallbackContext ctx)
        {
            if (ctx.ReadValue<float>() > 0) return;
            
            if(_showSlideCo != null) StopCoroutine(_showSlideCo);
            _showSlideCo = StartCoroutine(ShowSlideCo());
            
            handHeldItems[_currentIndex].Release();
            
            _currentIndex++;
            if (_currentIndex > handHeldItems.Count - 1)
            {
                _currentIndex = 0;
            }
        
            handHeldItems[_currentIndex].Grab();
            SetHeldItem(_currentIndex);
        }

        private void SetHeldItem(int index)
        {
            HandHeldItem item = handHeldItems[index];

            currentItemName.text = item.itemSO.name;
            currentItemIcon.sprite = item.itemSO.icon;

            if (handHeldItems.Count != 1)
            {
                int prevIndex = _currentIndex - 1;
                if (prevIndex < 0) prevIndex = handHeldItems.Count - 1;

                int nextIndex = _currentIndex + 1;
                if (nextIndex > handHeldItems.Count - 1) nextIndex = 0;
                
                previousItemName.text = handHeldItems[prevIndex].itemSO.name;
                previousItemIcon.sprite = handHeldItems[prevIndex].itemSO.icon;
                
                nextItemName.text = handHeldItems[nextIndex].itemSO.name;
                nextItemIcon.sprite = handHeldItems[nextIndex].itemSO.icon;
            }
            
            previousItem.SetActive(handHeldItems.Count != 1);
            nextItem.SetActive(handHeldItems.Count != 1);
        }
        
        
        private void OnItemAdded(Item item)
        {
            if (item.itemSO.category == ItemCategory.HandHeld && item is HandHeldItem handHeldItem)
            {
                handHeldItems.Add(handHeldItem);
            }
        }
    
        private void OnItemRemoved(Item item)
        {
            if (item.itemSO.category == ItemCategory.HandHeld && item is HandHeldItem handHeldItem)
            {
                handHeldItems.Remove(handHeldItem);
                if (handHeldItem.Held) _currentIndex = 0;
            }
        }

    }
}