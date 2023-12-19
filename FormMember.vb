Imports System.Data.Odbc
Imports System.Drawing.Printing

Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class FormMember
    Dim Conn As OdbcConnection
    Dim cmd As OdbcCommand
    Dim Da As OdbcDataAdapter
    Dim rd As OdbcDataReader
    Dim Ds As DataSet
    Dim MyDB As String
    Dim EditData As String
    Dim WithEvents PD As New PrintDocument
    Dim PPD As New PrintPreviewDialog

    Sub koneksi()
        MyDB = "Driver={Mysql ODBC 3.51 Driver}; Database=tiket;server=Localhost;uid=root"
        Conn = New OdbcConnection(MyDB)
        If Conn.State = ConnectionState.Closed Then
            Conn.Open()
        End If


    End Sub
    Sub kondisiawal()

        Call koneksi()
        Da = New OdbcDataAdapter("select * from member", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "member")
        DataGridView1.DataSource = Ds.Tables("member")
        DataGridView1.Refresh()
    End Sub
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        formtiket.Show()
        Me.Hide()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnreset.Click
        TextBox1.Text = ""
        DateTimePicker1.Text = ""
        TextBox2.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If btnsave.Text = "Tambah" Then
            btnsave.Text = "Simpan"
            btnedit.Enabled = False
            btnhapus.Enabled = False
        Else
            If TextBox2.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Then
                MsgBox("Data Belum Lengkap, Silahkan Isi Semua Field")
                TextBox1.Focus()
            Else
                Call koneksi()
                Dim Inputdata As String = "Insert into member values('" & TextBox1.Text & "','" & DateTimePicker1.Text & "','" & TextBox2.Text & "','" & TextBox3.Text & "','" & TextBox4.Text & "')"
                cmd = New OdbcCommand(Inputdata, Conn)
                cmd.ExecuteNonQuery()
                MsgBox("Data Berhasil Diinput", MsgBoxStyle.Information, "Informasi")
                btnsave.Text = "Tambah"
                btnedit.Enabled = True
                btnhapus.Enabled = True

            End If
        End If

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim i As Integer
        i = Me.DataGridView1.CurrentRow.Index
        With DataGridView1.Rows.Item(i)
            Me.TextBox1.Text = .Cells(0).Value
            Me.DateTimePicker1.Text = .Cells(1).Value
            Me.TextBox2.Text = .Cells(2).Value
            Me.TextBox3.Text = .Cells(3).Value
            Me.TextBox4.Text = .Cells(4).Value
        End With
    End Sub

    Private Sub FormMember_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Call kondisiawal()
        Call koneksi()
    End Sub

    Private Sub btnhapus_Click(sender As Object, e As EventArgs) Handles btnhapus.Click
        If MessageBox.Show("Apakah Anda Yakin Ingin Menghapus Data?", "Info", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Call koneksi()
            Dim Hapusdata As String = "Delete from member where id ='" & TextBox1.Text & "'"
            cmd = New OdbcCommand(Hapusdata, Conn)
            cmd.ExecuteReader()
            Call kondisiawal()
            MsgBox("Data Berhasil Dihapus", MsgBoxStyle.Information, "Informasi")
        End If
    End Sub

    Private Sub btnedit_Click(sender As Object, e As EventArgs) Handles btnedit.Click
        If btnedit.Text = "Edit" Then
            btnedit.Text = "Simpan"
            btnsave.Enabled = False
            btnhapus.Enabled = False
        Else
            If TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
                MsgBox("Data Belum Lengkap, Silahkan Isi Semua Field")
            Else
                Call koneksi()
                Dim EditData As String = "UPDATE member SET nama='" & TextBox2.Text & "', ktp='" & TextBox3.Text & "', telp='" & TextBox4.Text & "' WHERE id='" & TextBox1.Text & "'"
                cmd = New OdbcCommand(EditData, Conn)
                cmd = New OdbcCommand(EditData, Conn)
                cmd.ExecuteNonQuery()
                MsgBox("Data Berhasil Diedit", MsgBoxStyle.Information, "Informasi")
                btnedit.Text = "Edit"
                btnsave.Enabled = True
                btnhapus.Enabled = True
                Call kondisiawal()
            End If
        End If
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        PPD.Document = PD
        PPD.ShowDialog()
        'PD.Print()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As PrintPageEventArgs)
        Dim pagesetup As New PageSettings
        pagesetup.PaperSize = New PaperSize("Custom", 250, 500)
        PD.DefaultPageSettings = pagesetup
    End Sub

    Private Sub report_PrintPage(sender As Object, e As PrintPageEventArgs) Handles report.PrintPage
        Dim f10 As New Font("Times New Roman", 10, FontStyle.Regular)
        Dim f9 As New Font("Times New Roman", 12, FontStyle.Bold)
        Dim f8 As New Font("Times New Roman", 14, FontStyle.Bold)

        Dim leftmargin As Integer = PD.DefaultPageSettings.Margins.Left
        Dim centermargin As Integer = PD.DefaultPageSettings.PaperSize.Width / 2
        Dim rightmargin As Integer = PD.DefaultPageSettings.PaperSize.Width

        Dim kanan As New StringFormat
        Dim tengah As New StringFormat

        kanan.Alignment = StringAlignment.Far
        tengah.Alignment = StringAlignment.Center

        Dim garis As String
        Dim garis1 As String
        garis = "____________________________________________________________________________"
        garis1 = "______________________________________________________"


        e.Graphics.DrawString("Monumen Nasional (Monas)", f8, Brushes.Black, centermargin, 5, tengah)
        e.Graphics.DrawString("RT.5/RW.2, Gambir, Kecamatan Gambir, Kota Jakarta Pusat, Daerah Khusus Ibukota Jakarta 10110", f10, Brushes.Black, centermargin, 30, tengah)
        e.Graphics.DrawString("(021) 3853040", f10, Brushes.Black, centermargin, 45, tengah)
        e.Graphics.DrawString(garis, f10, Brushes.Black, centermargin, 45, tengah)
        e.Graphics.DrawString("REKAPITULASI DATA MEMBER", f9, Brushes.Black, centermargin, 68, tengah)


        DataGridView1.AllowUserToAddRows = False
        Dim tinggi As Integer
        For baris As Integer = 0 To DataGridView1.RowCount - 1
            tinggi += 15
            Dim pen As New Pen(Color.Black, 2)
            e.Graphics.DrawString(garis1, f8, Brushes.Black, centermargin, 100, tengah)
            e.Graphics.DrawString("id", f10, Brushes.Black, 170, 104)
            e.Graphics.DrawString("tanggal", f10, Brushes.Black, 230, 104)
            e.Graphics.DrawString("nama", f10, Brushes.Black, 410, 104)
            e.Graphics.DrawString("ktp", f10, Brushes.Black, 500, 104)
            e.Graphics.DrawString("telp", f10, Brushes.Black, 600, 104)
            e.Graphics.DrawString(garis1, f8, Brushes.Black, centermargin, 100, tengah)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(0).Value.ToString, f10, Brushes.Black, 170, 109 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(1).Value.ToString, f10, Brushes.Black, 230, 109 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(2).Value.ToString, f10, Brushes.Black, 410, 109 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(3).Value.ToString, f10, Brushes.Black, 500, 108 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(4).Value.ToString, f10, Brushes.Black, 600, 109 + tinggi)
        Next
        e.Graphics.DrawString("Bekasi," & Now().ToString("dd MMMM yyyy"), f10, Brushes.Black, 400, 200)
        e.Graphics.DrawString("HRD Manager", f10, Brushes.Black, 400, 215)
        e.Graphics.DrawString("(priekha revia)", f10, Brushes.Black, 400, 280)
    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles Button1.Click
        PPD.Document = report
        PPD.ShowDialog()
        'PD.Print()
    End Sub
End Class