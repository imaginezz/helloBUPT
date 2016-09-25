using System;
using System.Collections.Generic;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

class ToastLib {

    //定义通知模板和XML节点
    private ToastTemplateType toastTemplate;
    private XmlDocument toastXml;
    private IXmlNode toastNode;
    private XmlNodeList toastTextElements;
    private XmlNodeList toastImageAttributes;

    //消息类型
    private int toastType;
    private int ToastType {
        set {
            if (value >= 0 && value <= 7) {
                toastType = value;
            } else {
                throw new ArgumentOutOfRangeException();
            }
        }
        get {
            return toastType;
        }
    }

    //构造函数，传入通知模板种类即可
    public ToastLib(int type = 0) {
        ToastType = type;
        toastTemplate = (ToastTemplateType)ToastType;
        toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
        toastTextElements = toastXml.GetElementsByTagName("text");
        toastImageAttributes = toastXml.GetElementsByTagName("image");
    }

    /*
     * 传入内容的XML文档
        *<toast>
        *    <visual>
        *        <binding template="ToastImageAndText01">
        *            <image id="1" src="" />
        *            <text id="1"></text>
        *        </binding>
        *    </visual>
        *</toast>
        *<toast>
        *    <visual>
        *        <binding template="ToastImageAndText02">
        *            <image id="1" src="" />
        *            <text id="1"></text>
        *            <text id="2"></text>
        *        </binding>
        *    </visual>
        *</toast>
        *<toast>
        *    <visual>
        *        <binding template="ToastImageAndText03">
        *            <image id="1" src="" />
        *            <text id="1"></text>
        *            <text id="2"></text>
        *        </binding>
        *    </visual>
        *</toast>
        *<toast>
        *    <visual>
        *        <binding template="ToastImageAndText04">
        *            <image id="1" src="" />
        *            <text id="1"></text>
        *            <text id="2"></text>
        *            <text id="3"></text>
        *        </binding>
        *    </visual>
        *</toast>
        *<toast>
        *    <visual>
        *        <binding template="ToastText01">
        *            <text id="1"></text>
        *        </binding>
        *    </visual>
        *</toast>
        *<toast>
        *    <visual>
        *        <binding template="ToastText02">
        *            <text id="1"></text>
        *            <text id="2"></text>
        *        </binding>
        *    </visual>
        *</toast>
        *<toast>
        *    <visual>
        *        <binding template="ToastText03">
        *            <text id="1"></text>
        *            <text id="2"></text>
        *        </binding>
        *    </visual>
        *</toast>
        *<toast>
        *    <visual>
        *        <binding template="ToastText04">
        *            <text id="1"></text>
        *            <text id="2"></text>
        *            <text id="3"></text>
        *        </binding>
        *    </visual>
        *</toast>
     */

    //生成通知XML的关键函数，用于传入通知图片的连接和通知文字的内容
    //传入字符串sample "消息内容" or $"ms-appx:///assets/图片文件名"
    public ToastLib makeToast(List<string> toastPara) {
        int[] toastParaNumArr = { 2, 3, 3, 4, 1, 2, 2, 3 };
        if (toastPara.Count == toastParaNumArr[ToastType]) {
            switch (ToastType) {
                case 0:
                    toastTextElements[0].AppendChild(toastXml.CreateTextNode(toastPara[0]));
                    ((XmlElement)toastImageAttributes[0]).SetAttribute("src", toastPara[1]);
                    ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");
                    break;
                case 1:
                    ((XmlElement)toastImageAttributes[0]).SetAttribute("src", toastPara[0]);
                    ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");
                    toastTextElements[0].AppendChild(toastXml.CreateTextNode(toastPara[1]));
                    toastTextElements[1].AppendChild(toastXml.CreateTextNode(toastPara[2]));
                    break;
                case 2:
                    ((XmlElement)toastImageAttributes[0]).SetAttribute("src", toastPara[0]);
                    ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");
                    toastTextElements[0].AppendChild(toastXml.CreateTextNode(toastPara[1]));
                    toastTextElements[1].AppendChild(toastXml.CreateTextNode(toastPara[2]));
                    break;
                case 3:
                    ((XmlElement)toastImageAttributes[0]).SetAttribute("src", toastPara[0]);
                    ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "logo");
                    toastTextElements[0].AppendChild(toastXml.CreateTextNode(toastPara[1]));
                    toastTextElements[1].AppendChild(toastXml.CreateTextNode(toastPara[2]));
                    toastTextElements[2].AppendChild(toastXml.CreateTextNode(toastPara[3]));
                    break;
                case 4:
                    toastTextElements[0].AppendChild(toastXml.CreateTextNode(toastPara[0]));
                    break;
                case 5:
                    toastTextElements[0].AppendChild(toastXml.CreateTextNode(toastPara[0]));
                    toastTextElements[1].AppendChild(toastXml.CreateTextNode(toastPara[1]));
                    break;
                case 6:
                    toastTextElements[0].AppendChild(toastXml.CreateTextNode(toastPara[0]));
                    toastTextElements[1].AppendChild(toastXml.CreateTextNode(toastPara[1]));
                    break;
                case 7:
                    toastTextElements[0].AppendChild(toastXml.CreateTextNode(toastPara[0]));
                    toastTextElements[1].AppendChild(toastXml.CreateTextNode(toastPara[1]));
                    toastTextElements[2].AppendChild(toastXml.CreateTextNode(toastPara[2]));
                    break;
            }
        } else {
            throw new ArgumentException();
        }
        return this;
    }

    //加入点按通知进入App以后向App传入的参数
    //para example "{\"type\":\"toast\",\"param1\":\"12345\",\"param2\":\"67890\"}"
    public ToastLib addToastLaunchPara(string launchPara) {
        ((XmlElement)toastNode).SetAttribute("launch", launchPara);
        return this;
    }

    //改变通知的声音
    //the avaliable tones are here:Default,IM,Mail,Reminder,SMS,Looping.Alarm,Looping.Alarm2,Looping.Alarm3,Looping.Alarm4,Looping.Alarm5,Looping.Alarm6,Looping.Alarm7,Looping.Alarm8,Looping.Alarm9,Looping.Alarm10,Looping.Call,Looping.Call2,Looping.Call3,Looping.Call4,Looping.Call5,Looping.Call6,Looping.Call7,Looping.Call8,Looping.Call9,Looping.Call10
    public ToastLib setToastTone(string tone) {
        XmlElement audio = toastXml.CreateElement("audio");
        audio.SetAttribute("src", $"ms-winsoundevent:Notification." + tone);
        toastNode.AppendChild(audio);
        return this;
    }

    //改变通知的时长
    //only 2 types of paras short or long
    public ToastLib setToastDuringTime(string duringTime) {
        ((XmlElement)toastNode).SetAttribute("duration", duringTime);
        return this;
    }

    //显示通知
    public void showToast() {
        ToastNotification toast = new ToastNotification(toastXml);
        ToastNotificationManager.CreateToastNotifier().Show(toast);
    }
}