Imports System.Runtime.InteropServices
Imports P66_Multiple.Peak.Can.Basic
Imports TPCANHandle = System.Byte
Imports System.Text

'  Form1.vb
'
'  ~~~~~~~~~~~~
'
'  Form1 Main Class
'
'  ~~~~~~~~~~~~
'
'  ------------------------------------------------------------------
'  Author : David Kopp
'  Last change: 02.03.2020 Kopp
'
'  Language: VB .NET
'  ------------------------------------------------------------------
'   This class is the main coding body of the program for communcating with up to 12 Validyne P66 Transducers.
'   You are able to get the basic calibration information full scale pressure counts, zero pressure counts,
'   moving average, sample frequency, and output period. In this program you can also write to the Validyne P66
'   Tranducer a new valid value to either the moving average, sample frequency, or output period. (Or all three.)
Public Class Form1
    <DllImport("user32.dll", CharSet:=CharSet.Auto)>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As Integer, <MarshalAs(UnmanagedType.LPWStr)> ByVal lParam As String) As Int32
    End Function
    Private ReadOnly Transducers(11) As Transducer

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitCanbus()

        ' Configuring the hint text for the user input boxes on all panels. (Moving Average, Sample Frequency, and Output Period.
        SendMessage(Me.movingAverage0.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod0.Handle, &H1501, 0, "0 for Off, 1-100")
        SendMessage(Me.movingAverage1.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod1.Handle, &H1501, 0, "0 for Off, 1-100")
        SendMessage(Me.movingAverage2.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod2.Handle, &H1501, 0, "0 for Off, 1-100")
        SendMessage(Me.movingAverage3.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod3.Handle, &H1501, 0, "0 for Off, 1-100")
        SendMessage(Me.movingAverage4.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod4.Handle, &H1501, 0, "0 for Off, 1-100")
        SendMessage(Me.movingAverage5.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod5.Handle, &H1501, 0, "0 for Off, 1-100")
        SendMessage(Me.movingAverage6.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod6.Handle, &H1501, 0, "0 for Off, 1-100")
        SendMessage(Me.movingAverage7.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod7.Handle, &H1501, 0, "0 for Off, 1-100")
        SendMessage(Me.movingAverage8.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod8.Handle, &H1501, 0, "0 for Off, 1-100")
        SendMessage(Me.movingAverage9.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod9.Handle, &H1501, 0, "0 for Off, 1-100")
        SendMessage(Me.movingAverage10.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod10.Handle, &H1501, 0, "0 for Off, 1-100")
        SendMessage(Me.movingAverage11.Handle, &H1501, 0, "0-100")
        SendMessage(Me.outputPeriod11.Handle, &H1501, 0, "0 for Off, 1-100")

        ' Setting up the Reading Delegate and starting it's thread.
        m_ReadDelegate = New ReadDelegateHandler(AddressOf ReadMessages)

        m_ReceiveEvent = New System.Threading.AutoResetEvent(False)

        Dim threadDelate As New System.Threading.ThreadStart(AddressOf Me.CANReadThreadFunc)
        m_ReadThread = New System.Threading.Thread(threadDelate)
        m_ReadThread.IsBackground = True
        m_ReadThread.Start()
    End Sub

    ''' <summary>
    ''' Closing Form book keeping.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        PCANBasic.Uninitialize(m_PcanHandle)
        m_ReadThread.Abort()
    End Sub

    ''' <summary>
    ''' Adding Event Listeners to all serialNumber Textboxes.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SerialNumber0_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber0.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber0.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber0.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber0.Text)
                }
                Transducers(0) = o_transducer
                Call NewConnection(0)
                ShowPanel(2)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub
    Private Sub SerialNumber1_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber1.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber1.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber1.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber1.Text)
                }
                Transducers(1) = o_transducer
                Call NewConnection(1)
                ShowPanel(3)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub
    Private Sub SerialNumber2_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber2.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber2.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber2.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber2.Text)
                }
                Transducers(2) = o_transducer
                Call NewConnection(2)
                ShowPanel(4)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub
    Private Sub SerialNumber3_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber3.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber3.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber3.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber3.Text)
                }
                Transducers(3) = o_transducer
                Call NewConnection(3)
                ShowPanel(5)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub
    Private Sub SerialNumber4_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber4.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber4.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber4.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber4.Text)
                }
                Transducers(4) = o_transducer
                Call NewConnection(4)
                ShowPanel(6)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub
    Private Sub SerialNumber5_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber5.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber5.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber5.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber5.Text)
                }
                Transducers(5) = o_transducer
                Call NewConnection(5)
                ShowPanel(7)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub
    Private Sub SerialNumber6_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber6.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber6.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber6.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber6.Text)
                }
                Transducers(6) = o_transducer
                Call NewConnection(6)
                ShowPanel(8)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub
    Private Sub SerialNumber7_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber7.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber7.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber7.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber7.Text)
                }
                Transducers(7) = o_transducer
                Call NewConnection(7)
                ShowPanel(9)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub
    Private Sub SerialNumber8_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber8.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber8.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber8.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber8.Text)
                }
                Transducers(8) = o_transducer
                Call NewConnection(8)
                ShowPanel(10)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub
    Private Sub SerialNumber9_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber9.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber9.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber9.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber9.Text)
                }
                Transducers(9) = o_transducer
                Call NewConnection(9)
                ShowPanel(11)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub
    Private Sub SerialNumber10_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber10.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber10.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber10.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber10.Text)
                }
                Transducers(10) = o_transducer
                Call NewConnection(10)
                ShowPanel(12)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub
    Private Sub SerialNumber11_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs) Handles serialNumber11.KeyPress
        If e.KeyChar = ChrW(Keys.Return) And serialNumber11.Text.Length = 6 Then
            e.Handled = True
            If Not DoesUnitExist(serialNumber11.Text) Then
                Dim o_transducer As New Transducer With {
                .SerialNumber = Convert.ToInt32(serialNumber11.Text)
                }
                Transducers(11) = o_transducer
                Call NewConnection(11)
            Else
                MessageBox.Show("The serial number that was entered already exists. Please enter a different serial number.")
            End If
        End If
    End Sub

    ''' <summary>
    ''' This sub procedure initializes the PEAKVIEW CANBUS Device.
    ''' </summary>
    Private Sub InitCanbus()
        Dim stsResult As TPCANStatus
        Dim ibuffer As UInt32 = "1"

        m_PcanHandle = Convert.ToByte("51", 16)
        m_Baudrate = TPCANBaudrate.PCAN_BAUD_250K
        m_HwType = TPCANType.PCAN_TYPE_ISA

        stsResult = PCANBasic.Initialize(m_PcanHandle, m_Baudrate, m_HwType, Convert.ToUInt32("0100", 16), Convert.ToUInt16("3"))

        ' Reporting Error to user
        If stsResult <> TPCANStatus.PCAN_ERROR_OK Then
            MessageBox.Show(GetFormatedError(stsResult))
        End If

        'Auto resets the CAN controller when BussOFF error is encountered
        stsResult = PCANBasic.SetValue(m_PcanHandle, TPCANParameter.PCAN_BUSOFF_AUTORESET, ibuffer, CType(System.Runtime.InteropServices.Marshal.SizeOf(ibuffer), UInteger))
        'Console.WriteLine(GetFormatedError(stsResult)) 'Error Checking for Debugging
    End Sub

    ''' <summary>
    ''' This sub procedure is used to reset the CANBUS device if it's buffer is erroring.
    ''' </summary>
    Private Sub ResetCanbus()
        Dim stsResult As TPCANStatus

        stsResult = PCANBasic.Reset(m_PcanHandle)

        ' Reporting Error to user
        If stsResult <> TPCANStatus.PCAN_ERROR_OK Then
            MessageBox.Show(GetFormatedError(stsResult))
        End If
    End Sub

