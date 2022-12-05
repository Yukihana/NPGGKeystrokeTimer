Public Class Form1
    Private Declare Function myPostMessageA Lib "BypassedPostmessage.dll" (ByVal hWnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByRef lParam As Integer) As Integer
    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Integer
    Private Declare Function GetForegroundWindow Lib "user32" () As Integer
    Dim f2 As AboutBox1
    Const EWX_SHUTDOWN As Long = 1
    Private Const WM_KEYDOWN As Short = &H100
    Dim capctdn As Integer
    Dim lHandle As Integer
    Dim CTDNtime As Integer
    Dim xKey As Integer

    Private Sub xAlarm()
        ProgressBar1.Value = ProgressBar1.Maximum
        Deploy()
        If CheckBox5.Checked = False Then
            SecTimer.Stop()
            xReady()
            ExeKawaii()
        Else
            CTDNtime = xGetData()
            ProgressBar1.Value = 0
            If CTDNtime = 0 Then
                SecTimer.Stop()
                xReady()
            End If
        End If
    End Sub
    Private Sub ExeKawaii()
        Timer1.Start()
        If CheckBox2.Checked = True Then
            NotifyIcon1.Icon = SystemIcons.Information
            NotifyIcon1.Visible = True
            NotifyIcon1.ShowBalloonTip(15000)
        End If
        If CheckBox4.Checked = True Then
            PlaySystemSound()
        End If
    End Sub
    Private Sub xReady()
        HMSenab(True)
        Button2.Enabled = False
        Button2.Text = "Start Timer"
    End Sub
    Private Sub xMode(ByVal enb As Boolean)
        If NumH.Value = 0 And NumM.Value = 0 And NumS.Value = 0 Then
        Else
            Button2.Enabled = True
        End If
        Button3.Enabled = enb
    End Sub
    Private Sub Deploy()
        xKey = ComboBox1.SelectedIndex
        Select Case xKey
            Case 0
                myPostMessageA(lHandle, WM_KEYDOWN, Keys.F1, 0)
            Case 1
                myPostMessageA(lHandle, WM_KEYDOWN, Keys.F2, 0)
            Case 2
                myPostMessageA(lHandle, WM_KEYDOWN, Keys.F3, 0)
            Case 3
                myPostMessageA(lHandle, WM_KEYDOWN, Keys.F4, 0)
            Case 4
                myPostMessageA(lHandle, WM_KEYDOWN, Keys.F5, 0)
            Case 5
                myPostMessageA(lHandle, WM_KEYDOWN, Keys.F6, 0)
            Case 6
                myPostMessageA(lHandle, WM_KEYDOWN, Keys.F7, 0)
            Case 7
                myPostMessageA(lHandle, WM_KEYDOWN, Keys.F8, 0)
            Case 8
                myPostMessageA(lHandle, WM_KEYDOWN, Keys.F9, 0)
        End Select
    End Sub
    Private Sub xStart()
        CTDNtime = xGetData()
        ProgressBar1.Maximum = CTDNtime
        ProgressBar1.Value = 0
        If CTDNtime > 0 Then
            HMSenab(False)
            SecTimer.Start()
            Button2.Text = "Pause Timer"
        End If
    End Sub
    Function xGetData()
        If NumH.ToString = "" Then NumH.Value = 0
        If NumM.ToString = "" Then NumM.Value = 0
        If NumS.ToString = "" Then NumS.Value = 0
        xGetData = (NumH.Value * 60 + NumM.Value) * 60 + NumS.Value
    End Function
    Private Sub xSetData()
        If CheckBox5.Checked = False Then
            NumH.Value = (CTDNtime - (CTDNtime Mod 3600)) / 3600
            NumM.Value = (CTDNtime Mod 3600 - (CTDNtime Mod 60)) / 60
            NumS.Value = CTDNtime Mod 60
        End If
    End Sub
    Private Sub HMSenab(ByVal enb As Boolean)
        NumH.Enabled = enb
        NumM.Enabled = enb
        NumS.Enabled = enb
    End Sub
    Private Sub xAfter(ByVal enb As Boolean)
        CheckBox1.Enabled = enb
        CheckBox2.Enabled = enb
        CheckBox3.Enabled = enb
        CheckBox4.Enabled = enb
    End Sub
    Private Sub PlaySystemSound()
        My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Asterisk)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If NumericUpDown1.ToString = "" Then
            NumericUpDown1.Value = 2
        End If
        capTimer.Interval = Val(NumericUpDown1.Value * 1000)
        capTimer.Start()
    End Sub
    Private Sub capTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles capTimer.Tick
        lHandle = GetForegroundWindow()
        PlaySystemSound()
        TextBox1.Text = lHandle
        capTimer.Stop()
        If lHandle > 0 Then xMode(True)
    End Sub
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = 0
        ProgressBar1.Value = 0
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If SecTimer.Enabled = False Then
            xStart()
        Else
            SecTimer.Enabled = False
            HMSenab(True)
            Button2.Text = "Start Timer"
            SecTimer.Stop()
        End If
    End Sub
    Private Sub SecTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SecTimer.Tick
        CTDNtime = CTDNtime - 1
        xSetData()
        If CTDNtime = 0 Then
            xAlarm()
        Else
            ProgressBar1.Value = ProgressBar1.Value + 1
        End If
    End Sub
    Private Sub NumH_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NumS.ValueChanged, NumM.ValueChanged, NumH.ValueChanged
        If NumH.Value = 0 And NumM.Value = 0 And NumS.Value = 0 And SecTimer.Enabled = False Then
            Button2.Enabled = False
        ElseIf Button3.Enabled = True Then
            Button2.Enabled = True
        End If
    End Sub
    Private Sub CheckBox5_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox5.CheckedChanged
        If CheckBox5.Checked = True Then
            xAfter(False)
        Else
            xAfter(True)
        End If
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        SecTimer.Enabled = False
        Timer1.Enabled = False
        capTimer.Enabled = False
        TextBox1.Text = ""
        NumericUpDown1.Value = 2
        lHandle = 0
        CTDNtime = 0
        xKey = 0
        ComboBox1.SelectedIndex = 0
        NumH.Value = 0
        NumM.Value = 0
        NumS.Value = 0
        NumH.Enabled = True
        NumM.Enabled = True
        NumS.Enabled = True
        Button2.Enabled = False
        Button2.Text = "Start Timer"
        Button3.Enabled = False
        ProgressBar1.Value = 0
        CheckBox5.Checked = False
        CheckBox2.Checked = True
        CheckBox1.Checked = False
        CheckBox3.Checked = False
        CheckBox4.Checked = False
        CheckBox2.Enabled = True
        CheckBox1.Enabled = True
        CheckBox3.Enabled = True
        CheckBox4.Enabled = True
    End Sub
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        NotifyIcon1.Visible = False
        Timer1.Stop()
        If CheckBox3.Checked = True Then
            NotifyIcon1.Visible = True
            NotifyIcon1.BalloonTipText = "Shutting Down the Computer..."
            NotifyIcon1.ShowBalloonTip(15000)
            System.Diagnostics.Process.Start("Shutdown.exe", "/s")
        End If
        If CheckBox1.Checked = True Then
            Application.Exit()
        End If
    End Sub
    Private Sub Label9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label9.Click
        f2 = New AboutBox1
        Call f2.ShowDialog()
    End Sub
End Class
