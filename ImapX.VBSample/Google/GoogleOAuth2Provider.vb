Imports System.Net
Imports System.IO
Imports System.Text

Public Class GoogleOAuth2Provider
    ' Methods
    Public Shared Function BuildAuthenticationUri() As Uri
        Return New Uri(String.Format("{0}?response_type={1}&client_id={2}&redirect_uri={3}&scope={4}%20{5}", New Object() {"https://accounts.google.com/o/oauth2/auth", "code", "819410764762.apps.googleusercontent.com", "urn:ietf:wg:oauth:2.0:oob", "https://www.googleapis.com/auth/userinfo.email", "https://mail.google.com/"}))
    End Function

    Public Shared Function GetAccessToken(ByVal code As String) As GoogleAccessToken
        Dim postData As String = String.Format("client_id={0}&client_secret={1}&grant_type=authorization_code&code={2}&redirect_uri={3}", New Object() {"819410764762.apps.googleusercontent.com", "vz1VGPT2meoSJ5RXBco-56aR", code, "urn:ietf:wg:oauth:2.0:oob"})
        Dim request As HttpWebRequest = DirectCast(WebRequest.Create("https://accounts.google.com/o/oauth2/token"), HttpWebRequest)
        request.KeepAlive = False
        request.ProtocolVersion = HttpVersion.Version10
        request.Method = "POST"
        Dim postBytes As Byte() = Encoding.UTF8.GetBytes(postData)
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = postBytes.Length
        Dim requestStream As Stream = request.GetRequestStream
        requestStream.Write(postBytes, 0, postBytes.Length)
        requestStream.Close()
        Dim response As HttpWebResponse = DirectCast(request.GetResponse, HttpWebResponse)
        Using responseReader As StreamReader = New StreamReader(response.GetResponseStream)
            Return JsonHelper.From(Of GoogleAccessToken)(responseReader.ReadToEnd)
        End Using
    End Function

    Public Shared Function GetUserProfile(ByVal accessToken As GoogleAccessToken) As GoogleProfile
        Using w As WebClient = New WebClient
            Dim url As String = String.Format("{0}?access_token=", "https://www.googleapis.com/oauth2/v1/userinfo", accessToken.access_token)
            w.Headers.Add("Authorization", String.Format("Bearer  {0}", accessToken.access_token))
            w.Encoding = Encoding.UTF8
            Return JsonHelper.From(Of GoogleProfile)(w.DownloadString(url))
        End Using
    End Function


    ' Fields
    Private Const AUTH_URI As String = "https://accounts.google.com/o/oauth2/auth"
    Private Const CLIENT_ID As String = "819410764762.apps.googleusercontent.com"
    Private Const CLIENT_SECRET As String = "vz1VGPT2meoSJ5RXBco-56aR"
    Private Const GET_ACCESS_TOKEN_URI As String = "https://accounts.google.com/o/oauth2/token"
    Private Const GET_USER_PROFILE_URI As String = "https://www.googleapis.com/oauth2/v1/userinfo"
    Private Const REDIRECT_URI As String = "urn:ietf:wg:oauth:2.0:oob"
    Private Const RESPONSE_TYPE_CODE As String = "code"
    Private Const SCOPE_GMAIL As String = "https://mail.google.com/"
    Private Const SCOPE_USER_EMAIL As String = "https://www.googleapis.com/auth/userinfo.email"
End Class


