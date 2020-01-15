using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyTime : MonoBehaviour
{
    public static float deltaTime
    {
        get { return Time.deltaTime * timeScale; }
    }

    [SerializeField]
    public static float timeScale = 1f;

}
