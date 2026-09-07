# Summary

В ходе работы проведена комплексная оптимизация физической архитектуры, подсистемы памяти и геймплейного цикла WebGL-версии арена-шутера. Основной акцент был сделан на исключение оверхеда физического движка PhysX, устранение промежуточных аллокаций памяти (`0 B GC Alloc` на кадр) и достижение максимального FPS без изменений в геймплейной логике. Все решения подтверждены инструментальными измерениями в Unity Profiler и Chrome DevTools в реальном браузерном окружении (itch.io) на Release/LTO билде.

# Environment

Unity version: 2026 LTS (Unity 6 / 6000.5.3f1)

Browser: Google Chrome (WebGL 2.0 / itch.io Host)

Hardware: Mid-Range Desktop PC (Windows 10) #ПОСМОТРИ

# Baseline

FPS / Frame Time: ~60 FPS / ~16.6 ms (с регулярными микрофризами из-за PhysX и GC)

Memory: ~50 MB Total JS Heap (динамический рост Heap из-за `Instantiate`/`Destroy`)

GC Alloc: > 0 B на кадр (аллокации в `Update` и при физических проверках)

Build Size: Unoptimized WebGL baseline

Load Time: Standard

# Issues Found

1. **PhysX Overhead:** Более 50 активных динамических тел (`Active Dynamic Bodies`) нагружали главный поток CPU просчетом гравитации, инерции и сложных коллизий объемов.
2. **GC Alloc Spikes & Fragmentation:** Использование `Instantiate`/`Destroy` при спавне пуль/врагов и выделения памяти в `Update()` выдерживали постоянную нагрузку на сборщик мусора (V8 GC).
3. Были выключены динамическая и статическая типизация, не был включен GPU Instancing из-за чего было слишком много Draw Calls.
4. **Build Overhead:** Неоптимизированные параметры компиляции C++ и включенный перехват исключений снижали производительность итогового `.wasm` модуля.

# Changes Made

* **Kinematic Migration & Manager Pattern:** Все враги и снаряды переведены в кинематический режим (`Is Kinematic = true`). Нативная логика `Update()` у врагов заменена на единый централизованный `Tick()` из `EnemyManager` для устранения оверхеда вызовов C++/C# из движка.
* **Zero-Alloc Object Pooling:** Использован предварительно инициализированный пул объектов для снарядов и врагов, исключивший операции выделения и освобождения памяти во время боя.
* **Raycast Grounding:** Реализован алгоритм вертикального прижима к полному рельефу через `Physics.Raycast` с настройкой `yOffset` (компенсация высоты Pivot), явной маской `groundMask` и игнорированием триггеров (`QueryTriggerInteraction.Ignore`).
* **GPU Instancing & Rendering Batching:** Включил галочку **Enable GPU Instancing** на материалах. Это позволило движку упаковывать однотипную геометрию орды в единые вызовы отрисовки и сократить число Draw Calls.
* **Production Build Settings:** Настроена сборка в Unity 6:
  * **Code Optimization:** `Runtime Speed with LTO` (Link-Time Optimization)
  * **C++ Compiler Configuration:** `Release`
  * **Compression:** `Brotli`
  * **WebGL Template:** `Minimal`

# Measurements After Changes

FPS / Frame Time: 500+ FPS / ~1.5–2.0 ms

Scripting Execution Time (Chrome DevTools): 8 ms за 8.68 сек записи (~0.001 ms на кадр)

Memory (Physics): 2.5 MB

Total JS Heap (Chrome DevTools): 48.4 MB

GC Alloc: 0 B на кадр (Подтверждено в Unity Profiler и чистым графиком `Allocation timelines` в Chrome DevTools)

Active Dynamic Bodies: 1 (Только Player)

Active Kinematic Bodies: 63+ (Все враги и пули)

# Remaining Issues

* Static Colliders Sync: Оставлены базовые статические коллайдеры геометрии арены, так как при текущем числе объектов (<100) они не создают узких мест в производительности.
* Visual Polish: Не вносились изменения в шейдеры и графические материалы, чтобы сфокусироваться на измеренных системных Bottlenecks (Physics & CPU).
* Геймплейные баги

# What I Would Do Next

1. **Time-Sliced NavMesh:** При усложнении геометрии уровня (многоэтажность, мосты, динамические укрытия) заменить прямой `Raycast Grounding` на асинхронный NavMesh с распределением просчета путей по кадрам.
2. Исправил бы геймплейные баги(например как игрок может выйти за арену или улететь в небеса)