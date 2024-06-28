using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using System;

public class YesNo
{
    /// <summary>
    /// Принимает решение о том показывать вебвью или заглушку на основании данных с FirebaseRemoteConfig и домена запроса 1/0.
    /// </summary>
    public static async Task<bool> Decide()
    {
        AppData appData = AppData.Instance;
        // Пробуем инициализировать Фаербейс
        while(!await FirebaseTemplate.InitializeFirebase())
        {
            Debug.Log("Firebase init failed. Trying again");
            await Task.Delay(1000);
        }
    
        // Если кэшированная ссылка пуста, либо в конфиге фаера указано, что ссылку нужно обновить - берется ссылка из фаера.
        if(FirebaseTemplate.GetBooleanByKey("isChangeAllURL") || AppData.CachedLink == string.Empty)
        {
            AppData.CachedLink = FirebaseTemplate.GetStringByKey("url_link");
        }
        // Проверка, был ли успешно контент в вебвью ранее.
        if(AppData.WasShown)
            return true;
        // Проверка, на значение isDead в конфиге фаера (выставляется в случае, если прила удалена из стора).
        if(FirebaseTemplate.GetBooleanByKey("isDead"))
            return true;
        // Проверка, прошла ли дата, указанная в конфиге Фаера.
        if(!TimeHasCome())
            return false;
        // Проверка на значение с сервера 1/0.
        return  await OneZeroRequest.GetValue(appData.onezeroDomain, appData.onezeroKey, appData.appID) == 0;
    }

    private static bool TimeHasCome()
    {
        DateTime dateTime= DateTime.Parse(FirebaseTemplate.GetStringByKey("lastDate"));
        bool decision = dateTime <= DateTime.UtcNow;
        Debug.Log("Time has come: " + decision + "  The date is: " + dateTime);
        return decision;
    }
}

