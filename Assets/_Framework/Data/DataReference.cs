using System;

namespace Framework.Data
{
    public abstract class DataReference<T, U> where U : DataContainer<T, U>
    {
        public bool UseConstant = true;
        public T ConstantValue;
        public U Variable;

        public DataReference() { }

        public DataReference(T value)
        {
            UseConstant = true;
            ConstantValue = value;
        }

        public T Value
        {
            get { return UseConstant ? ConstantValue : Variable.Value; }
        }

        public static implicit operator T(DataReference<T, U> reference)
        {
            return reference.Value;
        }
    }
}