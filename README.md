# AACalculator

## Introduction

AACalculator is a simple little console application written in C# that calculates the most likely winner in an [Axis & Allies](https://www.amazon.com/Avalon-Hill-Axis-Allies-Board/dp/B007TB3R80) battle and the average number of army units that would remain. It does so, not by running an enormous number of simulations and averaging the results, but instead runs an *average simulation*.

## Background

Axis & Allies 1941 is a World War II strategy board game often likened to Risk. In it, two teams, the Axis and the Allies, strive to conquer their opponents' capitals while protecting their own. The game board is divided into territories and sea zones, which armies occupy and join battle in.

An *army* is any logical group of army units. An *army unit* is a single, unbreakable unit of a certain kind of fighting troops. It is the basic unit of battle in Axis & Allies. Army units can be divided into three general *categories*: land, air, and naval. Land units reside in territories and may be transported across sea zones, but take no part in naval battles. Naval units reside in sea zones and can neither enter territories nor take part in land battles. Air units reside in territories but can take part in any battle provided they return afterwards to a territory. There are 9 *types* of army units in Axis & Allies 1941, each with its own abilties and deployment costs:

+ **Land**
  - Infantry
  - Tanks
+ **Air**
  - Fighters
  - Bombers
+ **Naval**
  - Submarines
  - Destroyers
  - Aircraft Carriers
  - Battleships
  - Transports (useless in battle)
  
A *battle* between two armies is executed in a series of *rounds*. Each round, both the attacking army and the defending army have a chance to score hits against the opposing army. Hits are said to be scored at the same time, so casualties are not removed until both armies have "fired".

Every type of army unit has two *battle scores*: an offensive score and a defensive score. An army unit in an attacking army uses its offensive score, and an army unit in a defending army uses its defensive score. During a round, every army unit is "fired". To fire a unit, the current player rolls a die. If the result is less than or equal to the army unit's current battle score, then a single hit is scored against the opposing army. Otherwise, no hits are scored.

A battle continues until one of four conditions are met:

1. The attacking player decides to retreat.
2. One army has been defeated and no longer exists, at which point the other army has won.
3. Both armies no longer exist, at which point the battle is a tie, and the defending army retains control of the contested territory.
4. Neither army can hit the other, at which point no winner is possible (see below).

Battle is further complicated by the nature of a few of the unit types:

+ Battleships have 2 lives. Upon taking their first hit, they are merely placed on their side. Only after the second hit are they removed from the battle.
+ Submarines can never fire upon an air unit.
+ An air unit can only fire upon a submarine if their army contains a destroyer.
+ If no destroyer is present in the opposing army, a submarine has the option to perform a *surprise strike*, rather than participate in the general firing round. This attack is performed in the same manner as a normal one, but before the general firing round. Any resulting casualties are taken *immediately* and will not participate in the general firing round.

The goal of the AACalculator application is to:

1. Determine who the most likely winner of a battle is, and
2. Estimate the number of units that will remain after the battle.

It does so by simulating battle rounds, taking the calculated *average* number of casualties from each side until a result is reached.

## Installation (or lack thereof)

No offical installer exists for the AACalculator application. Standalone compiled executables for Linux and Windows are uploaded to the GitHub releases page. In this document, a bash alias has been set like so:

```
# ~/.bashrc
alias aacalc="/home/{user}/RiderProjects/AACalculator/AACalculator/bin/Debug/net5.0/AACalculatorConsole"
```

You will thus see commands like `aacalc -a "1 infantry" -d "1 tank"`. Depending on your setup, you may need to use something more like `./AACalculatorConsole ...`.

## Usage

### Basic Usage

At minimum, two options are required to invoke AACalculator: the attacking army and the defending army, specified with the **--attacker, -a** and **--defender, -d** options. Armies are given as comma-seperated lists of quantities of army units. An army containing 3 infantry, 2 tanks, and 1 fighter, for example, could be written like so:

```
3 infantry, 2 tanks, 1 fighter
```

