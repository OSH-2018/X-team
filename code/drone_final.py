#coding=utf-8
from dronekit import connect, VehicleMode
import time
import dronekit_sitl
import threading
from time import ctime,sleep
import serial
from time import sleep
from pymavlink import mavutil

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
     

def Read_Data_From_Pixhawk(vehicle):
    temp = vehicle.location.global_frame
    Read_Data = ''
    j = 0
    cnt = 0
    Read_Data += 'begin'
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



def recv(serial):  
    data =serial.readline()     
    return data  

def Read_From_Groundcontrol(vehicle, ser):
    while True:
        data = recv(ser)
        send_nav_velocity(vehicle, data)
        sleep(0.02)



def Send_To_Groundcontrol(vehicle, ser):
    while True:
        Read_data = Read_Data_From_Pixhawk(vehicle)
        ser.write(Read_data)
        sleep(0.02) 
    
ser = serial.Serial('/dev/ttyUSB0', 115200, timeout=0.5,bytesize=8,parity=serial.PARITY_NONE,stopbits=1)


connection_string = "/dev/ttyS0" #--for a serial port connection , use /dev/ttyAMA0
baud_rate = 57600

#--- Now that we have started the SITL and we have the connection string (basically the ip and udp port)...

vehicle = connect(connection_string,baud = baud_rate, wait_ready=True)     #- wait_ready flag hold the program untill all the parameters are been read (=, not .)


threads = []
t1 = threading.Thread(target=Read_From_Groundcontrol,args=(vehicle,ser))
threads.append(t1)
t2 = threading.Thread(target=Send_To_Groundcontrol,args=(vehicle,ser))
threads.append(t2)

if __name__ == '__main__':
    for t in threads:
        t.setDaemon(True)
        t.start()
