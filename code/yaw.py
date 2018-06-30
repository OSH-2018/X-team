from pymavlink import mavutil

def condition_yaw(self, heading, relative, clock_wise):
    # 使用相对角度或绝对方位
    if relative:
        isRelative = 1
    else:
        isRelative = 0

    # 若使用相对角度，则进行顺时针或逆时针转动
    if clock_wise:
        direction = 1  # "heading"所对应的角度将被加和到当前朝向角
    else:
        direction = -1 # "heading"所对应的角度将被从当前朝向角减去
    if not relative:
        direction = 0

    # 生成CONDITION_YAW命令
    msg = self.vehicle.message_factory.command_long_encode(
                    0, 0,       # target system, target component
                    mavutil.mavlink.MAV_CMD_CONDITION_YAW, # command
                    0,          # confirmation
                    heading,    # param 1, yaw in degrees
                    0,          # param 2, yaw speed (not used)
                    direction,  # param 3, direction
                    isRelative, # param 4, relative or absolute degrees 
                    0, 0, 0)    # param 5-7, not used
    # 发送指令
    self.vehicle.send_mavlink(msg)
    self.vehicle.flush()