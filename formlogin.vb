Public Class formlogin
    Private isPasswordVisible As Boolean = False
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If TextBox1.Text = "kasir" And TextBox2.Text = "12345" Then
            formtiket.Show()
            Me.Hide()
            MsgBox("login anda berhasil!", vbInformation, "SUKSES!")
            TextBox1.Text = ""
            TextBox2.Text = ""
            CheckBox1.Text = ""
        ElseIf TextBox1.Text = "" And TextBox2.Text = "" Then
            MsgBox("silahkan masukkan username dan password anda!", vbCritical, "GAGAL!")
        ElseIf TextBox1.Text IsNot "adminn" And TextBox2.Text IsNot "12345" Then
            MsgBox("maaf, username atau password anda salah!", vbCritical, "GAGAL!")
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
    End Sub

    Private Sub formlogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Panel2_Paint(sender As Object, e As PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked Then
            TextBox2.PasswordChar = ""
        Else
            TextBox2.PasswordChar = "*"
        End If
    End Sub
End Class
