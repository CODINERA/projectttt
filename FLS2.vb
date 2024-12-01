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

            ' Check if user exists in the survey table
            Dim checkQuery As String = "SELECT COUNT(*) FROM surveys WHERE user_id = @UserID"
            Dim checkCmd As New MySqlCommand(checkQuery, sqlConn)
            checkCmd.Parameters.AddWithValue("@UserID", UserID)
            Dim userExists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())

            If userExists = 0 Then
                MessageBox.Show("User does not exist in the survey table. Please check your data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Update query to modify weekly and monthly goals
            Dim query As String = "UPDATE surveys SET weekly_goal = @WeeklyGoal, monthly_goal = @MonthlyGoal WHERE user_id = @UserID"
            Dim cmd = New MySqlCommand(query, sqlConn)
            cmd.Parameters.AddWithValue("@UserID", UserID)
            cmd.Parameters.AddWithValue("@WeeklyGoal", CSng(weekly_goal)) ' Convert to Float
            cmd.Parameters.AddWithValue("@MonthlyGoal", CSng(monthly_goal)) ' Convert to Float

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

    ' Weekly Goal Button Clicks
    Private Sub Guna2Button1_Click(sender As Object, e As EventArgs) Handles Guna2Button1.Click
        weekly_goal = 0.25
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub Guna2Button3_Click(sender As Object, e As EventArgs) Handles Guna2Button3.Click
        weekly_goal = 0.5
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub Guna2Button2_Click(sender As Object, e As EventArgs) Handles Guna2Button2.Click
        weekly_goal = 0.75
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub Guna2Button4_Click(sender As Object, e As EventArgs) Handles Guna2Button4.Click
        weekly_goal = 1
        UpdateButtonAppearance(sender)
    End Sub

    ' Monthly Goal Button Clicks
    Private Sub Guna2Button5_Click(sender As Object, e As EventArgs) Handles Guna2Button5.Click
        monthly_goal = 0.25
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub Guna2Button7_Click(sender As Object, e As EventArgs) Handles Guna2Button7.Click
        monthly_goal = 0.5
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub Guna2Button6_Click(sender As Object, e As EventArgs) Handles Guna2Button6.Click
        monthly_goal = 0.75
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub Guna2Button8_Click(sender As Object, e As EventArgs) Handles Guna2Button8.Click
        monthly_goal = 1
        UpdateButtonAppearance(sender)
    End Sub

    ' Update Button Appearance Function
    Private Sub UpdateButtonAppearance(clickedButton As Object)
        Try
            ' Reset all buttons to default appearance
            ResetButtonAppearance()

            ' Check the type of the clicked button and cast accordingly
            If TypeOf clickedButton Is Button Then
                Dim button As Button = CType(clickedButton, Button)
                button.BackColor = Color.SeaGreen ' Highlight with dark orange background
                button.ForeColor = Color.Transparent ' Change text color to white
            ElseIf TypeOf clickedButton Is Guna.UI2.WinForms.Guna2Button Then
                Dim gunaButton As Guna.UI2.WinForms.Guna2Button = CType(clickedButton, Guna.UI2.WinForms.Guna2Button)
                gunaButton.FillColor = Color.SeaGreen ' Change the fill color for Guna2Button
                gunaButton.ForeColor = Color.Transparent ' Change text color to white
            End If
        Catch ex As Exception
            MessageBox.Show("Error in UpdateButtonAppearance: " & ex.Message & vbCrLf & ex.StackTrace)
        End Try
    End Sub

    ' Function to reset button appearance
    Private Sub ResetButtonAppearance()
        ' Reset the appearance of all buttons to the default state
        Guna2Button1.BorderRadius = 10
        Guna2Button2.BorderRadius = 10
        Guna2Button3.BorderRadius = 10
        Guna2Button4.BorderRadius = 10
        Guna2Button5.BorderRadius = 10
        Guna2Button6.BorderRadius = 10
        Guna2Button7.BorderRadius = 10
        Guna2Button8.BorderRadius = 10

    End Sub

End Class




