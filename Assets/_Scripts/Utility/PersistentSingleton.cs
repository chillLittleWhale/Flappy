using UnityEngine;

namespace AjaxNguyen.Utility
{
    public class PersistentSingleton<T> : MonoBehaviour where T : Component
    {
        public bool UnparentOnAwake = true;

        public static bool HasInstance => instance != null;
        public static T Current => instance;

        protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindFirstObjectByType<T>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name + "AutoCreated";
                        instance = obj.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake() => InitializeSingleton();

        protected virtual void InitializeSingleton()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (UnparentOnAwake)
            {
                transform.SetParent(null);
            }

            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(transform.gameObject);
                enabled = true;
            }
            else
            {
                if (this != instance)
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}

// using UnityEngine;

// namespace AjaxNguyen.Core.Manager
// {
//     public class SingletonManager<T> : MonoBehaviour where T : MonoBehaviour
//     {
//         private static T instance;

//         public static T Instance
//         {
//             get
//             {
//                 if (instance == null)
//                 {
//                     instance = FindObjectOfType<T>();
//                     if (instance == null)
//                     {
//                         GameObject obj = new GameObject(typeof(T).Name);
//                         instance = obj.AddComponent<T>();
//                     }
//                 }
//                 return instance;
//             }
//         }

//         protected virtual void Awake()
//         {
//             if (instance == null)
//             {
//                 instance = this as T;
//                 DontDestroyOnLoad(gameObject);
//             }
//             else if (instance != this)
//             {
//                 Destroy(gameObject);
//             }
//         }
//     }

// }