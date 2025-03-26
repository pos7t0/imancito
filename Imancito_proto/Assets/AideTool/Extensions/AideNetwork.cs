using AideTool.DataTransfer;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.Networking;

namespace AideTool.Extensions
{
    public static class AideNetwork
    {
        public static UnityWebRequest PostJson<T>(string url, T data)
        {
            string jsonText = JsonConvert.SerializeObject(data);
            byte[] jsonBin = Encoding.UTF8.GetBytes(jsonText);

            DownloadHandlerBuffer downloadHandler = new DownloadHandlerBuffer();

            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(jsonBin);
            uploadHandler.contentType = "application/json";

            UnityWebRequest outputRequest = new UnityWebRequest(url, "POST", downloadHandler, uploadHandler);
            return outputRequest;
        }

        public static UnityWebRequest PutJson<T>(string url, T data)
        {
            string jsonText = JsonConvert.SerializeObject(data);
            byte[] jsonBin = Encoding.UTF8.GetBytes(jsonText);

            DownloadHandlerBuffer downloadHandler = new DownloadHandlerBuffer();

            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(jsonBin);
            uploadHandler.contentType = "application/json";

            UnityWebRequest outputRequest = new UnityWebRequest(url, "PUT", downloadHandler, uploadHandler);
            return outputRequest;
        }

        public static UnityWebRequest DeleteJson<T>(string url, T data)
        {
            string jsonText = JsonConvert.SerializeObject(data);
            byte[] jsonBin = Encoding.UTF8.GetBytes(jsonText);

            DownloadHandlerBuffer downloadHandler = new DownloadHandlerBuffer();

            UploadHandlerRaw uploadHandler = new UploadHandlerRaw(jsonBin);
            uploadHandler.contentType = "application/json";

            UnityWebRequest outputRequest = new UnityWebRequest(url, "DELETE", downloadHandler, uploadHandler);
            return outputRequest;
        }

        public static bool Check(this RequestResult result)
        {
            if (!result.IsOK)
                return false;
            return true;
        }

        public static bool Check(this UnityWebRequest request)
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                Aide.LogWarning(request);
                return false;
            }
            return true;
        }
    }
}
