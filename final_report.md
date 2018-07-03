# Virtual Drone详细设计报告

## 零、组员分工

**查找资料**：徐亦舜、祝冠琪、赵瑞、邵军阳

**PPT制作**：祝冠琪、赵瑞、邵军阳

**无人机通信**：祝冠琪

**无人机安装**：祝冠琪，赵瑞

**Unity地面站**：邵军阳  

## 一、背景介绍

### 1、无人机

**无人机：**无人驾驶飞机的简称，缩写为“UAV”。是一种无线遥控的无人驾驶飞机。2016年无人机作为消费电子类的重点戏迅速点燃了整个消费市场,一时间家喻户晓,在引起消费者狂热追捧的同时,国内外的厂商也前赴后继地杀入无人机市场,力求在无人机市场占有自己的一席之地。

### 2、树莓派

**树莓派**（英语：**Raspberry Pi**），是一款基于[Linux](https://zh.wikipedia.org/wiki/Linux)的[单片机](https://zh.wikipedia.org/wiki/%E5%8D%95%E7%89%87%E6%9C%BA)电脑。它由[英国](https://zh.wikipedia.org/wiki/%E8%8B%B1%E5%9C%8B)的树莓派基金会所开发，目的是以低价[硬件](https://zh.wikipedia.org/wiki/%E7%A1%AC%E4%BB%B6)及[自由软件](https://zh.wikipedia.org/wiki/%E8%87%AA%E7%94%B1%E8%BB%9F%E9%AB%94)促进学校的基本[计算机科学](https://zh.wikipedia.org/wiki/%E7%94%B5%E8%84%91%E7%A7%91%E5%AD%A6)教育。树莓派配备一枚[博通](https://zh.wikipedia.org/wiki/%E5%8D%9A%E9%80%9A)（Broadcom）出产的[ARM架构](https://zh.wikipedia.org/wiki/ARM%E6%9E%B6%E6%A7%8B)700MHz BCM2835处理器，256MB[內存](https://zh.wikipedia.org/wiki/%E5%85%A7%E5%AD%98)（B型已升级到512MB内存），使用[SD卡](https://zh.wikipedia.org/wiki/SD%E5%8D%A1)当作存储媒体，且拥有一个[Ethernet](https://zh.wikipedia.org/wiki/Ethernet)、两个[USB接口](https://zh.wikipedia.org/wiki/USB%E6%8E%A5%E5%8F%A3)、以及[HDMI](https://zh.wikipedia.org/wiki/HDMI)（支持声音输出）和[RCA端子](https://zh.wikipedia.org/wiki/RCA%E7%AB%AF%E5%AD%90)输出支持。树莓派面积只有一张信用卡大小，体积大概是一个火柴盒大小，可以运行像《[雷神之锤III竞技场](https://zh.wikipedia.org/wiki/%E9%9B%B7%E7%A5%9E%E4%B9%8B%E9%94%A4III%E7%AB%9E%E6%8A%80%E5%9C%BA)》的游戏和进行[1080p](https://zh.wikipedia.org/wiki/1080p)视频的播放。操作系统采用开源的[Linux](https://zh.wikipedia.org/wiki/Linux)系统：[Debian](https://zh.wikipedia.org/wiki/Debian)、[ArchLinux](https://zh.wikipedia.org/wiki/ArchLinux)，自带的[Iceweasel](https://zh.wikipedia.org/wiki/Iceweasel)、[KOffice](https://zh.wikipedia.org/wiki/KOffice)等软件，能够满足基本的网络浏览、文字处理以及电脑学习的需要。

### 3、Unity

**Unity3D**是由Unity Technologies开发的一个让玩家轻松创建诸如三维视频游戏、建筑可视化、实时三维动画等类型互动内容的多平台的综合型游戏开发工具，是一个全面整合的专业游戏引擎。Unity类似于Director,Blender game engine, Virtools 或 Torque Game Builder等利用交互的图型化开发环境为首要方式的软件。其编辑器运行在Windows 和Mac OS X下，可发布游戏至Windows、Mac、Wii、iPhone、WebGL（需要HTML5）、Windows phone 8和Android平台。也可以利用Unity web player插件发布网页游戏，支持Mac和Windows的网页浏览。它的网页播放器也被Mac 所支持。



## 二、总体结构

![](report_pics\1.png)

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

结果如下

![](report_pics\mavproxy.png)

可以看见，成功连接上了pixhwak，并且无人机现处于MANUAL状态。如图输入mode命令可以得到所有可用的状态。输入`param show`可以得到无人机的参数

![](report_pics\params.png)

为了方便起见，我们讲树莓派设置成了开机自动启动连接命令，通过编辑**/etc/rc.local**来实现

```
(
date
echo $PATH
PATH=$PATH:/bin:/sbin:/usr/bin:/usr/local/bin
export PATH
cd /home/pi
screen -d -m -s /bin/bash mavproxy.py --master=/dev/ttyS0 --baudrate 57600 --aircraft MyCopter
) > /tmp/rc.log 2>&1
exit 0
```

开机后使用`sudo screen -x`便可以查看

每一次连接，在**/home/pi/MyCopter/logs/YYYY-MM-DD directory**以下文件都会被创建

- **mav.parm** : 含有从pixhawk中读取出来的所有参数
- **flight.tlog** : 包含无人机高度，姿态等的遥测日志，可以使用一些地面站工具打开 
- **flight.tlog.raw** :上面提到的.tlog中的所有数据加上从Pixhawk收到的任何其他串行数据，可能包括非MAVLink格式的消息，如启动字符串或调试输出 

然后我们便可以在**mavproxy**中调用dronekit，输入如下语句

```
api start drone_final.py
```

就可以使用dronekit来读取和设置无人机的相关参数

#### b.串口通信

因为我们无人机与地面站之间采用的是串口通信，所以使用了如下代码

```
def Read_Data_From_Pixhawk(vehicle):
    temp = vehicle.location.global_frame
    Read_Data = ''
    j = 0
    cnt = 0
    for i in range(len(data)):
        if data[i] == ',':
            Read_Data += data[j : i]
            if cnt == 0:
                Read_Data += 'lon'
                cnt += 1
            elif cnt == 1:
                Read_Data += 'lat'
                cnt += 1
            j = i + 1
        elif i == len(data) - 1:
            Read_Data += data[j : i]
            Read_Data += 'al'
    return velocity

def Send_To_Groundcontrol(vehicle, ser):
    while True:
        Read_data = Read_Data_From_Pixhawk(vehicle)
        ser.write(Read_data)
        sleep(0.02) 
        
def recv(serial):  
    data =serial.readline()     
    return data  

def Read_From_Groundcontrol(vehicle, ser):
    while True:
        data = recv(ser)
        send_nav_velocity(vehicle, data)
        sleep(0.02)
```

其中**vehicle**是通过

```
vehicle = connect(connection_string,baud = baud_rate, wait_ready=True) 
```

而得到的一个类，这是dronekit与pixhawk通信的语句。里面含有无人机的各个参数，比如无人机的gps信息，高度，俯仰角等

**ser**是通过

```
ser = serial.Serial('/dev/ttyUSB0', 115200, timeout=0.5,bytesize=8,parity=serial.PARITY_NONE,stopbits=1)
```

打开的树莓派串口，通过这个能让树莓派与地面站通信。

##### 树莓派与飞控间串口通信

对于读取地面站发来的信号我们采用如下代码：

```
def recv(serial):  
    data =serial.readline()     
    return data  

def Read_From_Groundcontrol(vehicle, ser):
    while True:
        data = recv(ser)
        send_nav_velocity(vehicle, data)
        sleep(0.02)
```

其中**recv**函数是从串口中每0.02秒读取地面站发来的信息，然后传送给**send_nav_velocity**来设置无人机的速度。

##### 树莓派与地面站间串口通信

```
def Read_Data_From_Pixhawk(vehicle):
    temp = vehicle.location.global_frame
    Read_Data = ''
    j = 0
    cnt = 0
    for i in range(len(data)):
        if data[i] == ',':
            Read_Data += data[j : i]
            if cnt == 0:
                Read_Data += 'lon'
                cnt += 1
            elif cnt == 1:
                Read_Data += 'lat'
                cnt += 1
            j = i + 1
        elif i == len(data) - 1:
            Read_Data += data[j : i]
            Read_Data += 'alt'
    return Read_Data

def Send_To_Groundcontrol(vehicle, ser):
    while True:
        Read_data = Read_Data_From_Pixhawk(vehicle)
        ser.write(Read_data)
        sleep(0.02) 
```

先通过**Read_Data_From_Pixhawk(vehicle)**函数读取无人机的参数，其中`vehicle.location.global_frame`是全球定位信息（经纬度，高度相对于平均海平面）。读取出来以后，将它转换成**x+'lon'+y+'lat'+z+'alt'** 的格式，便于地面站接收。

#### c、设置无人机速度

```
def send_nav_velocity(self, data):
    # 生成SET_POSITION_TARGET_LOCAL_NED命令
    velocity = changedata(data) 
    msg = self.vehicle.message_factory.set_position_target_local_ned_encode(
                    0,       # time_boot_ms (not used)
                    0, 0,    # target system, target component
                    mavutil.mavlink.MAV_FRAME_BODY_NED,   # frame
                    0b0000111111000111, # type_mask (only speeds enabled)
                    0, 0, 0, # x, y, z positions (not used)
                    velocity[0], velocity[1], velocity[2], # x, y, z velocity in m/s
                    0, 0, 0, # x, y, z acceleration (not used)
                    0, 0)    # yaw, yaw_rate (not used) 
    # 发送指令
    self.vehicle.send_mavlink(msg)
    self.vehicle.flush()



def changedata(data):
    velocity = []
    j = 0
    for i in range(len(data)):
        if data[i] == ',':
            velocity.append(float(data[j : i]))
            j = i + 1
        elif i == len(data) - 1:
            velocity.append(float(data[j:i]))
    sum = (velocity[0]**2 + velocity[1]**2 + velocity[2]**2)**0.5
    for i in range(len(velocity)):
        velocity[i] = velocity / sum
    if data == '':
        velocity = [0, 0, 0]
    return velocity
```

我们使用**send_nav_velocity**函数来设置无人机速度，但因为传送过来的是字符串，所以我们先用**changedata**来得到可用的数据格式。为了简化操作，我们将最后得到的速度归一化。当我们遇到没有接收到地面站信号时，我们采用让飞机悬停的方式，即速度变为[0，0，0]。

最后因为我们树莓派与地面站之间的串口是全双工的，所以我们开两个线程

```
threads = []
t1 = threading.Thread(target=Read_From_Groundcontrol,args=(vehicle,ser))
threads.append(t1)
t2 = threading.Thread(target=Send_To_Groundcontrol,args=(vehicle,ser))
threads.append(t2)
```

这样，便可以同时进行读和写了

### 2、Unity地面站

#### 框架：StrangeIOC

![](report_pics/5.png)



我在这个项目开始的时候得到了助教的一份代码，进而了解到软件工程的一些概念，包括控制反转和MVCS设计模式。抱着循序学习的态度，我认真学习了MVCS框架和控制反转的概念，并按照这个框架进行我的设计。

##### 通信部分

串口设置：

![](report_pics/4.png)

此图中的serialport将在startcommand中初始化。

![](report_pics/2.png)

##### 其他部分

由于其他部分助教的代码已经实现得很完善（实际上我们的工作是尝试去打通这个模型的通信，所以我们的重点应该是通信），所以我没有进行大幅度的修改。但是我修正了一个错误：

原代码跑着跑着会跳出错误: Object is destroyed but you're still trying to access it.

然后我发现这段代码会直接调用tile.GetComponent()而不做异常检测，所以我加了个判断。

![](report_pics/3.png)

#### 代码

##### startcommand：（程序初始化的时候创建串口用）

```
class StartCommCommand2 : Command
    {
    	[inject]
        public serialp Serialp {get;set;}
         public override void Execute()
        {
        	Serialp.init();
        }
        private void dispose()
        {
            Serialp.dispose();
        }
	}
```



##### 信号量：（另一个信号量类似）

```
using System;
using strange.extensions.signal.impl;

namespace airplanegame
{
    public class MySignalSend : Signal<string>
    {
    }
}
```

##### context：

```
injectionBinder.Bind<serialp>().ToSingleton();
injectionBinder.Bind<MySignalSend>().ToSingleton();
injectionBinder.Bind<MySignalWrite>().ToSingleton();
```

##### serialport：

```
public class serialp
    {
        [Inject]
        public MySignalSend mySignalSend { get; set; }

        [Inject]
        public MySignalWrite mySignalWrite { get; set; }

        public bool _continue;
        private SerialPort comm;
        private Thread readThread;
        private bool isRead = true;
        private Queue<string> lonq=new Queue<string>();
        private Queue<string> latq=new Queue<string>();
        private Queue<string> altq=new Queue<string>();
        public string lastlon ="121";
        public string lastlat = "41";
        public string lastalt = "1300";
        private int read2 = new int();

        public void serialread()
        {
            lonq.Enqueue(lastlon);
            latq.Enqueue(lastlat);
            altq.Enqueue(lastalt);
            while (_continue)
            {
                try
                {
                    if (comm.IsOpen)
                    {
                        lastlon = comm.ReadTo("lon");
                        lastlat = comm.ReadTo("lat");
                        lastalt = comm.ReadTo("alt");
                        lonq.Enqueue(lastlon);
                        latq.Enqueue(lastlat);
                        altq.Enqueue(lastalt);
                        mySignalWrite.Dispatch(lastlon + "lon" + lastlat + "lat" + lastalt + "alt");
                        Thread.Sleep(100);
                    }
                }
                catch (TimeoutException)
                {
                }
            }
        }
        public void serialwrite(string message)
        {
            if (message.Equals("stop"))
            {
                _continue = false;
                readThread.Join();
                comm.Close();
                comm.Dispose();
                return;
            }
            if (comm.IsOpen)
            {
                try
                {
                    comm.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }
        public bool init()
        {
            comm = new SerialPort();
            comm.PortName = "COM3";
            comm.BaudRate = 115200;
            comm.DataBits = 8;
            comm.Parity = Parity.None;
            comm.StopBits = StopBits.One;
            comm.ReadTimeout = 500;
            comm.WriteTimeout = 500;
            mySignalWrite.AddListener(serialwrite);
            _continue = true;
            try
            {
                if (!comm.IsOpen)
                {
                    comm.Open();
                    isRead = true;
                }
            }
            catch (Exception ex)
            {
                disposeserial();
                return false;
            }
            readThread = new Thread(serialread);
            readThread.Start();
            return true;
        }
        public void disposeserial()
        {
            comm.Dispose();
            return;
        }
    }
```

##### AirplanePrefabMediator：（只选出了展现我实现的功能的部分）

```
public class AirplanePrefabMediator : Mediator
    {
        [Inject]
        public UpdateAirplaneSignal updateSignal { get; set; }

        [Inject]
        public AirplanePrefabView view { get; set; }
        
        public override void OnRegister()
        {
            base.OnRegister();
            mySignalSend.AddListener(updateairplane2);
        }
        public void updateairplane2(string s)
        {
            string[] sarray = s.Split(new string[]{ "lon", "lat","alt" },StringSplitOptions.RemoveEmptyEntries);
            float lon = new float();
            float lat = new float();
            float alt = new float();
            try
            {
                float.TryParse(sarray[0], out lon);
                float.TryParse(sarray[1], out lat);
                float.TryParse(sarray[2], out alt);
                view.lon = lon;
                view.lat = lat;
                view.alt = alt;
            }
            catch (Exception ex) { }
        }
        public void Update()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            bool up = Input.GetKeyDown(KeyCode.A);
            bool down = Input.GetKeyDown(KeyCode.S);
            string z;
            if (up) z = "1";
            else if (down) z = "-1";
            else z = "0";
            mySignalWrite.Dispatch(string.Format("{0},{1},{2}", x.ToString(), y.ToString(), z.ToString()));
        }
```

#### 实现效果

我们在树莓派上实现了一个串口，它不断地发送自减的longitude和固定的latitude，altitude过来，我在unity里把这些参数print到console中。

##### 笔记本视角

![](report_pics/6.png)

可见程序正确地收到了数据

##### 树莓派视角

![](report_pics/7.png)

由于我在unity的update()函数中调度串口写，所以发送的频率是很高的，这是我按下一些键后树莓派收到的矢量值（以字符串形式表示）。在我不按键的时候是0,0,0。

## 四、未来展望



## 五、学习感想

这一次的项目我们经历了很多也收获了很多。

这一次的项目，总体来说做得不够顺利。我们也反思总结了很多，大体上的原因可以分为：

- 组员之间沟通不足：

  我们组会是每周按时开的，但是每次组会都没有会议总结，导致组员开完会之后对自己的任务不够清晰。

- 组员分工不够精细，存在工作重复冲突

- 个别组员划水

- 组员的危机感不够强烈

- 学习的方式不够灵活，没有learn by doing

- 