Note that spaces between the number and the unit type are required. Also note that plurals of all the unit type names are supported as well as common abbreviations. Example:

```
3 inf, 2 t, 1 ftr
```

Putting it all together, if you wanted to know who would most likely win if 6 tanks, 2 infantry, and 1 bomber attacked 10 infantry, 1 tank, and 1 fighter, the following could be executed:

```
aacalc -a "6 tanks, 2 infantry, 1 bomber" -d "10 infantry, 1 tank, 1 fighter"
```

Note that the quotes surrounding the armies are required. This command would output:

```
Completed simulation in 4 rounds.
The defender won, with 0.852 infantry, 1 tank, 1 fighter left.
```

There are three potential battle results:

1. **A winner is declared.** Occurs when only one army remains.
2. **A tie is declared.** Occurs when no armies remain.
3. **No winner is declared.** Occurs when both armies remain because neither side can hit the other.

An example of the first was given above. An example of a tie occurs when the following command is run:

```
aacalc -a "10 tanks" -d "10 tanks"
```

The result:

```
Completed simulation in 24 rounds.
The battle was a tie! Both teams lost all their troops!
```

This is because both armies have the exact combat scores. No matter how many rounds are simulated, the two teams are always going to score the same average number of hits. This would, in fact, normally result in an infinite recursion. AACalculator, however, treats an army as empty at a certain threshold, and thus simply declares the battle a tie.

An example of no winner being declared occurs with the following command:

```
aacalc -a "1 submarine" -d "1 fighter" -r
```
The result is:

```
Completed simulation in 1 rounds.
No one won the battle! The attacker was left with 1 submarine, and the defender was left with 1 fighter.
```

This is not surprising, of course. Neither army can hit the other, and no winner can result.

### Displaying each Round

In addition to providing a summary of the results, the AACalculator application also has the ability to display a round-by-round breakdown of the battle. This can be enabled with the **--show-rounds, -r** flag. For example, when this command is run,

```
aacalc -a "6 tanks, 2 infantry, 1 bomber" -d "10 infantry, 1 tank, 1 fighter" -r
```

the output is:

```
====== Round 1 ======
Attacker: 6 tanks, 2 infantry, 1 bomber
Defender: 10 infantry, 1 tank, 1 fighter
Attacker Hits: 4
Defender Hits: 4.5
=====================


====== Round 2 ======
Attacker: 3.5 tanks, 1 bomber
Defender: 6 infantry, 1 tank, 1 fighter
Attacker Hits: 2.417
Defender Hits: 3.167
=====================


====== Round 3 ======
Attacker: 0.333 tanks, 1 bomber
Defender: 3.583 infantry, 1 tank, 1 fighter
Attacker Hits: 0.833
Defender Hits: 1.333
=====================


Completed simulation in 3 rounds.
The defender won, with 2.75 infantry, 1 tank, 1 fighter left.
```

The application displays the participants in each rounds and the number of hits each side scored against the other, all the way till the final result. It also has the capability to display surprise strikes and a battleship's extra lives:

```
$ aacalc -a "2 submarines" -d "1 battleship" -r

====== Round 1 ======
Attacker: 2 submarines
Defender: 1 battleship (1 extra life)
Attacker Surprise Hits: 0.667
Attacker Hits: 0
Defender Hits: 0.667
=====================


====== Round 2 ======
Attacker: 1.333 submarines
Defender: 1 battleship (0.333 extra lives)
Attacker Surprise Hits: 0.444
Attacker Hits: 0
Defender Hits: 0.593
=====================


====== Round 3 ======
Attacker: 0.741 submarines
Defender: 0.889 battleships (0 extra lives)
Attacker Surprise Hits: 0.247
Attacker Hits: 0
Defender Hits: 0.428
=====================


====== Round 4 ======
Attacker: 0.313 submarines
Defender: 0.642 battleships (0 extra lives)
Attacker Surprise Hits: 0.104
Attacker Hits: 0
Defender Hits: 0.313
=====================


Completed simulation in 4 rounds.
The defender won, with 0.538 battleships (0 extra lives) left.
```

