Imports System.Data.Odbc
Imports System.Drawing.Printing

Imports System.Windows.Forms.VisualStyles.VisualStyleElement

Public Class formtiket
    Dim Conn As OdbcConnection
    Dim cmd As OdbcCommand
    Dim Da As OdbcDataAdapter
    Dim rd As OdbcDataReader
    Dim Ds As DataSet
    Dim MyDB As String
    Dim formmasuk As DataTable
    Dim EditData As String
    Dim Hapusdata As String
    Dim WithEvents PD As New PrintDocument
    Dim PPD As New PrintPreviewDialog

    Private dataToPrint As String = ""

    Sub koneksi()
        MyDB = "Driver={Mysql ODBC 3.51 Driver}; Database=tiket;server=Localhost;uid=root"
        Conn = New OdbcConnection(MyDB)
        If Conn.State = ConnectionState.Closed Then
            Conn.Open()
        End If

    End Sub
    Sub kondisiawal()

        Call koneksi()
        Da = New OdbcDataAdapter("select * from formmasuk", Conn)
        Ds = New DataSet
        Da.Fill(Ds, "formmasuk")
        DataGridView1.DataSource = Ds.Tables("formmasuk")
        DataGridView1.Refresh()
    End Sub
    Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        formlogin.Show()
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnhitung.Click
        TextBox3.Text = Val(TextBox1.Text) * Val(TextBox2.Text)

        If ComboBox1.SelectedItem IsNot Nothing Then
            Dim selectedItem As String = ComboBox1.SelectedItem.ToString()

            If selectedItem = "Member" Then
                Dim value1 As Double, value2 As Double, value3 As Double

                If Double.TryParse(TextBox1.Text, value1) AndAlso
                   Double.TryParse(TextBox2.Text, value2) AndAlso
                   Double.TryParse(TextBox3.Text, value3) Then
                    TextBox4.Text = ((value1 * value2) * 10) / 100
                    TextBox5.Text = (value3 - CDbl(TextBox4.Text)).ToString()
                Else
                    ' Handle invalid input (non-numeric values in TextBox1, TextBox2, TextBox3)
                End If
            ElseIf selectedItem = "No Member" Then
                Dim value1 As Double, value2 As Double, value3 As Double

                If Double.TryParse(TextBox1.Text, value1) AndAlso
                   Double.TryParse(TextBox2.Text, value2) AndAlso
                   Double.TryParse(TextBox3.Text, value3) Then
                    TextBox4.Text = "0" ' You had 0/100, so I assume you want TextBox4.Text to be "0" for "No Member"
                    TextBox5.Text = (value3 - CDbl(TextBox4.Text)).ToString()
                Else
                    ' Handle invalid input (non-numeric values in TextBox1, TextBox2, TextBox3)
                End If
            Else
                ' Handle the case where ComboBox1 has an unexpected value
            End If
        Else
            ' Handle the case where nothing is selected in ComboBox1
        End If

    End Sub

    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim i As Integer
        i = Me.DataGridView1.CurrentRow.Index
        With DataGridView1.Rows.Item(i)
            Me.TextBox6.Text = .Cells(0).Value
            Me.TextBox7.Text = .Cells(1).Value
            Me.DateTimePicker1.Text = .Cells(2).Value
            Me.TextBox1.Text = .Cells(3).Value
            Me.TextBox2.Text = .Cells(4).Value
            Me.ComboBox1.Text = .Cells(5).Value
            Me.TextBox3.Text = .Cells(6).Value
            Me.TextBox4.Text = .Cells(7).Value
            Me.TextBox5.Text = .Cells(8).Value
        End With
    End Sub

    Private Sub formtiket_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TextBox1.Text = "50000"
        Call kondisiawal()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles btnsave.Click
        If btnsave.Text = "Tambah" Then
            btnsave.Text = "Simpan"
            btnedit.Enabled = False
            btnhapus.Enabled = False
        Else
            If TextBox7.Text = "" Or DateTimePicker1.Text = "" Or TextBox1.Text = "" Or TextBox2.Text = "" Or ComboBox1.Text = "" Or TextBox3.Text = "" Or TextBox4.Text = "" Or TextBox5.Text = "" Then
                MsgBox("Data Belum Lengkap, Silahkan Isi Semua Field")
                TextBox1.Focus()
            Else
                Call koneksi()
                Dim Inputdata As String = "Insert into formmasuk values(null, '" & TextBox7.Text & "','" & DateTimePicker1.Text & "','" & TextBox1.Text & "','" & TextBox2.Text & "','" & ComboBox1.Text & "','" & TextBox3.Text & "' ,'" & TextBox4.Text & "' ,'" & TextBox5.Text & "')"
                cmd = New OdbcCommand(Inputdata, Conn)
                cmd.ExecuteNonQuery()
                MsgBox("Data Berhasil Diinput", MsgBoxStyle.Information, "Informasi")
                btnsave.Text = "Tambah"
                btnedit.Enabled = True
                btnhapus.Enabled = True
                Call kondisiawal()

            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles btnreset.Click
        DateTimePicker1.Text = ""
        TextBox1.Text = "50000"
        TextBox2.Text = ""
        ComboBox1.Text = ""
        TextBox3.Text = ""
        TextBox4.Text = ""
        TextBox5.Text = ""
        TextBox7.Text = ""

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
                Dim EditData As String = "UPDATE formmasuk SET id='" & TextBox6.Text & "',nama='" & TextBox7.Text & "', tanggal='" & DateTimePicker1.Text & "', satuan='" & TextBox1.Text & "', qty='" & TextBox2.Text & "', status='" & ComboBox1.Text & "', total='" & TextBox3.Text & "', diskon='" & TextBox4.Text & "', subtotal='" & TextBox5.Text & "' WHERE id='" & TextBox6.Text & "'"
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

    Private Sub btnhapus_Click(sender As Object, e As EventArgs) Handles btnhapus.Click
        If MessageBox.Show("Apakah Anda Yakin Ingin Menghapus Data?", "Info", MessageBoxButtons.YesNo) = Windows.Forms.DialogResult.Yes Then
            Call koneksi()
            Dim Hapusdata As String = "Delete from formmasuk where id ='" & TextBox6.Text & "'"
            cmd = New OdbcCommand(Hapusdata, Conn)
            cmd.ExecuteReader()
            Call kondisiawal()
            MsgBox("Data Berhasil Dihapus", MsgBoxStyle.Information, "Informasi")
        End If

    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        FormMember.Show()
        Me.Hide()
    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs)
        Dim pagesetup As New PageSettings
        pagesetup.PaperSize = New PaperSize("Custom", 250, 500)
        PD.DefaultPageSettings = pagesetup
    End Sub


    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        PPD.Document = PD
        PPD.ShowDialog()
        'PD.Print()
    End Sub

    Private Sub PD_BeginPrint(sender As Object, e As PrintEventArgs) Handles PD.BeginPrint
        Dim pagesetup As New PageSettings
        pagesetup.PaperSize = New PaperSize("custom", 250, 500)

    End Sub

    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles Button1.Click
        PPD.Document = PrintDocument1
        PPD.ShowDialog()
        'PD.Print()
    End Sub

    Private Sub PrintDocument1_PrintPage_1(sender As Object, e As PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Dim f10 As New Font("Times New Roman", 9, FontStyle.Regular)
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
        garis = "_________________________________________________________________________________________________"
        garis1 = "______________________________________________________________________"

        Dim gambar As Drawing.Image = My.Resources.ResourceManager.GetObject("logo")


        ' Pastikan gambar tidak null sebelum menggambar
        If gambar IsNot Nothing Then
            ' Tentukan posisi dan ukuran gambar
            Dim x As Integer = CInt((e.PageBounds.Width - 600) / 2)
            Dim y As Integer = 5
            Dim width As Integer = 150
            Dim height As Integer = 35

            ' Gambar gambar ke grafis
            e.Graphics.DrawImage(gambar, x, y, width, height)
        End If

        e.Graphics.DrawString("MONUMEN NASIONAL (MONAS)", f8, Brushes.Black, centermargin, 15, tengah)
        e.Graphics.DrawString("RT.5/RW.2, Gambir, Kecamatan Gambir, Kota Jakarta Pusat, Daerah Khusus Ibukota Jakarta 10110", f10, Brushes.Black, centermargin, 35, tengah)
        e.Graphics.DrawString("(021) 3853040", f10, Brushes.Black, centermargin, 50, tengah)
        e.Graphics.DrawString(garis, f10, Brushes.Black, centermargin, 51, tengah)
        e.Graphics.DrawString("LAPORAN REKAPITULASI DATA PEMBELIAN TIKET", f9, Brushes.Black, centermargin, 70, tengah)


        DataGridView1.AllowUserToAddRows = False
        Dim tinggi As Integer
        Dim xPosId As Integer = 67
        Dim xPosNama As Integer = 90
        Dim xPosTanggal As Integer = 150
        Dim xPosSatuan As Integer = 320
        Dim xPosQty As Integer = 390
        Dim xPosStatus As Integer = 430
        Dim xPosTotal As Integer = 510
        Dim xPosDiskon As Integer = 600
        Dim xPosSubtotal As Integer = 700

        For baris As Integer = 0 To DataGridView1.RowCount - 1
            tinggi += 20
            Dim pen As New Pen(Color.Black, 2)
            e.Graphics.DrawString(garis1, f8, Brushes.Black, centermargin, 80, tengah)

            ' Draw column headers and vertical lines
            e.Graphics.DrawLine(pen, xPosId, 101, xPosId, 230)
            e.Graphics.DrawString("ID", f10, Brushes.Black, xPosId + 3, 104)

            ' Menambahkan garis vertikal sebelum Nama
            e.Graphics.DrawLine(pen, xPosNama - 3, 101, xPosNama - 3, 230)
            e.Graphics.DrawString("PEMBELI", f10, Brushes.Black, xPosNama + 3, 104)

            ' Menambahkan garis vertikal sebelum tanggal
            e.Graphics.DrawLine(pen, xPosTanggal - 3, 101, xPosTanggal - 3, 230)
            e.Graphics.DrawString("TANGGAL", f10, Brushes.Black, xPosTanggal + 3, 104)

            ' Menambahkan garis vertikal sebelum satuan
            e.Graphics.DrawLine(pen, xPosSatuan - 3, 101, xPosSatuan - 3, 230)
            e.Graphics.DrawString("SATUAN", f10, Brushes.Black, xPosSatuan + 3, 104)

            ' Menambahkan garis vertikal sebelum qty
            e.Graphics.DrawLine(pen, xPosQty - 3, 101, xPosQty - 3, 230)
            e.Graphics.DrawString("QTY", f10, Brushes.Black, xPosQty + 3, 104)

            ' Menambahkan garis vertikal sebelum status
            e.Graphics.DrawLine(pen, xPosStatus - 3, 101, xPosStatus - 3, 230)
            e.Graphics.DrawString("STATUS", f10, Brushes.Black, xPosStatus + 3, 104)

            ' Menambahkan garis vertikal sebelum total
            e.Graphics.DrawLine(pen, xPosTotal - 3, 101, xPosTotal - 3, 230)
            e.Graphics.DrawString("TOTAL", f10, Brushes.Black, xPosTotal + 3, 104)

            ' Menambahkan garis vertikal sebelum diskon
            e.Graphics.DrawLine(pen, xPosDiskon - 3, 101, xPosDiskon - 3, 230)
            e.Graphics.DrawString("DISKON", f10, Brushes.Black, xPosDiskon + 3, 104)

            ' Menambahkan garis vertikal sebelum subtotal
            e.Graphics.DrawLine(pen, xPosSubtotal - 3, 101, xPosSubtotal - 3, 230)
            e.Graphics.DrawString("SUBTOTAL", f10, Brushes.Black, xPosSubtotal + 3, 104)

            ' Draw vertical line after subtotal column
            e.Graphics.DrawLine(pen, xPosSubtotal + 80, 101, xPosSubtotal + 80, 230)

            ' Draw horizontal line below column headers
            e.Graphics.DrawLine(pen, xPosId, 120, xPosSubtotal + 80, 120)

            ' Draw horizontal line below subtotal
            e.Graphics.DrawLine(pen, xPosId, 129 + tinggi, xPosSubtotal + 80, 129 + tinggi)

            e.Graphics.DrawString(garis1, f8, Brushes.Black, centermargin, 100, tengah)

            ' Draw data for each column
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(0).Value.ToString, f10, Brushes.Black, xPosId, 109 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(1).Value.ToString, f10, Brushes.Black, xPosNama, 109 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(2).Value.ToString, f10, Brushes.Black, xPosTanggal, 109 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(3).Value.ToString, f10, Brushes.Black, xPosSatuan, 109 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(4).Value.ToString, f10, Brushes.Black, xPosQty, 108 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(5).Value.ToString, f10, Brushes.Black, xPosStatus, 109 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(6).Value.ToString, f10, Brushes.Black, xPosTotal, 109 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(7).Value.ToString, f10, Brushes.Black, xPosDiskon, 109 + tinggi)
            e.Graphics.DrawString(DataGridView1.Rows(baris).Cells(8).Value.ToString, f10, Brushes.Black, xPosSubtotal, 109 + tinggi)
        Next

        ' Draw footer information
        e.Graphics.DrawString("Jakarta," & Now().ToString("dd MMMM yyyy"), f10, Brushes.Black, 670, 415)
        e.Graphics.DrawString("Marketing Manager", f10, Brushes.Black, 670, 430)
        e.Graphics.DrawString("(Priekha Revita)", f10, Brushes.Black, 670, 500)

        e.Graphics.DrawString(garis, f10, Brushes.Black, centermargin, 1010, tengah)
        e.Graphics.DrawString("Marketing Division", f10, Brushes.Black, 110, 1025)
        e.Graphics.DrawString("Page 1 of 1", f10, Brushes.Black, 680, 1025)

    End Sub

    Private Sub DateTimePicker1_ValueChanged(sender As Object, e As EventArgs) Handles DateTimePicker1.ValueChanged

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridView1.SelectedRows.Count > 0 Then
            ' Ambil data yang akan dicetak
            Dim selectedRow As DataGridViewRow = DataGridView1.SelectedRows(0)
            dataToPrint = $"id: {selectedRow.Cells(0).Value}, " &
                      $"nama: {selectedRow.Cells(1).Value}, " &
                      $"tanggal: {selectedRow.Cells(2).Value}, " &
                      $"satuan: {selectedRow.Cells(3).Value}, " &
                      $"qty: {selectedRow.Cells(4).Value}, " &
                      $"status: {selectedRow.Cells(5).Value}, " &
                      $"total: {selectedRow.Cells(6).Value}, " &
                      $"diskom: {selectedRow.Cells(7).Value}, " &
                      $"subtotal: {selectedRow.Cells(8).Value}"

            ' Membuat objek PrintDocument
            Dim printDoc As New PrintDocument()

            ' Menetapkan event handler untuk PrintPage
            AddHandler printDoc.PrintPage, AddressOf PrintPageHandler

            ' Menyesuaikan ukuran kertas
            printDoc.DefaultPageSettings.PaperSize = New PaperSize("CustomSize", 148, 210) ' Ganti dengan ukuran yang diinginkan

            ' Menampilkan dialog cetak sebelum mencetak
            Dim printDialog As New PrintDialog()
            printDialog.Document = printDoc
            If printDialog.ShowDialog() = DialogResult.OK Then
                ' Memulai proses pencetakan
                printDoc.Print()
            End If
        Else
            MessageBox.Show("Pilih baris data terlebih dahulu.")
        End If
    End Sub


    Private Sub PrintPageHandler(sender As Object, e As PrintPageEventArgs)
        ' Mendefinisikan font untuk teks yang akan dicetak
        Dim titleFont As New Font("Arial", 16, FontStyle.Bold)
        Dim headerFont As New Font("Arial", 12, FontStyle.Bold)
        Dim dataFont As New Font("Arial", 12)

        ' Mendapatkan lebar margin dan lebar area pencetakan
        Dim leftMargin As Single = e.MarginBounds.Left
        Dim topMargin As Single = e.MarginBounds.Top
        Dim printWidth As Single = e.MarginBounds.Width

        ' Mendeklarasikan variabel yPos untuk menentukan posisi vertikal saat mencetak
        Dim yPos As Single = topMargin

        ' Menggambar judul laporan
        e.Graphics.DrawString("Laporan Data Tiket", titleFont, Brushes.Black, leftMargin, yPos)
        yPos += titleFont.GetHeight() + 10 ' Menambahkan jarak antara judul dan header kolom

        ' Header kolom
        yPos += 20 ' Menambahkan jarak vertikal sebelum header kolom
        e.Graphics.DrawString("ID", headerFont, Brushes.Black, leftMargin, yPos)
        yPos += headerFont.GetHeight() ' Menambahkan jarak antara setiap header kolom
        e.Graphics.DrawString("Nama", headerFont, Brushes.Black, leftMargin, yPos)
        yPos += headerFont.GetHeight()
        e.Graphics.DrawString("Tanggal", headerFont, Brushes.Black, leftMargin, yPos)
        yPos += headerFont.GetHeight()
        e.Graphics.DrawString("Satuan", headerFont, Brushes.Black, leftMargin, yPos)
        yPos += headerFont.GetHeight()
        e.Graphics.DrawString("Qty", headerFont, Brushes.Black, leftMargin, yPos)
        yPos += headerFont.GetHeight()
        e.Graphics.DrawString("Status", headerFont, Brushes.Black, leftMargin, yPos)
        yPos += headerFont.GetHeight()
        e.Graphics.DrawString("Total", headerFont, Brushes.Black, leftMargin, yPos)
        yPos += headerFont.GetHeight()
        e.Graphics.DrawString("Diskon", headerFont, Brushes.Black, leftMargin, yPos)
        yPos += headerFont.GetHeight()
        e.Graphics.DrawString("Subtotal", headerFont, Brushes.Black, leftMargin, yPos)

        yPos += 20 ' Menambahkan jarak antara header kolom dan data

        ' Menggambar data yang akan dicetak
        e.Graphics.DrawString(dataToPrint, dataFont, Brushes.Black, leftMargin, yPos)

        ' Menggunakan metode MeasureString untuk mengukur tinggi teks yang akan dicetak
        Dim textHeight As Single = e.Graphics.MeasureString(dataToPrint, dataFont).Height

        ' Menetapkan posisi vertikal untuk halaman berikutnya
        yPos += textHeight

        ' Jika masih ada data yang akan dicetak, lanjutkan ke halaman berikutnya
        If yPos < e.MarginBounds.Bottom Then
            e.HasMorePages = False ' Tidak ada halaman berikutnya
        Else
            e.HasMorePages = True ' Masih ada data, lanjutkan ke halaman berikutnya
        End If
    End Sub
End Class