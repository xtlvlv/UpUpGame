# 介绍
- 由 **上上签UpUpDraw** 实现的游戏开发工具集，初版定位是利于新手使用和学习的游戏开发工具集，目标是简单易用，并在完善的过程中附上详细的视频讲解，
- 之后会一个类似《金铲铲之战》的自走棋游戏作为示例，来展示一个完整的游戏开发流程。

# 目录说明
- Assets/Editor/UIExport.cs  ---ui快速绑定工具

- Assets/Resourcecs/img  ---一些做demo用的UI资源
- Assets/Resourcecs/lua_script  ---lua脚本根目录

- Assets/Scripts/Core/Utils/ObjectExtension.cs  ---反射与json实现的深拷贝方法。
- Assets/Scripts/Core/Lua/*  ---该目录下所有为lua相关c#代码

- Scenes/LuaTest  ---xLua使用示例，可直接运行

- Analyzer/  ---代码分析器

- third/PythonTools/  ---python实现的一些工具
  - 1. AddNamespace.py  ---添加与修改C#代码的命名空间
    