using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.UI
{
    public class LoadingAnimator : MonoBehaviour
    {
        [SerializeField] private Image _loadingImage;
        [SerializeField] private float _animationSpeed;
        
        private bool _isAnimated;
        
        public void ShowLoading()
        {
            if(_isAnimated)
                return;
            
            StartCoroutine(LoadingAnimation());
        }

        public void StopLoading() =>
            _isAnimated = false;

        private IEnumerator LoadingAnimation()
        {
            _loadingImage.enabled = true;
            _isAnimated = true;

            while (_isAnimated)
            {
                _loadingImage.transform.Rotate(-Vector3.forward, _animationSpeed * Time.deltaTime);

                yield return null;
            }

            _loadingImage.enabled = false;
        }
    }
}