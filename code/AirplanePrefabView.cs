    using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;

namespace airplanegame
{
    public class AirplanePrefabView : View
    {
        public Transform FirstCamera;
        public Transform PanelCamera;
        public Transform VelPointer;
        public Transform YawPointer;
        public Transform AltPointer;
        public Transform AltPointerK;
        public Transform HorizonSensorBall;
        public Transform Airscrew;
        public Transform HandloopText;
        private TextMesh HandloopMesh;
        public GameObject qlj;
        public Transform TerrainGameObject;

        public float lon { get; set; }
        public float lat { get; set; }
        public float alt { get; set; }
        public float yaw { get; set; }
        public float pitch { get; set; }
        public float roll { get; set; }
        public float vel { get; set; }
        public int handloop { get; set; }

        private float lineWidth = 10f;
        private int lineCount = 0;
        private List<LineRenderer> linerenderList = new List<LineRenderer>();
        private int lineVNum = 0;
        private LineRenderer mline;
        private Vector3 prePosition = new Vector3(0f, 0f, 0f);
        private Color lineColor;

        private Vector3 playerdelta = new Vector3(0, 0, 0);
        //嘤嘤嘤
        public CameraEnum activeCamera;

        void OnEnable()
        {
            prePosition = Vector3.zero;
        }

        protected override void Start()
        {
            base.Start();
            lon = float.NaN; lat = float.NaN;
            yaw = 0; pitch = 0; roll = 0; vel = 0; handloop = 0; alt = 0;
            FirstCamera = gameObject.transform.Find("FirstCamera");
            PanelCamera = gameObject.transform.Find("PanelCamera");
            VelPointer = gameObject.transform.Find("velPanel/Pointer");
            YawPointer = gameObject.transform.Find("yawPanel/Pointer");
            AltPointer = gameObject.transform.Find("altitudePanel/Pointer");
            AltPointerK = gameObject.transform.Find("altitudePanel/PointerK");
            HorizonSensorBall = gameObject.transform.Find("horizonSensor/Pointer");
            Airscrew = gameObject.transform.Find("airscrew");
            HandloopText = gameObject.transform.Find("Panel/Handloop");
            qlj = gameObject.transform.Find("qlj").gameObject;
            HandloopMesh = HandloopText.GetComponent<TextMesh>();
            TerrainGameObject = GameObject.Find("ViewRoot/Terrain").transform;
            //StartCoroutine(drawLine());
            //mline = new LineRenderer();
            //lineObject = new GameObject();
            //lineObject.layer = 8;
            //lineObject.transform.parent = gameObject.transform;
            //lineObject.name = "Trace";
            //mline = lineObject.AddComponent<LineRenderer>();
            //Color tempColor = new Color(255, 0, 0, 255);
            //mline.SetColors(tempColor, tempColor);
            System.Random rm = new System.Random();
            //lineColor= new Color(rm.Next(0,255), rm.Next(0, 255), rm.Next(0, 255), 255);
            lineColor = new Color(0,255, 0, 255);
            createLineRender();
    
    }

