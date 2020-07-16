using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonGeneric<T> where T : SingletonGeneric<T>
{
    private static T genericInstance;
    public static T GenericInstance { get { return genericInstance; } }

    public SingletonGeneric()
    {
        if (genericInstance == null)
        {
            genericInstance = (T)this;
        }
        else
        {
            Debug.LogWarning("Someone tring to create a duplicate of Singleton!");
        }
    }


}
