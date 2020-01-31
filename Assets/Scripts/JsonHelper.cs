using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper {
    public static List<T> FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Lists;
    }

    public static GameStatistics StatisticsFromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.gameStatistics;
    }

    public static string ToJson<T>(List<T> array, GameStatistics gs)
    {
        Wrapper<T> wrapper      = new Wrapper<T>();
        wrapper.Lists           = array;
        wrapper.gameStatistics  = gs;

        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(List<T> array, GameStatistics gs, bool prettyPrint)
    {
        Wrapper<T> wrapper      = new Wrapper<T>();
        wrapper.Lists           = array;
        wrapper.gameStatistics  = gs;

        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T> {
        public List<T> Lists;
        public GameStatistics gameStatistics;
    }
}