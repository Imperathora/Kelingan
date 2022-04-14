namespace Framework.Character
{
    public interface IKillable
    {
        void Die();
    }

    public interface IDamageable<T>
    {
        void TakeDamage(T _damageTaken, out T _damageDealt);
    }

    public interface IDamageableSimple<T>
    {
        void TakeDamage(T _damageTaken);
    }
}