# 可行性报告

## 树莓派与Pixhawk间传输数据

### 使用器材

- Pixhawk飞控
- 树莓派3
- 运行win10的PC

### Pixhawk与树莓派接线

如图所示接线。将Pixhawk飞控上Telem2口的四根线引出：

- Telem2的5V连接树莓派的5V
- Telem2的TX连接树莓派的RX
- Telem2的RX连接树莓派的TX
- Telem2的GND连接树莓派的GND

![connect_pixhawk&RPi3B](http://ardupilot.org/dev/_images/RaspberryPi_Pixhawk_wiring1.jpg)

可以将与飞控接口适配的端子线与杜邦线焊接，或直接在端子线另一端连接杜邦头。

#### 树莓派供电问题

本打算直接从飞控处供电，但师兄说这样子的话供电有可能不稳定，所以打算直接从电池供电。

### 树莓派系统

树莓派有很多系统可以搭载，但是我们大多数别的系统对无人机配置都比较麻烦，所以我们选择了APSync。APSync对Pixhawk飞控进行了适配和优化，是专用于无人机机载电脑的Linux发行版本。APSync内置了许多实用功能，不必再对树莓派进行繁琐的配置，同时也不必再另购鼠标、键盘和显示器。

### 设置飞控参数

将飞控的端口使能、设置正确的波特率之后，才能够与树莓派建立通信。以下设置可以在**MissionPlanner->软件设置->全部参数设置**中完成。

```
# Telem2端口使能
SERIAL2_PROTOCOL = 1
# 设置端口波特率（921600）
SERIAL2_BAUD = 921

# 可选设置
# 在树莓派上创建DataFlash Log日志（该日志与飞控SD卡上记录的飞行日志相同）
# 设置后，可在/home/user/dflogger/dataflash下找到飞行日志
LOG_BACKEND_TYPE = 3123456789
```

设置完成后，需重启飞控使设置生效。

### 连接树莓派的无线网

APSync将树莓派板载的无线网卡作为无线热点使用，我们可以使用笔记本电脑的无线网卡连接到树莓派。之后，即可将飞控无线连接到地面站、通过树莓派访问互联网、通过SSH登录树莓派终端、或是使用FileZilla在本地和树莓派之间传输文件。

为树莓派和飞控上电。打开你笔记本电脑的无线网络配置，找到名为**ardupilot**的无线网络，使用**enRouteArduPilot**作为密码连接。

#### 无线连接到地面站

APSync为树莓派配置了数据转发服务，可以通过WiFi无线连接到飞控，使树莓派拥有数传的功能。

1. 打开MissionPlanner地面站，在右上角选择“UDP”，点击“Connect”
2. 在端口设置中，输入14550，确认

如下图所示

![MissionPlanner](https://github.com/OSH-2018/X-team/blob/master/feasibility_report/pic/RaspberryPi_MissionPlanner.jpg)

若连接成功，即可像USB连接、数传连接时一样，查看并配置飞控信息。

#### 使用FileZilla传输文件

FileZilla可以将PC本地编写的代码上传到树莓派，同时也可以从树莓派上下载飞行日志等数据文件。

1. 下载FileZilla并安装

2. 打开FileZilla，输入信息并登陆：

   - sftp://10.0.1.128
   - apsync
   - apsync

   ![FileZilla setup](https://img-blog.csdn.net/20170913170527865?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQvbGliZXJhdGV0aGV1cw==/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/SouthEast)

3. 可以像访问Windows下的文件夹一样，通过FileZilla访问树莓派的文件了

4. **注意当前用户是否具有读/写/执行的权限**！如果权限不足，FileZilla会提示“Permission Denied”


## Pixhawk操控无人机

### 使用器材

- Pixhawk飞控
- 四轴无人机

### Pixhawk飞控与无人机连线：

如图（待拍摄）

### 飞控的代码：

https://github.com/PX4/Firmware

### 使用协议：[MAVLink 2](https://mavlink.io/en/)

- MAVLink is a very lightweight, header-only message marshalling library for micro air vehicles / drones.

- dd

  - MAVLink is designed to support sending continuous telemetry streams including position, velocity, attitude and similar key states of a drone.

- Supported Languages

  - C
  - C++11

- [代码](https://github.com/mavlink/c_library_v2/tree/c55dd0ec312a667f28b0cab4f1fe697d2888c00c)可以直接在头文件调用

  - To use MAVLink, include the `mavlink.h` header file in your project:

    ```
    #include <mavlink/mavlink.h>
    ```

    If headers for multiple dialects and/or versions are installed, your include path might instead look similar to the following:

    ```
    #include <mavlink/v2.0/common/mavlink.h>
    ```

- Message Definitions

  - MAVLink messages are defined in XML files in the [mavlink/message definitions](https://github.com/mavlink/mavlink/blob/master/message_definitions/) folder. The messages that are common to all systems are defined in [common.xml](https://github.com/mavlink/mavlink/blob/master/message_definitions/v1.0/common.xml) (only messages contained in this file are considered standard messages). MAVLink protocol-specific messages and vendor-specific messages are stored in separate XML files.

## Unity 3D 通信部分

### Unity3D的特点

#### GameObject和Component

由于Unity是一个Component-Based的游戏引擎，所以游戏中所有的物体都是一个GameObject，为了给这个GameObject附加上各种各样的属性，所以我们引入了Component这个概念。

组件（component）附属于游戏物体.把一个 [Renderer ](http://game.ceeger.com/Script/Renderer/Renderer.html)(渲染器)组件附到游戏对象,可以使游戏对象显示到场景,附一个 [Camera ](http://game.ceeger.com/Script/Camera/Camera.html)(摄像机)可以把物体变成一个摄像机物体.所有脚本都是组件,因此都能附到游戏对象上.

常用的组件可以通过简单的成员变量取得:

附在游戏对象上的组件或脚本可以通过GetComponent获取.如下代码示例：

```
using UnityEngine;
using System.Collections;

public class example : MonoBehaviour {
    void Awake() {
        transform.Translate(0, 1, 0);
        GetComponent<Transform>().Translate(0, 1, 0);
    }
}
```

#### MonoBehaviour

GameObject是游戏中的基本物件。GameObject是由Component组合而成的，GameObject本身必须有Transform（**Position**, **Rotation**, and **Scale**）的Component，即GameObject是游戏场景中真实存在，而且有位置的一个物件。

但是我们怎么操纵这个GameObject呢？这就需要引入脚本组件了，也就是MonoBehaviour。

### StrangeIOC 框架

#### IOC：

控制反转（Inversion of Control，英文缩写为IoC）把创建对象的权利交给框架,是框架的重要特征

它包括依赖注入（Dependency Injection，简称[DI](https://baike.baidu.com/item/DI)）和依赖查找（Dependency Lookup）

可以把IoC模式看做是工厂模式的升华，可以把IoC看作是一个大工厂，只不过这个大工厂里要生成的对象都是在XML文件中给出定义的，然后利用Java 的“反射”编程，根据XML中给出的类名生成相应的对象。从实现来看，IoC是把以前在工厂方法里写死的对象生成代码，改变为由XML文件来定义，也就是把工厂和对象生成这两者独立分隔开来，目的就是提高灵活性和可维护性。（**摘自百度百科**）

我的理解是：把代码拆解，根据条件重新组合（注入），避免了耦合的代码执行不同操作的时候需要更改代码的情况，这依赖对不用代码块的查找。

一、介绍

　　Strange 是一个轻量的高扩展性的控制反转框架，专为C#和Unity而设计，它拥有如下特点，大部分特点是可选的：

　　一个核心的绑定系统，可以支持各种绑定（bindone or more of anything to one or more of anything else.）

依赖注入

1、映射为单例，值或者工厂（每次需要时创建一个新的实例）

2、命名注入

3、 构造函数注入或者setter注入（可以理解为属性注入）

4、标记指定的构造函数

5、标记指定函数在构造函数之后触发

6、注入到Monobehiavours

7、多态绑定：可以绑定接口或者实体类

　　两种风格的共享事件桥（Two styles of shared event bus）

1、都可以发送信号到程序的任何地方

2、都会为本地通信映射本地的事件桥

3、都会映射事件到相应的Command类来分离逻辑

4、使用新的信号实现增加了类型安全保证

　　MonoBehaviour 中介

1、帮助分离view与逻辑

2、隔离unity特有的代码与其它逻辑代码

　　可选的MVCS（Model/View/Controller/Service）结构

　　多个Context

1、允许子控件（子场景）单独运行，或者运行在主app之中

2、允许Context之间通信

　　扩展简单，可以自建新的绑定器

#### Unity串口通信

将Unity的.NET库从.NET 2.0 Subset改为.NET 2.0，原因是子集库太小了，不包含串口的类库。 
选择Edit->Project Settings -> Player->Api Compatibility Level 

引入类库：

```
using System.IO.Ports;
```

然后就可以在Unity的C# scripts中使用串口了

#### 初步构想

首先通过SerialPort（.NET Framework）的方式将无人机传输过来的数据传入Unity3D，然后解码，再把数据通过StrangeIoC提供的绑定方式绑定起来，利用绑定上来的数据，完成对Unity3D里面的GameObject的创建、运作方式调整等操作。





## 使用unity导入2D地图

#### 一、导入UIUG-RawImage

​	导入2D地图，我们不需要功能丰富的image，只需要轻量级的rawimage，RawImage只为我们提供了修改UV的方法，除此之外都是继承自MaskableGraphic的方法。

​	找到“hierarchy”界面，右键点击，选择“UI”（界面设计）、点击“rawimage”

#### 二、添加GoogleAPI

​	我们导入的是Google 2D地图，因此我们需要添加GoogleAPI。首先，我们需要给之前添加的rawimage添加一个组件（即component），在“inspecter里面”点击“add component”，命名“googleAPI”，即可添加元素

#### 三、使用C#修改Google api的script

​	修改后的代码如下：

```
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleApi : MonoBehaviour {

	public RawImage img;

	string url;

	public float lat;
	public float lon;

	LocationInfo li;

	public int zoom = 14;
	public int mapWidth =640;
	public int mapHeight = 640;

	public enum mapType {roadmap,satellite,hybrid,terrain}
	public mapType mapSelected;
	public int scale;


	IEnumerator Map()
	{
		url = "https://maps.googleapis.com/maps/api/staticmap?center=" + lat + "," + lon +
			"&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + scale 
			+"&maptype=" + mapSelected +
			"&markers=color:blue%7Clabel:S%7C40.702147,-74.015794&markers=color:green%7Clabel:G%7C40.711614,-74.012318&markers=color:red%7Clabel:C%7C40.718217,-73.998284&key=YourAPIKeyWillbeHere";
		WWW www = new WWW (url);
		yield return www;
		img.texture = www.texture;
		img.SetNativeSize ();

	}
	// Use this for initialization
	void Start () {
		img = gameObject.GetComponent<RawImage> ();
		StartCoroutine (Map());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
```

注意：

​	其中需要用自己获取的Google Maps API 来代替“YourAPIKeyWillbeHere”的位置

​	从 https://developers.google.com/maps/documentation/static-maps/intro?hl=en 网页的右上方“get a key”可以获得一个API，黏贴复制到代码相应位置即可

#### 四、显示Google Maps上面的位置

​	让我们进入Google Maps，搜索“中国科学技术大学 西校区”（https://www.google.com/maps/place/%E4%B8%AD%E5%9B%BD%E7%A7%91%E5%AD%A6%E6%8A%80%E6%9C%AF%E5%A4%A7%E5%AD%A6%E8%A5%BF%E6%A0%A1%E5%8C%BA/@31.8389495,117.2551922,17z/data=!4m5!3m4!1s0x35cb614f144990bf:0x1e10d2090ab9131c!8m2!3d31.838945!4d117.257303 ），这个时候我们可以得到地图上的一组坐标：31.838945, 117.257263，我们需要将数据输入unity中inspector模块中的Lat、Lon中去，调整好地图的大小，运行即可看到地图

参考文献

http://gad.qq.com/article/detail/19392

http://www.cnblogs.com/neverdie/category/584092.html

https://blog.csdn.net/ksgt00629518/article/details/53836570

https://blog.csdn.net/macroway/article/details/1353029

https://blog.csdn.net/u012632851/article/details/77008230

