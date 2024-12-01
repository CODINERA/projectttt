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
            ' Validate inputs before proceeding
            If Not ValidateInputs() Then
                Return ' Exit the function if validation fails
            End If

            ' Debugging UserID value before checking in the database
            MessageBox.Show("UserID being checked: " & UserID.ToString()) ' Debugging the UserID being passed

            ' First, check if the UserID exists in the users table
            If Not CheckUserIDExists(UserID) Then
                MessageBox.Show("User ID does not exist. Please register first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return ' Exit the function if UserID doesn't exist
            End If

            ' If UserID exists, proceed with inserting the survey data
            sqlConn.Open()

            ' Modified query to insert into USERS table with the additional fields
            Dim query As String = "UPDATE USERS SET 
                               sex = @Sex, age = @Age, country = @Country, height = @Height, weight = @Weight, goal_weight = @GoalWeight, bmi = @BMI 
                               WHERE user_id = @UserID"

            ' Prepare command
            Dim cmd = New MySqlCommand(query, sqlConn)
            cmd.Parameters.AddWithValue("@UserID", UserID)
            cmd.Parameters.AddWithValue("@Sex", txtSex.Text) ' Assuming txtSex is the TextBox for sex
            cmd.Parameters.AddWithValue("@Age", txtAge.Text) ' Assuming txtAge is the TextBox for age
            cmd.Parameters.AddWithValue("@Country", txtCountry.Text) ' Assuming txtCountry is the TextBox for country
            cmd.Parameters.AddWithValue("@Height", txtHeight.Text) ' Assuming txtHeight is the TextBox for height
            cmd.Parameters.AddWithValue("@Weight", txtWeight.Text) ' Assuming txtWeight is the TextBox for weight
            cmd.Parameters.AddWithValue("@GoalWeight", txtGoalWeight.Text) ' Assuming txtGoalWeight is the TextBox for goal weight
            cmd.Parameters.AddWithValue("@BMI", txtBMI.Text) ' Assuming txtBMI is the TextBox for BMI

            ' Execute the query to insert data into USERS table
            cmd.ExecuteNonQuery()

            ' Proceed with inserting survey data
            Dim surveyQuery As String = "INSERT INTO SURVEYS (user_id, fitness_goals, reason, fitness_act) 
                                    VALUES (@UserID, @FitnessGoal, @Reason, @FitnessAct)"
            Dim cmdSurvey = New MySqlCommand(surveyQuery, sqlConn)
            cmdSurvey.Parameters.AddWithValue("@UserID", UserID)
            cmdSurvey.Parameters.AddWithValue("@FitnessGoal", fitness_goal)
            cmdSurvey.Parameters.AddWithValue("@Reason", reason)
            cmdSurvey.Parameters.AddWithValue("@FitnessAct", fitness_act)

            cmdSurvey.ExecuteNonQuery()
            MessageBox.Show("Survey data saved successfully!")

            ' After successful data insertion, move to the next form
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
            ' Debugging the UserID and query before executing
            MessageBox.Show("Checking UserID in database: " & userID.ToString()) ' Debugging the UserID being checked

            ' Construct the query to check if the user ID exists
            Dim query As String = "SELECT COUNT(*) FROM users WHERE user_id = @UserID"
            Dim cmd As New MySqlCommand(query, sqlConn)
            cmd.Parameters.AddWithValue("@UserID", userID)

            sqlConn.Open()

            ' Execute the query and get the result
            Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())

            ' Debugging the count value returned from the query
            MessageBox.Show("Count returned from database: " & count.ToString()) ' This will show you the value returned by the query

            Return count > 0
        Catch ex As MySqlException
            MessageBox.Show("Database error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        Finally
            sqlConn.Close()
        End Try
    End Function

    ' Validate inputs before submission
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
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub btnMaintainWeight_Click(sender As Object, e As EventArgs) Handles btnMaintainWeight.Click
        fitness_goal = "maintain weight"
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub btnGainWeight_Click(sender As Object, e As EventArgs) Handles btnGainWeight.Click
        fitness_goal = "gain weight"
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub btnGainMuscle_Click(sender As Object, e As EventArgs) Handles btnGainMuscle.Click
        fitness_goal = "gain muscle"
        UpdateButtonAppearance(sender)
    End Sub

    ' Similarly, add button click events for reason and fitness_act
    Private Sub btnOption1_Click(sender As Object, e As EventArgs) Handles btnOption1.Click
        reason = "health"
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub btnOption2_Click(sender As Object, e As EventArgs) Handles btnOption2.Click
        reason = "appearance"
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub btnOption3_Click(sender As Object, e As EventArgs) Handles btnOption3.Click
        reason = "performance"
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub btnOption4_Click(sender As Object, e As EventArgs) Handles btnOption4.Click
        reason = "other"
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub btnNewbie_Click(sender As Object, e As EventArgs) Handles btnNewbie.Click
        fitness_act = "newbie"
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub btnIntermediate_Click(sender As Object, e As EventArgs) Handles btnIntermediate.Click
        fitness_act = "intermediate"
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub btnAdvance_Click(sender As Object, e As EventArgs) Handles btnAdvance.Click
        fitness_act = "advance"
        UpdateButtonAppearance(sender)
    End Sub

    Private Sub btnPro_Click(sender As Object, e As EventArgs) Handles btnPro.Click
        fitness_act = "pro"
        UpdateButtonAppearance(sender)
    End Sub

    ' Function to update button appearance when clicked
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

        btnLoseWeight.BorderRadius = 10
        btnMaintainWeight.BorderRadius = 10
        btnGainMuscle.BorderRadius = 10
        btnGainWeight.BorderRadius = 10
        btnOption1.BorderRadius = 10
        btnOption2.BorderRadius = 10
        btnOption3.BorderRadius = 10
        btnOption4.BorderRadius = 10
        btnNewbie.BorderRadius = 10
        btnIntermediate.BorderRadius = 10
        btnAdvance.BorderRadius = 10
        btnPro.BorderRadius = 10

        ' Reset Guna2Button colors if necessary
        ' For example:
        ' btnLoseWeight.FillColor = Color.Transparent
        ' btnLoseWeight.ForeColor = Color.Black
    End Sub

    ' BMI Calculation
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



