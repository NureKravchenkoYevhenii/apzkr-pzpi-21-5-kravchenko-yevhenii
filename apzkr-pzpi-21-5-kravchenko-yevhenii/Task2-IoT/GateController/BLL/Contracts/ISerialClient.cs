namespace BLL.Contracts;
public interface ISerialClient
{
    void SetPorts(int cameraPort, int gatePort);

    byte[] CaptureImage();

    void OpenGate();
}
