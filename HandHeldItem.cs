using System;
using CustomPackages.Silicom.Inventory.InventoryUI;
using CustomPackages.Silicom.Localization.Runtime;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CustomPackages.Silicom.Inventory
{
    public class HandHeldItem : Item
    {
        public bool Held { get; private set; }
        public event Action<bool> OnHeldState;
        
        [SerializeField] private GameObject hud;
        [SerializeField, ShowIf(nameof(hud))] private TMP_Text hudName;
        [SerializeField, ShowIf(nameof(hud))] private Image hudIcon;
        

        public override void PickUp()
        {
            base.PickUp();
            if (hud)
            {
                hud.transform.SetParent(HandHeldItemUI.Instance.hudAnchorPoint, false);
                hudName.text = itemSO.Name;
                hudIcon.sprite = itemSO.icon;
                hud.SetActive(false);
            }
        }

        public override void Drop()
        {
            base.Drop();
            Release();
        }

        public virtual void Grab()
        {
            Held = true;
            OnHeldState?.Invoke(true);
            if(hud) hud.SetActive(true);
        }

        public virtual void Release()
        {
            Held = false;
            OnHeldState?.Invoke(false);
            if(hud) hud.SetActive(false);
        }
        
        protected virtual void OnDestroy()
        {
            if(hud) Destroy(hud);
        }
    }
}