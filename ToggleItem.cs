using CustomPackages.Silicom.Localization.Runtime;
using CustomPackages.Silicom.Player.Interactions.Views;
using UnityEngine;

namespace CustomPackages.Silicom.Inventory
{
    public class ToggleItem : MonoBehaviour
    {

        [SerializeField] private Item item;
        [SerializeField] private GameObject baseObject;
        [SerializeField] private GameObject takenObject;

        [SerializeField] private ChangeCursor changeCursor;
        [SerializeField] private string takeKey;
        [SerializeField] private string dropKey;
        private string _takeString;
        private string _dropString;

        private bool _taken;

        private void Awake()
        {
            _takeString = LanguageManager.Instance.RequestValue(takeKey);
            _dropString = LanguageManager.Instance.RequestValue(dropKey);
            changeCursor.cursor.cursorHintSimple = $"{(_taken ? _dropString : _takeString)}";
        }

        public void Toggle()
        {
            if (_taken)
            {
                Inventory.Current.Remove(item);
            }
            else
            {
                Inventory.Current.Add(item);
            }
            
            _taken = !_taken;
            baseObject.SetActive(!_taken);
            takenObject.SetActive(_taken);
            changeCursor.cursor.cursorHintSimple = $"{(_taken ? _dropString : _takeString)}";
        }

        public void Use()
        {
            Toggle();
            changeCursor.SetCursor();
        }
    }
}
