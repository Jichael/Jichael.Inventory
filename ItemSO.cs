using CustomPackages.Silicom.Localization.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CustomPackages.Silicom.Inventory
{
    [CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item", order = 0)]
    public class ItemSO : ScriptableObject
    {
        public string Name
        {
            get
            {
                if (_needTranslation)
                {
                    _name = LanguageManager.Instance.RequestValue(nameKey);
                    _needTranslation = false;
                }
                return _name;
            }
        }
        private string _name;
        [SerializeField] private string nameKey;
        private bool _needTranslation = true;
        public string description;
        public Sprite icon;
        public int weight;
        public bool stackable;
        [ShowIf(nameof(stackable))] public int maxStack;
    }
}