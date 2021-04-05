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
    - Inventory class if more than just hotbar? Consumables? Throwables?
- Should `PlayerAbility.SpawnInstances` not be overridable and instead use a `projectileSpawnOffset`  in a non-overridable method instead?
- Better projectile animations, even in beta. Good practice.
    - Make fireball tower have splash effect. Randomly spawn mini fireballs around a circle outside body of hit target. Each mini fb flies outwards from center with random offset in xz direction assuming foward is y. Can put mini fireball as inactive child obj then use that obj to instantiate the splash effect copies. Instantiate with even distribution of x mini fbs then randomize in xz dir at unitval max then randomize forward at mbe 4-5 unitval for spawnlocs. Then randomize trajectory outward in y dir.
    - Magic missile tower shoots three missiles. One flying center w/ small offset and two going approx 4x 'small' offset out from center. Can also make mm projectiles shoot randomly in xz dir assuming forward is y until some percentage of distance travelled to target relative to init dist from instantiation. After reaching percentage, travel towards target center again but still add on random but lessening xz dir randomization.
    - Each of these "sub animations" should be in their own movements classes. Main projectile animation class loads them in `Start()`. Can program animations in order in projectile anim class with lengths of each anim. Maybe args to the anims to change dir and whatnot - How modular?
    - At this point should prob separate pc and tower projectile animations. Should also prob reorganize classes into folder structure.
- Why can structs not have unassigned fields? Why is assigning default null not being overriden by Ability setter upon instantiation?


## Design Thoughts
- Abilities are bought at the store and can individually be upgraded.
    - Think "Fireball1", "Fireball2", "Fireball3", etc. Each subsequent level should try to fundamentally change the spell in their effects both mechanically and visually. 
- Tower types are based on currently equipped ability. If have Fireball equipped, will create a Fireball Tower.
    - Targetting should be configurable (Closest, Farthest, Strongest, Randomized)
    - Tower type is determined by ability, but towers separately have upgradeable levels like "damage", "range", "speed".
    - However, if ability is upgraded (Say FB1 -> FB2) then towers all also get upgraded at same time.
    - Tower Type and Tower Levels should try to avoid overlapping functionality minus damage (Tower damage should be more "bonus" damage?)
- Ability to use consumables? What kind of consumables?
- Abilities should be split into "spells" and "abilities". Towers should be called "Spelltowers". Towers can take on spells. Abilities are powerful in their own way but can only be used by pc.
- Combo towers?
    - Eg, with fireball and mm towers combo'd, can shoot a small ring of super mini fbs surrounding each mm moving in similar fashion to normal mm. Decrease scale of mms but increase # of them - Eg 5x 'forward', 10x 'off to the side' mms. Can spawn the entire "salvo" some dist in y dir while setting xz to center of all towers in combo. 
    - Combos would be explicitly selected and each tower can only be in a single combo. Pay upfront - prob very expensive; maybe use new 'currency'? For now, can display combod towers with yellow rings with lines going perp outwards to connect to other towers. No collision on the yellow line/rings.
- Consider using ScriptableObjects for shared data for projectiles and other shared const data?
- Have enemies target towers and player?
    - Could also have healing/defense oriented towers then.
    - Makes gameplay a bit more interesting rather than shooting defenseless enemies

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
