using Game;
using Services;
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
        [SerializeField] private AudioSystem _audio;

        public static MoneyManager MoneyManager => Default._moneyManager;
        public static UINavigation UINavigation => Default._uiNavigation;
        public static Timer Timer => Default._timer;
        public static FXController FXController => Default._fxController;
        public static AudioSystem Audio => Default._audio;

        protected override void Awake()
        { 
            //PlayerPrefs.DeleteAll();
            base.Awake();
            _moneyManager.Init(0);
            _uiNavigation.Init();
            _timer.Init();
            _fxController.Init();
            _audio.Init();
        }
    }
}