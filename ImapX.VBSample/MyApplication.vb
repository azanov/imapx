Imports System.IO

Namespace My

    Partial Friend Class MyApplication

        Public Shared ImapClient As ImapClient

        Protected Overrides Function OnStartup(eventArgs As ApplicationServices.StartupEventArgs) As Boolean

            Dim tmpPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "tmp")

            Try
                If Directory.Exists(tmpPath) Then
                    Directory.Delete(tmpPath, True)
                End If
            Catch
            End Try

            Directory.CreateDirectory(tmpPath)

            Return MyBase.OnStartup(eventArgs)

        End Function

        Protected Overrides Function OnUnhandledException(e As ApplicationServices.UnhandledExceptionEventArgs) As Boolean

            Using frm = New FrmError(e.Exception)
                frm.ShowDialog()
            End Using


            Return MyBase.OnUnhandledException(e)
        End Function

    End Class


End Namespace

