using System;
using System.Collections.Generic;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

class ToastLib {

    private ToastTemplateType toastTemplate;
    private XmlDocument toastXml;
    private IXmlNode toastNode;

    private int toastType;
    private int ToastType {
        set {
            if (value >= 0 && value <= 7) {
                toastType = value;
            }else {
                throw new ArgumentOutOfRangeException();
            }
        }
        get {
            return toastType;
        }
    }

    public ToastLib(int type = 0) {
        ToastType = type;
        toastTemplate = (ToastTemplateType)ToastType;
        toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
    }

    public ToastLib makeToast(List<string> toastPara) {
        int[] toastParaNumArr = { 2, 3, 3, 4, 1, 2, 2, 3 };
        if (toastPara.Count == toastParaNumArr[ToastType]) {
            switch (ToastType) {
                case 0:
                    XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                    toastTextElements[0].AppendChild(toastXml.CreateTextNode("消息内容"));
                    XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
                    ((XmlElement)toastImageAttributes[0]).SetAttribute("src", $"ms-appx:///assets/图片文件名");
                    ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
            }
        }else {
            throw new ArgumentException();
        }
        return this;
    }

    //para example "{\"type\":\"toast\",\"param1\":\"12345\",\"param2\":\"67890\"}"
    public ToastLib addToastLaunchPara(string launchPara) {
        ((XmlElement)toastNode).SetAttribute("launch", launchPara);
        return this;
    }

    //the avaliable tones are here:Default,IM,Mail,Reminder,SMS,Looping.Alarm,Looping.Alarm2,Looping.Alarm3,Looping.Alarm4,Looping.Alarm5,Looping.Alarm6,Looping.Alarm7,Looping.Alarm8,Looping.Alarm9,Looping.Alarm10,Looping.Call,Looping.Call2,Looping.Call3,Looping.Call4,Looping.Call5,Looping.Call6,Looping.Call7,Looping.Call8,Looping.Call9,Looping.Call10
    public ToastLib setToastTone(string tone) {
        XmlElement audio = toastXml.CreateElement("audio");
        audio.SetAttribute("src", $"ms-winsoundevent:Notification."+tone);
        toastNode.AppendChild(audio);
        return this;
    }

    //only 2 types of paras short or long
    public ToastLib setToastDuringTime(string duringTime) {
        ((XmlElement)toastNode).SetAttribute("duration", duringTime);
        return this;
    }

    public void showToast() {
        ToastNotification toast = new ToastNotification(toastXml);
        ToastNotificationManager.CreateToastNotifier().Show(toast);
    }
}