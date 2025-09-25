using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;
    
    private LivingEntity target;

    public void Init(LivingEntity target)
    {
        this.target = target;
    }

    void Update()
    {
        if (target == null) return;

        
        Vector3 dir = (target.transform.position - transform.position).normalized;
        transform.position += dir * speed * Time.deltaTime;

        
        if (Vector3.Distance(transform.position, target.transform   .position) < 0.1f)
        {
            //target.GetComponent<Enemy>()?.OnDamage(damage);
            Destroy(gameObject);
        }
    }
}
