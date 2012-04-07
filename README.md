Chaotic Mind
============

Chaotic Mind is a top-down shooter set in a shape-shifting maze.

In ChaoticMind, you are a brilliant physicist plagued by a terrible virus of the mind. The madness haunts your dreams and is siphoning off your thoughts and memories. You've already forgotten the first few decades of your life and will forget everything you have ever known if you don't take action. The only cure is to delve deep within the maze of your own mind and rediscover your past, ridding yourself of the madness which has wormed its way into the deepest and darkest corners of your mind. You enter a deep meditation and confront the nightmares, wondering if you'll ever wake up again...

Youtube Demo
============
[![ChaoticMind Alpha Gameplay](http://i.ytimg.com/vi/boIDvoeGzjA/0.jpg)](https://www.youtube.com/watch?v=boIDvoeGzjA)

Playing the Game
================
- The main objective is to recover all the glowing collectables (3 to win).
- Collecting an object restores your sanity meter.
- The map isn't static, the rows and columns can be shifted using the shifting interface.
- You will need to shift the map in order to make it to your objectives.
- Careful, when shifting the map, you disrupt the enemies, causing more of them to spawn.

Controls
--------
- W/A/S/D - Move player up/left/down/right
- Mouse movement - Move aiming reticle
- Left click - Shoot current weapon at aiming reticle
- E - Reload weapon (discards rest of clip)
- TAB - Switch current weapon
- SPACE - Toggle shifting interface
- ESC - Exit game

Primary Interface
-----------------
- Top center is the sanity meter. This decreases when you take damage from enemies, go out of the map, of shift the current object to collect off the map.
- Bottom left is the minimap. This shows a representation of all the tiles that are currently on the map, as well as the location of the player (blue), the current object to collect (green), and any enemies (red).
- Bottom right shows the currently selected weapon, how many shots are left in the current clip, and how many clips there are left.

Shifting Interface
------------------
- The map in the center shows the current layout of the map along with representations of the objects in it.
- The buttons around the map select a shift to perform.
- The "Add shift to queue" button adds the currently selected shift to the shift queue.
- When the shiting interface is toggled off (with SPACE), the shifts in the shift queue will be executed.

Compiling
=========
- Install [Visual Studio >= 2010](http://www.visualstudio.com/downloads/download-visual-studio-vs) with the [Visual C# toolchain](https://msdn.microsoft.com/en-us/library/kx37x362.aspx)
- Install [Microsoft XNA Game Studio 4.0](http://www.microsoft.com/en-ca/download/details.aspx?id=23714)
- Open the `ChaoticMind.sln` project and build it
