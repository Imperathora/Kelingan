using UnityEngine;

namespace Framework.Data
{
    [CreateAssetMenu(fileName = "Vector3", menuName = "Data/Vector3")]
    public class Vector3Container : ArithmeticDataContainer<Vector3, Vector3Container>
    {
        public override void Add(Vector3 _value)
        {
            Value += _value;
        }

        public override void Add(Vector3Container _container)
        {
            Value += _container.Value;
        }
    }
}