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
        [SerializeField] private LoadingAnimator _loadingAnimator;

        private IDogCellPresenter _dogCellPresenter;

        public void Setup(string number, IDogCellPresenter dogCellPresenter)
        {
            _dogCellPresenter = dogCellPresenter;
            
            _numberText.text = number;
            _nameText.text = dogCellPresenter.Name;
            
            _button.onClick.AddListener(_dogCellPresenter.OnClicked);

            _dogCellPresenter.Disposed += OnPresenterDisposed;
            _dogCellPresenter.LoadAnimationStarted += OnLoadAnimationStarted;
            _dogCellPresenter.LoadAnimationFinished += OnloadAnimationFinished;
        }

        private void OnloadAnimationFinished() =>
            _loadingAnimator.StopLoading();

        private void OnLoadAnimationStarted() =>
            _loadingAnimator.ShowLoading();

        private void OnPresenterDisposed()
        {
            _loadingAnimator.StopLoading();
            
            _dogCellPresenter.Disposed -= OnPresenterDisposed;
            _dogCellPresenter.LoadAnimationStarted -= OnLoadAnimationStarted;
            _dogCellPresenter.LoadAnimationFinished -= OnloadAnimationFinished;
        }
    }
}