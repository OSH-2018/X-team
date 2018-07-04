using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Ports;
using System.Threading;
using strange.extensions.dispatcher.eventdispatcher.api;
using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
/*
 * 在final report中不是这份代码，这份代码没有按照strangeIOC的约定框架来编程，但是它能很快速地产生结果。
 * final report的代码会在后面上传
 */
namespace airplanegame
{
    public class serialp
    {
        public static bool _continue;
        private static SerialPort comm;
        private static Thread readThread;
        private static bool isRead = true;
        private static Queue<string> lonq=new Queue<string>();
        private static Queue<string> latq=new Queue<string>();
        private static Queue<string> altq=new Queue<string>();
        public static string lastlon ="121";
        public static string lastlat = "41";
        public static string lastalt = "1300";
        private static int read2 = new int();

        public static void serialread()
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
                        Thread.Sleep(100);
                    }
                }
                catch (TimeoutException)
                {
                }
            }
        }
        public static int getread2()
        {
            return read2;
        }
        public static float getlon()
        {
            float jvalue = new float();
            if (lonq.Count == 0)
            {
                float.TryParse(lastlon, out jvalue);
                return jvalue;
            }
            else
            {
                float.TryParse(lonq.Dequeue(), out jvalue);
                return jvalue;
            }
        }
        public static float getlat()
        {
            float jvalue=new float();
            if (latq.Count == 0)
            {
                float.TryParse(lastlat, out jvalue);
                return jvalue;
            }
            else
            {
                float.TryParse(latq.Dequeue(), out jvalue);
                return jvalue;
            }
        }
        public static float getal()
        {
            float jvalue = new float();
            if (altq.Count == 0)
            {
                float.TryParse(lastalt, out jvalue);
                return jvalue;
            }
            else
            {
                float.TryParse(altq.Dequeue(), out jvalue);
                return jvalue;
            }
        }
        public static void serialwrite(string message)
        {
            if (message.Equals("nmsl"))
            {
                _continue = false;
                readThread.Join();
                comm.Close();
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
        public static bool init()
        {
            comm = new SerialPort();
            comm.PortName = "COM3";
            comm.BaudRate = 115200;
            comm.DataBits = 8;
            comm.Parity = Parity.None;
            comm.StopBits = StopBits.One;
            comm.ReadTimeout = 500;
            comm.WriteTimeout = 500;
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
        public static void disposeserial()
        {
            comm.Dispose();
            return;
        }
    }
}
