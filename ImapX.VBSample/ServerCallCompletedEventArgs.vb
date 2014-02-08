Public Class ServerCallCompletedEventArgs
    Inherits EventArgs

    Private _Arg As Object
    Private _Exception As Exception
    Private _Result As Boolean

    Public Sub New(Optional ByVal result As Boolean = True, Optional ByVal exception As Exception = Nothing, Optional ByVal arg As Object = Nothing)
        Me.Result = result
        Me.Exception = exception
        Me.Arg = arg
    End Sub

    Public Property Arg As Object
        Get
            Return _Arg
        End Get
        Set(value As Object)
            _Arg = value
        End Set
    End Property

    Public Property Exception As Exception
        Get
            Return _Exception
        End Get
        Set(value As Exception)
            _Exception = value
        End Set
    End Property

    Public Property Result As Boolean
        Get
            Return _Result
        End Get
        Set(value As Boolean)
            _Result = value
        End Set
    End Property

End Class


