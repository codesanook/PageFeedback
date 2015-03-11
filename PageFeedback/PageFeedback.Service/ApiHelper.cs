using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using log4net;

namespace PageFeedback.Service
{
    public class ApiHelper
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(ApiHelper));
        private readonly Uri _baseUri;

        public ApiHelper(string baseUrl)
        {
            _baseUri = new Uri(baseUrl);
        }

        private Uri CreateRequestUri(string path)
        {

            var uriBuilder = new UriBuilder(_baseUri);
            uriBuilder.Query = null;
            uriBuilder.Fragment = null;
            uriBuilder.Path = path;
            return uriBuilder.Uri;

        }

        private void LogJsonFormatting(dynamic jsonResult)
        {

            var jsonString = JsonConvert.SerializeObject(jsonResult, Formatting.Indented);
            _log.DebugFormat("********** json result formatting ***************\n\n{0}\n\n", jsonString);
        }


        public dynamic GetJson(string urlWithoutRoot)
        {
            dynamic result = null;
            try
            {
                //alway accept certificate
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.ServerCertificateValidationCallback 
                    += (sender, cert, chain, sslPolicyErrors) => true;

                var uri = CreateRequestUri(urlWithoutRoot);
                var webRequest = (HttpWebRequest)WebRequest.Create(uri);
                //over hash base url
                webRequest.ContentType = "application/json";
                webRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)webRequest.GetResponse();
                using (var sr = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = JsonConvert.DeserializeObject<dynamic>(sr.ReadToEnd());
                    LogJsonFormatting(result);
                    return result;
                }
            }
            catch (WebException ex)
            {
                _log.Error(ex);
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    _log.Debug(reader.ReadToEnd());
                }
            }

            result = new ExpandoObject();
            result.status = "error";
            return result;


        }

        public dynamic PostJson(string urlWithoutRoot,  object jsonObject)
        {
            dynamic result = null;
            try
            {

                ServicePointManager.ServerCertificateValidationCallback 
                    += (sender, cert, chain, sslPolicyErrors) => true;

                var uri = CreateRequestUri(urlWithoutRoot);
                _log.DebugFormat("request uri {0}", uri.AbsoluteUri);
                var webRequest = (HttpWebRequest)WebRequest.Create(uri.AbsoluteUri);
                webRequest.UserAgent = ".NET client";
                webRequest.Method = "POST";
                webRequest.ContentType = "application/json";

                //เตรียมข้อมูลที่จะส่งไป server
                var parameters = JsonConvert.SerializeObject(jsonObject);
                var byteArray = Encoding.UTF8.GetBytes(parameters);
                //กำหนดขนาดของข้อมูลที่จะส่งไปเท่ากับขนาดของข้อมูลรูปแบบ array ของ byte
                webRequest.ContentLength = byteArray.Length;

                //เขียนข้อมูลลง request stream
                using (var dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                //รับ response กลับมาจาก server
                var response = (HttpWebResponse)webRequest.GetResponse();

                //ตรวจสอบสถานะของ response ที่ส่งกลับมาว่าทำงานได้ถูกต้องไม่มีข้อผิดพลาดเกิดขึ้น
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    //อ่าน response stream ด้วย stream reader
                    using (var dataStream = response.GetResponseStream())
                    using (var reader = new StreamReader(dataStream))
                    {
                        //อ่าน stream ตั้งแต่เริ่มต้นจนสิ้นสุด
                        var responseFromServer = reader.ReadToEnd();
                        result = JsonConvert.DeserializeObject<dynamic>(responseFromServer);
                        LogJsonFormatting(result);
                        return result;
                    }
                }

            }
            catch (WebException ex)
            {
                _log.Error(ex);
                if (ex.Response != null)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        _log.Error(reader.ReadToEnd());
                    }
                }
            }

            result = new ExpandoObject();
            result.status = "error";
            return result;
        }

        public dynamic PutJson(string urlWithoutRoot, object userId, object jsonObject)
        {
            dynamic result = null;
            try
            {

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var uri = CreateRequestUri(urlWithoutRoot);
                _log.DebugFormat("request uri {0}", uri.AbsoluteUri);
                var webRequest = (HttpWebRequest)WebRequest.Create(uri.AbsoluteUri);
                webRequest.UserAgent = ".NET client";
                webRequest.Method = "PUT";
                webRequest.ContentType = "application/json";
                //เตรียมข้อมูลที่จะส่งไป server
                var parameters = JsonConvert.SerializeObject(jsonObject);
                var byteArray = Encoding.UTF8.GetBytes(parameters);
                //กำหนดขนาดของข้อมูลที่จะส่งไปเท่ากับขนาดของข้อมูลรูปแบบ array ของ byte
                webRequest.ContentLength = byteArray.Length;

                //เขียนข้อมูลลง request stream
                using (var dataStream = webRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                //รับ response กลับมาจาก server
                var response = (HttpWebResponse)webRequest.GetResponse();

                //ตรวจสอบสถานะของ response ที่ส่งกลับมาว่าทำงานได้ถูกต้องไม่มีข้อผิดพลาดเกิดขึ้น
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    //อ่าน response stream ด้วย stream reader
                    using (var dataStream = response.GetResponseStream())
                    using (var reader = new StreamReader(dataStream))
                    {
                        //อ่าน stream ตั้งแต่เริ่มต้นจนสิ้นสุด
                        var responseFromServer = reader.ReadToEnd();
                        result = JsonConvert.DeserializeObject<dynamic>(responseFromServer);
                        LogJsonFormatting(result);
                        return result;
                    }
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var stream = ex.Response.GetResponseStream())
                        if (stream != null)
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                _log.Debug(reader.ReadToEnd());
                            }
                        }
                }

            }

            result = new ExpandoObject();
            result.status = "error";
            return result;

        }


        public T PostHttpForm<T>(string urlWithoutRoot,  object jsonObject)
        {
            dynamic result = null;
            try
            {

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;





                var uri = CreateRequestUri(urlWithoutRoot);
                _log.DebugFormat("request uri {0}", uri.AbsoluteUri);
                var webRequest = (HttpWebRequest)WebRequest.Create(uri.AbsoluteUri);

                _log.DebugFormat("request uri {0}", uri.AbsoluteUri);
                webRequest.UserAgent = ".NET client";
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                _log.DebugFormat("request uri {0}", uri.AbsoluteUri);
                _log.DebugFormat("webRequest.ContentType: {0}", webRequest.ContentType);

                //เตรียมข้อมูลที่จะส่งไป server
                if (jsonObject != null)
                {
                    var type = jsonObject.GetType();
                    var parameterList = new List<string>();
                    foreach (var property in type.GetProperties())
                    {
                        parameterList.Add(string.Format("{0}={1}", 
                            property.Name, property.GetValue(jsonObject, null)));
                    }

                    var parametersString = string.Join("&", parameterList.ToArray());
                    _log.DebugFormat("parametersString: {0}", parametersString);

                    var byteArray = Encoding.UTF8.GetBytes(parametersString);
                    //กำหนดขนาดของข้อมูลที่จะส่งไปเท่ากับขนาดของข้อมูลรูปแบบ array ของ byte
                    webRequest.ContentLength = byteArray.Length;

                    //เขียนข้อมูลลง request stream
                    using (var dataStream = webRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                }
                else
                {
                    webRequest.ContentLength = 0;
                }

                //รับ response กลับมาจาก server
                var response = (HttpWebResponse)webRequest.GetResponse();

                //ตรวจสอบสถานะของ response ที่ส่งกลับมาว่าทำงานได้ถูกต้องไม่มีข้อผิดพลาดเกิดขึ้น
                if (HttpStatusCode.OK == response.StatusCode)
                {
                    //อ่าน response stream ด้วย stream reader
                    using (var dataStream = response.GetResponseStream())
                    using (var reader = new StreamReader(dataStream))
                    {
                        //อ่าน stream ตั้งแต่เริ่มต้นจนสิ้นสุด
                        var responseFromServer = reader.ReadToEnd();
                        result = JsonConvert.DeserializeObject<T>(responseFromServer);
                        LogJsonFormatting(result);
                        return result;
                    }
                }

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var stream = ex.Response.GetResponseStream())
                    using (var reader = new StreamReader(stream))
                    {
                        _log.Debug(reader.ReadToEnd());
                    }
                }
            }

            result = new ExpandoObject();
            result.status = "error";
            return result;
        }

    }


}
