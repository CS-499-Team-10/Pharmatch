# Software Quality Plan

## System Test Plan

Our testing plan will likely differ a bit from a typical software package. The user is never prompted to provide any data to be validated, and as such the input they provide is rather limited. Additionally, randomness is a major factor in many aspects of our game, making it more difficult to craft any sort of deterministic test for gameplay purposes. We also believe that graphical and audio glitches are more relevant for game testing, and these can be harder to automatically detect. As such, we have found that simply playtesting the game and using exploratory testing were invaluable in finding and fixing bugs. Still, we were able to also implement automatic testing for some cases.

Our automatic tests use the Unity Test Framework, which consists of a Unity integration of the NUnit testing library. Additionally, GitHub Actions were added to automatically run all our tests and build the project whenever code is pushed. This makes it practically a trivial matter to run all our automatic tests regularly.

Generally speaking, gameplay changes to our three game modes are the highest priority to be tested, since this is where the user is expected to spend the most time and is the core of the project. However, the priority might shift depending on what files exactly was changed - For example, if only our "Time Attack" game mode's controller script was edited, then we'll focus on testing that game mode only. However, if the "SceneController" script that all three game modes' controllers inherit from is changed, then all game modes should be tested.

The next priority are User Interface elements, such as the the main menu through which the player may select a game mode, or the pause menu through which the user can mute/unmute sounds and music. Since these elements typically have deterministic outcomes, we were able to write automated tests for them more easily, though we also did manual testing.

A bit lower priority is testing the features that are nice to have, but not considered a core component, such as the "Hint Mode" we implemented. These are features we value, but if we are unable to get them perfectly working we are confident the project will still perform competently.

## Version Control

We used git and GitHub for version control throughout the project. This helped us roll back software changes that ended up being undesirable, and allowed multiple people to work on different aspects of the program independently.

## Pair Programming and Reviews

We found pair programming very helpful in hashing out the core mechanics of our game, such as the basic matching function that is essential to the core game. After we got the basics hammered out, we switched to implementing features individually and later being reviewed. 

## Coding Standards

VS Code's C# extension was used to format code regularly. This helped keep code looking uniform throughout the codebase.

## Prototyping and Sponsor Feedback

We were able to quickly hack together a few prototypes to make sure the core mechanics of our game were fun and functional. By presenting these to our sponsor every sprint, we could discuss changes to the game and get feedback on what we had implemented. This helped us stay focused on making improvements that they saw as most important and change what they weren't so fond of.