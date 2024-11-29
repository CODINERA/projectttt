Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient
Imports Microsoft.VisualBasic.ApplicationServices
Imports Microsoft.Win32
Imports Org.BouncyCastle.Crypto.Generators

Public Class Form1
    Private connectionString As String = "server=localhost;user id=root;password=;database=fitcheck;"

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtPassword.PasswordChar = "•"c

        Try
            Using sqlConn As New MySqlConnection(connectionString)
                sqlConn.Open()
                ''  MessageBox.Show("Connection successful")
                sqlConn.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        ' Validate user inputs
        If Not ValidateInputs() Then
            Return
        End If

        Dim email As String = txtEmail.Text
        Dim password As String = txtPassword.Text

        If Not CheckAccountExists(email, password) Then
            MessageBox.Show("Account doesn't exist or invalid credentials.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            If Not IsFLS1Completed(email) Then
                ' Redirect to FLS1 form
                Dim fls1Form As New FLS1()
                fls1Form.Show()
                Me.Hide()
            ElseIf Not IsFLS2Completed(email) Then
                ' Redirect to FLS2 form
                Dim fls2Form As New FLS2()
                fls2Form.Show()
                Me.Hide()
            Else
                ' Redirect to Homepage
                Dim homepage As New FLS2()
                homepage.Show()
                Me.Hide()
            End If
        End If
    End Sub


    Private Function ValidateInputs() As Boolean
        If String.IsNullOrWhiteSpace(txtEmail.Text) Then
            MessageBox.Show("Username cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtPassword.Text) Then
            MessageBox.Show("Email cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        Return True
    End Function

    Private Function CheckAccountExists(email As String, password As String) As Boolean
        Dim hashedPassword As String = ""
        Using connection As New MySqlConnection(connectionString)
            connection.Open()

            ' Retrieve the hashed password for the email
            Dim query As String = "SELECT Password FROM users WHERE Email = @Email"
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@Email", email)
                hashedPassword = Convert.ToString(command.ExecuteScalar())
            End Using
        End Using

        ' If no password is found, the account doesn't exist
        If String.IsNullOrEmpty(hashedPassword) Then
            Return False
        End If

        ' Verify the entered password with the stored hashed password
        Return BCrypt.Net.BCrypt.Verify(password, hashedPassword)
    End Function


    Private Function IsFLS1Completed(email As String) As Boolean
        Using connection As New MySqlConnection(connectionString)
            connection.Open()
            Dim query As String = "SELECT FLS1Status FROM users WHERE Email = @Email"
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@Email", email)
                Dim fls1Status As Boolean = Convert.ToBoolean(command.ExecuteScalar())
                Return fls1Status
            End Using
        End Using
    End Function

    Private Function IsFLS2Completed(email As String) As Boolean
        Using connection As New MySqlConnection(connectionString)
            connection.Open()
            Dim query As String = "SELECT FLS2Status FROM users WHERE Email = @Email"
            Using command As New MySqlCommand(query, connection)
                command.Parameters.AddWithValue("@Email", email)
                Dim fls2Status As Boolean = Convert.ToBoolean(command.ExecuteScalar())
                Return fls2Status
            End Using
        End Using
    End Function

    Private Sub lblSignUp_Click(sender As Object, e As EventArgs) Handles lblSignUp.Click
        Dim creationOfAccountForm As New CreationOfAccount()
        creationOfAccountForm.Show()
        Me.Hide()
    End Sub

    Private Sub chkShowPassword_CheckedChanged(sender As Object, e As EventArgs) Handles chkShowPassword.CheckedChanged
        If chkShowPassword.Checked Then
            txtPassword.PasswordChar = ControlChars.NullChar
        Else
            txtPassword.PasswordChar = "•"c
        End If
    End Sub

End Class


