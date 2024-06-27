using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public class GameUpgrades : MonoBehaviour
    {
        [SerializeField] private List<Button> _boostButtons;
        
        private void Start()
        {
            _boostButtons[0].onClick.AddListener(BuyFirstUpgrade);
            _boostButtons[1].onClick.AddListener(BuySecondUpgrade);
            _boostButtons[2].onClick.AddListener(BuyThirdUpgrade);
            
            CheckBoostBough();
        }

        private void OnDestroy()
        {
            _boostButtons[0].onClick.RemoveListener(BuyFirstUpgrade);
            _boostButtons[1].onClick.RemoveListener(BuySecondUpgrade);
            _boostButtons[2].onClick.RemoveListener(BuyThirdUpgrade);
        }

        public void BuyFirstUpgrade()
        {
            UpgradeBase(5000, 5,0);
        }
        
        public void BuySecondUpgrade()
        {
            UpgradeBase(15000, 15,1);
        }
        
        public void BuyThirdUpgrade()
        {
            UpgradeBase(25000, 25,2);
        }

        private void CheckBoostBough()
        {
            for (var i = 0; i < _boostButtons.Count; i++)
            {
                if (PlayerPrefs.GetInt($"Boost{i}Bought", 0) == 1)
                {
                    _boostButtons[i].interactable = false;
                }
            }
        }

        private void UpgradeBase(int upgradeCost, float timeBoost, int boostIndex)
        {
            if(!GameInstance.MoneyManager.HasEnoughCoinsCurrency((ulong) upgradeCost)) return;
            
            GameInstance.MoneyManager.SpendCoinsCurrency((ulong) upgradeCost);
            GameInstance.Timer.AddPermanentlySeconds(timeBoost);
            PlayerPrefs.SetInt($"Boost{boostIndex}Bought", 1);
            CheckBoostBough();
        }
    }
}