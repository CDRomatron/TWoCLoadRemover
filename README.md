# TWoCLoadRemover
A video based load remover for Crash Bandicoot: The Wrath of Cortex

## How to use
Download the latest release from https://github.com/CDRomatron/TWoCLoadRemover/releases

In your Livesplit folder, create a folder named TWoC and paste the .exe and the two .asl files in there.

IMPORTANT FOR SPEEDRUN SUBMISSIONS: You will need to manually re-add the time spent in the credits during 106% as the timer will stay paused during this.

### Configuring

It is highly recommended to use the [TWoC Wizard](https://github.com/CDRomatron/TWoCWizard) to initially configure the load remover, not this application, due to the complexity of it.

If you do need to use the TWoC load remover application to configure, it is similar in function to the [CTTR Load Remover](https://github.com/CDRomatron/CTTRLoadRemover). The main difference is that the TWoC load remover looks for 3 (ps2)/4(xbox) source images. X, Y, W, and H decide the top left coordinates of the full capture as well as the width and the height. The height and width of the wumpa fruit are represented by W1 and H1 (how far from top left corner), top loading is represented by W2 and H1 (how far from top right corner), and blackscreen and midload (xbox) are represented by W2 and H2 (how far from bottom right corner).

As this application is currently in a prerelease state, this mostly exists as a debugging tool for the asl script.

### Running
Now, start Livesplit, go to Edit Layout, and add a "Scriptable Autosplitter". Select either TWoC Xbox.asl or TWoC PS2.asl. As long as you have clicked Save Settings in the above step, this should now pause the time during load screens. Please remember to swap your comparision to game time, as it does not affect real time.

## Building from source
Clone the solution, and build. This application does not currently have any dependancies. The files TWoC Xbox.asl and TWoC PS2.asl are not present in the solution, and can be obtained the most recent release.

## Contact
If you are having issues with this application, have questions, or want to suggest improvements, please contact me on [Twitter @CDRomatron](https://twitter.com/CDRomatron), on my [Discord](https://discord.gg/AWam7rq), on [Speedrun.com @CDRomatron](https://www.speedrun.com/CDRomatron), or leave an issue on the project issue tracker.
