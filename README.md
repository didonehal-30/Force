# 🎮 Force

A 2D platformer game built in Unity where the player can **push and pull objects** to solve puzzles, avoid enemies, and escape traps.

## 🧠 Core Idea

Players use movement and **force manipulation (push/pull)** to interact with the world:
- Push: `E`
- Pull: `Q`
- Move: `W`, `A`, `S`, `D`
- Double Jump: `W` twice

Win by reaching the end of each level without dying.

## 🚀 Features

- **Push/Pull Mechanics**: Interact with objects without touching them
- **Physics-Based Puzzles**: Stack, shift, or launch objects
- **Enemies and Traps**: Avoid or manipulate them strategically
- **Power-ups**:
  - Double Jump
  - Gravity Reversal
  - Enemy-to-Trampoline
- **Prototypes for future mechanics**:
  - Gravity wells
  - Wall crawl & jump
  - Grappling movement

## 📦 Repository

- **Main GitHub**: [CSCI-526/main-project-dream-team](https://github.com/CSCI-526/main-project-dream-team.git)
- **Playable Alpha Build**: [Try It Now](https://didonehaln.github.io/ForceGame/)

## 🧪 Mechanics Inspiration

| Game               | Mechanic                          | Applied Twist in _Force_                    |
|--------------------|-----------------------------------|---------------------------------------------|
| Super Mario        | Jump to collect/avoid             | Add physics-based object interactions       |
| Fireboy & Watergirl| 2-player coordination             | Push/Pull replaces 2nd player help          |
| Outer Wilds        | Gravity wells                     | Pull objects using directional force        |
| Vex 3              | Wall-jumping                      | Prototype explored then scrapped            |
| Geometry Dash      | Auto-scroll + jump timing         | Reimagined as time-based precision levels   |

## 👥 Team Members

| Name                 | Role                        | GitHub ID         |
|----------------------|-----------------------------|--------------------|
| Vedaant Rajeshirke   | Gameplay, Physics           | `rocktopus101`     |
| Didonehal Nanje Gowda| UI, Product Manager         | `didonehaln`       |
| Jiahe Hu             | Level Design, Physics       | `clark0610_`       |
| Zhaohui Wang         | AI, Level Design, Systems   | `geoffrey1117`     |
| Aiswarya Madhu       | Captain, Analytics          | `aisw_123_54650`   |

## 📊 Analytics Highlights

Methodology Used: 
- Data Collection:
     Google sheets (link to sheet data: https://docs.google.com/spreadsheets/d/e/2PACX-1vTtFGAHNMMPe7F4WxuHsN1Gbk4IrvlZyHJkQS4tUl6jq6klnMyjMPvhdHP2eRDMvvwrTXI5MLIGx5we/pubhtml)
- Data Visualization:
     Python to create graphs (link to colab file: https://colab.research.google.com/drive/1uZMA722x5vrsnua-to961lr-GttYd9Qn#scrollTo=GPJt6--RbjvZ)


## 🛠️ Development Log

### Week 8
- Prototype development: Wall Jump, Dash, Gravity Well, Rope Grappling
- Level implemented with force-based object control

### Week 9
- Finalized core mechanics
- Created two functional levels with push/pull logic
