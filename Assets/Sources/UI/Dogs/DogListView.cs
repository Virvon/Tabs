using UnityEngine;

namespace Sources.UI.Dogs
{
    public class DogListView : ListView<DogView>
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private LoadingAnimator _loadingAnimator;
        
        public void Show() =>
            _canvas.enabled = true;

        public void Hide()
        {
            StopLoading();
            Clear();
            _canvas.enabled = false;
        }

        public void StopLoading() =>
            _loadingAnimator.StopLoading();

        public void ShowLoading() =>
            _loadingAnimator.ShowLoading();
    }
}