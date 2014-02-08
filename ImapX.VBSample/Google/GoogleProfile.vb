Public Class GoogleProfile
    ' Properties
    Public Property email As String
        Get
            Return Me._email
        End Get
        Set(ByVal value As String)
            Me._email = value
        End Set
    End Property

    Public Property id As String
        Get
            Return Me._id
        End Get
        Set(ByVal value As String)
            Me._id = value
        End Set
    End Property


    ' Fields
    Private _email As String
    Private _id As String
End Class


