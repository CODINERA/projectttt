Imports MySql.Data.MySqlClient
Imports Guna.UI2.WinForms

Public Class FLS2
    Public Property UserID As Integer ' Add UserID property to receive the user ID from FLS1

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

    Dim weekly_goal As Double
    Dim monthly_goal As Double
    Dim btn As Guna2Button

    Private Sub FLS2Form_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        sqlConn.ConnectionString = "server=" + Server + ";user id=" + username + ";password=" + password + ";database=" + database + ";"
    End Sub

    Private Sub BackButton_Click(sender As Object, e As EventArgs) Handles backButton.Click
        Dim fls1Form As New FLS1()
        fls1Form.UserID = UserID ' Pass UserID back to FLS1 if needed
        fls1Form.Show()
        Me.Hide()
    End Sub

    Private Sub SubmitButton_Click(sender As Object, e As EventArgs) Handles SubmitButton.Click
        If Not ValidateInputs() Then
            Return
        End If

        Try
            sqlConn.Open()
            Dim query As String = "UPDATE SURVEY SET weekly_goal = @WeeklyGoal, monthly_goal = @MonthlyGoal WHERE user_id = @UserID"

            Dim cmd = New MySqlCommand(query, sqlConn)
            cmd.Parameters.AddWithValue("@UserID", UserID)
            cmd.Parameters.AddWithValue("@WeeklyGoal", weekly_goal)
            cmd.Parameters.AddWithValue("@MonthlyGoal", monthly_goal)

            cmd.ExecuteNonQuery()
            MessageBox.Show("Weekly and Monthly goals saved successfully!")

            ' Navigate to the home page or next step
            Dim homePageForm As New Form1()
            homePageForm.Show()
            Me.Hide()
        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            sqlConn.Close()
        End Try
    End Sub

    Private Function ValidateInputs() As Boolean
        If weekly_goal <= 0 Then
            MessageBox.Show("Weekly goal cannot be empty or zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If monthly_goal <= 0 Then
            MessageBox.Show("Monthly goal cannot be empty or zero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        Return True
    End Function

    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        weekly_goal = 0.25
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        weekly_goal = 0.5
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        weekly_goal = 0.75
    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        weekly_goal = 1
    End Sub

    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click
        weekly_goal = 0.25
    End Sub

    Private Sub Guna2Button7_Click(sender As Object, e As EventArgs) Handles Guna2Button7.Click
        weekly_goal = 0.5
    End Sub

    Private Sub Guna2Button6_Click(sender As Object, e As EventArgs) Handles Guna2Button6.Click
        weekly_goal = 0.75
    End Sub

    Private Sub Guna2Button8_Click(sender As Object, e As EventArgs) Handles Guna2Button8.Click
        weekly_goal = 1
    End Sub

End Class




