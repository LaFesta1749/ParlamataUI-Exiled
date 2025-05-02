# ParlamataUI - Advanced Player UI for SCP:Secret Laboratory

ParlamataUI is a modular and dynamic UI plugin built for SCP: Secret Laboratory servers using **Exiled 9.6.0-beta7** and **HintServiceMeow (HSM) V5.4.0 Beta 1**.

It provides a fully customizable on-screen player interface that includes real-time player information, adaptive scaling based on resolution, server branding, and now **live effect tracking**.

---

## Features

- ✅ Dynamic player hint panel (lower-left)
  - Nickname and real name
  - Role name
  - Spectator count (for live players)
  - Kill count (for live players)
  - Round timer (formatted as MM:SS)

- ✅ Persistent server name display (bottom-center)

- ✅ Active effects HUD (left side)
  - Automatically displays effects affecting the player
  - Includes remaining duration (e.g. "RainbowTaste [6s]")
  - Completely disappears when no effects are active

- ✅ Adaptive positioning based on resolution & aspect ratio

- ✅ Uses [HintServiceMeow](https://github.com/MeowServer/HintServiceMeow) for stable and performant hints

- ✅ Configuration-driven control over enabled modules and emoji-style icons

- ✅ Debug logging support

---

## Dependencies

- Exiled 9.6.0-beta7
- HintServiceMeow-Exiled.dll (V5.4.0 Beta 1)

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

server_name: "[BUL/ENG] BULGARIA - ПАРЛАМАТА"

emoji_icons:
  name: 🔍
  role: 🤖
  spectators: 👥
  kills: ✈
  timer: ⏱
