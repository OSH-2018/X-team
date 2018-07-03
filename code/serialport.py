import serial
from time import sleep
ser = serial.Serial('/dev/ttyUSB0', 115200, timeout=0.5,bytesize=8,parity=serial.PARITY_NONE,stopbits=1)
def recv(serial):  
    while True:  
        data =serial.read(30) 
        if data == '':  
            continue
        else:
            print data 
            break
        sleep(0.02) 
    return data  
while True:  
    data =recv(ser)  
    ser.write(data)

    