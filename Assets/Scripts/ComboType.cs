using System;
using UnityEngine;

/// <summary>
/// <c>Spell</c> Isn't right :-/ Combos aren't <c>Purchaseable</c>
/// TODO: Fix class inheritance hierarchy
/// </summary>
public class ComboType : Spell {
    public ComboType(GameObject prefab) : base(prefab) { }
    public ComboType() : base(null) { }

    private string comboName;
    public string ComboName {
        get => comboName;
    }

    private string comboDesc;
    public string ComboDesc {
        get => comboDesc;
    }
}
