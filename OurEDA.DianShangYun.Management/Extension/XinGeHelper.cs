using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;
using ssdutPushServiceByTxXinGe;

namespace Extension.XinGe
{
    public class XinGeHelper
    {
        public string access_id { get; set; } //应用的唯一标识符，在提交应用时管理系统返回
        public uint valid_time { get; set; }

        public uint environment = 1;

        public string secret; //用于生成MD5的密钥

        public uint? expire_time;

        public string send_time;

        public uint? multi_pkg;

        public static string domina = "openapi.xg.qq.com/";

        public static string address = "http://" + domina;

        public XinGeHelper(string access_id, string secret_key)
        {
            secret = secret_key;
            this.access_id = access_id;
        }

        /// <summary>
        /// 推送到 单个设备 IOS
        /// </summary>
        /// <param name="DeviceToken"></param>
        /// <param name="msg"></param>
        /// <param name="expire_time"></param>
        /// <param name="send_time"></param>
        /// <returns></returns>
        public Ret PushToSingle_IOS(string DeviceToken, Msg_IOS msg)
        {
            return _PushToSingle(DeviceToken, JsonConvert.SerializeObject(msg), 0, environment);
        }

        /// <summary>
        /// 推送到 单个设备 安卓
        /// </summary>
        /// <param name="DeviceToken"></param>
        /// <param name="msg"></param>
        /// <param name="message_type"></param>
        /// <param name="expire_time"></param>
        /// <param name="send_time"></param>
        /// <param name="multi_pkg"></param>
        /// <returns></returns>
        public Ret PushToSingle_Android(string DeviceToken, Msg_Android msg, uint message_type)
        {
            return _PushToSingle(DeviceToken, JsonConvert.SerializeObject(msg), message_type, 0);
        }

        /// <summary>
        /// 推送到 单个用户 IOS
        /// </summary>
        /// <param name="Account"></param>
        /// <param name="msg"></param>
        /// <param name="message_type"></param>
        /// <param name="expire_time"></param>
        /// <param name="send_time"></param>
        /// <returns></returns>
        public Ret PushToAccount_IOS(string Account, Msg_IOS msg, uint message_type)
        {
            return _PushToAccount(Account, JsonConvert.SerializeObject(msg), message_type, environment);
        }

        /// <summary>
        /// 推送到 单个用户 Android
        /// </summary>
        /// <param name="DeviceToken"></param>
        /// <param name="msg"></param>
        /// <param name="message_type"></param>
        /// <param name="expire_time"></param>
        /// <param name="send_time"></param>
        /// <param name="multi_pkg"></param>
        /// <returns></returns>
        public Ret PushToAccount_Android(string DeviceToken, Msg_Android msg, uint message_type)
        {
            return _PushToAccount(DeviceToken, JsonConvert.SerializeObject(msg), message_type, 0);
        }

        /// <summary>
        /// 推送到 所有用户 IOS
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="expire_time"></param>
        /// <param name="send_time"></param>
        /// <param name="multi_pkg"></param>
        /// <param name="loop_times"></param>
        /// <param name="loop_interval"></param>
        /// <returns></returns>
        public Ret PushToAll_IOS(Msg_IOS msg, uint? loop_times, uint? loop_interval)
        {
            return _PushToAll(JsonConvert.SerializeObject(msg), 0, environment,
                 loop_times, loop_interval);
        }

        /// <summary>
        /// 推送到 所有用户 Android
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="message_type"></param>
        /// <param name="expire_time"></param>
        /// <param name="send_time"></param>
        /// <param name="multi_pkg"></param>
        /// <param name="loop_times"></param>
        /// <param name="loop_interval"></param>
        /// <returns></returns>
        public Ret PushToAll_Android(Msg_Android msg, uint message_type,
            uint? loop_times, uint? loop_interval)
        {
            return _PushToAll(JsonConvert.SerializeObject(msg), message_type, 0 ,
                 loop_times, loop_interval);
        }

