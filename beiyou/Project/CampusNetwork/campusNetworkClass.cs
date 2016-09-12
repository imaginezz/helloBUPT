using System.Net;
using System.Threading.Tasks;
using System;

namespace beiyou.Project.CampusNetwork {
    class CampusNetworkClass {

        private SettingLib settingLib = new SettingLib();
        private bool isSettingSaved;

        public CampusNetworkClass() {
            checkIsSettingSaved(); 
            if (!isSettingSaved) {
                settingLib.CreateContainer("CampusNetwork").CreateSetting("isCampusNetworkAccountSaved", false);
                DebugLib.DebugOutput(isSettingSaved);
            }
        }

        //需要新增对http://10.4.1.2/的处理
        public async Task<string> Login(string id, string passwd) {
            string loginUrl = "http://10.3.8.211/";
            string postData = "DDDDD=" + id + "&upass=" + passwd + "&savePWD=0&0MKKey=";
            CookieCollection cc = new CookieCollection();
            cc.Add(new Cookie("myusername", id));
            cc.Add(new Cookie("username", id));
            cc.Add(new Cookie("smartdot", passwd));
            cc.Add(new Cookie("pwd", passwd));
            string response,ret;
            try {
                response = await webLib.HttpPost(loginUrl, postData, "gb2312", cc);
                string regStr;
                string regRet;
                //html [^(<title>))] 
                regStr = @"(?<=<title>)(.*?)(?=</title>)";
                regRet = RegLib.RegexMatch(regStr, ref response);
                if (regRet == "登录成功窗") {
                    DebugLib.DebugOutput("登录成功");
                    ret = "登陆成功";
                } else if (regRet == "信息返回窗") {
                    //javascrpit Msg=\d*;
                    regStr = @"(?<=Msg=)(\d*)(?=;)";
                    regRet = RegLib.RegexMatch(regStr, ref response);
                    DebugLib.DebugOutput(regRet);
                    int msgId = Int32.Parse(regRet);
                    DebugLib.DebugOutput(msgId);
                    string errInfo = "";
                    regStr = @"(?<=xip=')(.*)('?=;)";
                    regRet = RegLib.RegexMatch(regStr, ref response);
                    string xipStr = regRet;
                    DebugLib.DebugOutput(xipStr);
                    switch (msgId) {
                        case 0:
                            errInfo = "账号或密码不对，请重新输入";
                            break;
                        case 1:
                            regStr = @"(?<=msga=)(.*)(?=;)";
                            regRet = RegLib.RegexMatch(regStr, ref response);
                            string msgaStr = regRet;
                            if (msgaStr != "") {
                                switch (msgaStr) {
                                    case "error0":
                                        errInfo = "本IP不允许Web方式登录";
                                        break;
                                    case "error1":
                                        errInfo = "本账号不允许Web方式登录";
                                        break;
                                    case "error2":
                                        errInfo = "本账号不允许修改密码";
                                        break;
                                    default:
                                        errInfo = msgaStr;
                                        break;
                                }
                            }
                            break;
                        case 2:
                            DebugLib.DebugOutput(regRet);
                            errInfo = "该账号正在IP为：" + xipStr + "的机器上使用，<br><br>请点击<a href='a11.htm'>继续</a>断开它的连接并重新输入用户名和密码登陆本机。";
                            break;
                        case 3:
                            errInfo = "本账号只能在指定地址使用" + xipStr;
                            break;
                        case 4:
                            errInfo = "本账号费用超支或时长流量超过限制";
                            break;
                        case 5:
                            errInfo = "本账号暂停使用";
                            break;
                        case 6:
                            errInfo = "System buffer full";
                            break;
                        case 7:
                            errInfo = "余额啥的正常";
                            break;
                        case 8:
                            errInfo = "本账号正在使用,不能修改";
                            break;
                        case 9:
                            errInfo = "新密码与确认新密码不匹配,不能修改";
                            break;
                        case 10:
                            errInfo = "密码修改成功";
                            break;
                        case 11:
                            regStr = @"(?<=mac=')(.*)('?=;)";
                            regRet = RegLib.RegexMatch(regStr, ref response);
                            string macStr = regRet;
                            errInfo = "本账号只能在指定地址使用" + macStr;
                            break;
                        case 14:
                            errInfo = "注销成功";
                            break;
                        case 15:
                            errInfo = "登录成功";
                            break;
                    }
                    DebugLib.DebugOutput(errInfo);
                    ret = errInfo;
                } else {
                    DebugLib.DebugOutput("登录失败");
                    ret = "登陆失败";
                }
            } catch {
                ret = "网络错误，登陆失败";
            }
            return ret;
        }

        public async Task<string> Logout() {
            string logoutUrl = "http://10.3.8.211/F.html";
            string response,ret;
            try {
               response= await webLib.HttpGet(logoutUrl, null, "gb2312");
                ret = "注销成功";
            } catch {
                ret = "网络错误，注销失败";
            }
            return ret;
        }

        public void saveAccount(string id, string passwd) {
            settingLib.CreateContainer("CampusNetwork").CreateSetting("campusNetworkAccountId", id).SaveSetting();
            settingLib.CreateContainer("CampusNetwork").CreateSetting("campusNetworkAccountPasswd", passwd).SaveSetting();
            settingLib.CreateContainer("CampusNetwork").CreateSetting("isCampusNetworkAccountSaved", true).SaveSetting();
        }
        public void clearAccount() {
            settingLib.CreateContainer("CampusNetwork").CreateSetting("campusNetworkAccountId", "").SaveSetting();
            settingLib.CreateContainer("CampusNetwork").CreateSetting("campusNetworkAccountPasswd", "").SaveSetting();
            settingLib.CreateContainer("CampusNetwork").CreateSetting("isCampusNetworkAccountSaved", false).SaveSetting();
            DebugLib.DebugOutput("accountSettting cleared");
        }
        public void readAccount(out string id, out string passwd,out bool check) {
            checkIsSettingSaved();
            if (isSettingSaved) {
                id = (string)settingLib.ReadSetting("CampusNetwork", "campusNetworkAccountId");
                passwd = (string)settingLib.ReadSetting("CampusNetwork", "campusNetworkAccountPasswd");
                check = true;
            } else {
                id = string.Empty;
                passwd = string.Empty;
                check = false;
            }
        }
        private void checkIsSettingSaved() {
            isSettingSaved = false;
            object readIsSaved = settingLib.ReadSetting("CampusNetwork", "isCampusNetworkAccountSaved");
            if (readIsSaved != null && (bool)readIsSaved != false) {
                isSettingSaved = true;
            }
        }

        public void toast() {
            ToastLib toastLib = new ToastLib();
        }
    }
}