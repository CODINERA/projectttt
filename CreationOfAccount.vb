Imports Guna.UI2.WinForms
Imports MySql.Data.MySqlClient
Imports Org.BouncyCastle.Crypto.Generators

Public Class CreationOfAccount
    Dim sqlConn As New MySqlConnection
    Dim sqlCmd As New MySqlCommand
    Dim sqlRd As MySqlDataReader
    Dim sqlDt As New DataTable
    Dim DtA As New MySqlDataAdapter

    Dim Server As String = "localhost"
    Dim username As String = "root"
    Dim password As String = ""
    Dim database As String = "fitcheck"
    Private butmap As Bitmap

    ' Declare the connection string variable
    Private connectionString As String = "server=" + Server + ";user id=" + username + ";password=" + password + ";database=" + database + ";"

    Private Sub CreationOfAccount_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        sqlConn.ConnectionString = connectionString
        Try
            sqlConn.Open()
            sqlConn.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            sqlConn.Dispose()
        End Try

        ' Set the PasswordChar to hide the password text
        txtpassword.PasswordChar = "•"c
        txtconfirmpass.PasswordChar = "•"c
    End Sub

    Private Sub Guna2TextBox4_TextChanged(sender As Object, e As EventArgs) Handles txtconfirmpass.TextChanged
        ValidatePasswords()
    End Sub

    Private Sub Guna2TextBox3_TextChanged(sender As Object, e As EventArgs) Handles txtpassword.TextChanged
        ValidatePasswords()
    End Sub

    Private Sub ValidatePasswords()
        If txtconfirmpass.Text <> txtpassword.Text Then
            txtconfirmpass.ForeColor = Color.Red
        Else
            txtconfirmpass.ForeColor = Color.Black
        End If
    End Sub

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        CreateAccount()
    End Sub

    ' Updated function to check if the inputs are valid
    Private Function ValidateInputs() As Boolean
        If String.IsNullOrWhiteSpace(txtname.Text) Then
            MessageBox.Show("Username cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtemail.Text) Then
            MessageBox.Show("Email cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtpassword.Text) Then
            MessageBox.Show("Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtconfirmpass.Text) Then
            MessageBox.Show("Confirm Password cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        Return True
    End Function

    ' Updated function for account creation with input validation and email check
    Private Sub CreateAccount()
        Dim name As String = txtname.Text
        Dim email As String = txtemail.Text
        Dim password As String = txtpassword.Text
        Dim confirmPassword As String = txtconfirmpass.Text

        ' Validate inputs before proceeding
        If Not ValidateInputs() Then
            Return ' Stop if inputs are invalid
        End If

        ' Check if the passwords match
        If password <> confirmPassword Then
            MessageBox.Show("Passwords do not match. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        ' Create the account and get the user ID
        Dim userID As Integer = SaveAccountData(name, email, password)

        If userID = -1 Then
            ' If userID is -1, that means there was an error, so stop further processing
            Return
        End If

        ' Clear the fields after successful account creation
        txtname.Clear()
        txtemail.Clear()
        txtpassword.Clear()
        txtconfirmpass.Clear()

        ' Show the next form
        Dim surveyForm As New FLS1()
        surveyForm.UserID = userID
        surveyForm.Show()
        Me.Hide()
    End Sub

    ' Function to save the account data and check if email is already registered
    Private Function SaveAccountData(name As String, email As String, password As String) As Integer
        Dim userID As Integer = -1
        Try
            Using connection As New MySqlConnection(connectionString)
                connection.Open()

                Dim transaction As MySqlTransaction = connection.BeginTransaction()

                Try
                    ' Check if the email already exists
                    Dim checkQuery As String = "SELECT COUNT(*) FROM USERS WHERE Email = @Email"
                    Using checkCmd As New MySqlCommand(checkQuery, connection, transaction)
                        checkCmd.Parameters.AddWithValue("@Email", email)
                        Dim count As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

                        If count > 0 Then
                            MessageBox.Show("This email is already registered. Please use a different email.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            Return -1 ' Return -1 if email is already registered
                        End If
                    End Using

                    ' Insert query for user data
                    Dim query1 As String = "INSERT INTO USERS (Username, Email, Password) VALUES (@Username, @Email, @Password); SELECT LAST_INSERT_ID();"
                    Using command1 As New MySqlCommand(query1, connection, transaction)
                        ' Hash the password before inserting
                        Dim hashedPassword As String = BCrypt.Net.BCrypt.HashPassword(password)
                        command1.Parameters.AddWithValue("@Username", name)
                        command1.Parameters.AddWithValue("@Email", email)
                        command1.Parameters.AddWithValue("@Password", hashedPassword)

                        ' Execute and get the last inserted ID (userID)
                        userID = Convert.ToInt32(command1.ExecuteScalar())
                    End Using

                    ' Commit the transaction
                    transaction.Commit()

                    MessageBox.Show("Account created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As MySqlException
                    ' Rollback transaction in case of error
                    transaction.Rollback()
                    MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End Using
        Catch ex As Exception
            MessageBox.Show("Unexpected error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

        Return userID
    End Function

    ' Back to login page when the label is clicked
    Private Sub Guna2HtmlLabel5_Click(sender As Object, e As EventArgs) Handles Guna2HtmlLabel5.Click
        Dim signInForm As New Form1()
        signInForm.Show()
        Me.Hide()
    End Sub
End Class








