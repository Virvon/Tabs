using UnityEngine;

namespace Sources.UI.Dogs
{
    public class DogListView : ListView<DogView>
    {
        [SerializeField] private Canvas _canvas;
        
        public void Show() =>
            _canvas.enabled = true;

        public void Hide() =>
            _canvas.enabled = false;
    }
}