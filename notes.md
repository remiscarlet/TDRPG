# Notes

## Todos
- Better wave spawn logic
- Menu?
- Score/Money/Point system
    - Upgradeable Towers
    - Upgradeable/purchaseable items/skills? No classes?
        - Start game with no shot ability, buy first ability/weapon. How many items/abilities? 
- Potential classes:
    - Clicker/UI Input Delay class for handling "delay repeating keystroke", "pause for .5 seconds on first press, then repeat keystroke every .1 seconds thereafter", etc
    - Items class as a parent of Purchaseable/PlayerAbility? Are purchaseables _all_ items?
    - Inventory class if more than just hotbar? Consumables? Throwables?
- Better projectile animations, even in beta. Good practice.
    - Magic missile tower shoots three missiles. One flying center w/ small offset and two going approx 4x 'small' offset out from center. Can also make mm projectiles shoot randomly in xz dir assuming forward is y until some percentage of distance travelled to target relative to init dist from instantiation. After reaching percentage, travel towards target center again but still add on random but lessening xz dir randomization.
    - Each of these "sub animations" should be in their own movements classes. Main projectile animation class loads them in `Start()`. Can program animations in order in projectile anim class with lengths of each anim. Maybe args to the anims to change dir and whatnot - How modular?
    - At this point should prob separate pc and tower projectile animations. Should also prob reorganize classes into folder structure.
- Why can structs not have unassigned fields? Why is assigning default null not being overriden by Ability setter upon instantiation?
- Projectile rotation should face velocity vector direction
- Projectile animations should all have "scaleToDist" arg. Particularly impulsefwd at start for MM animations make less sense the closer the enemy due to the constant impulse force at start causing projectiles against close enemies to overshoot/miss
- `ComboTower` should probably get upgraded into a class and not a struct `ComboManager`? So many managers...
- Code organization refactor. Controller, Managers, etc. More/nested folders?


## Design Thoughts
- Abilities are bought at the store and can individually be upgraded.
    - Think "Fireball1", "Fireball2", "Fireball3", etc. Each subsequent level should try to fundamentally change the spell in their effects both mechanically and visually. 
- Tower types are based on currently equipped ability. If have Fireball equipped, will create a Fireball Tower.
    - Targetting should be configurable (Closest, Farthest, Strongest, Randomized)
    - Tower type is determined by ability, but towers separately have upgradeable levels like "damage", "range", "speed".
    - ~~However, if ability is upgraded (Say FB1 -> FB2) then towers all also get upgraded at same time.~~
        - Actually, towers always stay at level 1 ability damage. Combos empower towers.
    - Tower Type and Tower Levels should try to avoid overlapping functionality minus damage (Tower damage should be more "bonus" damage?)
- Ability to use consumables? What kind of consumables?
- Abilities should be split into "spells" and "abilities". Towers should be called "Spelltowers". Towers can take on spells. Abilities are powerful in their own way but can only be used by pc.
    - Some examples of player abilities:
        - Taunt: Make enemies target you and also chase you? Could be used as a 
- Combo towers?
    - Eg, with fireball and mm towers combo'd, can shoot a small ring of super mini fbs surrounding each mm moving in similar fashion to normal mm. Decrease scale of mms but increase # of them - Eg 5x 'forward', 10x 'off to the side' mms. Can spawn the entire "salvo" some dist in y dir while setting xz to center of all towers in combo. 
    - Combos would be explicitly selected and each tower can only be in a single combo. Pay upfront - prob very expensive; maybe use new 'currency'? For now, can display combod towers with yellow rings with lines going perp outwards to connect to other towers. No collision on the yellow line/rings.
- Consider using ScriptableObjects for shared data for projectiles and other shared const data?
- Have enemies target towers and player?
    - Enemy AI can be implemented as a state machine with transitions dictated by environment
    - Could also have healing/defense oriented towers then.
    - Makes gameplay a bit more interesting rather than shooting defenseless enemies
    - Enemies can prioritize three things: Get to goal, target player/turrets, buff other enemies.
        - Enemies can have resistances to elements or tower/player.
        - Buffing enemies probably only targettable by player?
- Player placeable obstacles a la DW2. Will affect enemy AI as well.
- Display information about spelltower being built.
    - Damage stats
    - Rate of fire
    - Range overlay/ground indicator
    - etc.
- Combo towers addable? Ie, can a pre-existing combo have towers added to it? Could create an option for more strategic gameplay. What about removing towers? This also affects code design.


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
- Shocked: Probability of becoming immobilized temporarily
- Chilled: Slowed, DoT
- Poisoned: DoT

## Towers:
| Spell Type    | Base Damage | Rate of Fire | Splash Damage | Total Damage        | Projectiles |
|---------------|-------------|--------------|---------------|---------------------|-------------|
| Magic Missile | 50          | 300/min      | N/A           | 100 dps             | 1x proj. impulsed upwards then switch to homing on closest target |
| Fireball      | 150         | 60/min       | 25 x 10       | 150 dps + 150 sp./s | 1x large blue proj. lobbed toward target with 25x splash objs spawned on contact with enemy |


## Combos:
| Magic Missile | Fireball | | Combo            | | Base Damage | Rate of Fire | Splash           | Total Damage        | Projectiles |
|---------------|----------|-|------------------|-|-------------|--------------|------------------|---------------------|-------------|
| 1x            | 1x       | | Fiery Missiles   | | 4 x 125     | 60/min       | N/A              | 500 dps             | 4x red missiles shoot upward in spiral then home in on target |
| 1x            | 2x       | | Homing Fireballs | | 2 x 300     | 60/min       | 29 x 12 per proj | 600 dps + 480 sp./s | 2x fireballs shoot in v formation backwards at a high angle then home in on target. Fireball splash on contact |
| 2x            | 1x       | |                  | |             |              |                  |                     | |


## Enemies:
- How to differentiate? By achetype? By elemental affinity? Size? Speed? 
    - "Default" bois
    - "Fast"
    - "Slow and tanky"
    - "Splits into multiple smaller/weaker enemies"
    - "Immune/resistant to element XYZ"
    - "MOAB-esque enemies"
- Resistances to specific types of towers:
    - MM: Non-elemental "relatively weak but works on everything"
    - FB: Obviously any kind of anti fire enemies. Ice based? Ground/poison based?
    - 

## Thoughts
- How many Scenes? Should tower building be separate scene?
- :thinking: Could log stats about splash projectiles to make data for analyzing true splash dps...