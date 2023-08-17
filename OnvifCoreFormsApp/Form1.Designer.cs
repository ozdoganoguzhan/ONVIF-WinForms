namespace OnvifCoreFormsApp
{
    partial class Form1
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
            ipTextBox=new TextBox();
            loginButton=new Button();
            passwordTextBox=new TextBox();
            usernameTextBox=new TextBox();
            listBox1=new ListBox();
            listBox2=new ListBox();
            codecBox=new ComboBox();
            qualityBox=new ComboBox();
            resolutionBox=new ComboBox();
            label1=new Label();
            label2=new Label();
            label3=new Label();
            applyButton=new Button();
            video=new Vlc.DotNet.Forms.VlcControl();
            ((System.ComponentModel.ISupportInitialize)video).BeginInit();
            SuspendLayout();
            // 
            // ipTextBox
            // 
            ipTextBox.Location=new Point(97, 10);
            ipTextBox.Name="ipTextBox";
            ipTextBox.Size=new Size(100, 23);
            ipTextBox.TabIndex=0;
            ipTextBox.Text="IP";
            // 
            // loginButton
            // 
            loginButton.Location=new Point(16, 10);
            loginButton.Name="loginButton";
            loginButton.Size=new Size(75, 23);
            loginButton.TabIndex=1;
            loginButton.Text="Login";
            loginButton.UseVisualStyleBackColor=true;
            loginButton.Click+=loginButton_Click;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Location=new Point(309, 10);
            passwordTextBox.Name="passwordTextBox";
            passwordTextBox.Size=new Size(100, 23);
            passwordTextBox.TabIndex=2;
            passwordTextBox.Text="Password";
            // 
            // usernameTextBox
            // 
            usernameTextBox.Location=new Point(203, 10);
            usernameTextBox.Name="usernameTextBox";
            usernameTextBox.Size=new Size(100, 23);
            usernameTextBox.TabIndex=3;
            usernameTextBox.Text="Username";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled=true;
            listBox1.ItemHeight=15;
            listBox1.Location=new Point(589, 82);
            listBox1.Name="listBox1";
            listBox1.Size=new Size(199, 214);
            listBox1.TabIndex=5;
            listBox1.SelectedIndexChanged+=listBox1_SelectedIndexChanged;
            // 
            // listBox2
            // 
            listBox2.FormattingEnabled=true;
            listBox2.ItemHeight=15;
            listBox2.Location=new Point(794, 82);
            listBox2.Name="listBox2";
            listBox2.Size=new Size(836, 364);
            listBox2.TabIndex=7;
            // 
            // codecBox
            // 
            codecBox.FormattingEnabled=true;
            codecBox.Location=new Point(12, 462);
            codecBox.Name="codecBox";
            codecBox.Size=new Size(121, 23);
            codecBox.TabIndex=8;
            // 
            // qualityBox
            // 
            qualityBox.FormattingEnabled=true;
            qualityBox.Location=new Point(143, 462);
            qualityBox.Name="qualityBox";
            qualityBox.Size=new Size(121, 23);
            qualityBox.TabIndex=9;
            // 
            // resolutionBox
            // 
            resolutionBox.FormattingEnabled=true;
            resolutionBox.Location=new Point(270, 462);
            resolutionBox.Name="resolutionBox";
            resolutionBox.Size=new Size(121, 23);
            resolutionBox.TabIndex=10;
            // 
            // label1
            // 
            label1.AutoSize=true;
            label1.Location=new Point(53, 444);
            label1.Name="label1";
            label1.Size=new Size(46, 15);
            label1.TabIndex=11;
            label1.Text="CODEC";
            // 
            // label2
            // 
            label2.AutoSize=true;
            label2.Location=new Point(178, 441);
            label2.Name="label2";
            label2.Size=new Size(45, 15);
            label2.TabIndex=12;
            label2.Text="Quality";
            // 
            // label3
            // 
            label3.AutoSize=true;
            label3.Location=new Point(296, 441);
            label3.Name="label3";
            label3.Size=new Size(63, 15);
            label3.TabIndex=13;
            label3.Text="Resolution";
            // 
            // applyButton
            // 
            applyButton.Location=new Point(12, 562);
            applyButton.Name="applyButton";
            applyButton.Size=new Size(118, 31);
            applyButton.TabIndex=14;
            applyButton.Text="Apply Changes";
            applyButton.UseVisualStyleBackColor=true;
            applyButton.Click+=applyButton_Click;
            // 
            // video
            // 
            video.BackColor=Color.Black;
            video.Location=new Point(16, 82);
            video.Name="video";
            video.Size=new Size(567, 356);
            video.Spu=-1;
            video.TabIndex=6;
            video.Text="vlcControl1";
            video.VlcLibDirectory=null;
            video.VlcMediaplayerOptions=null;
            video.VlcLibDirectoryNeeded+=VlcLibDirectoryNeeded;
            video.Click+=video_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions=new SizeF(7F, 15F);
            AutoScaleMode=AutoScaleMode.Font;
            ClientSize=new Size(1635, 605);
            Controls.Add(applyButton);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(resolutionBox);
            Controls.Add(qualityBox);
            Controls.Add(codecBox);
            Controls.Add(listBox2);
            Controls.Add(video);
            Controls.Add(listBox1);
            Controls.Add(usernameTextBox);
            Controls.Add(passwordTextBox);
            Controls.Add(loginButton);
            Controls.Add(ipTextBox);
            Name="Form1";
            Text="Form1";
            ((System.ComponentModel.ISupportInitialize)video).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox ipTextBox;
        private Button loginButton;
        private TextBox passwordTextBox;
        private TextBox usernameTextBox;
        private ListBox listBox1;
        private ListBox listBox2;
        private ComboBox codecBox;
        private ComboBox qualityBox;
        private ComboBox resolutionBox;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button applyButton;
        private Vlc.DotNet.Forms.VlcControl video;
    }
}