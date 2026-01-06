# Ladder Sisyphus 

![Godot Engine](https://img.shields.io/badge/Godot-4.x_.NET-478cbf?logo=godot-engine&logoColor=white)
![Language](https://img.shields.io/badge/Language-C%23-239120?logo=c-sharp&logoColor=white)
![Status](https://img.shields.io/badge/Status-GDD_v1.0-orange)

**Game Design Document & Technical Overview**

## High Concept
The player controls **Sisyphus**, who must climb to the top of an infinite, surreal tower.

**The Catch:** Sisyphus carries a long, physical ladder on his back that he cannot unequip or hide. The ladder collides with everything in the game world. It is simultaneously the **only tool allowing progress** (creating bridges, extra height) and the **biggest obstacle** (getting stuck in corridors, knocking the player off ledges).

---

## Game Details
* **Genre:** Foddian / Precision Platformer / Physics Puzzle
* **Engine:** Godot 4.x (.NET Edition)
* **Language:** C#

---

## Technical Architecture (Godot + C#)
A **Hybrid Approach** was chosen to achieve the best balance between platforming responsiveness and physical chaos.

### 2.1. Player Structure (Scene Tree)
* **Player (`CharacterBody2D`)**:
    * Main node controlled by the player (`PlayerController.cs`).
    * Handles Left/Right movement (WSAD) and gravity.
    * Uses standard `MoveAndSlide()`.
* **PivotPoint (`Node2D`)**:
    * Anchor point for the ladder (positioned at shoulder/hand height).
* **`PinJoint2D`**:
    * Physical connection between the Player and the Ladder.
    * **Softness:** Low (stiff connection) but not zero (to prevent physics explosions).
* **Ladder (`RigidBody2D`)**:
    * The physical ladder object (`LadderPhysics.cs`).
    * **Critical Settings:**
        * `Solver -> Continuous CD`: **ON** (Prevents tunneling through walls during fast rotations).
        * `Mass`: High (Must feel heavy to the player).
        * `Linear/Angular Damp`: Tuned so the ladder doesn't spin like a fan, but provides resistance.

### 2.2. Collision Layers
Proper bitmask configuration is crucial for stability.

| Layer | Name | Description |
| :--- | :--- | :--- |
| **1** | **World** | Walls, floors, obstacles. |
| **2** | **Player** | Sisyphus hitbox. |
| **3** | **Ladder** | Ladder hitbox. |

**Collision Rules:**
* **Player** collides with **World**.
* **Ladder** collides with **World**.
* **Player** DOES NOT collide with **Ladder** (prevents self-collision jitters and physics bugs).

---

## Gameplay Mechanics

### 3.1. Controls
* **Movement (A / D):** Move the character. The ladder drags/follows the pivot point.
* **Jump (Space / W):** Small character jump. Primary height gain comes from climbing mechanics.
* **Ladder Control (Mouse):**
    * Mouse cursor position defines the *target angle* of the ladder.
    * **C# Implementation:** Use `ApplyTorqueImpulse` or a PID Controller to rotate the `RigidBody2D` toward the cursor.
    * *Note:* Never set rotation directly/rigidly. The ladder must "want" to turn but be physically blocked by walls if they are in the way.

### 3.2. Physics Interactions
* **Bridging:** Player lays the ladder horizontally across a gap. Walks on the ladder.
    * *Challenge:* Weight distribution. If the player walks too far without a counterweight, the ladder tips and falls.
* **The Wedge:** Player can jam the ladder between two walls to create an improvised platform to "rest" while falling.
* **Vaulting:** Running start + jamming the ladder into the ground at an angle = launching the character upwards (momentum transfer).

---

## Level Design
The game consists of one continuous vertical map. Difficulty increases non-linearly.

### 4.1. Biomes
1.  **The Slums (Tutorial):** Tight corridors, stairwells. Teaches the player that a long ladder does not fit around sharp corners.
2.  **Industrial (Construction Site):** Cranes, moving platforms. Teaches momentum management and hooking the ladder onto environment elements.
3.  **Clockwork (Mechanisms):** Rotating gears. The ladder can be crushed or launched by the cogs.
4.  **The Peaks:** High winds. Affects the ladder physics depending on its angle relative to the wind.

### 4.2. Key "Sadistic" Obstacles
* **The Needle's Eye:** A spiral staircase requiring pixel-perfect rotation precision.
* **The Pendulum:** Rope swings where the ladder on your back hooks onto everything.
* **The Fake Bridge:** Platforms that act as levers—they bow or tip under the combined weight of the player and ladder.

---

## Art & Audio

### 5.1. Visuals
* **Style:** Hi-bit Pixel Art or Hand-drawn (gritty, sketchy aesthetic).
* **Readability:** High contrast between the Ladder and the Background. The player must clearly see where the ladder's hitbox ends.

### 5.2. Audio (Feedback Loop)
Sound serves both informational and emotional purposes.
* **Collisions:** Different sounds for different materials (Wood on Stone, Wood on Metal). Volume scales with `LinearVelocity`.
* **Tension:** Wood creaking/groaning sounds when the ladder is bent or wedged under pressure.
* **Narrator:** A calm, slightly bureaucratic voice that comments on failures.

---

## Project Roadmap

### Phase 1: "Physics Prototype" (The Greybox)
> **Goal:** Working with grey boxes, a capsule character, and a rectangle (the ladder). Walking and swinging the object must feel satisfying yet frustrating. **Zero graphics.**

- [ ] **1.1 Project Setup (Godot + C#)**
    - [ ] Configure .NET environment.
    - [ ] Set up 2D Physics (Gravity, Collision Layers).
- [ ] **1.2 Player Controller (Sisyphus)**
    - [ ] Basic Movement (WSAD), Jumping.
    - [ ] `CharacterBody2D` implementation.
- [ ] **1.3 Ladder Physics**
    - [ ] Add `RigidBody2D` (Ladder).
    - [ ] Connect to Player via `PinJoint2D`.
    - [ ] Control Script: Mouse applies **Torque** (force), instead of setting rotation directly.
- [ ] **1.4 "The Playroom" (Test Room)**
    - [ ] Create 3 simple obstacles using grey blocks:
        - [ ] Narrow Tunnel (Collision test).
        - [ ] The Gap (Bridging test).
        - [ ] High Ledge (Climbing/Pull-up test).

** Milestone 1:** You can clear these 3 obstacles without physics bugs (e.g., flying into space, clipping through walls).

---

### Phase 2: "Deep Mechanics" (Core Systems)
> **Goal:** The ladder stops being just a block and becomes a tool. Adding control nuances.

- [ ] **2.1 Grip Mechanic**
    - [ ] Implement a key (e.g., Space) to "stiffen" the joint (increase `angular_damp` or joint bias), allowing for precise aiming at the cost of speed.
- [ ] **2.2 Surface Physics**
    - [ ] Add `PhysicsMaterial` to the ladder (Friction and Bounce). The ladder shouldn't slip like soap; it needs to "grip" edges.
- [ ] **2.3 Dynamic Camera**
    - [ ] Camera should not lock rigidly on the player but "look" where you are aiming the ladder (up/down).
- [ ] **2.4 Fall System (Ragdoll)**
    - [ ] High-velocity falls trigger a loss of control (character becomes a physical ragdoll), but the ladder remains attached.

** Milestone 2:** The game is mechanically fully playable. Falls hurt, and success feels rewarding.

---

### Phase 3: "Tower Construction" (Level Design)
> **Goal:** Create a continuous map that breaks the player's spirit.

- [ ] **3.1 Tilemap System**
    - [ ] Prepare Autotiles (simple placeholders) for rapid level building.
- [ ] **3.2 Biome Design (Draft)**
    - [ ] Build the full vertical path: **Slums -> Industrial -> Clockwork -> Peaks**.
    - [ ] Implement specific traps (The Needle's Eye, The Pendulum).
- [ ] **3.3 Save System**
    - [ ] Real-time position saving (every second).
    - [ ] **Rule:** No "Checkpoints," only pain. If you quit while falling, you resume falling when you reload.

** Milestone 3:** It is possible to climb (or attempt to climb) from the very bottom to the very top.

---

### Phase 4: "Polish & Atmosphere" (Art & Juice)
> **Goal:** The game stops looking like a prototype and starts looking like a product.

- [ ] **4.1 Visuals**
    - [ ] Replace grey blocks with final assets (Pixel Art / Hand-drawn).
    - [ ] Sisyphus Animations (Exertion, sweating, dangling legs).
- [ ] **4.2 Audio (Crucial)**
    - [ ] Ladder collision sounds (varied based on impact velocity).
    - [ ] Character grunts/effort sounds.
    - [ ] Ambience (Wind, distant city noise).
- [ ] **4.3 Narrator**
    - [ ] System to trigger voice lines/text based on zones or after significant falls.

** Milestone 4:** The game looks and sounds professional.

---

### Phase 5: Release & Testing
> **Goal:** Releasing the game to the world.

- [ ] **5.1 Playtesting**
    - [ ] Observe friends playing. Do they get stuck where intended, or is it bad design?
- [ ] **5.2 Bugfixing**
    - [ ] Patching collision holes.
- [ ] **5.3 Publication**
    - [ ] Itch.io / Steam page setup.
