using System.Threading.Tasks;
using GameServices.GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.GameServices.Runtime.Save
{
    public sealed class NullSaveService : ISaveService
    {
        public string ServiceId => "save.null";
        public GameServiceStatus Status => GameServiceStatus.Disabled;
        public bool IsReady => true;

        public Task InitializeAsync(GameServiceContext context)
        {
            return Task.CompletedTask;
        }

        public bool HasKey(string key)
        {
            return false;
        }

        public string GetString(string key, string defaultValue = "")
        {
            return defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            return defaultValue;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return defaultValue;
        }

        public Vector2 GetVector2(string key, Vector2 defaultValue = default)
        {
            return defaultValue;
        }

        public Vector3 GetVector3(string key, Vector3 defaultValue = default)
        {
            return defaultValue;
        }

        public Quaternion GetQuaternion(string key, Quaternion defaultValue = default)
        {
            return defaultValue;
        }

        public Color GetColor(string key, Color defaultValue = default)
        {
            return defaultValue;
        }

        public T GetJson<T>(string key, T defaultValue = default)
        {
            return defaultValue;
        }

        public void SetString(string key, string value)
        {
        }

        public void SetInt(string key, int value)
        {
        }

        public void SetFloat(string key, float value)
        {
        }

        public void SetBool(string key, bool value)
        {
        }

        public void SetVector2(string key, Vector2 value)
        {
        }

        public void SetVector3(string key, Vector3 value)
        {
        }

        public void SetQuaternion(string key, Quaternion value)
        {
        }

        public void SetColor(string key, Color value)
        {
        }

        public void SetJson<T>(string key, T value)
        {
        }

        public void DeleteKey(string key)
        {
        }

        public void Clear()
        {
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }
    }
}


