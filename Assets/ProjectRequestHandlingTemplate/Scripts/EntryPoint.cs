using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Threading.Tasks;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] bool hasOnboarding;
    [SerializeField] Web web;

    void Start()
    {
        Debug.Log("Entering App");
        Entry();
    }

    async void Entry()
    {
        //Принимается решение о показе контента для модерации либо для пользователя
        if(await AppData.Decide())
        {
            if(hasOnboarding)
            {
                // Открывается онбординг. Открыть веб контент можно через Web.ShowWebContent();
            }
            else
            {
                web.ShowWebContent();
            }
        }
        else
        {
            //Здесь следует функционал открытия заглушки
        }
    }

    public static void ChangeOrientation(bool top, bool left, bool right, bool bottom)
    {
        Screen.autorotateToPortrait = top;
        Screen.autorotateToLandscapeLeft = left;
        Screen.autorotateToLandscapeRight = right;
        Screen.autorotateToPortraitUpsideDown = bottom;

        CheckOrientation();
    }

    private static void CheckOrientation()
    {
        if(Screen.autorotateToPortrait)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else if(Screen.autorotateToLandscapeLeft)
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else if(Screen.autorotateToPortraitUpsideDown)
        {
            Screen.orientation = ScreenOrientation.PortraitUpsideDown;
        }
        else if(Screen.autorotateToLandscapeRight)
        {
            Screen.orientation = ScreenOrientation.LandscapeRight;
        }

        
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
}
