# System Test Plan

Our testing plan will likely differ a bit from a typical software package. The user is never prompted to provide any data to be validated, and as such the input they provide is rather limited. Additionally, randomness is a major factor in many aspects of our game, making it more difficult to craft any sort of deterministic test for gameplay purposes. We also believe that graphical and audio glitches are more relevant for game testing, and these can be harder to automatically detect. As such, we have found that simply playtesting the game and using exploratory testing were invaluable in finding and fixing bugs. Still, we were able to also implement automatic testing for some cases.

Our code is tested using the Unity Test Framework, which consists of a Unity integration of the NUnit testing library. Additionally, GitHub Actions were added to automatically run all our tests and build the project whenever code is pushed. This makes it practically a trivial matter to run all our automatic tests regularly.

Finally, VS Code's C# extension was used to format code regularly. This helped keep code looking uniform throughout the codebase.