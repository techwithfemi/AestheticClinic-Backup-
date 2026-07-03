VERSION 5.00
Object = "{CDE57A40-8B86-11D0-B3C6-00A0C90AEA82}#1.0#0"; "msdatgrd.ocx"
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomct2.ocx"
Begin VB.Form frmExpenses 
   BackColor       =   &H00FFC0C0&
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Expenses"
   ClientHeight    =   7050
   ClientLeft      =   2760
   ClientTop       =   3750
   ClientWidth     =   13335
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MDIChild        =   -1  'True
   MinButton       =   0   'False
   ScaleHeight     =   7050
   ScaleWidth      =   13335
   ShowInTaskbar   =   0   'False
   Begin VB.CommandButton cmdAll 
      Caption         =   "View All"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   420
      Left            =   6390
      TabIndex        =   27
      Top             =   495
      Width           =   1860
   End
   Begin VB.CommandButton cmdUnPosted 
      Caption         =   "View UnPosted"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   420
      Left            =   8460
      TabIndex        =   26
      Top             =   495
      Width           =   1860
   End
   Begin VB.CommandButton cmdDelete 
      Caption         =   "Delete"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   420
      Left            =   5940
      TabIndex        =   25
      Top             =   6570
      Width           =   1620
   End
   Begin VB.CommandButton cmdEdit 
      Caption         =   "Edit"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   420
      Left            =   2970
      TabIndex        =   24
      Top             =   6570
      Width           =   2565
   End
   Begin VB.CommandButton cmdAdd 
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
      Height          =   420
      Left            =   360
      TabIndex        =   23
      Top             =   6570
      Width           =   2385
   End
   Begin VB.CommandButton cmdPosted 
      Caption         =   "View Posted"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   420
      Left            =   10440
      TabIndex        =   18
      Top             =   495
      Width           =   1860
   End
   Begin VB.Frame Frame1 
      BackColor       =   &H00FFC0C0&
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   825
      Left            =   90
      TabIndex        =   12
      Top             =   990
      Width           =   13155
      Begin VB.TextBox txtDesc 
         Height          =   330
         Left            =   4140
         TabIndex        =   2
         Top             =   405
         Width           =   2985
      End
      Begin VB.CommandButton cmdAddToGrid 
         Caption         =   "Add to Journal"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         Left            =   11565
         TabIndex        =   5
         Top             =   360
         Width           =   1500
      End
      Begin VB.TextBox txtAmount 
         Height          =   330
         Left            =   2700
         TabIndex        =   1
         Top             =   405
         Width           =   1365
      End
      Begin VB.ComboBox cboCredit 
         Height          =   315
         Left            =   8910
         Sorted          =   -1  'True
         TabIndex        =   4
         Top             =   405
         Width           =   2625
      End
      Begin MSComCtl2.DTPicker DtDate 
         Height          =   375
         Left            =   7200
         TabIndex        =   3
         Top             =   405
         Width           =   1680
         _ExtentX        =   2963
         _ExtentY        =   661
         _Version        =   393216
         CheckBox        =   -1  'True
         Format          =   135528449
         CurrentDate     =   45189
      End
      Begin VB.ComboBox cboDebit 
         Height          =   315
         Left            =   0
         Sorted          =   -1  'True
         TabIndex        =   0
         Top             =   405
         Width           =   2670
      End
      Begin VB.Label Label6 
         BackStyle       =   0  'Transparent
         Caption         =   "Description"
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
         Left            =   4140
         TabIndex        =   17
         Top             =   135
         Width           =   2310
      End
      Begin VB.Label Label4 
         BackStyle       =   0  'Transparent
         Caption         =   "Amount"
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
         Left            =   2700
         TabIndex        =   16
         Top             =   135
         Width           =   690
      End
      Begin VB.Label Label3 
         BackStyle       =   0  'Transparent
         Caption         =   "Paying Acct (Credit)"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   8955
         TabIndex        =   15
         Top             =   135
         Width           =   2265
      End
      Begin VB.Label Label2 
         BackStyle       =   0  'Transparent
         Caption         =   "Date"
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
         Left            =   7200
         TabIndex        =   14
         Top             =   135
         Width           =   1320
      End
      Begin VB.Label Label1 
         BackStyle       =   0  'Transparent
         Caption         =   "Expense Acct (Debit)"
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
         Left            =   0
         TabIndex        =   13
         Top             =   135
         Width           =   2310
      End
   End
   Begin VB.CommandButton OKButton 
      Caption         =   "Post"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   420
      Left            =   360
      TabIndex        =   11
      Top             =   6570
      Width           =   2385
   End
   Begin VB.CommandButton CancelButton 
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
      Height          =   420
      Left            =   10710
      TabIndex        =   10
      Top             =   6570
      Width           =   1620
   End
   Begin VB.CommandButton cmdRefresh 
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
      Height          =   420
      Left            =   7920
      TabIndex        =   9
      Top             =   6570
      Width           =   2565
   End
   Begin VB.CommandButton cmdCancel 
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
      Height          =   420
      Left            =   2970
      TabIndex        =   8
      Top             =   6570
      Visible         =   0   'False
      Width           =   2565
   End
   Begin MSDataGridLib.DataGrid DataGrid1 
      Height          =   4605
      Left            =   45
      TabIndex        =   6
      Top             =   1845
      Width           =   13200
      _ExtentX        =   23283
      _ExtentY        =   8123
      _Version        =   393216
      AllowUpdate     =   0   'False
      BackColor       =   16777215
      HeadLines       =   1
      RowHeight       =   21
      TabAction       =   1
      BeginProperty HeadFont {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   13.5
         Charset         =   0
         Weight          =   700
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
      Caption         =   "Expenses"
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
   Begin MSComCtl2.DTPicker DTPicker1 
      DataField       =   "MaintDate"
      Height          =   360
      Left            =   1080
      TabIndex        =   19
      Top             =   540
      Width           =   2085
      _ExtentX        =   3678
      _ExtentY        =   635
      _Version        =   393216
      Format          =   135659521
      CurrentDate     =   38278
   End
   Begin MSComCtl2.DTPicker DTPicker2 
      DataField       =   "MaintDate"
      Height          =   360
      Left            =   4140
      TabIndex        =   20
      Top             =   540
      Width           =   2085
      _ExtentX        =   3678
      _ExtentY        =   635
      _Version        =   393216
      Format          =   135659521
      CurrentDate     =   38278
   End
   Begin VB.Label Label8 
      BackStyle       =   0  'Transparent
      Caption         =   "Between"
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
      Left            =   180
      TabIndex        =   22
      Top             =   630
      Width           =   780
   End
   Begin VB.Label Label7 
      Alignment       =   2  'Center
      BackStyle       =   0  'Transparent
      Caption         =   "AND"
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
      Left            =   3330
      TabIndex        =   21
      Top             =   585
      Width           =   600
   End
   Begin VB.Label Label5 
      Alignment       =   2  'Center
      BackColor       =   &H00404000&
      Caption         =   "Expenses (Journal Entries)"
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
      Height          =   420
      Left            =   0
      TabIndex        =   7
      Top             =   0
      UseMnemonic     =   0   'False
      Width           =   13200
   End
End
Attribute VB_Name = "frmExpenses"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'''''''''
Option Explicit
Private flgEdit As Boolean
Private AcctID_Debit As String
Private AcctID_Credit As String
Private strAcctDebit As String
Private strAcctCredit As String
Private strParam As String
Private lngSNo As Long
Private viewIndex As Integer


Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
        (ByVal hWnd As Long, ByVal wMsg As Long, ByVal wParam As Long, lParam As Any) As Long
    Private Const CB_FINDSTRING = &H14C

Private Sub CancelButton_Click()
Unload Me

End Sub

Private Sub cboCredit_Click()
On Error GoTo errH
If Trim(cboCredit.Text) = "" Then
    AcctID_Credit = ""
    strAcctCredit = ""
    Exit Sub
End If

'If cboCredit.ListIndex = 0 Or cboCredit.ListIndex = -1 Then
'    AcctID_Credit = ""
'    strAcctCredit = ""
'    Exit Sub
'End If

 AcctID_Credit = Mid(cboCredit.Text, InStr(cboCredit.Text, "[") + 1, Len(cboCredit.Text) - (InStr(cboCredit.Text, "[") + 1))
strAcctCredit = Mid(cboCredit.Text, 1, InStr(cboCredit.Text, "[") - 1)
Label5.Caption = "Credit: " & strAcctCredit

Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cboCredit_KeyUp(KeyCode As Integer, Shift As Integer)
        On Error GoTo errH 'Resume Next
                
       Dim sCurrentText As String
        Dim lItemIndex As Long
        
        'Allow the backspace
        If KeyCode = 8 Then Exit Sub  'backspace
        
        'Get the current text
        sCurrentText = cboCredit.Text 'Trim(cboCredit.Text)
        
        'search for a pattern match
        lItemIndex = SendMessage(cboCredit.hWnd, CB_FINDSTRING, -1, ByVal sCurrentText)
        If lItemIndex = -1 Then Exit Sub
        
        ''Set the index to the first matched item
        'cboCredit.ListIndex = lItemIndex  'lstNdx
        
        'Select the remaining text of the matched item
        cboCredit.SelStart = Len(sCurrentText)
        cboCredit.SelLength = Len(cboCredit.Text) - Len(sCurrentText)
        
        cboCredit.Text = sCurrentText
        cboCredit.SelStart = Len(sCurrentText) + 1
Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub cboCredit_GotFocus()
On Error GoTo errH
'cboDrug.DroppedDown = True
''Sendkeys "{F4}"
'Dim WshSell As Object
'Set WshShell = CreateObject("WScript.Shell")
'WshShell.SendKeys "{F4}"

Dim WshShell As Object
Set WshShell = CreateObject("WScript.Shell")
If (WshShell Is Nothing) Then
        'whatever you want to do on failure of setting object reference
Else
        WshShell.SendKeys "{F4}"
        Set WshShell = Nothing
End If

'ShowDropDown cboDrug
    
Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cboCredit_KeyPress(KeyAscii As Integer)

   If (KeyAscii = vbKeyReturn) Then
        cmdAddToGrid.SetFocus
    End If
    
    KeyAscii = Asc(UCase(Chr(KeyAscii)))

End Sub

Private Sub cboDebit_Click()
On Error GoTo errH
If Trim(cboDebit.Text) = "" Then
    AcctID_Debit = ""
    strAcctDebit = ""
    Exit Sub
End If

'If cboDebit.ListIndex = 0 Or cboDebit.ListIndex = -1 Then
'    AcctID_Debit = ""
'    strAcctDebit = ""
'    Exit Sub
'End If

AcctID_Debit = Mid(cboDebit.Text, InStr(cboDebit.Text, "[") + 1, Len(cboDebit.Text) - (InStr(cboDebit.Text, "[") + 1))
strAcctDebit = Mid(cboDebit.Text, 1, InStr(cboDebit.Text, "[") - 1)
Label5.Caption = "Debit: " & strAcctDebit

Exit Sub
errH:
MsgBox Err.Description
End Sub



Private Sub cboDebit_KeyPress(KeyAscii As Integer)
'Call EnterToTab(KeyAscii)
    If (KeyAscii = vbKeyReturn) Then
        txtAmount.SetFocus
    End If
End Sub

Private Sub cboDebit_KeyUp(KeyCode As Integer, Shift As Integer)
        On Error GoTo errH 'Resume Next
                
       Dim sCurrentText As String
        Dim lItemIndex As Long
        
        'Allow the backspace
        If KeyCode = 8 Then Exit Sub  'backspace
        
        'Get the current text
        sCurrentText = cboDebit.Text 'Trim(cbodebit.Text)
        
        'search for a pattern match
        lItemIndex = SendMessage(cboDebit.hWnd, CB_FINDSTRING, -1, ByVal sCurrentText)
        If lItemIndex = -1 Then Exit Sub
        
        ''Set the index to the first matched item
        'cbodebit.ListIndex = lItemIndex  'lstNdx
        
        'Select the remaining text of the matched item
        cboDebit.SelStart = Len(sCurrentText)
        cboDebit.SelLength = Len(cboDebit.Text) - Len(sCurrentText)
        
        cboDebit.Text = sCurrentText
        cboDebit.SelStart = Len(sCurrentText) + 1
Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub cboDebit_GotFocus()
On Error GoTo errH
'cboDrug.DroppedDown = True
''Sendkeys "{F4}"
'Dim WshSell As Object
'Set WshShell = CreateObject("WScript.Shell")
'WshShell.SendKeys "{F4}"

Dim WshShell As Object
Set WshShell = CreateObject("WScript.Shell")
If (WshShell Is Nothing) Then
        'whatever you want to do on failure of setting object reference
Else
        WshShell.SendKeys "{F4}"
        Set WshShell = Nothing
End If

'ShowDropDown cboDrug
    
Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cmdAdd_Click()
            flgEdit = False
            enableFields (True)
            SetButtons (False)
            'cboDebit.SetFocus

End Sub

Private Sub cmdAddToGrid_Click()
On Error GoTo errH

If IsNull(DtDate.Value) Then
    MsgBox "Please specify Tran Date"
    DtDate.SetFocus
    Exit Sub
End If

If Trim(txtAmount.Text) = 0 Or Trim(txtAmount.Text) = "" Then
    MsgBox "Please specify Amount"
    txtAmount.SetFocus
    Exit Sub
End If

If Trim(txtDesc.Text) = "" Then
    MsgBox "Please specify Description"
    txtDesc.SetFocus
    Exit Sub
End If

'If cboDebit.ListIndex = 0 Or cboDebit.ListIndex = -1 Then
'    MsgBox "Please Select Expense Acct to Debit"
'    cboDebit.SetFocus
'    Exit Sub
'End If

If Trim(cboDebit.Text) = "" Or Trim(AcctID_Debit) = "" Then
    MsgBox "Please Select Expense Acct to Debit"
    cboDebit.SetFocus
    Exit Sub
End If


'If cboCredit.ListIndex = 0 Or cboCredit.ListIndex = -1 Then
'    MsgBox "Please Select Paying Acct to Credit"
'    cboCredit.SetFocus
'    Exit Sub
'End If

If Trim(cboCredit.Text) = "" Or Trim(AcctID_Credit) = "" Then
    MsgBox "Please Select Paying Acct to Credit"
    cboCredit.SetFocus
    Exit Sub
End If

Screen.MousePointer = vbHourglass
       
    Call SaveToJournal

    
   
    On Error GoTo errH
        AcctID_Debit = ""
        strAcctDebit = ""
        AcctID_Credit = ""
        strAcctCredit = ""
        cboDebit.Text = ""
        cboCredit.Text = ""
        
        flgEdit = False
        Screen.MousePointer = vbDefault
        'SetButtons (True)
        'EnableFields (False)
        Call clearFields
        Call fillGrid(2)
        'MsgBox "Record Succesfully saved"
 
cboDebit.SetFocus
Screen.MousePointer = vbDefault
        
 Exit Sub
errTranFail:
Screen.MousePointer = vbDefault
'connTran.rollbaktrans
MsgBox Err.Description
        
Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
End Sub

Private Sub SaveToJournal()

 Screen.MousePointer = vbHourglass
 
       Dim connTran As New Connection
        connTran.ConnectionString = conStrAccts
        connTran.Open
        
        On Error GoTo TransFail
        
        connTran.BeginTrans

    Dim rsExP As New ADODB.Recordset
    With rsExP
    .CursorLocation = adUseClient
    .ActiveConnection = connTran
        
        'for edit, delete then insert
        If flgEdit Then
            .Open "select * from TranxactionJournalTemp where SNO=" & lngSNo, connTran, adOpenStatic, adLockOptimistic
            If Not .EOF Then
                If DataGrid1.Columns("Posted").Text = "YES" Then
                    Dim tranID As String
                    tranID = DataGrid1.Columns("TranNo").Text
                    If AcctPostOn = True And IsNull(tranID) = False Then
                       Call DeleteFromAccounts(tranID, connTran)
                    End If
                Else
                    ![TranDate] = DtDate.Value
                    '![TranID]=0
                    ![AccountDebit] = AcctID_Debit
                    ![AccountCredit] = AcctID_Credit
                    ![coyID] = coyID
                    ![Amount] = CDbl(txtAmount.Text)
                    ![Description] = Trim(txtDesc.Text)
                    ![TranCat] = "j"
                    '![isPost] = 0
                    ![Remarks] = "EXPENSES"
                    ![username] = m_Username
                    .Update
                    connTran.CommitTrans
                    Call Auditrail(m_Username, "Edited to Item: " & Trim(txtDesc.Text), cboDebit.Text, txtAmount.Text, strHostName)
                    Exit Sub ' nece
                    'MsgBox "Record Succesfully Updated"
                End If
            End If
        End If
            
            'insert
            If .State = adStateOpen Then .Close
            .Open "select * from TranxactionJournalTemp where 1=2", connTran, adOpenStatic, adLockOptimistic
            .AddNew
            ![TranDate] = DtDate.Value
            '![TranID]=0
            ![AccountDebit] = AcctID_Debit
            ![AccountCredit] = AcctID_Credit
            ![coyID] = coyID
            ![Amount] = CDbl(txtAmount.Text)
            ![Description] = Trim(txtDesc.Text)
            ![TranCat] = "j"
            ![isPost] = 0
            ![Remarks] = "EXPENSES"
            ![username] = m_Username
            .Update
            connTran.CommitTrans
            Call Auditrail(m_Username, "Added Item: " & Trim(txtDesc.Text), cboDebit.Text, txtAmount.Text, strHostName)
            'MsgBox "Record Succesfully saved"
    
    End With
    
 
Exit Sub

TransFail:
Screen.MousePointer = vbDefault
connTran.RollbackTrans
MsgBox Err.Description
 

End Sub

Private Sub cmdAddToGrid_KeyPress(KeyAscii As Integer)
    'If (KeyAscii = vbKeyReturn) Then
    '     cboDebit.SetFocus
    ' End If

End Sub

Private Sub cmdAll_Click()
On Error GoTo errH
Call fillGrid(1)

Exit Sub

errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
End Sub

Private Sub cmdCancel_Click()
flgEdit = False
SetButtons (True)
enableFields (False)
Call clearFields

End Sub

Private Sub cmdDelete_Click()
 
  Dim strDel As String
  Dim sSQlx As String
  Dim intOK As Integer
On Error GoTo errH
intOK = MsgBox("Are you sure to Delete Item " & DataGrid1.Columns("Description").Text, vbYesNo, "Delete")
If intOK = vbYes Then
    Screen.MousePointer = vbHourglass
    strDel = DataGrid1.Columns("SNO").Text
        
        Dim connTran As New Connection
        connTran.ConnectionString = conStrAccts
        connTran.Open
        
        On Error GoTo TransFail
        
        connTran.BeginTrans
        Dim Cmd As New Command
        Cmd.ActiveConnection = connTran
        Cmd.CommandType = adCmdText

    sSQlx = "delete from TranxactionJournalTemp where SNO = '" & strDel & "'"
    Cmd.CommandText = sSQlx
    Cmd.Execute
    
    If DataGrid1.Columns("Posted").Text = "YES" Then
        Dim tranID As String
        tranID = DataGrid1.Columns("TranNo").Text
        If AcctPostOn = True And IsNull(tranID) = False Then
           Call DeleteFromAccounts(tranID, connTran)
        End If
    End If
    
    connTran.CommitTrans
    Screen.MousePointer = vbDefault
    Call Auditrail(m_Username, "Deleted Item: " & DataGrid1.Columns("Description").Text, DataGrid1.Columns("DebitAccount").Text, DataGrid1.Columns("Amount").Text, strHostName)
    MsgBox " Record successfully deleted"
    Call fillGrid(1)
   

End If

Exit Sub
TransFail:
Screen.MousePointer = vbDefault
connTran.RollbackTrans
MsgBox Err.Description
                
Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub cmdPosted_Click()
On Error GoTo errH
Call fillGrid(3)

Exit Sub

errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub cmdEdit_Click()
On Error GoTo errH
If DataGrid1.Columns("Closed").Text = "YES" Then
    MsgBox "This Item Cannot be Edited! Accounting Period Already Closed"
    Exit Sub
End If

On Error Resume Next

SetButtons (False)
enableFields (True)
flgEdit = True
    lngSNo = DataGrid1.Columns("SNO").Text
    txtAmount.Text = DataGrid1.Columns("Amount").Text
    txtDesc.Text = DataGrid1.Columns("Description").Text
    DtDate.Value = DataGrid1.Columns("TranDate").Text
    
    cboDebit.Text = DataGrid1.Columns("DebitAccount").Text & "[" & DataGrid1.Columns("DebitAcctID").Text & "]"
    cboCredit.Text = DataGrid1.Columns("CreditAccount").Text & "[" & DataGrid1.Columns("CreditAcctID").Text & "]"
    
    Call cboDebit_Click
    Call cboCredit_Click
    
    ''one of them is zwro here for amount
    'If DataGrid1.Columns("Debit").Text <> 0 Then
    '    txtAmount.Text = DataGrid1.Columns("Debit").Text
    'End If
    '
    'If DataGrid1.Columns("Credit").Text <> 0 Then
    '    txtAmount.Text = DataGrid1.Columns("Credit").Text
    'End If
    


    
Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cmdRefresh_Click()
On Error GoTo errH

Call Form_Load
'Call loadComboAccts
'fillGrid (1)
Exit Sub

errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub cmdUnPosted_Click()

On Error GoTo errH
'Label5.Caption = "UnPosted Entries between " & DTPicker1.Value & " and " & DTPicker2.Value & " for " & m_Username
Call fillGrid(2)

Exit Sub

errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub DataGrid1_Click()
'    Dim colIndex As Integer
'    Dim rowIndex As Integer
'
'    colIndex = DataGrid1.Col
'    rowIndex = DataGrid1.row
'
'    'TODO: Adjust the position and size of the ComboBox and DatePicker according to your needs
'    ComboBox1.Left = DataGrid1.Left + DataGrid1.Columns(colIndex).Left
'    ComboBox1.Top = DataGrid1.Top + DataGrid1.RowHeight * (DataGrid1.row + 3)
'    ComboBox1.Width = DataGrid1.Columns(colIndex).Width
'
'    DatePicker1.Left = DataGrid1.Left + DataGrid1.Columns(colIndex).Left
'    DatePicker1.Top = DataGrid1.Top + DataGrid1.RowHeight * (DataGrid1.row + 3)
'
'    'Show ComboBox or DatePicker depending on the column
'    If colIndex = 1 Then
'        ComboBox1.Visible = True
'        ComboBox1.SetFocus
'    ElseIf colIndex = 2 Then
'        DatePicker1.Visible = True
'        DatePicker1.SetFocus
'    End If


End Sub

Private Sub ComboBox1_LostFocus()
'    DataGrid1.Text = ComboBox1.Text
'    ComboBox1.Visible = False
End Sub

Private Sub DataGrid1_RowColChange(lastRow As Variant, ByVal LastCol As Integer)
'    Static lastRow2 As Variant
'
'    If Not IsEmpty(lastRow) Then  'And Not lastRow Is Nothing
'        'If Not lastRow2 Is Nothing Then
'        'Save the last row to the database
'        Call SaveRow(lastRow)
'
'        'Update the last row
'        lastRow2 = DataGrid1.Bookmark
'    End If
'
'    'Update the last row
'    'lastRow2 = DataGrid1.Bookmark
'
End Sub

Private Sub SaveRow(row As Variant)
'    Dim expense As String
'    Dim dateValue As Date
'    Dim Amount As Double
'
'    'Get the values from the DataGrid
'    expense = DataGrid1.Columns(0).CellValue(row)
'    dateValue = CDate(DataGrid1.Columns(1).CellValue(row))
'    Amount = CDbl(DataGrid1.Columns(2).CellValue(row))
'
'    'TODO: Validate the values
'
'    'TODO: Insert the values into the database
End Sub

Private Sub DatePicker1_LostFocus()
'    DataGrid1.Text = Format(DatePicker1.Value, "mm/dd/yyyy")
'    DatePicker1.Visible = False
End Sub

Private Sub cmdInsert_Click()
'    Dim expense As String
'    Dim dateValue As Date
'    Dim Amount As Double
'
'    'Get the values from the controls
'    expense = TextBox1.Text
'    dateValue = CDate(DataGrid1.TextMatrix(DataGrid1.row, 2))
'    Amount = CDbl(DataGrid1.TextMatrix(DataGrid1.row, 3))
'
'    'TODO: Validate the values
'
'    'TODO: Insert the values into the database
End Sub





Private Sub DtDate_KeyPress(KeyAscii As Integer)
   If (KeyAscii = vbKeyReturn) Then
        cboCredit.SetFocus
        KeyAscii = 0
    End If

End Sub

Private Sub Form_Load()
    'TODO: Load the DataGrid with data from your database

On Error GoTo errH
DTPicker1.Value = sysDate
DTPicker2.Value = sysDate
DtDate.Value = sysDate
DtDate.Value = ""
DataGrid1.Caption = "Tranxactions"
Call loadComboAccts

Call enableFields(False)
SetButtons (True)
Call clearFields
Call fillGrid(1)
'Label5.Caption = "All Entries between " & DTPicker1.Value & " and " & DTPicker2.Value & " for " & m_Username

Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub enableFields(ByVal xVal As Boolean)
        txtAmount.Enabled = xVal
        cboCredit.Enabled = xVal
        cboDebit.Enabled = xVal
        DtDate.Enabled = xVal
        txtDesc.Enabled = xVal
        'DTPicker1.Enabled = xVal
        'DTPicker2.Enabled = xVal

    End Sub
    
    Public Sub clearFields()
        viewIndex = 0
        flgEdit = False
        Label5.Caption = "Expenses (Direct Entries)"
        AcctID_Credit = ""
        AcctID_Debit = ""
        strAcctCredit = ""
        strAcctDebit = ""
        cboCredit.Text = ""
        cboDebit.Text = ""
        DtDate.Value = ""
        txtDesc.Text = ""
        txtAmount.Text = ""
 End Sub
 
 Private Sub SetButtons(ByVal bVal As Boolean)
        cmdAdd.Visible = bVal
        cmdEdit.Visible = bVal
        cmdCancel.Visible = Not bVal
        cmdDelete.Visible = bVal
        'cmdRefresh.Visible = bVal
        If AcctPostOn = True Then
            OKButton.Visible = Not bVal
        Else
            OKButton.Visible = False
        End If

    End Sub

Public Sub loadComboAccts()
On Error GoTo errH

Screen.MousePointer = vbHourglass

Dim UserAcctSQL As String
Dim rstLoadDebit As ADODB.Recordset
Dim rstLoadCredit As ADODB.Recordset

Dim conuseracct As ADODB.Connection
Dim cnt As Integer
cnt = 0

'Set rstLoadAcct = New ADODB.Recordset
'Set conuseracct = New ADODB.Connection
'
'    With conuseracct
'        .ConnectionString = conStrAccts
'        .Open
'    End With
    
    
    'UserAcctSQL = "SELECT distinct empName,empID FROM vwPaymentsByCashier where receiptdate between '" & DTPicker1.Value & "' and '" & DTPicker2.Value & "' order by empName"
    ''UserAcctSQL = "SELECT * FROM qryhCardNotExpired order by psurname"
    'rstLoadAcct.Open UserAcctSQL, conuseracct, adOpenStatic, adLockOptimistic
    
    cboDebit.Clear
    cboDebit.AddItem ""
    Set rstLoadDebit = loadCombo("EXPENSES") 'returns expense headers
        
    With rstLoadDebit
        Do While Not .EOF
        cboDebit.AddItem !AccountName & "[" & !AccountNo & "]"
        .MoveNext
        Loop
    End With
    rstLoadDebit.Close
    Set rstLoadDebit = Nothing
    
    
    cboCredit.Clear
    cboCredit.AddItem ""
    Set rstLoadCredit = loadCombo("BANKS-CASH") 'returns cash
    With rstLoadCredit
        Do While Not .EOF
        cboCredit.AddItem !AccountName & "[" & !AccountNo & "]"
        .MoveNext
        Loop
    End With
    rstLoadCredit.Close
    Set rstLoadCredit = Nothing

Screen.MousePointer = vbDefault
    
    Exit Sub
    
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub




Public Sub fillGrid(X As Integer)
On Error GoTo errH
Dim UserAcctSQL As String
           Dim ssQL As String
            Dim rs As New Recordset
            Dim conn As New Connection
            With conn
                .CursorLocation = adUseClient
                .ConnectionString = conStrAccts
                .Open
            End With
            
            Dim Cmd As New Command
            Cmd.ActiveConnection = conn
            Cmd.CommandType = adCmdStoredProc
            Cmd.CommandTimeout = 600
            'conn.Open
            
            rs.CursorLocation = adUseClient
            Set DataGrid1.DataSource = Nothing
            viewIndex = X 'source of data to view in grid
            Cmd.CommandText = "getDayBookForGridTemp"
            Cmd.Parameters.Append Cmd.CreateParameter("@StartDate", adDBTimeStamp, adParamInput, 50, DTPicker1.Value)
            Cmd.Parameters.Append Cmd.CreateParameter("@EndDate", adDBTimeStamp, adParamInput, 50, DTPicker2.Value)
            'cmd.Parameters.Append cmd.CreateParameter("@ActionTime", adDBTimeStamp, adParamInput, 50, sysTime)
            'cmd.Parameters.Append cmd.CreateParameter("@AccountNo", adVarChar, adParamInput, 50, "(ALL)")
            Cmd.Parameters.Append Cmd.CreateParameter("@CoyID", adVarChar, adParamInput, 50, coyID)
            Cmd.Parameters.Append Cmd.CreateParameter("@userName", adVarChar, adParamInput, 1000, m_Username)
            Cmd.Parameters.Append Cmd.CreateParameter("@ViewIndex", adInteger, adParamInput, 1000, X)
            Set rs = Cmd.Execute
        With rs
            If Not .EOF Then
                Set DataGrid1.DataSource = rs
                DataGrid1.Columns("Amount").NumberFormat = "#,###.00"
                DataGrid1.Columns("Amount").Alignment = dbgRight
                'DataGrid1.Columns("Credit").NumberFormat = "#,###.00"
                'DataGrid1.Columns("Credit").Alignment = dbgRight
                DataGrid1.Columns("SNo").Visible = False
                DataGrid1.Columns("SN").Width = 1000
                DataGrid1.Columns("SN").NumberFormat = "#,###"
                'DataGrid1.Columns("AmountPaid").Alignment = dbgRight
                'DataGrid1.Columns("Balance").NumberFormat = "#,###.00"
                'DataGrid1.Columns("Balance").Alignment = dbgRight
            Else
                 Set DataGrid1.DataSource = Nothing
            End If
        End With
        
        Select Case X
        Case 1
            Label5.Caption = "All Entries between " & DTPicker1.Value & " and " & DTPicker2.Value & " for " & m_Username
        Case 2
            Label5.Caption = "UnPosted Entries between " & DTPicker1.Value & " and " & DTPicker2.Value & " for " & m_Username
        Case 3
            Label5.Caption = "Posted Entries between " & DTPicker1.Value & " and " & DTPicker2.Value & " for " & m_Username
        Case Else
            Label5.Caption = "Expenses (Direct Entries)"
        End Select
        

Exit Sub
errH:
MsgBox Err.Description

End Sub



Private Sub datagrid1_DblClick()
On Error GoTo errH

Call cmdEdit_Click

Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub Form_Resize()
Me.Left = (Screen.Width - Me.Width) \ 2
Me.Top = 0

End Sub

Private Sub OKButton_Click()
       ''''''''post to Accounts''''''if isPost=0'''''''And AcctPostType_Expenses = "AUTO" And '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If AcctPostOn = True Then ''''''' And AcctPostType_Expenses_PostAfterPayment = "YES" Then
                    Screen.MousePointer = vbHourglass
                    
                    Dim rsAccts2 As New Recordset
                    Dim IX As Integer
                    Dim connTran2 As New Connection
                    connTran2.ConnectionString = conStrAccts
                    connTran2.Open
                   
                    On Error GoTo TransFailAccts
                   
                   connTran2.BeginTrans
                   
                Dim Description As String
                Dim AcctNoDebit As String
                Dim AcctNoCredit As String
                Dim PayDate As Date
                Dim AmtPaid As Double
                Dim snoIDx As Long
                Dim vouchNo As String
                
                vouchNo = "AUTO"
                
                Dim cmd2 As New Command
                cmd2.ActiveConnection = connTran2
                cmd2.CommandType = adCmdText
                Dim strSuspGp As String
                
                With rsAccts2 ''dummy acct is acctNo '0000000'
                .Open "select SNo,TranDate,Description,Amount,AccountCredit,AccountDebit from Accounting..vwTranxJournalTempWithNoDummyAcctNo where isnull(isPost,0)=0 " & _
                " and tranDate between '" & DTPicker1.Value & "' and '" & DTPicker2.Value & "' order by SNo ", connTran2, adOpenStatic, adLockReadOnly
                If Not .EOF Then
                    
                    'MsgBox .RecordCount
                    'Call getTranID(connTran2) 'very nece 'outside the for---next statement of vwRctForAccts
                    'Period = getPeriod(connTran2, !PayDate)
                    
                    Call getAccountInfo(connTran2, sysDate, "PAYABLE", "EXPENSES")
                 
                    .MoveFirst
                    Do While Not .EOF
                        snoIDx = !SNo
                        Description = !Description & ""
                        AcctNoDebit = !AccountDebit & ""
                        AcctNoCredit = !AccountCredit & ""
                        PayDate = CDate(!TranDate)
                        AmtPaid = CDbl(!Amount)
                        
                        Select Case Mid(AcctNoDebit, 1, 1)
                            Case 5
                                strSuspGp = "EXPENSES"
                            Case 2
                                strSuspGp = "PAYABLE"
                        End Select
                        
                        Call PostToAccounts(connTran2, PayDate, AcctNoDebit, AmtPaid, Description, strSuspGp, "j")  'debit side
                        Call PostToAccounts(connTran2, PayDate, AcctNoCredit, -(AmtPaid), Description, "ASSET", "j") 'credit side
                        
                        cmd2.CommandText = "update TranxactionJournalTemp set isPost=1, tranID='" & TRAN_ID & "' where SNo=" & snoIDx
                        cmd2.Execute
                        
                        .MoveNext
                    Loop
                
                    'Else ' cos of AcctPostType_Expenses_PostAfterPayment
                    '
                    '    cmd2.CommandText = "update " & DBName & "..hExpensePayDetails set isPost=1 where VouchNo='" & vouchNo & "'"
                    '    cmd2.Execute
                End If
            End With
            
            'hospital..tbl used here bcos connTran2 refers to conStrAccts
            'cmd2.CommandText = "update " & DBName & "..hExpensePayDetails set isPost=1 where VouchNo='" & vouchNo & "'"
            'cmd2.Execute
            '''''''''''''''''Confirm Dr=Cr'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            
            Dim rsAmt As New Recordset
                    
            If AcctPostType = "AUTO" Then
                    'Dim rsX As New Recordset
                    'rsX.Open "SELECT  SUM (amount) as totTrx FROM tranxaction where Period='09/2023' and CoyID = '0001'", connTran2, adOpenDynamic, adLockBatchOptimistic
                    ' If Not rsX.EOF Then
                    '    MsgBox rsX!totTrx
                    '    rsX.Close
                    '    rsX.Open "SELECT  SUM (AccountClAmt) as totCOA FROM ChartOfAccounts where Period='09/2023' and CoyID = '0001'", connTran2, adOpenDynamic, adLockBatchOptimistic
                    '    If Not rsX.EOF Then
                    '       MsgBox rsX!totCOA
                    '     End If
                    'End If
                 rsAmt.Open "select  dbo.TranBalance('" & Period & "','" & coyID & "') as Amount", connTran2, adOpenStatic, adLockOptimistic
             ElseIf AcctPostType = "BATCH" Then
                 rsAmt.Open "select  dbo.TranBalanceJournal('" & Period & "','" & coyID & "') as Amount", connTran2, adOpenStatic, adLockOptimistic
             End If

                    
            
            If Not rsAmt.EOF Then
                If rsAmt!Amount <> 0 Then
                    Screen.MousePointer = vbDefault
                    Err.Raise vbObjectError + 999, Err.Source, "Account Posting Failed"
                End If
            Else
                Err.Raise vbObjectError + 999, Err.Source, "Account Posting Failed"
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            
            'cmd2.CommandText = "update hExpensePayDetails set isPost=1 where VouchNo='" & vouchNo & "'"
            'cmd2.Execute
            
            
            Dim cmdAccts As New Command
            cmdAccts.ActiveConnection = connTran2
            cmdAccts.CommandType = adCmdText
            cmdAccts.CommandText = "insert into TranFromAppsTrail (TranDate,TranID,TranNoApp,Remarks) values('" & _
            sysDate & "','" & _
            TRAN_ID & "','" & _
            vouchNo & "','EXPENSE')"
            cmdAccts.Execute
        
            'cmd2.CommandText = "update hExpensePayDetails set isPost=1 where VouchNo='" & vouchNo & "'"
            'cmd2.Execute
                            
            Screen.MousePointer = vbDefault
            connTran2.CommitTrans
            
        
        
        
        End If
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    flgEdit = False
    Screen.MousePointer = vbDefault
    MsgBox "Record Succesfully Posted", vbInformation
  
    On Error GoTo errH
        
    SetButtons (True)
    Call clearFields
    Call enableFields(False)
    Call fillGrid(1)
   
    
    
'End If

Exit Sub
TransFailAccts:
    connTran2.RollbackTrans
    Screen.MousePointer = vbDefault
    MsgBox "Could not Post to Account Module!!! " & vbCrLf & vbCrLf & Err.Description
    
    Set connTran2 = Nothing
   
        'Call clearFields
        'Call enableFields(False)
        'SetButtons True
        'Call fillGrid(1)
        

Exit Sub

errTran:

Screen.MousePointer = vbDefault

Screen.MousePointer = vbDefault
connTran2.RollbackTrans
MsgBox Err.Description

Exit Sub
errH:
    Screen.MousePointer = vbDefault

MsgBox Err.Description
End Sub

Private Sub txtAmount_KeyPress(KeyAscii As Integer)

   If (KeyAscii = vbKeyReturn) Then
        txtDesc.SetFocus
        KeyAscii = 0
    End If
    
Select Case KeyAscii
Case Is < 32 ' Control keys are OK.
Case 46 ' This is a period.
    If KeyAscii = 46 Then
         If InStr(1, txtAmount.Text, ".") > 0 Then
             KeyAscii = 0
        End If
    End If
Case 48 To 57 ' This is a digit.
Case Else ' Reject any other key.
KeyAscii = 0

End Select

End Sub

Private Sub txtAmount_LostFocus()
On Error GoTo errH
If Trim(txtAmount.Text) = "" Then Exit Sub
Dim dblPr As Double

dblPr = CDbl(txtAmount.Text)
txtAmount.Text = FormatNumber(dblPr, 2)

Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub txtDesc_KeyPress(KeyAscii As Integer)

   If (KeyAscii = vbKeyReturn) Then
        DtDate.SetFocus
        KeyAscii = 0
    End If

KeyAscii = Asc(UCase(Chr(KeyAscii)))

End Sub


Public Function EnterToTab(KeyAscii As Integer)
    If KeyAscii = vbKeyReturn Then
        SendKeys "{tab}"
        KeyAscii = 0
    End If
End Function

Private Sub txtUserCode_KeyPress(KeyAscii As Integer)
'    Call EnterToTab(KeyAscii)
End Sub