        /// <summary>
        /// 推送到 Tags 指定设备 IOS
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="taglist"></param>
        /// <param name="tags_op"></param>
        /// <param name="expire_time"></param>
        /// <param name="send_time"></param>
        /// <param name="multi_pkg"></param>
        /// <param name="loop_times"></param>
        /// <param name="loop_interval"></param>
        /// <returns></returns>
        public Ret PushToTags_IOS(Msg_IOS msg, string taglist, string tags_op,
            uint? loop_times, uint? loop_interval)
        {
            return _PushToTags(JsonConvert.SerializeObject(msg), taglist, tags_op, 0, environment,
                 loop_times, loop_interval);
        }

        /// <summary>
        /// 推送到 Tags 指定设备  Android
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="message_type"></param>
        /// <param name="taglist"></param>
        /// <param name="tags_op"></param>
        /// <param name="expire_time"></param>
        /// <param name="send_time"></param>
        /// <param name="multi_pkg"></param>
        /// <param name="loop_times"></param>
        /// <param name="loop_interval"></param>
        /// <returns></returns>
        public Ret PushToTags_Android(Msg_Android msg, uint message_type, string taglist, string tags_op,
             uint? loop_times, uint? loop_interval)
        {
            return _PushToTags(JsonConvert.SerializeObject(msg), taglist, tags_op, message_type, 0,
                 loop_times, loop_interval);
        }

        /// <summary>
        /// 获取群发发送状态
        /// </summary>
        public Ret GetStatus(List<string> PushIds)
        {

            List<Object> push_ids = new List<object>();
            foreach (string s in PushIds)
            {
                push_ids.Add(new
                {
                    push_id = s
                });
            }
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("push_ids", JsonConvert.SerializeObject(push_ids));
            return Emit(parameters, "v2/push/get_msg_status");
        }

        /// <summary>
        /// 查询应用覆盖的设备数
        /// </summary>
        /// <returns></returns>
        public Ret GetDeviceNum()
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            return Emit(parameters, "v2/application/get_app_device_num");
        }

