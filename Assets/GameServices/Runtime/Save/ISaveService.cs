using System.Threading.Tasks;
using GameServices.Runtime.Core;
using UnityEngine;

namespace GameServices.Runtime.Save
{
    public interface ISaveService : IGameService
    {
        bool HasKey(string key);
        string GetString(string key, string defaultValue = "");
        int GetInt(string key, int defaultValue = 0);
        float GetFloat(string key, float defaultValue = 0f);
        bool GetBool(string key, bool defaultValue = false);
        Vector2 GetVector2(string key, Vector2 defaultValue = default);
        Vector3 GetVector3(string key, Vector3 defaultValue = default);
        Quaternion GetQuaternion(string key, Quaternion defaultValue = default);
        Color GetColor(string key, Color defaultValue = default);
        T GetJson<T>(string key, T defaultValue = default);

        void SetString(string key, string value);
        void SetInt(string key, int value);
        void SetFloat(string key, float value);
        void SetBool(string key, bool value);
        void SetVector2(string key, Vector2 value);
        void SetVector3(string key, Vector3 value);
        void SetQuaternion(string key, Quaternion value);
        void SetColor(string key, Color value);
        void SetJson<T>(string key, T value);
        void DeleteKey(string key);
        void Clear();
        Task SaveAsync();
    }
}
