
Imports System.Net.Mail

Public Class Form2

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Me.Dispose()

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim strData2 As Date = Today
        Dim Mail1 As New MailMessage
        Dim Mail2 As New MailMessage
        Dim Mail3 As New MailMessage
        Dim Mail4 As New MailMessage


        ProgressBar1.Value = 20
        If CheckBox2.Checked = True Then
            Mail2.To.Add(CheckBox2.Text)
            Mail2.Subject = "asd2"
            Mail2.From = New MailAddress("borek371@o2.pl")
            Mail2.Body = ""
            Dim attachment2 As System.Net.Mail.Attachment
            attachment2 = New System.Net.Mail.Attachment("C:\Users\Hym\Downloads\backup-" + strData2 + ".xlsx")
            Mail2.Attachments.Add(attachment2)
            Dim SMTP2 As New SmtpClient("poczta.o2.pl")
            SMTP2.EnableSsl = True
            SMTP2.Credentials = New System.Net.NetworkCredential("borek371@o2.pl", "Boreczek6")
            SMTP2.Port = "587"
            SMTP2.Send(Mail2)
            

        End If
        ProgressBar1.Value = 40
        If CheckBox3.Checked = True Then
            Mail3.To.Add(CheckBox3.Text)
            Mail3.Subject = "asd3"
            Mail3.From = New MailAddress("borek371@o2.pl")
            Mail3.Body = ""
            Dim attachment3 As System.Net.Mail.Attachment
            attachment3 = New System.Net.Mail.Attachment("C:\Users\Hym\Downloads\backup-" + strData2 + ".xlsx")
            Mail3.Attachments.Add(attachment3)
            Dim SMTP3 As New SmtpClient("poczta.o2.pl")
            SMTP3.EnableSsl = True
            SMTP3.Credentials = New System.Net.NetworkCredential("borek371@o2.pl", "Boreczek6")
            SMTP3.Port = "587"
            SMTP3.Send(Mail3)
          

        End If
        ProgressBar1.Value = 70

        MessageBox.Show("Completed")
        Me.Dispose()

    End Sub

End Class