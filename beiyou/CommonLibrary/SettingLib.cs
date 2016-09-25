using System;
using System.Collections.Generic;
using Windows.Storage;

class SettingLib {

    //声明存储容器
    private ApplicationDataContainer dataSettings;

    //声明一些临时变量
    private string tempSettingName;
    private object tempSettingValue;
    private string tempContainerName;
    private string tempCompositeName;
    private ApplicationDataContainer tempContainer;
    private ApplicationDataCompositeValue tempComposite;
    private ApplicationDataCompositeValue tempReadComposite;

    //是否自动清除缓存的变量
    private bool isAutoClearTemp;

    //构造函数，默认为漫游设置和自动清除历史设置的缓存
    public SettingLib(bool isRoaming = true, bool isAutoClearTemp = true) {
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

    //创建直接存储,需要传入一个键值对
    public SettingLib CreateSetting(string key, object value) {
        tempSettingName = key;
        tempSettingValue = value;
        return this;
    }
    //创建存储集合，需要传入集合的名称和键值对的List
    public SettingLib CreateComposite(string compositeName, List<Tuple<string, object>> vals) {
        ApplicationDataCompositeValue composite = new ApplicationDataCompositeValue();
        foreach (Tuple<string, object> v in vals) {
            composite[v.Item1] = v.Item2;
        }
        tempComposite = composite;
        tempCompositeName = compositeName;
        return this;
    }
    //创建存储容器，需要传入容器的名称
    //不考虑嵌套Container
    public SettingLib CreateContainer(string containerName) {
        ApplicationDataContainer container = dataSettings.CreateContainer(containerName, ApplicationDataCreateDisposition.Always);
        tempContainer = container;
        tempContainerName = containerName;
        return this;
    }

    //将之前创建的各种存储类型进行组合之后进行存储
    //可以在Container中包含Composite或者键值对，可以在Composite中包含多条键值对，可以直接存储键值对
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

    //一次操作后清除缓存的方法
    public SettingLib ClearTemp() {
        tempSettingValue = null;
        tempContainer = null;
        tempComposite = null;
        tempReadComposite = null;
        return this;
    }
    //如果没有设置自动清除缓存的话可以手动清除缓存,需要传入清除缓存的类型
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

    //读取键值对存储
    public object ReadSetting(string key) {
        object value = dataSettings.Values[key];
        return value;
    }

    //读取Composite中的所有键值对，并且以List返回
    //不考虑嵌套Container
    public List<Tuple<string, object>> ReadSetting(string compositeName, List<string> settingNames) {
        ApplicationDataCompositeValue composite;
        if (tempReadComposite != null) {
            composite = tempReadComposite;
        } else {
            composite = (ApplicationDataCompositeValue)ReadSetting(compositeName);
        }
        if (composite == null) {
            return null;
        } else {
            List<Tuple<string, object>> cs = new List<Tuple<string, object>>();
            foreach (string sn in settingNames) {
                cs.Add(new Tuple<string, object>(sn, composite[sn]));
            }
            return cs;
        }
    }
    //读取Container中指定名称的设置
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
    //读取Container中设置集合中的所有键值对
    public List<Tuple<string, object>> ReadSetting(string containerName, string compositeName, List<string> settingNames) {
        tempReadComposite = (ApplicationDataCompositeValue)ReadSetting(containerName, compositeName);
        List<Tuple<string, object>> ret = ReadSetting(null, settingNames);
        if (isAutoClearTemp) {
            ClearTemp();
        }
        return ret;
    }

    //删除某种设置
    //不考虑嵌套Container
    public void RemoveSetting(string settingName, bool isContainer = false) {
        if (isContainer) {
            dataSettings.DeleteContainer(settingName);
        } else {
            dataSettings.Values.Remove(settingName);
        }
    }

}