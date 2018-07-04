using System;
using System.Threading;
using strange.extensions.context.api;
using strange.extensions.command.impl;
using strange.extensions.dispatcher.eventdispatcher.impl;

namespace airplanegame
{
    //串口或者读文件开始命令
    class StartCommCommand2 : Command
    {
        [Inject]
        public CommType commType { get; set; }
        [Inject]
        public object[] param { get; set; }
        [Inject]
        public serialp easyport { get; set; }

        [Inject]
        public ICommService comm { get; set; }
        [Inject]
        public CommReplySignal replySignal { get; set; }
        [Inject]
        public BackSignal backSignal { get; set; }


        public override void Execute()
        {
            backSignal.AddListener(dispose);
            //if (CommType.SerialPort == commType)
            //{
            //    if (comm.init(param))
            //    {
            //        replySignal.Dispatch(true,commType);
            //    }
            //}
            //else if (CommType.FileRead == commType)
            //{
            //    if (comm.init(param))
            //    {
            //        replySignal.Dispatch(true, commType);
            //    }
            //}
            if (comm.init(param))
            {
                replySignal.Dispatch(true, commType);
            }
            easyport.init();
        }

        private void dispose()
        {
            easyport.disposeserial();
            backSignal.RemoveListener(dispose);
            comm.dispose();
            comm = null;
        }
    }
}
