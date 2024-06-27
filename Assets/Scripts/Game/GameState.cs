using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using TMPro;
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
        
        [SerializeField] private List<TextMeshProUGUI> _passportStatsText;
        
        [SerializeField] private Image _passportPortraitImage;
        [SerializeField] private Image manImage;
        
        [SerializeField] private TextMeshProUGUI _playersReceivedText;
        [SerializeField] private TextMeshProUGUI _moneyReceivedText;
        
        [SerializeField] private List<string> _manNames;
        [SerializeField] private List<string> _womanNames;

        private bool _isExpandedStats = false;

        private List<int> _currentStats = new List<int> {1, 2, 3, 4, 5, 6};

        private List<int> _requiredStats1 = new List<int> {1, 2, 3, 4, 5};
        private List<int> _requiredStats2 = new List<int> {1, 2, 3, 4, 5};
        private List<int> _requiredStats3 = new List<int> {1, 2, 3, 4, 5};
        private List<int> _requiredStats4 = new List<int> {1, 2, 3, 4, 5};

        [SerializeField] private List<TextMeshProUGUI> _requiredStatsText1;
        [SerializeField] private List<TextMeshProUGUI> _requiredStatsText2;
        [SerializeField] private List<TextMeshProUGUI> _requiredStatsText3;
        [SerializeField] private List<TextMeshProUGUI> _requiredStatsText4;
        
        private List<List<int>> _statsList = new List<List<int>>();
        private List<List<TextMeshProUGUI>> _statsTextList = new List<List<TextMeshProUGUI>>(4);

        private bool _isGoodChoice = false;

        private ulong _moneyReceived = 0;
        private int _playersReceived = 0;
        
        private void Start()
        {
            GameInstance.UINavigation.OnGameStarted += SelectNextMan;
            GameInstance.UINavigation.OnGameWindowClosed += ResetGame;
            GameInstance.Timer.OnTimerStopped += SetRequiredStats;
            AddListComponents();
            SetRequiredStats();
        }
        
        private void OnDestroy()
        {
            GameInstance.UINavigation.OnGameStarted -= SelectNextMan;
            GameInstance.UINavigation.OnGameWindowClosed -= ResetGame;
            GameInstance.Timer.OnTimerStopped -= SetRequiredStats;
        }

        public void SelectNextMan()
        {
            var randomIndex = Random.Range(0, _manBigSprites.Count);

            _passportPortraitImage.sprite = _manPortraitsSprites[randomIndex];
            manImage.sprite = _manBigSprites[randomIndex];

            _passportStatsText[0].text = randomIndex <= 6 ? _manNames[randomIndex] : _womanNames[randomIndex];
            SetPassportRandomStats();
            _gameAnimator.SetTrigger("NextMan");
        }

        private void SetPassportRandomStats()
        {
            _currentStats[1] = Random.Range(0, 12);
            _passportStatsText[1].text = $"{_currentStats[1]} years";
            for (var i = 2; i < _currentStats.Count; i++)
            {
                _currentStats[i] = Random.Range(80, 101);
                _passportStatsText[i].text = _currentStats[i].ToString();
            }
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
            SetRequiredStats();
            _gameAnimator.SetTrigger("ToIdle");
        }
        
        public void AcceptMan()
        {
            GameInstance.FXController.PlayHitParticle();
            _gameAnimator.SetTrigger("OutMan");
            StartCoroutine(SendNextMan());
        }
        
        public void RejectMan()
        {
            GameInstance.FXController.PlayMissParticle();
            _gameAnimator.SetTrigger("OutMan");
            StartCoroutine(SendNextManReject());
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
                GameInstance.UINavigation.OpenPausePopup();
            }
        }

        private IEnumerator SendNextMan()
        {
            yield return new WaitForSeconds(1f);
            
            CompareCurrentStatsWithStatsList();
            if(_isGoodChoice)
                SelectNextMan();
        }

        private IEnumerator SendNextManReject()
        {
            yield return new WaitForSeconds(1f);
            SelectNextMan();
        }
        
        private void SetRequiredStats()
        {
            _moneyReceivedText.text = _moneyReceived.ToString();
            _playersReceivedText.text = _playersReceived.ToString();
            _moneyReceived = 0;
            _playersReceived = 0;
            
            for (var i = 0; i < _statsList.Count; i++)
            {
                var statsList = _statsList[i];
                var statsTextList = _statsTextList[i];
        
                if (statsList.Count == 0 || statsTextList.Count == 0)
                    continue; 

                statsList[0] = Random.Range(0, 10);
                statsTextList[0].text = statsList[0].ToString();
                for (var j = 1; j < statsList.Count; j++)
                {
                    if (j >= statsTextList.Count)
                        break; 

                    statsList[j] = Random.Range(0, 101);
                    statsTextList[j].text = statsList[j].ToString();
                }
            }
        }
        
        private void AddListComponents()
        {
            _statsTextList.Add(_requiredStatsText1);
            _statsTextList.Add(_requiredStatsText2);
            _statsTextList.Add(_requiredStatsText3);
            _statsTextList.Add(_requiredStatsText4);
            
            _statsList.Add(_requiredStats1);
            _statsList.Add(_requiredStats2);
            _statsList.Add(_requiredStats3);
            _statsList.Add(_requiredStats4);
        }

        private void CompareCurrentStatsWithStatsList()
        {
            var tempCounter1 = 0;
            var tempCounter2 = 0;
            var tempCounter3 = 0;
            var tempCounter4 = 0;
            
            for (int i = 0; i < _requiredStats1.Count; i++)
            {
                if (_currentStats[i+1] >= _requiredStats1[i])
                {
                    tempCounter1++;
                }
            }
            
            for (int i = 0; i < _requiredStats2.Count; i++)
            {
                if (_currentStats[i+1] >= _requiredStats2[i])
                {
                    tempCounter2++;
                }
            }
            
            for (int i = 0; i < _requiredStats3.Count; i++)
            {
                if (_currentStats[i+1] >= _requiredStats3[i])
                {
                    tempCounter3++;
                }
            }
            
            for (int i = 0; i < _requiredStats4.Count; i++)
            {
                if (_currentStats[i+1] >= _requiredStats4[i])
                {
                    tempCounter4++;
                }
            }

            if (tempCounter1 == 5 ||
                tempCounter2 == 5 ||
                tempCounter3 == 5 ||
                tempCounter4 == 5)
            {
                _moneyReceived += 10;
                _playersReceived += 1;
                GameInstance.Timer.AddInGameSeconds(10);
                GameInstance.MoneyManager.AddCoinsCurrency(10);
                _isGoodChoice = true;
            }
            else
            {
                _isGoodChoice = false;
                GameInstance.Timer.TimerEnd();
            }
        }
    }
}