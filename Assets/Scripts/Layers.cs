using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layers {
    public static int Default = 0;
    public static LayerMask DefaultMask = Default; // Implicit casting

    public static int Enemy = 16;
    public static LayerMask EnemyMask = Enemy;
    public static int Friendly = 17;
    public static LayerMask FriendlyMask = Friendly;

    public static int EnemyProjectiles = 18;
    public static LayerMask EnemyProjectilesMask = EnemyProjectiles;
    public static int FriendlyProjectiles = 19;
    public static LayerMask FriendlyProjectilesMask = FriendlyProjectiles;
}
