namespace GateController.Forms;

partial class Login
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
        LoginButton = new Button();
        PasswordTextBox = new TextBox();
        LoginTextBox = new TextBox();
        LanguageComboBox = new ComboBox();
        LanguageLabel = new Label();
        SuspendLayout();
        // 
        // LoginButton
        // 
        resources.ApplyResources(LoginButton, "LoginButton");
        LoginButton.Name = "LoginButton";
        LoginButton.UseVisualStyleBackColor = true;
        LoginButton.Click += LoginButton_Click;
        // 
        // PasswordTextBox
        // 
        resources.ApplyResources(PasswordTextBox, "PasswordTextBox");
        PasswordTextBox.Name = "PasswordTextBox";
        // 
        // LoginTextBox
        // 
        resources.ApplyResources(LoginTextBox, "LoginTextBox");
        LoginTextBox.Name = "LoginTextBox";
        // 
        // LanguageComboBox
        // 
        LanguageComboBox.FormattingEnabled = true;
        LanguageComboBox.Items.AddRange(new object[] { resources.GetString("LanguageComboBox.Items"), resources.GetString("LanguageComboBox.Items1") });
        resources.ApplyResources(LanguageComboBox, "LanguageComboBox");
        LanguageComboBox.Name = "LanguageComboBox";
        LanguageComboBox.SelectedIndexChanged += LanguageComboBox_SelectedIndexChanged;
        // 
        // LanguageLabel
        // 
        resources.ApplyResources(LanguageLabel, "LanguageLabel");
        LanguageLabel.Name = "LanguageLabel";
        // 
        // Login
        // 
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(LanguageLabel);
        Controls.Add(LanguageComboBox);
        Controls.Add(LoginButton);
        Controls.Add(PasswordTextBox);
        Controls.Add(LoginTextBox);
        Name = "Login";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button LoginButton;
    private TextBox PasswordTextBox;
    private TextBox LoginTextBox;
    private ComboBox LanguageComboBox;
    private Label LanguageLabel;
}