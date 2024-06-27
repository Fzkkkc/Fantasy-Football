using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameState : MonoBehaviour
    {
        [SerializeField] private Animator _gameAnimator;
        
        [SerializeField] private List<Sprite> _manBigSprites;
        [SerializeField] private List<Sprite> _manPortraitsSprites;
        
        [SerializeField] private Image _passportPortraitImage;
        [SerializeField] private Image manImage;
        
        [SerializeField] private List<string> _manNames;

        private bool _isExpandedStats = false;
        
        private void Start()
        {
            GameInstance.UINavigation.OnGameWindowOpened += SelectNextMan;
            GameInstance.UINavigation.OnGameWindowClosed += ResetGame;
        }
        
        private void OnDestroy()
        {
            GameInstance.UINavigation.OnGameWindowOpened -= SelectNextMan;
            GameInstance.UINavigation.OnGameWindowClosed -= ResetGame;
        }

        public void SelectNextMan()
        {
            var randomIndex = Random.Range(0, _manBigSprites.Count);

            _passportPortraitImage.sprite = _manPortraitsSprites[randomIndex];
            manImage.sprite = _manBigSprites[randomIndex];
            _gameAnimator.SetTrigger("NextMan");
        }

        public void ThrowCard()
        {
            _gameAnimator.SetTrigger("CardThrow");
        }
        
        public void ExpandStats()
        {
            _gameAnimator.SetTrigger("Expand");
            _isExpandedStats = true;
        }
        
        public void ResetGame()
        {
            _gameAnimator.SetTrigger("ToIdle");
        }

        public void AcceptMan()
        {
            _gameAnimator.SetTrigger("OutMan");
            StartCoroutine(SendNextMan());
        }
        
        public void KickMan()
        {
            _gameAnimator.SetTrigger("OutMan");
            StartCoroutine(SendNextMan());
        }
        
        public void BackButtonState()
        {
            if (_isExpandedStats)
            {
                _isExpandedStats = false;
                _gameAnimator.SetTrigger("Minimize");
            }
            else
            {
                GameInstance.UINavigation.OnGameWindowClosed?.Invoke();
                StartCoroutine(GameInstance.UINavigation.OpenMenuPopup(0));
            }
        }

        private IEnumerator SendNextMan()
        {
            yield return new WaitForSeconds(1f);
            SelectNextMan();
        }
    }
}