using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

[RequireComponent (typeof (UniWebView))]
public class Web : MonoBehaviour
{
    [SerializeField] UniWebView uniWebView;

    public void ShowWebContent()
    {
        EntryPoint.ChangeOrientation(true, true, true, true);
        uniWebView.gameObject.SetActive(true);
        uniWebView.Load(AppData.CachedLink);
        uniWebView.Show();
        Debug.Log(AppData.CachedLink);
    }

    void Start()
    {
        uniWebView = GetComponent<UniWebView> ();
        uniWebView.SetContentInsetAdjustmentBehavior(UniWebViewContentInsetAdjustmentBehavior.Always);
        uniWebView.SetSupportMultipleWindows(true,true);
        uniWebView.SetBackButtonEnabled(false);
        uniWebView.OnShouldClose += (view) =>
        {
            uniWebView.Hide();
            return false;
        };
        uniWebView.OnPageFinished += (view, code, url) =>
        {
            AppData.CachedLink = url;
            AppData.WasShown = true;
        };
        uniWebView.OnOrientationChanged += (view, orientation) => 
        { uniWebView.Frame = new Rect(Vector2.zero, new(Screen.width, Screen.height)); };
    }
}