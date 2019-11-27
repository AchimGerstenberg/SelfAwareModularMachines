#include "I2Cdev.h"
#include "MPU6050.h"
#include "Wire.h"

MPU6050 accelgyro;

int16_t ax, ay, az;
int16_t gx, gy, gz;




void setup() {
  Wire.begin();
  Serial.begin(115200);
  accelgyro.initialize();
  accelgyro.setFullScaleAccelRange(MPU6050_ACCEL_FS_4);   // largest g range 
  pinMode(13,OUTPUT);
}

void loop() {
    digitalWrite(13,1);
    float phi = 0;
    float accelx = 0;
    
    /*for(int i = 0; i < 10; i++)
    {
      accelgyro.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);
      phi += atan2(ay,ax); 
      accelx += ax;
    }
    */
    accelgyro.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);
    phi = atan2(ay,ax);
    Serial.println((phi)*180/(3.141),4);
    //Serial.println(accelx,4);
    digitalWrite(13,0);

  
  
  //Serial.print("X: ");
  //Serial.println(ax);
  //Serial.print("Y: ");
  //Serial.println(ay);
  delay(100);
  
}
