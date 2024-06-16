using BLL.Contracts;
using GateController.Forms;
using GateController.Infrastructure.Helpers;
using Infrastructure.DIContainer;
using Microsoft.Extensions.DependencyInjection;

namespace GateController;

public partial class Main : Form
{
    private readonly IAuthService _loginService;
    private readonly ISerialClient _serialClient;
    private readonly IParkingSessionService _parkingSessionService;

    public Main(
        IAuthService loginService,
        ISerialClient serialClient,
        IParkingSessionService parkingSessionService)
    {
        InitializeComponent();
        _loginService = loginService;
        _serialClient = serialClient;
        _parkingSessionService = parkingSessionService;
    }

    private void SaveButton_Click(object sender, EventArgs e)
    {
        var cameraPortNumber = (int)CameraPortNumberInput.Value;
        var gatePortNumber = (int)GatePortNumberInput.Value;

        _serialClient.SetPorts(cameraPortNumber, gatePortNumber);
    }

    private void LogoutButton_Click(object sender, EventArgs e)
    {
        _loginService.Logout();
        var loginForm = DIContainer.Services!.GetRequiredService<Login>();

        Hide();
        loginForm.ShowDialog();
        Close();
    }

    private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        CultureHelper.ChangeCulture(
            LanguageComboBox.SelectedIndex);

        Controls.Clear();
        InitializeComponent();
    }

    private void CheckInButton_Click(object sender, EventArgs e)
    {
        _parkingSessionService.CheckIn();
    }

    private void CheckOutButton_Click(object sender, EventArgs e)
    {
        _parkingSessionService.CheckOut();
    }
}
