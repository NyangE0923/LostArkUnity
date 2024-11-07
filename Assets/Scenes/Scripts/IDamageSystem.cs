using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageSystem
{
    public void TakeDamage(int damage);
}

public interface IHealingSystem
{
    public void RecoveryHealth(EntityStats stats);
}
