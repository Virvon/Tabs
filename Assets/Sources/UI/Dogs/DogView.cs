using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.UI.Dogs
{
    public class DogView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _numberText;
        [SerializeField] private TMP_Text _nameText;

        private IDogCellPresenter _dogCellPresenter;

        public void Setup(string number, IDogCellPresenter dogCellPresenter)
        {
            _dogCellPresenter = dogCellPresenter;
            
            _numberText.text = number;
            _nameText.text = dogCellPresenter.Name;
            
            _button.onClick.AddListener(_dogCellPresenter.OnClicked);
        }
    }
}