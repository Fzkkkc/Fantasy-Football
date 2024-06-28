using System.Net.NetworkInformation;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;
#if Unity_IOS
using UnityEngine.iOS;
#endif

/// <summary>
/// Запрос на домен 
/// </summary>
public class OneZeroRequest
{

    public static async Task<int> GetValue(string url, string key, string appID, int waitTimeMilliseconds = 10000)
    {
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        HttpResponseMessage response = new HttpResponseMessage();
        Func <Task> func = new Func<Task> (async () => {response = await client.PostAsync(url, new StringContent(GetJsonData(appID), Encoding.UTF8, "application/json"));});
        try
        {
            await Task.WhenAny(func(), Task.Delay(waitTimeMilliseconds));
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
            return -1;
        }

        string responseText = await response.Content.ReadAsStringAsync();
        Debug.Log(responseText);
        string regex = $"\"{key}\":\\w+";
        string keyValue = Regex.Match(responseText, regex).ToString();

        Debug.Log(keyValue);
        if(keyValue.Contains("true"))
            return 1;
        else if(keyValue.Contains("false"))
            return 0;
        else
            return -1;
    }

    private static string GetJsonData(string appID)
    {
        Root root = new()
        {
            code = appID,
            userData = new UserData
            {        
                gdpsjPjg = SystemInfo.deviceModel,
                bvoikOGjs = new BvoikOGjs(),
                StwPp = false,
                gfpbvjsoM = (int)SystemInfo.batteryLevel,
                vivisWork = IsVpn(),
                bpPjfns = System.Globalization.RegionInfo.CurrentRegion.EnglishName,
    #if Unity_IOS      
                bcpJFs = Device.systemVersion,
    #endif
                Fpbjcv = SystemInfo.systemMemorySize.ToString(),
                KDhsd = false,
                GOmblx = Application.systemLanguage.ToString(),
                gciOFm = new List<string>
                    {

                    },
                poguaKFP = SystemInfo.deviceUniqueIdentifier,
                gpaMFOfa = new List<string>
                    {

                    },
                Fpvbduwm = SystemInfo.batteryStatus == BatteryStatus.Charging,
                biMpaiuf = true,
                oahgoMAOI = SystemInfo.batteryLevel == 100,
                gfdokPS = SystemInfo.deviceName,
                gfdosnb = new List<string>
                    {

                    },
                G0pxum = TimeZoneInfo.Local.ToString()
            },
            setUserAgent = "opera"

        };
        string result = JsonUtility.ToJson(root, true);
        Debug.Log(result);
        return result;
    }

    private static bool IsVpn()
    {
        bool isVPN = false;
        if (NetworkInterface.GetIsNetworkAvailable())
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface Interface in interfaces)
            {
                if (Interface.OperationalStatus == OperationalStatus.Up)
                {
                    if (((Interface.NetworkInterfaceType == NetworkInterfaceType.Ppp) && (Interface.NetworkInterfaceType != NetworkInterfaceType.Loopback)) || Interface.Description.Contains("VPN") || Interface.Description.Contains("vpn"))
                    {
                        IPv4InterfaceStatistics statistics = Interface.GetIPv4Statistics();
                        isVPN = true;
                    }
                }
            }
        }
        return isVPN;
    }

    private static string  GetWIFIAddress()
    {
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface Interface in interfaces)
        {
            if(Interface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
            {
                foreach(UnicastIPAddressInformation inf in Interface.GetIPProperties().UnicastAddresses)
                {
                    return inf.Address.ToString();
                }
            }
        }
        return string.Empty;
    }
}

public class BvoikOGjs
{
}

public class Root
{
    public string code;
    public string setIp;
    public UserData userData;
    public string setUserAgent;
}

[Serializable]
public struct UserData
{
    public string gdpsjPjg;
    public BvoikOGjs bvoikOGjs;
    public bool StwPp;
    public int gfpbvjsoM;
    public bool vivisWork;
    public string bpPjfns;
    public string bcpJFs;
    public string Fpbjcv;
    public bool KDhsd;
    public string GOmblx;
    public List<string> gciOFm;
    public string poguaKFP;
    public List<string> gpaMFOfa;
    public bool Fpvbduwm;
    public bool biMpaiuf;
    public bool oahgoMAOI;
    public string gfdokPS;
    public List<string> gfdosnb;
    public string G0pxum;
}