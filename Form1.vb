Imports System.Data.OleDb
Imports System.Data
Imports System.IO

Imports Microsoft.Office.Interop
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Drawing.Printing




Public Class Form1
    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        


    End Sub




    Private Sub ValidateInputs(ByVal Sender As Object, ByVal e As EventArgs)
        If TextBox1.Text.Trim.Length = 13 Then

            Button2.Enabled = True
        Else
            Button2.Enabled = False

        End If
    End Sub

    ' Dim Button2_Clicked As Boolean = False
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        DataGridView1.ColumnCount = 2
        DataGridView1.Columns(0).Name = "HmNumbers"
        DataGridView1.Columns(1).Name = "Time"

        Dim strData2 As Date = Today
        Dim strData As Date = Now






        '  Dim s As String
        ' s = TextBox1.Text
        ' Dim HMnumber As String = Mid(s, 6, 10)
        ' TextBox1.Clear()
        Dim x As Integer


        For x = 0 To ListBox1.Items.Count - 1
            '  If InStr(ListBox1.Items(x), HMnumber) > 0 Then
            Dim row As String() = New String() {TextBox1.Text(x), Now}
            DataGridView1.Rows.Add(row)

            'To remove it, use this
            ListBox1.Items.RemoveAt(x)
            Exit For
            '  End If
        Next


        Dim xlApp As Excel.Application
        Dim xlWorkBook As Excel.Workbook
        Dim xlWorkSheet As Excel.Worksheet
        Dim misValue As Object = System.Reflection.Missing.Value
        Dim i As Integer
        Dim j As Integer

        xlApp = New Excel.Application
        xlWorkBook = xlApp.Workbooks.Add(misValue)
        xlWorkSheet = xlWorkBook.Sheets("Arkusz1")

        xlApp.DisplayAlerts = False
        For i = 0 To DataGridView1.RowCount - 2
            For j = 0 To DataGridView1.ColumnCount - 1
                xlWorkSheet.Cells(i + 1, j + 1) = _
                DataGridView1(j, i).Value.ToString()
            Next
        Next
        For j = 0 To DataGridView1.ColumnCount - 1
            xlWorkSheet.Cells(1, j + 1) = DataGridView1.Columns(j).Name
        Next
        For i = 0 To DataGridView1.RowCount - 1
            For j = 0 To DataGridView1.ColumnCount - 1
                Dim cell As DataGridViewCell
                cell = DataGridView1(j, i)
                xlWorkSheet.Cells(i + 2, j + 1) = cell.Value
            Next
        Next
        xlWorkSheet.SaveAs("C:\Users\Matt\Downloads\backup-" + strData2 + ".xlsx")
        xlWorkBook.Close()
        xlApp.DisplayAlerts = True
        xlApp.Quit()

        releaseObject(xlApp)
        releaseObject(xlWorkBook)
        releaseObject(xlWorkSheet)
        My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Hand)




    End Sub



    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        PrintPreviewDialog1.Document = PrintDocument1
        PrintPreviewDialog1.PrintPreviewControl.Zoom = 1
        PrintPreviewDialog1.ShowDialog()
    End Sub



    Private oStringFormat As StringFormat
    Private oStringFormatComboBox As StringFormat
    Private oButton As Button
    Private oCheckbox As CheckBox
    Private oComboBox As ComboBox

    Private nTotalWidth As Int16
    Private nRowPos As Int16
    Private NewPage As Boolean
    Private nPageNo As Int16
    Private Header As String = "Today dispensed"
    Private sUserName As String = "Lloyds"

    Private Sub PrintDocument1_BeginPrint(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintEventArgs) Handles PrintDocument1.BeginPrint

        oStringFormat = New StringFormat
        oStringFormat.Alignment = StringAlignment.Near
        oStringFormat.LineAlignment = StringAlignment.Center
        oStringFormat.Trimming = StringTrimming.EllipsisCharacter

        oStringFormatComboBox = New StringFormat
        oStringFormatComboBox.LineAlignment = StringAlignment.Center
        oStringFormatComboBox.FormatFlags = StringFormatFlags.NoWrap
        oStringFormatComboBox.Trimming = StringTrimming.EllipsisCharacter

        oButton = New Button
        oCheckbox = New CheckBox
        oComboBox = New ComboBox

        nTotalWidth = 0
        For Each oColumn As DataGridViewColumn In DataGridView1.Columns

            nTotalWidth += oColumn.Width

        Next
        nPageNo = 1
        NewPage = True
        nRowPos = 0

    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        Static oColumnLefts As New ArrayList
        Static oColumnWidths As New ArrayList
        Static oColumnTypes As New ArrayList
        Static nHeight As Int16

        Dim nWidth, i, nRowsPerPage As Int16
        Dim nTop As Int16 = e.MarginBounds.Top
        Dim nLeft As Int16 = e.MarginBounds.Left

        If nPageNo = 1 Then

            For Each oColumn As DataGridViewColumn In DataGridView1.Columns

                nWidth = CType(Math.Floor(oColumn.Width / nTotalWidth * nTotalWidth * (e.MarginBounds.Width / nTotalWidth)), Int16)

                nHeight = e.Graphics.MeasureString(oColumn.HeaderText, oColumn.InheritedStyle.Font, nWidth).Height + 11

                oColumnLefts.Add(nLeft)
                oColumnWidths.Add(nWidth)
                oColumnTypes.add(oColumn.GetType)
                nLeft += nWidth

            Next

        End If

        Do While nRowPos < DataGridView1.Rows.Count - 1

            Dim oRow As DataGridViewRow = DataGridView1.Rows(nRowPos)

            If nTop + nHeight >= e.MarginBounds.Height + e.MarginBounds.Top Then

                DrawFooter(e, nRowsPerPage)

                NewPage = True
                nPageNo += 1
                e.HasMorePages = True
                Exit Sub

            Else

                If NewPage Then

                    ' Draw Header
                    e.Graphics.DrawString(Header, New Font(DataGridView1.Font, FontStyle.Bold), Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top - e.Graphics.MeasureString(Header, New Font(DataGridView1.Font, FontStyle.Bold), e.MarginBounds.Width).Height - 13)

                    ' Draw Columns
                    nTop = e.MarginBounds.Top
                    i = 0
                    For Each oColumn As DataGridViewColumn In DataGridView1.Columns

                        e.Graphics.FillRectangle(New SolidBrush(Drawing.Color.LightGray), New Rectangle(oColumnLefts(i), nTop, oColumnWidths(i), nHeight))
                        e.Graphics.DrawRectangle(Pens.Black, New Rectangle(oColumnLefts(i), nTop, oColumnWidths(i), nHeight))
                        e.Graphics.DrawString(oColumn.HeaderText, oColumn.InheritedStyle.Font, New SolidBrush(oColumn.InheritedStyle.ForeColor), New RectangleF(oColumnLefts(i), nTop, oColumnWidths(i), nHeight), oStringFormat)
                        i += 1

                    Next
                    NewPage = False

                End If

                nTop += nHeight
                i = 0
                For Each oCell As DataGridViewCell In oRow.Cells

                    If oColumnTypes(i) Is GetType(DataGridViewTextBoxColumn) OrElse oColumnTypes(i) Is GetType(DataGridViewLinkColumn) Then

                        e.Graphics.DrawString(oCell.Value.ToString, oCell.InheritedStyle.Font, New SolidBrush(oCell.InheritedStyle.ForeColor), New RectangleF(oColumnLefts(i), nTop, oColumnWidths(i), nHeight), oStringFormat)

                    ElseIf oColumnTypes(i) Is GetType(DataGridViewButtonColumn) Then

                        oButton.Text = oCell.Value.ToString
                        oButton.Size = New Size(oColumnWidths(i), nHeight)
                        Dim oBitmap As New Bitmap(oButton.Width, oButton.Height)
                        oButton.DrawToBitmap(oBitmap, New Rectangle(0, 0, oBitmap.Width, oBitmap.Height))
                        e.Graphics.DrawImage(oBitmap, New Point(oColumnLefts(i), nTop))

                    ElseIf oColumnTypes(i) Is GetType(DataGridViewCheckBoxColumn) Then

                        oCheckbox.Size = New Size(14, 14)
                        oCheckbox.Checked = CType(oCell.Value, Boolean)
                        Dim oBitmap As New Bitmap(oColumnWidths(i), nHeight)
                        Dim oTempGraphics As Graphics = Graphics.FromImage(oBitmap)
                        oTempGraphics.FillRectangle(Brushes.White, New Rectangle(0, 0, oBitmap.Width, oBitmap.Height))
                        oCheckbox.DrawToBitmap(oBitmap, New Rectangle(CType((oBitmap.Width - oCheckbox.Width) / 2, Int32), CType((oBitmap.Height - oCheckbox.Height) / 2, Int32), oCheckbox.Width, oCheckbox.Height))
                        e.Graphics.DrawImage(oBitmap, New Point(oColumnLefts(i), nTop))

                    ElseIf oColumnTypes(i) Is GetType(DataGridViewComboBoxColumn) Then

                        oComboBox.Size = New Size(oColumnWidths(i), nHeight)
                        Dim oBitmap As New Bitmap(oComboBox.Width, oComboBox.Height)
                        oComboBox.DrawToBitmap(oBitmap, New Rectangle(0, 0, oBitmap.Width, oBitmap.Height))
                        e.Graphics.DrawImage(oBitmap, New Point(oColumnLefts(i), nTop))
                        e.Graphics.DrawString(oCell.Value.ToString, oCell.InheritedStyle.Font, New SolidBrush(oCell.InheritedStyle.ForeColor), New RectangleF(oColumnLefts(i) + 1, nTop, oColumnWidths(i) - 16, nHeight), oStringFormatComboBox)

                    ElseIf oColumnTypes(i) Is GetType(DataGridViewImageColumn) Then

                        Dim oCellSize As Rectangle = New Rectangle(oColumnLefts(i), nTop, oColumnWidths(i), nHeight)
                        Dim oImageSize As Size = CType(oCell.Value, Image).Size
                        e.Graphics.DrawImage(oCell.Value, New Rectangle(oColumnLefts(i) + CType(((oCellSize.Width - oImageSize.Width) / 2), Int32), nTop + CType(((oCellSize.Height - oImageSize.Height) / 2), Int32), CType(oCell.Value, Image).Width, CType(oCell.Value, Image).Height))

                    End If

                    e.Graphics.DrawRectangle(Pens.Black, New Rectangle(oColumnLefts(i), nTop, oColumnWidths(i), nHeight))

                    i += 1

                Next

            End If

            nRowPos += 1
            nRowsPerPage += 1

        Loop

        DrawFooter(e, nRowsPerPage)

        e.HasMorePages = False

    End Sub

    Private Sub DrawFooter(ByVal e As System.Drawing.Printing.PrintPageEventArgs, ByVal RowsPerPage As Int32)

        Dim sPageNo As String = nPageNo.ToString + " of " + Math.Ceiling(DataGridView1.Rows.Count / RowsPerPage).ToString

        ' Right Align - User Name
        e.Graphics.DrawString(sUserName, DataGridView1.Font, Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width - e.Graphics.MeasureString(sPageNo, DataGridView1.Font, e.MarginBounds.Width).Width), e.MarginBounds.Top + e.MarginBounds.Height + 7)

        ' Left Align - Date/Time
        e.Graphics.DrawString(Now.ToLongDateString + " " + Now.ToShortTimeString, DataGridView1.Font, Brushes.Black, e.MarginBounds.Left, e.MarginBounds.Top + e.MarginBounds.Height + 7)

        ' Center  - Page No. Info
        e.Graphics.DrawString(sPageNo, DataGridView1.Font, Brushes.Black, e.MarginBounds.Left + (e.MarginBounds.Width - e.Graphics.MeasureString(sPageNo, DataGridView1.Font, e.MarginBounds.Width).Width) / 2, e.MarginBounds.Top + e.MarginBounds.Height + 31)

    End Sub


    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Using ofd As New OpenFileDialog
            If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                AddHandler TextBox1.TextChanged, AddressOf ValidateInputs
                Dim xlApp As New Excel.Application
                'change the path of Excel file
                Dim xlBook = xlApp.Workbooks.Open(ofd.FileName)
                Dim xlSheet = DirectCast(xlBook.Worksheets(1), Excel.Worksheet)
                'make sure there is data in A1 to A5 cell in Sheet1
                Dim a As Excel.Range = xlSheet.Range("A1:A34")

                For Each rngg As Excel.Range In a
                    'add the data from Excel to combobox column
                    ListBox1.Items.Add(rngg.Value.ToString)
                Next

                xlBook.Close(SaveChanges:=True)
                xlApp.Quit()
                xlApp = Nothing
                GC.Collect()
                GC.WaitForFullGCComplete()
            End If
        End Using




    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Form2.ShowDialog()
    End Sub
End Class



