# ⚡ **Energy Flux**

**Energy Flux** is an educational VR game where players learn about **kinetic** and **potential energy** by observing a sliding ball and answering energy-related questions at specific moments. The game is designed for simple, fast physics learning through interaction.

---

## 🎮 **Gameplay Overview**

In **Energy Flux**, players control a ball sliding down a slope and must answer questions regarding which energy type (kinetic or potential) is dominant at specific points along the way. The game combines physics-based mechanics and educational content, creating a hands-on learning experience.

### 🔧 **Gameplay Steps**

1. **Start the Game**: Players begin by clicking the **Start** button.
2. **Slide the Ball**: The ball starts sliding down the slope.
3. **Answer Questions**: When the ball stops, a question appears, asking the player which energy is higher: **Kinetic** or **Potential**.
4. **Correct Answer**: If the player selects the correct energy type, the ball continues sliding to the next point.
5. **Incorrect Answer**: If the player selects the wrong energy type, the ball resets to the initial position.
6. **Timer**: The player has **3 minutes** to finish the game.

---

## ✨ **Key Features**

| Feature | Description |
|----------|-------------|
| ⚡ **Interactive Ball Movement** | The ball slides along a randomized path, allowing players to experience different energy states. |
| 🔄 **Dynamic Questions** | Players must determine which energy is higher based on the ball’s position and movement. |
| ⏳ **Timed Challenge** | Players must answer within a set time frame to succeed, encouraging quick thinking. |
| 🖱️ **Mouse & PC Test Mode** | Simulate all interactions via mouse for PC debugging and testing. |
| 🎧 **Audio Feedback** | Includes audio cues for correct/incorrect answers and background music to enhance immersion. |
| 🔁 **Replayability** | The game restarts after each successful mission, allowing for repeated playthroughs. |

---

## 🧩 **Gameplay Flow**

### **Scene 1 – Energy Flux**

1. **Start Screen**: The game starts with an intro panel, where the player can choose between **Start**, **About**, and **Quit** options.
2. **Sliding Ball**: The ball slides down the slope and stops at random intervals.
3. **Question Panel**: At each stop, a question asks the player which type of energy (kinetic or potential) is higher at that point.
4. **Correct Answer**: The ball continues sliding, and the next question appears.
5. **Incorrect Answer**: The ball resets, and the player must start over.
6. **Timer**: The player has 3 minutes to complete the game.

---

## 🧠 **Educational Objectives**

- **Energy Types**: Teach players the difference between **kinetic energy** and **potential energy**.
- **Energy Conservation**: Show how energy transforms from potential to kinetic and vice versa.
- **Critical Thinking**: Encourage players to think critically about energy states based on the ball's position.
- **Timing**: Improve players' decision-making speed under a time constraint.

---

## 🛠️ **Technical Overview**

| Component | Description |
|------------|-------------|
| **Engine** | Unity 2022.3 LTS |
| **Framework** | XR Interaction Toolkit + OpenXR |
| **Input Modes** | Hand tracking, VR controllers, or mouse (debug) |
| **Scenes** | `Energy Flux` (interactive ball scene) |
| **Audio System** | Persistent, non-repeating, cross-scene `AudioManager` |
| **Physics** | Trigger-based collision system to detect when the ball reaches each stop point. |
| **Timer** | Centralized timer shared across all scripts to handle time-based success/failure. |

---

## 📜 **Updated Script Summary**

| Script | Purpose |
|--------|----------|
| **`AudioManager.cs`** | Centralized system for all audio cues, ensuring no repetition; persistent across scenes. |
| **`GameTimer.cs`** | Global timer accessible to all mission scripts for countdown-based logic. |
| **`MenuScript.cs`** | Handles main menu, mission start, and time initialization. |
| **`BallSliding.cs`** | Controls the ball’s movement, random stop points, and interaction with the question panel. |
| **`QuestionPanel.cs`** | Handles the display and functionality of the question panel and player interaction with answers. |
| **`EnergyFluxGameManager.cs`** | Oversees game state, success/failure, and game-over UI logic. |

---

## 🧭 **Mission Flow Summary**

> **Press the Start Button → Ball Slides → Question Panel → Answer Correctly → Continue → Answer Incorrectly → Restart → Reach End to Win**

---

## 🎧 **Audio System Enhancements**

| Feature | Behavior |
|----------|-----------|
| 🎵 **Menu Music** | Loops continuously on main menu only. |
| 🔈 **Narration Clips** | Play once per session and do not repeat. |
| 🚀 **Cross-Scene Audio** | Win/Fail narrations persist across scene loads. |
| 🎚️ **Music Ducking** | Music volume lowers automatically during narration playback. |

---

## 📦 **Requirements**

- **Unity Version:** 2022.3 LTS or later  
- **Packages Required:**
  - XR Interaction Toolkit (v3.x or newer)
  - OpenXR Plugin  
  - TextMeshPro  
- **Hardware:**
  - Meta Quest / HTC Vive / OpenXR-compatible headset  
  - Optional: mouse and keyboard for testing

---

## 👨‍🚀 **Credits**

Developed, scripted, and produced by  
**🎖️ Ristwak Pandey**

> I am the sole developer and rightful owner of this project and hold full authority over its logic, code, visuals, and distribution.

---

## 📜 **License**

This project is copyrighted under **Ristwak Pandey**.  
All rights reserved.  
Unauthorized reproduction, modification, or distribution of this project or its assets is strictly prohibited.

---

## 🪐 **Closing Note**

> *Energy Flux* offers an exciting and educational journey into the world of **energy**. Through interactive play and real-time feedback, players gain a deeper understanding of how **kinetic** and **potential** energy work in the real world, while having fun in the process!
