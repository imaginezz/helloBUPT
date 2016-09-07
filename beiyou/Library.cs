using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;


class DebugLib {
    public static void DebugOutput(object log) {
        Debug.WriteLine(log);
    }
}
class webLib {
    public static async Task<string> HttpPost(string url, string postDataStr, string encode = "utf-8", CookieCollection cc = null) {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.CookieContainer = new CookieContainer();
        if (cc != null) {
            request.CookieContainer.Add(new Uri(url), cc);
        }
        request.Method = "POST";
        request.Headers["Content-Length"] = Encoding.UTF8.GetByteCount(postDataStr).ToString();
        request.ContentType = "application/x-www-form-urlencoded";

        //test request header
        //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        //request.Headers["Accept-Encoding"] = "gzip, deflate";
        //request.Headers["Accept-Language"] = "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3";
        //request.Headers["Connection"] = "keep-alive";
        //request.Headers["DNT"] = "1";
        //request.Headers["Host"] = "10.3.8.211";
        //request.Headers["Referrer"] = "http://10.3.8.211/";
        //request.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:47.0) Gecko/20100101 Firefox/47.0";

        Stream myRequestStream = await request.GetRequestStreamAsync();

        //use this function to register the encoding machine or it will throw expect
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        //I Don't know why it doesn't work, so I use direct function to write
        //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding(encode));
        //myStreamWriter.Write(postDataStr);
        byte[] bs = Encoding.ASCII.GetBytes(postDataStr);
        myRequestStream.Write(bs, 0, bs.Length);

        HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
        Stream myResponseStream = response.GetResponseStream();
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(encode));
        string retString = myStreamReader.ReadToEnd();
        return retString;
    }
    public static async Task<string> HttpGet(string url, string getDataStr = null, string encode = "utf-8") {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + (getDataStr == null ? "" : "?") + getDataStr);
        request.Method = "GET";
        request.ContentType = "text/html;charset=UTF-8";
        HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
        Stream myResponseStream = response.GetResponseStream();
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(encode));
        string retString = myStreamReader.ReadToEnd();
        return retString;
    }

}
class RegLib {
    public static string RegexMatch(string regStr, ref string input) {
        Regex reg = new Regex(regStr);
        Match match = reg.Match(input);
        return match.Value;
    }
}

class SettingLib {

    private static ApplicationDataContainer dataSettings;

    public SettingLib(bool isRoaming = true) {
        if (isRoaming) {
            dataSettings = ApplicationData.Current.RoamingSettings;
        } else {
            dataSettings = ApplicationData.Current.LocalSettings;
        }
    }

    public void SaveSetting(string key, Object value) {
        dataSettings.Values[key] = value;
    }

    public void SaveCompositeSetting(string compositeName,List<Tuple<string, Object>> vals) {
        ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
        foreach(Tuple<string ,Object> v in vals) {
            composite[v.Item1] = v.Item2;
        }
        SaveSetting(compositeName,composite);
    }

    public object ReadSingleSetting(string key) {
        object value = dataSettings.Values[key];
        return value;
    }
    public List<Tuple<string,Object>> ReadCompositeSetting(string compositeName,List<string> settingNames) {
        ApplicationDataCompositeValue composite = (ApplicationDataCompositeValue)ReadSingleSetting(compositeName);
        if (composite == null) {
            return null;
        }else {
            List<Tuple<string, Object>> cs = new List<Tuple<string, object>>();
            foreach (string sn in settingNames) {
                cs.Add(new Tuple<string, Object>(sn, composite[sn]));
            }
            return cs;
        }
    }

    //remove normal or composite setting
    public void RemoveSetting(string settingName) {
        dataSettings.Values.Remove(settingName);
    }

}