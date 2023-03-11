
public interface IEnemy
{
    void Attack() { }

    void ReceiveDamage(int damage) { }

    void ChasePlayer() { }

    void DisplayHealth() { }

    void Die() { }
}