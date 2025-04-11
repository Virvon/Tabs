using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.UI.Dogs
{
    public class DogPopup : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;
        [SerializeField] private Button _button;
        
        public void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            
            _button.onClick.AddListener(Hide);
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
            
            _button.onClick.RemoveListener(Hide);
        }

        public void SetInfo(string name, string description)
        {
            _nameText.text = name;
            _descriptionText.text = description;
        }
    }
}