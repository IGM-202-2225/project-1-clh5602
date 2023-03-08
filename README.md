# Project Prism Planet

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

### Student Info

-   Name: Colby Heaton
-   Section: ##202.01

## Game Design

-   Camera Orientation: Side
-   Camera Movement: Camera will pan ahead the player in the direction they're facing.
-   Player Health: Time. Time constantly decreases, and it decreases even more when hit! Defeat enemies to regain time.
-   End Condition: A round ends once all enemies on the map are defeated. The game has infinite rounds, but the game will end once Time is up.
-   Scoring: The player earns points by defeating enemies. A combo meter will increase the amount of points gained.

### Game Description

Gameplay will by similar to that of _Defender_, in the sense that levels will be looping arenas where you must defeat each enemy to progress.
The player can fly around the looping terrain while utilizing two different types of projectiles to clear waves of foes. Players will aim for a high-score
by defeating enemies at a rapid pace. A combo system encourages players to act fast and reckless in order to capitalize on the point bonus.

### Controls

-   Movement
    -   Up: up arrow, increase altitude
    -   Down: down arrow, decrease altitude
    -   Left: left arrow, turn left, increase leftward speed
    -   Right: right arrow, turn right, increase rightward speed
-   Fire: Z Key fires forward shots.

## You Additions

The combo system will be interesting to implement. Having a combo meter in addition to a time limit incentivises reckless play, which is good for a risk/reward feeling. I plan to have at least two enemy types that move and act completely different from each other. I created every art asset in this game, except for the font. Implementing a map was difficult, yet a necessity to make the game fun to play. Although the procedural generation is nothing fancy, it randomly places and creates enemies around the map, and they'll always start offscreen as to be fair to the player.

## Sources

Font "Alarm Clock" by David J Patterson on dafont.com

## Known Issues

Can't think of any outstanding issues.

### Requirements not completed

It's debatable whether I properly used velocity vectors for the player, enemies, and bullets. In fact, I didn't really: 
- The player has seperate x and y velocity vectors - I did this so that horizontal movement was seperate from ascending / descending. I thought that if the player was moving forward at fullspeed, then moved upward, it would feel jagged or discombobulating when the ship would slow down (due to normalized vectors). 

- Player bullets only moved horizontally, so I made the code easier on myself by having a single float dictate the laser's velocity. 

- The first enemy variant moves on a Sin wave pattern with its amplitude, speed, and wavelength being randomly generated. I did not really see the need to use velocity vectors here, and I didn't want to compromise my vision.

- The second enemy variant is stationary, but its bullets DO properly use direction and velocity vectors, so that's cool.

So yeah, I believe I'm going to get partial credit here, but it's all good. I'm happy with what I made, so I'm not too worried about the grade :)

