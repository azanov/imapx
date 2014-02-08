Public Class GoogleAccessToken
    ' Properties
    Public Property access_token As String

        Get
            Return Me._access_token
        End Get

        Set(ByVal value As String)
            Me._access_token = value
        End Set
    End Property

    Public Property expires_in As String

        Get
            Return Me._expires_in
        End Get

        Set(ByVal value As String)
            Me._expires_in = value
        End Set
    End Property

    Public Property refresh_token As String

        Get
            Return Me._refresh_token
        End Get

        Set(ByVal value As String)
            Me._refresh_token = value
        End Set
    End Property


    ' Fields

    Private _access_token As String

    Private _expires_in As String

    Private _refresh_token As String
End Class



