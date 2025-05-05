# ParlamataUI - Advanced Player UI for SCP: Secret Laboratory

ParlamataUI is a modular and dynamic UI plugin built for SCP: Secret Laboratory servers using **Exiled 9.6.0-beta7** and **HintServiceMeow (HSM) V5.4.0 Beta 1**.

It provides a fully customizable on-screen player interface that includes real-time player information, adaptive scaling based on resolution, server branding, and now **live effect tracking** and **XP + Leveling System**.

---

## Features

* ✅ Dynamic player hint panel (lower-left)

  * Nickname and real name
  * Role name
  * Spectator count (for live players)
  * Kill count (for live players)
  * Round timer (formatted as MM\:SS)

* ✅ Persistent server name display (bottom-center)

* ✅ Active effects HUD (left side, above the player hint UI)

  * Automatically displays effects affecting the player
  * Includes remaining duration (e.g. "RainbowTaste \[6s]")
  * Completely disappears when no effects are active

* ✅ XP & Level UI (top-center)

  * Displays current XP and Level
  * Real-time updates on progress
  * Includes XP gain notifications below level bar (auto-hide after 3s)

* ✅ XP System Events:

  * Kill — +5 XP
  * Death — +2 XP
  * Escape — +25 XP
  * Win (alive) — +3 XP
  * Open door — +1 XP
  * Pickup item — +2 XP
  * Drop item — +1 XP
  * Use medkit — +2 XP
  * Throw grenade — +1 XP
  * Activate generator — +5 XP
  * Use SCP-914 — +3 XP
  * Spawn — +3 XP
  * Resurrect (as SCP-049) — +2 XP

* ✅ Adaptive positioning based on resolution & aspect ratio

* ✅ Uses [HintServiceMeow](https://github.com/MeowServer/HintServiceMeow) for stable and performant hints

* ✅ Configuration-driven control over enabled modules, emoji-style icons, and XP reward values

* ✅ Debug logging support

---

## Dependencies

* Exiled 9.6.0-beta7
* HintServiceMeow-Exiled.dll (V5.4.0 Beta 1)

---

## Installation

1. Download and install Exiled 9.6.0-beta7 on your server.
2. Place `HintServiceMeow-Exiled.dll` in your `dependencies` folder.
3. Place the compiled `ParlamataUI.dll` into your `plugins` folder.
4. Make sure your `config.yml` for the plugin is set correctly (see below).

---

## Configuration

```yaml
is_enabled: true
update_interval: 1.0
show_real_name: true
show_role: true
show_spectators: true
show_kills: true
show_elapsed_round_time: true
debug: false
show_xp: true

server_name: "[BUL/ENG] BULGARIA - ПАРЛАМАТА"

emoji_icons:
  name: 🔍
  role: 🤖
  spectators: 👥
  kills: ✈
  timer: ⏱

xp_rewards:
  on_kill: 5
  on_death: 2
  on_escape: 25
  on_win: 3
  on_door_open: 1
  on_pickup_item: 2
  on_drop_item: 1
  on_use_medical: 2
  on_throw_grenade: 1
  on_generator_activate: 5
  on_upgrade_item: 3
  on_spawn: 3
  on_resurrect: 2
```

---

## Developer Notes

* Positioning of hints is resolution-independent, calculated with a custom formula:

```cs
float GetLeftXPosition(float aspect) =>
  622.27444f * Pow(aspect, 3f) - 2869.08991f * Pow(aspect, 2f) +
  3827.03102f * aspect - 1580.21554f;
```

* Server name hint is created once per player and stays persistent.
* Active effect hints are dynamically created/removed based on effect presence.
* XP feedback hint uses timed removal via coroutine (disappears after 3s).
* All hint rendering is handled through `HintServiceMeow.Core.Models.Hints.Hint`.

---

## License

This plugin is developed for and used by the SCP Bulgaria community. You are free to use, fork, and adapt it under the MIT License.

---

**Created by:** LaFesta1749
**Server:** SCP Bulgaria ПАРЛАМАТА
