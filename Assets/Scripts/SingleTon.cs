using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    public const string gameObjectName = "Manager";

    private static T instance;
    public static T Instance
    {
        get
        {
            if (null == instance)
            {
                GameObject gameObject = GameObject.Find(gameObjectName);
                if (null == gameObject)
                {
                    gameObject = new GameObject();
                    gameObject.name = gameObjectName;
                    gameObject.AddComponent<T>();
                }
                instance = gameObject.GetOrAddComponent<T>();
            }
            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);  // 씬이 변경되어도 제거되지 않는 게임오브젝트로 설정
    }
}