#Region "Members"
    ''' <summary>
    ''' Saves the handle Of a PCAN hardware
    ''' </summary>
    Private m_PcanHandle As TPCANHandle
    ''' <summary>
    ''' Saves the baudrate register for a conenction
    ''' </summary>
    Private m_Baudrate As TPCANBaudrate
    ''' <summary>
    ''' Saves the type of a non-plug-and-play hardware
    ''' </summary>
    Private m_HwType As TPCANType
    ''' <summary>
    ''' Handles of the current available PCAN-Hardware
    ''' </summary>
    Private m_HandlesArray As TPCANHandle()
    ''' <summary>
    ''' Read Delegate For calling the Function "ReadMessages"
    ''' </summary>
    Private m_ReadDelegate As ReadDelegateHandler
    ''' <summary>
    ''' Receive-Event
    ''' </summary>
    Private m_ReceiveEvent As System.Threading.AutoResetEvent
    ''' <summary>
    ''' Thread For message reading (Using events)
    ''' </summary>
    Private m_ReadThread As System.Threading.Thread
#End Region

#Region "Delegates"
    ''' <summary>
    ''' Read-Delegate Handler
    ''' </summary>
    Private Delegate Sub ReadDelegateHandler()
#End Region

    ''' <summary>
    ''' This sub procedure sends transducer commands H,C,X,O,A,Q,E - see P66 protocols for more information.
    ''' </summary>
    ''' <param name="unit">The unit number as an integer 0 to 11</param>
    Private Sub NewConnection(unit As Integer)
        Dim NetID As String

        ' Setting NetID
        NetID = CanbusConnect(Transducers(unit).SerialNumber)
        Call WriteToCanbus("H", unit, NetID)
        Call WriteToCanbus("C", unit, NetID)
        Call WriteToCanbus("X", unit, NetID)
        Call WriteToCanbus("O", unit, NetID)
        Call WriteToCanbus("A", unit, NetID)
        Call WriteToCanbus("Q", unit, NetID)
        Call WriteToCanbus("E", unit, NetID)
    End Sub

    ''' <summary>
    ''' This function is used to compute the correct NetID for 1 to 1 communications.
    ''' </summary>
    ''' <param name="sn">The serial number of the transducer as an Integer.</param>
    ''' <returns></returns>
    Private Function CanbusConnect(sn As Integer) As String
        Dim id As String

        id = Int(4194304 Or sn)
        id = Hex(id)

        While id.Length < 8
            id = "0" + id
        End While

        Return id
    End Function

    ''' <summary>
    ''' This sub procedure constructs the CANBUS data packet by transducer command.
    ''' </summary>
    ''' <param name="cmd">Transducer command as String</param>
    ''' <param name="unit">Unit number as Integer</param>
    ''' <param name="NetID">NetID is optional As String</param>
    ''' <param name="value">Value is optional as String</param>
    Private Sub WriteToCanbus(cmd As String, unit As Integer, Optional NetID As String = "", Optional value As String = "")
        Dim packet As Byte()
        Dim len As Integer '# of Bytes
        Dim stsResult As TPCANStatus

        packet = {}
        len = 0
        Select Case cmd
            Case "H"
                len = 1
                packet = CreatePacket(cmd, len)
            Case "C"
                len = 1
                packet = CreatePacket(cmd, len)
            Case "X"
                len = 1
                packet = CreatePacket(cmd, len)

            Case "O"
                If value.Length = 0 Then
                    len = 5
                    packet = CreatePacket(cmd, len)
                Else
                    len = 2
                    If Convert.ToInt32(value) > -1 And Convert.ToInt32(value) < 101 Then
                        packet = CreatePacket(cmd, len, value)
                    Else
                        MessageBox.Show("Output Period value for Unit with SN: " + Transducers(unit).SerialNumber +
                                        " is invalid. Please choose a value in the correct range of 0 for Off, 1 to 100")
                        Exit Sub
                    End If
                End If
            Case "A"
                If value.Length = 0 Then
                    len = 5
                    packet = CreatePacket(cmd, len)
                Else
                    len = 2
                    If Convert.ToInt32(value) > 0 And Convert.ToInt32(value) < 101 Then
                        packet = CreatePacket(cmd, len, value)
                    Else
                        MessageBox.Show("Moving Average value for Unit with SN: " + Transducers(unit).SerialNumber +
                                        " is invalid. Please choose a value in the correct range of 1 to 100")
                        Exit Sub
                    End If

                End If
            Case "Q"
                If value.Length = 0 Then
                    len = 5
                    packet = CreatePacket(cmd, len)
                Else
                    len = 2
                    packet = CreatePacket(cmd, len, value)
                End If
            Case "W"
                len = 1
                packet = CreatePacket(cmd, len)
            Case "E"
                len = 1
                packet = CreatePacket(cmd, len)
        End Select

        stsResult = PCANBasic.GetStatus(m_PcanHandle)

        ' This checks to see if CANBUS is in error and if it is resets the bus.
        If stsResult = TPCANStatus.PCAN_ERROR_BUSHEAVY Then
            Call ResetCanbus()
        End If

        ' This is the call to send created packet or payload to the transducer.
        Call SendPacket(packet, len, NetID, unit)
    End Sub

    ''' <summary>
    ''' This function creates the Packet for sending to the P66 Transducer.
    ''' </summary>
    ''' <param name="cmd">P66 command that is being sent.</param>
    ''' <param name="len">Length of the packet being sent.</param>
    ''' <param name="value">Value is optional if the Moving average, sample frequency, or output period is being changed.</param>
    ''' <returns>The return value is a Byte Array of the Hex value of the cmd and its values.</returns>
    Private Function CreatePacket(cmd As String, len As Integer, Optional value As String = "") As Byte()
        Dim packet As String

        packet = ""
        Select Case cmd
            Case "H"
                packet = ConvertTohex(cmd)
                Call ResetCanbus()
            Case "C"
                packet = ConvertTohex(cmd)
            Case "X"
                packet = ConvertTohex(cmd)
            Case "O"
                If value.Length = 0 Then
                    packet = ConvertTohex(cmd) + "FFFFFFFF" 'Get
                Else
                    packet = ConvertTohex(cmd) + Hex(value) 'Set
                End If
            Case "A"
                If value.Length = 0 Then
                    packet = ConvertTohex(cmd) + "FFFFFFFF" 'Get
                Else
                    packet = ConvertTohex(cmd) + Hex(value) 'Set
                End If
            Case "Q"
                If value.Length = 0 Then
                    packet = ConvertTohex(cmd) + "FFFFFFFF" 'Get
                Else
                    packet = ConvertTohex(cmd) + Hex(value) 'Set
                End If
            Case "W"
                packet = ConvertTohex(cmd)
            Case "E"
                packet = ConvertTohex(cmd)
        End Select

        Return ConvertToByteArray(packet, len)
    End Function

    ''' <summary>
    ''' This sub procedure sends the created byte package through the CANBUS to the Transducer.
    ''' </summary>
    ''' <param name="packet">Byte array package</param>
    ''' <param name="len">Length of the Byte array</param>
    ''' <param name="NetID">The ID of the device we are communcating with over the CANBUS device.</param>
    ''' <param name="unit">This is the Transducer unit number 0-11.</param>
    Private Sub SendPacket(packet As Byte(), len As Integer, NetID As String, unit As Integer)
        Dim CANMsg As TPCANMsg
        Dim stsResult As TPCANStatus

        CANMsg = New TPCANMsg With {
            .DATA = New Byte(7) {}
        }
        Array.Copy(packet, CANMsg.DATA, len)
        CANMsg.LEN = Convert.ToByte(len)
        CANMsg.ID = Convert.ToUInt32(NetID, 16)
        CANMsg.MSGTYPE = TPCANMessageType.PCAN_MESSAGE_EXTENDED

        Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
        stsResult = PCANBasic.Write(m_PcanHandle, CANMsg)

        If stsResult = TPCANStatus.PCAN_ERROR_OK Then
            'Debug.Print("Message Sent") ' Debugging purpose only
        Else
            MessageBox.Show(GetFormatedError(stsResult))
        End If
    End Sub

    ''' <summary>
    ''' This sub procedure pharses the return message from the transducer.
    ''' </summary>
    ''' <param name="readBuffer">This is the buffer as a List of Byte Arrays from the CANBUS device.</param>
    ''' <param name="unit">Transducer's unit number 0-11</param>
    Private Sub ProcessMessage(readBuffer As List(Of Byte()), unit As Integer)
        Dim response As String = ""
        Dim cmd As Char

        For Each message In readBuffer
            cmd = Convert.ToChar(message(0))
            If cmd = "C" Then
                For Each value In readBuffer
                    Dim msg As Byte() = value
                    For i = 0 To msg.Length - 1
                        response += Convert.ToChar(msg(i))
                    Next
                Next
                UpdateTransducers(cmd, response, unit)
            ElseIf cmd = "X" Then
                Dim str1 As String = LeadingZero(Hex(message(2))) + LeadingZero(Hex(message(1)))
                Dim str2 As String = LeadingZero(Hex(message(4))) + LeadingZero(Hex(message(3)))
                response = Convert.ToInt32(str1, 16)
                response = String.Concat(response, "*", Convert.ToInt32(str2, 16))
                UpdateTransducers(cmd, response, unit)
            ElseIf cmd = "O" Then
                Dim str1 As String = LeadingZero((Hex(message(1))))
                Dim str2 As String = LeadingZero((Hex(message(2))))
                response = Convert.ToInt32(str2 + str1, 16)
                UpdateTransducers(cmd, response, unit)
            ElseIf cmd = "A" Then
                response = Convert.ToInt32(LeadingZero(Hex(message(1))), 16)
                UpdateTransducers(cmd, response, unit)
            ElseIf cmd = "Q" Then
                response = Convert.ToInt32(LeadingZero(Hex(message(1))), 16)
                UpdateTransducers(cmd, response, unit)
            End If
        Next
    End Sub

    ''' <summary>
    ''' This sub procedure updates the Transducer object array called Transducers with the pharsed values from the ProcessMessage Sub procedure.
    ''' The calls are responses from the tranducer and read from the CANBUS device.
    ''' </summary>
    ''' <param name="cmd">Command that was send As Char</param>
    ''' <param name="value">Value that was returned from the transducer</param>
    ''' <param name="unit">This is the unit number that has returned a response</param>
    Private Sub UpdateTransducers(cmd As Char, value As String, unit As Integer)
        Dim temp() As String = value.Split("*")

        Select Case cmd
            Case "C"
                Transducers(unit).ModelNumber = temp(1)
                Transducers(unit).SerialNumber = temp(2)
                Transducers(unit).CalDate = temp(3)
                Transducers(unit).FSPressure = temp(4) + " " + temp(5)
            Case "X"
                Transducers(unit).ZeroCounts = temp(0)
                Transducers(unit).FSCounts = temp(1)
            Case "O"
                If Convert.ToInt32(temp(0)) > 100 Then
                    Dim newVal As Integer = Convert.ToInt32(temp(0)) / 100
                    Transducers(unit).OutputPeriod = Convert.ToString(newVal)
                Else
                    Transducers(unit).OutputPeriod = Convert.ToInt32(temp(0))
                End If
            Case "A"
                Transducers(unit).MovingAverage = temp(0)
            Case "Q"
                Transducers(unit).SampleFrequency = temp(0)
        End Select

        UpdateGUI()
    End Sub

    ''' <summary>
    ''' This sub procedure updates the Main Thread GUI in real time. Each transducer object is checked and will only display if
    ''' all the values are obtained.
    ''' </summary>
    Private Sub UpdateGUI()

        If Transducers(0) IsNot Nothing Then
            If Transducers(0).DisplayRdy Then
                serialNumber0.Invoke(Sub() serialNumber0.Text = Transducers(0).SerialNumber)
                modelNumber0.Invoke(Sub() modelNumber0.Text = Transducers(0).ModelNumber)
                calDate0.Invoke(Sub() calDate0.Text = Transducers(0).CalDate)
                fsPressure0.Invoke(Sub() fsPressure0.Text = Transducers(0).FSPressure)
                fsCounts0.Invoke(Sub() fsCounts0.Text = Transducers(0).FSCounts)
                zeroCounts0.Invoke(Sub() zeroCounts0.Text = Transducers(0).ZeroCounts)
                maverage0.Invoke(Sub() maverage0.Text = Transducers(0).MovingAverage)
                sfrequency0.Invoke(Sub() sfrequency0.Text = GetSampleFrequency(Transducers(0).SampleFrequency))
                operiod0.Invoke(Sub() operiod0.Text = Transducers(0).OutputPeriod)
            End If
        End If
        If Transducers(1) IsNot Nothing Then
            If Transducers(1).DisplayRdy Then
                serialNumber1.Invoke(Sub() serialNumber1.Text = Transducers(1).SerialNumber)
                modelNumber1.Invoke(Sub() modelNumber1.Text = Transducers(1).ModelNumber)
                calDate1.Invoke(Sub() calDate1.Text = Transducers(1).CalDate)
                fsPressure1.Invoke(Sub() fsPressure1.Text = Transducers(1).FSPressure)
                fsCounts1.Invoke(Sub() fsCounts1.Text = Transducers(1).FSCounts)
                zeroCounts1.Invoke(Sub() zeroCounts1.Text = Transducers(1).ZeroCounts)
                maverage1.Invoke(Sub() maverage1.Text = Transducers(1).MovingAverage)
                sfrequency1.Invoke(Sub() sfrequency1.Text = GetSampleFrequency(Transducers(1).SampleFrequency))
                operiod1.Invoke(Sub() operiod1.Text = Transducers(1).OutputPeriod)
            End If
        End If
        If Transducers(2) IsNot Nothing Then
            If Transducers(2).DisplayRdy Then
                serialNumber2.Invoke(Sub() serialNumber2.Text = Transducers(2).SerialNumber)
                modelNumber2.Invoke(Sub() modelNumber2.Text = Transducers(2).ModelNumber)
                calDate2.Invoke(Sub() calDate2.Text = Transducers(2).CalDate)
                fsPressure2.Invoke(Sub() fsPressure2.Text = Transducers(2).FSPressure)
                fsCounts2.Invoke(Sub() fsCounts2.Text = Transducers(2).FSCounts)
                zeroCounts2.Invoke(Sub() zeroCounts2.Text = Transducers(2).ZeroCounts)
                maverage2.Invoke(Sub() maverage2.Text = Transducers(2).MovingAverage)
                sfrequency2.Invoke(Sub() sfrequency2.Text = GetSampleFrequency(Transducers(2).SampleFrequency))
                operiod2.Invoke(Sub() operiod2.Text = Transducers(2).OutputPeriod)
            End If
        End If
        If Transducers(3) IsNot Nothing Then
            If Transducers(3).DisplayRdy Then
                serialNumber3.Invoke(Sub() serialNumber3.Text = Transducers(3).SerialNumber)
                modelNumber3.Invoke(Sub() modelNumber3.Text = Transducers(3).ModelNumber)
                calDate3.Invoke(Sub() calDate3.Text = Transducers(3).CalDate)
                fsPressure3.Invoke(Sub() fsPressure3.Text = Transducers(3).FSPressure)
                fsCounts3.Invoke(Sub() fsCounts3.Text = Transducers(3).FSCounts)
                zeroCounts3.Invoke(Sub() zeroCounts3.Text = Transducers(3).ZeroCounts)
                maverage3.Invoke(Sub() maverage3.Text = Transducers(3).MovingAverage)
                sfrequency3.Invoke(Sub() sfrequency3.Text = GetSampleFrequency(Transducers(3).SampleFrequency))
                operiod3.Invoke(Sub() operiod3.Text = Transducers(3).OutputPeriod)
            End If
        End If
        If Transducers(4) IsNot Nothing Then
            If Transducers(4).DisplayRdy Then
                serialNumber4.Invoke(Sub() serialNumber4.Text = Transducers(4).SerialNumber)
                modelNumber4.Invoke(Sub() modelNumber4.Text = Transducers(4).ModelNumber)
                calDate4.Invoke(Sub() calDate4.Text = Transducers(4).CalDate)
                fsPressure4.Invoke(Sub() fsPressure4.Text = Transducers(4).FSPressure)
                fsCounts4.Invoke(Sub() fsCounts4.Text = Transducers(4).FSCounts)
                zeroCounts4.Invoke(Sub() zeroCounts4.Text = Transducers(4).ZeroCounts)
                maverage4.Invoke(Sub() maverage4.Text = Transducers(4).MovingAverage)
                sfrequency4.Invoke(Sub() sfrequency4.Text = GetSampleFrequency(Transducers(4).SampleFrequency))
                operiod4.Invoke(Sub() operiod4.Text = Transducers(4).OutputPeriod)
            End If
        End If
        If Transducers(5) IsNot Nothing Then
            If Transducers(5).DisplayRdy Then
                serialNumber5.Invoke(Sub() serialNumber5.Text = Transducers(5).SerialNumber)
                modelNumber5.Invoke(Sub() modelNumber5.Text = Transducers(5).ModelNumber)
                calDate5.Invoke(Sub() calDate5.Text = Transducers(5).CalDate)
                fsPressure5.Invoke(Sub() fsPressure5.Text = Transducers(5).FSPressure)
                fsCounts5.Invoke(Sub() fsCounts5.Text = Transducers(5).FSCounts)
                zeroCounts5.Invoke(Sub() zeroCounts5.Text = Transducers(5).ZeroCounts)
                maverage5.Invoke(Sub() maverage5.Text = Transducers(5).MovingAverage)
                sfrequency5.Invoke(Sub() sfrequency5.Text = GetSampleFrequency(Transducers(5).SampleFrequency))
                operiod5.Invoke(Sub() operiod5.Text = Transducers(5).OutputPeriod)
            End If
        End If
        If Transducers(6) IsNot Nothing Then
            If Transducers(6).DisplayRdy Then
                serialNumber6.Invoke(Sub() serialNumber6.Text = Transducers(6).SerialNumber)
                modelNumber6.Invoke(Sub() modelNumber6.Text = Transducers(6).ModelNumber)
                calDate6.Invoke(Sub() calDate6.Text = Transducers(6).CalDate)
                fsPressure6.Invoke(Sub() fsPressure6.Text = Transducers(6).FSPressure)
                fsCounts6.Invoke(Sub() fsCounts6.Text = Transducers(6).FSCounts)
                zeroCounts6.Invoke(Sub() zeroCounts6.Text = Transducers(6).ZeroCounts)
                maverage6.Invoke(Sub() maverage6.Text = Transducers(6).MovingAverage)
                sfrequency6.Invoke(Sub() sfrequency6.Text = GetSampleFrequency(Transducers(6).SampleFrequency))
                operiod6.Invoke(Sub() operiod6.Text = Transducers(6).OutputPeriod)
            End If
        End If
        If Transducers(7) IsNot Nothing Then
            If Transducers(7).DisplayRdy Then
                serialNumber7.Invoke(Sub() serialNumber7.Text = Transducers(7).SerialNumber)
                modelNumber7.Invoke(Sub() modelNumber7.Text = Transducers(7).ModelNumber)
                calDate7.Invoke(Sub() calDate7.Text = Transducers(7).CalDate)
                fsPressure7.Invoke(Sub() fsPressure7.Text = Transducers(7).FSPressure)
                fsCounts7.Invoke(Sub() fsCounts7.Text = Transducers(7).FSCounts)
                zeroCounts7.Invoke(Sub() zeroCounts7.Text = Transducers(7).ZeroCounts)
                maverage7.Invoke(Sub() maverage7.Text = Transducers(7).MovingAverage)
                sfrequency7.Invoke(Sub() sfrequency7.Text = GetSampleFrequency(Transducers(7).SampleFrequency))
                operiod7.Invoke(Sub() operiod7.Text = Transducers(7).OutputPeriod)
            End If
        End If
        If Transducers(8) IsNot Nothing Then
            If Transducers(8).DisplayRdy Then
                serialNumber8.Invoke(Sub() serialNumber8.Text = Transducers(8).SerialNumber)
                modelNumber8.Invoke(Sub() modelNumber8.Text = Transducers(8).ModelNumber)
                calDate8.Invoke(Sub() calDate8.Text = Transducers(8).CalDate)
                fsPressure8.Invoke(Sub() fsPressure8.Text = Transducers(8).FSPressure)
                fsCounts8.Invoke(Sub() fsCounts8.Text = Transducers(8).FSCounts)
                zeroCounts8.Invoke(Sub() zeroCounts8.Text = Transducers(8).ZeroCounts)
                maverage8.Invoke(Sub() maverage8.Text = Transducers(8).MovingAverage)
                sfrequency8.Invoke(Sub() sfrequency8.Text = GetSampleFrequency(Transducers(8).SampleFrequency))
                operiod8.Invoke(Sub() operiod8.Text = Transducers(8).OutputPeriod)
            End If
        End If
        If Transducers(9) IsNot Nothing Then
            If Transducers(9).DisplayRdy Then
                serialNumber9.Invoke(Sub() serialNumber9.Text = Transducers(9).SerialNumber)
                modelNumber9.Invoke(Sub() modelNumber9.Text = Transducers(9).ModelNumber)
                calDate9.Invoke(Sub() calDate9.Text = Transducers(9).CalDate)
                fsPressure9.Invoke(Sub() fsPressure9.Text = Transducers(9).FSPressure)
                fsCounts9.Invoke(Sub() fsCounts9.Text = Transducers(9).FSCounts)
                zeroCounts9.Invoke(Sub() zeroCounts9.Text = Transducers(9).ZeroCounts)
                maverage9.Invoke(Sub() maverage9.Text = Transducers(9).MovingAverage)
                sfrequency9.Invoke(Sub() sfrequency9.Text = GetSampleFrequency(Transducers(9).SampleFrequency))
                operiod9.Invoke(Sub() operiod9.Text = Transducers(9).OutputPeriod)
            End If
        End If
        If Transducers(10) IsNot Nothing Then
            If Transducers(10).DisplayRdy Then
                serialNumber10.Invoke(Sub() serialNumber10.Text = Transducers(10).SerialNumber)
                modelNumber10.Invoke(Sub() modelNumber10.Text = Transducers(10).ModelNumber)
                calDate10.Invoke(Sub() calDate10.Text = Transducers(10).CalDate)
                fsPressure10.Invoke(Sub() fsPressure10.Text = Transducers(10).FSPressure)
                fsCounts10.Invoke(Sub() fsCounts10.Text = Transducers(10).FSCounts)
                zeroCounts10.Invoke(Sub() zeroCounts10.Text = Transducers(10).ZeroCounts)
                maverage10.Invoke(Sub() maverage10.Text = Transducers(10).MovingAverage)
                sfrequency10.Invoke(Sub() sfrequency10.Text = GetSampleFrequency(Transducers(10).SampleFrequency))
                operiod10.Invoke(Sub() operiod10.Text = Transducers(10).OutputPeriod)
            End If
        End If
        If Transducers(11) IsNot Nothing Then
            If Transducers(11).DisplayRdy Then
                serialNumber11.Invoke(Sub() serialNumber11.Text = Transducers(11).SerialNumber)
                modelNumber11.Invoke(Sub() modelNumber11.Text = Transducers(11).ModelNumber)
                calDate11.Invoke(Sub() calDate11.Text = Transducers(11).CalDate)
                fsPressure11.Invoke(Sub() fsPressure11.Text = Transducers(11).FSPressure)
                fsCounts11.Invoke(Sub() fsCounts11.Text = Transducers(11).FSCounts)
                zeroCounts11.Invoke(Sub() zeroCounts11.Text = Transducers(11).ZeroCounts)
                maverage11.Invoke(Sub() maverage11.Text = Transducers(11).MovingAverage)
                sfrequency11.Invoke(Sub() sfrequency11.Text = GetSampleFrequency(Transducers(11).SampleFrequency))
                operiod11.Invoke(Sub() operiod11.Text = Transducers(11).OutputPeriod)
            End If
        End If
    End Sub

    ' <summary>
    ' Help Function used to get an error as text
    ' </summary>
    ' <param name="error">Error code to be translated</param>
    ' <returns>A text with the translated error</returns>
    Private Function GetFormatedError(ByVal [error] As TPCANStatus) As String
        Dim strTemp As StringBuilder

        ' Creates a buffer big enough for a error-text
        '
        strTemp = New StringBuilder(256)
        ' Gets the text using the GetErrorText API function
        ' If the function success, the translated error is returned. If it fails,
        ' a text describing the current error is returned.
        '
        If PCANBasic.GetErrorText([error], 0, strTemp) <> TPCANStatus.PCAN_ERROR_OK Then
            Return String.Format("An error occurred. Error-code's text ({0:X}) couldn't be retrieved", [error])
        Else
            Return strTemp.ToString()
        End If
    End Function

    ''' <summary>
    ''' This function is a helper function that adds LeadingZeros to a String Hex value that is missing said zeros.
    ''' </summary>
    ''' <param name="value">The value in Hex that may need a leading zero.</param>
    ''' <returns>Returns a String representing a proper Hex value with leading zeros.</returns>
    Private Function LeadingZero(value As String) As String
        Dim temp As String

        temp = value

        If temp.Length < 2 Then
            temp = "0" + temp
        End If

        Return temp
    End Function

    ''' <summary>
    ''' This function is a helper function the convery a Hex string to a string of bytes.
    ''' </summary>
    ''' <param name="comd">Hex string</param>
    ''' <returns>A Hex to byte string representation.</returns>
    Private Function ConvertTohex(ByVal comd As String) As String
        Dim bytes As Byte() = Encoding.ASCII.GetBytes(comd)
        Dim hex As String() = Array.ConvertAll(bytes, Function(b) b.ToString("X2"))
        ConvertTohex = String.Join(String.Empty, hex)

    End Function

    ''' <summary>
    ''' This helper function converts a string into a byte array.
    ''' </summary>
    ''' <param name="value">Hex value string representation</param>
    ''' <param name="len">Length of the string</param>
    ''' <returns>A Byte array representing a Hex value formally in a String format</returns>
    Private Function ConvertToByteArray(value As String, len As Integer) As Byte()
        Dim j As Integer
        Dim byteArr(len - 1) As Byte

        j = 0
        For i = 0 To byteArr.Length - 1
            byteArr(i) = Convert.ToByte(Mid(value, j + 1, 2), 16)
            j += 2
        Next

        Return byteArr
    End Function

    ''' <summary>
    ''' This sub procedure is the Apply All button that writes any filled valid value to the Transducer through the CANBUS channel.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim result As MsgBoxResult = MsgBox("You are about to write to the EEPROM's on all units that are listed. Do you still want to continue?",
                                                     MsgBoxStyle.Exclamation Or MsgBoxStyle.YesNo, "Warning")
        Dim i As Integer

        If result = MsgBoxResult.Yes Then
            PictureBox1.Visible = True ' Show loading GIF
            Call DisabledPanels() ' Disabling Panels so that user can not interrupt writing process or crash background worker thread.

            For i = 0 To Transducers.Length - 1
                If Transducers(i) IsNot Nothing Then
                    WriteToCanbus("H", i, CanbusConnect(Transducers(i).SerialNumber))
                    If i = 0 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage0.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage0.Text)
                                movingAverage0.Text = ""
                            End If
                            If sampleFrequency0.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency0.SelectedIndex)
                                sampleFrequency0.SelectedIndex = -1
                            End If
                            If outputPeriod0.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod0.Text)
                                outputPeriod0.Text = ""
                            End If
                        End If
                    ElseIf i = 1 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage1.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage1.Text)
                                movingAverage1.Text = ""
                            End If
                            If sampleFrequency1.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency1.SelectedIndex)
                                sampleFrequency1.SelectedIndex = -1
                            End If
                            If outputPeriod1.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod1.Text)
                                outputPeriod1.Text = ""
                            End If
                        End If
                    ElseIf i = 2 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage2.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage2.Text)
                                movingAverage2.Text = ""
                            End If
                            If sampleFrequency2.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency2.SelectedIndex)
                                sampleFrequency2.SelectedIndex = -1
                            End If
                            If outputPeriod2.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod2.Text)
                                outputPeriod2.Text = ""
                            End If
                        End If
                    ElseIf i = 3 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage3.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage3.Text)
                                movingAverage3.Text = ""
                            End If
                            If sampleFrequency3.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency3.SelectedIndex)
                                sampleFrequency3.SelectedIndex = -1
                            End If
                            If outputPeriod3.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod3.Text)
                                outputPeriod3.Text = ""
                            End If
                        End If
                    ElseIf i = 4 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage4.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage4.Text)
                                movingAverage4.Text = ""
                            End If
                            If sampleFrequency4.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency4.SelectedIndex)
                                sampleFrequency4.SelectedIndex = -1
                            End If
                            If outputPeriod4.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod4.Text)
                                outputPeriod4.Text = ""
                            End If
                        End If
                    ElseIf i = 5 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage5.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage5.Text)
                                movingAverage5.Text = ""
                            End If
                            If sampleFrequency5.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency5.SelectedIndex)
                                sampleFrequency5.SelectedIndex = -1
                            End If
                            If outputPeriod5.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod5.Text)
                                outputPeriod5.Text = ""
                            End If
                        End If
                    ElseIf i = 6 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage6.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage6.Text)
                                movingAverage6.Text = ""
                            End If
                            If sampleFrequency6.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency6.SelectedIndex)
                                sampleFrequency6.SelectedIndex = -1
                            End If
                            If outputPeriod6.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod6.Text)
                                outputPeriod6.Text = ""
                            End If
                        End If
                    ElseIf i = 7 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage7.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage7.Text)
                                movingAverage7.Text = ""
                            End If
                            If sampleFrequency7.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency7.SelectedIndex)
                                sampleFrequency7.SelectedIndex = -1
                            End If
                            If outputPeriod2.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod7.Text)
                                outputPeriod7.Text = ""
                            End If
                        End If
                    ElseIf i = 8 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage8.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage8.Text)
                                movingAverage8.Text = ""
                            End If
                            If sampleFrequency8.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency8.SelectedIndex)
                                sampleFrequency8.SelectedIndex = -1
                            End If
                            If outputPeriod8.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod8.Text)
                                outputPeriod8.Text = ""
                            End If
                        End If
                    ElseIf i = 9 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage9.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage9.Text)
                                movingAverage9.Text = ""
                            End If
                            If sampleFrequency9.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency9.SelectedIndex)
                                sampleFrequency9.SelectedIndex = -1
                            End If
                            If outputPeriod9.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod9.Text)
                                outputPeriod9.Text = ""
                            End If
                        End If
                    ElseIf i = 10 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage10.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage10.Text)
                                movingAverage10.Text = ""
                            End If
                            If sampleFrequency10.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency10.SelectedIndex)
                                sampleFrequency10.SelectedIndex = -1
                            End If
                            If outputPeriod10.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod10.Text)
                                outputPeriod10.Text = ""
                            End If
                        End If
                    ElseIf i = 11 Then
                        If Transducers(i) IsNot Nothing Then
                            If movingAverage11.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("A", i, CanbusConnect(Transducers(i).SerialNumber), movingAverage11.Text)
                                movingAverage11.Text = ""
                            End If
                            If sampleFrequency11.SelectedIndex > -1 Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("Q", i, CanbusConnect(Transducers(i).SerialNumber), sampleFrequency11.SelectedIndex)
                                sampleFrequency11.SelectedIndex = -1
                            End If
                            If outputPeriod11.Text <> "" Then
                                Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
                                WriteToCanbus("O", i, CanbusConnect(Transducers(i).SerialNumber), outputPeriod11.Text)
                                outputPeriod11.Text = ""
                            End If
                        End If
                    End If

                    WriteToCanbus("W", i, CanbusConnect(Transducers(i).SerialNumber))
                    WriteToCanbus("E", i, CanbusConnect(Transducers(i).SerialNumber))
                End If
            Next
        Else
            Exit Sub
        End If
        PictureBox1.Visible = False ' Hides loading GIF
        Call EnablePanels() ' Enable all UI features
    End Sub

    ''' <summary>
    ''' This sub procedure disables all GUI panel and button elements.
    ''' </summary>
    Private Sub DisabledPanels()
        Panel1.Enabled = False
        Panel2.Enabled = False
        Panel3.Enabled = False
        Panel4.Enabled = False
        Panel5.Enabled = False
        Panel6.Enabled = False
        Panel7.Enabled = False
        Panel8.Enabled = False
        Panel9.Enabled = False
        Panel10.Enabled = False
        Panel11.Enabled = False
        Panel12.Enabled = False
        Button1.Enabled = False
    End Sub

    ''' <summary>
    ''' This sub procedure enables all GUI panel and button elements.
    ''' </summary>
    Private Sub EnablePanels()
        Panel1.Enabled = True
        Panel2.Enabled = True
        Panel3.Enabled = True
        Panel4.Enabled = True
        Panel5.Enabled = True
        Panel6.Enabled = True
        Panel7.Enabled = True
        Panel8.Enabled = True
        Panel9.Enabled = True
        Panel10.Enabled = True
        Panel11.Enabled = True
        Panel12.Enabled = True
        Button1.Enabled = True
    End Sub

    ''' <summary>
    ''' This function finds the unit number or index of the Transducer object in the transducer array that matches a serial number.
    ''' </summary>
    ''' <param name="sn">Transducer serial number.</param>
    ''' <returns>Unit Number As Integer</returns>
    Private Function FindUnitNumber(sn As String) As Integer
        Dim unitNumber As Integer = -1
        Console.WriteLine("IN SN:" + sn)
        For i = 0 To Transducers.Length - 1
            If Transducers(i) IsNot Nothing Then
                If sn = Transducers(i).SerialNumber Then
                    unitNumber = i
                    'Console.WriteLine("OUT SN:" + Transducers(unitNumber).SerialNumber) ' For debugging only
                End If
            End If
        Next

        Return unitNumber
    End Function

    ''' <summary>
    ''' This function is in development. It checks the transducers array for a matching serial number and sends a boolean result back.
    ''' </summary>
    ''' <param name="sn">Serial number of the unit in question as a String</param>
    ''' <returns>Returns a Boolean value. Yes if the sn already exists and no if it is a new serial number found.</returns>
    Private Function UnitExist(sn As String) As Boolean
        Dim result As Boolean = False
        Dim snInt As Integer = Convert.ToInt32(sn)

        If snInt > 99999 And snInt <= 999999 Then
            For Each unit In Transducers
                If unit IsNot Nothing Then
                    If sn = unit.SerialNumber Then
                        result = True
                    End If
                End If
            Next
        End If

        Return result
    End Function

    ''' <summary>
    ''' This is in development. This function will automatically connect to the transducer in an
    ''' effort to auto sense Transducers connected to the CANBUS channel.
    ''' </summary>
    ''' <param name="sn">Units serial number as a String</param>
    Private Sub ConnectUnit(sn As String)
        Dim index As Integer = 0

        For Each unit In Transducers
            If unit IsNot Nothing Then
                index += 1
            End If
        Next

        Dim o_transducer As New Transducer With {
                    .SerialNumber = sn
                }
        Transducers(index) = o_transducer
        Call NewConnection(index) ' Calls the connection sub procedure
        ShowPanel(index + 2) ' Shows next transducer panel
    End Sub

    ''' <summary>
    ''' This sub procedure shows the a panel number and changes the cursor focus.
    ''' </summary>
    ''' <param name="panelNum">Panel Number as an Integer</param>
    Private Sub ShowPanel(panelNum As Integer)
        Select Case panelNum
            Case "1"
                Panel1.Invoke(Sub()
                                  Panel1.Visible = True
                                  serialNumber0.Focus()
                              End Sub)
            Case "2"
                Panel2.Invoke(Sub()
                                  Panel2.Visible = True
                                  serialNumber1.Focus()
                              End Sub)
            Case "3"
                Panel3.Invoke(Sub()
                                  Panel3.Visible = True
                                  serialNumber2.Focus()
                              End Sub)
            Case "4"
                Panel4.Invoke(Sub()
                                  Panel4.Visible = True
                                  serialNumber3.Focus()
                              End Sub)
            Case "5"
                Panel5.Invoke(Sub()
                                  Panel5.Visible = True
                                  serialNumber4.Focus()
                              End Sub)
            Case "6"
                Panel6.Invoke(Sub()
                                  Panel6.Visible = True
                                  serialNumber5.Focus()
                              End Sub)
            Case "7"
                Panel7.Invoke(Sub()
                                  Panel7.Visible = True
                                  serialNumber6.Focus()
                              End Sub)
            Case "8"
                Panel8.Invoke(Sub()
                                  Panel8.Visible = True
                                  serialNumber7.Focus()
                              End Sub)
            Case "9"
                Panel9.Invoke(Sub()
                                  Panel9.Visible = True
                                  serialNumber8.Focus()
                              End Sub)
            Case "10"
                Panel10.Invoke(Sub()
                                   Panel10.Visible = True
                                   serialNumber9.Focus()
                               End Sub)
            Case "11"
                Panel11.Invoke(Sub()
                                   Panel11.Visible = True
                                   serialNumber10.Focus()
                               End Sub)
            Case "12"
                Panel12.Invoke(Sub()
                                   Panel12.Visible = True
                                   serialNumber11.Focus()
                               End Sub)
        End Select
        Threading.Thread.Sleep(100) : Application.DoEvents() ' try 50(doesn't work)
    End Sub

    ''' <summary>
    ''' This is another function that checks to see if a unit exists already in the Tranducer object array called transducers.
    ''' </summary>
    ''' <param name="sn">Serial Number of the unit in question as a String</param>
    ''' <returns>Returns a Boolean - True = it exists in the Transducers array or 
    ''' False = it does not exist in the Transducers array.</returns>
    Private Function DoesUnitExist(sn As String) As Boolean
        Dim result As Boolean = False

        For Each unit In Transducers
            If unit IsNot Nothing Then
                If sn = unit.SerialNumber Then
                    result = True
                End If
            End If
        Next

        Return result
    End Function

    ''' <summary>
    ''' This function converts the NetID to a serial number.
    ''' </summary>
    ''' <param name="netID">The NetID as a UInteger</param>
    ''' <returns>The serial number of the unit that is transmitting information as a String.</returns>
    Private Function GetSerialNumber(netID As UInteger) As String
        Dim sn As String

        Dim temp1 As Integer = Convert.ToInt32(netID)
        sn = Convert.ToString(temp1 - 4194304)

        Return sn
    End Function

    ''' <summary>
    ''' This function gets the Sample Frequency value by index number.
    ''' </summary>
    ''' <param name="index">The index of the value that the user would like to set for the Transducer's sample frequency.</param>
    ''' <returns>Value as String</returns>
    Private Function GetSampleFrequency(index As Integer) As String
        Dim values() As Integer = {5, 10, 20, 40, 80, 160, 320, 640, 1000}

        Return values(index).ToString()
    End Function

    ''' <summary>
    ''' This Sub procedure is a delegate function that Reads Messages from the CANBUS channel everytime a message is in the buffer.
    ''' </summary>
    ''' <param name="ReadLen">Is an optional value if the message contains a value other than a command.</param>
    Private Sub ReadMessages(Optional ReadLen As Integer = 1)
        Dim CANMsg As TPCANMsg = Nothing
        Dim CANTimeStamp As TPCANTimestamp
        Dim stsResult As TPCANStatus
        Dim cmd As Char
        Dim sn As String = ""
        Dim cCalled As Boolean = False
        Dim packet As Integer = 0
        Dim readBuffer As New List(Of Byte())()
        Dim calRequestUnitSN = 0
        Dim unit As Integer = -1

        ' We read at least one time the queue looking for messages.
        ' If a message is found, we look again trying to find more.
        ' If the queue is empty or an error occurr, we get out from
        ' the dowhile statement.
        '
        Do
            ' We execute the "Read" function of the PCANBasic                
            '

            stsResult = PCANBasic.Read(m_PcanHandle, CANMsg, CANTimeStamp)
            ' A message was received
            ' We process the message(s)
            '
            If stsResult = TPCANStatus.PCAN_ERROR_OK Then
                'ProcessMessage(CANMsg, ReadLen) ' This line process the message once it is recieved by the buffer
                Console.WriteLine("Message was Read!")
                sn = GetSerialNumber(CANMsg.ID)
                unit = FindUnitNumber(sn)
                'If Not UnitConnected(sn) Then
                'If Not UnitExist(sn) Then
                '    'UnitExist(sn)
                '    ConnectUnit(sn)
                '    Console.WriteLine("Unit is new! %d", sn)
                'End If
                cmd = Convert.ToChar(CANMsg.DATA(0))

                If cmd = "C" Then
                    readBuffer.Add(CANMsg.DATA)
                    cCalled = True
                    calRequestUnitSN = sn
                    packet = 1
                ElseIf cCalled = True And sn = calRequestUnitSN Then
                    readBuffer.Add(CANMsg.DATA)
                    packet += 1
                    If packet = 6 Then
                        cCalled = False
                        Call ProcessMessage(readBuffer, unit)
                        Exit Do
                    End If
                Else
                    readBuffer.Add(CANMsg.DATA) 'This is unit output - Pressure Counts
                End If
                'readBuffer.Add(CANMsg.DATA) 'read for cmd C / unit serialNumber

            End If

        Loop While Not Convert.ToBoolean(stsResult And TPCANStatus.PCAN_ERROR_QRCVEMPTY)

        If readBuffer.Count > 0 Then
            'If unit = 0 Then
            unit = FindUnitNumber(sn)
            'End If
            If unit > -1 Then
                Call ProcessMessage(readBuffer, unit)
            End If
        End If
    End Sub

    ''' <summary>
    ''' This sub procedure pools the CANBU buffer for receiving events.
    ''' </summary>
    Private Sub CANReadThreadFunc()
        Dim iBuffer As UInt32
        Dim stsResult As TPCANStatus

        iBuffer = Convert.ToUInt32(m_ReceiveEvent.SafeWaitHandle.DangerousGetHandle().ToInt32())
        ' Sets the handle of the Receive-Event.
        '
        stsResult = PCANBasic.SetValue(m_PcanHandle, TPCANParameter.PCAN_RECEIVE_EVENT, iBuffer, CType(System.Runtime.InteropServices.Marshal.SizeOf(iBuffer), UInteger))

        If stsResult <> TPCANStatus.PCAN_ERROR_OK Then
            MessageBox.Show(GetFormatedError(stsResult), "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        While (1)
            If m_ReceiveEvent.WaitOne(50) Then
                ' Process Receive-Event using .NET Invoke function
                ' in order to interact with Winforms UI (calling the 
                ' function ReadMessages)
                ' 
                Me.Invoke(m_ReadDelegate)
            End If
        End While
    End Sub
End Class