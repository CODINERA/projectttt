Imports Guna.UI2.WinForms

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Guna2TextBox2.PasswordChar = "•"c
    End Sub

    Private Sub Guna2CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles Guna2CheckBox1.CheckedChanged
        If Guna2CheckBox1.Checked Then
            ' Show the password as plain text
            Guna2TextBox2.PasswordChar = ChrW(0)
        Else
            ' Hide the password with an asterisk (*)
            Guna2TextBox2.PasswordChar = "•"c
        End If
    End Sub

End Class
