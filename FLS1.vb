Imports MySql.Data.MySqlClient
Imports Guna.UI2.WinForms

Public Class FLS1
    Public Property UserID As Integer ' Property to hold the user ID

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

    Dim fitness_goal As String
    Dim reason As String
    Dim fitness_act As String
    Dim BMI As Double
    Dim height_cm As Double
    Dim weight As Double

    Private Sub FLS1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        sqlConn.ConnectionString = "server=" + Server + ";user id=" + username + ";password=" + password + ";database=" + database + ";"
    End Sub

    Private Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            sqlConn.Open()
            Dim query As String = "INSERT INTO SURVEYS (user_id, fitness_goals, reason, fitness_act) VALUES (@UserID, @FitnessGoal, @Reason, @FitnessAct)"

            Dim cmd = New MySqlCommand(query, sqlConn)
            cmd.Parameters.AddWithValue("@UserID", UserID)
            cmd.Parameters.AddWithValue("@FitnessGoal", fitness_goal)
            cmd.Parameters.AddWithValue("@Reason", reason)
            cmd.Parameters.AddWithValue("@FitnessAct", fitness_act)

            cmd.ExecuteNonQuery()
            MessageBox.Show("Survey data saved successfully!")

            Dim surveyForm2 As New FLS2()
            surveyForm2.UserID = UserID ' Pass UserID to the next form
            surveyForm2.Show()
            Me.Hide()
        Catch ex As MySqlException
            MessageBox.Show("Error: " & ex.Message)
        Finally
            sqlConn.Close()
        End Try
    End Sub

    Private Function CheckUserIDExists(userID As Integer) As Boolean
        Try
            Dim query As String = "SELECT COUNT(*) FROM users WHERE user_id = @UserID"
            Dim cmd As New MySqlCommand(query, sqlConn)
            cmd.Parameters.AddWithValue("@UserID", userID)
            sqlConn.Open()
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
            Return count > 0
        Catch ex As MySqlException
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Finally
            sqlConn.Close()
        End Try
    End Function



    Private Function ValidateInputs() As Boolean
        If String.IsNullOrWhiteSpace(txtName.Text) Then
            MessageBox.Show("Username cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtSex.Text) Then
            MessageBox.Show("Sex cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtAge.Text) Then
            MessageBox.Show("Age cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtCountry.Text) Then
            MessageBox.Show("Country cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtHeight.Text) Then
            MessageBox.Show("Height cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtWeight.Text) Then
            MessageBox.Show("Weight cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtGoalWeight.Text) Then
            MessageBox.Show("Goal Weight cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        If String.IsNullOrWhiteSpace(txtBMI.Text) Then
            MessageBox.Show("BMI cannot be empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End If
        Return True
    End Function

    ' Example button click events to set enum values
    Private Sub btnLoseWeight_Click(sender As Object, e As EventArgs) Handles btnLoseWeight.Click
        fitness_goal = "lose weight"
    End Sub

    Private Sub btnMaintainWeight_Click(sender As Object, e As EventArgs) Handles btnMaintainWeight.Click
        fitness_goal = "maintain weight"
    End Sub

    Private Sub btnGainWeight_Click(sender As Object, e As EventArgs) Handles btnGainWeight.Click
        fitness_goal = "gain weight"
    End Sub

    Private Sub btnGainMuscle_Click(sender As Object, e As EventArgs) Handles btnGainMuscle.Click
        fitness_goal = "gain muscle"
    End Sub

    ' Similarly, add button click events for reason and fitness_act
    Private Sub btnOption1_Click(sender As Object, e As EventArgs) Handles btnOption1.Click
        reason = "health"
    End Sub

    Private Sub btnOption2_Click(sender As Object, e As EventArgs) Handles btnOption2.Click
        reason = "appearance"
    End Sub

    Private Sub btnOption3_Click(sender As Object, e As EventArgs) Handles btnOption3.Click
        reason = "performance"
    End Sub

    Private Sub btnOption4_Click(sender As Object, e As EventArgs) Handles btnOption4.Click
        reason = "other"
    End Sub

    Private Sub btnNewbie_Click(sender As Object, e As EventArgs) Handles btnNewbie.Click
        fitness_act = "newbie"
    End Sub

    Private Sub btnIntermediate_Click(sender As Object, e As EventArgs) Handles btnIntermediate.Click
        fitness_act = "intermediate"
    End Sub

    Private Sub btnAdvance_Click(sender As Object, e As EventArgs) Handles btnAdvance.Click
        fitness_act = "advance"
    End Sub

    Private Sub btnPro_Click(sender As Object, e As EventArgs) Handles btnPro.Click
        fitness_act = "pro"
    End Sub

    Private Sub txtBMI_TextChanged(sender As Object, e As EventArgs) Handles txtWeight.TextChanged, txtHeight.TextChanged
        Dim weight As Double
        Dim height_cm As Double

        ' Validate and parse weight
        If Not Double.TryParse(txtWeight.Text, weight) OrElse weight <= 0 Then
            txtBMI.Text = "Invalid weight"
            Return
        End If

        ' Validate and parse height
        If Not Double.TryParse(txtHeight.Text, height_cm) OrElse height_cm <= 0 Then
            txtBMI.Text = "Invalid height"
            Return
        End If

        ' Calculate BMI
        Dim BMI As Double = (weight / (height_cm / 100) ^ 2)
        txtBMI.Text = BMI.ToString("F2") ' Format to two decimal places
    End Sub

End Class



