﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;

namespace airplanegame
{
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
        private Queue<string> lonq = new Queue<string>();
        private Queue<string> latq = new Queue<string>();
        private Queue<string> altq = new Queue<string>();
        public string lastlon = "121";
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
                        comm.ReadTo("begin");
                        lastlon = comm.ReadTo("lon");
                        lastlat = comm.ReadTo("lat");
                        lastalt = comm.ReadTo("alt");
                        lonq.Enqueue(lastlon);
                        latq.Enqueue(lastlat);
                        altq.Enqueue(lastalt);
                        mySignalSend.Dispatch(lastlon + "lon" + lastlat + "lat" + lastalt + "alt");
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
            readThread.Join();
            comm.Close();
            comm.Dispose();
            return;
        }
    }

}
