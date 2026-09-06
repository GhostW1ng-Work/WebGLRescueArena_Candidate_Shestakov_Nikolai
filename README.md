# WebGL Rescue Arena

Перед вами небольшой Unity-проект top-down survival shooter, первоначально разработанный с расчётом на мобильную платформу. Теперь его необходимо подготовить к работе в WebGL.

## Задача

1. Разберитесь в существующем коде и игровом потоке.
2. Найдите и исправьте наиболее существенные функциональные проблемы.
3. Снимите baseline в Unity Profiler и оптимизируйте runtime performance.
4. Уменьшите заметные frame spikes, GC pressure и проблемы, специфичные для WebGL.
5. Проверьте настройки проекта и сборки, не меняя gameplay без необходимости.

Целевая среда: WebGL в Chrome/Chromium на desktop, 1920×1080. Запускайте stress scenario через F8 или флаг `Stress Mode` на `GameManager`.

## Результат

Добавьте `CANDIDATE_REPORT.md`: найденные и исправленные проблемы, подход к profiling, измерения до/после, оставшиеся риски и следующие шаги. Не требуется исправить всё: приоритеты и аргументация оцениваются отдельно.

Сцены и prefabs собираются по [SCENE_SETUP.md](SCENE_SETUP.md). Для baseline WebGL build используйте [WEBGL_BASELINE_SETUP.md](WEBGL_BASELINE_SETUP.md).
