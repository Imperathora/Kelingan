using UnityEngine;

namespace Framework.Data
{
    [CreateAssetMenu(fileName = "Float", menuName = "Data/Float")]
    public class FloatContainer : ArithmeticDataContainer<float, FloatContainer>
    {
        public override void Add(float _value)
        {
            Value += _value;
        }

        public override void Add(FloatContainer _container)
        {
            Value += _container.Value;
        }
    }
}