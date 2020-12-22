using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class Inventory : MonoBehaviour
    {
        public static Inventory Current { get; private set; }
    
        private readonly List<Item> _items = new List<Item>();

        public static event Action<Item> OnItemAdded;
        public static event Action<Item> OnItemRemoved;

        [SerializeField] private int maxWeight;
        public int CurrentWeight { get; private set; }
        public int MaxWeight => maxWeight;

        private void Awake()
        {
            Current = this;
        }

        public void Add(Item item)
        {
            if (_items.Contains(item))
            {
                Debug.LogError($"Inventory already contains {item} ! Could not add.");
                return;
            }

            if (CurrentWeight + item.itemSO.weight > maxWeight)
            {
                Debug.Log($"Could not add item : too heavy. (MaxWeight : {maxWeight}, CurrentWeight : {CurrentWeight}, ItemWeight : {item.itemSO.weight})");
                return;
            }
        
            _items.Add(item);
            CurrentWeight += item.itemSO.weight;
            OnItemAdded?.Invoke(item);
            item.PickUp();
        }

        public void Remove(Item item)
        {
            if (!_items.Contains(item))
            {
                Debug.LogError($"Inventory does not contain {item} ! Could not remove.");
                return;
            }
        
            _items.Remove(item);
            CurrentWeight -= item.itemSO.weight;
            OnItemRemoved?.Invoke(item);
            item.Drop();
        }
    }
}