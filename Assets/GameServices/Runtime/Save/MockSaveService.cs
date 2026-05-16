using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.Runtime.Save
{
    public sealed class MockSaveService : ISaveService
    {
        private readonly Dictionary<string, string> _values = new();
        private readonly bool _logCalls;
        private GameServiceStatus _status = GameServiceStatus.NotInitialized;

        public MockSaveService(bool logCalls)
        {
            _logCalls = logCalls;
        }

        public string ServiceId => "save.mock";
        public GameServiceStatus Status => _status;
        public bool IsReady => _status == GameServiceStatus.Ready;

        public Task InitializeAsync(GameServiceContext context)
        {
            _status = GameServiceStatus.Ready;
            Log("Mock save service initialized.");
            return Task.CompletedTask;
        }

        public bool HasKey(string key)
        {
            return _values.ContainsKey(key);
        }

        public string GetString(string key, string defaultValue = "")
        {
            return _values.TryGetValue(key, out var value) ? value : defaultValue;
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return int.TryParse(GetString(key), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
                ? value
                : defaultValue;
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            return float.TryParse(GetString(key), NumberStyles.Float, CultureInfo.InvariantCulture, out var value)
                ? value
                : defaultValue;
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return bool.TryParse(GetString(key), out var value) ? value : defaultValue;
        }

        public Vector2 GetVector2(string key, Vector2 defaultValue = default)
        {
            return SaveValueSerializer.TryParseVector2(GetString(key), out var value) ? value : defaultValue;
        }

        public Vector3 GetVector3(string key, Vector3 defaultValue = default)
        {
            return SaveValueSerializer.TryParseVector3(GetString(key), out var value) ? value : defaultValue;
        }

        public Quaternion GetQuaternion(string key, Quaternion defaultValue = default)
        {
            return SaveValueSerializer.TryParseQuaternion(GetString(key), out var value) ? value : defaultValue;
        }

        public Color GetColor(string key, Color defaultValue = default)
        {
            return SaveValueSerializer.TryParseColor(GetString(key), out var value) ? value : defaultValue;
        }

        public T GetJson<T>(string key, T defaultValue = default)
        {
            return SaveValueSerializer.TryParseJson(GetString(key), out T value) ? value : defaultValue;
        }

        public void SetString(string key, string value)
        {
            _values[key] = value;
            Log($"Mock save string: {key} = {value}");
        }

        public void SetInt(string key, int value)
        {
            _values[key] = value.ToString(CultureInfo.InvariantCulture);
            Log($"Mock save int: {key} = {value}");
        }

        public void SetFloat(string key, float value)
        {
            _values[key] = value.ToString(CultureInfo.InvariantCulture);
            Log($"Mock save float: {key} = {value}");
        }

        public void SetBool(string key, bool value)
        {
            _values[key] = value.ToString();
            Log($"Mock save bool: {key} = {value}");
        }

        public void SetVector2(string key, Vector2 value)
        {
            SetString(key, SaveValueSerializer.ToString(value));
        }

        public void SetVector3(string key, Vector3 value)
        {
            SetString(key, SaveValueSerializer.ToString(value));
        }

        public void SetQuaternion(string key, Quaternion value)
        {
            SetString(key, SaveValueSerializer.ToString(value));
        }

        public void SetColor(string key, Color value)
        {
            SetString(key, SaveValueSerializer.ToString(value));
        }

        public void SetJson<T>(string key, T value)
        {
            SetString(key, SaveValueSerializer.ToJson(value));
        }

        public void DeleteKey(string key)
        {
            _values.Remove(key);
            Log($"Mock save key deleted: {key}");
        }

        public void Clear()
        {
            _values.Clear();
            Log("Mock save cleared.");
        }

        public Task SaveAsync()
        {
            Log("Mock save flushed.");
            return Task.CompletedTask;
        }

        private void Log(string message)
        {
            if (_logCalls)
            {
                Debug.Log(message);
            }
        }
    }
}
