using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonoBehaviour<T> where T : MonoBehaviour
{
    private static T instance;

    public static void Clear()
    {
        instance = null;
    }
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<T>();
                //Debug.LogError(typeof(T).Name + " == null");
            }

            return instance;
        }
    }

    void OnDestroy()
    {
        instance = null;
    }
}