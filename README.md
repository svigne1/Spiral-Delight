# Spiral-Delight

This is a simple Match-3 game like Candy-Crush written in Unity C#

The reason the gameplay in Spiral-Delight is different from Candy-Crush comes from the fact that the gems are being arranged in a circle rather than a rectangle.

Hence the gems so placed don't form rows and columns like in the case of Candy-Crush, instead they form concentric rings.
As a result, the matches are not calculated along X and Y axis but along the radius and circumference of the board - The circle that contains all the gameObjects (gems) in the scene.

The gameplay doesn't offer the ability to swap gems, but instead offers the ability to rotate individual rings.
Player is expected to match all gems and complete the game without any single gem remaining on the board.

# Current State
* Mesh creation completed in Blender. Can be found in Assets/elements.blend
* Gems are being populated into the scene at runtime. Check PartPopulator.cs attached to gameObject board in the scene.
* Every gem has 4 box colliders attached to it.

# ToDo
* Since new gems are not populated into the scene during gameplay. There is a decent chance that the player may find himself in a stalemate situation often times where valid moves are no longer available to complete the game. Need to come up with a solution for this.
* Try to avoid prepopulating the board with matches at the start of the game.
* Identifying Matches and disappearing them.
* Enabling rotation to the rings. Rotations cannot be continuous. It will destroy the ability to match gems.Hence they need to be discrete rotations.
