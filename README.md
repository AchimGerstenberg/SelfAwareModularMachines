# SelfAwareModularMachines

Can a modular machine with linear and rotary stages detect its own configuration from acceleration data? The aim is that all I care about is the tool path. Whenever the geometrical configuration of the machine changes it shall recognize this and adapt the necessary motor controls to execute the tool path accordingly.

The work of Nadya Peek at the center for bits and atoms (MIT) on modular machines makes it much faster to prototype digital fabrication machines by combining linear and rotary stage modules. However, the translation from gcode to motor commands changes whenever the geometric configuration of the machine changes and needs to be manually adjusted. Can adjusting this "translation" be automized by using linear and rotary acceleration data based on cheap MEMS-based inertial measurement unit sensors (accelerometer and gyroscope)?

This repository includes an Arduino file that shows the acceleration data from the MPU6050 IMU, gcode files that accelerate the print bed of a 3d printer back and forth or with constant acceleration and C# and Unity files for programming a digital twin of the desired machines as a noise free testbed for developing the mathematics of the inverse kinematics.

The angular precision of the MPU6050 was tested by rotating the sensor and using gravitational acceleration. However, for unambigiously detecting the configuration of the machines it is also necessary to use and measure the acceleration of the modules. The accuracy of measuring these accelerations was tested by mounting the sensor on the print bed of a 3D printer and accelerating the sensor abruptly and with constant acceleration. Both methods did not show sufficient accuracy for determining the machine configuration sufficiently accurately. Therefore the project was halted.

I still believe the concept could work but the acceleration data would require extensive filtering and more advanced motor controls for smooth acceleration.

Feel free to give it a try and write me an email to Achim.Gerstenberg@ntnu.no if you have questions or ideas. Have fun!
