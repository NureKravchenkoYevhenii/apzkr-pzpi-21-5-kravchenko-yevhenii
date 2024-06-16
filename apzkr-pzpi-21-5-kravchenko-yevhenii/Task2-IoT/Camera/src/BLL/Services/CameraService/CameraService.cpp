#include "CameraService.h"

void CameraService::initSpiffs() {
    if (!SPIFFS.begin(true)) {
        Serial.println("Error while mounting mounting SPIFFS");
        ESP.restart();
    } else {
        delay(500);
        Serial.println("SPIFFS mounted successfully");
    }
}

void CameraService::initCamera() {
    WRITE_PERI_REG(RTC_CNTL_BROWN_OUT_REG, 0);
    camera_config_t config = setupCameraConfiguration();

    esp_err_t err = esp_camera_init(&config);
    if (err != ESP_OK) {
        Serial.printf("Camera init failed with error 0x%x", err);
        ESP.restart();
    }
}

camera_config_t CameraService::setupCameraConfiguration() {
    camera_config_t config;
    
    config.ledc_channel = LEDC_CHANNEL_0;
    config.ledc_timer = LEDC_TIMER_0;
    config.pin_d0 = Y2_GPIO_NUM;
    config.pin_d1 = Y3_GPIO_NUM;
    config.pin_d2 = Y4_GPIO_NUM;
    config.pin_d3 = Y5_GPIO_NUM;
    config.pin_d4 = Y6_GPIO_NUM;
    config.pin_d5 = Y7_GPIO_NUM;
    config.pin_d6 = Y8_GPIO_NUM;
    config.pin_d7 = Y9_GPIO_NUM;
    config.pin_xclk = XCLK_GPIO_NUM;
    config.pin_pclk = PCLK_GPIO_NUM;
    config.pin_vsync = VSYNC_GPIO_NUM;
    config.pin_href = HREF_GPIO_NUM;
    config.pin_sccb_sda = SIOD_GPIO_NUM;
    config.pin_sccb_scl = SIOC_GPIO_NUM;
    config.pin_pwdn = PWDN_GPIO_NUM;
    config.pin_reset = RESET_GPIO_NUM;
    config.xclk_freq_hz = 20000000;
    config.pixel_format = PIXFORMAT_JPEG;

    if (psramFound()) {
        config.frame_size = FRAMESIZE_UXGA;
        config.jpeg_quality = 10;
        config.fb_count = 2;
    } else {
        config.frame_size = FRAMESIZE_SVGA;
        config.jpeg_quality = 12;
        config.fb_count = 1;
    }

    return config;
}

void CameraService::capturePhotoAndSaveToSpiffs() {
    camera_fb_t * fb = NULL;
    bool ok = 0;

    do {
        Serial.println("Taking a photo...");

        fb = esp_camera_fb_get();
        if (!fb) {
            Serial.println("Camera capture failed");
            return;
        }

        Serial.printf("Picture file name: %s\n", FILE_PHOTO);
        File file = SPIFFS.open(FILE_PHOTO, FILE_WRITE);

        if (!file) {
            Serial.println("Failed to open file in writing mode");
        } else {
            file.write(fb->buf, fb->len);
            Serial.print("The picture has been saved in ");
            Serial.print(FILE_PHOTO);
            Serial.print(" - Size: ");
            Serial.print(file.size());
            Serial.println(" bytes");
        }

        file.close();
        esp_camera_fb_return(fb);

        ok = isPhotoValid();
    } while ( !ok );
}

bool CameraService::isPhotoValid() {
    File f_pic = SPIFFS.open(FILE_PHOTO);
    unsigned int pic_sz = f_pic.size();

    f_pic.close();

    return pic_sz > 100;
}

void CameraService::sendPhotoToSerial() {
    File f_pic = SPIFFS.open(FILE_PHOTO);
    size_t pic_sz = f_pic.size();

    char buffer[pic_sz];

    char t = 255;
    while(f_pic.available()) {
        size_t bytes_read = f_pic.readBytes(buffer, pic_sz);

        Serial.write(buffer, bytes_read);
        Serial.write('\n');
    }

    f_pic.close();
}
