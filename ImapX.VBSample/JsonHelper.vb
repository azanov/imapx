Imports System.IO
Imports System.Text
Imports System.Runtime.Serialization.Json

Public Class JsonHelper

    Public Shared Function From(Of T)(ByVal json As String) As T
        Dim obj As T = Activator.CreateInstance(Of T)()
        Using ms As MemoryStream = New MemoryStream(Encoding.Unicode.GetBytes(json))
            Dim serializer As New DataContractJsonSerializer(obj.GetType)
            Return DirectCast(serializer.ReadObject(ms), T)
        End Using
    End Function

    Public Shared Function [To](Of T)(ByVal obj As T) As String
        Dim serializer As New DataContractJsonSerializer(obj.GetType)
        Using ms As MemoryStream = New MemoryStream
            serializer.WriteObject(ms, obj)
            Return Encoding.Default.GetString(ms.ToArray)
        End Using
    End Function

End Class
