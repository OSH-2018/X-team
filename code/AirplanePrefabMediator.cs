using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System.IO;
using System.IO.Ports;
using System.Threading;
using strange.extensions.context.api;

namespace airplanegame
{
    public class AirplanePrefabMediator : Mediator
    {
        [Inject]
        public UpdateAirplaneSignal updateSignal { get; set; }

        [Inject]
        public AirplanePrefabView view { get; set; }

        [Inject]
        public MySignalSend mySignalSend { get; set; }

        [Inject]
        public MySignalWrite mySignalWrite { get; set; }

        [Inject]
        public SwitchCameraSignal switchCameraSignal { get; set; }

        [Inject]
        public SwitchAirplaneSignal swithcAirplaneSignal { get; set; }

        [Inject]
        public SetLineSignal setLineSignal { get; set; }

        [Inject]
        public EndSignal Esignal { get; set; }

        private string airplaneName = string.Empty;

        public override void OnRegister()
        {
            base.OnRegister();
            airplaneName = gameObject.name;
            //updateSignal.AddListener(updateAirplane);
            mySignalSend.AddListener(updateairplane2);
            switchCameraSignal.AddListener(onSwitchCameraSignal);
            setLineSignal.AddListener(onSetLineSignal);
            Esignal.AddListener(dispose);
        }
        public void updateairplane2(string s)
        {
            print(s);
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
            bool up = Input.GetKey(KeyCode.Z);
            bool down = Input.GetKey(KeyCode.X);
            string z;
            if (up) z = "1";
            else if (down) z = "-1";
            else z = "0";
            mySignalWrite.Dispatch(string.Format("{0},{1},{2}", x.ToString(), y.ToString(), z.ToString()));
        }
        public override void OnRemove()
        {
            base.OnRemove();
            //updateSignal.RemoveListener(updateAirplane);
            switchCameraSignal.RemoveListener(onSwitchCameraSignal);
            setLineSignal.RemoveListener(onSetLineSignal);
            Esignal.RemoveListener(dispose);
        }

        private void updateAirplane(DataFrame revData)
        {
            if (airplaneName != revData.name)
            {
                return;
            }
            updateViewData(revData);
        }

        public void updateViewData(DataFrame revData)
        {
            if(revData.isGPS==true)
            {
                view.lon = revData.longitude;
                view.lat = revData.latitude;
                view.alt = revData.altitude;
                //Debug.Log(revData.longitude + "          " + revData.altitude + "         " + revData.latitude);
            }
            else
            {
                view.yaw = revData.yaw;
                view.pitch = revData.pitch;
                view.roll = revData.roll;
                view.vel = revData.speedLand;
                view.handloop = revData.handloop;
                //Debug.Log(revData.yaw + "          " + revData.pitch + "         " + revData.roll);
            }
        }

        private void onSetLineSignal(float width)
        {
            view.setLineWidth(width);
        }

        private void onSwitchCameraSignal(string name,CameraEnum cameraType)
        {
            if(name!=gameObject.name)
            {
                return;
            }
            if(cameraType==CameraEnum.ThirdCamera||cameraType==CameraEnum.MainCamera)
            {               
                swithcAirplaneSignal.Dispatch(gameObject.transform, cameraType);
                gameObject.transform.localScale = new Vector3(1, 1, 1);
                view.SetAirscrew(true);
                if(cameraType==CameraEnum.ThirdCamera)
                {
                    view.activeCamera = CameraEnum.ThirdCamera;
                }
                else
                {
                    view.activeCamera = CameraEnum.MainCamera;
                }
            }
            else if (cameraType == CameraEnum.FirstCamera)
            {
                swithcAirplaneSignal.Dispatch(view.FirstCamera.transform, cameraType);
                view.SetAirscrew(false);
                view.activeCamera = CameraEnum.FirstCamera;
            }
            else if (cameraType == CameraEnum.PanelCamera)
            {
                swithcAirplaneSignal.Dispatch(view.PanelCamera.transform, cameraType);
                view.SetAirscrew(false);
                view.activeCamera = CameraEnum.PanelCamera;
            }           
        }

        private void dispose()
        {
            gameObject.transform.DestroyChildren();
        }
    }
}