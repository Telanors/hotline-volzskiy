using System;
using UnityEngine;
public class EnemyBodyPart : MonoBehaviour, IDamageable, IStunable
{
    public IDamageable parentDamageable;
    public IStunable parentStunable;
    private Rigidbody rigitBody;

    private void Awake()
    {
        rigitBody = GetComponent<Rigidbody>();
        SwitchKinematic(true);
    }
    public void SwitchKinematic(bool value) => rigitBody.isKinematic = value;
    public void Damage(float damage) => parentDamageable.Damage(damage);
    public void Stun() => parentStunable.Stun();
}
