#include <Arduino.h>
#include <BLL/Services/GateService/GateService.h>
#include <Domain/Enums/Command.h>

GateService gateService;

void setup() {
    Serial.begin(115200);

    gateService.initGate();
}

void loop() {
    if (Serial.available()) {
        String request = Serial.readStringUntil('\n');
        Command cmd = (Command)(request.toInt());

        switch(cmd) {
            case Command::OpenGate:
                gateService.openGate();
                delay(7000);
                gateService.closeGate();
                break;
            default:
                break;
        }
    }
}
