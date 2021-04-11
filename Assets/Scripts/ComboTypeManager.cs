using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class ComboTypeManager : MonoBehaviour {
    public ComboTypeManager() {

    }

    /// <summary>
    /// TODO: This function needs a complete overhaul once I have more than 1 combo implemented.
    /// </summary>
    /// <param name="towerControllers"></param>
    /// <returns>Empty string if invalid combo. String of fully qualified combo class name to be used with reflections if valid.</returns>
    public ComboType DetermineComboType(List<TowerController> towerControllers) {
        List<Spell> spellsInCombo = (
            from towerController in towerControllers
            select towerController.TowerSpell
            ).ToList();

        string comboClassName = "";
        if (towerControllers.Count == 2) {
            // Gross
            if (spellsInCombo.OfType<Spells.Fireball>().Any() &&
                spellsInCombo.OfType<Spells.MagicMissile>().Any()) {
                comboClassName = "mm1fb1";
            }
        }

        if (comboClassName != "") {
            print($"ComboTypes.{comboClassName}");
            Type type = Type.GetType($"ComboTypes.{comboClassName}");
            print(type);
            return (ComboType) Activator.CreateInstance(type);
        } else {
            return null;
        }
    }
}
