using System.Collections;
using GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class LoadingAnimation : MonoBehaviour
    {
        public TextMeshProUGUI _percentageText;

        [SerializeField] private Slider _loadingSlider;

        private void Start()
        {
            StartCoroutine(AnimatePercentage());
        }

        IEnumerator AnimatePercentage()
        {
            var duration = 2.0f; 
            float elapsedTime = 0;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                _loadingSlider.value = elapsedTime;
                float percentage = Mathf.Lerp(0, 100, elapsedTime / duration);
                _percentageText.text = string.Format("{0:0}%", percentage);
                yield return null;
            }
            _loadingSlider.value = 2f;
            _percentageText.text = "100%";
            GameInstance.UINavigation.OpenMainMenu();
        }
    }
}