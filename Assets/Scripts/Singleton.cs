using System;

public abstract class Singleton<T>
{
    protected static object instance = null;

    protected static readonly object padlock = new object();

    public static T Instance
    {
        get
        {
            object obj = Singleton<T>.padlock;
            T result;
            lock (obj)
            {
                if (Singleton<T>.instance == null)
                {
                    Singleton<T>.instance = Activator.CreateInstance(typeof(T));
                }
                result = (T)((object)Singleton<T>.instance);
            }
            return result;
        }
    }

    public Singleton()
    {
    }
}