    void Update()
        {
            //UnityEngine.Debug.Log(lineColor);
            if (float.IsNaN(lon))
            {
                return;
            }
            //嘤嘤嘤
            if (Input.GetKey(KeyCode.UpArrow))
                playerdelta.x += 10f;
            else if (Input.GetKey(KeyCode.DownArrow))
                playerdelta.x -= 10f;
            if (Input.GetKey(KeyCode.LeftArrow))
                playerdelta.z += 10f;
            else if (Input.GetKey(KeyCode.RightArrow))
                playerdelta.z -= 10f;
            if (Input.GetKey(KeyCode.A))
                playerdelta.y += 10f;
            else if (Input.GetKey(KeyCode.S))
                playerdelta.y -= 10f;
            if ((activeCamera==CameraEnum.FirstCamera||activeCamera==CameraEnum.PanelCamera)&&alt>500)
            {
                gameObject.transform.localScale = new Vector3(20, 20, 20);
            }
            else if(activeCamera == CameraEnum.ThirdCamera)
            {
                gameObject.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                float height = lineWidth * 200f;
                float scale = Mathf.Max(1f, height / 200f);
                gameObject.transform.localScale = new Vector3(scale,scale,scale);
            }

            //嘤嘤嘤
            gameObject.transform.position = new Vector3(MapLib.WorldPosToCoord(lon, lat)[0]+playerdelta.x, alt+playerdelta.y, MapLib.WorldPosToCoord(lon, lat)[1]+playerdelta.z);
            print(gameObject.transform.position.x);
            print((MapLib.WorldPosToCoord(lon, lat)[0] + playerdelta.x));
            //嘤嘤嘤
            gameObject.transform.rotation = MapLib.yawPitchRolltoRotation(yaw, -1f*pitch, -1f*roll);
            updateVelPointer(vel);
            updateYawPointer(yaw);
            updateAltPointer(alt);
            updateHorizon(pitch, roll);
            HandloopMesh.text = handloop.ToString();   
            if((alt-TerrainGameObject.position.y)>200f)
            {
                qlj.SetActive(false);
            }
            else
            {
                qlj.SetActive(true);
            }
            if((!(prePosition==Vector3.zero))&& Vector3.Distance(prePosition, transform.position)>1f)
            {
                if(Vector3.Distance(prePosition,transform.position)>50f)
                {
                    createLineRender();
                }  
                else
                {
                    mline.positionCount = lineVNum + 1;
                    mline.useWorldSpace = true;
                    if (lineVNum == 1)
                    {
                        mline.SetPosition(lineVNum - 1, new Vector3(prePosition.x,prePosition.y,prePosition.z));
                    }

                    mline.SetPosition(lineVNum,new Vector3(transform.position.x,prePosition.y,transform.position.z));
                    lineVNum++;
                    for (int i = 0; i < lineCount; i++)
                    {
                        linerenderList[i].startWidth = lineWidth;
                        linerenderList[i].endWidth = lineWidth;
                    }
                }                
            }
            prePosition = transform.position;
        }

        private void createLineRender()
        {
            LineRenderer mline = new LineRenderer();
            GameObject lineObject = new GameObject();
            lineObject.layer = 8;
            lineObject.transform.parent = gameObject.transform;
            lineObject.name = "Trace";
            mline = lineObject.AddComponent<LineRenderer>();

            mline.startColor = lineColor;
            mline.endColor = new Color(255 - lineColor.r, 255 - lineColor.g, 255 - lineColor.b, lineColor.a);
            linerenderList.Add(mline);
            this.mline = mline;
            lineCount++;
            lineVNum = 1;
        }

       

        public void setLineWidth(float width)
        {
            lineWidth = width;
        }

        public void SetAirscrew(bool status)
        {
            Airscrew.gameObject.SetActive(status);
        }

        public void setHandloop(int num)
        {
            handloop = num;
        }

        private void updateVelPointer(float vel)
        {
            float zangle = 0;
            if(vel<50f)
            {
                zangle = (vel) / 50f * 13f;
            }
            else if(vel<100f)
            {
                zangle = (vel - 50f) / 50f * (50f - 13f) + 13f;
            }
            else if(vel<150f)
            {
                zangle = (vel - 100f) / 50f * (100f - 50f) + 50f;
            }
            else if (vel < 200f)
            {
                zangle = (vel - 150f) / 50f * (155f - 100f) + 100f;
            }
            else if (vel < 250f)
            {
                zangle = (vel - 200f) / 50f * (200f - 155f) + 155f;
            }
            else if (vel < 300f)
            {
                zangle = (vel - 250f) / 50f * (242f - 200f) + 200f;
            }
            else if (vel < 350f)
            {
                zangle = (vel - 300f) / 50f * (280f - 242f) + 242f;
            }
            else if (vel < 400f)
            {
                zangle = (vel - 350f) / 50f * (313f - 280f) + 280f;
            }
            else if (vel < 450f)
            {
                zangle = (vel - 400f) / 50f * (345f - 313f) + 313f;
            }
            else
            {
                zangle = 350f;
            }
            VelPointer.localEulerAngles = new Vector3(0f, 0f, zangle);
        }

        private void updateYawPointer(float yaw)
        {
            YawPointer.localEulerAngles = new Vector3(0f, 0f, yaw);
        }

        private void updateAltPointer(float alt)
        {
            int tempK = (int)(alt / 1000f);
            float tempH = alt - tempK * 1000f;
            AltPointerK.localEulerAngles = new Vector3(0f, 0f, tempK * 180f / 15f);
            AltPointer.localEulerAngles = new Vector3(0f, 0f, tempH / 1000f * 360f);
        }

        private void updateHorizon(float pitch, float roll)
        {
            HorizonSensorBall.localEulerAngles = new Vector3(roll, 90f, -1f*pitch);
        }
    }
}