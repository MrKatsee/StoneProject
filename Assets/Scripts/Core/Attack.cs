using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Creature attacker;

    [SerializeField]
    protected float _damage;
    [SerializeField]
    public bool _defendable;
    [SerializeField]
    protected float _duration;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        gameObject.layer = LayerMask.NameToLayer("NonPhysicalAffectable");
    }

    public virtual void Init(float duration, float damage, bool defendable, Creature attacker)
    {
        _duration = duration;
        _damage = damage;
        _defendable = defendable;

        this.attacker = attacker;
    }

    public virtual void OnEnable()
    {
        StartCoroutine(AttackRoutine(_duration));
    }

    protected virtual IEnumerator AttackRoutine(float duration)
    {
        yield return new WaitForSeconds(duration);

        DestroyCallback();
    }

    public virtual void DestroyCallback()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    protected virtual void OnTriggerEnter2D(Collider2D c)
    {
        if (attacker.creatureTag == CreatureTag.MONSTER)
        {
            if (c.tag == "Player")
            {
                c.GetComponent<Player>().GetDamage(_damage);
                gameObject.SetActive(false);
            }
        }
        else if (attacker.creatureTag == CreatureTag.PLAYER)
        {
            if (c.tag == "Attack")
            {
                Attack attack = c.GetComponent<Attack>();

                if (attack.attacker.creatureTag != CreatureTag.PLAYER)
                {
                    if (attack._defendable)
                    {
                        attack.attacker.AddStun(1f);
                        attack.DestroyCallback();

                        CameraManager.Instance.StartCoroutine(CameraManager.Instance.Shake(0.05f, 0.1f));
                        CameraManager.Instance.HitEffect(attack.transform.position);
                    }
                }
            }
            else if (c.tag == "Monster")
            {

            }
        }

    }
}