# WebGL baseline build

1. Откройте проект в Unity `6000.5.3f1`.
2. Assemble `Boot`, `MainMenu`, and `Game` using [SCENE_SETUP.md](SCENE_SETUP.md).
3. Open **File → Build Settings**, switch platform to **WebGL**, and add scenes in this order: `Boot`, `MainMenu`, `Game`.
4. Build, then use **Build And Run** or serve the generated folder from a local web server.
5. In the browser, play a session and record a baseline in the Unity Profiler.
6. Press F8 during Game, or enable `Stress Mode` on `GameManager` before building, to exercise the high-load scenario.
