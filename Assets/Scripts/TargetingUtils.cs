using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetingUtils {
    /// <summary>
    /// Given the "self" transform and the enemy GameObject, returns the distance as a float.
    /// </summary>
    /// <param name="selfTransform"></param>
    /// <param name="enemy"></param>
    /// <returns>Distance between args as a float</returns>
    private static float GetDistanceToEnemy(Transform selfTransform, GameObject enemy) {
        return Vector3.Distance(enemy.transform.position, selfTransform.position);
    }

    /// <summary>
    /// Given the "self" transform and the max range, return the closest enemy GameObject.
    ///
    /// Returns null if no enemies in range.
    /// </summary>
    /// <param name="selfTransform"></param>
    /// <param name="range"></param>
    /// <returns>GameObject of closest enemy if in range. Null if no enemies in range.</returns>
    public static GameObject? GetClosestEnemyInRange(Transform selfTransform, float range) {
        List<GameObject> enemiesInRange = new List<GameObject>();
        List<GameObject> enemiesAlive = ReferenceManager.SpawnManagerComponent.GetAliveEnemies();
        enemiesInRange = (from enemy in enemiesAlive
                         where GetDistanceToEnemy(selfTransform, enemy) < range
                         select enemy)
                         .ToList();

        if (enemiesInRange.Count == 0) {
            return null;
        }

        return enemiesInRange
            .OrderBy(enemy => GetDistanceToEnemy(selfTransform, enemy))
            .FirstOrDefault();
    }

    /// <summary>
    /// Applies a bullet drop compensation on the X axis rotation - ie angle upwards.
    ///
    /// Compensation applied scales proportionally to distance to enemy. Scale is based on dist-to-enemy vs max-range.
    /// </summary>
    /// <param name="selfTransform"></param>
    /// <param name="enemy"></param>
    /// <param name="range"></param>
    /// <param name="maxCompensationAngle"></param>
    /// <returns></returns>
    public static Vector3 GetTargetPosWithCompensation(Transform selfTransform, GameObject enemy, float range,
        float maxCompensationAngle) {
        // "Compensation" as in bullet drop compensation
        float upwardCorrectionAngle = (maxCompensationAngle *
            (Vector3.Distance(enemy.transform.position, selfTransform.position)) / range);
        Vector3 angleOffset = new Vector3(0.0f, upwardCorrectionAngle, 0.0f);
        return enemy.transform.position - selfTransform.transform.position + angleOffset;
    }

    /// <summary>
    /// Applies explicit turret direction offset based on spell.
    ///
    /// TODO: This can probably be combined with GetTargetPosWithCompensation with a bit of refactoring.
    /// </summary>
    /// <param name="turretOffset"></param>
    /// <param name="targetPos"></param>
    /// <param name="currPos"></param>
    /// <param name="towerRange"></param>
    /// <returns></returns>
    public static Vector3 ApplySpellTowerTurretOffset(Vector3 turretOffset, Vector3 targetPos, Vector3 currPos,
        float towerRange) {
        float distToEnemy = Vector3.Distance(currPos, targetPos);
        Vector3 newTargetPos = targetPos + (distToEnemy / towerRange * turretOffset);
        return newTargetPos;
    }

}