Finally, it will indicate impossible/ineffective hits:

```
$ aacalc -a "1 submarine, 1 destroyer" -d "1 fighter" -r

====== Round 1 ======
Attacker: 1 submarine, 1 destroyer
Defender: 1 fighter
Attacker Surprise Hits: 0 (0.333 ineffective)
Attacker Hits: 0.333
Defender Hits: 0.667
=====================


====== Round 2 ======
Attacker: 1 submarine, 0.333 destroyers
Defender: 0.667 fighters
Attacker Surprise Hits: 0 (0.333 ineffective)
Attacker Hits: 0.111
Defender Hits: 0.333 (0.111 ineffective)
=====================


====== Round 3 ======
Attacker: 1 submarine
Defender: 0.556 fighters
Attacker Surprise Hits: 0 (0.333 ineffective)
Attacker Hits: 0
Defender Hits: 0 (0.37 ineffective)
=====================


Completed simulation in 3 rounds.
No one won the battle! The attacker was left with 1 submarine, and the defender was left with 0.556 fighters.
```

### Hit Method

In an Axis & Allies battle, the *players* get to choose which units to remove as casualties. Thus, by default, the AACalcuator application simply makes an educated guess as to which unit the player would want to remove. It uses these three criteria (in order):

1. The current battle score of the unit.
2. The deployment cost of the unit.
3. The order the units are specified.

If you would prefer to manually specify the casualty to remove each firing round, the **--hit-method, -h** option can be used. The options are "score" (default) or "manual". Others may be added in the future.

### TODO

This application is by no means complete! Some intended additions (roughly in order of importance):

+ **Unit Tests** — Until thorough unit tests are written, it is impossible to guarantee the accuracy of the calculator.
+ **Thorough Breakdown** — The application stores **every single hit** that is simulated. In other words, every round, the application keeps track of which units hit which units and how much. Very little of this information is currently displayed. In the future, there will be a "thorough breakdown" mode.
+ **Better Error Handling** — The current error handling is rather poor. Given an invalid army string, for example, the application explodes.
+ **Other Hit Methods** — A more intelligent hit method that takes into account lives, surprise strikes, destroyers, etc. is needed.

## How it Works

### Structure

WordSearchSolver is divided into two primary parts: the WordSearchSolver class library and the WordSearchSolverConsole console application. The class library contains the representative classes and calculation logic. The console application is the actual front-end. In the future, other interfaces (desktop application, phone application, etc.) may be added.

### Unit Types

Most fundamental to the application is the concept of an army unit type. A type of army unit is represented by the `UnitType` class. Every `UnitType` has a name, plural name, aliases, category (`UnitCategory.Air`, `UnitCategory.Land`, or `UnitCategory.Naval`), offensive score, defensive score, deployment cost, and number of extra lives. Each unit type is held in a `public static readonly` member of `UnitType`. All constructed unit type are stored in the static `UnitType.Values`. `UnitType` has the capability to find a unit type given its name, plural name, or one of its aliases (`.Find()`).

### Armies

An army is a logical group containing any quantities of any types of army units. Armies are represented with the `Army` class, which uses two dictionaries with `UnitType` keys internally—one to store the total amounts of each unit type and another to store the total extra lives of each unit type. `Army` has several key abilities:

+ **Taking Casualties** — In the `.Hit()` method, `Army` can attempt to remove a given number of the given unit type from the army. It tries first to remove any extra lives, then removes as many units as possible, until none of the given type are left. It returns a `HitResult` instance (see below) that contains the number of units successfully taken.
+ **Parsing** — With the `.Parse()` method, army strings (e.g. "1 infantry, 1 tnk, 5 fighters") can be parsed into `Army` instances.
+ **Formatting** — `.ToString()` returns a properly formatted representative string for the army.

