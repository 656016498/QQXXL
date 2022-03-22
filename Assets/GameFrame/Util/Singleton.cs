using System;
using System.Collections;
using System.Diagnostics;

namespace Util
{
    public class Singleton<T> where T : new()
    {
        protected static T _instance = default(T);
        private static Object _objLock = new Object();
  

        protected Singleton()
        {
            Debug.Assert(_instance == null);
            this.Init();
        }

        protected virtual void Init()
        {
           
        }

        public static void Depose()
        {
            _instance = default(T);
        }

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                        {
                            _instance = new T();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}