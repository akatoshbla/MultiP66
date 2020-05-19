'  Transducer.vb
'
'  ~~~~~~~~~~~~
'
'  Transducer Class Object
'
'  ~~~~~~~~~~~~
'
'  ------------------------------------------------------------------
'  Author : David Kopp
'  Last change: 02.03.2020 Kopp
'
'  Language: VB .NET
'  ------------------------------------------------------------------
'   This class is an object that houses the information for the transducers.
Public Class Transducer
    Private m_SerialNumber As String = ""
    Private m_ModelNumber As String = ""
    Private m_CalDate As String = ""
    Private M_FSPressure As String = ""
    Private m_FSCounts As String = ""
    Private m_ZeroCounts As String = ""
    Private m_MovingAverage As String = ""
    Private m_SampleFrequency As String = ""
    Private m_OutputPeriod As String = ""
    Private m_Value(8) As Boolean

    Public Property SerialNumber() As String
        Get
            Return m_SerialNumber
        End Get
        Set(value As String)
            m_SerialNumber = value
            m_Value(0) = True
        End Set
    End Property
    Public Property ModelNumber() As String
        Get
            Return m_ModelNumber
        End Get
        Set(value As String)
            m_ModelNumber = value
            m_Value(1) = True
        End Set
    End Property
    Public Property CalDate() As String
        Get
            Return m_CalDate
        End Get
        Set(value As String)
            m_CalDate = value
            m_Value(2) = True
        End Set
    End Property
    Public Property FSPressure() As String
        Get
            Return M_FSPressure
        End Get
        Set(value As String)
            M_FSPressure = value
            m_Value(3) = True
        End Set
    End Property
    Public Property FSCounts() As String
        Get
            Return m_FSCounts
        End Get
        Set(value As String)
            m_FSCounts = value
            m_Value(4) = True
        End Set
    End Property
    Public Property ZeroCounts() As String
        Get
            Return m_ZeroCounts
        End Get
        Set(value As String)
            m_ZeroCounts = value
            m_Value(5) = True
        End Set
    End Property
    Public Property MovingAverage() As String
        Get
            Return m_MovingAverage
        End Get
        Set(value As String)
            m_MovingAverage = value
            m_Value(6) = True
        End Set
    End Property
    Public Property SampleFrequency() As String
        Get
            Return m_SampleFrequency
        End Get
        Set(value As String)
            m_SampleFrequency = value
            m_Value(7) = True
        End Set
    End Property
    Public Property OutputPeriod() As String
        Get
            Return m_OutputPeriod
        End Get
        Set(value As String)
            m_OutputPeriod = value
            m_Value(8) = True
        End Set
    End Property

    ''' <summary>
    ''' This function checks to make sure all the information has been gathered and results in a boolean.
    ''' </summary>
    ''' <returns>A Boolean - True = has all values and is ready to be displayed or false = it is not ready to be displayed.</returns>
    Public Function DisplayRdy() As Boolean
        Dim result As Boolean = True

        For Each element As Boolean In m_Value
            If Not element Then
                result = False
            End If
        Next

        Return result
    End Function
End Class
