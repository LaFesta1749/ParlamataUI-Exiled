# ParlamataUI - Advanced Player UI for SCP: Secret Laboratory

ParlamataUI is a modular and dynamic UI plugin built for SCP: Secret Laboratory servers using **Exiled 9.6.0-beta7** and **HintServiceMeow (HSM) V5.4.0 Beta 1**.

It provides a fully customizable on-screen player interface that includes:
- real-time player information
- adaptive scaling based on resolution
- custom server branding
- XP + Leveling System
- status effect tracking

## Features

✅ **Dynamic player hint panel (lower-left)**  
- Nickname and real name  
- Role name  
- Spectator count  
- Kill count (for non-106)  
- Kill count + pocket trap victims (for SCP-106)  
- Round timer (formatted as MM:SS)

✅ **Persistent server name display (bottom-center)**  
- Configurable static branding text

✅ **Status Effect HUD (upper-left)**  
- Shows active effects and their remaining duration  
- Only appears if effects are active  
- Auto-hides when empty

✅ **Status Effect Intensity**
- Active effects now also show their intensity level (if applicable)
- Format: [Time Left, Lvl X]
- Helps track effect strength (e.g. Bleeding Lvl 2)

✅ **XP + Level UI (top-center)**  
- Live XP bar and level info  
- XP gained from events  
- XP popup messages

✅ SCP-106 Escape Hint (center screen)
- Displays a message when a player escapes SCP-106's pocket dimension
- Auto-removes after 3 seconds
- Replaces previous message if multiple players escape

✅ **Supports spectators**  
- Spectators see stats of the player they're watching  
- UI adapts to role

✅ **Resolution-aware positioning**  
- Hint placement recalculated dynamically with formula:
```cs
float GetLeftXPosition(float aspect) =>
  622.27444f * Pow(aspect, 3f) - 2869.08991f * Pow(aspect, 2f) +
  3827.03102f * aspect - 1580.21554f;
```

✅ **Kill tracking + Pocket logic**  
- Standard kills tracked  
- SCP-106 gets additional victim tracking for pocket dimension  
- Broadcast when someone escapes the pocket

✅ **Built-in XP/Level System**  
- Level formula: Level XP requirement increases with each level  
- Event-based XP rewards  
- Data saved per UserID

✅ **XP Commands (.xplvl / .xpset / .xpadd / .xpr / .xpbackup etc)**  
- Manage levels and XP from console or RA  
- View top players with `.xpleaderboard`

✅ **Configurable Emojis and UI text**  
✅ **Debug mode**

## Installation

1. Download and install **Exiled 9.6.0-beta7** on your server.
2. Place `HintServiceMeow-Exiled.dll` into your `dependencies` folder.
3. Place `ParlamataUI.dll` into your `plugins` folder.
4. Start the server once to generate the default config.
5. Edit the config (`ParlamataUI/config.yml`) to match your preferences.

## Configuration Example
is_enabled: true
debug: false
update_interval: 1.0
enable_for_spectators: true
show_real_name: true
show_role: true
show_spectators: true
show_kills: true
show_elapsed_round_time: true
show_xp: true

server_name: "[BUL/ENG] BULGARIA - ПАРЛАМАТА"

emoji_icons:
  name: 👤
  role: 🎭
  spectators: 👥
  kills: 🔪
  timer: ⏱
  pocket: 🕳

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

## XP Console/RA Commands

- `.xpleaderboard` / `.xpl`  
  Show top 10 players and your own XP/level.

- `.xplvl <UserID> <amount>`  
  Add level(s) to a user.

- `.xpset <UserID> <amount>`  
  Set XP directly and recalculate level.

- `.xpadd <UserID> <amount>`  
  Add raw XP to user.

- `.xpr <UserID>`  
  Reset one user’s XP.

- `.xpra`  
  Reset ALL users' XP.

- `.xpbackup` / `.xpb`  
  Create a `.bak` backup of the XP database.

## Developer Notes

- Hints are stored per player in dictionaries.
- Effects are dynamically removed if empty.
- Hint positions adapt to resolution using aspect ratio math.
- XP data is stored using LiteDB in `XPSystem.db`.
- XPManager handles all operations and formatting.
- XPEventCache prevents farming the same event repeatedly in one round.

## License

MIT License – Free to use, modify, and distribute.

---

**Created by:** LaFesta1749  
**Server:** SCP Bulgaria ПАРЛАМАТА  
**Version:** 1.0.4  
