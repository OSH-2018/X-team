from pymavlink import mavutil

def send_nav_velocity(self, velocity_x, velocity_y, velocity_z):
    # 生成SET_POSITION_TARGET_LOCAL_NED命令
    msg = self.vehicle.message_factory.set_position_target_local_ned_encode(
                    0,       # time_boot_ms (not used)
                    0, 0,    # target system, target component
                    mavutil.mavlink.MAV_FRAME_BODY_NED,   # frame
                    0b0000111111000111, # type_mask (only speeds enabled)
                    0, 0, 0, # x, y, z positions (not used)
                    velocity_x, velocity_y, velocity_z, # x, y, z velocity in m/s
                    0, 0, 0, # x, y, z acceleration (not used)
                    0, 0)    # yaw, yaw_rate (not used) 
    # 发送指令
    self.vehicle.send_mavlink(msg)
    self.vehicle.flush()