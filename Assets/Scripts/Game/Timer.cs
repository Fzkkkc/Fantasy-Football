using System;
using System.Collections;
using GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private TextMeshProUGUI _bonusText;

        private float _timeRemaining;
        private bool isRunning = false;
        
        public Action OnTimerStopped;
        
        private float PrefsTimeRemaining
        {
            get => float.Parse(PlayerPrefs.GetString("PREFS_Timer", "60"));
            set => PlayerPrefs.SetString("PREFS_Timer", value.ToString());
        }
        
        public void Init()
        {
            ResetTimer();
            GameInstance.UINavigation.OnGameStarted += StartTimer;
        }

        private void OnDestroy()
        {
            GameInstance.UINavigation.OnGameStarted -= StartTimer;
        }

        private void Update()
        {
            TimerRun();
        }

        private void TimerRun()
        {
            if (!isRunning) return;
            _timeRemaining -= Time.deltaTime;
            if (_timeRemaining <= 0)
            {
                _timeRemaining = 0;
                TimerEnd();
            }
            UpdateTimerText();
        }

        public void StartTimer()
        {
            ResetTimer();
            isRunning = true;
        }

        public void TimerEnd()
        {
            OnTimerStopped?.Invoke();
            isRunning = false;
        }

        public void StopTimer()
        {
            isRunning = false;
        }
        
        public void ResumeTimer()
        {
            isRunning = true;
        }
        
        public void ResetTimer()
        {
            _timeRemaining = PrefsTimeRemaining; 
            UpdateTimerText();
        }

        private void ShowBonusTime()
        {
            StartCoroutine(BonusTimePopup());
        }

        private IEnumerator BonusTimePopup()
        {
            _bonusText.color = new Color(255f,255f,255f,255f);
            yield return new WaitForSeconds(1f);
            _bonusText.color = new Color(255f,255f,255f,0f);
        }

        public void AddPermanentlySeconds(float seconds)
        {
            PrefsTimeRemaining = _timeRemaining = (_timeRemaining + seconds);
            UpdateTimerText();
        }
        
        public void AddInGameSeconds(float seconds)
        {
            _timeRemaining += seconds;
            UpdateTimerText();
            ShowBonusTime();
        }

        private void UpdateTimerText()
        {
            int minutes = Mathf.FloorToInt(_timeRemaining / 60);
            int seconds = Mathf.FloorToInt(_timeRemaining % 60);
            _timerText.text = string.Format("{0:00} {1:00}", minutes, seconds);
        }
    }
}