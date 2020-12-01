# TopDownShootem

# Description:
  - Our project is our take on a top down shooter where we attempted to create a game that was enjoyable for the average person stuck home during COVID. 
  - We created a fantasy map that is somewhat reminiscent of old pokemon games while adding a push to newer age with new looking character designs.
# How to use:
  - Build is located in assets simply click the build and the game will open
  
# Dependencies:
  - Our game uses Unity Hub 2.4.2 and Unity 2020.1.7f1
    - Both can be updated to newer versions of Unity however some items might become deprecated at that time
  - Mirror 26.2.2 (MMO Scale Networking Library)
    - Requires Unity 2018.4 LT, Runtime .Net 4.x and .Net 2.0
  - ParrelSync 1.4.1 (Test Multiplayer Gameplay)
    - Tested with 2020.1.2f1, 2019.3.0f6, 2018.4.22f1 but should work with others
  - TextMesh Pro 1.0.54 (Advanced Text Rendering)

# How it works:
  - Asset Folder contains all the scripts, scenes and prefabs we have created along with the addons and dependencies. 
  - Used KCP for networking
    - KCP is a fast and reliable protocol that can achieve the transmission effect of a reduction of the average latency by 30% to 40% and reduction of the maximum delay by a factor of three, at the cost of 10% to 20% more bandwidth wasted than TCP. It is implemented by using the pure algorithm, and is not responsible for the sending and receiving of the underlying protocol (such as UDP), requiring the users to define their own transmission mode for the underlying data packet, and provide it to KCP in the way of callback. Even the clock needs to be passed in from the outside, without any internal system calls. (According to https://github.com/skywind3000/kcp/blob/master/README.en.md)
    
# Contributions and Updates:
  - Matthew Levine: Created Home Screen, Map and Game Transitions
  - Dhruv Patel: Created Networking 
  - Zaire Wilson: Created Gameplay, Characters, Camera Movement and Animation Manager 
# Version:
  - 1.0
# Future Versions:
  - Future Vvrsions will include proper UI for players allowing for better interactions while playing
