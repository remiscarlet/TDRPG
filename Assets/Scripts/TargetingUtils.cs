using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetingUtils {
    private static float GetDistanceToEnemy(Transform selfTransform, GameObject enemy) {
        return Vector3.Distance(enemy.transform.position, selfTransform.position);
    }

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


    public static Vector3 GetTargetPosWithCompensation(Transform selfTransform, GameObject enemy, float range, float maxCompensationAngle) {
        // "Compensation" as in bullet drop compensation
        float upwardCorrectionAngle = (maxCompensationAngle * (Vector3.Distance(enemy.transform.position, selfTransform.position)) / range);
        Vector3 angleOffset = new Vector3(0.0f, upwardCorrectionAngle, 0.0f);
        return enemy.transform.position - selfTransform.transform.position + angleOffset;
    }

}
