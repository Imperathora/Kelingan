using UnityEngine;

namespace Framework.Data
{
    [CreateAssetMenu(fileName = "Vector2", menuName = "Data/Vector2")]
    public class Vector2Container : ArithmeticDataContainer<Vector2, Vector2Container>
    {
        public override void Add(Vector2 _value)
        {
            Value += _value;
        }

        public override void Add(Vector2Container _container)
        {
            Value += _container.Value;
        }
    }
}