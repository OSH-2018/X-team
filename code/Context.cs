
using System;
using UnityEngine;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.dispatcher.eventdispatcher.impl;
using strange.extensions.command.api;
using strange.extensions.command.impl;

namespace airplanegame
{
    public class Context : MVCSContext
    {

        public Context(MonoBehaviour view)
            : base(view)
        {
        }

        public Context(MonoBehaviour view, ContextStartupFlags flags)
            : base(view, flags)
        {
        }

        // Unbind the default EventCommandBinder and rebind the SignalCommandBinder
        protected override void addCoreComponents()
        {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        // Override Start so that we can fire the StartSignal 
        override public IContext Start()
        {
            base.Start();
            StartSignal startSignal = (StartSignal)injectionBinder.GetInstance<StartSignal>();
            startSignal.Dispatch();
            return this;
        }

        protected override void mapBindings()
        {


            //injectionBinder.Bind<IExampleModel>().To<ExampleModel>().ToSingleton();
            //injectionBinder.Bind<IExampleService>().To<ExampleService>().ToSingleton();
            //injectionBinder.Bind<INetService>().To<NetService>();
            //injectionBinder.Bind<ICommService>().To<SerialPortService>().ToSingleton();
            injectionBinder.Bind<ICommService>().To<NetComService>().ToSingleton();
            injectionBinder.Bind<serialp>().ToSingleton();

            mediationBinder.Bind<TestView>().To<TestMediator>();
            mediationBinder.Bind<UIRootView>().To<UIRootMediator>();
            mediationBinder.Bind<TerrainView>().To<TerrainMediator>();
            mediationBinder.Bind<AirplanePrefabView>().To<AirplanePrefabMediator>();
            mediationBinder.Bind<AirplaneManagerView>().To<AirplaneManagerMediator>();
            mediationBinder.Bind<CameraTexturePanelView>().To<CameraTexturePanelMediator>();
            mediationBinder.Bind<CameraView>().To<CameraMediator>();


            //commandBinder.Bind<CallWebServiceSignal>().To<CallWebServiceCommand>();

            //StartSignal is now fired instead of the START event.
            //Note how we've bound it "Once". This means that the mapping goes away as soon as the command fires.
            commandBinder.Bind<StartSignal>().To<StartCommand>().Once();
            commandBinder.Bind<StartCommSignal>().To<StartCommCommand1>();
            commandBinder.Bind<StartCommSigna2>().To<StartCommCommand2>();

            ////In MyFirstProject, there's are SCORE_CHANGE and FULFILL_SERVICE_REQUEST Events.
            ////Here we change that to a Signal. The Signal isn't bound to any Command,
            ////so we map it as an injection so a Command can fire it, and a Mediator can receive it
            //injectionBinder.Bind<ScoreChangedSignal>().ToSingleton();
            //injectionBinder.Bind<FulfillWebServiceRequestSignal>().ToSingleton();
            injectionBinder.Bind<MySignalSend>().ToSingleton();
            injectionBinder.Bind<MySignalWrite>().ToSingleton();
            injectionBinder.Bind<ReceiveDataSignal>().ToSingleton();
            injectionBinder.Bind<EndSignal>().ToSingleton();
            injectionBinder.Bind<MsgSignal>().ToSingleton();
            injectionBinder.Bind<LocationSignal>().ToSingleton();
            injectionBinder.Bind<UpdateAirplaneSignal>().ToSingleton();
            injectionBinder.Bind<CommReplySignal>().ToSingleton();
            injectionBinder.Bind<SwitchAirplaneSignal>().ToSingleton();
            injectionBinder.Bind<SwitchCameraSignal>().ToSingleton();
            injectionBinder.Bind<AddUIAirplaneSignal>().ToSingleton();
            injectionBinder.Bind<DelUIAirplaneSignal>().ToSingleton();            
            injectionBinder.Bind<BackSignal>().ToSingleton();
            injectionBinder.Bind<SetLineSignal>().ToSingleton();
            injectionBinder.Bind<SetFileReadProgressSignal>().ToSingleton();
            injectionBinder.Bind<SetProgressBarSignal>().ToSingleton();
            injectionBinder.Bind<PlayPauseSignal>().ToSingleton();
            injectionBinder.Bind<LogSignal>().ToSingleton();
        }
    }
}