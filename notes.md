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


## Random Design Thoughts
- Abilities are bought at the store and can individually be upgraded.
    - Think "Fireball1", "Fireball2", "Fireball3", etc. Each subsequent level should try to fundamentally change the spell in their effects both mechanically and visually. 

- Tower types are based on currently equipped ability. If have Fireball equipped, will create a Fireball Tower.
    - Targetting should be configurable (Closest, Farthest, Strongest, Randomized)
    - Tower type is determined by ability, but towers separately have upgradeable levels like "damage", "range", "speed".
    - ~~However, if ability is upgraded (Say FB1 -> FB2) then towers all also get upgraded at same time.~~
        - Actually, towers always stay at level 1 ability damage. Combos empower towers.
    - Tower Type and Tower Levels should try to avoid overlapping functionality minus damage (Tower damage should be more "bonus" damage?)

- Consumables? What kinds?

- Abilities should be split into "spells" and "abilities". Towers should be called "Spelltowers". Towers can take on spells. Abilities are powerful in their own way but can only be used by pc.
    - Some examples of player abilities:
        - Taunt: Make enemies target you and also chase you?

- Consider using ScriptableObjects for shared data for projectiles and other shared const data?

- Player placeable obstacles ala DW2. Will affect enemy AI as well.

- CONSIDER: Towers are built pre-start of game. Waves are split into two halves (+-? depending on level?) Can build/upgrade towers at "intermissions" as well. Can also buy player item/spells. However, player can still upgrade own spells/buy/etc mid-wave. Combos still work with towers.
    - Simplifies a lot of UX stuff

- With enemy loot/drops and backstory, could make a tech tree for unlocking new spells/enhancements.

### Theme
- Dwarf themed, so defending mountain castle (Utilizing height elevation) -> placing towers around? Too similar to DD?
    - Could be like you're a "dwarf hero" (dwarf wizard?) that starts by protecting/chasing out enemies from the castle/inner keep area. Progression of areas can be retaking certain areas of the city etc.
        - Areas:
        - The Keep
        - The 
    - Could make maps with Inkarnate... 
    - Retaking each area unlocks new abilities/towers?
        - Would imply a freeplay mode would be good too

### Story/Lore
- NAMES:
    - CONTINENT: ???
    - DWARVENKINGDON: ???
    - BADGUYS: ???
    - MAGICKENERGY: ???
    - GOODPLANE: ???
    - EVILPLANE: ???
    - KINGNAME: King Khurfim III
    - MOUNTKEEP: Mount Dhorluhm's Keep
    - DWARFHERO: ??? Playerchosen name?

- Story overall takes place in "the 1800's" of high fantasy if magic is equivalent to technology in terms of technical progression. That is to say, the "world" is just discovering magic/starting to experiment with magic. This game will be focused specifically on dwarves.
    - More generally on the world, given this still takes place in a traditionally "high fantasy" setting, there are naturally a ton of monsters etc. While the "civilized" races haven't quite figured out <MAGICKENERGY> yet, evil from the plane of <EVILPLANE> have footholds into this <GOODPLANE>. The inhabitants of <EVILPLANE>, while much less intelligent, have innate connections to <MAGICENERGY> which allow them to use <MAGICKENERGY>. This crossing of planes only happened recently (How many years?) which is when the wars began.
    - Due to not having figured out magic yet, the high fantasy races are effectively technologically equivalent to middle ages irl humans, ie weak af.
    - There are also beasts native to this world, but generally do not have any magic. Sea monsters exist.
        - Because of large sea monsters and no magic, the various races spread across the world on different landmasses have not yet explored/discovered each other except in very faint whispers of rumors from daring adventurers that claim to have met other races across the seas.
        - Thus, dwarves in this world are isolated and effectively only know about themselves.

