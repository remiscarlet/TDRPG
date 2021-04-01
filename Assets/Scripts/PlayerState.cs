using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState {
    private PlayerAbility[] hotbar;
    private int hotbarSize;
    private int selectedHotbarSlot;

    public PlayerState() {
        hotbar = new PlayerAbility[6];
        hotbar[0] = new MagicMissile(); 

        hotbarSize = hotbar.Length;
        selectedHotbarSlot = 0;
    }
}