        /// <summary>
        /// 查询应用的Tags
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public Ret QueryTag(uint? start, uint? limit)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            if (start.HasValue)
            {
                parameters.Add("start", start.ToString());
            }
            if (limit.HasValue)
            {
                parameters.Add("limit", limit.ToString());
            }
            return Emit(parameters, "v2/tags/query_app_tags");
        }

        /// <summary>
        /// 取消定时任务
        /// </summary>
        /// <returns></returns>
        public Ret CancalTimingTask(string PushId)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("push_id", PushId);
            return Emit(parameters, "v2/application/get_app_device_num");
        }




        private Ret _PushToSingle(string DeviceToken, string msg,
            uint message_type, uint environment)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("device_token", DeviceToken);
            parameters.Add("message", msg);
            parameters.Add("message_type", message_type.ToString());
            parameters.Add("environment", environment.ToString());
            return EmitEx(parameters, "v2/push/single_device");
        }

        private Ret _PushToAccount(string account, string msg,
            uint message_type, uint environment)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("account", account);
            parameters.Add("message", msg);
            parameters.Add("message_type", message_type.ToString());
            parameters.Add("environment", environment.ToString());
            return EmitEx(parameters, "v2/push/single_account");
        }

        private Ret _PushToAll(string msg,
            uint message_type, uint environment, uint? loop_times, uint? loop_interval)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            if (loop_times.HasValue)
            {
                parameters.Add("loop_times", loop_times.Value.ToString());
            }
            if (loop_interval.HasValue)
            {
                parameters.Add("loop_interval", loop_interval.Value.ToString());
            }
            parameters.Add("message", msg);
            parameters.Add("message_type", message_type.ToString());
            parameters.Add("environment", environment.ToString());
            return EmitEx(parameters, "v2/push/all_device");
        }

        private Ret _PushToTags(string msg, string taglist, string tags_op,
            uint message_type, uint environment, uint? loop_times, uint? loop_interval)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            if (loop_times.HasValue)
            {
                parameters.Add("loop_times", loop_times.Value.ToString());
            }
            if (loop_interval.HasValue)
            {
                parameters.Add("loop_interval", loop_interval.Value.ToString());
            }
            parameters.Add("message", msg);
            parameters.Add("tags_list", taglist);
            parameters.Add("tags_op", tags_op);
            parameters.Add("message_type", message_type.ToString());
            parameters.Add("environment", environment.ToString());
            return EmitEx(parameters, "v2/push/tags_device");
        }

        private Ret EmitEx(IDictionary<string, string> parameters, string url)
        {
            if (expire_time.HasValue)
            {
                parameters.Add("expire_time", expire_time.Value.ToString());
            }
            if (send_time != null && send_time != String.Empty && send_time != "")
            {
                parameters.Add("send_time", send_time);
            }
            if (multi_pkg.HasValue)
            {
                parameters.Add("multi_pkg", multi_pkg.Value.ToString());
            }
            return Emit(parameters, url);
        }

        private Ret Emit(IDictionary<string, string> parameters, string url)
        {
            parameters.Add("access_id", access_id);
            parameters.Add("timestamp", ((int)(DateTime.Now - TimeZone.CurrentTimeZone.ToLocalTime(
                new System.DateTime(1970, 1, 1))).TotalSeconds).ToString());
            string md5sing = getSignature(parameters, secret, domina + url);
            parameters.Add("sign", md5sing);
            var res = HttpWebResponseUtility.CreatePostHttpResponse(address + url, parameters, null, null, Encoding.UTF8, null);
            var resstr = res.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(resstr);
            var resstring = sr.ReadToEnd();
            return JsonConvert.DeserializeObject<Ret>(resstring);
        }



        /// <summary>
        /// 计算参数签名
        /// </summary>
        /// <param name="params">请求参数集，所有参数必须已转换为字符串类型</param>
        /// <param name="secret">签名密钥</param>
        /// <returns>签名</returns>
        public string getSignature(IDictionary<string, string> parameters, string secret, string url)
        {
            // 先将参数以其参数名的字典序升序进行排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parameters);
            IEnumerator<KeyValuePair<string, string>> iterator = sortedParams.GetEnumerator();

            // 遍历排序后的字典，将所有参数按"key=value"格式拼接在一起
            StringBuilder basestring = new StringBuilder();
            basestring.Append("POST").Append(url);
            while (iterator.MoveNext())
            {
                string key = iterator.Current.Key;
                string value = iterator.Current.Value;
                if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
                {
                    basestring.Append(key).Append("=").Append(value);
                }
            }
            basestring.Append(secret);

            // 使用MD5对待签名串求签
            MD5 md5 = MD5.Create();
            byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(basestring.ToString()));

            // 将MD5输出的二进制结果转换为小写的十六进制
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = bytes[i].ToString("x");
                if (hex.Length == 1)
                {
                    result.Append("0");
                }
                result.Append(hex);
            }

            return result.ToString();
        }

    }

    public class Ret
    {
        public int ret_code { get; set; } //返回码
        public string err_msg { get; set; }	//	请求出错时的错误信息
        public dynamic result { get; set; }  //请求正确时，若有额外数据要返回，则结果封装在该字段的json中。若无额外数据，则可能无此字段

        //         0	调用成功
        //         -1	参数错误，请对照错误提示和文档检查请求参数
        //         -2	请求时间戳不在有效期内
        //         -3	sign校验无效，检查access id和secret key（注意不是access key）
        //         2	参数错误，请对照文档检查请求参数
        //         7	别名/账号绑定的终端数满了（10个）
        //         14	收到非法token，例如ios终端没能拿到正确的token
        //         15	信鸽逻辑服务器繁忙
        //         19	操作时序错误
        //         例如进行tag操作前未获取到deviceToken 没有获取到deviceToken的原因: 1.没有注册信鸽或者苹果推送。 2.provisioning profile制作不正确。
        //         40	推送的token没有在信鸽中注册，请检查终端注册是否成功
        //         48	推送的账号没有在信鸽中注册，请检查终端注册是否成功
        //         71	APNS服务器繁忙
        //         73	消息字符数超限，请减少消息内容再试
        //         76	请求过于频繁，请稍后再试
        //         100	APNS证书错误。请重新提交正确的证书
        //         其他	内部错误
    }


    public class Msg_IOS
    {
        public dynamic aps { get; set; }

        /// <summary>
        /// 构建一个简单的信息
        /// { "aps" : { "alert" : 内容}}
        /// </summary>
        /// <param name="msg">内容</param>
        public Msg_IOS(string msg)
        {
            this.aps = new
            {
                alert = msg,
            };
        }

        /// <summary>
        /// 构建一个内容复杂的消息
        /// 结构为
        /// 
        ///  {
        ///    "alert" : {
        ///           "body" : "",
        ///           "action-loc-key" : "PLAY"
        ///       },
        ///     "badge" : 5,
        ///       “category” : “INVITE_CATEGORY”
        ///       自定义也可不获取
        ///  }
        /// </summary>
        /// <param name="msg">内容字典</param>
        public Msg_IOS(IDictionary<string, string> msg)
        {
            this.aps = msg;
        }
    }

    public class Msg_IOSEx : Msg_IOS
    {
        public dynamic accept_time { get; set; }
        public dynamic custom { get; set; }

        /// <summary>
        /// 构建一个简单的信息
        /// { "aps" : { "alert" : 内容}}
        /// </summary>
        /// <param name="msg">内容</param>
        public Msg_IOSEx(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// 构建一个内容复杂的消息
        /// 结构为
        /// 
        ///  {
        ///    "alert" : {
        ///           "body" : "",
        ///           "action-loc-key" : "PLAY"
        ///       },
        ///     "badge" : 5,
        ///       “category” : “INVITE_CATEGORY”
        ///       自定义也可不获取
        ///  }
        /// </summary>
        /// <param name="msg">内容字典</param>
        public Msg_IOSEx(IDictionary<string, string> msg)
            : base(msg)
        {
        }

        /// <summary>
        /// 构建内容简单的消息 + 限定接受时间
        /// 限定时间结构:
        /// {
        ///     [
        ///         {
        ///             "start" :[],
        ///             "end" : [],
        ///          }
        ///      ]
        /// }
        /// </summary>
        /// <param name="msg">内容</param>
        /// <param name="acceptTime">获取时间</param>
        public Msg_IOSEx(string msg, IDictionary<string, string> acceptTime)
            : base(msg)
        {
            this.accept_time = acceptTime;
        }

        /// <summary>
        /// 构建内容复杂的消息 + 限定接受时间
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="acceptTime"></param>
        public Msg_IOSEx(IDictionary<string, string> msg, IDictionary<string, string> acceptTime)
            : base(msg)
        {
            this.accept_time = acceptTime;
        }


        /// <summary>
        /// 构建内容简单， 限定接受时间， 附带自定义内容的消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="acceptTime"></param>
        /// <param name="custom"></param>
        public Msg_IOSEx(string msg, IDictionary<string, string> acceptTime, IDictionary<string, string> custom)
            : this(msg, acceptTime)
        {
            this.custom = custom;
        }

        /// <summary>
        /// 构建内容复杂， 限定接受时间， 附带自定义内容的消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="acceptTime"></param>
        /// <param name="custom"></param>
        public Msg_IOSEx(IDictionary<string, string> msg, IDictionary<string, string> acceptTime, IDictionary<string, string> custom)
            : this(msg, acceptTime)
        {
            this.custom = custom;
        }
    }

    public class Msg_Android
    {
        public Msg_Android(string title, string content)
        {
            this.title = title;
            this.content = content;
        }
        public string title { get; set; }
        public string content { get; set; }
        public dynamic custom { get; set; }
    }

    //通知消息
    public class Msg_Android_Info : Msg_Android
    {
        public Msg_Android_Info(string title, string content)
            : base(title, content)
        {
        }
        public dynamic accept_time { get; set; }
        public int n_id { get; set; }
        public int builder_id { get; set; }
        public int ring { get; set; }
        public string ring_raw { get; set; }
        public int vibrate { get; set; }
        public int lights { get; set; }
        public int clearable { get; set; }
        public int icon_type { get; set; }
        public string icon_res { get; set; }

        public int style_id { get; set; }
        public string small_icon { get; set; }
        public string action { get; set; }
    }

    //透传消息
    public class Msg_Android_TouChuan : Msg_Android
    {
        public Msg_Android_TouChuan(string title, string content)
            : base(title, content)
        {
        }
        public dynamic accept_time { get; set; }
    }

}