#include "GateService.h"

void GateService::initGate() {
    servo.attach(SERVO_PIN, 0, 2000);
    servo.write(0);
}

void GateService::openGate() {
    for (int pos = 0; pos <= 180; pos += 1) {
        servo.write(pos);
        delay(10);
    }
}

void GateService::closeGate() {
    for (int pos = 180; pos >= 0; pos -= 1) {
        servo.write(pos);
        delay(10);
    }
}
