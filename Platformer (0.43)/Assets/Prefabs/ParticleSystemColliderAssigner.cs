using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemColliderAssigner : MonoBehaviour
{
    [SerializeField] string targetTag;
    ParticleSystem ps;

    void OnEnable()
    {
        Setup();
    }

    void Setup()
    {
        ps = GetComponent<ParticleSystem>();
        AssignColliders();
    }

    void AssignColliders()
    {
        var trigger_module = ps.trigger;

        GameObject[] objs = GameObject.FindGameObjectsWithTag(targetTag);

        foreach(GameObject obj in objs)
        {
            Collider2D[] cols = obj.GetComponents<Collider2D>();
            foreach(Collider2D col in cols)
            {
                trigger_module.AddCollider(col);
            }
        }
    }
}
