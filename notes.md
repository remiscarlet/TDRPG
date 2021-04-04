# Notes

## Todos
- Better wave spawn logic
- Buildable towers
- Menu?
- Score/Money/Point system
    - Upgradeable Towers
    - Upgradeable/purchaseable items/skills? No classes?
        - Start game with no shot ability, buy first ability/weapon. How many items/abilities? 
- Potential classes:
    - Clicker/UI Input Delay class for handling "delay repeating keystroke", "pause for .5 seconds on first press, then repeat keystroke every .1 seconds thereafter", etc
    - Items class as a parent of Purchaseable/PlayerAbility? Are purchaseables _all_ items?
    - Hotbars class
    - Inventory class if more than just hotbar? Consumables? Throwables?

## Design Thoughts
- Abilities are bought at the store and can individually be upgraded.
    - Think "Fireball1", "Fireball2", "Fireball3", etc. Each subsequent level should try to fundamentally change the spell in their effects both mechanically and visually. 
- Tower types are based on currently equipped ability. If have Fireball equipped, will create a Fireball Tower.
    - Targetting should be configurable (Closest, Farthest, Strongest, Randomized)
    - Tower type is determined by ability, but towers separately have upgradeable levels like "damage", "range", "speed".
    - However, if ability is upgraded (Say FB1 -> FB2) then towers all also get upgraded at same time.
    - Tower Type and Tower Levels should try to avoid overlapping functionality minus damage (Tower damage should be more "bonus" damage?)
- Ability to use consumables? What kind of consumables?

## Elements:
- Fire
- Lightning
- Ice
- Poison

## Abilities:
- Magic Missile: "Basic" attack/no elemental affinity
- Fireball: Slow AoE fire-affinity
- Lightning: Medium single target electric-affinity, chance to shock.

## Status/Effects:
- Statuses have "levels" in terms of severity. Eg, Burning1, Burning2, Shocked3, etc. Max level? Match to ability level?
- Burning: DoT
- Shocked: Probabilty of becoming immobilized temporarily
- Chilled: Slowed, DoT
- Poisoned: DoT

## Thoughts
- How many Scenes? Should tower building be separate scene?

## Ideas
- Three basic towers:
    - Single shot, weak, fast (Tower1)
    - Single shot, strong, pierce, medium
    - Single shot, medium, aoe, slow, DoT
- Three classes:
    - Warrior - Melee/Med/Cleave
    - Ranger - Range/Slow/Pierce
    - Wizard - Range/Fast/Single Target
