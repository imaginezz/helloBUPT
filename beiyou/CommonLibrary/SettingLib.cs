using System;
using System.Collections.Generic;
using Windows.Storage;

class SettingLib {

    private ApplicationDataContainer dataSettings;

    private string tempSettingName;
    private Object tempSettingValue;
    private string tempContainerName;
    private string tempCompositeName;
    private ApplicationDataContainer tempContainer;
    private ApplicationDataCompositeValue tempComposite;
    private ApplicationDataCompositeValue tempReadComposite;

    private bool isAutoClearTemp;

    public SettingLib(bool isRoaming = true,bool isAutoClearTemp=true) {
        if (isRoaming) {
            dataSettings = ApplicationData.Current.RoamingSettings;
        } else {
            dataSettings = ApplicationData.Current.LocalSettings;
        }
        this.isAutoClearTemp = isAutoClearTemp;
        tempContainer = null;
        tempComposite = null;
        tempReadComposite = null;
    }

    public SettingLib CreateSetting(string key, Object value) {
        tempSettingName = key;
        tempSettingValue = value;
        return this;
    }
    public SettingLib CreateComposite(string compositeName, List<Tuple<string, Object>> vals) {
        ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
        foreach (Tuple<string, Object> v in vals) {
            composite[v.Item1] = v.Item2;
        }
        tempComposite = composite;
        tempCompositeName = compositeName;
        return this;
    }
    //不考虑嵌套Container
    public SettingLib CreateContainer(string containerName) {
        ApplicationDataContainer container = dataSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);
        tempContainer = container;
        tempContainerName = containerName;
        return this;
    }

    //不考虑嵌套Container
    public SettingLib SaveSetting() {
        if (tempContainer != null) {
            if (dataSettings.Containers.ContainsKey(tempContainerName)) {
                if (tempComposite != null) {
                    dataSettings.Containers[tempContainerName].Values[tempCompositeName] = tempComposite;
                } else {
                    dataSettings.Containers[tempContainerName].Values[tempSettingName] = tempSettingValue;
                }
            } else {
                throw new ArgumentException();
            }
        } else {
            if (tempComposite != null) {
                dataSettings.Values[tempCompositeName] = tempComposite;
            } else {
                dataSettings.Values[tempSettingName] = tempSettingValue;
            }
        }
        if (isAutoClearTemp) {
            ClearTemp();
        }
        return this;
    }

    public SettingLib ClearTemp() {
        tempSettingValue = null;
        tempContainer = null;
        tempComposite = null;
        tempReadComposite = null;
        return this;
    }
    public SettingLib ClearTemp(string content) {
        switch (content) {
            case "Setting":
                tempSettingValue = null;
                break;
            case "Container":
                tempContainer = null;
                break;
            case "Composite":
                tempComposite = null;
                break;
            case "ReadComposite":
                tempReadComposite = null;
                break;
            default:
                throw new ArgumentException();
        }
        return this;
    }

    public object ReadSetting(string key) {
        object value = dataSettings.Values[key];
        return value;
    }

    //不考虑嵌套Container
    public List<Tuple<string, Object>> ReadSetting(string compositeName, List<string> settingNames) {
        ApplicationDataCompositeValue composite;
        if (tempReadComposite != null) {
            composite = tempReadComposite;
        } else {
            composite = (ApplicationDataCompositeValue)ReadSetting(compositeName);
        }
        if (composite == null) {
            return null;
        } else {
            List<Tuple<string, Object>> cs = new List<Tuple<string, object>>();
            foreach (string sn in settingNames) {
                cs.Add(new Tuple<string, Object>(sn, composite[sn]));
            }
            return cs;
        }
    }
    //不考虑嵌套Container
    public object ReadSetting(string containerName, string settingName) {
        bool hasContiner = dataSettings.Containers.ContainsKey(containerName);
        bool hasSetting = false;
        object settingValue = null;
        if (hasContiner) {
            hasSetting = dataSettings.Containers[containerName].Values.ContainsKey(settingName);
        }
        if (hasSetting) {
            settingValue = dataSettings.Containers[containerName].Values[settingName];
        }
        return settingValue;
    }
    public List<Tuple<string, object>> ReadSetting(string containerName, string compositeName, List<string> settingNames) {
        tempReadComposite = (ApplicationDataCompositeValue)ReadSetting(containerName, compositeName);
        List<Tuple<string, object>> ret = ReadSetting(null, settingNames);
        if (isAutoClearTemp) {
            ClearTemp();
        }
        return ret;
    }

    //不考虑嵌套Container
    public void RemoveSetting(string settingName,bool isContainer=false) {
        if (isContainer) {
            dataSettings.DeleteContainer(settingName);
        }else {
            dataSettings.Values.Remove(settingName);
        }
    }

}