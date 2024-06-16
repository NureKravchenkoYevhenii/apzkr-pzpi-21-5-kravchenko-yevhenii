#ifndef GATE_SERVICE_H
#define GATE_SERVICE_H

#define SERVO_PIN D2

#include <Servo.h>

class GateService {
public:
    void initGate();
    void openGate();
    void closeGate();

private:
    Servo servo;
};

#endif
