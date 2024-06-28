using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Firebase.RemoteConfig;
using System;

public static class FirebaseTemplate
{
    public static Firebase.FirebaseApp app = null;

    public static async Task<bool> InitializeFirebase()
    {
        try
        {
            Firebase.DependencyStatus dependencyStatus = await Firebase.FirebaseApp.CheckAndFixDependenciesAsync();

            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = Firebase.FirebaseApp.DefaultInstance;
                return await FetchRemoteConfig();
            }
            else
            {
                Debug.LogError(
                $"Could not resolve all Firebase dependencies: {dependencyStatus}\n" +
                "Firebase Unity SDK is not safe to use here");
                return false;
            }
        }
        catch (Exception ex)
        {
            Debug.Log("Firebase initialization failed");
            Debug.Log(ex);
            return false;
        }

    }

    public static async Task <bool> FetchRemoteConfig()
    {
        if (app == null)
        {
            Debug.LogWarning("app is null");
            return false;
        }

        Task task = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
        await task;

        if (!task.IsCompleted)
        {
            Debug.LogWarning("fetch failed");
            return false;
        }
        else
        {
            await FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
            return true;
        }
    }

    public static string GetStringByKey(string key)
    {
        string result = FirebaseRemoteConfig.DefaultInstance.GetValue(key).StringValue;
        Debug.Log(key +": " + result);
        return result;
    }

    public static bool GetBooleanByKey(string key)
    {
        bool result = FirebaseRemoteConfig.DefaultInstance.GetValue(key).BooleanValue;
        Debug.Log(key +": " + result);
        return result;
    }
}