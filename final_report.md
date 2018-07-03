# Virtual Drone详细设计报告

## 一、背景介绍

### 1、无人机

**无人机：**无人驾驶飞机的简称，缩写为“UAV”。是一种无线遥控的无人驾驶飞机。2016年无人机作为消费电子类的重点戏迅速点燃了整个消费市场,一时间家喻户晓,在引起消费者狂热追捧的同时,国内外的厂商也前赴后继地杀入无人机市场,力求在无人机市场占有自己的一席之地。

### 2、树莓派

**树莓派**（英语：**Raspberry Pi**），是一款基于[Linux](https://zh.wikipedia.org/wiki/Linux)的[单片机](https://zh.wikipedia.org/wiki/%E5%8D%95%E7%89%87%E6%9C%BA)电脑。它由[英国](https://zh.wikipedia.org/wiki/%E8%8B%B1%E5%9C%8B)的树莓派基金会所开发，目的是以低价[硬件](https://zh.wikipedia.org/wiki/%E7%A1%AC%E4%BB%B6)及[自由软件](https://zh.wikipedia.org/wiki/%E8%87%AA%E7%94%B1%E8%BB%9F%E9%AB%94)促进学校的基本[计算机科学](https://zh.wikipedia.org/wiki/%E7%94%B5%E8%84%91%E7%A7%91%E5%AD%A6)教育。树莓派配备一枚[博通](https://zh.wikipedia.org/wiki/%E5%8D%9A%E9%80%9A)（Broadcom）出产的[ARM架构](https://zh.wikipedia.org/wiki/ARM%E6%9E%B6%E6%A7%8B)700MHz BCM2835处理器，256MB[內存](https://zh.wikipedia.org/wiki/%E5%85%A7%E5%AD%98)（B型已升级到512MB内存），使用[SD卡](https://zh.wikipedia.org/wiki/SD%E5%8D%A1)当作存储媒体，且拥有一个[Ethernet](https://zh.wikipedia.org/wiki/Ethernet)、两个[USB接口](https://zh.wikipedia.org/wiki/USB%E6%8E%A5%E5%8F%A3)、以及[HDMI](https://zh.wikipedia.org/wiki/HDMI)（支持声音输出）和[RCA端子](https://zh.wikipedia.org/wiki/RCA%E7%AB%AF%E5%AD%90)输出支持。树莓派面积只有一张信用卡大小，体积大概是一个火柴盒大小，可以运行像《[雷神之锤III竞技场](https://zh.wikipedia.org/wiki/%E9%9B%B7%E7%A5%9E%E4%B9%8B%E9%94%A4III%E7%AB%9E%E6%8A%80%E5%9C%BA)》的游戏和进行[1080p](https://zh.wikipedia.org/wiki/1080p)视频的播放。操作系统采用开源的[Linux](https://zh.wikipedia.org/wiki/Linux)系统：[Debian](https://zh.wikipedia.org/wiki/Debian)、[ArchLinux](https://zh.wikipedia.org/wiki/ArchLinux)，自带的[Iceweasel](https://zh.wikipedia.org/wiki/Iceweasel)、[KOffice](https://zh.wikipedia.org/wiki/KOffice)等软件，能够满足基本的网络浏览、文字处理以及电脑学习的需要。

### 3、Unity

**Unity3D**是由Unity Technologies开发的一个让玩家轻松创建诸如三维视频游戏、建筑可视化、实时三维动画等类型互动内容的多平台的综合型游戏开发工具，是一个全面整合的专业游戏引擎。Unity类似于Director,Blender game engine, Virtools 或 Torque Game Builder等利用交互的图型化开发环境为首要方式的软件。其编辑器运行在Windows 和Mac OS X下，可发布游戏至Windows、Mac、Wii、iPhone、WebGL（需要HTML5）、Windows phone 8和Android平台。也可以利用Unity web player插件发布网页游戏，支持Mac和Windows的网页浏览。它的网页播放器也被Mac 所支持。



## 二、总体结构



## 三、具体实现

### 0、无人机的安装

因为没有现成的无人机，我们首先进行了无人机的安装，安装完如下图

![](C:\Users\guanq\git\X-team\report_pics\drone.JPG)

接下来进行了无人机的试飞，在这一部分我们花费了很长的时间。因为网上并没有完备的QgroundControl使用资料，所以有两个不怎么注意得到的参数设置错误我们也没注意到，导致无人机一直无法正常飞行。

### 1、无人机与树莓派

#### a.树莓派与飞控的连接

首先是安装树莓派的系统，这里我们选择了 RASPBIAN STRETCH WITH DESKTOP 来作为我们的系统。

这里我们通过串口连接的方式连接两者。如下图

![](report_pics\RaspberryPi_Pixhawk_wiring1.jpg)

通过飞控的Telem2端口与树莓派相连

接下来我们安装了两个库分别是`mavproxy`和`dronekit`以及使用它们所需的库

在上面运行`sudo raspi-config`设置树莓派的serial，disable `serial login shell`，activate `serial interface`

运行如下命令连接飞控

```
sudo mavproxy.py --master=/dev/ttyS0 --baudrate 57600 --aircraft MyCopter
```





### 2、Unity地面站

## 四、未来展望

## 五、学习感想