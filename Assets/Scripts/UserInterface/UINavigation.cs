using System;
using System.Collections;
using System.Collections.Generic;
using GameCore;
using UnityEngine;

namespace UserInterface
{
    public class UINavigation : MonoBehaviour
    {
        public List<CanvasGroup> GamePopups;
        public List<CanvasGroup> MainMenuPopups;
        [SerializeField] private Animator _transitionAnimator;
        
        public CanvasGroup LoadingMenu;
        public CanvasGroup MainMenu;
        public CanvasGroup GameMenu;

        public Action OnActivePopupChanged;
        public Action OnGameStarted;
        public Action OnGameWindowClosed;
        
        public void Init()
        {
            ResetPopups();
            GameInstance.Timer.OnTimerStopped += OpenGameOverPopup;
        }

        private void OnDestroy()
        {
            GameInstance.Timer.OnTimerStopped -= OpenGameOverPopup;
        }

        private void ResetGamePopups()
        {
            foreach (var popup in GamePopups)
            {
                popup.alpha = 0f;
                popup.blocksRaycasts = false;
                popup.interactable = false;
            }
        }

        private void ResetMenuPopups()
        {
            foreach (var popup in MainMenuPopups)
            {
                popup.alpha = 0f;
                popup.blocksRaycasts = false;
                popup.interactable = false;
            }
        }
        
        private void ResetPopups()
        {
            OpenGroup(LoadingMenu);
            
            CloseGroup(GameMenu);
            CloseGroup(MainMenu);
            
            ResetGamePopups();
            ResetMenuPopups();
        }
        
        public void OpenMainMenu()
        {
            StartCoroutine(OpenMenuPopup(0));
        }

        public void CloseGameUI()
        {
            OnGameWindowClosed?.Invoke();
            StartCoroutine(OpenMenuPopup(0));
            ResetGamePopups();
        }
        
        public void BackToMainMenu()
        {
            SelectMenuPopup(0);
        }
        
        public void OpenShopMenu()
        {
            SelectMenuPopup(1);
        }
        
        public void OpenSettingsMenu()
        {
            SelectMenuPopup(2);
        }
        
        public void OpenGameMenu()
        {
            StartCoroutine(OpenGamePopup());
        }

        public void OpenPausePopup()
        {
            SelectGamePopup(0);
            GameInstance.Timer.StopTimer();
        }
        
        public void ClosePausePopup()
        {
            ResetGamePopups();
            GameInstance.Timer.ResumeTimer();
        }
        
        private void OpenGameOverPopup()
        {
            SelectGamePopup(1);
        }
        
        public IEnumerator OpenMenuPopup(int index)
        {
            TransitionAnimation();
            yield return new WaitForSeconds(0.5f);
            SelectMenuPopup(index);
        }
        
        private IEnumerator OpenGamePopup()
        {
            TransitionAnimation();
            yield return new WaitForSeconds(0.5f);
            ResetGamePopups();
            OpenGroup(GameMenu);
            OnGameStarted?.Invoke();
        }

        public void TransitionAnimation()
        {
            _transitionAnimator.SetTrigger("Transition");
        }
        
        public void OpenGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
        
        public void CloseGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
        
        private void SelectMenuPopup(int selectedIndex)
        {
            OpenGroup(MainMenu);
            CloseGroup(GameMenu);
            
            for (var i = 0; i < MainMenuPopups.Count; i++)
            {
                if (i == selectedIndex)
                {
                    MainMenuPopups[i].alpha = 1f;
                    MainMenuPopups[i].blocksRaycasts = true;
                    MainMenuPopups[i].interactable = true;
                }
                else
                {
                    MainMenuPopups[i].alpha = 0f;
                    MainMenuPopups[i].blocksRaycasts = false;
                    MainMenuPopups[i].interactable = false;
                }
            }
            
            foreach (var popup in GamePopups)
            {
                popup.alpha = 0f;
                popup.blocksRaycasts = false;
                popup.interactable = false;
            }
            
            OnActivePopupChanged?.Invoke();
        }
        
        private void SelectGamePopup(int selectedIndex)
        {
            OpenGroup(GameMenu);
            CloseGroup(MainMenu);
            
            for (var i = 0; i < GamePopups.Count; i++)
            {
                if (i == selectedIndex)
                {
                    GamePopups[i].alpha = 1f;
                    GamePopups[i].blocksRaycasts = true;
                    GamePopups[i].interactable = true;
                }
                else
                {
                    GamePopups[i].alpha = 0f;
                    GamePopups[i].blocksRaycasts = false;
                    GamePopups[i].interactable = false;
                }
            }
            
            foreach (var popup in MainMenuPopups)
            {
                popup.alpha = 0f;
                popup.blocksRaycasts = false;
                popup.interactable = false;
            }
            
            OnActivePopupChanged?.Invoke();
        }
    }
}