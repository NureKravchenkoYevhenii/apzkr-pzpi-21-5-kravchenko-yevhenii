namespace GateController;

partial class Main
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
        CameraPortNumberInput = new NumericUpDown();
        GatePortNumberInput = new NumericUpDown();
        SaveButton = new Button();
        CameraPortNumberLabel = new Label();
        GatePortNumberLabel = new Label();
        LogoutButton = new Button();
        LanguageLable = new Label();
        LanguageComboBox = new ComboBox();
        CheckInButton = new Button();
        CheckOutButton = new Button();
        ((System.ComponentModel.ISupportInitialize)CameraPortNumberInput).BeginInit();
        ((System.ComponentModel.ISupportInitialize)GatePortNumberInput).BeginInit();
        SuspendLayout();
        // 
        // CameraPortNumberInput
        // 
        resources.ApplyResources(CameraPortNumberInput, "CameraPortNumberInput");
        CameraPortNumberInput.Name = "CameraPortNumberInput";
        // 
        // GatePortNumberInput
        // 
        resources.ApplyResources(GatePortNumberInput, "GatePortNumberInput");
        GatePortNumberInput.Name = "GatePortNumberInput";
        // 
        // SaveButton
        // 
        resources.ApplyResources(SaveButton, "SaveButton");
        SaveButton.Name = "SaveButton";
        SaveButton.UseVisualStyleBackColor = true;
        SaveButton.Click += SaveButton_Click;
        // 
        // CameraPortNumberLabel
        // 
        resources.ApplyResources(CameraPortNumberLabel, "CameraPortNumberLabel");
        CameraPortNumberLabel.Name = "CameraPortNumberLabel";
        // 
        // GatePortNumberLabel
        // 
        resources.ApplyResources(GatePortNumberLabel, "GatePortNumberLabel");
        GatePortNumberLabel.Name = "GatePortNumberLabel";
        // 
        // LogoutButton
        // 
        resources.ApplyResources(LogoutButton, "LogoutButton");
        LogoutButton.Name = "LogoutButton";
        LogoutButton.UseVisualStyleBackColor = true;
        LogoutButton.Click += LogoutButton_Click;
        // 
        // LanguageLable
        // 
        resources.ApplyResources(LanguageLable, "LanguageLable");
        LanguageLable.Name = "LanguageLable";
        // 
        // LanguageComboBox
        // 
        LanguageComboBox.FormattingEnabled = true;
        LanguageComboBox.Items.AddRange(new object[] { resources.GetString("LanguageComboBox.Items"), resources.GetString("LanguageComboBox.Items1") });
        resources.ApplyResources(LanguageComboBox, "LanguageComboBox");
        LanguageComboBox.Name = "LanguageComboBox";
        LanguageComboBox.SelectedIndexChanged += LanguageComboBox_SelectedIndexChanged;
        // 
        // CheckInButton
        // 
        CheckInButton.FlatAppearance.BorderSize = 0;
        CheckInButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
        CheckInButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
        resources.ApplyResources(CheckInButton, "CheckInButton");
        CheckInButton.Name = "CheckInButton";
        CheckInButton.UseVisualStyleBackColor = true;
        CheckInButton.Click += CheckInButton_Click;
        // 
        // CheckOutButton
        // 
        CheckOutButton.FlatAppearance.BorderSize = 0;
        CheckOutButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
        CheckOutButton.FlatAppearance.MouseOverBackColor = Color.Transparent;
        resources.ApplyResources(CheckOutButton, "CheckOutButton");
        CheckOutButton.Name = "CheckOutButton";
        CheckOutButton.UseVisualStyleBackColor = true;
        CheckOutButton.Click += CheckOutButton_Click;
        // 
        // Main
        // 
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(CheckOutButton);
        Controls.Add(CheckInButton);
        Controls.Add(LanguageComboBox);
        Controls.Add(LanguageLable);
        Controls.Add(LogoutButton);
        Controls.Add(GatePortNumberLabel);
        Controls.Add(CameraPortNumberLabel);
        Controls.Add(SaveButton);
        Controls.Add(GatePortNumberInput);
        Controls.Add(CameraPortNumberInput);
        Name = "Main";
        ((System.ComponentModel.ISupportInitialize)CameraPortNumberInput).EndInit();
        ((System.ComponentModel.ISupportInitialize)GatePortNumberInput).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private NumericUpDown CameraPortNumberInput;
    private NumericUpDown GatePortNumberInput;
    private Button SaveButton;
    private Label CameraPortNumberLabel;
    private Label GatePortNumberLabel;
    private Button LogoutButton;
    private Label LanguageLable;
    private ComboBox LanguageComboBox;
    private Button CheckInButton;
    private Button CheckOutButton;
}
