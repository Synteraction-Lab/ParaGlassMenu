# ParaGlassMenu
ParaGlassMenu is an AR software developed for OHMD (Nreal smartglasses) that enables users to conduct digital interactions subtly using a ring mouse (sanwa) during conversations.

## Publications
- [ParaGlassMenu: Towards Social-Friendly Subtle Interactions in Conversations](https://doi.org/10.1145/3544548.3581065), CHI'2023
```
Runze Cai, Nuwan Janaka, Shengdong Zhao, and Minghui Sun. 2023. 
ParaGlassMenu: Towards Social-Friendly Subtle Interactions in Conversations. 
In Proceedings of the 2023 CHI Conference on Human Factors in Computing Systems (CHI ’23), 
April 23–28, 2023, Hamburg, Germany. ACM, New York, NY, USA, 21 pages. 
https://doi.org/10.1145/3544548.3581065
```

## Contact person
- [Runze Cai](http://runzecai.com): runze.cai [at] u.nus.edu


## Project links
- [Version info](VERSION.md)


## Requirements

1. To run the Python IoT Server, install `Python 3.9` and then run `pip install -r requirement.txt` in the `ParaGlassMenu - Python IoT Server` folder.
2. To use the Unity AR Menu, install `Unity 2019.4.39f1` and prepare Nreal Light glasses.
3. To use the ring mouse, connect it to the Nreal's Computing Unit and install the [Key Mapper](https://play.google.com/store/apps/details?id=io.github.sds100.keymapper&hl=en&gl=US&pli=1) APK file to the Computing Unit. Then modify the four buttons on the ring mouse to the `[up, down, right, left]` arrow keys.

## Installation

1. Modify the `address` and `tokens` in the code:
   1. Replace the computer's IP address (where the Python server code runs) in the Unity project: `ParaGlassMenu - Unity MenuAssets/NetMQExample/Scripts/HelloRequester.cs`.
   2. Replace the IoT devices' address and token in the Python script: `ParaGlassMenu - Python IoT Server/unity_iot_control_sever.py`.
      1. You can refer to this [link](https://github.com/jghaanstra/com.xiaomi-miio/blob/master/docs/obtain_token.md) to get the token of the Mi Home IoT devices.
      2. Download Hue on your smartphone to get the Philips Hue's IP address. [Note: The first connect may require you to click the button on the bridge first]
2. Open and build the Unity project in Unity's editor. Then, install the APK file of ParaGlassMenu to the Computing Unit for Nreal glasses using ADB.
3. To use the music player, create a folder `music` under `ParaGlassMenu - Python IoT Server` and add downloaded music to it.


