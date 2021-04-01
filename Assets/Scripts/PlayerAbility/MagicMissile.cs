using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissile : PlayerAbility {
    public float damagePerHit = 25.0f;
    public float GetAbilityDamage() {
        return damagePerHit;
    }
}
