using BLL.Contracts;
using Domain.Models;
using GateController.Infrastructure.Helpers;

namespace GateController.Forms;
public partial class Login : Form
{
    private readonly Main _mainForm;
    private readonly IAuthService _loginService;
    private readonly IParkingSessionService _parkingSessionService;

    public Login(
        Main mainForm,
        IAuthService loginService,
        IParkingSessionService parkingSessionService)
    {
        _mainForm = mainForm;
        _loginService = loginService;
        _parkingSessionService = parkingSessionService;
        
        InitializeComponent();
    }

    private void LoginButton_Click(object sender, EventArgs e)
    {
        var loginModel = new LoginModel
        {
            Login = LoginTextBox.Text,
            Password = PasswordTextBox.Text,
        };

        _loginService.Login(loginModel);

        Hide();
        _mainForm.ShowDialog();
        Close();
    }

    private void LanguageComboBox_SelectedIndexChanged(object sender, EventArgs e)
    {
        CultureHelper.ChangeCulture(
            LanguageComboBox.SelectedIndex);

        Controls.Clear();
        InitializeComponent();
    }
}
