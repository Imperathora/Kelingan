using UnityEngine;

namespace Framework.Data
{
    public abstract class DataContainer <T, U> : ScriptableObject where U : DataContainer<T, U>
    {
        [Multiline]
        public string DeveloperDescription = "";
        public T Value;

        public void SetValue(T value)
        {
            Value = value;
        }

        public void SetValue(U value)
        {
            Value = value.Value;
        }
    }

    public abstract class ArithmeticDataContainer <T, U> : DataContainer<T, U> where U : ArithmeticDataContainer<T, U>
    {
        public abstract void Add(T _value);

        public abstract void Add(U _value);
    }
}