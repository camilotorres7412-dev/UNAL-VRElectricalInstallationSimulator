# ⚡ VR Electrical Network Installation Simulator

> A Virtual Reality training simulator that replicates the real working conditions of installing an electrical network in low-income housing, built to local construction guidelines — giving Electronic Engineering students hands-on practice they can't safely get with live electricity.

![Engine](https://img.shields.io/badge/Engine-Unity-black?logo=unity)
![XR](https://img.shields.io/badge/XR-OpenXR-blue)
![Platform](https://img.shields.io/badge/Platform-Meta%20Quest%203-informational)
![Language](https://img.shields.io/badge/Language-C%23-239120)
![License](https://img.shields.io/badge/License-PolyForm%20Strict%201.0.0-lightgrey)
![Status](https://img.shields.io/badge/Status-Graduation%20Project-success)

---

## 🎥 Preview

![Simulator Preview](docs/media/preview.png)

---

## 📖 Overview

This project is my graduation thesis project: a **VR simulator built in Unity with OpenXR**, designed to replicate the real working conditions and technical process of installing an electrical network in low-income housing, following **local construction guidelines**.

Electronic Engineering students rarely get the chance to work hands-on with real electrical networks during their studies, since doing so carries real physical risk. This simulator closes that gap by letting students practice the full installation process — safely, repeatably, and to code — inside an interactive VR scene optimized to run comfortably on **standalone Meta Quest 3** hardware.

The project was built end-to-end by me, covering simulation design, 3D art, and systems programming.

All source code and files is written and designed around English - however, the user-facing language is all in Spanish. English translation is currently underway.

---

## ✨ Key Features

- **Guideline-accurate installation process** — the simulated workflow follows real local electrical construction codes, not a simplified abstraction of them.
- **Standalone Quest 3 performance** — the entire scene was built with mobile-XR performance budgets in mind: optimized meshes, texture atlasing/compression, and lean, allocation-conscious C# code.
- **Hand-crafted 3D asset pipeline** — every asset was modeled, textured, and exported from **Blender** into Unity, keeping poly counts and texture sizes within Quest 3 limits.
- **XR-native UI/UX** — interface and interaction design built specifically around what the Quest 3's tracked controllers/hands can comfortably do, rather than a flat-screen UI ported into VR.
- **Systems-driven architecture** — gameplay and simulation logic written with OOP principles in C# to keep the codebase modular and extensible for future installation scenarios.

---

## 🛠️ Tech Stack & Skills Demonstrated

| Area | Details |
|---|---|
| **Engine** | Unity (XR Interaction Toolkit / OpenXR) |
| **XR Runtime** | OpenXR — target device: Meta Quest 3 |
| **Programming** | C#, Object-Oriented Programming |
| **3D Art** | Blender — modeling, texturing, UV work, export/optimization pipeline to Unity |
| **Design** | UI/UX design for XR, Game Design (onboarding, feedback loops, task pacing) |
| **Optimization** | Mesh optimization, texture compression/atlasing, code-level performance tuning for standalone headset hardware |

---

```

> ⚠️ This repository contains **source files and project assets only** — no compiled builds/binaries are included. To run the project, open it in Unity (see below) or build it yourself for Quest 3.

---

## 🚀 Getting Started

<!-- Fill in your actual Unity version and any package dependencies -->
1. Install **Unity 6000.4.0f1** via Unity Hub.
2. Clone this repository:
   ```bash
   git clone https://github.com/[YOUR_USERNAME]/[YOUR_REPO_NAME].git
   ```
3. Open the project folder in Unity Hub.
4. Make sure the following packages are installed (Unity Package Manager):
   - XR Interaction Toolkit
   - OpenXR Plugin
   - Meta XR / Oculus XR Plugin (if targeting Quest 3 natively)
5. Set the build platform to **Android** and target **Meta Quest 3** for a headset build, or use the Unity Editor with an OpenXR-compatible runtime for desktop testing.

---

## 🎓 Project Context

This project was developed as my final graduation project for the Electronic Engineering program at **Universidad Nacional de Colombia**, 2026.

---

## 📜 License

This repository is source-available under the **[PolyForm Strict License 1.0.0](LICENSE)**.

In short:
- ✅ You're welcome to **view, study, and run** the code for personal, educational, or other non-commercial purposes.
- 🚫 You **may not redistribute** the code or **create derivative works/forks** from it.
- 🚫 **Commercial use is not permitted.**

All rights not explicitly granted by the license are reserved by the author. See the [LICENSE](LICENSE) file for the full legal text.

*(Note: this repository is shared to showcase the skills and process behind the project — not as an open-source template for reuse. If you're interested in using or licensing this work outside these terms, reach out — see below.)*

---

## 👤 Author

**Juan Camilo Torres Arboleda**
Systems Engineering — Universidad Nacional de Colombia - Sede Medellín

- Find my LinkedIn [Here](https://www.linkedin.com/in/juan-camilo-torres-arboleda-11b359286/)
- Contact me via [Email](mailto:camilotorres7412@gmail.com)

---

<p align="center"><i>Built to give the next generation of engineers a safer way to learn the real thing.</i></p>
