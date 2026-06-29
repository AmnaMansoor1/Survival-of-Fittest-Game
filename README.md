# Survival-of-Fittest-Game
This game was developed as my semester end project for my Game Development course . Player has to survive a dark night full of enemies attacking him to win
---

## 🎮 Game Overview

**Theme:** A lone knight must survive a dark with set timer, treacherous night in a dense forest. Enemies randomly spawn at different locations across the map, and the player must fight to stay alive until dawn.

**Environment:** The forest environment is built using the **Polygon City** asset pack from Synty Studios, providing a rich, low-poly aesthetic.

**Objective:** Survive as long as possible by defeating enemies that relentlessly hunt you down. Every kill buys you more time, but the night is long and the enemies keep coming...

---

## 🛠️ Key Technical Features

This project was developed to fulfill the following technical requirements:

### Core Mechanics
- **Random Spawning & Instantiation**: Enemies are spawned dynamically at random positions across the map using Unity's `Instantiate()` method.
- **GameObject Animations**: Custom animations for player attacks, enemy movements, and environmental interactions.
- **Rigidbody Physics**: Realistic physics interactions including bouncing, collisions, and force-based movements.

### Input & Control
- **Mouse & Touch Controls**: Responsive input handling for both desktop and mobile platforms.
- **Viewport Responsive Screen**: UI and game elements adapt to different screen sizes and resolutions.
- **Anchor Based Scaling**: UI components are anchored properly to maintain layout consistency across devices.

### AI & Navigation
- **AI Navigation**: Enemies use Unity's NavMesh system to intelligently navigate the terrain and chase the player.
- **Raycast**: Used for line-of-sight detection, enemy vision, and attack mechanics.

### Visual & Audio Design
- **Texturing & Materials**: Custom materials applied to game objects for visual richness.
- **Skybox & Particle Systems**: Atmospheric skybox and particle effects (fire, dust, magic, etc.) for immersive background design.
- **Mixamo Characters & Animations**: Character models and animations imported from Adobe Mixamo.

### Advanced Features
- **Unity Ads Integration**: In-game ad support for monetization (rewarded ads, interstitials, etc.).
- **IEnumerators**: Used extensively for coroutines, timers, spawn delays, and asynchronous events.

---

## 📦 Asset Sources

| Asset | Source |
| :--- | :--- |
| Forest Environment | Synty Studios - Polygon City (Unity Asset Store) |
| Character Models & Animations | Adobe Mixamo |
| Skybox & Particle Systems | Unity Asset Store & Custom Creation |

---

## 📱 Platform Support

- Windows (Standalone)
- Android (APK build available)

---

## 👨‍💻 Project Information

- **Course:** Game Development Lab
- **Submission:** Individual project (APK + full project zip, excluding Library folder)
- **Status:** ✅ Completed

---

## 🚀 How to Run

1. Clone this repository.
2. Open the project in Unity (2021.3+ recommended).
3. Open the main scene (located in `Assets/Scenes/`).
4. Press Play in the Unity Editor, or build for your target platform.

---

## 📸 Screenshots
<img width="640" height="254" alt="Screenshot 2026-06-29 033141" src="https://github.com/user-attachments/assets/78c1234b-4361-43e1-9379-1c05b0b374fc" />
<img width="587" height="248" alt="Screenshot 2026-06-29 033217" src="https://github.com/user-attachments/assets/96357333-2a4b-47d9-9830-9b2d1b419455" />




## 🙏 Acknowledgments

- Synty Studios for the amazing Polygon City asset pack.
- Adobe Mixamo for character animations.
- Unity Technologies for the game engine and documentation.

---

**Made with ❤️ as part of the Game Development Lab**
