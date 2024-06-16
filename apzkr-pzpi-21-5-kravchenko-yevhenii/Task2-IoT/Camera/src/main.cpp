#include <Arduino.h>
#include <Domain/Enums/Command.h>
#include <BLL/Services/CameraService/CameraService.h>

CameraService cameraService;

void setup() {
    Serial.begin(115200);

    cameraService.initSpiffs();
    cameraService.initCamera();
}

void loop() {
    if (Serial.available()) {
        String request = Serial.readStringUntil('\n');
        Command cmd = (Command)(request.toInt());

        switch(cmd) {
            case Command::CaptureImage:
                cameraService.capturePhotoAndSaveToSpiffs();
                cameraService.sendPhotoToSerial();
                break;
            default:
                break;
        }
    }
}
