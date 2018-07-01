import serial
from time import sleep
ser = serial.Serial('/dev/ttyUSB0', 115200, timeout=0.5)
ser.bytesize = 8
ser.parity = 'E'
ser.stopbits = 1 
def recv(serial):  
    data
    while True:  
        data =serial.read(30)  
        if data == '':  
            continue
        else:
            break
        sleep(0.02) 
    return data  
while True:  
    data =recv(ser)  
    ser.write(data)