using System.Threading.Tasks;
using GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.Runtime.Save
{
    public sealed class PlayerPrefsSaveService : ISaveService
    {
        private const char KeySeparator = '\n';

        private readonly string _keyPrefix;
        private readonly string _keyIndexKey;
        private GameServiceStatus _status = GameServiceStatus.NotInitialized;

        public PlayerPrefsSaveService(string keyPrefix)
        {
            _keyPrefix = string.IsNullOrWhiteSpace(keyPrefix) ? string.Empty : keyPrefix;
            _keyIndexKey = $"{_keyPrefix}__keys";
        }

        public string ServiceId => "save.player-prefs";
        public GameServiceStatus Status => _status;
        public bool IsReady => _status == GameServiceStatus.Ready;

        public Task InitializeAsync(GameServiceContext context)
        {
            _status = GameServiceStatus.Ready;
            return Task.CompletedTask;
        }

        public bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(GetFullKey(key));
        }

        public string GetString(string key, string defaultValue = "")
        {
            return PlayerPrefs.GetString(GetFullKey(key), defaultValue);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return PlayerPrefs.GetInt(GetFullKey(key), defaultValue);
        }

        public float GetFloat(string key, float defaultValue = 0f)
        {
            return PlayerPrefs.GetFloat(GetFullKey(key), defaultValue);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return PlayerPrefs.GetInt(GetFullKey(key), defaultValue ? 1 : 0) == 1;
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
            var fullKey = GetFullKey(key);
            PlayerPrefs.SetString(fullKey, value);
            TrackKey(fullKey);
        }

        public void SetInt(string key, int value)
        {
            var fullKey = GetFullKey(key);
            PlayerPrefs.SetInt(fullKey, value);
            TrackKey(fullKey);
        }

        public void SetFloat(string key, float value)
        {
            var fullKey = GetFullKey(key);
            PlayerPrefs.SetFloat(fullKey, value);
            TrackKey(fullKey);
        }

        public void SetBool(string key, bool value)
        {
            var fullKey = GetFullKey(key);
            PlayerPrefs.SetInt(fullKey, value ? 1 : 0);
            TrackKey(fullKey);
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
            var fullKey = GetFullKey(key);
            PlayerPrefs.DeleteKey(fullKey);
            UntrackKey(fullKey);
        }

        public void Clear()
        {
            var keys = GetTrackedKeys();
            for (var i = 0; i < keys.Length; i++)
            {
                PlayerPrefs.DeleteKey(keys[i]);
            }

            PlayerPrefs.DeleteKey(_keyIndexKey);
        }

        public Task SaveAsync()
        {
            PlayerPrefs.Save();
            return Task.CompletedTask;
        }

        private string GetFullKey(string key)
        {
            return $"{_keyPrefix}{key}";
        }

        private void TrackKey(string fullKey)
        {
            var keys = GetTrackedKeys();
            for (var i = 0; i < keys.Length; i++)
            {
                if (keys[i] == fullKey)
                {
                    return;
                }
            }

            var value = string.IsNullOrEmpty(PlayerPrefs.GetString(_keyIndexKey, string.Empty))
                ? fullKey
                : $"{PlayerPrefs.GetString(_keyIndexKey)}{KeySeparator}{fullKey}";

            PlayerPrefs.SetString(_keyIndexKey, value);
        }

        private void UntrackKey(string fullKey)
        {
            var keys = GetTrackedKeys();
            var value = string.Empty;

            for (var i = 0; i < keys.Length; i++)
            {
                if (keys[i] == fullKey)
                {
                    continue;
                }

                value = string.IsNullOrEmpty(value)
                    ? keys[i]
                    : $"{value}{KeySeparator}{keys[i]}";
            }

            if (string.IsNullOrEmpty(value))
            {
                PlayerPrefs.DeleteKey(_keyIndexKey);
                return;
            }

            PlayerPrefs.SetString(_keyIndexKey, value);
        }

        private string[] GetTrackedKeys()
        {
            var value = PlayerPrefs.GetString(_keyIndexKey, string.Empty);
            return string.IsNullOrEmpty(value)
                ? new string[0]
                : value.Split(KeySeparator);
        }
    }
}
