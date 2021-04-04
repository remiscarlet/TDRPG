using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers {
    public static int Default = 0;
    public static LayerMask DefaultMask = Default; // Implicit casting

    public static int Projectiles = 3;
    public static LayerMask ProjectilesMask = Projectiles;

    public static LayerMask IgnoreProjectilesMask = ~ProjectilesMask;
}
