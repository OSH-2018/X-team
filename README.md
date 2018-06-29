# Unity3D部分

前言：因为我们已经得到了助教的一份比较完整的代码，我们想重点实现传输部分，所以我们不想破坏地图的生成和飞机视角，摄像机视角的调整等需要大量数学知识的部分，这样会让事情变得更加复杂，所以我们学习了助教使用的设计模式，按照他们的设计模式来进行我们的作业。

## 

在这次大作业中，我们学会了设计一个大型的程序的时候的一种设计模式：

IOC（Inversion Of Control）模式

![img](http://p.blog.csdn.net/images/p_blog_csdn_net/wanghao72214/EntryImages/20090308/%E9%BD%BF%E8%BD%AE_%E8%80%A6%E5%90%88%E5%85%B3%E7%B3%BB_full.jpg)

![img](http://p.blog.csdn.net/images/p_blog_csdn_net/wanghao72214/EntryImages/20090308/%E4%B9%B1%E9%BA%BB_%E8%80%A6%E5%90%88%E5%85%B3%E7%B3%BB.JPG)

设计程序的时候我们会发现，当我们创建一个类的时候，其中很可能声明了另一个具体的类的实例，或者使用了另一个模块，则我们修改某个类（模块）的时候，另一个类（模块）也会发生变化，也就是各个类之间产生了依赖关系，这时候就会让debug变得很痛苦。

![img](http://p.blog.csdn.net/images/p_blog_csdn_net/wanghao72214/EntryImages/20090308/%E9%BD%BF%E8%BD%AE_%E8%A7%A3%E8%80%A6%E5%90%88_full.jpg)

控制反转的设计思想是让各个部件解耦，通过一个IoC容器让耦合在一起的部件不在和彼此联系，而一起与一个第三方产生联系。这就可以让你在IoC容器中注入依赖关系，使得依赖关系这个概念独立出来，成为你可以操作的一个对象。

## 

StrangeIoC是一个设计为Unity3D上使用的IOC框架

![MVCSContext Architecture](http://strangeioc.github.io/strangeioc/class-flow.png)

核心是绑定（Binder），按照作者的说法，我们可以StrangeIoC的确是一个IoC框架，但是它的核心是绑定。

实际上，StrangeIoC提供以下绑定类型：

注入（injection）绑定，反射（reflector）绑定，调度器（dispatcher）绑定

命令（command）绑定，信号（signal）绑定，中介（mediation）绑定，上下文（context）绑定

依赖注入的绑定仅仅是其中一条，除了其IoC特性之外，其他的绑定也有很重要的功能。

例如signal绑定：

 `commandBinder.Bind<StartSignal>().To<StartCommand>().Once();`

这个代码将一个command绑定到一个signal，则signal被dispatch的时候，command被调用，形成一种类似进程间通信的效果（我感觉像是fork之后exec）。

不过在我们的代码中主要是这样使用signal：

```
Xsignal.Addlistener(method_name);
```

这样子Xsignal.dispatch之后，method_name()会被立即调用。

## 

分析我们大作业的需求，我们的重点是上图中的Service，View

## 通信部分

我们选择无线串口通信（简单易实现，而且比较稳定）

### 读信号：

#### Service实现串口：

我们使用多个线程来实现，这样程序各部分才能并行：

读串口：

```
 public class SerialPortService : ICommService
    {
        [Inject]
        public MsgSignal msgSignal { get; set; }

        [Inject]
        public UpdateAirplaneSignal updateSignal { get; set; }

        private SerialPortRead comm;
        Thread readThread = new Thread(Read);
        ……
        //线程读数据，把数据记录到dataframe
        ……
        updateSignal.Dispatch(dataframe);//把dataframe用信号发出去，触发View那边的响应
        ……
   }
```

#### View得到串口传来的信号：

首先将startcomsignal2（开启程序的时候会执行的一个进程，要求用户提供信息以产生信号，也就是界面的“通信类型”选择那里的用户选择）

```
 commandBinder.Bind<StartSignal>().To<StartCommand>().Once();
 commandBinder.Bind<StartCommSignal>().To<StartCommCommand1>();
 commandBinder.Bind<StartCommSigna2>().To<StartCommCommand2>();
 ……
```

然后在startcommand2里注入IcommService（通过Injection Binder，根据用户选择进行依赖注入）

并初始化它

```
 	class StartCommCommand2 : Command
    {
        [Inject]
        public ICommService comm { get; set; }
	……
		comm.init();
	……
```

Icommservice的接口定义:

```
public interface ICommService
    {
        MsgSignal msgSignal { get; set; }
        UpdateAirplaneSignal updateSignal { get; set; }
        bool init(object[] param);
        void dispose();
    }
```

通过把UpdateAirplaneSignal绑定到singleton（），制定updateairplanesignal只有一个单例：

`injectionBinder.Bind<UpdateAirplaneSignal>().ToSingleton();`

我们可以在airplane里设置一个updatesignal的listener：

```
public class AirplanePrefabMediator : Mediator
    {
        [Inject]
        public UpdateAirplaneSignal updateSignal { get; set; }
        ……
        public override void OnRegister()//这是strange给mediator类带的一个初始化函数，我们重写它
        {
            updateSignal.AddListener(updateAirplane);
            ……

```

我们实现一个updateairplane方法：

这个方法接收一个DataFrame类型的变量，则我们此时如果updateairplane接收到signal，它会收到通过signal.dispatch（ DataFrame X）发送的一个X，这就是它的传入值。

我们已经可以在updateairplane里实现更新View了。

```
private void updateAirplane(DataFrame revData)
        {
            ……
        }
```
### 写信号：

与读信号方向相反，从View（mediator）读取键盘输入，然后通过一个Signal.Dispatch发送出去。

在Service里加一个此信号的Listener，接收到信号做出响应（把信号通过串口写到无人机去）即可。

※读串口和写串口在同一个类中，用信号触发的方式实现，而且串口是临界区，用信号量实现互斥。