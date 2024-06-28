using System;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

/// <summary>
/// Класс нужен для хранения и простого доступа ко всем данным, связанным с проектом.
/// </summary>
/// <remarks>
/// Примечание: все url указываются в формате htpps://....
/// </remarks>
public class AppData : MonoBehaviour
{
    public static AppData Instance;

    /// К этой группе данных доступ через статичный Instance.
    [SerializeField]public string appID;
    [SerializeField]public string onezeroDomain;
    [SerializeField]public string onezeroKey;

    /// <summary>
    /// Ссылка из конфига Фаербейса, либо последняя успешно открытая в вебвью
    /// </summary>
    public static string CachedLink
    {
        get
        {
            return PlayerPrefs.GetString("CachedLink", string.Empty);
        }
        set
        {
            PlayerPrefs.SetString("CachedLink", value);
        }
    }
    
    /// <summary>
    /// Был ли успешно показан контент в вебвью.
    /// </summary>
    public static bool WasShown
    {
        get
        {
            return PlayerPrefs.HasKey("WasShown");
        }
        set
        {
            if(value)
                PlayerPrefs.SetInt("WasShown", 1);
            else
                PlayerPrefs.DeleteKey("WasShown");
        }
    }
    

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// В случае true - показываем контент в вебвью, false - заглушку
    /// </summary>
    public static async Task<bool> Decide()
    {
        Debug.Log("Deciding...");
        return await YesNo.Decide();
    }
}

