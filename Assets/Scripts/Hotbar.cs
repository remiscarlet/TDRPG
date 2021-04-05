using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class Hotbar {
    private int hotbarSize = 4;

    private bool hasHotbarUpdated = true;
    private int selectedSlot = 0;

    private Structs.HotbarSlot[] hotbar;

    public Hotbar() {
        Debug.Log("INITIALIZING HOTBAR");
        Debug.Log($"hotbarSize is {hotbarSize}");
        hotbar = new Structs.HotbarSlot[hotbarSize];
        for (int slot = 0; slot < hotbarSize; slot++) {
            // 0-index vs 1-index
            GameObject go = GameObject.Find($"Canvas/HotbarPanel/Slot{slot + 1}");
            hotbar[slot] = new Structs.HotbarSlot(slot, go);
            Debug.Log($"Inserting hotbar slot object: {hotbar[slot]}");
        }
    }

    public Structs.HotbarSlot GetCurrentSelectedSlot() {
        return hotbar[selectedSlot];
    }

    private readonly Color selectedColor = new Color(0.0f, 0.0f, 0.0f, 255.0f);
    private readonly Color unselectedColor = new Color(255.0f, 255.0f, 255.0f, 255.0f);
    public void DrawIfHotbarIsUpdated() {
        if (hasHotbarUpdated) {
            // Draw hotbar items
            for (int i = 0; i < hotbarSize; i++) {
                Structs.HotbarSlot hotbarSlot = hotbar[i];
                Spell ability = hotbarSlot.Ability;
                if (ability == null) {
                    continue;
                }

                hotbarSlot.UIRawImageComponent.texture = ability.IconTex;
            }

            // Draw selected hotbar border
            foreach (Structs.HotbarSlot slot in hotbar) {
                slot.UIImageComponent.color = unselectedColor;
            }

            GetCurrentSelectedSlot().UIImageComponent.color = selectedColor;
            hasHotbarUpdated = false;
        }
    }

    public void UpdateSelectedSlot() {
        int prevSlot = selectedSlot;
        if (Input.GetKey(KeyCode.Alpha1)) {
            selectedSlot = 0;
        } else if (Input.GetKey(KeyCode.Alpha2)) {
            selectedSlot = 1;
        } else if (Input.GetKey(KeyCode.Alpha3)) {
            selectedSlot = 2;
        } else if (Input.GetKey(KeyCode.Alpha4)) {
            selectedSlot = 3;
        }

        if (prevSlot != selectedSlot) {
            hasHotbarUpdated = true;
        }
    }

    public bool AddNewAbility(Spell ability) {
        int slotToInsertInto = GetLowestUnoccupiedHotbarSlot();
        if (slotToInsertInto == -1) {
            // Hotbar full. Failed to insert.
            return false;
        }

        hasHotbarUpdated = true;
        hotbar[slotToInsertInto].Ability = ability;
        return true;
    }

    private int GetLowestUnoccupiedHotbarSlot() {
        for (int i = 0; i < hotbarSize; i++) {
            if (!hotbar[i].IsOccupied) {
                return i;
            }
        }
        return -1;
    }
}
