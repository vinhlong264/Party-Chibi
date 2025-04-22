using System.Collections.Generic;
using System.Diagnostics;

public static class EventChanel
{
    private static Dictionary<KeyEvent, HashSet<System.Action<object[]>>> observer = new Dictionary<KeyEvent, HashSet<System.Action<object[]>>>();

    public static void Register(KeyEvent key, System.Action<object[]> callBack)
    {
        if (observer.ContainsKey(key))
        {
            UnityEngine.Debug.Log("Đăng kí thêm sự kiện vào key sự kiện");
            observer[key].Add(callBack);
        }
        else
        {
            UnityEngine.Debug.Log("Đăng kí sự kiện");
            observer.Add(key, new HashSet<System.Action<object[]>>());
            observer[key].Add(callBack);
        }
    }

    public static void UnRegister(KeyEvent key, System.Action<object[]> callBack)
    {
        if (!observer.ContainsKey(key)) return;

        if (observer[key].Count > 0)
        {
            observer[key].Remove(callBack);
        }
        else
        {
            observer.Remove(key);
        }
    }

    public static void NotifyEvent(KeyEvent key, object[] value = null)
    {
        if (observer.ContainsKey(key))
        {
            foreach (var o in observer[key])
            {
                o?.Invoke(value);
            }
        }
    }
}

public enum KeyEvent
{
    HealthBar,
    Status,
    System
}