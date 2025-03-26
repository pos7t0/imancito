using AideTool.Geometry;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using AideTool.Extensions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AideTool
{
    public static partial class Aide
    {
        private static partial bool IsInDevelopment();
        private static partial LogLevel LevelLogued();
        private static partial void DefineLevelFormat(string log, LogLevel logLevel);
        private static Dictionary<string, LogWatch> m_watchers;

        #region Log
        public static void Log(object obj, LogLevel logLevel = LogLevel.Info)
        {
            object[] args = new object[] { obj };

            if(obj is object[] objs)
                args = objs;

            if (obj is List<string> list)
                args = list.ToArray();

            string log = FormatLog(args);
            DefineLevelFormat(log, logLevel);
        }

        public static void Assert(bool assertion, string ifTrue, string ifFalse, LogLevel logLevel = LogLevel.Debug)
        {
            if(assertion)
            {
                Log(ifTrue, logLevel);
                return;
            }
            Log(ifFalse, logLevel);
        }

        public static void Assert(bool assertion, string ifTrue, LogLevel logLevel = LogLevel.Debug)
        {
            if(assertion)
                Log(ifTrue, logLevel);
        }

        public static void LogWarning(object obj)
        {
            Log(obj, LogLevel.Warning);
        }

        public static void LogError(object obj)
        {
            Log(obj, LogLevel.Error);
        }

        public static void LogException(Exception ex)
        {
            List<string> list = new()
            {
                ex.GetType().ToString(),
                ex.Message
            };
            Log(list, LogLevel.Exception);
        }

        public static void Debug(params object[] objs)
        {
            Log(objs, LogLevel.Debug);
        }

        public static void LogVerbose<T>(T obj)
        {
            if (LogNull(obj))
                return;

            List<string> log = new() { obj.GetType().ToString() };

            log.AddRange(GetObjectProperties(obj));

            Log(log, LogLevel.Verbose);
        }

        public static void LogList<T>(IEnumerable<T> list, Func<T, string> predicate = null)
        {
            if (LogNull(list))
                return;

            List<string> log = new() { list.GetType().ToString() };

            int index = 0;

            string handleText(T o)
            {
                if (predicate == null)
                    return $"[{index}]: {o}";
                
                string r = predicate(o);
                return $"[{index}] : {r}";
            }

            foreach (T obj in list)
            {
                log.Add(handleText(obj));
                index++;
            }

            Log(log, LogLevel.Verbose);
        }

        public static void LogDictionary<TKey, TValue>(Dictionary<TKey, TValue> dictionary, string dictionaryName = "Dictionary Log")
        {
            if (LogNull(dictionary))
                return;

            List<string> log = new() { dictionaryName };

            foreach (KeyValuePair<TKey, TValue> kv in dictionary)
            {
                if (kv.Value != null)
                {
                    log.Add($"[{kv.Key}]: {kv.Value}");
                    continue;
                }
                
                log.Add($"[{kv.Key}]: <i>null</i>");
            }

            Log(log, LogLevel.Verbose);
        }

        private static string[] GetObjectProperties<T>(T obj)
        {
            List<string> objMsgs = new();

            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (PropertyInfo p in properties)
            {
                string value;
                try
                {
                    value = p.GetValue(obj).ToString();
                }
                catch (NullReferenceException)
                {
                    value = "<i>null</i>";
                }
                catch(NotSupportedException)
                {
                    value = "<i>deprecated</i>";
                }
                catch(Exception ex)
                {
                    value = $"<i>{ex.GetType().ToString().ToLower()}</i>";
                }

                string msg = $"\"{p.Name}\": {value}";
                objMsgs.Add(msg);
            }

            return objMsgs.ToArray();
        }

        private static string FormatLog(object[] objs)
        {
            StringBuilder builder = new();
            foreach (object obj in objs)
            {
                if (obj != null)
                    builder.AppendLine(obj.ToString());
                else
                    builder.AppendLine("<i>null</i>");
            }
            return builder.ToString();
        }

        private static void LogColor(string log, Color color)
        {
            string colorString = color.ColorToHex();
            string[] logs = log.Split("\n");

            StringBuilder builder = new();

            foreach (string line in logs)
                builder.AppendLine($"<color={colorString}>{line}</color>");

            UnityEngine.Debug.Log(builder.ToString());
        }

        private static bool LogNull(object obj)
        {
            if(obj == null)
            {
                Log("<i>null</i>", LogLevel.Error);
                return true;
            }
            return false;
        }

        public static void ClearLog(MonoBehaviour sender)
        {
#if UNITY_EDITOR
            Assembly assembly = Assembly.GetAssembly(typeof(Editor));
            Type log = assembly.GetType("UnityEditor.LogEntries");
            MethodInfo method = log.GetMethod("Clear");
            method.Invoke(sender, null);
#endif
        }
        #endregion

        #region Watch
        public static void StartWatch(string operationName, out string watcherId)
        {
            watcherId = null;
            if(IsInDevelopment())
            {
                if (m_watchers == null)
                    m_watchers = new();

                LogWatch watch = new(operationName);
                m_watchers.Add(watch.WatchId, watch);
                m_watchers[watch.WatchId].Start();
                watcherId = watch.WatchId;
            }
        }

        public static void StopWatch(string watchId)
        {
            if(IsInDevelopment())
            {
                if(m_watchers.ContainsKey(watchId))
                    m_watchers[watchId].Stop();

                List<string> disposedWatchersList = new();

                foreach(KeyValuePair<string, LogWatch> kv in m_watchers)
                    if(kv.Value.WatchId == null)
                        disposedWatchersList.Add(kv.Key);

                foreach (string id in disposedWatchersList)
                    m_watchers.Remove(id);
            }
        }
        #endregion

        #region Geometry
        public static void DrawRay(Ray ray, float distance, Color color, bool debugRay = false)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.DrawRay(ray.origin, ray.direction * distance, color);
            if (debugRay)
                Debug("Debug Ray", $"Origin: {ray.origin}", $"Direction: {ray.direction}", $"Magnitude: {ray.direction.magnitude}", $"Distance: {distance}");
#endif
        }

        public static void DrawRay(Ray ray, float distance, Color color, Vector3 offset, bool debugRay = false)
        {
            Ray newRay = new(ray.origin + offset, ray.direction);
            DrawRay(newRay, distance, color, debugRay);
        }

        public static void DrawRay(Ray ray, float distance)
        {
#if UNITY_EDITOR
            DrawRay(ray, distance, Color.red);
#endif
        }

        public static void DrawLine(Vector3 origin, Vector3 direction, float distance)
        {
#if UNITY_EDITOR
            DrawLine(origin, direction, distance, Color.red);
#endif
        }

        public static void DrawLine(Vector3 origin, Vector3 direction, float distance, Color color)
        {
#if UNITY_EDITOR
            Ray ray = new(origin, direction);
            DrawRay(ray, distance, color);
#endif
        }

        public static void DrawLine(Vector3 origin, Vector3 direction, float distance, Color color, Vector3 offset)
        {
#if UNITY_EDITOR
            Ray ray = new(origin, direction);
            DrawRay(ray, distance, color, offset);
#endif
        }

        public static void DrawLine(Line line, Color color)
        {
#if UNITY_EDITOR
            /*Debug.DrawLine(line.Start, line.End, color);
            if (debugLine)
                DebugLog($"Debug Line", $"Start: {line.Start}", $"End; {line.End}");*/
#endif
        }

        public static void DrawLine(Line line)
        {
#if UNITY_EDITOR
            DrawLine(line, Color.red);
#endif
        }

        public static void DrawLineArray(Vector3 origin, Vector3[] points, Color color)
        {
#if UNITY_EDITOR
            int length = points.Length - 1;
            for (int i = 0; i < length; i++)
            {
                Vector3 first = points[i];
                Vector3 second = points[i + 1];

                Vector3 start = new(origin.x + first.x, origin.y + first.y, origin.z + first.z);
                Vector3 end = new(origin.x + second.x, origin.y + second.y, origin.z + second.z);
                //Line line = new(start, end);
                //DrawLine(line, color, debugLine);
            }
#endif
        }

        public static void DrawLineArray(Vector3 origin, Vector3[] points)
        {
#if UNITY_EDITOR
            DrawLineArray(origin, points, Color.red);
#endif
        }

        public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
        {
#if UNITY_EDITOR
            Box box = new(origin, halfExtents, orientation);

            DrawBox(box, color);
#endif
        }

        public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation)
        {
#if UNITY_EDITOR
            DrawBox(origin, halfExtents, orientation, AideColors.Red);
#endif
        }

        public static void DrawBox(Box box)
        {
#if UNITY_EDITOR
            DrawBox(box, AideColors.Red);
#endif
        }

        public static void DrawBox(Box box, Color color)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.DrawLine(box.FrontTopLeft, box.FrontTopRight, color);
            UnityEngine.Debug.DrawLine(box.FrontTopRight, box.FrontBottomRight, color);
            UnityEngine.Debug.DrawLine(box.FrontBottomRight, box.FrontBottomLeft, color);
            UnityEngine.Debug.DrawLine(box.FrontBottomLeft, box.FrontTopLeft, color);

            UnityEngine.Debug.DrawLine(box.BackTopLeft, box.BackTopRight, color);
            UnityEngine.Debug.DrawLine(box.BackTopRight, box.BackBottomRight, color);
            UnityEngine.Debug.DrawLine(box.BackBottomRight, box.BackBottomLeft, color);
            UnityEngine.Debug.DrawLine(box.BackBottomLeft, box.BackTopLeft, color);

            UnityEngine.Debug.DrawLine(box.FrontTopLeft, box.BackTopLeft, color);
            UnityEngine.Debug.DrawLine(box.FrontTopRight, box.BackTopRight, color);
            UnityEngine.Debug.DrawLine(box.FrontBottomRight, box.BackBottomRight, color);
            UnityEngine.Debug.DrawLine(box.FrontBottomLeft, box.BackBottomLeft, color);
#endif
        }

        public static void DrawBoxCast(Vector3 origin, Vector3 halfExtents, Vector3 direction, float distance)
        {
#if UNITY_EDITOR

#endif
        }
#endregion

#region Editor
        public static void Pause([CallerFilePath] string filepath = "", [CallerMemberName] string methodName = "", [CallerLineNumber] int lineNumber = 0)
        {
#if UNITY_EDITOR
            StringBuilder builder = new();
            builder.Append("Editor Pause was called from");
            
            int assetsIndex = filepath.LastIndexOf("Assets");
            filepath = filepath[assetsIndex..];
            builder.AppendLine($"...{filepath}");

            builder.AppendLine($"{methodName}()");
            builder.AppendLine($"Line: {lineNumber}");

            Log(builder.ToString(), LogLevel.Verbose);

            UnityEngine.Debug.Break();
#endif
        }

        public static void ExitApplication()
        {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
        }
#endregion
    }
}
