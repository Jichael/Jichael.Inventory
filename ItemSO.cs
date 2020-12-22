using Sirenix.OdinInspector;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 0)]
    public class ItemSO : ScriptableObject
    {
        public new string name;
        public string description;
        public Sprite icon;
        public ItemCategory category;
        public int weight;
        public bool stackable;
        [ShowIf(nameof(stackable))] public int maxStack;
    }
    
    public enum ItemCategory
    {
        Other,
        HandHeld,
        Protection
    }
}