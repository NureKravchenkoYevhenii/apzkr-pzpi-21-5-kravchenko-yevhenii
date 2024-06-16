using BLL.Contracts;
using Infrastructure.Enums;
using Newtonsoft.Json;
using System.IO.Ports;
using System.Text;

namespace BLL.Services;
public class SerialClient : ISerialClient
{
    private const int BAUD_RATE = 115200;
    private const int DATA_BITS = 8;

    private static int _cameraPortNumber;
    private static int _gatePortNumber;

    public void SetPorts(int cameraPort, int gatePort)
    {
        _cameraPortNumber = cameraPort;
        _gatePortNumber = gatePort;
    }

    public byte[] CaptureImage()
    {
        var imageBytes = SendCommand(Command.CaptureImage);

        return imageBytes ?? Array.Empty<byte>();
    }

    public void OpenGate()
    {
        SendCommand(Command.OpenGate);
    }

    private byte[]? SendCommand(Command command)
    {
        byte[]? response = null;
        var commandToSend = JsonConvert.SerializeObject(command);
        var port = GetPort(command);

        try
        {
            port.Open();
            port.Write($"{commandToSend}\n");

            if (command == Command.CaptureImage)
                response = GetImageFromSerial(port);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            port.Close();
        }

        return response;
    }

    private SerialPort GetPort(Command command)
    {
        int portNumber = 0;
        switch (command)
        {
            case Command.CaptureImage:
                portNumber = _cameraPortNumber;
                break;
            case Command.OpenGate:
                portNumber = _gatePortNumber;
                break;
        }

        var port = new SerialPort(
            $"COM{portNumber}",
            BAUD_RATE,
            Parity.None,
            DATA_BITS,
            StopBits.One);

        return port;
    }

    private byte[] GetImageFromSerial(SerialPort port)
    {
        using var memoryStream = new MemoryStream();

        while (true)
        {
            try
            {
                var response = port.ReadLine();
                if (string.IsNullOrWhiteSpace(response) || response.Length < 100)
                    continue;

                var buffer = Encoding.ASCII.GetBytes(response);
                memoryStream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {
                break;
            }
        }

        return memoryStream.ToArray();
    }
}