### Removing Casualties

One of the more complicated aspects of the calculator involves removing the proper number and types of casualties each round. Multiple components are involved. The `IHitSelector` interface defines a class that is capable of choosing a *type* of unit as a casualty from an army. Currently only two implementations exist: `HitSelectorByScore` and `AACalculatorConsole.ManualHitSelector`.

The `Hitter.Hit()` method contains logic for removing the appropriate number/types of units from an army, given the firing army/unit, desired number of casualties, whether the firing army is the attacker, and an `IHitSelector` used to determine the types of units to remove. It essentially does the following:

1. Selects the type of army unit to remove using the `IHitSelector`.
2. Uses `Army.Hit()` to remove as many units of that type as possible.
3. Recursively removes units of a different type if `Army.Hit()` failed to remove the desired number of casualties.

`Hitter.Hit()` also stores all hits in a `List` of `HitResult` instances (see below).

### Calculation Logic

Finally, tying everything together, is the calculation logic itself, implemented in the `BattleSimulator` class. `BattleSimulator` essentially pits an attacking army and a defending army against each other until a result is reached. At the highest level, it simulates battle rounds—capturing the results—until one of the following conditions are met:

1. Both armies are empty, meaning the battle is a tie.
2. One army is empty, meaning one army has won.
3. In the last round, no hits whatsoever were scored, meaning that no further rounds are possible.

The simulation of an individual round happens in the `.DoRound()` method. It does the following:

1. Clones the two armies so that a "snapshot" can be stored in the final `RoundResult` instance (see below).
2. If applicable, simulates the surprise strikes of any attacking submarines.
3. If applicable, simulates the surprise strikes of any defending submarines. Note that the the defending army *before the attacker's surprise strikes* is used.
4. Simulates the attacker's general firing round, ignoring any submarines that already performed a surprise strike.
5. Simulates the defender's general firing round, ignoring any submarines that already performed a surprise strike. Note that the defending army *before the attacker's general firing round* is used.
6. Constructs a `RoundResult` instance (see below) representing the result of the round simulation.

Whenever a unit type is fired, whether as a surprise strike or as part of a general firing round, `Hitter.Hit()` is used to remove the appropriate number of casualties, which is calculated by `.FireUnitGroup()` as follows:

1. Gets the current battle score of the firing untit type, depending on whether it is attacking or defending.
2. Divides that score by six—since there are six sides to a standard die, and 1 hit is scored for every die result less than or equal to the battle score, the *average* number of hits for one unit is its battle score divided by six.
3. Multiplies the average number of hits for one unit by the number of units of the given type.

### Results

The final component to the AACalculator library is the storage and representation of the calculation results. While it would certainly have been possible to simply print the results out as they were calculated, a different, more useful approach was taken. Four main classes are used to represent the results of various stages of the calculation process:

+ `HitResult` — Represents the result of a single hit against a *single unit type*. It stores the hit unit type, the number of hits scored, and whether the hits were effective.
+ `FireResult` — Represents the result of one army firing against another army. It stores these results in a dictionary with `UnitType` keys and lists of `HitResult` instances as values. In other words, it stores each hit made by each unit type. Note that the only time one of the `HitResult` lists will contain more than one entry is when there were not enough units of a single type to satisfy the casualty requirement, meaning that multiple hits had to be made.
+ `RoundResult` — Represents the result of an entire round of battle between two armies. It stores the attacking and defending armies *at the beginning of the round*, the results of any surprise strikes (as `FireResult` instances), and the results of the general firing rounds (as `FireResult` instances).
+ `BattleResult` — Represents the result of an entire battle between two armies. It stores the results of all its constituent rounds (as `RoundResult` instances), the winner of the battle (`BattleWinner.Attacker`, `BattleWinner.Defender`, `BattleWinner.Tie`, or `BattleWinner.None`), and the final attacking and defending armies at the end of the battle. (Remember that `RoundResult` does not store the armies as they are at the *end* of the round.)