- The continent of <CONTINENT> has been largely overrun by the agents of <BADGUYS>. Inhabited by the kingdom of <DWARVENKINGDON>, <CONTINENT> is on the losing end of a war. One of the last remnants of the <DWARVENKINGDON>, lead by <KINGNAME>, are barricaded within their mountain keep of <MOUNTKEEP>.
    - While the <DWARVENKINGDOM>s have been holding on for a while, the <BADGUYS> have just broken through some of the last defensive walls of <MOUNTKEEP> and are fast encroaching on inner parts of the castle keep.

- <KINGNAME> has a dwarf son by the name of <DWARFHERO>. While a competent fighter, <DWARFHERO> always preferred his scholarly pursuits given the choice. Starting his studies as many dwarves do in the fields of stonemasonry and architecture, <DWARFHERO> eventually fell into the niche experimental field of <MAGICKENERGY> Studies. The magick of <MAGICKENERGY> was only recently "discovered" and was more speculation and theory than anything practical like building strongholds in mountains or constructing city buildings. 
    - While the study was still in its infancy, having been studying and experimenting for most of his adult life <DWARFHERO> finally made a breakthrough discovery in recent years that had allowed him to successfully create the first "protospells". This is still in a largely experimental state and knowledge has not been widely shared of these feats.

- While <KINGNAME> had been asking <DWARFHERO> if his new discoveries might aid in the fight against <BADGUYS>, <DWARFHERO> had long said it was too experimental and risky, especially given there were so few dwarves including himself that knew how to harness these powers. 
    - However, recently <DWARFHERO> has made strides in stabilizing the powers to create proper "spells".
    - Informing <KINGNAME>, the two agree that <DWARFHERO> will prepare to attempt to utilize these new discoveries to push back the forces of <BADGUYS> 

### Levels
- Try to incorporate new mechanics/ideas per district to distinguish them
- Game will be split into taking back "districts", each of which have multiple "areas" (levels).
- Districts:
    - The Keep
    - Tunnels
    - Inner City
    - Outer City
    - City Outskirts
- Early districts have fewer levels, later districts have more. Ie, more area in each district as it radiates out from center. Also allows fewer "tutorial" levels and more lategame levels.

### Towers/Abilities
- Combo towers?
    - Eg, with fireball and mm towers combo'd, can shoot a small ring of super mini fbs surrounding each mm moving in similar fashion to normal mm. Decrease scale of mms but increase # of them - Eg 5x 'forward', 10x 'off to the side' mms. Can spawn the entire "salvo" some dist in y dir while setting xz to center of all towers in combo. 
    - Combos would be explicitly selected and each tower can only be in a single combo. Pay upfront - prob very expensive; maybe use new 'currency'? For now, can display combod towers with yellow rings with lines going perp outwards to connect to other towers. No collision on the yellow line/rings.

- Combo towers addable? Ie, can a pre-existing combo have towers added to it? Could create an option for more strategic gameplay. What about removing towers? This also affects code design.

### Enemies
- Have enemies target towers and player?
    - Enemy AI can be implemented as a state machine with transitions dictated by environment
    - Could also have healing/defense oriented towers then.
    - Makes gameplay a bit more interesting rather than shooting defenseless enemies
    - Enemies can prioritize three things: Get to goal, target player/turrets, buff other enemies.
        - Enemies can have resistances to elements or tower/player.
        - Buffing enemies probably only targettable by player?
    - Enemies could drop loot and have an auto pickup radius on player?
        - Loot would be currencies and consumables?

### UI/UX
- Display information about spelltower being built.
    - Damage stats
    - Rate of fire
    - Range overlay/ground indicator
    - etc.

- Menus should probably pause the world :-/
    - Interface-wise, go for a ff menu vibe? KISS etc. Solid color bg with simple lines marking subsects of ui elems
    - Can buy/interact with npcs where camera zooms in kinda fallout style but put camera center as portrait of npc. Portrait can go in eg upper corner and have the actual menu take up the majority of the space.
    - Tower menus too? Mbe, could also still animate towers if they have "idle animations" eg.

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