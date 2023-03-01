# ParaGlassMenu
ParaGlassMenu is an AR software developed on OHMD (Nreal smartglasses) to enable users to conduct digital interactions subtly in conversations.

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
- [Runze Cai](http://runzecai.com)


## Project links
- Project folder: [here](project_link)
- Documentation: [here](guide_link)
- [Version info](VERSION.md)


## Requirements
- For the Python IoT Server, please install `Python 3.9.7`, then run `pip install -r requirement.txt` in the `ParaGlassMenu - Python IoT Server` folder.
- For the Unity Menu, please install `Unity 2019.4.39f1` first and prepare a Nreal light glasses.

## Installation
- For the Unity project, you can open and build it in Unity. Then please use adb install to install the apk file to the computer unit for Nreal glasses.
- Please to modify your code based on your real laptop's IP address in the Unity project: `ParaGlassMenu - Unity MenuAssets/NetMQExample/Scripts/HelloRequester.cs` 
and IoT devices' address and token in the python script: `ParaGlassMenu - Python IoT Server/unity_iot_control_sever.py`.


## References
- 



