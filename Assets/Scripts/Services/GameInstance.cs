using Game;
using UnityEngine;
using UserInterface;

namespace GameCore
{
    public class GameInstance : Singleton<GameInstance>
    {
        [SerializeField] private MoneyManager _moneyManager;
        [SerializeField] private UINavigation _uiNavigation;
        [SerializeField] private Timer _timer;
        [SerializeField] private FXController _fxController;

        public static MoneyManager MoneyManager => Default._moneyManager;
        public static UINavigation UINavigation => Default._uiNavigation;
        public static Timer Timer => Default._timer;
        public static FXController FXController => Default._fxController;

        protected override void Awake()
        { 
            //PlayerPrefs.DeleteAll();
            base.Awake();
            _moneyManager.Init(50000);
            _uiNavigation.Init();
            _timer.Init();
            _fxController.Init();
        }
    }
}