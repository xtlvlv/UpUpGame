# 介绍
由 **上上签UpUpDraw** 实现的游戏开发框架，本项目的初版定位是利于新手使用和学习的游戏开发框架，框架的设计目标是简单易用，并在完善的过程中附上详细的视频讲解，同时也是一个完整的游戏开发框架，包含了游戏开发的常用功能，目前在完善中......
初版会放弃资源热更和资源管理，专注用于做Demo和独立单机游戏，但会保留热更和资源管理的接口，后续可以方便替换。

# 目录说明
Assets/Editor/UIExport.cs  ---ui快速绑定工具

Assets/Resourcecs/img  ---一些做demo用的UI资源
Assets/Resourcecs/lua_script  ---lua脚本根目录

Assets/Scripts/Core/Utils/ObjectExtension.cs  ---反射与json实现的深拷贝方法。
Assets/Scripts/Core/Lua/*  ---该目录下所有为lua相关c#代码

Scenes/LuaTest  ---xLua使用示例，可直接运行