# ParaGlassMenu
ParaGlassMenu is an AR software developed on OHMD (Nreal smartglasses) to enable users to conduct digital interactions subtly using a ring mouse (sanwa) in conversations.

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
- Project folder: [here](project_link)
- Documentation: [here](guide_link)
- [Version info](VERSION.md)


## Requirements

1. For the Python IoT Server, please install `Python 3.9`, and then run `pip install -r requirement.txt` in the `ParaGlassMenu - Python IoT Server` folder.
2. For the Unity Menu, please install `Unity 2019.4.39f1` first and prepare a Nreal Light glasses.
3. For the ring mouse usage, please connect it to the Nreal's Computing Unit and install the [Key Mapper](https://play.google.com/store/apps/details?id=io.github.sds100.keymapper&hl=en&gl=US&pli=1) apk file to the Computing Unit. Then modify the four buttons on the ring mouse to the `[up, down, right, left]` arrow keys.

## Installation

1. For the Unity project, you can open and build it in Unity. Then, use ADB to install the APK file to the Computing Unit for Nreal glasses.
2. Modify your code based on your real computer's IP address (where your run the python sever code) in the Unity project: `ParaGlassMenu - Unity MenuAssets/NetMQExample/Scripts/HelloRequester.cs` and IoT devices' address and token in the Python script: `ParaGlassMenu - Python IoT Server/unity_iot_control_sever.py`.
3. For the music player, please create a folder `music` under `ParaGlassMenu - Python IoT Server` and put your downloaded music in it.


