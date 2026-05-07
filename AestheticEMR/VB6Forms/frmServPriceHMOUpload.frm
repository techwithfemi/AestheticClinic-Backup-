VERSION 5.00
Object = "{CDE57A40-8B86-11D0-B3C6-00A0C90AEA82}#1.0#0"; "msdatgrd.ocx"
Object = "{5E9E78A0-531B-11CF-91F6-C2863C385E30}#1.0#0"; "msflxgrd.ocx"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Begin VB.Form frmServPriceHMOUpload 
   BackColor       =   &H00FFC0C0&
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Services"
   ClientHeight    =   8985
   ClientLeft      =   2670
   ClientTop       =   1095
   ClientWidth     =   11415
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MDIChild        =   -1  'True
   MinButton       =   0   'False
   ScaleHeight     =   8985
   ScaleWidth      =   11415
   ShowInTaskbar   =   0   'False
   Begin VB.CommandButton cmdLab 
      BackColor       =   &H00C0C000&
      Caption         =   "Upload Investigations"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   8730
      Style           =   1  'Graphical
      TabIndex        =   27
      Top             =   8550
      Width           =   2565
   End
   Begin VB.CommandButton cmdDrug 
      BackColor       =   &H00C0C000&
      Caption         =   "Upload Drug"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   6075
      Style           =   1  'Graphical
      TabIndex        =   26
      Top             =   8550
      Width           =   2520
   End
   Begin MSDataGridLib.DataGrid grdData 
      Height          =   5100
      Left            =   45
      TabIndex        =   25
      Top             =   3375
      Width           =   11310
      _ExtentX        =   19950
      _ExtentY        =   8996
      _Version        =   393216
      AllowUpdate     =   0   'False
      HeadLines       =   1
      RowHeight       =   23
      TabAction       =   1
      BeginProperty HeadFont {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ColumnCount     =   2
      BeginProperty Column00 
         DataField       =   ""
         Caption         =   ""
         BeginProperty DataFormat {6D835690-900B-11D0-9484-00A0C91110ED} 
            Type            =   0
            Format          =   ""
            HaveTrueFalseNull=   0
            FirstDayOfWeek  =   0
            FirstWeekOfYear =   0
            LCID            =   1033
            SubFormatType   =   0
         EndProperty
      EndProperty
      BeginProperty Column01 
         DataField       =   ""
         Caption         =   ""
         BeginProperty DataFormat {6D835690-900B-11D0-9484-00A0C91110ED} 
            Type            =   0
            Format          =   ""
            HaveTrueFalseNull=   0
            FirstDayOfWeek  =   0
            FirstWeekOfYear =   0
            LCID            =   1033
            SubFormatType   =   0
         EndProperty
      EndProperty
      SplitCount      =   1
      BeginProperty Split0 
         BeginProperty Column00 
         EndProperty
         BeginProperty Column01 
         EndProperty
      EndProperty
   End
   Begin VB.Frame Frame2 
      BackColor       =   &H00FFC0C0&
      Caption         =   "Create Tariff Template Section"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   1950
      Left            =   270
      TabIndex        =   14
      Top             =   360
      Width           =   10950
      Begin VB.CommandButton cmdUpload 
         Caption         =   "Upload Data"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   4140
         TabIndex        =   24
         Top             =   1485
         Width           =   1530
      End
      Begin VB.ComboBox cboSheet 
         Height          =   315
         ItemData        =   "frmServPriceHMOUpload.frx":0000
         Left            =   1710
         List            =   "frmServPriceHMOUpload.frx":0010
         TabIndex        =   22
         Top             =   1485
         Width           =   2310
      End
      Begin VB.CommandButton cmdPix 
         Caption         =   "Select File..."
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   8190
         TabIndex        =   20
         Top             =   1035
         Width           =   1530
      End
      Begin VB.TextBox txtPix 
         Enabled         =   0   'False
         Height          =   330
         Left            =   1710
         Locked          =   -1  'True
         TabIndex        =   19
         Top             =   1080
         Width           =   6405
      End
      Begin VB.CheckBox chkDel 
         BackColor       =   &H00FFC0C0&
         Caption         =   "Delete Existing Tariff for this Company"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   240
         Left            =   3150
         TabIndex        =   17
         Top             =   720
         Width           =   4020
      End
      Begin VB.ComboBox cboHMO 
         Height          =   315
         ItemData        =   "frmServPriceHMOUpload.frx":002E
         Left            =   1710
         List            =   "frmServPriceHMOUpload.frx":0030
         Sorted          =   -1  'True
         Style           =   2  'Dropdown List
         TabIndex        =   0
         Top             =   225
         Width           =   6405
      End
      Begin VB.ComboBox cboDrug 
         Height          =   315
         ItemData        =   "frmServPriceHMOUpload.frx":0032
         Left            =   1710
         List            =   "frmServPriceHMOUpload.frx":0034
         Style           =   2  'Dropdown List
         TabIndex        =   1
         Top             =   675
         Width           =   1095
      End
      Begin VB.Label Label8 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "WorkShhet"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   0
         Left            =   225
         TabIndex        =   23
         Top             =   1530
         Width           =   1410
      End
      Begin VB.Label lblPCent 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Select Excel File"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Left            =   180
         TabIndex        =   18
         Top             =   1170
         Width           =   1455
      End
      Begin VB.Label Label11 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Template for"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   240
         Left            =   315
         TabIndex        =   16
         Top             =   270
         Width           =   1365
      End
      Begin VB.Label Label8 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Has Own Tariff?"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   195
         Index           =   2
         Left            =   225
         TabIndex        =   15
         Top             =   720
         Width           =   1455
      End
   End
   Begin VB.TextBox txtSearch 
      Appearance      =   0  'Flat
      Height          =   375
      Left            =   8775
      TabIndex        =   10
      Top             =   2565
      Width           =   1920
   End
   Begin VB.CommandButton cmdAdd 
      BackColor       =   &H00C0C000&
      Caption         =   "Add"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   1665
      Style           =   1  'Graphical
      TabIndex        =   7
      Top             =   2475
      Width           =   1215
   End
   Begin VB.CommandButton CancelButton 
      BackColor       =   &H00C0C000&
      Caption         =   "Close"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   7020
      Style           =   1  'Graphical
      TabIndex        =   6
      Top             =   2475
      Width           =   1215
   End
   Begin VB.CommandButton cmdEdit 
      BackColor       =   &H00C0C000&
      Caption         =   "Edit"
      Enabled         =   0   'False
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3015
      Style           =   1  'Graphical
      TabIndex        =   5
      Top             =   2475
      Width           =   1215
   End
   Begin VB.CommandButton cmdDel 
      BackColor       =   &H00C0C000&
      Caption         =   "Delete"
      Enabled         =   0   'False
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   5670
      Style           =   1  'Graphical
      TabIndex        =   4
      Top             =   2475
      Width           =   1215
   End
   Begin VB.CommandButton cmdRefresh 
      BackColor       =   &H00FFFF00&
      Caption         =   "Refresh"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   4365
      Style           =   1  'Graphical
      TabIndex        =   3
      Top             =   2475
      Width           =   1215
   End
   Begin VB.CommandButton cmdCancel 
      BackColor       =   &H00C0C000&
      Caption         =   "Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   3015
      Style           =   1  'Graphical
      TabIndex        =   8
      Top             =   2475
      Width           =   1215
   End
   Begin VB.CommandButton OKButton 
      BackColor       =   &H00C0C000&
      Caption         =   "Save"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   1665
      Style           =   1  'Graphical
      TabIndex        =   9
      Top             =   2475
      Width           =   1215
   End
   Begin VB.CommandButton Command1 
      BackColor       =   &H00FFFF00&
      Caption         =   "View Bulk Items"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   375
      Left            =   10800
      Style           =   1  'Graphical
      TabIndex        =   12
      Top             =   2115
      Visible         =   0   'False
      Width           =   2115
   End
   Begin MSComDlg.CommonDialog CommonDialog1 
      Left            =   0
      Top             =   720
      _ExtentX        =   847
      _ExtentY        =   847
      _Version        =   393216
   End
   Begin MSFlexGridLib.MSFlexGrid OrderGridDrug 
      Height          =   5100
      Left            =   45
      TabIndex        =   21
      Top             =   3375
      Visible         =   0   'False
      Width           =   11310
      _ExtentX        =   19950
      _ExtentY        =   8996
      _Version        =   393216
      FixedCols       =   0
      BackColorFixed  =   14737632
      BackColorBkg    =   -2147483643
      ScrollTrack     =   -1  'True
      AllowUserResizing=   1
   End
   Begin VB.Label Label1 
      Alignment       =   2  'Center
      BackColor       =   &H00404000&
      Caption         =   "***"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   13.5
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FFFFFF&
      Height          =   330
      Left            =   0
      TabIndex        =   13
      Top             =   2970
      Width           =   11490
   End
   Begin VB.Label Label27 
      Alignment       =   2  'Center
      BackStyle       =   0  'Transparent
      Caption         =   "Quick Search"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   8775
      TabIndex        =   11
      Top             =   2340
      Width           =   1815
   End
   Begin VB.Label Label5 
      Alignment       =   2  'Center
      BackColor       =   &H00404000&
      Caption         =   "Upload Services' Tariff from Excel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   13.5
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FFFFFF&
      Height          =   375
      Left            =   0
      TabIndex        =   2
      Top             =   0
      Width           =   11445
   End
End
Attribute VB_Name = "frmServPriceHMOUpload"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'''
Option Explicit
Public strParam As String
Public flgEdit As Boolean
Public strParam2 As String
Public strHMOID As String
Public strHMO As String
Dim strRem As String
Dim strUseID As String
Dim strUseCoy As String
Dim OldPrice As Double
Dim strFileOpen As String
Dim strTxt As String
Dim fileExt As String
Private rsSend As ADODB.Recordset
Private folderPath As String
Dim uploadFromExcel  As Boolean

Private Sub CancelButton_Click()
Unload Me
End Sub

Private Sub cboCat_Click()
'On Error GoTo errH
' Dim rsDrg As New Recordset
'  cboDrug.Clear
'  With rsDrg
'    Select Case cboCat.Text
'    Case "(ALL)"
'        .Open "select distinct drug from vwDrugsForGrid order by drug", conSTR, adOpenForwardOnly, adLockReadOnly
'    Case Else
'        .Open "select distinct drug from vwDrugsForGrid where category='" & Replace(cboCat.Text, "'", "''") & "'  order by drug ", conSTR, adOpenForwardOnly, adLockReadOnly
'    End Select
'
'If Not .EOF Then
'.MoveFirst
'Do While Not .EOF
'cboDrug.AddItem !Drug
'.MoveNext
'Loop
'End If
'End With
'
'Set rsDrg = Nothing
'
'Exit Sub
'errH:
'MsgBox Err.Description
''dblPrice = 0
''MsgBox "This drug/service has no price attached !!! Please see the Auditor"
'
End Sub


Private Sub cboHMO_Click()
On Error GoTo errH
 Label1.Caption = "Tariff"
 If cboHMO.ListIndex = 0 Or cboHMO.ListIndex = -1 Then Exit Sub
 strHMOID = ""
 strHMOID = Mid(cboHMO.Text, InStr(cboHMO.Text, "[") + 1, Len(cboHMO.Text) - (InStr(cboHMO.Text, "[") + 1))
 strHMO = Mid(cboHMO.Text, 1, InStr(cboHMO.Text, "[") - 2)
 Label1.Caption = strHMO & " Tariff"
 
Dim rsRem As New Recordset
rsRem.Open "select * from vwCoyAndNhis where coyID='" & Replace(strHMOID, "'", "''") & "'", conSTR, adOpenStatic, adLockOptimistic
If Not rsRem.EOF Then
    strRem = rsRem!Remarks
Else
    strRem = "HMO"
End If
 
grdData.Visible = True
OrderGridDrug.Visible = False


Call fillGrid

'auto display of coy names in other pages
strHmoUpload = cboHMO.Text


Exit Sub
errH:
MsgBox Err.Description
'Call fillGridHMO
End Sub





Private Sub cboUse_Click()
'On Error GoTo errH
' 'Label1.Caption = "Tariff"
'
' If cboUse.ListIndex = 0 Or cboUse.ListIndex = -1 Then Exit Sub
'    strUseID = ""
' strUseID = Mid(cboUse.Text, InStr(cboUse.Text, "[") + 1, Len(cboUse.Text) - (InStr(cboUse.Text, "[") + 1))
' strUseCoy = Mid(cboUse.Text, 1, InStr(cboUse.Text, "[") - 2)
'
'
' 'Label1.Caption = strHMO & " Tariff"
'
''Dim rsRem As New Recordset
''rsRem.Open "select * from vwCoyAndNhis where coyID='" & Replace(strHMOID, "'", "''") & "'", conSTR, adOpenStatic, adLockOptimistic
''If Not rsRem.EOF Then
''    strRem = rsRem!Remarks
''Else
''    strRem = "HMO"
''End If
'
''Call fillGrid
'Exit Sub
'errH:
'MsgBox Err.Description
''Call fillGridHMO
End Sub

Private Sub cmdAdd_Click()
    strParam = "" 'nece
    enableFields True
    SetButtons (False)
'Frame1.Enabled = False
Frame2.Enabled = True

If strHmoUpload <> "" Then

    cboHMO.Text = strHmoUpload
    cboDrug.Text = "YES"
Else

    cboHMO.ListIndex = -1


End If


End Sub

Private Sub cmdDrug_Click()
frmDrugPriceHMOUpload.Hide
frmDrugPriceHMOUpload.Show

End Sub

Private Sub cmdLab_Click()
frmLabPriceHMOUpload.Hide
frmLabPriceHMOUpload.Show
End Sub

Private Sub cmdPix_Click()
On Error GoTo errH
txtPix.Text = ""
If Trim(cboHMO.Text) = "" Then
    MsgBox "Specify Company Name!"
    cboHMO.SetFocus
    Exit Sub
End If

Screen.MousePointer = vbHourglass

CommonDialog1.DialogTitle = "Select the File to Upload..."

CommonDialog1.Flags = cdlOFNFileMustExist
CommonDialog1.Filter = "Excel/CSV Files |*.XLSX;*.XLS;*.CSV;"  '(EXCEL)
'CommonDialog1.Filter = "HMOPlan Excel Files |*.XLSX;*.XLS;"  '(EXCEL)
'CommonDialog1.Filter = "HMOPlan Excel Files |*.DOCX;*.DOC;*.XLSX;.XLS;*.PDF;*.JPEG;*.JPG;"  '(WORD,EXCEL,PDF)
'CommonDialog1.Filter = "HMOPlan Files(*.DOC)|*.XLS)|PDF|*.*;"
'CommonDialog1.Filter = "HMOPlan Files|*.doc|Excel Worksheets|*.xls|PowerPoint Presentations|*.ppt" & _
'"|Office Files|*.doc;*.xls;*.ppt" & "|All Files|*.*"
CommonDialog1.ShowOpen

If Len(CommonDialog1.FileName) <> 0 Then
    'MsgBox InStrRev(CommonDialog1.FileName, "\")
  ' Right(CommonDialog1.FileName,)
    
    strFileOpen = ""
    strFileOpen = CommonDialog1.FileName
    
    strTxt = ""
    strTxt = Mid(CommonDialog1.FileName, InStrRev(CommonDialog1.FileName, "\") + 1)
    
    txtPix.Text = UCase(strTxt)    ' use strFileOpen to read from excel
    
    folderPath = ""
    folderPath = Mid(CommonDialog1.FileName, 1, InStrRev(CommonDialog1.FileName, "\"))
    
    
    fileExt = ""
    fileExt = Mid(strTxt, InStrRev(strTxt, "."))

  ''''''''''''''''''''''''''''''''''''''
        'Dim xlApp As Object
        'Dim xlWb As Object
        'Dim xlWs As Object
        
        Dim xlApp As New Excel.Application
        xlApp.Visible = False
        
        Dim xlWb As Excel.Workbook
        Dim xlWs As Excel.Worksheet

        'Set xlApp = CreateObject("Excel.Application")
        ''Set xlWb = xlApp.Workbooks.Add
        Set xlWb = xlApp.Workbooks.Open(strFileOpen)
        Set xlWs = xlWb.ActiveSheet   'Worksheets.Add
        
        ''Display Excel and give user control of Excel's lifetime
        xlApp.Visible = False
        'xlApp.UserControl = True
        
        
        
        cboSheet.Clear
        For Each xlWs In xlWb.Sheets
            cboSheet.AddItem (xlWs.Name)
        Next
        'cboSheet.ListIndex = 0
        
        
        ''xlWb.Close False
        xlApp.ActiveWorkbook.Close False, strFileOpen
        
        'For Each xlWb In Workbooks
        
        '    xlWb.Close SaveChanges:=False
        
        'Next xlWb
        
         
        'Release Excel references
        Set xlWs = Nothing
        Set xlWb = Nothing
        Set xlApp = Nothing
        
        'xlApp.Quit
        Excel.Application.Quit ' This solves the problem of leaving an excel instance hanging after completing the VB6 App
'''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        'uploadFromExcel = True
    

Else
    txtPix.Text = ""
End If

 Screen.MousePointer = vbDefault


'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''




Exit Sub
errH:
 Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub cmdRefresh_Click()
grdData.Visible = True
OrderGridDrug.Visible = False

Call fillGrid
End Sub

Private Sub cmdCancel_Click()

'Frame1.Enabled = False
Frame2.Enabled = True

SetButtons (True)
enableFields False
flgEdit = False

grdData.Visible = True
OrderGridDrug.Visible = False
fillGrid

Call clearFields

End Sub

Private Sub cmdDel_Click()
'On Error GoTo errH
'
''If cboDrug.Text = "" Or cboDrug.Text = " " Then
''MsgBox "Please specify Item to delete"
''cboDrug.SetFocus
''Exit Sub
''End If
'
'
'
'  Dim cmd As New Command
'  Dim strDel As String
'  Dim sSQlx As String
'  Dim intOk As Integer
'
'Dim cnn As Connection
'Set cnn = New Connection
'cnn.ConnectionString = conSTR
'cnn.Open
'cnn.BeginTrans
'
' intOk = MsgBox("Are you sure to Delete record ", vbYesNo, "Delete")
' If intOk = vbYes Then
'
'     strDel = grdData.Columns("Drug")
'        cmd.ActiveConnection = cnn
'        cmd.CommandType = adCmdText
'        cmd.CommandText = "Delete  from DrugNHIS where drgname ='" & strDel & "'"
'        cmd.Execute
'        cnn.CommitTrans
'        MsgBox "Record successfully Deleted"
'
'    On Error GoTo Er
'    Call fillGrid
'End If
'
'Exit Sub
'errH:
'cnn.RollbackTrans
'MsgBox Err.Description
'Exit Sub
'Er:
'MsgBox Err.Description
End Sub

Private Sub cmdEdit_Click()
'On Error Resume Next
''MsgBox "Sorry No edit for this Item!!!! You can only Delete item since bulk entry is involved"
'
'If grdData.Columns("SNo") = "" Then
'MsgBox "No Item to Edit. Select a Company Tariff to Display Items to Edit"
'Exit Sub
'End If
'
'Frame1.Enabled = True
'Frame2.Enabled = False
'
'EnableFields (True)
'SetButtons (False)
'
'
'flgEdit = True
'txtPrice.Enabled = True
'cboCap.Enabled = True
'cboHMO.Enabled = False
'strParam = "" 'nece
'strParam = grdData.Columns("SNo").Text
'strParam2 = grdData.Columns("Company").Text
'
'OldPrice = 0
'OldPrice = grdData.Columns("price").Text
'
'
'If OldPrice = 0 Then
'    txtPrice.Text = ""
'Else
'    txtPrice.Text = OldPrice
'End If
'
'txtDrug.Text = grdData.Columns("Drug").Text
'
'
'
'
'
'Exit Sub
'errH:
'  MsgBox Err.Description

End Sub

Private Sub Command2_Click()

End Sub

Private Sub cmdUpload_Click()
On Error GoTo errH



If Trim(txtPix.Text) = "" Then
    MsgBox "Specify Excel file to upload"
    cmdPix.SetFocus
    Exit Sub
End If

If Trim(cboSheet.Text) = "" Then
    MsgBox "Specify WorkSheet Name"
    cboSheet.SetFocus
    Exit Sub
End If

Screen.MousePointer = vbHourglass

    Dim sFile As String

        Dim rs As ADODB.Recordset
        Set rs = New ADODB.Recordset
        Dim sConn As String
        Dim itM As Integer
        Dim N As Integer
        'Dim sFile As String
                'Dim rs As ADODB.Recordset
                'Set rs = New ADODB.Recordset
                'Dim sConn As String
                'Dim itM As Integer
                'Dim N As Integer
                
        

    grdData.Visible = False
    OrderGridDrug.Visible = True

        OrderGridDrug.Rows = 2
        OrderGridDrug.TextMatrix(1, 0) = ""
        OrderGridDrug.TextMatrix(1, 1) = ""
        OrderGridDrug.TextMatrix(1, 2) = ""
    
With rs
  .CursorLocation = adUseClient
  .CursorType = adOpenKeyset
  .LockType = adLockBatchOptimistic
      

      'sConn = "DRIVER=Microsoft Excel Driver (*.xls);" & "DBQ=" & strFileOpen      'sFile
      '.Open "SELECT * FROM [" & cboSheet.Text & "$]", sConn      'sheet1$
      



'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    If fileExt = ".csv" Then
        Dim path As String
        path = folderPath ' "C:\TasuedStudents\" 'strFileOpen '
        Dim cN As ADODB.Connection
        'Dim rs As ADODB.Recordset
        Set cN = New ADODB.Connection
        'Set rs = New ADODB.Recordset
        cN.Open ("Provider=Microsoft.Jet.OLEDB.4.0;" & _
                       "Data Source=" & path & ";" & _
                       "Extended Properties=""text; HDR=Yes; FMT=Delimited; IMEX=1;""")
        rs.ActiveConnection = cN
        rs.Source = "select * from " & txtPix.Text '  fileName
        If rs.State = adStateOpen Then rs.Close
        rs.Open
        'Set grdData.DataSource = rs
        'Set getData = rs
    
    
    Else 'excel file
    
        
        
        If Trim(cboSheet.Text) = "" Then
            MsgBox "Specify WorkSheet Name"
            cboSheet.SetFocus
            Exit Sub
        End If
        
             
        If fileExt = ".xls" Then
              
            sConn = "DRIVER=Microsoft Excel Driver (*.xls);" & "DBQ=" & strFileOpen      'sFile
              
                        
        ElseIf fileExt = ".xlsx" Then
            
            sConn = "Driver={Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)};" & "DBQ=" & strFileOpen
            'sConn = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" & strFileOpen & "; Extended Properties=Excel 12.0 Xml;HDR=YES; IMEX=1;"
        End If
            
        If rs.State = adStateOpen Then rs.Close
        rs.Open "SELECT * FROM [" & Trim(cboSheet.Text) & "$]", sConn      'sheet1$
        'Set grdData.DataSource = rs
        
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    
    End If

     If Not .EOF Then
         .MoveFirst
         itM = 1
         For itM = 1 To .RecordCount
             If Not IsNull(.Fields(0)) Then
                 N = OrderGridDrug.Rows - 1
                 'MsgBox .Fields(1).Name
                 'If !price = 0 Or !price = 0 = "" Then
                 If Trim(.Fields(1)) = "" Then .Fields(1) = 0
                 'If .Fields(1) = 0 Or .Fields(1) = "" Then
                     'don't load
                 'Else
                 OrderGridDrug.TextMatrix(N, 0) = Replace(.Fields(0), " []", "") ' & " [" & .Fields(2) & "]", " []", "")
                 OrderGridDrug.TextMatrix(N, 1) = IIf(IsNull(.Fields(1)), 0, .Fields(1))
                 OrderGridDrug.TextMatrix(N, 2) = .Fields(2) & ""
                 
                 OrderGridDrug.Rows = OrderGridDrug.Rows + 1
                 'End If
             End If
             .MoveNext
         Next
             
    Else
             OrderGridDrug.Rows = 2
             OrderGridDrug.TextMatrix(1, 0) = ""
             OrderGridDrug.TextMatrix(1, 1) = ""
             OrderGridDrug.TextMatrix(1, 2) = ""
    End If
           


End With

    
    
    Label1.Caption = ""
    
    Label1.Caption = FormatNumber(rs.RecordCount, 0) & " Items found"

    Set rs = Nothing

'If rs.RecordCount > 0 Then
'    uploadFromExcel = True
'Else
'    uploadFromExcel = False
'End If

''''''''''''''''''''
Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub Form_Load()
On Error GoTo errH
  
If strApp <> "TARIFF" Then
    cmdAdd.Enabled = False
    OKButton.Enabled = False
End If
  
  
'Frame1.Enabled = False
Frame2.Enabled = True

    enableFields False
  
'Dim rsDrg As New Recordset
'    cboCat.Clear
'    cboCat.AddItem ""
'    cboCat.AddItem "(ALL)"
'  With rsDrg
'  .Open "select distinct drgcatname from DrugCategories", conSTR, adOpenForwardOnly, adLockReadOnly
'If Not .EOF Then
'.MoveFirst
'Do While Not .EOF
'cboCat.AddItem !drgcatname
'.MoveNext
'Loop
'End If
'End With

'cboPCent.Clear
'cboPCent.AddItem ""
'Dim X As Integer
'For X = 10 To 100 Step 10
'    cboPCent.AddItem X
'Next


cboDrug.Clear
'cboDrug.AddItem ""
cboDrug.AddItem "YES"
cboDrug.Text = "YES"


'cboUse.Clear
'cboUse.AddItem ""
'cboUse.AddItem "(OWN)"

cboHMO.Clear
cboHMO.AddItem ""
'cboHMO.AddItem "NHIS [NHIS]"

''Set rsDrg = Nothing
  Dim rsBL As New Recordset
  With rsBL
  .Open "select company,coyID from vwCoyAndNHIS order by Company", conSTR, adOpenForwardOnly, adLockReadOnly
If Not .EOF Then
    .MoveFirst
    Do While Not .EOF
    cboHMO.AddItem !Company & " [" & !CoyID & "]"
    .MoveNext
    Loop
End If


OrderGridDrug.FormatString = "-----<-------------------------------------Item-----------------------------------------------------------|----------------Price----------------->-----|-------<-------------------------------Category-----------------------------------"


'.Close
'  .Open "select coyname as company,coyID from vwDrugNHISListEntered where type='DRUG'  order by coyname", conSTR, adOpenForwardOnly, adLockReadOnly
'If Not .EOF Then
'    .MoveFirst
'    Do While Not .EOF
'    cboUse.AddItem !Company & " [" & !coyID & "]"
'    .MoveNext
'    Loop
'End If

End With


'Dim rsCat2 As New Recordset
'    cboCat2.Clear
'    cboCat2.AddItem ""
'  With rsCat2
'  .Open "select distinct pharmcat from DrugpharmCat", conSTR, adOpenForwardOnly, adLockReadOnly
'If Not .EOF Then
'.MoveFirst
'Do While Not .EOF
'cboCat2.AddItem !pharmCat
'.MoveNext
'Loop
'End If
'End With
'



Set rsBL = Nothing

Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub Form_Resize()
Me.Top = 0  '(Screen.Height - Me.Height) / 2
Me.Left = (Screen.Width - Me.Width) / 2

End Sub

Private Sub grdData_DblClick()
Call cmdEdit_Click
End Sub

Private Sub OKButton_Click()
On Error GoTo errH


'    'verify if template bulk entry for the coy already exists
'    Dim rsVer As New Recordset
'    rsVer.Open "select * from DrugNHIS where company='" & Replace(strHMOID, "'", "''") & "'", conSTR, adOpenStatic, adLockOptimistic
'    If Not rsVer.EOF Then
'        MsgBox "Bulk entry already exists for " & strHMO
'        Exit Sub
'    End If


    If cboHMO.ListIndex = -1 Or cboHMO.ListIndex = 0 Then
        MsgBox "Please specify Tariff Template Name"
        cboHMO.SetFocus
        Exit Sub
    End If


    
 

    If strHMOID = "" Then
        strHMOID = Mid(cboHMO.Text, InStr(cboHMO.Text, "[") + 1, Len(cboHMO.Text) - (InStr(cboHMO.Text, "[") + 1))
        strHMO = Mid(cboHMO.Text, 1, InStr(cboHMO.Text, "[") - 2)
        Label1.Caption = strHMO & " Tariff"
        'MsgBox "Please SELECT/Re-SELECT Company"
        'If cboHMO.Enabled = True Then cboHMO.SetFocus
        'Exit Sub
    End If
    
   
    
    
    If strHMOID = "" Then 'ok
        MsgBox "Please SELECT/Re-SELECT Company"
        If cboHMO.Enabled = True Then cboHMO.SetFocus
        Exit Sub
    End If
    
    

        
    If cboDrug.Text = "" Then
        cboDrug.Text = "YES"
        'cboDrug.SetFocus
        'Exit Sub
    End If

    If OrderGridDrug.Rows <= 2 Then
        MsgBox "No Data to Save! Please Click 'Upload Data' Button to upload Data "
        cmdUpload.SetFocus
        Exit Sub
    End If
                Dim OK As Integer
                Dim rsVer As New Recordset
                rsVer.Open "select * from hServiceNHIS where company='" & Replace(strHMOID, "'", "''") & "'", conSTR, adOpenStatic, adLockOptimistic
                If Not rsVer.EOF Then
                    If chkDel.Value = vbChecked Then
                        OK = MsgBox("You are about to make bulk save of SERVICE items 'Deleting Existing SERVICE Tariff' for " & strHMO & " Click YES to save items", vbYesNo, "bulk save")
                    Else
                        MsgBox "Bulk entry already exists for " & strHMO
                        Exit Sub
                    End If
                Else
                    OK = MsgBox("You are about to make bulk save of SERVICE items for " & strHMO & " Click YES to save items", vbYesNo, "bulk save")
                End If
            
            
            If OK = vbNo Then
                Exit Sub
            Else
                    Screen.MousePointer = vbHourglass
                    Dim rsHMO As New ADODB.Recordset
                    Dim rsExP As New ADODB.Recordset
                    '.CursorLocation = adUseServer
                    '.ActiveConnection = conSTR
                    
                    Dim cnn As Connection
                    Set cnn = New Connection
                    cnn.ConnectionString = conSTR
                    cnn.Open
                    cnn.BeginTrans
                    
                    Dim Cmd As New Command
                    Cmd.ActiveConnection = cnn
                    Cmd.CommandType = adCmdText
                    

                            
                            'verify if template bulk entry for the coy already exists
                           
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                                Call saveTariff(cnn)
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                                                cnn.CommitTrans
                                                flgEdit = False
                                                MsgBox "Record Succesfully saved!!!"
                                      Call Auditrail(m_fullname, "Add SERVICE Tariff for: " & cboHMO.Text, "", "New Tariff Entry", strHostName)
                    
                    End If
                     
                    enableFields False
                    SetButtons (True)
                    
                    grdData.Visible = True
                    OrderGridDrug.Visible = False

                    Call fillGrid 'çome b4 clearfields
                    Call clearFields
                    Call DeleteEmptyRows("Service")

                    Screen.MousePointer = vbDefault


Exit Sub
On Error GoTo Er
Screen.MousePointer = vbDefault
MsgBox Err.number & ":" & Err.Description
Exit Sub

errH:
    cnn.RollbackTrans
    Screen.MousePointer = vbDefault
    MsgBox Err.number & ":" & Err.Description

Set cnn = Nothing
Exit Sub

'If Err.number = "-2147217873" Then
'    Resume Next
'Else
'    cnn.RollbackTrans
'    Screen.MousePointer = vbDefault
'    MsgBox Err.number & ":" & Err.Description
'End If

Er:
'Set rsExP = Nothing
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub


Public Sub saveTariff(cnn As Connection)
On Error GoTo errH_Dup
        Dim rsHMO As New Recordset
        Dim rsExP As New Recordset
        Dim Cmd As New Command
        Cmd.ActiveConnection = cnn
        Cmd.CommandType = adCmdText
        Dim N As Integer
        
        If OrderGridDrug.Rows > 2 Then
        
            If chkDel.Value = vbChecked Then
                Cmd.CommandText = "delete from DrugNHISListEntered where CoyID='" & Replace(strHMOID, "'", "''") & "' and type ='SERVICE'" 'nece to prevent dup insert
                Cmd.Execute
                Cmd.CommandText = "delete from hServiceNHIS where Company='" & Replace(strHMOID, "'", "''") & "'" 'nece to prevent dup insert
                Cmd.Execute
                
                'cmd.CommandText = "update hRetainership set useTariff='" & Null & "', Pcent='" & Null & "' where retainID='" & strHMOID & "'"
                'cmd.Execute
            End If
            
            rsHMO.Open "select * from hServiceNHIS where 1=2", cnn, adOpenStatic, adLockOptimistic
        
                For N = 1 To OrderGridDrug.Rows - 2
                     If IsNumeric(OrderGridDrug.TextMatrix(N, 1)) Then  ' if has price
                        rsHMO.AddNew
                        rsHMO!service = TrimAllSpaces(OrderGridDrug.TextMatrix(N, 0))
                        rsHMO!Price = Trim(OrderGridDrug.TextMatrix(N, 1))
                        rsHMO!Category = Trim(OrderGridDrug.TextMatrix(N, 2))
                        rsHMO!Company = strHMOID
                        'rsHMO!pharmCat = "" '"XXXX"
                        rsHMO!Remarks = strRem   'cboHMO.Text
                        rsHMO!CoyName = strHMO
                        rsHMO!Capitated = "NO"
                        rsHMO!tariffStatus = "FIXED"
                        rsHMO.Update
                        
                        
                   End If
                Next
              
                            
                If cboDrug.Text = "YES" Then
                    Cmd.CommandText = "insert into DrugNHISListEntered(CoyID,Type,remarks,useTariff,Pcent) values ('" & Replace(strHMOID, "'", "''") & "','SERVICE','" & strRem & "','" & Replace(strHMOID, "'", "''") & "',1)"
                    Cmd.Execute
                    'cmd.CommandText = "update hRetainership set useTariff='" & Replace(strHMOID, "'", "''") & "', Pcent=" & 1 & " where retainID='" & strHMOID & "'"
                    'cmd.Execute
                End If
                       
                If strHMOID = "0001" Then
                    Cmd.CommandType = adCmdStoredProc
                    Cmd.CommandText = "TariffUpdates_ForPrivate_Service"
                    'Cmd.Parameters.Append Cmd.CreateParameter("@userName", adVarChar, adParamInput, 50, username)
                    Cmd.Execute
                End If
        End If

Exit Sub
errH_Dup:
If Err.number = "-2147217873" Then
    Resume Next
Else
    Err.Raise vbObjectError + 999, Err.Source, Err.Description
End If

End Sub

Public Sub clearFields()
'Set grdData.DataSource = Nothing

    
 'strHMOID = "" 'ok here
 'strUseID = "" 'ok here
    chkDel.Value = False
    'txtItem.Text = ""
    'txtNAme.Text = ""
     'cboHMO.ListIndex = -1
     cboSheet.ListIndex = -1
     'cboVal.ListIndex = -1
     cboDrug.ListIndex = -1
     'cboUse.ListIndex = -1
     'cboPCent.ListIndex = -1
    txtPix.Text = ""
    'txtPrice.Text = ""
    'txtUnits.Text = 1
    'txtReorder.Text = 0
    'txtPrice.Text = ""
    'txtPrice2.Text = 0
    'lblUnit.Caption = ""
    'lblCat.Caption = ""
     'cboCap.ListIndex = -1
    'txtDrug.Text = ""
'Frame1.Enabled = False
Frame2.Enabled = True

strParam = ""
strParam2 = ""
cboHMO.Enabled = True

        OrderGridDrug.Rows = 2
        OrderGridDrug.TextMatrix(1, 0) = ""
        OrderGridDrug.TextMatrix(1, 1) = ""
        OrderGridDrug.TextMatrix(1, 2) = ""

End Sub

Public Sub fillGrid()
On Error GoTo errH

Dim rsVal As New Recordset
With rsVal
Set grdData.DataSource = Nothing
grdData.clearFields
.CursorLocation = adUseClient

.Open "select SNO,Service,Category,Price,Capitated,tariffStatus,Company,Remarks,CoyID from vwServiceNHIS WHERE CoyID ='" & strHMOID & "' order by service", conSTR, adOpenStatic, adLockOptimistic
'.Open "select drgname as Service,drgcatname as Category,cost as Price,Remarks from hmoservices where remarks='" & cboHMO.Text & "' order by remarks,drgname", conSTR, adOpenStatic, adLockOptimistic


If Not .EOF Then
Set grdData.DataSource = rsVal
grdData.Columns("Service").Width = 3500
grdData.Columns("Category").Width = 2500
'grdData.Columns("Remarks").Width = 3500
grdData.Columns("Price").Width = 1000
grdData.Columns("sno").Visible = False
grdData.Columns("coyID").Visible = False
grdData.Columns("remarks").Visible = False
'grdData.Columns("unitsinstock").Visible = False
'grdData.Columns("reorderlevel").Visible = False
If strParam <> "" Then
    'locate updated row
    'On Error GoTo Er
    .Find "SNO = " & CSng(strParam)

End If
Else
Set grdData.DataSource = Nothing

End If
End With
'Set rsVal = Nothing

Call UseTariff


Exit Sub
errH:
MsgBox Err.Description

End Sub


Public Sub UseTariff()
'lblTariff ok here

'lblTariff.Caption = ""
'Dim rsRemX As New Recordset
'rsRemX.Open "select UseName,UseTariff,pcent from vwhRetainershipUseTariff where type='DRUG' and coyID='" & Replace(strHMOID, "'", "''") & "'", conSTR, adOpenStatic, adLockOptimistic
'If Not rsRemX.EOF Then
'    If IsNull(rsRemX!UseTariff) Then
'        lblTariff.Caption = "NONE"
'    ElseIf rsRemX!UseTariff = strHMOID Then
'        lblTariff.Caption = "OWN TARIFF"
'    ElseIf rsRemX!UseTariff <> strHMOID Then
'        lblTariff.Caption = (rsRemX!pcent * 100) & "% of " & rsRemX!UseName & ""
'    Else
'        lblTariff.Caption = "NONE"
'    End If
'Else
'        lblTariff.Caption = "NONE"
'End If
End Sub

Public Sub fillGridHMO()
'On Error GoTo errH
'Dim rsVal As New Recordset
'With rsVal
'Set grdData.DataSource = Nothing
'grdData.clearFields
'
'.Open "select drgname as Service,drgcatname as Category,cost as Price,Remarks from hmoservices where remarks='" & cboHMO.Text & "' order by remarks,drgname", conSTR, adOpenStatic, adLockOptimistic
'If Not .EOF Then
'Set grdData.DataSource = rsVal
''grdData.Columns("discontinued").Visible = False
''grdData.Columns("Service").Visible = False
''grdData.Columns("unitsinstock").Visible = False
''grdData.Columns("reorderlevel").Visible = False
'Else
'Set grdData.DataSource = Nothing
'
'End If
'End With
''Set rsVal = Nothing
'Exit Sub
'errH:
'MsgBox Err.Description
End Sub


Private Sub SetButtons(bVal As Boolean)
  cmdAdd.Visible = bVal
  cmdEdit.Visible = bVal
  OKButton.Visible = Not bVal
  cmdCancel.Visible = Not bVal
  cmdDel.Visible = bVal
  cmdRefresh.Visible = bVal
  
    ''cboCat.Enabled = bVal
    'cboDrug.Enabled = bVal
    'cboCat2.Enabled = bVal
    'txtPrice.Enabled = bVal
End Sub

Private Sub txtPrice_Change()
'On Error GoTo errH
'Dim objCon As New figToWrd
'Dim strCon As String
'strCon = objCon.Num2String(vaL(Replace(txtPrice.Text, ",", "")))
'lblWord.Caption = UCase(strCon)
'Exit Sub
'errH:
'MsgBox Err.Description

End Sub

Private Sub txtPrice2_Change()
'On Error GoTo errH
'Dim objCon As New figToWrd
'Dim strCon As String
'strCon = objCon.Num2String(vaL(Replace(txtPrice2.Text, ",", "")))
'lblWord2.Caption = UCase(strCon)
'Exit Sub
'errH:
'MsgBox Err.Description

End Sub


Private Sub txtSearch_Change()

If cboHMO.Text = "" Or cboHMO.Text = " " Then
        MsgBox "Please specify Tariff Company before Search"
        cboHMO.SetFocus
        txtSearch.Text = ""
        Exit Sub
    End If

Set grdData.DataSource = Nothing
grdData.Refresh

If Trim(txtSearch.Text) = "" Then Exit Sub
Dim strNameVal As String
Dim rsVal As New Recordset
strNameVal = txtSearch.Text
Dim ssQL As String
ssQL = "select SNO,Service,Category,Price,Capitated,tariffStatus,Company,Remarks,CoyID from vwServiceNHIS where coyID='" & strHMOID & "' and Service like '" & strNameVal & "%' order by Service"
With rsVal
.CursorLocation = adUseClient
.Open ssQL, conSTR, adOpenStatic, adLockOptimistic
'MsgBox ssQL
If Not .EOF Then
Set grdData.DataSource = rsVal
grdData.Columns("Service").Width = 3500
grdData.Columns("Category").Width = 2500
'grdData.Columns("Remarks").Width = 3500
grdData.Columns("Price").Width = 1000
grdData.Columns("sno").Visible = False
grdData.Columns("coyID").Visible = False
grdData.Columns("remarks").Visible = False
'grdData.Columns("unitsinstock").Visible = False
'grdData.Columns("reorderlevel").Visible = False

Else
Set grdData.DataSource = Nothing
End If
End With
'Set rsVal = Nothing



End Sub


Public Sub enableFields(bVal As Boolean)
Dim ctl As Control
For Each ctl In Me.Controls
    If TypeOf ctl Is TextBox Then
    ctl.Enabled = bVal
    End If
    
    If TypeOf ctl Is ComboBox Then
    ctl.Enabled = bVal
    End If
    
    If TypeOf ctl Is CheckBox Then
    ctl.Enabled = bVal
    End If
    
    If TypeOf ctl Is DTPicker Then
    ctl.Enabled = bVal
    End If
    
Next

txtSearch.Enabled = True
cboHMO.Enabled = True

'Frame1.Enabled = False
'Frame2.Enabled = True

End Sub
