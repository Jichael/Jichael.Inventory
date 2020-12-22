using System;
using CustomPackages.Silicom.Player.Players;
using UnityEngine;

namespace Inventory
{
    public class HandHeldItem : Item
    {
        [SerializeField] private Vector3 cameraGrabOffset;
        [SerializeField] private Vector3 cameraGrabRotation;
        public bool Held { get; private set; }
        public event Action<bool> OnHeldState;

        public override void PickUp()
        {
            base.PickUp();
            gameObject.SetActive(false);
            Transform t = transform;
            t.SetParent(PlayerController.Current.CameraTransform);
            t.localPosition = cameraGrabOffset;
            t.localEulerAngles = cameraGrabRotation;
        }

        public override void Drop()
        {
            base.Drop();
            transform.SetParent(null);
        }

        public virtual void Grab()
        {
            Held = true;
            gameObject.SetActive(true);
            OnHeldState?.Invoke(true);
        }

        public virtual void Release()
        {
            Held = false;
            gameObject.SetActive(false);
            OnHeldState?.Invoke(false);
        }
    }
}