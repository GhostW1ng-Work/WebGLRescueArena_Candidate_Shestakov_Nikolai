# WebGL Rescue Arena — Performance Optimization Report

## Summary

В ходе работы проведена оптимизация физической архитектуры, памяти,
геймплейного цикла и rendering pipeline WebGL-версии арена-шутера.

Основной фокус был направлен на измеренные bottlenecks:
CPU overhead от большого количества Dynamic Rigidbody,
GC allocations в gameplay loop и большое количество Draw Calls.

Изменения выполнялись без изменения основной gameplay logic.
Проблемы и результаты проверялись с помощью Unity Profiler и
Chrome DevTools в браузере на Release/LTO WebGL build.

## Environment

- Unity: Unity 6 / 6000.5.3f1
- Browser: Google Chrome
- Platform: WebGL 2.0
- Host: itch.io
- GPU: NVIDIA RTX 3050
- CPU: Intel Core i5-11260H @ 2.6 GHz, 6 cores / 12 threads
- RAM: 16 GB
- OS: Windows 10

## Baseline

- Frame Time:и~16.6 ms
- Total JS Heap: ~50 MB
- GC Alloc: non-zero allocations during gameplay
- Draw Calls / Batches: ~331
- Active Dynamic Bodies: 50+
- WebGL build: unoptimized baseline configuration

## Bottlenecks Found

### CPU / Gameplay

- Каждый EnemyController выполнял собственный Update().
- FindGameObjectWithTag("Player") вызывался из Update().
- Использовались Vector3.Distance() и normalized в горячем пути.
- EnemyManager использовал LINQ и сортировку списка.
- UI обновлялся чаще, чем это было необходимо.

### Memory / GC

- Instantiate/Destroy использовались во время gameplay.
- Создавались временные объекты и коллекции.
- В корутинах создавались повторные WaitForSeconds.

### Physics

- Большое количество Dynamic Rigidbody увеличивало стоимость
  physics simulation.
- Enemy ↔ Enemy collisions не требовались gameplay-логике.
- Projectile использовал более тяжёлую физическую конфигурацию,
  чем требовалось для его поведения.

### Rendering

- Dynamic Batching и Static Batching были отключены.
- GPU Instancing не использовался.
- В результате сцена имела около 331 draw call/batch.

## Changes Made

### Enemy Logic

- EnemyController переведён с индивидуального Update() на
  централизованный Tick() через EnemyManager.
- Ссылка на Player кешируется.
- Vector3.Distance()/normalized заменены на проверки через sqrMagnitude.
- Удалён LINQ из горячих участков EnemyManager.
- Сортировка переработана для исключения временных allocations.

### Object Pooling

- Для врагов и снарядов внедрён Object Pooling.
- Пул предварительно прогревается.
- Instantiate/Destroy исключены из основного gameplay loop.
- Состояние Projectile защищено от повторного release.

### Physics

- Враги и снаряды переведены в Kinematic Rigidbody.
- Projectile переведён на Discrete collision detection.
- Настроена Layer Collision Matrix.
- Отключены ненужные пары столкновений.
- Для привязки врагов к рельефу реализован Raycast Grounding
  с groundMask, yOffset и QueryTriggerInteraction.Ignore.

### Rendering

- Включён GPU Instancing на подходящих материалах.
- Включены Dynamic Batching и Static Batching в Project Settings.
- Проверено отсутствие лишнего создания material instances.

### WebGL Build

- Runtime Speed with LTO
- C++ Compiler Configuration: Release
- Compression: Brotli
- WebGL Template: Minimal

## Results

| Metric | Before | After |
|---|---:|---:|
| Frame Time | ~16.6 ms | ~1.5–2.0 ms |
| Draw Calls / Batches | ~331 | 39 |
| Instanced Objects | 0 | 291 |
| Active Dynamic Bodies | 50+ | 1 |
| Active Kinematic Bodies | 0 | 63+ |
| Physics Frame Time | >1.5 ms | <0.3 ms |
| Physics Memory | — | ~2.5 MB |
| Total JS Heap | ~50 MB | 48.4 MB |

### GC

После оптимизации в измеренном gameplay path:

- 0 B/frame GC Alloc;
- единственная оставшаяся allocation — ~264 B при обновлении UI Timer
  примерно раз в секунду;
- аллокации от Enemy/Projectile spawning и основных gameplay systems
  устранены.

### Rendering

Draw Calls / Batches:

**331 → 39**

Instanced Objects:

**0 → 291**

## Biggest Performance Improvements

1. Перевод большого количества Dynamic Rigidbody в Kinematic.
2. Object Pooling для врагов и снарядов.
3. Централизация Enemy Update в EnemyManager.Tick().
4. Настройка Layer Collision Matrix.
5. Устранение LINQ и временных allocations.
6. GPU Instancing + Dynamic/Static Batching.

## Remaining Issues

- **UI Timer allocation:** ~264 B при обновлении таймера. Оставлена,
  поскольку происходит редко и не является performance bottleneck.
- **Static Colliders:** Базовые коллайдеры арены оставлены без
  дополнительной оптимизации, поскольку профилирование не показало
  существенного влияния на frame time.
- **Gameplay bugs:** Некоторые игровые edge cases сознательно не
  исправлялись, поскольку не относились к performance bottlenecks.

## What I Would Do Next

1. Отдельно пересмотреть систему pathfinding при усложнении геометрии
   уровня и распределять дорогие вычисления по кадрам.
2. Исправить оставшиеся gameplay edge cases, например возможность игрока
   покинуть арену или получить некорректное вертикальное перемещение.
3. При необходимости провести дополнительный memory profiling на
   длительной игровой сессии для проверки отсутствия долгосрочного
   роста памяти.