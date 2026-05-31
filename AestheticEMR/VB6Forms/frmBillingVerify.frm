VERSION 5.00
Object = "{CDE57A40-8B86-11D0-B3C6-00A0C90AEA82}#1.0#0"; "msdatgrd.ocx"
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "tabctl32.ocx"
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomct2.ocx"
Begin VB.Form frmBillingVerify 
   BackColor       =   &H00FFC0C0&
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Billing"
   ClientHeight    =   9150
   ClientLeft      =   3810
   ClientTop       =   585
   ClientWidth     =   13965
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MDIChild        =   -1  'True
   MinButton       =   0   'False
   ScaleHeight     =   9150
   ScaleWidth      =   13965
   ShowInTaskbar   =   0   'False
   Begin VB.CommandButton cmdClose 
      BackColor       =   &H00FF8080&
      Caption         =   "Close"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   465
      Left            =   12375
      TabIndex        =   185
      Top             =   8280
      Width           =   1395
   End
   Begin VB.CommandButton cmdPrint 
      BackColor       =   &H00FF8080&
      Caption         =   "Print Invoice"
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
      Height          =   465
      Left            =   10080
      TabIndex        =   97
      Top             =   8295
      Width           =   2160
   End
   Begin VB.CommandButton cmdAdd 
      BackColor       =   &H00FF8080&
      Caption         =   "Collate Bill"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   465
      Left            =   585
      TabIndex        =   2
      Top             =   8190
      Width           =   2520
   End
   Begin VB.CommandButton cmdCancel 
      BackColor       =   &H00FF8080&
      Caption         =   "Cancel"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   465
      Left            =   3240
      TabIndex        =   96
      Top             =   8190
      Width           =   2160
   End
   Begin VB.CommandButton cmdRefresh 
      BackColor       =   &H00FF8080&
      Caption         =   "Refresh"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   465
      Left            =   5535
      TabIndex        =   95
      Top             =   8190
      Width           =   2160
   End
   Begin VB.CommandButton OKButton 
      BackColor       =   &H00FF8080&
      Caption         =   "Save Collated Bill"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   465
      Left            =   585
      TabIndex        =   98
      Top             =   8190
      Width           =   2520
   End
   Begin TabDlg.SSTab SSTab1 
      Height          =   8340
      Left            =   30
      TabIndex        =   6
      Top             =   585
      Width           =   13875
      _ExtentX        =   24474
      _ExtentY        =   14711
      _Version        =   393216
      Tab             =   1
      TabHeight       =   520
      TabCaption(0)   =   "Billing"
      TabPicture(0)   =   "frmBillingVerify.frx":0000
      Tab(0).ControlEnabled=   0   'False
      Tab(0).Control(0)=   "cmdPR"
      Tab(0).Control(1)=   "chkLock"
      Tab(0).Control(2)=   "chkUnlock"
      Tab(0).Control(3)=   "cmdAudit"
      Tab(0).Control(4)=   "cmdUnBilled"
      Tab(0).Control(5)=   "cmdCoy"
      Tab(0).Control(6)=   "cmdSplit"
      Tab(0).Control(7)=   "cmdExt"
      Tab(0).Control(8)=   "Frame2"
      Tab(0).Control(9)=   "cmdPresc"
      Tab(0).Control(10)=   "cmdDispense"
      Tab(0).Control(11)=   "cboVehNo"
      Tab(0).Control(12)=   "Frame1"
      Tab(0).Control(13)=   "txtDiag"
      Tab(0).Control(14)=   "txtDuty"
      Tab(0).Control(15)=   "cmdBill"
      Tab(0).Control(16)=   "txtProf"
      Tab(0).Control(17)=   "cmdAddBill"
      Tab(0).Control(18)=   "txtApprv"
      Tab(0).Control(19)=   "cmdNHIS"
      Tab(0).Control(20)=   "txtNHIS"
      Tab(0).Control(21)=   "txtSearch"
      Tab(0).Control(22)=   "cmdOK"
      Tab(0).Control(23)=   "cmdLastDiag"
      Tab(0).Control(24)=   "Label23"
      Tab(0).Control(25)=   "lblAdmit"
      Tab(0).Control(26)=   "lblDisch"
      Tab(0).Control(27)=   "Label25"
      Tab(0).Control(28)=   "Label1(5)"
      Tab(0).Control(29)=   "lblClinic"
      Tab(0).Control(30)=   "lblBillDate"
      Tab(0).Control(31)=   "Label20"
      Tab(0).Control(32)=   "lblDate"
      Tab(0).Control(33)=   "Label1(19)"
      Tab(0).Control(34)=   "lblStatus"
      Tab(0).Control(35)=   "Label1(18)"
      Tab(0).Control(36)=   "lblPNo"
      Tab(0).Control(37)=   "Label1(17)"
      Tab(0).Control(38)=   "lblLastClinic"
      Tab(0).Control(39)=   "Label1(16)"
      Tab(0).Control(40)=   "Label21"
      Tab(0).Control(41)=   "lblDebt"
      Tab(0).Control(42)=   "Line5"
      Tab(0).Control(43)=   "lblBillPayable"
      Tab(0).Control(44)=   "Label8"
      Tab(0).Control(45)=   "lblDiscount"
      Tab(0).Control(46)=   "Label1(0)"
      Tab(0).Control(47)=   "lblCoy"
      Tab(0).Control(48)=   "Label6(1)"
      Tab(0).Control(49)=   "Label3"
      Tab(0).Control(50)=   "Label14(0)"
      Tab(0).Control(51)=   "Line1"
      Tab(0).Control(52)=   "Line2"
      Tab(0).Control(53)=   "Label1(1)"
      Tab(0).Control(54)=   "lblCat"
      Tab(0).Control(55)=   "Label2"
      Tab(0).Control(56)=   "Line3"
      Tab(0).Control(57)=   "Label1(6)"
      Tab(0).Control(58)=   "Label9(18)"
      Tab(0).Control(59)=   "Label14(1)"
      Tab(0).Control(60)=   "Label1(7)"
      Tab(0).Control(61)=   "lblDep"
      Tab(0).Control(62)=   "Label5"
      Tab(0).Control(63)=   "Label7"
      Tab(0).Control(64)=   "lblTotal"
      Tab(0).Control(65)=   "Label16"
      Tab(0).Control(66)=   "Label10"
      Tab(0).Control(67)=   "lblAmtDue"
      Tab(0).Control(68)=   "Label1(8)"
      Tab(0).Control(69)=   "Label1(9)"
      Tab(0).Control(70)=   "lblDateLast"
      Tab(0).Control(71)=   "lblLabels(2)"
      Tab(0).Control(72)=   "lblLabels(3)"
      Tab(0).ControlCount=   73
      TabCaption(1)   =   "Receipt (Payment)"
      TabPicture(1)   =   "frmBillingVerify.frx":001C
      Tab(1).ControlEnabled=   -1  'True
      Tab(1).Control(0)=   "Label1(10)"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).Control(1)=   "grdDataRct"
      Tab(1).Control(1).Enabled=   0   'False
      Tab(1).Control(2)=   "fraReceipt"
      Tab(1).Control(2).Enabled=   0   'False
      Tab(1).Control(3)=   "chkRct"
      Tab(1).Control(3).Enabled=   0   'False
      Tab(1).Control(4)=   "cmdOKRct"
      Tab(1).Control(4).Enabled=   0   'False
      Tab(1).Control(5)=   "txtSearchRct"
      Tab(1).Control(5).Enabled=   0   'False
      Tab(1).Control(6)=   "Frame3"
      Tab(1).Control(6).Enabled=   0   'False
      Tab(1).Control(7)=   "cmdPay"
      Tab(1).Control(7).Enabled=   0   'False
      Tab(1).Control(8)=   "cmdNET"
      Tab(1).Control(8).Enabled=   0   'False
      Tab(1).Control(9)=   "cmdMove"
      Tab(1).Control(9).Enabled=   0   'False
      Tab(1).ControlCount=   10
      TabCaption(2)   =   "Attendance History"
      TabPicture(2)   =   "frmBillingVerify.frx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "lblLabels(10)"
      Tab(2).Control(1)=   "lblLabels(9)"
      Tab(2).Control(2)=   "lblBalance"
      Tab(2).Control(3)=   "lblLabels(14)"
      Tab(2).Control(4)=   "lblAmountPaid"
      Tab(2).Control(5)=   "lblLabels(13)"
      Tab(2).Control(6)=   "lblTAmount"
      Tab(2).Control(7)=   "lblLabels(12)"
      Tab(2).Control(8)=   "lblLabels(11)"
      Tab(2).Control(9)=   "lblTGenCap"
      Tab(2).Control(10)=   "lblTGen"
      Tab(2).Control(11)=   "DTAttnd2"
      Tab(2).Control(12)=   "DTAttnd1"
      Tab(2).Control(13)=   "grdAttend"
      Tab(2).Control(14)=   "cmdAttend"
      Tab(2).Control(15)=   "cmdToday"
      Tab(2).Control(16)=   "cboGroup"
      Tab(2).Control(17)=   "cmdHidden"
      Tab(2).Control(18)=   "cmdUnPro"
      Tab(2).Control(19)=   "cmdTran"
      Tab(2).ControlCount=   20
      Begin VB.CommandButton cmdPR 
         BackColor       =   &H00FF8080&
         Caption         =   "Print Receipt"
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
         Height          =   465
         Left            =   -63000
         TabIndex        =   184
         Top             =   7170
         Visible         =   0   'False
         Width           =   2160
      End
      Begin VB.CheckBox chkLock 
         BackColor       =   &H000000FF&
         Caption         =   "UnLock This Bill"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   -66060
         TabIndex        =   182
         Top             =   4350
         Width           =   195
      End
      Begin VB.CommandButton cmdMove 
         BackColor       =   &H00FF8080&
         Caption         =   "Move Payment to Private Bill"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   345
         Left            =   5760
         TabIndex        =   180
         Top             =   5220
         Width           =   3270
      End
      Begin VB.CommandButton cmdNET 
         BackColor       =   &H00FF8080&
         Caption         =   "Print Net Receipt"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   345
         Left            =   9180
         TabIndex        =   176
         Top             =   5220
         Width           =   2160
      End
      Begin VB.CommandButton cmdTran 
         BackColor       =   &H00FFFF80&
         Caption         =   "Daily Tranxactions"
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
         Left            =   -69300
         TabIndex        =   170
         Top             =   360
         Width           =   2220
      End
      Begin VB.CheckBox chkUnlock 
         BackColor       =   &H000000FF&
         Caption         =   "UnLock This Bill"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   -64620
         TabIndex        =   162
         Top             =   4350
         Width           =   195
      End
      Begin VB.CommandButton cmdUnPro 
         BackColor       =   &H00FFFF80&
         Caption         =   "View Unprocessed Bills"
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
         Left            =   -64590
         TabIndex        =   158
         Top             =   330
         Width           =   3180
      End
      Begin VB.CommandButton cmdHidden 
         BackColor       =   &H00FFFF80&
         Caption         =   "View UnBilled Items"
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
         Left            =   -67020
         TabIndex        =   153
         Top             =   330
         Width           =   2220
      End
      Begin VB.ComboBox cboGroup 
         Height          =   315
         Left            =   -71310
         Style           =   2  'Dropdown List
         TabIndex        =   141
         Top             =   795
         Width           =   4245
      End
      Begin VB.CommandButton cmdToday 
         BackColor       =   &H00FFC0C0&
         Caption         =   "Attendance"
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
         Left            =   -67020
         TabIndex        =   140
         Top             =   780
         Width           =   2220
      End
      Begin VB.CommandButton cmdPay 
         BackColor       =   &H00FF8080&
         Caption         =   "Payment for Today"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   345
         Left            =   11550
         TabIndex        =   139
         Top             =   5220
         Width           =   2160
      End
      Begin VB.Frame Frame3 
         Height          =   1995
         Left            =   9060
         TabIndex        =   133
         Top             =   5520
         Visible         =   0   'False
         Width           =   3660
         Begin VB.Label lblAcctNo 
            BackStyle       =   0  'Transparent
            Caption         =   "AcctNoCash"
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
            Left            =   90
            TabIndex        =   138
            Top             =   405
            Visible         =   0   'False
            Width           =   1275
         End
         Begin VB.Label lblAcctNo 
            BackStyle       =   0  'Transparent
            Caption         =   "AcctNoPOS"
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
            Index           =   1
            Left            =   90
            TabIndex        =   137
            Top             =   765
            Visible         =   0   'False
            Width           =   1275
         End
         Begin VB.Label lblAcctNo 
            BackStyle       =   0  'Transparent
            Caption         =   "AcctNoCheque"
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
            Left            =   90
            TabIndex        =   136
            Top             =   1125
            Visible         =   0   'False
            Width           =   1275
         End
         Begin VB.Label lblAcctNo 
            BackStyle       =   0  'Transparent
            Caption         =   "AcctNoTransfer"
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
            Index           =   3
            Left            =   90
            TabIndex        =   135
            Top             =   1485
            Visible         =   0   'False
            Width           =   1365
         End
         Begin VB.Label Label12 
            Caption         =   "Do not remove below labels useful for posting"
            Height          =   195
            Left            =   45
            TabIndex        =   134
            Top             =   135
            Visible         =   0   'False
            Width           =   3300
         End
      End
      Begin VB.TextBox txtSearchRct 
         Appearance      =   0  'Flat
         Height          =   285
         Left            =   2535
         TabIndex        =   130
         Top             =   5280
         Width           =   1905
      End
      Begin VB.CommandButton cmdOKRct 
         BackColor       =   &H00FFC0C0&
         Caption         =   "OK"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   4500
         TabIndex        =   129
         Top             =   5250
         Width           =   765
      End
      Begin VB.CommandButton cmdAudit 
         BackColor       =   &H00FFC0C0&
         Caption         =   "Audit Trail"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   360
         Left            =   -63600
         TabIndex        =   128
         Top             =   3900
         Visible         =   0   'False
         Width           =   2280
      End
      Begin VB.CommandButton cmdAttend 
         BackColor       =   &H00FFC0C0&
         Caption         =   "Attendance History by Patient"
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
         Left            =   -64620
         TabIndex        =   124
         Top             =   780
         Width           =   3210
      End
      Begin VB.CommandButton cmdUnBilled 
         BackColor       =   &H00FFC0C0&
         Caption         =   "View UnBilled"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   480
         Left            =   -62130
         TabIndex        =   120
         Top             =   6720
         Visible         =   0   'False
         Width           =   1080
      End
      Begin VB.CommandButton cmdCoy 
         BackColor       =   &H00FFC0C0&
         Caption         =   "Update Company"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   480
         Left            =   -67350
         TabIndex        =   103
         Top             =   4035
         Width           =   1170
      End
      Begin VB.CommandButton cmdSplit 
         BackColor       =   &H00FFC0C0&
         Caption         =   "Split Bill"
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
         Height          =   480
         Left            =   -68655
         TabIndex        =   8
         Top             =   4050
         Width           =   1170
      End
      Begin VB.CommandButton cmdExt 
         BackColor       =   &H00FF8080&
         Caption         =   "Service Outlet"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   465
         Left            =   -67215
         TabIndex        =   99
         Top             =   7545
         Width           =   2160
      End
      Begin VB.CheckBox chkRct 
         Caption         =   "Check1"
         Height          =   195
         Left            =   315
         TabIndex        =   20
         Top             =   555
         Width           =   195
      End
      Begin VB.Frame fraReceipt 
         Caption         =   "     Tick to Receive payment"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000000&
         Height          =   4575
         Left            =   135
         TabIndex        =   82
         Top             =   555
         Width           =   13905
         Begin VB.ComboBox cboRct 
            Height          =   315
            Left            =   9270
            Sorted          =   -1  'True
            Style           =   2  'Dropdown List
            TabIndex        =   186
            Top             =   0
            Width           =   3795
         End
         Begin VB.CheckBox chkDep 
            Caption         =   "Tick to Deposit"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H00FFFFFF&
            Height          =   240
            Left            =   4170
            TabIndex        =   166
            Top             =   630
            Width           =   210
         End
         Begin VB.CheckBox chkNil 
            Caption         =   "Tick to Remove from Payment"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H00FFFFFF&
            Height          =   240
            Left            =   4170
            TabIndex        =   159
            Top             =   1290
            Width           =   180
         End
         Begin VB.CheckBox chkRefund 
            Caption         =   "Tick to Refund"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H00FFFFFF&
            Height          =   240
            Left            =   4170
            TabIndex        =   127
            Top             =   960
            Width           =   180
         End
         Begin VB.Frame fraPay 
            Height          =   1050
            Index           =   3
            Left            =   7455
            TabIndex        =   114
            Top             =   3435
            Width           =   5955
            Begin VB.CheckBox chkExact 
               Caption         =   "Tick to Enter exact amount"
               BeginProperty Font 
                  Name            =   "MS Sans Serif"
                  Size            =   8.25
                  Charset         =   0
                  Weight          =   700
                  Underline       =   0   'False
                  Italic          =   0   'False
                  Strikethrough   =   0   'False
               EndProperty
               Height          =   330
               Index           =   3
               Left            =   3465
               TabIndex        =   33
               Top             =   135
               Width           =   2265
            End
            Begin VB.TextBox txtAmt 
               Height          =   330
               Index           =   3
               Left            =   1215
               TabIndex        =   32
               Top             =   135
               Width           =   2175
            End
            Begin VB.ComboBox cboBank 
               Height          =   315
               Index           =   3
               Left            =   1170
               Sorted          =   -1  'True
               Style           =   2  'Dropdown List
               TabIndex        =   34
               Top             =   585
               Width           =   4695
            End
            Begin VB.Label lblAmt 
               Alignment       =   1  'Right Justify
               BackStyle       =   0  'Transparent
               Caption         =   "TRANSFER"
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
               Index           =   3
               Left            =   -540
               TabIndex        =   116
               Top             =   225
               Width           =   1635
            End
            Begin VB.Label Label1 
               Alignment       =   1  'Right Justify
               BackStyle       =   0  'Transparent
               Caption         =   "Bank Name"
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
               Index           =   15
               Left            =   -540
               TabIndex        =   115
               Top             =   630
               Width           =   1635
            End
         End
         Begin VB.Frame fraPay 
            Height          =   1050
            Index           =   2
            Left            =   765
            TabIndex        =   109
            Top             =   3435
            Width           =   6540
            Begin VB.ComboBox cboBank 
               Height          =   315
               Index           =   2
               Left            =   1170
               Sorted          =   -1  'True
               Style           =   2  'Dropdown List
               TabIndex        =   31
               Top             =   585
               Width           =   5280
            End
            Begin VB.CheckBox chkExact 
               Caption         =   "Tick to Enter exact amount"
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
               Index           =   2
               Left            =   3420
               TabIndex        =   30
               Top             =   225
               Width           =   2850
            End
            Begin VB.TextBox txtAmt 
               Height          =   330
               Index           =   2
               Left            =   1170
               TabIndex        =   29
               Top             =   135
               Width           =   2175
            End
            Begin VB.Label Label1 
               Alignment       =   1  'Right Justify
               BackStyle       =   0  'Transparent
               Caption         =   "Bank Name"
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
               Index           =   14
               Left            =   -540
               TabIndex        =   113
               Top             =   630
               Width           =   1635
            End
            Begin VB.Label lblAmt 
               Alignment       =   1  'Right Justify
               BackStyle       =   0  'Transparent
               Caption         =   "CHEQUE"
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
               Left            =   -585
               TabIndex        =   110
               Top             =   225
               Width           =   1635
            End
         End
         Begin VB.Frame fraPay 
            Height          =   1050
            Index           =   1
            Left            =   765
            TabIndex        =   106
            Top             =   2385
            Width           =   6540
            Begin VB.ComboBox cboBank 
               Height          =   315
               Index           =   1
               Left            =   1125
               Sorted          =   -1  'True
               Style           =   2  'Dropdown List
               TabIndex        =   28
               Top             =   540
               Width           =   5280
            End
            Begin VB.CheckBox chkExact 
               Caption         =   "Tick to Enter exact amount"
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
               Index           =   1
               Left            =   3375
               TabIndex        =   27
               Top             =   180
               Width           =   2850
            End
            Begin VB.TextBox txtAmt 
               Height          =   330
               Index           =   1
               Left            =   1125
               TabIndex        =   26
               Top             =   90
               Width           =   2175
            End
            Begin VB.Label Label1 
               Alignment       =   1  'Right Justify
               BackStyle       =   0  'Transparent
               Caption         =   "Bank Name"
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
               Index           =   12
               Left            =   -585
               TabIndex        =   108
               Top             =   585
               Width           =   1635
            End
            Begin VB.Label lblAmt 
               Alignment       =   1  'Right Justify
               BackStyle       =   0  'Transparent
               Caption         =   "POS"
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
               Index           =   1
               Left            =   -630
               TabIndex        =   107
               Top             =   180
               Width           =   1635
            End
         End
         Begin VB.Frame fraPay 
            Height          =   780
            Index           =   0
            Left            =   810
            TabIndex        =   104
            Top             =   1530
            Width           =   6540
            Begin VB.ComboBox cboBank 
               Height          =   315
               Index           =   0
               Left            =   1125
               Sorted          =   -1  'True
               Style           =   2  'Dropdown List
               TabIndex        =   111
               Top             =   630
               Visible         =   0   'False
               Width           =   5280
            End
            Begin VB.CheckBox chkExact 
               Caption         =   "Tick to Enter exact amount"
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
               Index           =   0
               Left            =   3330
               TabIndex        =   25
               Top             =   270
               Width           =   2850
            End
            Begin VB.TextBox txtAmt 
               Height          =   330
               Index           =   0
               Left            =   1125
               TabIndex        =   24
               Top             =   180
               Width           =   2175
            End
            Begin VB.Label Label1 
               Alignment       =   1  'Right Justify
               BackStyle       =   0  'Transparent
               Caption         =   "Bank Name"
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
               Index           =   13
               Left            =   -585
               TabIndex        =   112
               Top             =   675
               Visible         =   0   'False
               Width           =   1635
            End
            Begin VB.Label lblAmt 
               Alignment       =   1  'Right Justify
               BackStyle       =   0  'Transparent
               Caption         =   "CASH"
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
               Left            =   -630
               TabIndex        =   105
               Top             =   270
               Width           =   1635
            End
         End
         Begin VB.TextBox txtCheque 
            Height          =   330
            Left            =   9270
            TabIndex        =   38
            Top             =   2700
            Width           =   1590
         End
         Begin VB.CommandButton cmdRev 
            BackColor       =   &H00FFC0C0&
            Caption         =   "..."
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   330
            Left            =   13140
            TabIndex        =   84
            Top             =   900
            Width           =   360
         End
         Begin VB.CheckBox chkCurr 
            Caption         =   "Tick to Enter Fresh Bill ONLY"
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
            Left            =   12045
            TabIndex        =   22
            Top             =   495
            Visible         =   0   'False
            Width           =   3165
         End
         Begin VB.TextBox txtHt 
            Appearance      =   0  'Flat
            BackColor       =   &H8000000F&
            Height          =   330
            Left            =   1935
            Locked          =   -1  'True
            TabIndex        =   23
            Top             =   1170
            Width           =   2175
         End
         Begin VB.TextBox txtEmp 
            Appearance      =   0  'Flat
            BackColor       =   &H8000000F&
            Height          =   330
            Left            =   1935
            Locked          =   -1  'True
            TabIndex        =   83
            Top             =   765
            Width           =   2175
         End
         Begin VB.TextBox txtWord 
            Appearance      =   0  'Flat
            Height          =   735
            Left            =   9270
            Locked          =   -1  'True
            MultiLine       =   -1  'True
            TabIndex        =   36
            Top             =   1350
            Width           =   3840
         End
         Begin VB.ComboBox cboPay 
            Height          =   315
            Left            =   9270
            TabIndex        =   37
            Top             =   2205
            Width           =   3930
         End
         Begin VB.ComboBox cboPayFor 
            Height          =   315
            Left            =   9270
            Sorted          =   -1  'True
            TabIndex        =   35
            Top             =   945
            Width           =   3840
         End
         Begin MSComCtl2.DTPicker dtRctDate 
            DataField       =   "MaintDate"
            Height          =   360
            Left            =   1935
            TabIndex        =   21
            Top             =   315
            Width           =   2175
            _ExtentX        =   3836
            _ExtentY        =   635
            _Version        =   393216
            Format          =   160890881
            CurrentDate     =   38278
         End
         Begin VB.Label Label1 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Receipt Type"
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
            Index           =   20
            Left            =   7515
            TabIndex        =   187
            Top             =   45
            Width           =   1635
         End
         Begin VB.Label Label24 
            BackColor       =   &H000000FF&
            Caption         =   "Tick to Reverse Payment"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H00FFFFFF&
            Height          =   225
            Left            =   4380
            TabIndex        =   169
            Top             =   1290
            Width           =   2715
         End
         Begin VB.Label lblRefund 
            BackColor       =   &H000000FF&
            Caption         =   "Tick to Refund"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H00FFFFFF&
            Height          =   225
            Left            =   4380
            TabIndex        =   168
            Top             =   960
            Width           =   1425
         End
         Begin VB.Label Label22 
            BackColor       =   &H000000FF&
            Caption         =   "Tick to Deposit"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H00FFFFFF&
            Height          =   225
            Left            =   4380
            TabIndex        =   167
            Top             =   630
            Width           =   1425
         End
         Begin VB.Label Label17 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Receipt Date"
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
            Left            =   630
            TabIndex        =   118
            Top             =   405
            Width           =   1230
         End
         Begin VB.Label Label1 
            BackStyle       =   0  'Transparent
            Caption         =   "Cheque No"
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
            Index           =   11
            Left            =   8145
            TabIndex        =   94
            Top             =   2790
            Width           =   1005
         End
         Begin VB.Label Label11 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Amount Paid"
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
            TabIndex        =   93
            Top             =   1260
            Width           =   1635
         End
         Begin VB.Label Label13 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Amount Due"
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
            Left            =   270
            TabIndex        =   92
            Top             =   810
            Width           =   1545
         End
         Begin VB.Label Label15 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Amount Paid in Words"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   465
            Left            =   7785
            TabIndex        =   91
            Top             =   1575
            Width           =   1320
         End
         Begin VB.Label Label1 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Clinic Type"
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
            Index           =   4
            Left            =   8010
            TabIndex        =   90
            Top             =   495
            Width           =   1140
         End
         Begin VB.Label Label1 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Pay Type"
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
            Index           =   3
            Left            =   8235
            TabIndex        =   89
            Top             =   2250
            Width           =   870
         End
         Begin VB.Label Label1 
            BackStyle       =   0  'Transparent
            Caption         =   "Being Payment for"
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
            Left            =   7515
            TabIndex        =   88
            Top             =   990
            Width           =   1635
         End
         Begin VB.Label lblRCt 
            BackStyle       =   0  'Transparent
            Caption         =   "***"
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
            Left            =   5655
            TabIndex        =   87
            Top             =   330
            Width           =   1770
         End
         Begin VB.Label Label18 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Receipt No:"
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
            Left            =   4485
            TabIndex        =   86
            Top             =   330
            Width           =   1050
         End
         Begin VB.Label lblClinic2 
            BackStyle       =   0  'Transparent
            Caption         =   "***"
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
            Left            =   9270
            TabIndex        =   85
            Top             =   495
            Width           =   2715
         End
      End
      Begin VB.Frame Frame2 
         Height          =   1110
         Left            =   -74640
         TabIndex        =   48
         Top             =   945
         Width           =   9885
         Begin VB.CommandButton cmdPVT 
            BackColor       =   &H00FFC0C0&
            Caption         =   "Private ONLY"
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
            Left            =   8700
            TabIndex        =   181
            Top             =   150
            Visible         =   0   'False
            Width           =   1800
         End
         Begin VB.CommandButton cmdAdm 
            BackColor       =   &H00FFC0C0&
            Caption         =   "Admission ONLY"
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
            Left            =   8010
            TabIndex        =   117
            Top             =   630
            Width           =   1800
         End
         Begin VB.CommandButton cmdList 
            BackColor       =   &H00FFC0C0&
            Caption         =   "Get Patient"
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
            Left            =   6750
            TabIndex        =   51
            Top             =   630
            Width           =   1170
         End
         Begin VB.CheckBox chkAll 
            Caption         =   "Check1"
            Height          =   195
            Left            =   6480
            TabIndex        =   50
            Top             =   315
            Width           =   195
         End
         Begin VB.ComboBox cboHMO 
            Height          =   315
            Left            =   2025
            Style           =   2  'Dropdown List
            TabIndex        =   49
            Top             =   675
            Width           =   4695
         End
         Begin MSComCtl2.DTPicker DTPicker1 
            DataField       =   "MaintDate"
            Height          =   360
            Left            =   2025
            TabIndex        =   52
            Top             =   180
            Width           =   1860
            _ExtentX        =   3281
            _ExtentY        =   635
            _Version        =   393216
            Format          =   160497665
            CurrentDate     =   38278
         End
         Begin MSComCtl2.DTPicker DTPicker2 
            DataField       =   "MaintDate"
            Height          =   360
            Left            =   4545
            TabIndex        =   53
            Top             =   180
            Width           =   1860
            _ExtentX        =   3281
            _ExtentY        =   635
            _Version        =   393216
            Format          =   160497665
            CurrentDate     =   38278
         End
         Begin VB.Label lblLabels 
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
            Height          =   255
            Index           =   0
            Left            =   4005
            TabIndex        =   57
            Top             =   270
            Width           =   510
         End
         Begin VB.Label lblLabels 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Attendance Between"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   1
            Left            =   135
            TabIndex        =   56
            Top             =   225
            Width           =   1860
         End
         Begin VB.Label Label19 
            BackColor       =   &H00FFC0C0&
            BackStyle       =   0  'Transparent
            Caption         =   "Show All"
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
            Left            =   6720
            TabIndex        =   55
            Top             =   315
            Width           =   780
         End
         Begin VB.Label lblLabels 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Company Name"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   255
            Index           =   4
            Left            =   450
            TabIndex        =   54
            Top             =   720
            Width           =   1545
         End
      End
      Begin VB.CommandButton cmdPresc 
         BackColor       =   &H00FFC0C0&
         Caption         =   "View Prescription / Lab"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   465
         Left            =   -69375
         TabIndex        =   47
         Top             =   4185
         Visible         =   0   'False
         Width           =   2565
      End
      Begin VB.CommandButton cmdDispense 
         BackColor       =   &H00FFC0C0&
         Caption         =   "View Treatment Plan"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   465
         Left            =   -70815
         TabIndex        =   46
         Top             =   4065
         Visible         =   0   'False
         Width           =   1395
      End
      Begin VB.ComboBox cboVehNo 
         Height          =   315
         Left            =   -72930
         Sorted          =   -1  'True
         Style           =   2  'Dropdown List
         TabIndex        =   45
         Top             =   2130
         Width           =   6315
      End
      Begin VB.Frame Frame1 
         Caption         =   "Bill Analysis"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000000&
         Height          =   2895
         Left            =   -74775
         TabIndex        =   17
         Top             =   4575
         Width           =   13560
         Begin VB.CommandButton cmdAcct 
            BackColor       =   &H00FFC0C0&
            Caption         =   "Tranx / Debt Info"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   9.75
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   750
            Left            =   12375
            TabIndex        =   102
            Top             =   -135
            Width           =   1110
         End
         Begin VB.CommandButton cmdCap 
            BackColor       =   &H00FFC0C0&
            Caption         =   "Capitate All"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   9.75
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   480
            Left            =   11250
            TabIndex        =   101
            Top             =   135
            Width           =   1080
         End
         Begin VB.CommandButton cmdShift 
            BackColor       =   &H00FFC0C0&
            Caption         =   "Shift Bill"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   9.75
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   480
            Left            =   10125
            TabIndex        =   100
            Top             =   135
            Width           =   1080
         End
         Begin VB.CommandButton cmdPrice 
            BackColor       =   &H00FFC0C0&
            Caption         =   "Adjust Bill"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   9.75
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            Height          =   345
            Left            =   90
            TabIndex        =   18
            Top             =   225
            Width           =   1305
         End
         Begin MSDataGridLib.DataGrid grdData 
            Height          =   2175
            Left            =   270
            TabIndex        =   19
            Top             =   630
            Width           =   13200
            _ExtentX        =   23283
            _ExtentY        =   3836
            _Version        =   393216
            AllowUpdate     =   0   'False
            HeadLines       =   1
            RowHeight       =   20
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
         Begin VB.Label lblPaid 
            BackStyle       =   0  'Transparent
            DataField       =   "0"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H000000FF&
            Height          =   195
            Left            =   8730
            TabIndex        =   44
            Top             =   315
            Width           =   1275
         End
         Begin VB.Label Label9 
            BackStyle       =   0  'Transparent
            Caption         =   "Amount Paid"
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
            Index           =   7
            Left            =   7515
            TabIndex        =   43
            Top             =   315
            Width           =   1140
         End
         Begin VB.Label lblBilled 
            BackStyle       =   0  'Transparent
            DataField       =   "0"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H000000FF&
            Height          =   195
            Left            =   6120
            TabIndex        =   42
            Top             =   315
            Width           =   1275
         End
         Begin VB.Label Label9 
            BackStyle       =   0  'Transparent
            Caption         =   "Amount Billed"
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
            Index           =   5
            Left            =   4770
            TabIndex        =   41
            Top             =   315
            Width           =   1230
         End
         Begin VB.Label Label9 
            BackStyle       =   0  'Transparent
            Caption         =   "Amount Generated"
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
            Index           =   3
            Left            =   1440
            TabIndex        =   40
            Top             =   315
            Width           =   1680
         End
         Begin VB.Label lblGen 
            BackStyle       =   0  'Transparent
            DataField       =   "0"
            BeginProperty Font 
               Name            =   "MS Sans Serif"
               Size            =   8.25
               Charset         =   0
               Weight          =   700
               Underline       =   0   'False
               Italic          =   0   'False
               Strikethrough   =   0   'False
            EndProperty
            ForeColor       =   &H000000FF&
            Height          =   195
            Left            =   3240
            TabIndex        =   39
            Top             =   315
            Width           =   1455
         End
      End
      Begin VB.TextBox txtDiag 
         Height          =   690
         Left            =   -66135
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   16
         Top             =   3255
         Width           =   2580
      End
      Begin VB.TextBox txtDuty 
         Height          =   645
         Left            =   -63480
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   15
         Top             =   3255
         Width           =   2220
      End
      Begin VB.CommandButton cmdBill 
         BackColor       =   &H00FFC0C0&
         Caption         =   "Recalculate Bill"
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
         Height          =   345
         Left            =   -68250
         TabIndex        =   14
         Top             =   3075
         Width           =   2070
      End
      Begin VB.TextBox txtProf 
         Height          =   330
         Left            =   -68925
         TabIndex        =   13
         Top             =   3075
         Width           =   645
      End
      Begin VB.CommandButton cmdAddBill 
         BackColor       =   &H00FFC0C0&
         Caption         =   "Add to Bill"
         Enabled         =   0   'False
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   -66555
         TabIndex        =   12
         Top             =   2130
         Width           =   1260
      End
      Begin VB.TextBox txtApprv 
         Height          =   480
         Left            =   -72975
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   11
         Top             =   3075
         Width           =   2535
      End
      Begin VB.CommandButton cmdNHIS 
         BackColor       =   &H00FFC0C0&
         Caption         =   "Recalculate Bill"
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
         Height          =   300
         Left            =   -68250
         TabIndex        =   10
         Top             =   3525
         Width           =   2070
      End
      Begin VB.TextBox txtNHIS 
         Height          =   330
         Left            =   -68925
         TabIndex        =   9
         Text            =   "10"
         Top             =   3480
         Width           =   645
      End
      Begin VB.TextBox txtSearch 
         Appearance      =   0  'Flat
         Height          =   285
         Left            =   -72660
         TabIndex        =   0
         Top             =   420
         Width           =   1680
      End
      Begin VB.CommandButton cmdOK 
         BackColor       =   &H00FFC0C0&
         Caption         =   "OK"
         Default         =   -1  'True
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   -70950
         TabIndex        =   1
         Top             =   390
         Width           =   540
      End
      Begin VB.CommandButton cmdLastDiag 
         BackColor       =   &H00FFC0C0&
         Caption         =   "Edit && Save"
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
         Left            =   -65100
         TabIndex        =   7
         Top             =   3000
         Width           =   1575
      End
      Begin MSDataGridLib.DataGrid grdAttend 
         Height          =   5625
         Left            =   -74910
         TabIndex        =   123
         Top             =   1260
         Width           =   13515
         _ExtentX        =   23839
         _ExtentY        =   9922
         _Version        =   393216
         AllowUpdate     =   0   'False
         HeadLines       =   1
         RowHeight       =   15
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
         Caption         =   "Attendance History"
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
      Begin MSDataGridLib.DataGrid grdDataRct 
         Height          =   1875
         Left            =   120
         TabIndex        =   131
         Top             =   5640
         Width           =   13620
         _ExtentX        =   24024
         _ExtentY        =   3307
         _Version        =   393216
         AllowUpdate     =   0   'False
         HeadLines       =   1
         RowHeight       =   20
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
      Begin MSComCtl2.DTPicker DTAttnd1 
         DataField       =   "MaintDate"
         Height          =   360
         Left            =   -74895
         TabIndex        =   142
         Top             =   780
         Width           =   1470
         _ExtentX        =   2593
         _ExtentY        =   635
         _Version        =   393216
         Format          =   160563201
         CurrentDate     =   38278
      End
      Begin MSComCtl2.DTPicker DTAttnd2 
         DataField       =   "MaintDate"
         Height          =   360
         Left            =   -72870
         TabIndex        =   143
         Top             =   780
         Width           =   1440
         _ExtentX        =   2540
         _ExtentY        =   635
         _Version        =   393216
         Format          =   160563201
         CurrentDate     =   38278
      End
      Begin VB.Label Label23 
         BackColor       =   &H000000FF&
         Caption         =   "Lock Bill"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   -65850
         TabIndex        =   183
         Top             =   4350
         Width           =   1095
      End
      Begin VB.Label lblAdmit 
         BackColor       =   &H000000FF&
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   300
         Left            =   -72960
         TabIndex        =   179
         Top             =   2670
         Width           =   1950
      End
      Begin VB.Label lblDisch 
         BackColor       =   &H000000FF&
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   300
         Left            =   -68940
         TabIndex        =   178
         Top             =   2670
         Width           =   1950
      End
      Begin VB.Label Label25 
         BackStyle       =   0  'Transparent
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FF0000&
         Height          =   240
         Left            =   -71400
         TabIndex        =   177
         Top             =   2970
         Width           =   2055
      End
      Begin VB.Label Label1 
         BackStyle       =   0  'Transparent
         Caption         =   "Clinic:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   240
         Index           =   5
         Left            =   -67470
         TabIndex        =   175
         Top             =   330
         Width           =   660
      End
      Begin VB.Label lblClinic 
         BackStyle       =   0  'Transparent
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FF0000&
         Height          =   240
         Left            =   -66780
         TabIndex        =   174
         Top             =   330
         Width           =   2055
      End
      Begin VB.Label lblBillDate 
         BackStyle       =   0  'Transparent
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FF0000&
         Height          =   210
         Left            =   -72930
         TabIndex        =   173
         Top             =   3660
         Width           =   1215
      End
      Begin VB.Label lblTGen 
         BackColor       =   &H000000FF&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   -73485
         TabIndex        =   172
         Top             =   7080
         Visible         =   0   'False
         Width           =   1710
      End
      Begin VB.Label lblTGenCap 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "Total Generated"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   -74940
         TabIndex        =   171
         Top             =   7080
         Visible         =   0   'False
         Width           =   1395
      End
      Begin VB.Label Label20 
         BackColor       =   &H000000FF&
         Caption         =   "UnLock Bill"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Left            =   -64410
         TabIndex        =   165
         Top             =   4350
         Width           =   1095
      End
      Begin VB.Label lblDate 
         BackStyle       =   0  'Transparent
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FF0000&
         Height          =   240
         Left            =   -68655
         TabIndex        =   164
         Top             =   345
         Width           =   1095
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Attnd Date:"
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
         Index           =   19
         Left            =   -69780
         TabIndex        =   163
         Top             =   360
         Width           =   1050
      End
      Begin VB.Label lblStatus 
         BackColor       =   &H000000FF&
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   -73860
         TabIndex        =   161
         Top             =   4290
         Width           =   4995
      End
      Begin VB.Label Label1 
         BackColor       =   &H000000FF&
         Caption         =   "Bill Status:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Index           =   18
         Left            =   -74820
         TabIndex        =   160
         Top             =   4290
         Width           =   930
      End
      Begin VB.Label lblPNo 
         BackStyle       =   0  'Transparent
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FF0000&
         Height          =   195
         Left            =   -66900
         TabIndex        =   157
         Top             =   660
         Width           =   1575
      End
      Begin VB.Label Label1 
         BackStyle       =   0  'Transparent
         Caption         =   "PNo:"
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
         Index           =   17
         Left            =   -67470
         TabIndex        =   156
         Top             =   690
         Width           =   420
      End
      Begin VB.Label lblLastClinic 
         BackStyle       =   0  'Transparent
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FF0000&
         Height          =   240
         Left            =   -63390
         TabIndex        =   155
         Top             =   330
         Width           =   2055
      End
      Begin VB.Label Label1 
         BackStyle       =   0  'Transparent
         Caption         =   "Last Clinic:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   240
         Index           =   16
         Left            =   -64650
         TabIndex        =   154
         Top             =   330
         Width           =   1140
      End
      Begin VB.Label lblLabels 
         BackStyle       =   0  'Transparent
         Caption         =   "Company Name"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   11
         Left            =   -71310
         TabIndex        =   152
         Top             =   540
         Width           =   1545
      End
      Begin VB.Label lblLabels 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "Total Amount Billed"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Index           =   12
         Left            =   -71730
         TabIndex        =   151
         Top             =   7080
         Width           =   1725
      End
      Begin VB.Label lblTAmount 
         BackColor       =   &H000000FF&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   -69915
         TabIndex        =   150
         Top             =   7080
         Width           =   1710
      End
      Begin VB.Label lblLabels 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "Total Amount Paid"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Index           =   13
         Left            =   -68160
         TabIndex        =   149
         Top             =   7080
         Width           =   1725
      End
      Begin VB.Label lblAmountPaid 
         BackColor       =   &H000000FF&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   -66345
         TabIndex        =   148
         Top             =   7080
         Width           =   1710
      End
      Begin VB.Label lblLabels 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "Total Balance"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Index           =   14
         Left            =   -64560
         TabIndex        =   147
         Top             =   7080
         Width           =   1215
      End
      Begin VB.Label lblBalance 
         BackColor       =   &H000000FF&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   -63285
         TabIndex        =   146
         Top             =   7080
         Width           =   1920
      End
      Begin VB.Label lblLabels 
         BackStyle       =   0  'Transparent
         Caption         =   "Date Between"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   255
         Index           =   9
         Left            =   -74940
         TabIndex        =   145
         Top             =   525
         Width           =   1815
      End
      Begin VB.Label lblLabels 
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
         Height          =   255
         Index           =   10
         Left            =   -73350
         TabIndex        =   144
         Top             =   825
         Width           =   450
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Search by Receipt No"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000000&
         Height          =   285
         Index           =   10
         Left            =   75
         TabIndex        =   132
         Top             =   5295
         Width           =   2340
      End
      Begin VB.Label Label21 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Debt:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   240
         Left            =   -65100
         TabIndex        =   126
         Top             =   1095
         Width           =   1725
      End
      Begin VB.Label lblDebt 
         Alignment       =   1  'Right Justify
         BackColor       =   &H00000000&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   300
         Left            =   -63315
         TabIndex        =   125
         Top             =   1020
         Width           =   1950
      End
      Begin VB.Line Line5 
         X1              =   -64695
         X2              =   -59745
         Y1              =   1710
         Y2              =   1710
      End
      Begin VB.Label lblBillPayable 
         Alignment       =   1  'Right Justify
         BackColor       =   &H0000C000&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   300
         Left            =   -63315
         TabIndex        =   122
         Top             =   1740
         Width           =   1950
      End
      Begin VB.Label Label8 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Total Bill:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000000&
         Height          =   240
         Left            =   -64875
         TabIndex        =   121
         Top             =   1785
         Width           =   1515
      End
      Begin VB.Label lblDiscount 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   300
         Left            =   -63315
         TabIndex        =   119
         Top             =   1365
         Width           =   1950
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Patient Name"
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
         Index           =   0
         Left            =   -74640
         TabIndex        =   81
         Top             =   2175
         Width           =   1635
      End
      Begin VB.Label lblCoy 
         BackColor       =   &H000000FF&
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   -73845
         TabIndex        =   80
         Top             =   3975
         Width           =   4995
      End
      Begin VB.Label Label6 
         BackColor       =   &H000000FF&
         Caption         =   "Company:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Index           =   1
         Left            =   -74820
         TabIndex        =   79
         Top             =   3975
         Width           =   945
      End
      Begin VB.Label Label3 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Purpose/ Diagnosis"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   465
         Left            =   -75000
         TabIndex        =   78
         Top             =   9330
         Visible         =   0   'False
         Width           =   870
      End
      Begin VB.Label Label14 
         BackStyle       =   0  'Transparent
         Caption         =   "Amount Billed in Word"
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
         Left            =   -63390
         TabIndex        =   77
         Top             =   3030
         Width           =   2040
      End
      Begin VB.Line Line1 
         X1              =   -65190
         X2              =   -60960
         Y1              =   2880
         Y2              =   2880
      End
      Begin VB.Line Line2 
         X1              =   -65190
         X2              =   -60960
         Y1              =   2070
         Y2              =   2070
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Billing Cat:"
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
         Index           =   1
         Left            =   -65835
         TabIndex        =   76
         Top             =   4050
         Width           =   960
      End
      Begin VB.Label lblCat 
         BackStyle       =   0  'Transparent
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FF0000&
         Height          =   240
         Left            =   -64770
         TabIndex        =   75
         Top             =   4035
         Width           =   1005
      End
      Begin VB.Label Label2 
         BackStyle       =   0  'Transparent
         Caption         =   "Bill Date"
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
         Left            =   -73860
         TabIndex        =   74
         Top             =   3675
         Width           =   780
      End
      Begin VB.Line Line3 
         X1              =   -65190
         X2              =   -60960
         Y1              =   2460
         Y2              =   2460
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Add Prof Fee to Bill in (%)"
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
         Index           =   6
         Left            =   -70185
         TabIndex        =   73
         Top             =   3075
         Width           =   1140
      End
      Begin VB.Label Label9 
         BackStyle       =   0  'Transparent
         Caption         =   "Approval Code"
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
         Index           =   18
         Left            =   -74325
         TabIndex        =   72
         Top             =   3180
         Width           =   1275
      End
      Begin VB.Label Label14 
         BackStyle       =   0  'Transparent
         Caption         =   "Diagnosis"
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
         Index           =   1
         Left            =   -66045
         TabIndex        =   71
         Top             =   3030
         Width           =   1185
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Payable fee  (%)"
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
         Index           =   7
         Left            =   -70500
         TabIndex        =   70
         Top             =   3570
         Width           =   1455
      End
      Begin VB.Label lblDep 
         Alignment       =   1  'Right Justify
         BackColor       =   &H00FF0000&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   300
         Left            =   -63330
         TabIndex        =   69
         Top             =   2115
         Width           =   1950
      End
      Begin VB.Label Label5 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Amount Paid:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000000&
         Height          =   240
         Left            =   -65190
         TabIndex        =   68
         Top             =   2145
         Width           =   1815
      End
      Begin VB.Label Label7 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Discount:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   240
         Left            =   -65100
         TabIndex        =   67
         Top             =   1440
         Width           =   1725
      End
      Begin VB.Label lblTotal 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   300
         Left            =   -63315
         TabIndex        =   66
         Top             =   690
         Width           =   1950
      End
      Begin VB.Label Label16 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Amount Billed:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000000&
         Height          =   240
         Left            =   -65370
         TabIndex        =   65
         Top             =   750
         Width           =   1995
      End
      Begin VB.Label Label10 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Balance:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000000&
         Height          =   240
         Left            =   -65160
         TabIndex        =   64
         Top             =   2595
         Width           =   1785
      End
      Begin VB.Label lblAmtDue 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "0"
         BeginProperty Font 
            Name            =   "Microsoft Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   300
         Left            =   -63330
         TabIndex        =   63
         Top             =   2520
         Width           =   1950
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Search by Rct/Bill No"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00000000&
         Height          =   285
         Index           =   8
         Left            =   -74910
         TabIndex        =   62
         Top             =   465
         Width           =   2265
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Last Attnd Date:"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   240
         Index           =   9
         Left            =   -70350
         TabIndex        =   61
         Top             =   675
         Width           =   1650
      End
      Begin VB.Label lblDateLast 
         BackStyle       =   0  'Transparent
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FF0000&
         Height          =   240
         Left            =   -68670
         TabIndex        =   60
         Top             =   675
         Width           =   1095
      End
      Begin VB.Label lblLabels 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "Admission Date"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Index           =   2
         Left            =   -74730
         TabIndex        =   59
         Top             =   2700
         Width           =   1635
      End
      Begin VB.Label lblLabels 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "Discharge Date"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         ForeColor       =   &H00FFFFFF&
         Height          =   285
         Index           =   3
         Left            =   -70695
         TabIndex        =   58
         Top             =   2685
         Width           =   1635
      End
   End
   Begin VB.Label lblBill 
      BackStyle       =   0  'Transparent
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   11295
      TabIndex        =   5
      Top             =   315
      Width           =   2460
   End
   Begin VB.Label Label4 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Bill No:"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00FFFFFF&
      Height          =   240
      Left            =   10305
      TabIndex        =   4
      Top             =   315
      Width           =   870
   End
   Begin VB.Label lblScreen 
      Alignment       =   2  'Center
      BackColor       =   &H00404000&
      Caption         =   "Generate Bill"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H0000FFFF&
      Height          =   555
      Left            =   0
      TabIndex        =   3
      Top             =   0
      Width           =   13965
   End
End
Attribute VB_Name = "frmBillingVerify"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'''''
Option Explicit
Dim rsExP As Recordset
Dim iDNo As Long
Dim dblVal As Double
Dim strPatient As String
Dim newRow As Integer
Dim I As Integer
Dim intRec As Integer
Dim strX As String
'dim strCon As String
Dim strName As String
'dim dblbf As Double
Dim dblProf As Double
Dim dblTotal As Double
Dim dblSub As Double
Dim dblSubTotal As Double

Dim strpCatID As String
Dim strBillAdm As String
Dim strCoy As String

Dim AccountNo_Recv As String
Dim dblMed As Double
Dim strParam As String
Dim flgEdit As Boolean
Dim StrClientCatX  As String
Const cnstVal As String = "ADMISSION"
Dim gtBill As Double
Dim AmountPaid As Double
Dim AmountDue As Double
Dim dblPay As Double
Dim strRct As String

'Dim strEmpID As String
Dim strClinic As String
Dim strCode As String
Dim strRctCode As String
Dim strClinicID As String
'Dim strClinicName As String
Dim blnSave As Boolean
Dim strBillTo As String
Dim strCoyID As String
Dim strDiag As String

Dim dblBills As Double
Dim prevDebt As Double
Private isOnAdmit As Boolean
Private isDisch As Boolean

Dim strWord As String
Dim BillEndDate As Integer
Dim BillDate As Date
Dim AttndDate As Date
Dim LastAttndDate As Date

Dim BillCoy As String

Dim isSaved As Boolean
Dim strConIDVal As Long

Dim strBCode As String
Dim strBCodePOS As String
Dim strBCodeCHQ As String
Dim strBCodeTRF As String

Dim isAdmission As Boolean
Dim AttndCoy As String

Dim isFromAttendGrid As Boolean
Dim isFromHidden As Boolean
Dim strCompany As String
Dim EnrolleNo As String
Dim PolicyType As String
Dim isNormalPay As Boolean
Dim BillTo As String
Private dtAttndDate As Date
Private flgAllowPay As Boolean 'to allow payment for pat yet to be disch 'does not affect OPD pat
Private entryTime As Date
Private dtBillDate As Date

Dim isProcess As Boolean
Dim isLock  As Boolean

Dim intRctNum As Integer
Dim isReversePay As Boolean

Private flg_Enforce_Saving As Boolean
Private vouchNo As String
Dim ClinicID As String
Dim RctDateFroRpt As String 'Date
'Dim OrigPreviewStr As String
Dim flgPrivate As Boolean

'Dim isProcess As Boolean
'Dim isLock As Boolean

Private Sub cboCat_Click()
'  Dim rsBL As New Recordset
'  With rsBL
'  .Open "select drgname from drugs where drgcatName='" & cboCat.Text & "'", conSTR, adOpenForwardOnly, adLockReadOnly
'cboDrug.Clear
'If Not .EOF Then
'.MoveFirst
'Do While Not .EOF
'cboDrug.AddItem !drgname
'.MoveNext
'Loop
'End If
'End With
'Set rsBL = Nothing
'
End Sub


Private Sub cboClient_Click()
'Dim rsBLV As New Recordset
'  With rsBLV
'  .Open "select distinct clientID,clientName from Clients where clientID='" & cboClient.Text & "'", conSTR, adOpenForwardOnly, adLockReadOnly
'If Not .EOF Then
'lblCoy.Caption = rsBLV!clientName
'End If
'End With
'Set rsBLV = Nothing

End Sub

Private Sub cboDrug_Click()
  
'  Dim rsBL As New Recordset
'  With rsBL
'  .Open "select drgname,qtyPerUnit,remarks,Price from qryDrugPrice where drgName='" & cboDrug.Text & "'", conSTR, adOpenForwardOnly, adLockReadOnly
'If Not .EOF Then
'lblPrice.Caption = !Price & ""
'lblUnit.Caption = !qtyPerUnit & ""
'Else
'MsgBox "No Price or Package Info for this drug"
'lblPrice.Caption = ""
'lblUnit.Caption = ""
'End If
'End With
'Set rsBL = Nothing

End Sub

Private Sub cboReceived_Click()
'If cboReceived.Text = "" Then Exit Sub
''MsgBox "Please enter Name of Nurse"
''Call clearFields
''Exit Sub
''End If
'strEmpID = Mid(cboReceived.Text, InStr(cboReceived.Text, "[") + 1, Len(cboReceived.Text) - (InStr(cboReceived.Text, "[") + 1))

End Sub

Private Sub cboBank_Click(Index As Integer)
On Error GoTo errH
If cboBank(Index).ListIndex = -1 Or cboBank(Index).ListIndex = 0 Then Exit Sub

'strBCodePOS = ""
'strBCodeCHQ = ""
'strBCodeTRF = ""

strBCode = cboBank(Index).Text  'Mid(cboBank(Index).Text, InStr(cboBank(Index).Text, "[") + 1, Len(cboBank(Index).Text) - (InStr(cboBank(Index).Text, "[") + 1))

If Index = 1 Then
    strBCodePOS = Mid(strBCode, InStr(strBCode, "[") + 1, Len(strBCode) - (InStr(strBCode, "[") + 1))
ElseIf Index = 2 Then
    strBCodeCHQ = Mid(strBCode, InStr(strBCode, "[") + 1, Len(strBCode) - (InStr(strBCode, "[") + 1))
ElseIf Index = 3 Then
    strBCodeTRF = Mid(strBCode, InStr(strBCode, "[") + 1, Len(strBCode) - (InStr(strBCode, "[") + 1))
End If

Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cboGroup_Click()
 On Error GoTo errH
If cboGroup.ListIndex = 0 Or cboGroup.ListIndex = -1 Then Exit Sub
AttndCoy = Mid(cboGroup.Text, InStr(cboGroup.Text, "[") + 1, Len(cboGroup.Text) - (InStr(cboGroup.Text, "[") + 1))
'cboCust.Text   'Mid(cboCust.Text, InStr(cboCust.Text, "[") + 1, Len(cboCust.Text) - (InStr(cboCust.Text, "[") + 1))
Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub cboHMO_Click()
 If cboHMO.ListIndex = 0 Or cboHMO.ListIndex = -1 Then Exit Sub
BillCoy = Mid(cboHMO.Text, InStr(cboHMO.Text, "[") + 1, Len(cboHMO.Text) - (InStr(cboHMO.Text, "[") + 1))
'cboCust.Text   'Mid(cboCust.Text, InStr(cboCust.Text, "[") + 1, Len(cboCust.Text) - (InStr(cboCust.Text, "[") + 1))

End Sub

Private Sub cboPay_Click()
    strPayType = cboPay.Text
    'call loadBanks 'rem for now 'not nece
End Sub

Private Sub cboPayFor_Click()
    strPaymentFor = Trim(cboPayFor.Text)
End Sub

Public Sub cboVehNo_Click()
On Error GoTo errH
'isLockBill = False

'Call clearFields


If cboVehNo.ListIndex = -1 Or cboVehNo.ListIndex = 0 Then
    Call clearFields
    Exit Sub
End If

'''''''''''''''''
If Enforce_Saving_In_Collate_Bill = "YES" And flg_Enforce_Saving = True Then
    If lblBill.Caption <> "" Then
        If CDbl(lblAmtDue.Caption) <> 0 Then
            Call OKButton_Click ' to save bill auto
            'flg_Enforce_Saving = False 'already in OKButton_Click
            cmdAdd_Click
            cboVehNo.SetFocus
            Exit Sub
        End If
    End If
End If


'txtSearch.SetFocus ' to prev scrolling to anothr patient

Screen.MousePointer = vbHourglass


blnSave = False

cmdAddBill.Enabled = True
cmdNHIS.Enabled = True

Dim isInj As Boolean
'Call genIDNo
strX = Mid(cboVehNo.Text, InStr(cboVehNo.Text, "[") + 1, Len(cboVehNo.Text) - (InStr(cboVehNo.Text, "[")))

strCon = Mid(strX, 1, Len(strX) - Len(Mid(strX, InStr(strX, "#") - 1)))
lblBill.Caption = strCon
strConRecall = strCon 'to recall last strCon 'cannot be cleared

strPatient = Mid(strX, InStr(strX, "#") + 1)
'gIntIDx = cboVehNo.ItemData(cboVehNo.ListIndex)
lblPNo.Caption = strPatient

strName = Mid(cboVehNo.Text, 1, InStr(cboVehNo.Text, "@") - 2)
gName = strName
strBillAdm = ""
strCoy = ""
dblMed = 0

gStrCon = strCon
gStrPatient = strPatient 'pno
gPatNo = strPatient

    'Call updateRevType
    
    Call getConFromAdmission '' whether patient is on Admission
    
    'If isOnAdmit = True Then Exit Sub
    
ClinicID = ""
gBillDate = vbEmpty
dtAttndDate = vbEmpty
Dim rsBLV As New Recordset
  With rsBLV
      .Open "select ClinicType,BillDate,pCatid,recdate,retainCode,AcctID,consultID,clientcat,referal,retainID,RetainName,BillEndDate,policyType,empNo from vwhRecords where consultid ='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
            If Not .EOF Then
                
                dtAttndDate = !recDate & ""
                gAttndDate = dtAttndDate
                lblDate.Caption = dtAttndDate
                
                ClinicID = rsBLV!ClinicType & ""
                lblClinic.Caption = ClinicID
                
                dtBillDate = !BillDate & ""
                lblBillDate.Caption = dtBillDate
                gBillDate = dtBillDate
               
                
                StrClientCatX = !clientCat & ""
                lblCat.Caption = StrClientCatX
                
                
                AccountNo_Recv = rsBLV!AcctID & ""
                strCoy = rsBLV!retainID & ""
                gStrCoy = strCoy
                strBillTo = !retainCode & ""
                BillTo = strBillTo
                'gBillTo = strBillTo 'from Appdefault
                strClientID = strCoy
                
                strCompany = rsBLV!RetainName & ""
                lblCoy.Caption = strCompany
                
                strpCatID = !pCatid & ""
                EnrolleNo = !empNo & ""
                PolicyType = !PolicyType & ""
                
    
            BillEndDate = 31 'defult ok  ''31 is max value for the mth
            BillEndDate = IIf(IsNull(!BillEndDate), 31, !BillEndDate) '31 is max value for the mth
            
            Dim rsDate As New Recordset 'check for Attnd dates
            Dim D As Integer
            With rsDate
               .Open "Select ConsultID,recdate,ClinicType from hRecords where  pno='" & strPatient & "' order by recID desc", conStr, adOpenStatic, adLockOptimistic
                If Not .EOF Then
                   .MoveFirst
                    Do While Not .EOF
                        If !consultID <> strCon Then
                            .MoveNext
                        Else
                            .MoveNext
                            If Not .EOF Then
                                lblDateLast.Caption = Format(!recDate, "Short Date") & ""
                                lblLastClinic.Caption = !ClinicType & ""
                                Exit Do
                            Else
                                lblDateLast.Caption = ""
                                lblLastClinic.Caption = ""
                                Exit Do
                            End If
                        End If
                    Loop
                Else
                    lblDateLast.Caption = ""
                    lblLastClinic.Caption = ""
                End If
            End With
              ''''''''''''''''''''''''''''
    '    frmBillPat.Hide
    '    frmBillPat.Show vbModal
        
        ''''''''''''''''''''''''''''''''
    
        dblTotal = 0
        dblSub = 0
        'dblbf = 0
        gtBill = 0
    
    
        '    isInj = isPatOnInj 'to det if pat is for inj for cashier to collate. unser observation
        '    If isInj = True Then
        '        MsgBox "Patient is for Injection Or for Dressing !!! Please enter His/Her Injection/Dressing Information"
        '        frmDispensing.Show
        '        Exit Sub
        '    End If
          
        'nece ' have checked if already updated, but no problem
        'Dim rsLatest As New Recordset
        'rsLatest.Open "select latestBillno from hpatients  where   pno = '" & strPatient & "'", conStr, adOpenForwardOnly, adLockReadOnly
        'If Not rsLatest.EOF Then
        '    If IsNull(rsLatest!latestBillNo) Then
        '        Dim cmdUpd As New Command
        '        cmdUpd.ActiveConnection = conStr
        '        cmdUpd.CommandText = "update hpatients  set latestBillno='" & strCon & "' where pno = '" & strPatient & "'"
        '        cmdUpd.Execute
        '    End If
        '
        ' End If
        
            Call preProcessing
            'Call getDeposit
            'Call getConFromAdmission '' whether patient is on Admission
            'Call prevPay2 'already done by UpdatePay in utility module
            
            'Call UpdateAmountPaid(strCon) 'now in getAccumBill
            
            'Call getDebtForBill(strPatient)
            'Call UpdatePay(Me, strCon)
        Call getAccumBill
        
        Call getDiagnosis
        Call getApprvCode
        
        If is_Private_Patient = True Then
            SSTab1.Tab = 0
            lblScreen.Caption = "Prepare Receipt for " & strName & vbNewLine & "=N=" & lblAmtDue.Caption
        Else
            SSTab1.Tab = 0
            lblScreen.Caption = "Generated Bill for " & strName & vbNewLine & "=N=" & lblAmtDue.Caption
        End If
        
        strpNO = cboVehNo.Text '''glbal var to pass to rptBilling Form
Else
    cboVehNo.ListIndex = -1
    dtBillDate = vbEmpty
    lblBillDate.Caption = ""
    
    dtAttndDate = vbEmpty
    gAttndDate = vbEmpty
    lblDate.Caption = ""
    lblClinic.Caption = ""
    ClinicID = ""
    strBillTo = ""
    strCompany = ""
    AccountNo_Recv = ""
    strCoy = ""
    lblCoy.Caption = ""
    lblCat.Caption = ""
    
    If is_Private_Patient = True Then
        lblScreen.Caption = "Generate Cash Receipt"
    Else
        lblScreen.Caption = "Generate Bill"
    End If
    
    MsgBox "Patient does not Exist"
    Exit Sub
End If
End With
Set rsBLV = Nothing

'Screen.MousePointer = vbHourglass
''''''''Call getComments for billing
Dim Comments As String
Comments = getComments(strApp, strCon, 3)
If Comments <> "" Then
    MsgBox Comments, vbInformation, "General Comments"
End If



If InStrRev(strCon, "B") > 0 Or InStrRev(strCon, "C") > 0 _
Or InStrRev(strCon, "D") > 0 Or InStr(strCon, "/") > 0 Then   '"/"  for ext pat
    'do nothing 'split bill
Else
    If strCoy <> strPrivate Then
        Dim rsSplit As New Recordset 'check for Attnd dates
        With rsSplit
            Screen.MousePointer = vbHourglass
            If .State = adStateOpen Then .Close
           .Open "Select ConsultID from hRecords where  substring(ConsultID,1,12)='" & strCon & "' order by recID desc", conStr, adOpenStatic, adLockOptimistic
            If Not .EOF Then
               .MoveFirst
                Do While Not .EOF 'strCon with B,C or D cannot be in this loop 'filtered out by inStrRev
                    If Len(!consultID) = 13 Then 'split bill for this Pat
                        Dim strConSplitBill As String
                        strConSplitBill = !consultID & ""
                        If .State = adStateOpen Then .Close
                        .Open "select BillNo from payments where billNo='" & strConSplitBill & "'", conStr, adOpenForwardOnly, adLockReadOnly
                        If Not .EOF Then
                            MsgBox "This Patient has a Split Bill AND has Paid for it", vbInformation
                            Exit Do
                        Else
                            MsgBox "This Patient has a Split Bill BUT NO Payment yet", vbCritical
                            Exit Do
                        End If
                    Else
                        .MoveNext
                    End If
                Loop
            End If
        End With
    End If

End If


 flg_Enforce_Saving = True
 
''''''####''''''''''''''''''''''''''''''''''det if frame is enabled
If (strCoy = strPrivate Or StrClientCatX = ClientCatPrivate) Then ''''And strApp = "BILLING" Then '"PRIVATE" Or strCoy = "0001" Then
    flgPrivate = True
    chkRct.Value = vbChecked
    fraReceipt.Enabled = True
    dtRctDate.Value = sysDate
Else
    flgPrivate = False
    chkRct.Value = False
    fraReceipt.Enabled = False
End If

If chkRct.Value = vbChecked Then
    'fraReceipt.Enabled = True
    
    'Call genIDNo
    'txtEmp.Text = FormatNumber(dblPay, 2)
    'cboPayFor.Text = "CONSULTATION AND DRUGS"
    'cboClinic.Text = "OUT-PATIENT"
    'cboPay.Text = "CASH"
    

Else '''If chkRct.Value = False Then
    'fraReceipt.Enabled = False
    txtHt.Text = "0"
    'txtEmp.Text = ""
    'cboReceived.ListIndex = 0
    cboPayFor.Text = ""
    'cboClinic.Text = ""
    cboPay.ListIndex = -1
    txtWord.Text = ""
    lblRCt.Caption = ""
    'dtRctDate.Value = ""

End If

cmdPrint.Enabled = True

 
' ''''''''####''''Also in txtEmp_change and okButton_click proc'''''''''''''
'If (strCoy = strPrivate or StrClientCatX =ClientCatPrivate)  Then ''''''And strApp = "BILLING" Then '"PRIVATE" Or strCoy = "0001" Then
'    chkRct.Value = vbChecked
'    fraReceipt.Enabled = True
'    '' rem cos of billItem edit & delete will not reflct
'    '    If Print_From_Small_Printer = "YES" Then
'    '        If CDbl(txtEmp.Text) > 0 Then
'    '            'enter default values
'    '            chkExact(0).Value = vbChecked 'POS_PayType_Default_Cash = "YES"
'    '            cboPayFor.Text = "Medical Services"
'    '            'txtHt.Text = FormatNumber(CDbl(txtEmp.Text), 2)
'    '            'cboPay.Text = "CASH"
'    '        End If
'    '
'    '    End If
'    '
'    'Else
'    '    chkRct.Value = False
'    '    fraReceipt.Enabled = False
'End If

 
 ''''''''''''''''''''''''''''


Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
End Sub


Private Sub updateRevType()
On Error GoTo errH


Dim Param2 As ADODB.Parameter
Dim Cmd As New ADODB.Command
Dim cnn As New ADODB.Connection



cnn.ConnectionString = conStr
cnn.Open
Cmd.ActiveConnection = cnn
Cmd.CommandType = adCmdStoredProc
Cmd.CommandText = "updateRevType"

Cmd.Parameters.Append Cmd.CreateParameter("@ConsultID", adVarChar, adParamInput, 50, strCon)
Cmd.Execute




Exit Sub
errH:
MsgBox Err.Description

End Sub

Public Sub getDeposit()
'On Error GoTo errH
'dblAmtDep = 0
'
'
'  Dim rsBLvX As New Recordset
'  With rsBLvX
'  .Open "select billno,amountpaid from vwhPaymentsSummDeposit where billno='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
'If Not .EOF Then
'    dblAmtDep = IIf(IsNull(!amountpaid), 0, !amountpaid)
'    'lblDep.Caption = FormatNumber(dblAmtDep, 2)
'Else
'    dblAmtDep = 0
'End If
'End With
'Set rsBLvX = Nothing
'
'
'Exit Sub
'errH:
'MsgBox Err.Description

End Sub


Public Sub getConFromAdmission()

isDisch = False
isOnAdmit = False

  Dim rsBLb As New Recordset
  With rsBLb
  .Open "select * from vwAdmissionAndDischargeInfo  where consultID='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
                '''table and not query is used here so one can be sure to locate conId if avail. qryhAdmission does not show discharged patient
    If Not .EOF Then
        strBillAdm = cnstVal
        isOnAdmit = True
        
        lblAdmit.Caption = !admdate  'ok here
        'dtAdmit.Enabled = False
        
        If IsNull(!dischDate) Then
            lblDisch.Caption = ""
            'dtDisch.Enabled = True
            isDisch = False
            MsgBox "This Patient is on Admission!!!" '''' Discharge Patient before Collating Bill"
        Else
            isDisch = True
            lblDisch.Caption = !dischDate
            'dtDisch.Enabled = False
            MsgBox "This Patient has been Discharged!!! Please Generate Bill"
        End If
    Else
        lblAdmit.Caption = ""
        lblDisch.Caption = ""
        
        'Dim rsBLb2 As New Recordset
        'With rsBLb2
        '    .Open "select AdmDate,DischDate from Billing  where billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
        '                  '''table and not query is used here so one can be sure to locate conId if avail. qryhAdmission does not show discharged patient
        '    If Not .EOF Then
        '
        '        If Not IsNull(!admdate) Then
        '            dtAdmit.Value = !admdate 'ok here
        '            dtDisch.Value = !dischDate
        '        Else
        '
        '            dtAdmit.Value = sysDate
        '            dtDisch.Value = sysDate
        '
        '            dtAdmit.Value = Null
        '            dtDisch.Value = Null
        '
        '            dtAdmit.Enabled = True
        '            dtDisch.Enabled = True
        '        End If
        '    Else
        '
        '            dtAdmit.Value = sysDate
        '            dtDisch.Value = sysDate
        '
        '            dtAdmit.Value = Null
        '            dtDisch.Value = Null
        '
        '            dtAdmit.Enabled = True
        '            dtDisch.Enabled = True
        '    End If
        'End With
    End If
End With
Set rsBLb = Nothing


End Sub


Private Sub cmdReset_Click()
'Call clearFields
'
'OrderGrid.Rows = 2
'newRow = 1
'    OrderGrid.TextMatrix(newRow, 0) = ""
'    OrderGrid.TextMatrix(newRow, 1) = ""
'    OrderGrid.TextMatrix(newRow, 2) = ""
'    OrderGrid.TextMatrix(newRow, 3) = ""
'    OrderGrid.TextMatrix(newRow, 4) = ""
'    OrderGrid.TextMatrix(newRow, 5) = ""
'    OrderGrid.TextMatrix(newRow, 6) = ""
End Sub

Private Sub Command1_Click()



''''check against duplicate values
'For i = 1 To OrderGrid.Rows - 2
' If OrderGrid.TextMatrix(i, 3) = cboDrug.Text Then
'MsgBox "Duplicate Drug Entry not allowed"
'Exit Sub
'End If
'Next i
''''''''''''''''''

'If strPatient = "" Then
'    MsgBox "Please specify a Patient File No "
'    Exit Sub
'End If
'
'If cboVehNo.Text = "" Then
'    MsgBox "Please specify Drug/Service used "
'    Exit Sub
'End If
'
'
'If txtQty = "" Then
'    MsgBox "Please specify Quantity of Drug/Service Used"
'    Exit Sub
'End If
'
'''''''''''''''''''''''''''''
'newRow = OrderGrid.Rows - 1
'OrderGrid.TextMatrix(newRow, 0) = sysdate
'OrderGrid.TextMatrix(newRow, 1) = lblBill.Caption
'OrderGrid.TextMatrix(newRow, 2) = strPatient
'OrderGrid.TextMatrix(newRow, 3) = cboDrug.Text
'OrderGrid.TextMatrix(newRow, 4) = lblPrice.Caption
'OrderGrid.TextMatrix(newRow, 5) = CDbl(txtQty.Text)
'OrderGrid.TextMatrix(newRow, 6) = lblSub.Caption
'
'OrderGrid.Rows = OrderGrid.Rows + 1


'lblSub.Caption = 0
'lblUnit.Caption = ""
'lblPrice.Caption = ""
'txtQty.Text = ""
'cboDrug.Text = ""
''cboCat.Text = ""
'
'dblVal = dblVal + Val(lblSub.Caption)
'lblTotal.Caption = CStr(dblVal)

End Sub


Private Sub chkCurr_Click()
'On Error Resume Next
'txtHt.Text = ""
'If chkCurr.Value = vbChecked Then
'    If IsNumeric(lblSub.Caption) Then
'        Dim Bal As Double
'        Bal = CDbl(lblSub.Caption)
'        txtEmp.Text = FormatNumber(Bal, 2)
'    End If
'Else
'    txtEmp.Text = lblAmtDue.Caption
'End If

End Sub

Private Sub chkDep_Click()
On Error GoTo errH

isNormalPay = True 'ok as init 'will be alterd in the procedure

If chkDep.Value = vbChecked Then
    flgAllowPay = True
    
    isNormalPay = True
    cboPayFor.Text = "DEPOSIT"
    chkNil.Value = False
    chkRefund.Value = False
Else
    flgAllowPay = False
    cboPayFor.Text = ""
    
    isNormalPay = False
    chkRefund.Value = False
    chkDep.Value = False
    chkNil.Value = False
End If

Exit Sub

errH:
MsgBox Err.Description
End Sub



Private Sub chkExact_Click(Index As Integer)
On Error Resume Next
txtAmt(Index).Text = ""
If chkExact(Index).Value = vbChecked Then
    If IsNumeric(txtEmp.Text) Then
        Dim Bal As Double
        Bal = CDbl(txtEmp.Text)
        txtAmt(Index).Text = FormatNumber(Bal, 2)
    End If
Else
    txtAmt(Index).Text = 0
End If
Call txtAmt_Change(Index)
End Sub

Private Sub chkLock_Click()
On Error GoTo errH

If chkLock.Value = vbChecked Then


    If cboVehNo.Text = "" Then
        MsgBox "No Patient is selected"
        chkLock.Value = False
        Exit Sub
    End If
    
    

    'verify if already processed'''''''''''''''''''''''''''''''''''''''
    Dim rsVerX As New Recordset
    rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
    If Not rsVerX.EOF Then 'ie its processed
        chkLock.Value = False
        MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
        Exit Sub
    End If
    
    rsVerX.Close 'Locked Bill
    rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
    If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
        chkLock.Value = False
        MsgBox "This Bill is already Locked! requires Management Permission to Unlock"
        Screen.MousePointer = vbDefault
        Exit Sub
    End If
    
    Dim intOK As Integer
    intOK = MsgBox("Are you sure to lock this Bill", vbYesNo, "Unlock Bill")
    If intOK = vbNo Then
        chkLock.Value = False
        Exit Sub
    End If

    

        Screen.MousePointer = vbHourglass
        chkUnlock.Value = False
  
    
    
    Dim Cmd As New ADODB.Command
    Dim strBNum As String
    Cmd.ActiveConnection = conStr
    Cmd.CommandType = adCmdText
  
    Dim rsVer As New Recordset
    'Dim PNo As String
    Dim subAmt As Double
    
    rsVer.Open "select  sum(Subtotal) as Subtotal from BillingDetails where billno='" & strCon & "'", conStr, adOpenStatic, adLockOptimistic
    If Not rsVer.EOF Then
        subAmt = rsVer!SubTotal
                            
        Cmd.CommandText = "update  billing set isSigned=1, AmountSigned= " & subAmt & " where billno = '" & strCon & "'"
        Cmd.Execute
        
        Call Auditrail(m_Username, "Bill Locked", strCon, "Amount: " & subAmt, strHostName)
             
        Call getBillStatus
        chkLock.Value = False
        Screen.MousePointer = vbDefault
        MsgBox "This Bill is hereby Locked"

    Else
        subAmt = 0 'very impt
    
    End If

End If


Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub chkNil_Click()
On Error GoTo errH

isNormalPay = True 'ok as init 'will be alterd in the procedure

If chkNil.Value = vbChecked Then
 
    ''verify if already processed'''''''''''''''''''''''''''''''''''''''
    'Dim rsVerX As New Recordset
    'rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
    'If Not rsVerX.EOF Then 'ie its processed
    '    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    '    Screen.MousePointer = vbDefault
    '    chkNil.Value = False
    '    Exit Sub
    'End If
    
    Dim rsBLV As New Recordset
    With rsBLV
      .Open "select username from vwusers where loginrole = 'MANAGEMENT' and username='" & m_Username & "'", conStr, adOpenForwardOnly, adLockReadOnly
      If .EOF Then 'not mgt user
          MsgBox "This Bill requires Management Permission to Remove from Payment"
          Screen.MousePointer = vbDefault
          chkNil.Value = False
          
            flgAllowPay = False
            chkRefund.Value = False
            chkDep.Value = False
          Exit Sub
      End If
    End With
    
    
    

 
 

    If lblDep.Caption <= 0 Or lblDep.Caption = "" Then
        MsgBox "Invald! No Amount has been Paid", vbCritical
          Screen.MousePointer = vbDefault
          chkNil.Value = False
          
          
        flgAllowPay = False
        chkRefund.Value = False
        chkDep.Value = False
        Exit Sub
    End If
    
    
    
    
    frmReceiptReverse.Hide
    frmReceiptReverse.Show vbModal
    
    'amtReverse = InputBox("Enter Amount to Remove", "Amount to Remove", 0)
    
   
    If amtReverse > CDbl(lblDep.Caption) Then
        MsgBox "Amount to Reverse cannot be greater than AmountPaid"
          Screen.MousePointer = vbDefault
          chkNil.Value = False
          
        flgAllowPay = False
        chkRefund.Value = False
        chkDep.Value = False
        Exit Sub
    End If
    
   If amtReverse <= 0 Then
        MsgBox "Amount to Reverse cannot be Less or equal to zero"
          Screen.MousePointer = vbDefault
          chkNil.Value = False
          
        flgAllowPay = False
        chkRefund.Value = False
        chkDep.Value = False
        Exit Sub
    End If
    
    'If amtReverse < 0 Then amtReverse = -(amtReverse) 'no neg from inputBox
    
    isNormalPay = False
    
    txtAmt(0).Locked = True
    txtAmt(1).Locked = True
    txtAmt(2).Locked = True
    txtAmt(3).Locked = True
    
    
    txtAmt(0).Text = 0
    txtAmt(1).Text = 0
    txtAmt(2).Text = 0
    txtAmt(3).Text = 0
    
    
    
    amtReverse = -(amtReverse) ' has to be -ve
    txtHt.Text = amtReverse '-ve
    
    If AcctPostOn = True Then
    
        ''' code here to get paym types values
        Dim AcctNoX As String
        Dim AmtPaid As Double
        Dim rsTypeX As New Recordset
        With rsTypeX
            .Open "Select ReceiptNo,AmountPaid,PayType,AccountNo from vwPaymentTypesNoReversal where ReceiptNo='" & strReceiptNo & "' order by SNo", conStr, adOpenStatic, adLockOptimistic
            If Not .EOF Then
                .MoveFirst
                Do While Not .EOF
                    
                    AmtPaid = -(!AmountPaid) ' very nece here
                    AcctNoX = !AccountNo & ""
                    
                    Select Case !PayType & ""
                    Case "CASH"
                        txtAmt(0).Text = AmtPaid
                         'AcctNoCash = AcctNoX
                         'cboBank(0).Text = getBank(AcctNoX) ' none for cash
                    Case "POS"
                        txtAmt(1).Text = AmtPaid
                        'strBCodePOS = AcctNoX
                         cboBank(1).Text = getBank(AcctNoX)
                    Case "CHEQUE"
                        txtAmt(2).Text = AmtPaid
                        'strBCodeCHQ = AcctNoX
                         cboBank(2).Text = getBank(AcctNoX)
                    Case "TRANSFER"
                        txtAmt(3).Text = AmtPaid
                        'strBCodeTRF = AcctNoX
                         cboBank(3).Text = getBank(AcctNoX)
                    End Select
                    
                    .MoveNext ' -ve
                Loop
            End If
        End With
    
    Else
        txtAmt(0).Text = amtReverse
    End If
    
    
    
    
    isReversePay = True
    flgAllowPay = True
    chkRefund.Value = False
    chkDep.Value = False
    
    
    
    cboPayFor.Text = "Reverse Payment for (" & strReceiptNo & ")"
    cboPay.Text = "CASH"
    
    Call Auditrail(m_Username, "About to Reverse payment for " & strName, strCon, "Amount: " & Abs(amtReverse), strHostName)

Else
    
    isReversePay = False
    flgAllowPay = False
    chkRefund.Value = False
    chkDep.Value = False
    chkNil.Value = False
    
    isNormalPay = True
    
    txtAmt(0).Locked = False
    txtAmt(1).Locked = False
    txtAmt(2).Locked = False
    txtAmt(3).Locked = False
    
    txtHt.Text = 0
    cboPayFor.Text = ""
    cboPay.Text = ""
    
    txtAmt(0).Locked = False
    txtAmt(0).Text = 0
    txtAmt(1).Text = 0
    txtAmt(2).Text = 0
    txtAmt(3).Text = 0
End If

Exit Sub
errH:
MsgBox Err.Description


End Sub


Private Function getBank(AcctNo As String) As String

    Dim rsB As New Recordset
    With rsB
        .Open "select distinct AccountName,AccountNo from vwAccountsInfo where AccountNo='" & AcctNo & "'", conStrAccts, adOpenForwardOnly, adLockReadOnly
        If Not .EOF Then
            getBank = !AccountName & " [" & !AccountNo & "]"
        End If
    End With

End Function


Private Sub chkRefund_Click()
On Error GoTo errH

isNormalPay = True 'ok as init 'will be alterd in the procedure

Dim AmtRefund As Double
If chkRefund.Value = vbChecked Then
    
    ''verify if already processed'''''''''''''''''''''''''''''''''''''''
    'Dim rsVerX As New Recordset
    'rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
    'If Not rsVerX.EOF Then 'ie its processed
    '    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    '      Screen.MousePointer = vbDefault
    '      chkRefund.Value = False
    '    Exit Sub
    'End If
    
    Dim rsBLV As New Recordset
    With rsBLV
      .Open "select username from vwusers where loginrole = 'MANAGEMENT' and username='" & m_Username & "'", conStr, adOpenForwardOnly, adLockReadOnly
      If .EOF Then 'not mgt user
          MsgBox "This Bill requires Management Permission to Refund Balance payment"
          Screen.MousePointer = vbDefault
          chkRefund.Value = False
            flgAllowPay = False
            chkNil.Value = False
            chkDep.Value = False
          Exit Sub
      End If
    End With

 
    If txtEmp.Text = 0 Or txtEmp.Text = "" Then
        MsgBox "There is no Refund to make! Amount Due (Balance) must be negative to show Patient is been owed"
          Screen.MousePointer = vbDefault
          chkRefund.Value = False
            flgAllowPay = False
            chkNil.Value = False
            chkDep.Value = False
        Exit Sub
    End If
    

    If lblDep.Caption <= 0 Or lblDep.Caption = "" Then
        MsgBox "Invald! No Amount to Refund", vbCritical
          Screen.MousePointer = vbDefault
          chkRefund.Value = False
            flgAllowPay = False
            chkNil.Value = False
            chkDep.Value = False
        Exit Sub
    End If
    
 
    
    AmtRefund = txtEmp.Text
    txtHt.Text = AmtRefund '-ve
    
    
    If AmtRefund >= 0 Then
        MsgBox "There is no Refund to make! Amount Due (Balance) must be negative to show Patient is been owed"
          Screen.MousePointer = vbDefault
          chkRefund.Value = False
            flgAllowPay = False
            chkNil.Value = False
            chkDep.Value = False
        Exit Sub
    End If
    
    flgAllowPay = True
    isNormalPay = False
    txtAmt(0).Locked = True
    txtAmt(0).Text = AmtRefund
    txtAmt(1).Text = 0
    txtAmt(2).Text = 0
    txtAmt(3).Text = 0
    
    cboPayFor.Text = "REFUND"
    cboPay.Text = "CASH"
    
    Call Auditrail(m_Username, "About to Refund Amount to " & strName, strCon, "Amount: " & Abs(AmtRefund), strHostName)

Else
    
    flgAllowPay = False
    chkNil.Value = False
    chkDep.Value = False
    
    isNormalPay = True
    txtAmt(0).Locked = False
    txtHt.Text = "0"
    cboPayFor.Text = ""
    cboPay.Text = ""
    
    chkRefund.Value = False
    txtAmt(0).Text = 0
    txtAmt(1).Text = 0
    txtAmt(2).Text = 0
    txtAmt(3).Text = 0
End If
Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub chkUnlock_Click()
On Error GoTo errH
If chkUnlock.Value = vbChecked Then
    
    If cboVehNo.Text = "" Then
        MsgBox "No Patient is selected"
        chkUnlock.Value = False
        Exit Sub
    End If
    
    
    Dim intOK As Integer
    intOK = MsgBox("Are you sure to Unlock this Bill", vbYesNo, "Unlock Bill")
    If intOK = vbNo Then
        chkUnlock.Value = False
        Exit Sub
    End If

Screen.MousePointer = vbHourglass

'verify if already processed'''''''''''''''''''''''''''''''''''''''
    Dim rsVerX As New Recordset
    rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
    If Not rsVerX.EOF Then 'ie its processed
        MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
        Screen.MousePointer = vbDefault
        chkUnlock.Value = False
        Exit Sub
    End If
    
    rsVerX.Close 'Locked Bill
    rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
    If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
          Dim rsBLV As New Recordset
          With rsBLV
            .Open "select username from vwusers where loginrole = 'MANAGEMENT' and username='" & m_Username & "'", conStr, adOpenForwardOnly, adLockReadOnly
            If .EOF Then 'not mgt user
                MsgBox "This Bill is Locked! requires Management Permission to Unlock"
                Screen.MousePointer = vbDefault
                chkUnlock.Value = False
                Exit Sub
            Else
                'mgt user 'go ahead
                Dim Cmd As New ADODB.Command
                Dim strBNum As String
                Cmd.ActiveConnection = conStr
                Cmd.CommandType = adCmdText
              
                Dim rsVer As New Recordset
                Dim PNo As String
                Dim subAmt As Double
                
                rsVer.Open "select  AmountBilled from billing where isSigned=1 and billno='" & strCon & "'", conStr, adOpenStatic, adLockOptimistic
                If Not rsVer.EOF Then
                    subAmt = rsVer!AmountBilled
                    'PNo = rsVer!PNo & ""
                                        
                    Cmd.CommandText = "update  billing set isSigned=0, AmountSigned= " & subAmt & " where billno = '" & strCon & "'"
                    Cmd.Execute
                    
                    Call Auditrail(m_Username, "Bill Unlocked by Mgt for" & strName, strCon, "Amount: " & subAmt, strHostName)
                    
                    Call getBillStatus
                    
                    MsgBox "This Bill is now UnLocked (Open)"
                    chkUnlock.Value = False
                Else
                    subAmt = 0 'very impt
                    'cmd.CommandText = "update  billing set isSigned=0, AmountSigned= 0 where billno = '" & strCon & "'"
                    'cmd.Execute
                    MsgBox "This Bill is Open"
                    chkUnlock.Value = False
                End If


            End If
          End With
    Else
        MsgBox "This Bill is Open (Not Locked)"
        chkUnlock.Value = False
        Screen.MousePointer = vbDefault
    End If
        

End If


Screen.MousePointer = vbDefault


Exit Sub
errH:
Screen.MousePointer = vbDefault

MsgBox Err.Description
            
End Sub

Private Sub cmdAcct_Click()
On Error GoTo errH
    If cboVehNo.Text = "" Then
        debtFlg = "SEARCH"
        frmDebtInfo.Label5.Caption = "Search patients Tranx History"
        'MsgBox "No Patient is selected"
        'Exit Sub
    Else
        debtFlg = "TRANX"
        frmDebtInfo.Label5.Caption = gName & " (Tranx YTD)"
    End If
    
frmDebtInfo.Hide
frmDebtInfo.Show vbModal
'If IsDate(CDate(lblBillDate.Caption)) Then
'    frmDebtInfo.DTPicker1.Value = CDate(lblBillDate.Caption)
'End If

Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cmdAddBill_Click()
On Error GoTo errH
    
    If cboVehNo.Text = "" Then
    MsgBox "Specify name of Patient"
    cboVehNo.SetFocus
    Exit Sub
    End If
    
    
    
    Call getConFromAdmission '' whether patient is on Admission
    
    'If isOnAdmit = True Then Exit Sub
    
    
    frmBillPat.Hide
    frmBillPat.Show vbModal

    If isCloseBillPage = True Then
        isCloseBillPage = False
        Unload frmBillPat
        
        If InStr(1, strCon, "/") > 0 Then '''walkIn pat
            Screen.MousePointer = vbHourglass
            frmInvestigatePublic.Hide
            frmInvestigatePublic.Show
            frmInvestigatePublic.txtSearch.Text = strCon
            Call frmInvestigatePublic.cmdOK_Click
            Screen.MousePointer = vbDefault
        Else
            Screen.MousePointer = vbHourglass
            frmInvestigateJKH.Hide
            frmInvestigateJKH.Show
            Dim dtDateLab As Date
            dtDateLab = CDate(lblDate.Caption)
            frmInvestigateJKH.DTPicker1.Value = dtDateLab
            frmInvestigateJKH.DTPicker2.Value = dtDateLab
            frmInvestigateJKH.cmdNew_Click
            frmInvestigateJKH.chkAll.Value = vbChecked
            Call frmInvestigateJKH.cmdList_Click
            'If lblClinic.Caption <> "(IN-PATIENT)" Then
            'End If
            
            Screen.MousePointer = vbDefault
        End If
        
        Unload Me
        Exit Sub
    End If
    
    
    Call getAccumBill
    
    
    If is_Private_Patient = True Then
        SSTab1.Tab = 0
        lblScreen.Caption = "Prepare Cash Receipt for " & strName & vbNewLine & "=N=" & lblAmtDue.Caption
    Else
        SSTab1.Tab = 0
        lblScreen.Caption = "Generate / Verify Bill for " & strName & vbNewLine & "=N=" & lblAmtDue.Caption
    End If
    
    strpNO = cboVehNo.Text '''glbal var to pass to rptBilling Form
    


Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cmdAdm_Click()
On Error GoTo errH
Screen.MousePointer = vbHourglass
isAdmission = True
Call getPatForBill
Screen.MousePointer = vbDefault
Exit Sub
errH:
Screen.MousePointer = vbDefault

MsgBox Err.Description

End Sub

Private Sub cmdAttend_Click()
On Error GoTo errH

If strPatient = "" Then
    MsgBox "No Patient! Please select a Patient"
    Exit Sub
End If

'optHide = 0
'isFromAttendGrid = False
Screen.MousePointer = vbHourglass


lblTGenCap.Visible = False
lblTGen.Visible = False

lblTAmount.Caption = 0
lblAmountPaid.Caption = 0
lblBalance.Caption = 0


Dim rsVal As New Recordset
Set grdAttend.DataSource = Nothing
grdAttend.Caption = "Attendance History"
With rsVal
    Dim ssQL As String
    .CursorLocation = adUseClient
    ssQL = "select ROW_NUMBER() OVER (ORDER BY RecID desc) AS SNo,recDate as Date,htime as Time,FullName,ClinicType as Clinic,ConsultID as BillNo,AmountBilled,DebtBF as PrevDebt,Discount,AmountPaid,RetainName as Company,Remarks as Purpose,PhoneNo from vwhRecordsAndBill where pno='" & strPatient & "'"
    .Open ssQL, conStr, adOpenStatic, adLockOptimistic
    'MsgBox ssQL
    If Not .EOF Then
        Set grdAttend.DataSource = Nothing
        Set grdAttend.DataSource = rsVal
        grdAttend.Columns("SNo").Width = 400
        grdAttend.Columns("Date").Width = 1000
        grdAttend.Columns("Time").Width = 1000

        
        grdAttend.Columns("AmountBilled").NumberFormat = "#,###.00"
        grdAttend.Columns("AmountBilled").Alignment = dbgRight
        
        grdAttend.Columns("AmountPaid").NumberFormat = "#,###.00"
        grdAttend.Columns("AmountPaid").Alignment = dbgRight
       
        grdAttend.Columns("Discount").NumberFormat = "#,###.00"
        grdAttend.Columns("Discount").Alignment = dbgRight
        
        grdAttend.Columns("PrevDebt").NumberFormat = "#,###.00"
        grdAttend.Columns("PrevDebt").Alignment = dbgRight
        
        
        grdAttend.Columns("PrevDebt").Width = 1000
        grdAttend.Columns("Discount").Width = 1000
        
        grdAttend.Columns("AmountPaid").Width = 1200
        grdAttend.Columns("AmountBilled").Width = 1200
        
 
        
        
    Else
        Set grdAttend.DataSource = Nothing
    End If
End With
Set rsVal = Nothing
Screen.MousePointer = vbDefault


Exit Sub
errH:
Screen.MousePointer = vbDefault

MsgBox Err.Description
End Sub

Private Sub cmdAudit_Click()
On Error GoTo errH
    If cboVehNo.Text = "" Then
        MsgBox "No Patient is selected"
        Exit Sub
    End If
    
Screen.MousePointer = vbHourglass
    VerifyFlg = "VERIFY"
    FrmRptAudit.txtSearch.Text = strCon
    Call FrmRptAudit.cmdOK_Click
    FrmRptAudit.Show

Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub cmdBill_Click()
On Error GoTo errH
If cboVehNo.Text = "" Then
    MsgBox "Specify Patient Name"
    cboVehNo.SetFocus
    Exit Sub
End If


'verify if already processed'''''''''''''''''''''''''''''''''''''''
Dim rsVerX As New Recordset
rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its processed
    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    Exit Sub
End If

rsVerX.Close 'Locked Bill
rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
    MsgBox "This Bill is Locked! requires Management Permission to Unlock"
    Exit Sub
End If



If txtProf = "" Or Not IsNumeric(txtProf.Text) Then
    MsgBox "Specify Prof fee! Numeric Only"
    txtProf.SetFocus
    Exit Sub
End If

'
'If Trim(strNHIS) = "" Then
'    MsgBox "No Description or title for this fee! Please Specify"
'    Exit Sub
'End If



Dim dblPrf As Double
Dim proFee As Double
Dim Debt2 As Double
Dim rsDetails As New Recordset

'Debt2 = CDbl(lblBF.Caption)
'ProFee = CDbl(lblPay.Caption) - Debt2 'or lblcurr.caption

proFee = CDbl(lblTotal.Caption) '- Debt2 'or lbltotal.caption
dblPrf = (proFee * CDbl(txtProf.Text)) / 100

'Dim dblSubX As Double
'dblSubX = CDbl(txtPrice.Text) * CDbl(txtQty.Text)
If dblPrf < 0 Then
    MsgBox "Prof Fee Cannot be less than Zero"
    Exit Sub
End If



''''''''''''Add prof fee to billAccum'''''''''''''''''''''
'dblPrf = (CDbl(lblPay.Caption) * CDbl(txtProf.Text)) / 100
Dim strProf As String
strProf = "Prof Fee (" & txtProf.Text & "%)"
rsDetails.Open "select * from BillAccum where 1=2 ", conStr, adOpenStatic, adLockOptimistic
     rsDetails.AddNew
     rsDetails!DtDate = sysDate
     rsDetails!consultID = strCon
     rsDetails!drgName = strProf
     rsDetails!price = dblPrf
     rsDetails!Qty = 1
     rsDetails!SubTotal = dblPrf
     rsDetails!billType = "SERVICE"
     rsDetails!conID = 0
     rsDetails!Usage = ""
     rsDetails!Capitated = "NO"
     rsDetails!isbilled = 0
     rsDetails!attendedto = 1
     rsDetails!suppres = 0
     rsDetails!Category = "CONSULTATION"
     rsDetails!PNo = strPatient
     rsDetails!coyName = strCoy
     rsDetails!BillTo = strBillTo
     rsDetails!revType = RevType_Prof_Fee
     rsDetails!AppVersion = App.Major
     rsDetails.Update
    
    Call Auditrail(m_Username, "insert Prof Fee (" & txtProf.Text & "%) for: " & strName, strCon, rsDetails!SubTotal, strHostName)
    'dblProf = CDbl(txtProf.Text)
    
            isbillAdjust = True
            adjustTo = ""
            adjustTo = strProf
    
    Call getAccumBill

MsgBox txtProf.Text & "% Prof fee of " & FormatNumber(proFee, 2) & " Added" & vbNewLine & "Prof Fee is based on Current Bill" & vbNewLine & "Prof Fee Charged is " & FormatNumber(dblPrf, 2)


Exit Sub
errH:
MsgBox Err.Description

End Sub


Private Sub cboVehNo_GotFocus()

'Call getPatForBill
'
End Sub

Private Sub chkBF_Click()
'If chkBF.Value = vbChecked Then
'        dblPay = (dblSub - dblAmtDep)
'        lblPay.Caption = FormatNumber(dblPay, 2) 'payable bill
'
'ElseIf chkBF.Value = False Then
'
'        dblPay = (dblSub + dblBF - dblAmtDep)
'        lblPay.Caption = FormatNumber(dblPay, 2) 'payable bill
'
'End If
End Sub

Private Sub chkRct_Click()
On Error GoTo errH
If blnSave = True Then Exit Sub

If cboVehNo.Text = "" And cboVehNo.Enabled = True Then
    MsgBox "Specify Name of Patient"
    cboVehNo.SetFocus
    chkRct.Value = False
    Exit Sub
End If

If chkRct.Value = vbChecked Then

    If (strCoy = strPrivate Or StrClientCatX = ClientCatPrivate) Then
        ' go ahead
    Else
        If lblCat.Caption = "NHIS" And Split_NHIS_Bill_For_Payment = "NO" Then
            ' go ahead
        Else
            MsgBox "Payment can only be Accepted on PRIVATE Bills Only, Split the Bill"
            chkRct.Value = False
            Exit Sub
        End If
    End If


    fraReceipt.Enabled = True
    lblRCt.Caption = ""  ''Call genIDNo
    'txtEmp.Text = FormatNumber(lblPay.Caption, 2)
    'cboPayFor.Text = "CONSULTATION AND DRUGS"
    'cboClinic.Text = "OUT-PATIENT"
    'cboPay.Text = "CASH"

ElseIf chkRct.Value = False Then
    fraReceipt.Enabled = False
    txtHt.Text = "0"
    'txtEmp.Text = ""
    'cboReceived.ListIndex = 0
    cboPayFor.Text = ""
    'cboClinic.ListIndex = -1
    cboPay.ListIndex = -1
    txtWord.Text = ""
    lblRCt.Caption = ""

End If

Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cmdAdd_Click()
Screen.MousePointer = vbHourglass
 flg_Enforce_Saving = False
 isSaved = False
 
 isFromAttendGrid = False
 isFromHidden = False
 
 
    SSTab1.Tab = 0
'    txtAmt(0).Text = 0
'    txtAmt(1).Text = 0
'    txtAmt(2).Text = 0
'    txtAmt(3).Text = 0

cboPayFor.Text = ""
cboPay.ListIndex = -1

chkRct.Value = False

chkAll.Value = False 'nece here 'not in clearfields

blnSave = False


dtSys = getSysDateTime


If isBillNo Then

    SetButtons False
    enableFields True
    
    
    'strBillAccum = getBillAccumType  '''do not forget to comment this out
    gtBill = 0
    
    Screen.MousePointer = vbHourglass
    Call getPatForBill
    Screen.MousePointer = vbDefault
Else
    Call getPatForBill 'for now
    SetButtons False
    enableFields True
    
End If
Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
End Sub



Private Sub cmdCancel_Click()
blnSave = False
enableFields False
SetButtons True
Call clearFields

End Sub

Private Sub cmdCap_Click()
On Error GoTo errH

    If cboVehNo.Text = "" Then
        MsgBox "No Patient is selected"
        Exit Sub
    End If
    
    
'verify if already processed'''''''''''''''''''''''''''''''''''''''
Dim rsVerX As New Recordset
rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its processed
    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    Exit Sub
End If

rsVerX.Close 'Locked Bill
rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
    MsgBox "This Bill is Locked! requires Management Permission to Unlock"
    Exit Sub
End If

    
    
    Dim rsMgt As New Recordset
    With rsMgt
        .Open "select  * from vwUsers where loginRole='MANAGEMENT' and username='" & m_Username & "'", conStr, adOpenStatic, adLockOptimistic
        If .EOF Then
            MsgBox "Only User with Mgt Previlege can initiate this Action"
            Exit Sub
        End If
    End With
    
    
    If Capitate_NHIS_ONLY = "YES" Then
        If lblCat.Caption <> "NHIS" Then
            MsgBox "Only NHIS can be Capitated"
            Exit Sub
        End If
    End If


Dim Cmd As New Command
  'Dim strDel As Long
  Dim sSQlx As String
  Dim intOK As Integer
On Error GoTo errH
 intOK = MsgBox("Are you sure to Capitate all Items in this Bill", vbYesNo, "Capitation")
 If intOK = vbYes Then
    'strDel = BillNoCap
    Cmd.CommandType = adCmdText
    
    Dim connTran As New Connection
    connTran.ConnectionString = conStr
    connTran.Open
    connTran.BeginTrans
    
    Cmd.ActiveConnection = connTran
    
    Dim BillNoCap As String
    BillNoCap = lblBill.Caption
    
        
        Dim X As Integer
        Dim rsAdj  As New Recordset
        Dim rs  As New Recordset
        Dim oldPriceX As Double
        Dim oldQtyX As Double
        'If rsAdj.State = adStateOpen Then rsAdj.Close
        rsAdj.Open "select * from BillingDetailsAdjust where 1=2", connTran, adOpenStatic, adLockOptimistic
        rs.Open "select * from billingdetails where BillNo='" & BillNoCap & "'", connTran, adOpenStatic, adLockOptimistic
        For X = 1 To rs.RecordCount
            rsAdj.AddNew
            rsAdj!AdjustDate = sysDate
            rsAdj!AdjustTime = sysTime
            rsAdj!billNo = BillNoCap
            rsAdj!BillItem = rs!drgName
            
            oldQtyX = rs!Qty
            rsAdj!OldQty = oldQtyX
            rsAdj!newQty = oldQtyX
            
            oldPriceX = rs!price
            rsAdj!OldPrice = oldPriceX
            rsAdj!newPrice = 0
            rsAdj!AdjustBy = strEmpID
            rsAdj!Remarks = "BILL Capitated"
            rsAdj.Update
            
            
            Call Tranx(connTran, sysDate, strPatient, BillNoCap, -(oldQtyX * oldPriceX), BillNoCap, "Bill Item Capitated: " & rs!drgName, 1)
            Call Auditrail(m_Username, "Capitated  Bill Item for " & fullName, BillNoCap, "Item Capitated" & ": Qty:" & oldQtyX & " Amount: " & oldPriceX * oldQtyX, strHostName)
        
        Next
        
    
    sSQlx = "delete from billingdetails where BillNo = '" & BillNoCap & "'"
    Cmd.CommandText = sSQlx
    Cmd.Execute
    
    
    Cmd.CommandText = "update  billing set AmountBilled=0,AmountBilledInWord='NAIRA'  where BillNo = '" & BillNoCap & "'"
    Cmd.Execute
    
    Cmd.CommandText = "update  BillAccum set Capitated='YES',isBilled=0,AttendedTo=1  where consultID = '" & BillNoCap & "'"
    Cmd.Execute
    
    
        'Dim SubTotal2 As Double
        'Dim PNo As String
        'Dim rsVer2 As New Recordset
        'rsVer2.Open "select  PNo,Subtotal from vwBillingProcessDetailsGrouped where billno='" & BillNoCap & "'", connTran, adOpenStatic, adLockOptimistic
        'If Not rsVer2.EOF Then
        '    SubTotal2 = rsVer2!SubTotal
        '    PNo = rsVer2!PNo
        '    rsVer2.Close
        '    Call getValInWord(SubTotal2)
        '
        '    Cmd.CommandText = "update  billing set AmountBilledInWord='" & strWord & "', amountBilled= " & SubTotal2 & " where billno = '" & BillNoCap & "'"
        '    Cmd.Execute
        'End If
    
    
    connTran.CommitTrans

    MsgBox " Bill Item successfully Capitated"
    
    Call getAccumBill
    
    On Error GoTo Er
    'Call Auditrail(m_Username, "Capitated all Bill Items for " & fullName, BillNoCap, OldAdjust & ": Qty:" & OldQty & " Amount: " & OldPrice, strHostName)
    
    'Unload Me

   
 End If
Exit Sub


errH:

connTran.RollbackTrans
MsgBox Err.Description

Exit Sub


Er:

connTran.RollbackTrans
MsgBox Err.Description


End Sub

Public Sub cmdClose_Click()
Unload Me
End Sub

Private Sub cmdCoy_Click()
On Error GoTo errH

    If cboVehNo.Text = "" Then
        MsgBox "Please Select Patient Name"
        Exit Sub
    End If
    
    
    'If strPrivate = strCoy And lblDep.Caption <> 0 Then
    '    MsgBox "There is Payment on this Private bill! Remove it before changing Company"
    '    Exit Sub
    'End If

    
    
    


'verify if already processed'''''''''''''''''''''''''''''''''''''''
Dim rsVerX As New Recordset
rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its processed
    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    Exit Sub
End If

rsVerX.Close 'Locked Bill
rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
    MsgBox "This Bill is Locked! requires Management Permission to Unlock"
    Exit Sub
End If



    frmCoyUpdate.txtBill.Text = strCon
    frmCoyUpdate.txtNo.Text = strPatient
    'frmCoyUpdate.cboClient.Text = PolicyType
    frmCoyUpdate.txtEmp.Text = EnrolleNo
    frmCoyUpdate.lblCoy.Caption = strCompany
    frmCoyUpdate.lblClient.Caption = StrClientCatX
    
    frmCoyUpdate.Hide
    frmCoyUpdate.Show vbModal
    
    Call cboVehNo_Click

Exit Sub
errH:
MsgBox Err.Description


End Sub





Public Sub cmdHidden_Click()
On Error GoTo errH

If IsNull(DTAttnd1.Value) Or IsNull(DTAttnd2.Value) Then
    MsgBox "Please specify Attendance Date Range"
    DTPicker1.SetFocus
    Exit Sub
End If

If cboGroup.Text = "" Then    'Or cbogroup.ListIndex = -1 Then Exit Sub
    MsgBox "Specify Company"
    cboGroup.SetFocus
    Exit Sub
End If
'optHide = 1
'isFromAttendGrid = True

Screen.MousePointer = vbHourglass


lblTGenCap.Visible = False
lblTGen.Visible = False

lblTAmount.Caption = 0
lblAmountPaid.Caption = 0
lblBalance.Caption = 0



Dim rsVal As New Recordset
Set grdAttend.DataSource = Nothing
With rsVal
    Dim ssQL As String
    .CursorLocation = adUseClient
    
    grdAttend.Caption = "UnBilled Items"
    
    
        lblTAmount.Caption = "N/A"
        lblAmountPaid.Caption = "N/A"
        lblBalance.Caption = "N/A"
    
    
    'If cboGroup.Text = "(ALL)" Then
    '    .Open "select sum(subTotal) as Amount FROM  vwBillingProcessHiddenBills where Date between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'", conStr, adOpenStatic, adLockOptimistic
    'Else
    '    .Open "select sum(subTotal) as Amount FROM  vwBillingProcessHiddenBills where RetainCode = '" & AttndCoy & "' and Date between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'", conStr, adOpenStatic, adLockOptimistic
    'End If
    '
    'If Not .EOF Then
    '    lblTAmount.Caption = IIf(IsNull(!Amount), 0, FormatNumber(!Amount, 2))
    '    lblAmountPaid.Caption = 0 'IIf(IsNull(!AmountPaid), 0, FormatNumber(!AmountPaid, 2))
    '    lblBalance.Caption = 0 'IIf(IsNull(!Balance), 0, FormatNumber(!Balance, 2))
    '
    'Else
    '    lblTAmount.Caption = 0
    '    lblAmountPaid.Caption = 0
    '    lblBalance.Caption = 0
    'End If
    '
    '.Close
        
    
    If cboGroup.Text = "(ALL)" Then
        ssQL = "select  ROW_NUMBER() OVER (ORDER BY  Date,FullName,ConsultID) AS SNo,Date,Time,FullName,Service as Item,Qty,Price,Subtotal as Amount,coyName as Company,ClinicType as Clinic,Remarks as Purpose,ConsultID as BillNo,PhoneNo from vwBillingProcessHiddenBills where Date between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'"
    Else
        ssQL = "select  ROW_NUMBER() OVER (ORDER BY  Date,FullName,ConsultID) AS SNo,Date,Time,FullName,Service as Item,Qty,Price,Subtotal as Amount,coyName as Company,ClinicType as Clinic,Remarks as Purpose,ConsultID as BillNo,PhoneNo from vwBillingProcessHiddenBills where RetainCode='" & AttndCoy & "' and Date between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'"
    End If
    
    .Open ssQL, conStr, adOpenStatic, adLockOptimistic
    'MsgBox ssQL
    If Not .EOF Then
        Set grdAttend.DataSource = Nothing
        Set grdAttend.DataSource = rsVal
        grdAttend.Columns("SNo").Width = 400
        grdAttend.Columns("Date").Width = 1000
        grdAttend.Columns("Time").Width = 1000
        
        grdAttend.Columns("Amount").NumberFormat = "#,###.00"
        grdAttend.Columns("Amount").Alignment = dbgRight
        
        grdAttend.Columns("Price").NumberFormat = "#,###.00"
        grdAttend.Columns("Price").Alignment = dbgRight
        
        grdAttend.Columns("Qty").Width = 800
        grdAttend.Columns("Price").Width = 1000
        grdAttend.Columns("Amount").Width = 1200
        
        'grdAttend.Columns("coyname").Visible = False
    Else
        Set grdAttend.DataSource = Nothing
    End If
End With
Set rsVal = Nothing
Screen.MousePointer = vbDefault


Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub cmdMove_Click()
On Error GoTo errH


 
    ''verify if already processed'''''''''''''''''''''''''''''''''''''''
    'Dim rsVerX As New Recordset
    'rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
    'If Not rsVerX.EOF Then 'ie its processed
    '    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    '    Screen.MousePointer = vbDefault
    '    chkNil.Value = False
    '    Exit Sub
    'End If
    
    Dim rsBLV As New Recordset
    With rsBLV
      .Open "select username from vwusers where loginrole = 'MANAGEMENT' and username='" & m_Username & "'", conStr, adOpenForwardOnly, adLockReadOnly
      If .EOF Then 'not mgt user
          MsgBox "This Bill requires Management Permission to Move Payment"
          Screen.MousePointer = vbDefault
          Exit Sub
      End If
    End With
    
    
    
 
 

    If lblDep.Caption <= 0 Or lblDep.Caption = "" Then
        MsgBox "Invald! No Amount has been Paid", vbCritical
          Screen.MousePointer = vbDefault
        Exit Sub
    End If
    
    
    Dim BillNoMoveTo As String
    BillNoMoveTo = InputBox("Enter Bill No to Move Payment to", "Move Payment to Another Bill No", strCon & "B")

    
    
    Dim rsBLV2 As New Recordset
    With rsBLV2
      .Open "select ConsultID from hrecords where PNo ='" & lblPNo.Caption & "' and ConsultID='" & BillNoMoveTo & "'", conStr, adOpenForwardOnly, adLockReadOnly
      If .EOF Then 'not mgt user
          MsgBox "This Bill No does NOT belong to this Patient"
          Screen.MousePointer = vbDefault
          Exit Sub
      End If
    
        .Close
        .Open "select ConsultID from hrecords where CoyName='" & strPrivate & "' and PNo ='" & lblPNo.Caption & "' and ConsultID='" & BillNoMoveTo & "'", conStr, adOpenForwardOnly, adLockReadOnly
        If .EOF Then 'not mgt user
            MsgBox "This Bill No is not a Private Bill for this Patient"
            Screen.MousePointer = vbDefault
            Exit Sub
        End If
    
    
    End With
    
        Dim connTran As New Connection
        connTran.ConnectionString = conStr
        connTran.Open
        connTran.BeginTrans
        Dim dblPrice As Double
        Dim latestBillNo As String
        Dim rsExP As New ADODB.Recordset
        Dim Cmd As New Command
        Cmd.ActiveConnection = connTran
        Cmd.CommandType = adCmdText
        
    Screen.MousePointer = vbHourglass
        
        
    On Error GoTo errTran
    
    Dim AmountToMove As Double
    AmountToMove = CDbl(lblDep.Caption)
        
        Cmd.CommandText = "update billing set AmountPaid=0 where billno ='" & strCon & "'"
        Cmd.Execute
        
        Cmd.CommandText = "update billing set AmountPaid=AmountPaid +" & AmountToMove & "  where billno ='" & BillNoMoveTo & "'"
        Cmd.Execute
        
        Cmd.CommandText = "update Payments set BillNo='" & BillNoMoveTo & "'  where billno ='" & strCon & "'"
        Cmd.Execute
        
        Cmd.CommandText = "update PaymentDetails set BillNo='" & BillNoMoveTo & "'  where billno ='" & strCon & "'"
        Cmd.Execute
        
        
        
        connTran.CommitTrans
        
        'recalc tranaxaction
        Call CalcDebtByPat(connTran, lblPNo.Caption)
        
        Call UpdatePay(Me, strCon)
        
    Screen.MousePointer = vbDefault
        
    MsgBox "Move Payment to Another Private Bill Successful"
        
     
    
    Call Auditrail(m_Username, "Payment Moved to Another Private Bill for " & strName, strCon, "To: " & BillNoMoveTo & " Amount: " & AmountToMove, strHostName)

    Screen.MousePointer = vbDefault


Exit Sub
errTran:
Screen.MousePointer = vbDefault
connTran.RollbackTrans
MsgBox Err.Description


Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description


End Sub

Private Sub cmdNET_Click()
On Error GoTo errH

If strPatient = "" Then
    MsgBox "No Patient! Please select a Patient"
    Exit Sub
End If

If intRctNum <= 1 Then
    MsgBox "To Print Net Receipt, Number of Recepts issued must be greater than one"
    Exit Sub
End If


frmCashReceiptNet.Hide
frmCashReceiptNet.Show


Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description


End Sub

Private Sub cmdPVT_Click()
On Error GoTo errH
Screen.MousePointer = vbHourglass
isAdmission = False
Call getPatForBill
Screen.MousePointer = vbDefault
Exit Sub
errH:
Screen.MousePointer = vbDefault

MsgBox Err.Description
End Sub

Private Sub cmdShift_Click()
On Error GoTo errH

    If cboVehNo.Text = "" Then
        MsgBox "No Patient is selected"
        Exit Sub
    End If
    
    
'verify if already processed'''''''''''''''''''''''''''''''''''''''
Dim rsVerX As New Recordset
rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its processed
    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    Exit Sub
End If

rsVerX.Close 'Locked Bill
rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
    MsgBox "This Bill is Locked! requires Management Permission to Unlock"
    Exit Sub
End If

    
    
    Dim rsMgt As New Recordset
    With rsMgt
        .Open "select  * from vwUsers where loginRole='MANAGEMENT' and username='" & m_Username & "'", conStr, adOpenStatic, adLockOptimistic
        If .EOF Then
            MsgBox "Only User with Mgt Previlege can initiate this Action"
            Exit Sub
        End If
    End With
    

If CDbl(lblAmtDue.Caption) <= 0 Then
    MsgBox "Amount Due must be Greater than Zero" 'vwBillingProcess has amt > 0
    Exit Sub
End If



'Screen.MousePointer = vbHourglass



frmBillProcessingShift.getPatListByBillNo (strCon)
frmBillProcessingShift.Hide
frmBillProcessingShift.Show vbModal



Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description



End Sub

Private Sub cmdTran_Click()
On Error GoTo errH

If IsNull(DTAttnd1.Value) Or IsNull(DTAttnd2.Value) Then
    MsgBox "Please specify Attendance Date Range"
    DTPicker1.SetFocus
    Exit Sub
End If

If cboGroup.Text = "" Then    'Or cbogroup.ListIndex = -1 Then Exit Sub
    cboGroup.Text = "(ALL)"
    'MsgBox "Specify Company"
    'cboGroup.SetFocus
    'Exit Sub
End If
'optHide = 0
'isFromAttendGrid = False
Screen.MousePointer = vbHourglass
    
    lblTGenCap.Visible = True
    lblTGen.Visible = True
    
    lblTGen.Caption = 0
    lblTAmount.Caption = 0
    lblAmountPaid.Caption = "N/A"
    lblBalance.Caption = "N/A"



Dim rsVal As New Recordset
Set grdAttend.DataSource = Nothing
grdAttend.Caption = "Daily Tranxactions"
With rsVal
    Dim ssQL As String
    .CursorLocation = adUseClient
    
    If cboGroup.Text = "(ALL)" Then
        .Open "select sum(subTotal) as AmountGen ,sum(subTotal2) as AmountBilled FROM  qryBillAccumAll2 where BillDate between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'", conStr, adOpenStatic, adLockOptimistic
    Else
        .Open "select sum(subTotal) as AmountGen ,sum(subTotal2) as AmountBilled FROM  qryBillAccumAll2 where RetainCode = '" & AttndCoy & "' and BillDate between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'", conStr, adOpenStatic, adLockOptimistic
    End If
        
    'qryBillAccumAll has no amtPaid 'click on Attendance to see bills and Payments
    If Not .EOF Then
        lblTGen.Caption = IIf(IsNull(!amountGen), 0, FormatNumber(!amountGen, 2))
        lblTAmount.Caption = IIf(IsNull(!AmountBilled), 0, FormatNumber(!AmountBilled, 2))
        'lblAmountPaid.Caption = IIf(IsNull(!AmountPaid), 0, FormatNumber(!AmountPaid, 2))
        'lblBalance.Caption = IIf(IsNull(!Balance), 0, FormatNumber(!Balance, 2))
        lblAmountPaid.Caption = "N/A"
        lblBalance.Caption = "N/A"
    Else
        lblTGen.Caption = 0
        lblTAmount.Caption = 0
        lblAmountPaid.Caption = "N/A"
        lblBalance.Caption = "N/A"
    End If

    .Close
    
    
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    
                    
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        
    
    If cboGroup.Text = "(ALL)" Then
        ssQL = "select distinct ROW_NUMBER() OVER (order by BillDate,FullName,Service) AS Num , BillDate,RecDate as AttndDate,FullName,ConsultID as BillNo,Service,ClientCat,Referal,SNo,conID,case AttendedTo when 1  then 'YES' else 'NO' end as AttendedTo ,Capitated ,case isBilled when 0  then 'NO' when 1 then 'YES' end as Billed,Qty,UnitPrice,Subtotal,RevType,BillType,BillType2,RevType2,CoyName,BillTo,EntryDate,EntryTime,AppName,ClientName,EntryDate2,EntryTime2,AppName2,ClientName2,Company,StaffName as EnteredBy,StaffName2 as EnteredBy2,Clinic,Purpose from qryBillAccumAll2 where BillDate between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'"
    Else
        ssQL = "select distinct ROW_NUMBER() OVER (order by BillDate,FullName,Service) AS Num , BillDate,RecDate as AttndDate,FullName,ConsultID as BillNo,Service,ClientCat,Referal,SNo,conID,case AttendedTo when 1  then 'YES' else 'NO' end as AttendedTo ,Capitated ,case isBilled when 0  then 'NO' when 1 then 'YES' end as Billed,Qty,UnitPrice,Subtotal,RevType,BillType,BillType2,RevType2,CoyName,BillTo,EntryDate,EntryTime,AppName,ClientName,EntryDate2,EntryTime2,AppName2,ClientName2,Company,StaffName as EnteredBy,StaffName2 as EnteredBy2,Clinic,Purpose from qryBillAccumAll2 where RetainCode='" & AttndCoy & "' and BillDate between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'"
    End If
    
    .Open ssQL, conStr, adOpenStatic, adLockOptimistic
    'MsgBox ssQL
    If Not .EOF Then
        Set grdAttend.DataSource = Nothing
        Set grdAttend.DataSource = rsVal
                    
        grdAttend.Columns("Subtotal").NumberFormat = "#,###.00"
        grdAttend.Columns("Subtotal").Alignment = dbgRight
        grdAttend.Columns("UnitPrice").NumberFormat = "#,###.00"
        grdAttend.Columns("UnitPrice").Alignment = dbgRight
        grdAttend.Columns("Qty").NumberFormat = "#,##;(#,##0)"  '"#,###.00"
        grdAttend.Columns("Qty").Alignment = dbgRight
    
    
        grdAttend.Columns("SNO").Visible = False
        'grdAttend.Columns("Billtype").Visible = False
        grdAttend.Columns("conID").Visible = False
        grdAttend.Columns("clientcat").Visible = False
        grdAttend.Columns("Referal").Visible = False
        grdAttend.Columns("Capitated").Width = 800
        grdAttend.Columns("Num").Width = 500
        grdAttend.Columns("Qty").Width = 1000
        grdAttend.Columns("UnitPrice").Width = 1200
        grdAttend.Columns("Subtotal").Width = 1500
        grdAttend.Columns("BillNo").Width = 1500
        grdAttend.Columns("BillDate").Width = 1200
        grdAttend.Columns("AttndDate").Width = 1200
        
    Else
        Set grdAttend.DataSource = Nothing
    End If
End With
Set rsVal = Nothing

Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description


End Sub

Private Sub cmdUnPro_Click()
On Error GoTo errH

If IsNull(DTAttnd1.Value) Or IsNull(DTAttnd2.Value) Then
    MsgBox "Please specify Attendance Date Range"
    DTPicker1.SetFocus
    Exit Sub
End If

If cboGroup.Text = "" Then    'Or cbogroup.ListIndex = -1 Then Exit Sub
    cboGroup.Text = "(ALL)"
    'MsgBox "Specify Company"
    'cboGroup.SetFocus
    'Exit Sub
End If

'optHide = 0
'isFromAttendGrid = False
Screen.MousePointer = vbHourglass

Dim BatchNo As String

BatchNo = CStr(Year(DTAttnd1.Value)) & "/" & CStr(Right("00" & Month(DTAttnd1.Value), 2))

lblTGenCap.Visible = False
lblTGen.Visible = False

lblTAmount.Caption = 0
lblAmountPaid.Caption = 0
lblBalance.Caption = 0


Dim rsVal As New Recordset
Set grdAttend.DataSource = Nothing
With rsVal
    Dim ssQL As String
    .CursorLocation = adUseClient
    
 grdAttend.Caption = "UnProcessed Bills"
    If cboGroup.Text = "(ALL)" Then
        .Open "select sum(ISNULL(AmountBilled, 0) + ISNULL(DebtBF, 0) - ISNULL(Discount, 0)) as AmountPayable,sum(AmountPaid) as AmountPaid,sum((ISNULL(AmountBilled, 0) + ISNULL(DebtBF, 0)) - (ISNULL(Discount, 0) + ISNULL(AmountPaid, 0))) as Balance FROM  vwBillingProcessAll where isProcess=0 and BatchNo ='" & BatchNo & "'", conStr, adOpenStatic, adLockOptimistic
    Else
        .Open "select sum(ISNULL(AmountBilled, 0) + ISNULL(DebtBF, 0) - ISNULL(Discount, 0)) as AmountPayable,sum(AmountPaid) as AmountPaid,sum((ISNULL(AmountBilled, 0) + ISNULL(DebtBF, 0)) - (ISNULL(Discount, 0) + ISNULL(AmountPaid, 0))) as Balance FROM  vwBillingProcessAll where isProcess=0 and BatchNo ='" & BatchNo & "' and RetainCode = '" & AttndCoy & "'", conStr, adOpenStatic, adLockOptimistic
    End If
        
    If Not .EOF Then
        lblTAmount.Caption = IIf(IsNull(!AmountPayable), 0, FormatNumber(!AmountPayable, 2))
        lblAmountPaid.Caption = IIf(IsNull(!AmountPaid), 0, FormatNumber(!AmountPaid, 2))
        lblBalance.Caption = IIf(IsNull(!Balance), 0, FormatNumber(!Balance, 2))
    
    Else
        lblTAmount.Caption = 0
         lblAmountPaid.Caption = 0
         lblBalance.Caption = 0
    End If

    .Close
        
    
    If cboGroup.Text = "(ALL)" Then
        ssQL = "select ROW_NUMBER() OVER ( order by AttdDate,BillNo) AS SNo,AttdDate as Date,htime as Time,FullName,AdmDate,DischDate,BatchNo,ISNULL(AmountBilled, 0) + ISNULL(DebtBF, 0) - ISNULL(Discount, 0)as AmountPayable,AmountPaid,CoyName as Company,ClinicType as Clinic,Remarks as Purpose,BillNo,PhoneNo from vwBillingProcessAll where isProcess=0 and BatchNo ='" & BatchNo & "'"
    Else
        ssQL = "select ROW_NUMBER() OVER ( order by AttdDate,BillNo) AS SNo,AttdDate as Date,htime as Time,FullName,AdmDate,DischDate,BatchNo,ISNULL(AmountBilled, 0) + ISNULL(DebtBF, 0) - ISNULL(Discount, 0)as AmountPayable,AmountPaid,CoyName as Company,ClinicType as Clinic,Remarks as Purpose,BillNo,PhoneNo from vwBillingProcessAll where isProcess=0 and BatchNo ='" & BatchNo & "' and RetainCode = '" & AttndCoy & "'"
    End If
    
    .Open ssQL, conStr, adOpenStatic, adLockOptimistic
    'MsgBox ssQL
    If Not .EOF Then
        Set grdAttend.DataSource = Nothing
        Set grdAttend.DataSource = rsVal
        grdAttend.Columns("SNo").Width = 600
        grdAttend.Columns("Date").Width = 1000
        grdAttend.Columns("Time").Width = 1000
        grdAttend.Columns("AdmDate").Width = 1000
        grdAttend.Columns("DischDate").Width = 1000
        grdAttend.Columns("AmountPayable").Width = 1200
        grdAttend.Columns("AmountPaid").Width = 1200
        grdAttend.Columns("AmountPayable").NumberFormat = "#,###.00"
        grdAttend.Columns("AmountPayable").Alignment = dbgRight
        grdAttend.Columns("AmountPaid").NumberFormat = "#,###.00"
        grdAttend.Columns("AmountPaid").Alignment = dbgRight
        'grdAttend.Columns("expired").Visible = False
        'grdAttend.Columns("coyname").Visible = False
    Else
        Set grdAttend.DataSource = Nothing
    End If
End With
Set rsVal = Nothing

Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault

MsgBox Err.Description

End Sub

Private Sub Form_Activate()
Screen.MousePointer = vbDefault

'On Error GoTo errH
'       Dim rsOldVer As New Recordset
'       Dim StrAppBilling As String
'       StrAppBilling = "BILLING"
'       With rsOldVer
'        .Open "select idval from AppDefaults where ID='LockOldVersion'", conStr, adOpenStatic, adLockOptimistic
'        If Not .EOF Then
'            LockOldVersion = !idval & ""
'            If LockOldVersion = "YES" Then
'                .Close
'                .Open "select MinVer from Roles where LoginRole='" & StrAppBilling & "'", conStr, adOpenStatic, adLockOptimistic
'                If Not .EOF Then
'                    Dim MinVer As Integer
'                    MinVer = IIf(IsNull(!MinVer), 0, !MinVer)
'                    If MinVer > App.Major Then
'                        MsgBox "This Version of " & StrAppBilling & " Module is OLD!" & vbNewLine & _
'                        "Only Versions " & MinVer & " and Above are Allowed", vbCritical, "Old Version Not Allowed"
'                        Unload Me
'                    Else
'                        LockOldVersion = "NO"
'
'                        'Dim cmd As New Command
'                        'cmd.ActiveConnection = conStr
'                        'cmd.CommandType = adCmdText
'                        'cmd.CommandText = "Updae Roles set MinVer=" & App.Major & " where LoginRole='" & strApp & "'"
'                        'cmd.Execute
'                    End If
'                Else
'                    LockOldVersion = "NO"
'                End If
'            Else
'                LockOldVersion = "NO"
'            End If
'
'        Else
'            LockOldVersion = "NO"
'        End If
'
' End With
'
'Exit Sub
'
'errH:
'MsgBox Err.Description
End Sub


Private Sub Form_QueryUnload(Cancel As Integer, UnloadMode As Integer)
On Error GoTo errH
If Enforce_Saving_In_Collate_Bill = "YES" And flg_Enforce_Saving = True Then
    If lblBill.Caption <> "" Then
        If CDbl(lblAmtDue.Caption) <> 0 Then
            Call OKButton_Click ' to save bill auto
            'flg_Enforce_Saving = False 'already in OKButton_Click
            'cmdAdd_Click 'form is closing
            Exit Sub
        End If
    End If
End If

      
Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub lblDiscount_dblClick()
    On Error GoTo errH
    If cboVehNo.Text = "" Then
        MsgBox "No Patient is selected"
        Exit Sub
    End If
    
    
    If LockDiscount = "YES" Then
        Dim rsBLV2 As New Recordset
        With rsBLV2
          .Open "select username from vwusers where loginrole = 'MANAGEMENT' and username='" & m_Username & "'", conStr, adOpenForwardOnly, adLockReadOnly
          If .EOF Then 'not mgt user
              MsgBox "Giving Discount requires Management Permission"
              Exit Sub
          End If
        End With
    End If
    'verify if already processed'''''''''''''''''''''''''''''''''''''''
Dim rsVerX As New Recordset
rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its processed
    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    Exit Sub
End If

rsVerX.Close 'Locked Bill
rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
    MsgBox "This Bill is Locked! requires Management Permission to Unlock"
    Exit Sub
End If


Dim dblBal2 As Double
dblBal2 = CDbl(lblAmtDue.Caption)
'If dblBal2 <= 0 Then
If dblBal2 = 0 Then
    MsgBox "Zero Balance Amount Cannot be Discounted. No Bill to Discount", vbCritical
    'MsgBox "The Amount Billed Cannot be Discounted. Please Check the Amount", vbCritical
    Exit Sub
End If
    

'rem bcos corp clients may request discount for previous bills
'         Dim Tran As New Recordset
'         With Tran 'use hrecords not Billing
'             .Open "select top 1 ConsultID from hrecords where PNo='" & strPatient & "'  order by recID Desc", conStr, adOpenStatic, adLockOptimistic
'             If Not .EOF Then
'                If strCon <> !consultID & "" Then
'                    MsgBox "Previous Transaction Cannot be Discounted. Only Last Or Current Transaction  can be Discounted", vbCritical, "Previous Transaction Cannot be Discounted"
'                    Exit Sub
'                End If
'             End If
'        End With

    
    
    frmDiscountType.Hide
    frmDiscountType.Show vbModal
    
    
    Dim Cmd As Command
    Set Cmd = New Command
    Dim connTran As New Connection
    connTran.ConnectionString = conStr
    connTran.Open
    
    Cmd.ActiveConnection = connTran
    Cmd.CommandType = adCmdText
    
    Dim subAmt As Double
    subAmt = 0
    
If DiscType = "" Then
    DiscType = "PCENT"
    MsgBox "No Option Chosen! Discount will be in Percentage"
End If

Dim dblDisc As Double
If DiscType = "PCENT" Then
    dblDisc = InputBox("Enter Discount In %", "Enter  Discount In %", "5")
Else
    dblDisc = InputBox("Enter Discount In Figures", "Flat Rate", "0")
End If

Dim rsDetails As New Recordset
Dim dblPrf As Double


If dblDisc < 0 Then
    MsgBox "Discount cannot be less than Zero"
    Exit Sub
End If

      
      On Error GoTo TransFail
    
      connTran.BeginTrans
      
      Dim dblDebt As Double
      Dim dblAmtPaid2 As Double
      Dim dblBillPayable As Double
      Dim totAmountBilled As Double
      Dim dblDiscAmt As Double
      Dim OldDiscount As Double
      Dim dblDiscPCent As Double
      Dim isReversed As Boolean
      Dim intSNo As Long
   
      dblAmtPaid2 = CDbl(lblDep.Caption)
      totAmountBilled = CDbl(lblTotal.Caption) 'shld come b4 discount
      dblDebt = CDbl(lblDebt.Caption)
      
      OldDiscount = CDbl(lblDiscount.Caption)
      
      If DiscType = "PCENT" Then
        dblDiscAmt = FormatNumber((dblDisc * totAmountBilled) / 100, 2)
        dblDiscPCent = FormatNumber(CDbl(dblDisc), 2)
      Else
        dblDiscAmt = FormatNumber(CDbl(dblDisc), 2) '  flat rate
        dblDiscPCent = FormatNumber((dblDisc * 100) / totAmountBilled, 2)
      End If
      
      dblBillPayable = FormatNumber((totAmountBilled + dblDebt), 2) - dblDiscAmt
      
      Dim discRemarks  As String
      discRemarks = "Discount of " & dblDiscAmt & " ( " & dblDiscPCent & "%) given to " & strName & " from " & OldDiscount & " to " & dblDiscAmt


    isReversed = False

With rsDetails
      
    .Open "Select BillNo from billingDiscount where BillNo='" & strCon & "'", connTran, adOpenStatic, adLockOptimistic
    If Not .EOF Then
        Cmd.CommandText = "Update BillingDiscount set PCent=" & dblDiscPCent & ",AmountBilled=" & totAmountBilled & ",Amount=" & dblDiscAmt & "  where BillNo='" & strCon & "'"
        Cmd.Execute
        
        Cmd.CommandText = "Update Billing set Discount=" & dblDiscAmt & "  where BillNo='" & strCon & "'"
        Cmd.Execute
        
        If .State = adStateOpen Then .Close
        isReversed = True
        .Open "Select SNo from BillingDiscountDetails where Reversed=0 and BillNo='" & strCon & "'", connTran, adOpenStatic, adLockOptimistic
        If Not .EOF Then
            intSNo = !SNo
            Cmd.CommandText = "Update BillingDiscountDetails set Reversed=1 where BillNo='" & strCon & "' and SNo=" & intSNo 'rev all
            Cmd.Execute
        End If
        
        If .State = adStateOpen Then .Close
        .Open "Select * from BillingDiscountDetails where 1=2", connTran, adOpenStatic, adLockOptimistic
        .AddNew
        !DtDate = sysDate
        !billNo = strCon
        !drgName = "REVERSED INITIAL DISCOUNT for " & strName  '+ ve to knock off prev
        !Amount = OldDiscount
        !isPost = 0
        !Reversed = 1
        .Update
        
        .AddNew
        !DtDate = sysDate
        !billNo = strCon
        !drgName = "NEW DISCOUNT for " & strName  '- ve
        !Amount = -(dblDiscAmt)
        !isPost = 0
        !Reversed = 0
        .Update
        
        
        '+ve discount to nullify old discount
        Call Tranx(connTran, sysDate, strPatient, strCon, OldDiscount, strCoy, "REVERSED INITIAL DISCOUNT", 2)
        '-ve new discount
        Call Tranx(connTran, sysDate, strPatient, strCon, -(dblDiscAmt), strCoy, discRemarks, 2)
        
    Else
        If .State = adStateOpen Then .Close
        .Open "Select * from billingDiscount where 1=2", connTran, adOpenStatic, adLockOptimistic
        .AddNew
        !DtDate = sysDate
        !dtTime = sysTime
        !billNo = strCon
        !Amount = dblDiscAmt
        !pcent = dblDiscPCent
        !AmountBilled = totAmountBilled
        !drgName = "DISCOUNT for " & strName
        !Remarks = discRemarks
        .Update
        
        
        If .State = adStateOpen Then .Close
        isReversed = False
        .Open "Select * from BillingDiscountDetails where 1=2", connTran, adOpenStatic, adLockOptimistic
        .AddNew
        !DtDate = sysDate
        !billNo = strCon
        !drgName = "DISCOUNT for " & strName '- ve
        !Amount = -(dblDiscAmt)
        !isPost = 0
        !Reversed = 0
        .Update
        
        Cmd.CommandText = "Update Billing set Discount=" & dblDiscAmt & "  where BillNo='" & strCon & "'"
        Cmd.Execute
        
        
        '-ve new discount
        Call Tranx(connTran, sysDate, strPatient, strCon, -(dblDiscAmt), strCoy, discRemarks, 2)

    End If
    

End With


connTran.CommitTrans

On Error GoTo errH

Call Auditrail(m_Username, "Discount for " & strName, strCon, discRemarks, strHostName)

'Call updateBill(strCon, strCoy, strPatient)
Call UpdatePay(Me, strCon)
     
''''''''''''''''''''''''''''''PostToAccounts'''''''''''''''''''''''''''''''''

        If AcctPostOn = True Then
                    
                    Dim rsAccts2 As New Recordset
                    Dim KX As Integer
                    Dim connTran2 As New Connection
                    connTran2.ConnectionString = conStrAccts
                    connTran2.Open
                    
                    
                    Dim cmd2 As New Command
                    cmd2.ActiveConnection = connTran2
                    cmd2.CommandType = adCmdText
    
                On Error GoTo TransFailAccts
    
                   connTran2.BeginTrans
                    
            
                    Call getTranID(connTran2) 'very nece 'outside the for---next statement of vwRctForAccts
                    Period = getPeriod(connTran2, entryDate) 'entryDate as for rct
                    Call CreateAccounts(connTran2, "RECEIVABLE") 'after getPeriod
                    
                    
                    Dim AmtCredit As Double
                    Dim AmtDebit As Double
                    
                    If isReversed = True Then 'knock off has 2 tranx ' one tranx has 2 legs dr , cr
                        'Abnormal Entry'''
                        'debit''Recv'''''''''''''''''''''
                        Call PostToAccounts(connTran2, sysDate, AccountNo_Recv, (OldDiscount), "REVERSED INITIAL DISCOUNT for " & strName, "ASSET", "h")    'credit side
                        'credit sales Disc''is''ok for reversal to knock off old values''''Abnormal Entry'''''''''''''''
                        Call PostToAccounts(connTran2, sysDate, AcctNo_Sales_Discount, -(OldDiscount), "REVERSED INITIAL DISCOUNT for " & strName, "ASSET", "h")  'credit side
                        
                        'Normal Entry'''
                        'debit''sales Disc for Normal entry'''''''''''''''''''''
                        Call PostToAccounts(connTran2, sysDate, AcctNo_Sales_Discount, (dblDiscAmt), "NEW DISCOUNT for " & strName, "ASSET", "h")   'credit side
                        'credit'''recv''''''''''''''''''''
                        Call PostToAccounts(connTran2, sysDate, AccountNo_Recv, -(dblDiscAmt), "NEW DISCOUNT for " & strName, "ASSET", "h")    'credit side
                    
                    Else '
                        
                        ''Normal entry'''debit''sales Disc for ''''''''''''''''''
                        Call PostToAccounts(connTran2, sysDate, AcctNo_Sales_Discount, (dblDiscAmt), "DISCOUNT for " & strName, "ASSET", "b") 'debit side
                        'credit'''''''''''''''''''''''
                        Call PostToAccounts(connTran2, sysDate, AccountNo_Recv, -(dblDiscAmt), "DISCOUNT for " & strName, "ASSET", "b")  'credit side
                    End If
                
                
                    'set isPost=1 for the entire billNo --cos of xple SNOs
                    cmd2.CommandText = "update " & DBName & "..BillingDiscountDetails set isPost=1 where billNo = '" & strCon & "'"
                    cmd2.Execute
                
                '''''''''''''''''Confirm Dr=Cr'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                
                
                Dim rsAmt As New Recordset
                Select Case AcctPostType
                Case "AUTO"
                    rsAmt.Open "select  dbo.TranBalance('" & Period & "','" & coyID & "') as Amount", connTran2, adOpenStatic, adLockOptimistic
                Case "BATCH"
                    rsAmt.Open "select  dbo.TranBalanceJournal('" & Period & "','" & coyID & "') as Amount", connTran2, adOpenStatic, adLockOptimistic
                End Select
        
                If Not rsAmt.EOF Then
                    If rsAmt!Amount <> 0 Then
                        connTran2.RollbackTrans
                        'Call clearFields
                        'Call EnableFields(False)
                        'SetButtons True
                        MsgBox "Account Posting Failed"
                        Exit Sub
                    End If
                Else
                    connTran2.RollbackTrans
                    'Call clearFields
                    'Call EnableFields(False)
                    'SetButtons True
                    MsgBox "Account Posting Failed"
                    Exit Sub
                End If
  
        
                connTran2.CommitTrans
                
        
        End If
            
Exit Sub

TransFailAccts:
connTran.RollbackTrans
MsgBox Err.Description
      
'-----------------------------------------------------------------------------------
      
Exit Sub

TransFail:
connTran.RollbackTrans
MsgBox Err.Description
      
Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub cmdOKRct_Click()
On Error GoTo errH

If Trim(txtSearchRct.Text) = "" Then
    MsgBox "Specify Receipt No"
    txtSearchRct.SetFocus
    Exit Sub
End If

        Dim StrRctNo As String
        
        StrRctNo = Trim(txtSearchRct.Text)
        '''''''''''''''''Receipt History'''''''''''''''''''''''''''''
        Set grdDataRct.DataSource = Nothing
        grdDataRct.Caption = "Payment History"
        Dim rsRct As New Recordset
        With rsRct
         .CursorLocation = adUseClient
         .Open "select ReceiptDate,rTime as Time,ReceiptNo,BillNo,AmountBilled as AmountDue,AmountPaid,Balance,PaymentFor,PayType,ClinicID as Clinic,ReceivedBy from qryhBillingIncome where ReceiptNo='" & StrRctNo & "' order by ReceiptNo", conStr, adOpenStatic, adLockOptimistic
         If Not .EOF Then
             Set grdDataRct.DataSource = rsRct
             
             grdDataRct.Columns("AmountDue").NumberFormat = "#,###.00"
             grdDataRct.Columns("AmountDue").Alignment = dbgRight
             grdDataRct.Columns("AmountPaid").NumberFormat = "#,###.00"
             grdDataRct.Columns("AmountPaid").Alignment = dbgRight
             grdDataRct.Columns("Balance").NumberFormat = "#,###.00"
             grdDataRct.Columns("Balance").Alignment = dbgRight
        Else
             Set grdDataRct.DataSource = Nothing
        End If
    End With

Exit Sub
errH:
MsgBox Err.Description


End Sub

Private Sub cmdPay_Click()
On Error GoTo errH
        '''''''''''''''''Receipt History'''''''''''''''''''''''''''''
        Set grdDataRct.DataSource = Nothing
        grdDataRct.Caption = "Payment For Today"
        Dim rsRct As New Recordset
        With rsRct
         .CursorLocation = adUseClient
         .Open "select ReceiptDate,rTime as Time,ReceiptNo,BillNo,AmountBilled as AmountDue,AmountPaid,Balance,PaymentFor,PayType,ClinicID as Clinic,ReceivedBy from qryhBillingIncome where ReceiptDate='" & sysDate & "' order by ReceiptNo", conStr, adOpenStatic, adLockOptimistic
         If Not .EOF Then
             Set grdDataRct.DataSource = rsRct
             
             grdDataRct.Columns("AmountDue").NumberFormat = "#,###.00"
             grdDataRct.Columns("AmountDue").Alignment = dbgRight
             grdDataRct.Columns("AmountPaid").NumberFormat = "#,###.00"
             grdDataRct.Columns("AmountPaid").Alignment = dbgRight
             grdDataRct.Columns("Balance").NumberFormat = "#,###.00"
             grdDataRct.Columns("Balance").Alignment = dbgRight
        Else
             Set grdDataRct.DataSource = Nothing
        End If
    End With
Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub cmdSplit_Click()
On Error GoTo errH

If Len(strCon) = 13 And Mid(strCon, 4, 3) <> "OFL" Then 'split bill
    MsgBox "this bill cannot be Split! Already a Split  Bill", vbInformation, "Split Bill"
    Exit Sub
End If


'If Len(strCon) > 12 Then
'    MsgBox "this bill cannot be Split! Already a Split  Bill"
'    Exit Sub
'End If


'verify if already processed'''''''''''''''''''''''''''''''''''''''
Dim rsVerX As New Recordset
rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its processed
    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    Exit Sub
End If

rsVerX.Close 'Locked Bill
rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
    MsgBox "This Bill is Locked! requires Management Permission to Unlock"
    Exit Sub
End If





            Dim rsVer As New Recordset
            With rsVer
                .Open "select  recDate from hRecords where substring(consultID,1,12)='" & strCon & "'", conStr, adOpenStatic, adLockOptimistic
                If Not .EOF Then
                    If .RecordCount > 3 Then
                        MsgBox "No of Bill Splitting Exceeded"
                        Exit Sub
                    End If
                
                End If
            End With

frmBillSplitting.Label5.Caption = strName
frmBillSplitting.Show vbModal


txtSearch.Text = gStrConSplit
Call cmdOK_Click
'Call frmBillingVerify.cboVehNo_Click 'not nece


Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cmdDispense_Click()
On Error GoTo errH
If Trim(cboVehNo.Text) = "" Then
MsgBox " No Patient is specified"
Exit Sub
End If
Dim strPol As String
Dim strZ As String
Dim strTPlan As String
  Dim rsBL As New Recordset
  Set grdData.DataSource = Nothing
  With rsBL
  .Open "select consultid,treatplan from qryhconsulting where consultid='" & strCon & "'", conStr, adOpenStatic, adLockOptimistic
If Not .EOF Then
    If !treatPlan = "" Then
        MsgBox "No Treatment Plan "
    Else
        'strZ = ""
        'strZ = "Policy/HMO Type: "
        'strPol = !policyType
            'If strPol <> "" Then
                'strZ = strZ & strPol
             'Else
                'strZ = ""
            'End If
        getAccumBill
        'getDebtForBill (strPatient)
        
        .MoveFirst
        Do While Not .EOF
        strTPlan = strTPlan & !treatPlan & vbNewLine
        Loop
        
        MsgBox strTPlan & vbNewLine & vbNewLine & strZ & vbNewLine & vbNewLine & "Treated by Dr. " & !treatedBy
        
        'grdData.Refresh

    End If
Else
        getAccumBill
        getDebtForBill (strPatient)
        MsgBox "No Treatment Plan "

End If
End With

Set rsBL = Nothing

Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub cmdExt_Click()
Call cmdCancel_Click
frmInvestigatePublic.Hide
frmInvestigatePublic.Show
End Sub

Private Sub cmdLastDiag_Click()
On Error GoTo errH

            Dim Cmd As New ADODB.Command
            Cmd.ActiveConnection = conStr
            Cmd.CommandType = adCmdText
            'cmd.CommandText = "update  billing set ApprvCode='" & Trim(Replace(txtApprv.Text, "'", "''")) & "', diagnosis='" & UCase(Trim(Replace(txtDiag.Text, "'", "''"))) & "' where billNo = '" & strCon & "'"
            'no update for billing month and year
            Cmd.CommandText = "update  billing set diagnosis='" & UCase(Trim(Replace(txtDiag.Text, "'", "''"))) & "' where billNo = '" & strCon & "'"
            Cmd.Execute
            MsgBox "Diagnosis Updated"

 

'''Last Diagnosis
' Dim rsBL As New Recordset
'  With rsBL
'  .Open "select ConsultID, cDate,Diagnosis from qryhconsulting where pno='" & strPatient & "' order by ID Desc", conStr, adOpenStatic, adLockOptimistic
'    If Not .EOF Then
'       .MoveFirst
'        Do While Not .EOF
'            If !consultID <> strCon Then
'                .MoveNext
'            Else
'                .MoveNext
'                If Not .EOF Then
'                    MsgBox "Previous Diagnosis on " & !CDate & " is:" & vbNewLine & !diagnosis & ""
'                    Exit Do
'                Else
'                    MsgBox "No Previous Diagnosis"
'                    Exit Do
'                End If
'            End If
'        Loop
'    Else
'        MsgBox "No Previous Diagnosis"
'    End If
'
'End With

Exit Sub
    
errH:
    MsgBox Err.Description
End Sub

Private Sub cmdList_Click()
On Error GoTo errH
Screen.MousePointer = vbHourglass
isAdmission = False
Call getPatForBill
Screen.MousePointer = vbDefault
Exit Sub
errH:
Screen.MousePointer = vbDefault

MsgBox Err.Description
End Sub

Private Sub cmdNHIS_Click()
On Error GoTo errH
If cboVehNo.Text = "" Then
    MsgBox "Specify Patient Name"
    cboVehNo.SetFocus
    Exit Sub
End If

'verify if already processed'''''''''''''''''''''''''''''''''''''''
Dim rsVerX As New Recordset
rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its processed
    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    Exit Sub
End If

rsVerX.Close 'Locked Bill
rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
    MsgBox "This Bill is Locked! requires Management Permission to Unlock"
    Exit Sub
End If



If txtNHIS = "" Or Not IsNumeric(txtNHIS.Text) Then
    MsgBox "Specify NHIS fee! Numeric Only"
    txtNHIS.SetFocus
    Exit Sub
End If


    Dim Cmd As Command
    Set Cmd = New Command
    Dim connTran As New Connection
    connTran.ConnectionString = conStr
    connTran.Open
    
    Cmd.ActiveConnection = connTran
    Cmd.CommandType = adCmdText
    
    Dim subAmt As Double
    subAmt = 0
   
   ''''''''''''Add NHIS 10% to bill'''''''''''''''''''''
Dim strNHIS As String
strNHIS = InputBox("Enter Item Description", "Enter Item Description", "NHIS  (" & txtNHIS.Text & "%) Fee")
Dim rsDetails As New Recordset
Dim dblPrf As Double


If Trim(strNHIS) = "" Then
    MsgBox "No Description or title for this fee! Please Specify"
    Exit Sub
End If

      
      On Error GoTo TransFail
    
      connTran.BeginTrans
      


Select Case NHISFee
Case "ALL", ""
    rsDetails.Open "select sum(subtotal) as Subtotal from BillAccum where consultid='" & strCon & "'", connTran, adOpenStatic, adLockOptimistic
Case Else
    rsDetails.Open "select sum(isnull(subtotal,0)) as Subtotal from BillAccum where consultid='" & strCon & "' and billtype='DRUG'", connTran, adOpenStatic, adLockOptimistic
End Select

If Not rsDetails.EOF Then
    dblPrf = (IIf(IsNull(rsDetails!SubTotal), 0, rsDetails!SubTotal) * CDbl(txtNHIS.Text)) / 100
Else
    dblPrf = 0
End If

'Dim dblSubX As Double
'dblSubX = CDbl(txtPrice.Text) * CDbl(txtQty.Text)
If dblPrf < 0 Then
    MsgBox strNHIS & " Cannot be less than Zero"
    Exit Sub
End If




'rem for now
''first delete any bill
' Cmd.CommandText = "Delete from billingDetails where billNo='" & strCon & "'"
' Cmd.Execute
'
' 'then
' Cmd.CommandText = "update BillAccum set Capitated='YES',isbilled=1  where consultID='" & strCon & "'"
' Cmd.Execute


rsDetails.Close
rsDetails.Open "select * from BillAccum where 1=2 ", connTran, adOpenStatic, adLockOptimistic
     rsDetails.AddNew
     rsDetails!DtDate = sysDate
     rsDetails!consultID = strCon
     rsDetails!drgName = strNHIS
     rsDetails!price = dblPrf 'dblPrf 'ok 'now split bill
     rsDetails!Qty = 1
     rsDetails!SubTotal = dblPrf
     rsDetails!billType = "SERVICE"
     rsDetails!conID = 0
     rsDetails!Usage = ""
     rsDetails!Capitated = "NO" 'ok for NHIS Fee in BillAccum, and Yet a bill in Billing Details
     rsDetails!isbilled = 0
     rsDetails!attendedto = 1
     rsDetails!suppres = 0
     rsDetails!Category = "CONSULTATION"
     rsDetails!PNo = strPatient
     rsDetails!coyName = strCoy
     rsDetails!BillTo = strCoy
     If NHISFee = "DRUG" Then
        rsDetails!revType = RevType_Drug
     Else
        If InStr(strNHIS, "NHIS") > 0 Then
            rsDetails!revType = RevType_NHIS_Fee 'RevType_Misc
        Else
            rsDetails!revType = RevType_Misc
        End If
     End If
     rsDetails!conID = 0      'gIntIDx     'null ok since bill may exists without seeing the doctor
     rsDetails!BillBy = strEmpID
     rsDetails!AppVersion = App.Major
     rsDetails.Update
     
     
        '
        '    Dim rsX As New Recordset
        '    rsX.Open "select top 1 SNO from billaccum order by SNo desc", connTran, adOpenForwardOnly, adLockReadOnly
        '    strConIDVal = rsX!SNo
        '    Set rsX = Nothing
        '
        '    Dim rsBillDetl As New Recordset
        '    rsBillDetl.Open "Select * from BillingDetails where 1=2", connTran, adOpenStatic, adLockOptimistic
        '    rsBillDetl.AddNew
        '    rsBillDetl!dtDate = sysdate
        '    rsBillDetl!SNo = strConIDVal
        '    rsBillDetl!billNo = strCon
        '    'rsBillDetl!Category = "CONSULTATION"
        '    rsBillDetl!drgName = strNHIS
        '    rsBillDetl!Price = dblPrf 'ok
        '    rsBillDetl!Qty = 1
        '    rsBillDetl!SubTotal = dblPrf
        '    rsBillDetl!dosage = ""
        '    rsBillDetl!billType = "SERVICE"
        '    rsBillDetl!conID = 0      'gIntIDx     'null ok since bill may exists without seeing the doctor
        '    rsBillDetl!Capitated = "NO"
        '    rsBillDetl!BillTo = strCoy
        '    rsBillDetl!coyName = strCoy
        '    If NHISFee = "DRUG" Then
        '        rsDetails!revType = "DRUG"
        '     Else
        '        rsDetails!revType = "CONSULTATION"
        '     End If
        '
        '     rsBillDetl!BillBy = strEmpID
        '
        '    rsBillDetl.Update
        '
        ' 'then
        ' 'Cmd.CommandText = "update BillAccum set attendedTo=1,isbilled=1 where SNO =" & strConIDVal)
        ' 'Cmd.Execute
        '
        'Call Tranx(connTran, sysDate, strPatient, strCon, dblPrf, strCoy, strNHIS, 1)
        '
        '
        '
        '
        ''and then
        'Dim PNo As String
        'Dim rsVer As New Recordset
        'rsVer.Open "select  PNo,Subtotal from vwBillingProcessDetailsGrouped where billno='" & strCon & "'", connTran, adOpenStatic, adLockOptimistic
        'If Not rsVer.EOF Then
        '    subAmt = rsVer!SubTotal
        '    PNo = rsVer!PNo & ""
        '
        '    'dblBill = SubTotal
        '    rsVer.Close
        '    Call getValInWord(subAmt)
        '
        '
        '
        '    Dim rsBilling As New Recordset
        '    rsBilling.Open "select * from Billing where billno = '" & strCon & "'", connTran, adOpenStatic, adLockOptimistic
        '    If Not rsBilling.EOF Then
        '        cmd.CommandText = "update  billing set AmountBilledInWord='" & strWord & "', amountBilled= " & subAmt & " where billno = '" & strCon & "'"
        '        cmd.Execute
        '    End If
        '
        'End If
        '

connTran.CommitTrans

On Error GoTo errH:

Call Auditrail(m_Username, "insert Service/NHIS  (" & txtNHIS.Text & "%) fee for Drugs/Services for " & strName, strCon, (subAmt), strHostName)
'dblProf = CDbl(txtNHIS.Text)
            
    isbillAdjust = True
    adjustTo = ""
    adjustTo = strNHIS
    
    
 
    
'''''''''''''''''''''''''''''''''''''''''''''''''''''''

    Call getAccumBill

Exit Sub
errH:
MsgBox Err.Description

Exit Sub

TransFail:
connTran.RollbackTrans
MsgBox Err.Description

End Sub

Public Sub cmdOK_Click()
On Error GoTo errH

If Trim(txtSearch.Text) = "" Then
    MsgBox "Specify Bill No"
    txtSearch.SetFocus
    Exit Sub
End If

'If Not IsNumeric(txtSearch.Text) Then ' OFL exists
'    MsgBox "Character NOT allowed for Bill No "
'    txtSearch.SetFocus
'    Exit Sub
'End If


strSrearch = ""
strSrearch = Trim(txtSearch.Text)

'If IsNumeric(Mid(strSrearch, 1, 6)) Then 'cos of billNo with OFL
'    strSrearch = Right("000000000000" & strSrearch, 12)
'End If

txtSearch.Text = strSrearch

'''search with receiptNo
If InStr(strSrearch, "RN") > 0 Then
    Dim rsRct As New Recordset
    With rsRct
        .Open "select BillNo from Payments where receiptNo ='" & strSrearch & "'", conStr, adOpenForwardOnly, adLockReadOnly
        If Not .EOF Then
            strSrearch = !billNo & ""
        Else
            MsgBox "Invalid receipt No", vbCritical
            txtSearch.SetFocus
            Exit Sub
        End If
    End With
End If

'txtSearch.Text = strSrearch



isAdmission = False
isBillNo = True

Call getPatForBill


Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub PrintFromPOS()
On Error GoTo errH
Screen.MousePointer = vbHourglass

If Print_From_Small_Printer = "YES" Then
    Select Case cboRct.Text
        Case "SMALL_RECEIPT"
            Screen.MousePointer = vbDefault
            frmCashReceiptPOS.Hide
            frmCashReceiptPOS.Show vbModal

        Case "SMALL_INVOICE"
           strBillNo = ""
           'strReceiptNo = ""
           fullName = strName
           strBillNo = lblBill.Caption
           strReceiptNo = strReceiptNo   'lblRct.Caption
           'AttndDate = lblDate.Caption
           ''
           
           Dim rctPix As String
           Dim ClinicType As String
           Dim RetainName As String
           Dim recDate As Date
           Dim rs1 As New Recordset
           
           With rs1 'cos of grdReceipt grid
               .Open "select ClinicType,BillDate,recdate,retainCode,AcctID,consultID,clientcat,referal,retainID,RetainName,BillEndDate,policyType,empNo from vwhRecords where consultid ='" & strBillNo & "'", conStr, adOpenForwardOnly, adLockReadOnly
               If Not .EOF Then
                   ClinicType = !ClinicType & ""
                   RetainName = !RetainName & ""
                   recDate = !recDate & ""
               End If
               
               .Close
               .Open "select * from vwAppSettings where ClinicID='" & ClinicID & "'", conStr, adOpenStatic, adLockOptimistic
               If Not .EOF Then
                   RctHead = !idval & ""
                   RctHead2 = !idval2 & ""
                   RctHead3 = !idval3 & ""
                   RctHead4 = !idval4 & ""
               
                   rctPix = !idvalPix & ""
                   'rctPix = !PixName & ""
                   RctPixPath = App.Path & "\" & rctPix
               
                   '''presc info
                    Dim strConCat As String
                    Dim strTreat As String
                    strConCat = "" ''' ok here b4 Print_Prescription
                     If Print_Prescription = "YES" Then
                         strTreat = getTreatment(strBillNo) ''' all records
                         If strTreat <> "" Then
                             strConCat = strConCat & vbNewLine & _
                             "----Prescription-----" & vbNewLine
                              strConCat = strConCat & strTreat & vbNewLine
                         End If
                    
                     End If
                   
                   Dim de1 As New dEcA
                   de1.Connection1.ConnectionString = "Data Source=" & strSVR & ";Initial Catalog=Hospital;User Id=biller;Password=Logic@$$321!;"
                   de1.cmGetBill strBillNo
                   rptBill.Orientation = rptOrientPortrait
                   rptBill.Sections(1).Controls("Label10").Caption = RctHead
                   rptBill.Sections(1).Controls("Label13").Caption = RctHead2 & ", " & RctHead3
                   rptBill.Sections(1).Controls("Label18").Caption = RctHead4
                   rptBill.Sections(1).Controls("Label15").Caption = fullName
                   rptBill.Sections(1).Controls("Label19").Caption = RctDateFroRpt
                   rptBill.Sections(1).Controls("Label22").Caption = ClinicType
               
                   rptBill.Sections(1).Controls("Label5").Caption = strReceiptNo
                   rptBill.Sections(1).Controls("Label16").Caption = strBillNo
               
                   'rptBill.Sections(1).Controls("Image1").Picture = LoadPicture(RctPixPath)
                   'rptBill.Image1.Picture = LoadPicture(RctPixPath)
                   'MsgBox rptBill.Sections(1).Controls(9).Caption
                   
                   'Picture property is an Object so it has to be Set. 'use SET keyword
                   'Also, try not to use indexes but rather names when accessing Sections/Controls/etc...
                   Set rptBill.Sections("ReportHeader").Controls("Image1").Picture = LoadPicture(RctPixPath)
               
                   
                    If Print_Prescription = "YES" Then
                        '''rptBill.Sections("ReportFooter").Controls("lblTreat").Caption = strConCat
                        strConCat = strConCat & vbNewLine & vbNewLine & Space(5) & "------------------------------------" & vbNewLine & Space(10) & "Cashier"  '''m_fullname
                        rptBill.Sections("ReportFooter").Controls("lblCashier").Caption = strConCat   '''& vbNewLine & m_fullname
                    Else
                        rptBill.Sections("ReportFooter").Controls("lblCashier").Caption = m_fullname
                    End If
                  
                   If Print_From_Small_Printer_With_Preview = "YES" Then
                       Screen.MousePointer = vbDefault
                       rptBill.Hide
                       rptBill.Show vbModal
                       
    
                   Else
                       Dim X As Printer ' auto print
                       For Each X In Printers
                         If X.DeviceName = strPrint Then
                            Set Printer = X
                            Screen.MousePointer = vbDefault
                            rptBill.printReport False, rptRangeAllPages
                            Exit For
                        End If
                       Next
                   End If
                   
                   Set de1 = Nothing
               
               Else
               
                   RctHead = ""
                   RctHead2 = ""
                   RctHead3 = ""
                   RctHead4 = ""
                   RctPixPath = ""
                   rctPix = ""
               
               End If
           End With
    
        Case "NORMAL_RECEIPT"
            Screen.MousePointer = vbDefault
            frmCashReceipt.Hide
            frmCashReceipt.Show
        Case Else
            Screen.MousePointer = vbDefault
            frmCashReceiptPOS.Hide
            frmCashReceiptPOS.Show vbModal
    End Select
    
Else
    Screen.MousePointer = vbDefault
    frmCashReceipt.Hide
    frmCashReceipt.Show
End If

'SetButtons True
'Call clearFields
'Call enableFields(False)

Screen.MousePointer = vbDefault
   
Exit Sub

errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub cmdPR_Click()
On Error GoTo errH
If strCoy <> strPrivate Then
    MsgBox "Printing of RECEIPT is for PRIVATE Patients only"
    Exit Sub
End If

If lblBill.Caption = "" Then
    MsgBox "To Print Receipt, Select a patient"
    Exit Sub
End If

    If Print_From_Small_Printer = "YES" Then
        Call PrintFromPOS
        'frmCashReceiptPOS.Hide
        'frmCashReceiptPOS.Show vbModal
    Else
        frmCashReceipt.Hide
        frmCashReceipt.Show

    End If


    'frmCashReceipt.Show
    'Call clearFields
    'cmdPR.Enabled = False
    ''cmdPrint.Enabled = False
    
    'frmReceiptTemp.cboVehNo.Text = cboVehNo.Text
    'frmReceiptTemp.cboVehNo_Click
    ''frmReceiptTemp.cboBill_Click
    'frmReceiptTemp.Hide
    'frmReceiptTemp.Show

Exit Sub

errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub cmdPresc_Click()
On Error GoTo errH
If Trim(cboVehNo.Text) = "" Then
MsgBox " No Patient is specified"
Exit Sub
End If
Dim strPol As String
Dim strZ As String
  Dim rsBL As New Recordset
  Set grdData.DataSource = Nothing
  With rsBL
  .Open "select consultid,prescription,treatedby,policytype from qryhconsulting where consultid='" & strCon & "'", conStr, adOpenStatic, adLockOptimistic
If Not .EOF Then
    If !prescription = "" Then
        MsgBox "No prescription "
    Else
        strZ = ""
        strZ = "Policy/HMO Type: "
        strPol = !PolicyType
            If strPol <> "" Then
                strZ = strZ & strPol
             Else
                strZ = ""
            End If
        
        MsgBox !prescription & vbNewLine & vbNewLine & strZ & vbNewLine & vbNewLine & "Treated by Dr. " & !treatedBy
        getAccumBill
        getDebtForBill (strPatient)
    End If
Else
        getAccumBill
        getDebtForBill (strPatient)
    MsgBox "No prescription "
End If
End With

Set rsBL = Nothing

Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub cmdPrice_Click()
On Error GoTo errH
    If cboVehNo.Text = "" Then
    MsgBox "No Patient is selected"
    Exit Sub
    End If
    
    
'verify if already processed'''''''''''''''''''''''''''''''''''''''
Dim rsVerX As New Recordset
rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its processed
    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    Exit Sub
End If

rsVerX.Close 'Locked Bill
rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
    MsgBox "This Bill is Locked! requires Management Permission to Unlock"
    Exit Sub
End If

intSNo = 0
intSNo = grdData.Columns("SNO")
 Dim rs As New Recordset
 
' rs.Open "select isprocess from billing where billno='" & grdData.Columns("ConsultID") & "'", conStr, adOpenForwardOnly, adLockReadOnly
' If Not rs.EOF Then
'    If rs!isProcess = 1 Then
'        MsgBox "Please Note! This Bill Item is Already Processed!!! Reverse Processing to Adjust"
'        Exit Sub
'    End If
'End If
' rs.Close
 rs.Open "select sno from billingdetails where sno=" & intSNo, conStr, adOpenForwardOnly, adLockReadOnly
 If Not rs.EOF Then
    'MsgBox "This Item cannot be Adjusted!! Already Billed"
    MsgBox "Please Note! This Selected Item is Already Billed"
    'Exit Sub
 End If
    
    
'    OldPrice = grdData.Columns("qty") * grdData.Columns("UnitPrice")
'    OldAdjust = " Item: " & grdData.Columns("service") & " Qty: " & grdData.Columns("qty") & " Price: " & grdData.Columns("UnitPrice")
    'frmAdjust = Me
    
    On Error Resume Next
    
    fullName = Mid(cboVehNo.Text, 1, InStr(cboVehNo.Text, "@") - 2)
    frmPriceAdjustVerify.Label5.Caption = "Adjust Bill for " & fullName
    frmPriceAdjustVerify.txtDrug.Text = grdData.Columns("service")
    frmPriceAdjustVerify.txtQty.Text = grdData.Columns("qty")
    frmPriceAdjustVerify.lblClient.Caption = StrClientCatX     'strpCatID
    frmPriceAdjustVerify.lblCon.Caption = strCon
    frmPriceAdjustVerify.txtPrice.Text = grdData.Columns("UnitPrice") '0
    
    If grdData.Columns("Revtype") = "" Then
        frmPriceAdjustVerify.cboRev.Text = "OTHERS"
    Else
        frmPriceAdjustVerify.cboRev.Text = grdData.Columns("Revtype") '0
    End If
    
    ''''''''''''''''''''''''''''''''''''''''''''''''''
    Dim strCapXX As String
    strCapXX = "NO" 'seed
    If grdData.Columns("Capitated") = "" Then
        'cboCap.AddItem "NO"
        strCapXX = "NO"
    Else
        If StrClientCatX = "HMO" Or StrClientCatX = "PHIS" Or StrClientCatX = "NHIS" Then
            strCapXX = grdData.Columns("Capitated")
        Else
            strCapXX = "NO"
        End If
    End If
    
    frmPriceAdjustVerify.cboCap.Text = strCapXX
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
   
    
    If grdData.Columns("AttendedTo") = "" Then
        'cboCap.AddItem "NO"
        frmPriceAdjustVerify.cboAttend.Text = "NO"
    Else
        frmPriceAdjustVerify.cboAttend.Text = grdData.Columns("AttendedTo")
    End If
    
    If grdData.Columns("BillType") = "" Or grdData.Columns("BillType") = Null Then
        'do nothing
    ElseIf grdData.Columns("BillType") = "LAB" Then
        frmPriceAdjustVerify.cboCat.Text = "INVESTIGATION"
    Else
        frmPriceAdjustVerify.cboCat.Text = grdData.Columns("BillType").Text '), "", grdData.Columns("BillType").Text)
    End If
    
    frmPriceAdjustVerify.Show vbModal

    'Call getDebtForBill(strPatient)  ''this should come first since is Brought forward Bill.dBlBf value will reflect in lBltotal.caption
    
    On Error GoTo errH
    
    Call getAccumBill
    

Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub cmdPrint_Click()
On Error GoTo errH

        'If Not IsNull(dtAdmit.Value) Then
        '    If IsNull(dtDisch.Value) Then
        '        MsgBox "Please Specify Discharge date of Patient and Click Save before Printing"
        '        dtDisch.Value = sysDate
        '        dtDisch.Value = Null
        '        dtDisch.Enabled = True
        '        Exit Sub
        '    End If
        'End If
    
    
        'If IsNull(dtAdmit.Value) Then
        '    If Not IsNull(dtDisch.Value) Then
        '        MsgBox "Discharge date of Patient Requires Admission Date"
        '        dtDisch.Value = sysDate
        '        dtDisch.Enabled = True
        '        dtDisch.Value = Null
        '
        '        dtAdmit.Value = sysDate
        '        dtAdmit.Enabled = True
        '        dtAdmit.Value = Null
        '
        '        Exit Sub
        '    End If
        'End If

If isLockBill = True Then
    MsgBox "There is a Printed bill awaiting Confirmation. Tick to Confirm if Signed or not."
    FrmRptBillsByPat.Hide
    FrmRptBillsByPat.Show
    'With FrmRptBillsByPat
        '.Width = Screen.Width
        '.Height = Screen.Height
        '.Top = 0
        '.Left = 0
    'End With
    FrmRptBillsByPat.WindowState = vbMaximized
    Exit Sub
End If




If dtAttndDate <> vbEmpty Then
    isFromCollatedBill = True
    FrmRptBillsByPat.DTPicker1.Value = dtAttndDate
    FrmRptBillsByPat.DTPicker2.Value = dtAttndDate

    FrmRptBillsByPat.cboCust.Clear
    FrmRptBillsByPat.cboCust.AddItem ""
    FrmRptBillsByPat.cboCust.AddItem gName & " @" & dtAttndDate & " BillTo: " & strCompany & " [" & strCon & "]"
    FrmRptBillsByPat.cboCust.Text = gName & " @" & dtAttndDate & " BillTo: " & strCompany & " [" & strCon & "]"
    FrmRptBillsByPat.cboCust_Click
Else
    isFromCollatedBill = False
    MsgBox "Invalid Attendance Date! Please ReSelect Patient Name"
    Exit Sub
End If

Select Case StrClientCatX
Case "HMO", "NHIS", "PHIS"
    'FrmRptBillsByPat.cmdDisplayNoDebt_Click
    FrmRptBillsByPat.cmdDisplay_Click
Case "PRIVATE", "MTHLY"
    FrmRptBillsByPat.cmdAdmInv_Click
        'Case "MTHLY"
        '    'FrmRptBillsByPat.cmdAdmInvNoDebt_Click
        '    FrmRptBillsByPat.cmdAdmInv_Click
Case Else
    FrmRptBillsByPat.cmdDisplay_Click
End Select


FrmRptBillsByPat.Hide
FrmRptBillsByPat.Show


Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub checkAndMoveToBilling()
'  Dim rsBLb As New Recordset
'  With rsBLb
'  cboVehNo.Clear
'  .Open "select consultID from qryBillAccum where consultID='" & strCon & "'", conSTR, adOpenForwardOnly, adLockReadOnly
'If Not .EOF Then
'call
'Loop
'End If
'End With
'Set rsBLb = Nothing

End Sub





Private Sub cmdRefresh_Click()
'clearFields
'Call Form_Load
blnSave = False 'nece
Call getAccumBill
Call getRctValue 'nece to correct duplicate entry
'Call genIDNo
enableFields (True)
End Sub

Private Sub cmdRev_Click()
frmRevHeaders.Hide
frmRevHeaders.Show vbModal
End Sub

Private Sub cmdToday_Click()
On Error GoTo errH

If IsNull(DTAttnd1.Value) Or IsNull(DTAttnd2.Value) Then
    MsgBox "Please specify Attendance Date Range"
    DTPicker1.SetFocus
    Exit Sub
End If

If cboGroup.Text = "" Then    'Or cbogroup.ListIndex = -1 Then Exit Sub
    cboGroup.Text = "(ALL)"
    'MsgBox "Specify Company"
    'cboGroup.SetFocus
    'Exit Sub
End If
'optHide = 0
'isFromAttendGrid = False
Screen.MousePointer = vbHourglass


lblTGenCap.Visible = False
lblTGen.Visible = False

lblTAmount.Caption = 0
lblAmountPaid.Caption = 0
lblBalance.Caption = 0



Dim rsVal As New Recordset
Set grdAttend.DataSource = Nothing
grdAttend.Caption = "Attendance For Today"
With rsVal
    Dim ssQL As String
    .CursorLocation = adUseClient
    
    If cboGroup.Text = "(ALL)" Then
        .Open "select sum(AmountBilled) as AmountBilled,sum(AmountPaid) as AmountPaid,sum(AmountBilled-AmountPaid) as Balance FROM  vwhRecordsAndBill where recDate between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'", conStr, adOpenStatic, adLockOptimistic
    Else
        .Open "select sum(AmountBilled) as AmountBilled,sum(AmountPaid) as AmountPaid,sum(AmountBilled-AmountPaid) as Balance  FROM  vwhRecordsAndBill where RetainCode = '" & AttndCoy & "' and recDate between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'", conStr, adOpenStatic, adLockOptimistic
    End If
        
    If Not .EOF Then
        lblTAmount.Caption = IIf(IsNull(!AmountBilled), 0, FormatNumber(!AmountBilled, 2))
        lblAmountPaid.Caption = IIf(IsNull(!AmountPaid), 0, FormatNumber(!AmountPaid, 2))
        lblBalance.Caption = IIf(IsNull(!Balance), 0, FormatNumber(!Balance, 2))
    
    Else
        lblTAmount.Caption = 0
         lblAmountPaid.Caption = 0
         lblBalance.Caption = 0
    End If

    .Close
        
    
    If cboGroup.Text = "(ALL)" Then
        ssQL = "select  ROW_NUMBER() OVER (ORDER BY  FullName) AS SNo,recDate as Date,htime as Time,FullName,ClinicType as Clinic,Remarks as Purpose,AmountBilled,AmountPaid,RetainName as Company,ConsultID as BillNo,PhoneNo from vwhRecordsAndBill where recDate between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'"
    Else
        ssQL = "select  ROW_NUMBER() OVER (ORDER BY  FullName) AS SNo,recDate as Date,htime as Time,FullName,ClinicType as Clinic,Remarks as Purpose,AmountBilled,AmountPaid,RetainName as Company,ConsultID as BillNo,PhoneNo from vwhRecordsAndBill where RetainCode='" & AttndCoy & "' and recDate between '" & DTAttnd1.Value & "' and '" & DTAttnd2.Value & "'"
    End If
    
    .Open ssQL, conStr, adOpenStatic, adLockOptimistic
    'MsgBox ssQL
    If Not .EOF Then
        Set grdAttend.DataSource = Nothing
        Set grdAttend.DataSource = rsVal
        grdAttend.Columns("SNo").Width = 400
        grdAttend.Columns("Date").Width = 1000
        grdAttend.Columns("Time").Width = 1000
        grdAttend.Columns("AmountBilled").NumberFormat = "#,###.00"
        grdAttend.Columns("AmountBilled").Alignment = dbgRight
        
        grdAttend.Columns("AmountPaid").NumberFormat = "#,###.00"
        grdAttend.Columns("AmountPaid").Alignment = dbgRight
        
        'grdAttend.Columns("coyname").Visible = False
    Else
        Set grdAttend.DataSource = Nothing
    End If
End With
Set rsVal = Nothing

Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub cmdUnBilled_Click()
On Error GoTo errH
    If cboVehNo.Text = "" Then
        MsgBox "No Patient is selected"
        Exit Sub
    End If
    
    
Dim strRS As String
 Dim rs As New Recordset
 
 'rs.Open "select SNo,DrgName from billAccum where  consultID='" & strCon & "' and SNo not in (Select SNo from BillingDetails)", conStr, adOpenStatic, adLockOptimistic
 rs.Open "select SNo,DrgName from billAccum where attendedTo = 0 and consultID='" & strCon & "' and SNo not in (Select SNo from BillingDetails)", conStr, adOpenStatic, adLockOptimistic
 If Not rs.EOF Then
    rs.MoveFirst
    Do While Not rs.EOF
        strRS = strRS & rs!drgName & vbNewLine
        rs.MoveNext
    Loop
    
    MsgBox strRS
Else
    MsgBox "All Items Billed"
End If
Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub Form_Load()
On Error GoTo errH

'If AcctPostOn = True Then
'    chkRefund.Visible = False
'    lblRefund.Visible = False
'Else
'    chkRefund.Visible = True
'    lblRefund.Visible = True
'End If


SSTab1.Tab = 0

'dtAdmit.Value = sysDate
'dtDisch.Value = sysDate

'dtAdmit.Value = Null
'dtDisch.Value = Null


DTAttnd1.Value = sysDate
DTAttnd2.Value = sysDate


'dtSys = getSysDateTime

dtRctDate.Value = sysDate 'Format(dtSys, "Short Date") 'sysDate
'dtRctDate.Value = ""



DTPicker1.Value = sysDate
DTPicker2.Value = sysDate

strBillAdm = ""
dblMed = 0

cboPayFor.Clear
cboPayFor.AddItem "CARD"
cboPayFor.AddItem "CONSULTING"
cboPayFor.AddItem "CONSULTING AND DRUGS"
cboPayFor.AddItem "CONSULTING AND GLASSES"
cboPayFor.AddItem "CONSULTING AND EYE SERVICES"
cboPayFor.AddItem "CONSULTING AND DENTAL SERVICES"
cboPayFor.AddItem "DEPOSIT"
cboPayFor.AddItem "DEBT"
cboPayFor.AddItem "DRUGS"
cboPayFor.AddItem "GP REVIEW"
cboPayFor.AddItem "SPECIALIST REVIEW"
cboPayFor.AddItem "ANTE NATAL"
cboPayFor.AddItem "EXAMINATION"
cboPayFor.AddItem "ADMISSION"
cboPayFor.AddItem "GP FIRST VISIT"
cboPayFor.AddItem "SPECIALIST FIRST VISIT"
cboPayFor.AddItem "CONSULTING AND INJECTION"
cboPayFor.AddItem "CONSULTING AND DRESSING"
cboPayFor.AddItem "CONSULTING AND INJECTION"

cboPayFor.Text = ""



cboPay.Clear

'cboPay.Clear

cboPay.AddItem "CASH"
cboPay.AddItem "CHEQUE"
cboPay.AddItem "POS"
cboPay.AddItem "OTHERS"

'cboPay.Text = "CASH"

        cboHMO.Clear
        cboGroup.Clear
  
    cboHMO.AddItem "(ALL)"
    cboGroup.AddItem "(ALL)"
  
  Dim rsBL As New Recordset
  With rsBL
  If is_Private_Patient = True Then
    'SSTab1.Tab = 0
    '.Open "select distinct retainName,retainID,retainCode from vwhretainerShip where retainID='" & strPrivate & "' order by retainNAME ", conStr, adOpenForwardOnly, adLockReadOnly
    .Open "select distinct retainName,retainID,retainCode from vwhretainerShip where Category='" & ClientCatPrivate & "' order by retainNAME ", conStr, adOpenForwardOnly, adLockReadOnly
  Else
    'SSTab1.Tab = 0
    .Open "select distinct retainName,retainID,retainCode from vwhretainerShip order by retainNAME ", conStr, adOpenForwardOnly, adLockReadOnly
  End If
    
    If Not .EOF Then
        .MoveFirst
        Do While Not .EOF
        cboHMO.AddItem !RetainName & "[" & !retainID & "]"
        cboGroup.AddItem !RetainName & "[" & !retainID & "]"
        
        'cboHMO.ItemData(cboHMO.NewIndex) = !retainID
        .MoveNext
        Loop
    End If
    
End With

cboGroup.Text = "(ALL)"

If is_Private_Patient = True Then
    If cboHMO.ListCount > 1 Then
        cboHMO.Text = cboHMO.List(0)
    Else
        '''cboHMO.Text = cboHMO.List(1)
    End If
Else
    cboHMO.Text = "(ALL)"
    cboGroup.Text = "(ALL)"
End If

    If is_Private_Patient = True Then
        lblScreen.Caption = "Generate Cash Receipt"
    Else
        lblScreen.Caption = "Generate Bill"
    End If

'
'  Dim rsBLV As New Recordset
'  With rsBLV
'  cboClinic.Clear
'
'  .Open "select ClinicID from clinicTypes", conStr, adOpenStatic, adLockOptimistic
'If Not .EOF Then
'.MoveFirst
'Do While Not .EOF
'cboClinic.AddItem !clinicID
'.MoveNext
'Loop
'cboClinic.Text = "OUT-PATIENT"
'End If
'End With
''Set rsBL = Nothing
'Set rsBLV = Nothing
''
'

    cboBank(1).Clear
    cboBank(1).AddItem ""
    cboBank(2).Clear
    cboBank(2).AddItem ""
    
    cboBank(3).Clear
    cboBank(3).AddItem ""
    
    
    
    Dim rsRev As New Recordset
    With rsRev
    If AcctPostOn = True Then
        .Open "select distinct AccountName,AccountNo from vwAccountsInfo where groupID='" & Acct_Banks & "' order by AccountName", conStrAccts, adOpenForwardOnly, adLockReadOnly
    Else
        .Open "select distinct BankName as AccountName,AcctID as AccountNo from vwBanks where status='BRANCH' order by BankName", conStr, adOpenForwardOnly, adLockReadOnly
    End If
    
    If Not .EOF Then
        .MoveFirst
        Do While Not .EOF
            cboBank(1).AddItem !AccountName & " [" & !AccountNo & "]"
            cboBank(2).AddItem !AccountName & " [" & !AccountNo & "]"
            cboBank(3).AddItem !AccountName & " [" & !AccountNo & "]"
            .MoveNext
        Loop
    End If
    
    End With

Dim rsUsersForAccts As New Recordset
rsUsersForAccts.Open "select username from vwUsers where loginrole='MANAGEMENT' and username ='" & m_Username & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsUsersForAccts.EOF Then
    cmdAudit.Visible = True
Else
    cmdAudit.Visible = False
End If

Set rsUsersForAccts = Nothing

cboRct.Clear
cboRct.AddItem "NORMAL_RECEIPT"
cboRct.AddItem "SMALL_RECEIPT"
cboRct.AddItem "SMALL_INVOICE"

'''''cboRct must have def value
If strPrint = "(NORMAL RECEIPT)" Then '''"(SMALL RECEIPT)"
    cboRct.Text = "NORMAL_RECEIPT"
Else
    cboRct.Text = "SMALL_INVOICE"
    
End If


enableFields False
Call SetButtons(True)


''subclass the datagrid control
'lpPrevWndProc = SetWindowLong(grdAttend.hWnd, GWL_WNDPROC, AddressOf WndProc)


Exit Sub
errH:
MsgBox Err.Description
End Sub


Private Sub loadbanks()
On Error GoTo errH
    
    cboBank(1).Clear
    cboBank(1).AddItem ""
    cboBank(2).Clear
    cboBank(2).AddItem ""
    
    cboBank(3).Clear
    cboBank(3).AddItem ""
    
    Dim rsRev As New Recordset
    With rsRev
        If AcctPostOn = True Then
            Select Case cboPay.Text
                Case "CASH"
                    'cboBank.AddItem "(PETTY CASH)" & " [" & AcctNo_PettyCash & "]"
                    .Open "select SNo,AccountNo,AccountName from vwAccountsInfo where groupID='" & Acct_Cash & "'  order by AccountName ", conStrAccts, adOpenForwardOnly, adLockReadOnly
                Case "CHEQUE", "TRANSFER"
                    .Open "select SNo,AccountNo,AccountName from vwAccountsInfo where groupID='" & Acct_Banks & "'  order by AccountName ", conStrAccts, adOpenForwardOnly, adLockReadOnly
                Case Else
                    .Open "select SNo,AccountNo,AccountName from vwAccountsInfo where groupID='" & Acct_Banks & "'  order by AccountName ", conStrAccts, adOpenForwardOnly, adLockReadOnly
            End Select
        Else
            .Open "select distinct BankName as AccountName,AcctID as AccountNo from vwBanks order by BankName", conStr, adOpenForwardOnly, adLockReadOnly
        End If
        
        If Not .EOF Then
            .MoveFirst
            Do While Not .EOF
                cboBank(1).AddItem !AccountName & " [" & !AccountNo & "]"
                cboBank(2).AddItem !AccountName & " [" & !AccountNo & "]"
                cboBank(3).AddItem !AccountName & " [" & !AccountNo & "]"
                .MoveNext
            Loop
        End If
    
    End With
    
    
    
Exit Sub
errH:
MsgBox Err.Description

End Sub


Public Sub getPatForBill()
On Error GoTo errH
Dim X As Integer
  Call clearFields
  
  cboVehNo.Clear
  cboVehNo.AddItem ""
  
    Dim rsBLb As New Recordset
  Dim rsBLV As New Recordset
  Dim Cmd As New Command
  Dim conn As New Connection
  conn.ConnectionString = conStr
  conn.Open
  Cmd.ActiveConnection = conn
  Cmd.CommandType = adCmdText
  Cmd.CommandTimeout = 600
    
    
    
    
    
    'With rsBLb
    
    
    If isAdmission = True Then
        Cmd.CommandText = "select distinct  AdmDate as Date,'Admission' as Clinic,Company as Coyname,pno,fullname,consultID from qryhAdmission"
    Else
  
        If isBillNo = True Then
            Cmd.CommandText = "select distinct  Date,Clinic,coyname,pno,fullname,consultID from vwhConsultingVisitsList2 where consultid ='" & strSrearch & "'"
        Else
            If cboHMO.Text = "(ALL)" Or cboHMO.Text = "" Then
                If chkAll.Value = vbChecked Then
                    Cmd.CommandText = "select distinct  Date,Clinic,coyname,pno,fullname,consultID from vwhConsultingVisitsList2 where date between '" & DTPicker1.Value & "' AND '" & DTPicker2.Value & "'"
                Else
                    Cmd.CommandText = "select distinct  Date,Clinic,coyname,pno,fullname,consultID from vwhConsultingVisitsList2 where attendedTo=0 and date between '" & DTPicker1.Value & "' AND '" & DTPicker2.Value & "'"
                End If
        
            Else
                If chkAll.Value = vbChecked Then
                    Cmd.CommandText = "select distinct  Date,Clinic,coyname,pno,fullname,consultID from vwhConsultingVisitsList2 where retainID='" & BillCoy & "' and date between '" & DTPicker1.Value & "' AND '" & DTPicker2.Value & "'"
                    Else
                    Cmd.CommandText = "select distinct  Date,Clinic,coyname,pno,fullname,consultID from vwhConsultingVisitsList2 where  retainID='" & BillCoy & "' and  attendedTo=0 and date between '" & DTPicker1.Value & "' AND '" & DTPicker2.Value & "'"
                End If
            End If
        
        End If
            
     End If
     
    '.Open "select distinct DATE,coyname,patno,fullname,consultID from qryBillAccum where billtype='" & strBillAccum & "'", cnn2, adOpenForwardOnly, adLockReadOnly
    'If strBillAccum = "CONSULTING" Then
    '.Open "select distinct patno,psurname,pfirstname,consultID,pcatID from qryBillAccum where billtype='" & strBillAccum & "' and pcatID IN ('PRIVATE','DEFAULT')", cnn2, adOpenForwardOnly, adLockReadOnly
    'Else
    '.Open "select distinct patno,psurname,pfirstname,consultID from qryBillAccum where billtype='" & strBillAccum & "'", cnn2, adOpenForwardOnly, adLockReadOnly
    'End If
X = 1 'to serialise consultations or to specify the time

Set rsBLV = Cmd.Execute

If Not rsBLV.EOF Then
    'rsBLV.MoveFirst
    Do While Not rsBLV.EOF
        cboVehNo.AddItem rsBLV!fullName & " @" & rsBLV!Date & " " & rsBLV!Clinic & " - " & rsBLV!coyName & " [" & rsBLV!consultID & "]" & "#" & rsBLV!PNo
        'cboVehNo.ItemData(cboVehNo.NewIndex) = !ID
        rsBLV.MoveNext
    Loop
Else
    If isBillNo = True Then
        isBillNo = False
        MsgBox "No Record/Transaction for this Bill No"
        clearFields
        Exit Sub
    End If
End If
'End With
Set rsBLb = Nothing

If cboVehNo.ListCount > 1 And isBillNo = True Then 'only "" and  one rec expected
    cboVehNo.Text = cboVehNo.List(1)
End If

isBillNo = False
Call enableFields(True)
SetButtons False





Exit Sub

errH:
MsgBox Err.Description

End Sub

Private Sub Form_Resize()
Me.Top = 0  '-450
Me.Left = (Screen.Width - Me.Width) / 2
End Sub




Private Sub grdAttend_DblClick()
On Error GoTo errH

If grdAttend.Caption = "UnBilled Items" Then
    isFromHidden = True
Else
    isFromHidden = False
End If


Dim strCon2 As String
SSTab1.Tab = 0
strCon2 = grdAttend.Columns("BillNo").Text
txtSearch.Text = strCon2
    
isFromAttendGrid = True
Call cmdOK_Click
    
Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub grdData_dblClick()
Call cmdPrice_Click

End Sub













Private Sub grdDataRct_dblClick()
On Error GoTo errH

    strReceiptNo = grdDataRct.Columns("ReceiptNo").Text
    RctDateFroRpt = grdDataRct.Columns("ReceiptDate").Text
    
    If Print_From_Small_Printer = "YES" Then
        Call PrintFromPOS
        'frmCashReceiptPOS.Hide
        'frmCashReceiptPOS.Show vbModal
    Else
        frmCashReceipt.Hide
        frmCashReceipt.Show

    End If

Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub lblBillPayable_Change()
On Error GoTo errH
If Trim(lblBillPayable.Caption) = "" Then Exit Sub
'txtEmp.Text = CDbl(lblPay.Caption)
Dim objCon As New figToWrd
Dim strWord As String
strWord = objCon.Num2String(CDbl(Replace(lblBillPayable.Caption, ",", "")))
txtDuty.Text = UCase(strWord)
Set objCon = Nothing
Exit Sub
errH:
MsgBox Err.Description
End Sub




Private Sub lblDebt_DblClick()
    On Error GoTo errH
    If cboVehNo.Text = "" Then
        MsgBox "No Patient is selected"
        Exit Sub
    End If

    If LockDebt = "YES" Then
        Dim rsBLV2 As New Recordset
        With rsBLV2
          .Open "select username from vwusers where loginrole = 'MANAGEMENT' and username='" & m_Username & "'", conStr, adOpenForwardOnly, adLockReadOnly
          If .EOF Then 'not mgt user
              MsgBox "Adjusting Debt requires Management Permission"
              Exit Sub
          End If
        End With
    End If


'verify if already processed'''''''''''''''''''''''''''''''''''''''
Dim rsVerX As New Recordset
rsVerX.Open "select * from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its processed
    MsgBox "This Bill is already processed! to Modify, unProcess the Batch"
    Exit Sub
End If

rsVerX.Close 'Locked Bill
rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
    MsgBox "This Bill is Locked! requires Management Permission to Unlock"
    Exit Sub
End If



            '
            '
            'If strPrivate <> strCoy Then
            '    MsgBox "Only Debt in PRIVATE Bills can be Adjusted! Debt does not Affect HMO/Company Bills", vbInformation, "Debt affects Private Bills Only"
            '    Exit Sub
            'Else
            'End If

             Dim Tran As New Recordset
             With Tran 'use hrecords not Billing
                 '.Open "select top 1 RecDate,ConsultID from hrecords where PNo='" & strPatient & "' and coyName='" & strPrivate & "' order by recID Desc", conStr, adOpenStatic, adLockOptimistic
                 .Open "select top 1 RecDate,ConsultID from hrecords where PNo='" & strPatient & "' order by recID Desc", conStr, adOpenStatic, adLockOptimistic
                 If Not .EOF Then
                    If strCon <> !consultID & "" Then '!consultID last tran no
                        MsgBox "This Bill is not the last Transaction of this Patient. Only Last Transaction Bill can be used to Adjust Debt!" & vbNewLine & "Open the last Bill of this Patient. The Bill No is: " & !consultID & " Dated: " & !recDate, vbInformation, "Debt Adjustment"
                        Exit Sub
                    Else
                        'last tran 'carry on
                    End If
                 End If
            End With

            'If strPrivate <> strCoy Then 'only pvt bill reflects in tranx tbl 'so split bill with empty items to use for debt adj
            '    MsgBox "To Adjust the Private Debt of this HMO/Company Patient," & vbNewLine & "Finish with this Bill and then use 'Split Bill' Page to Adjust or Pay the Debt" & vbNewLine & "Note: You Cannot Adjust Debt OR make Payment on a HMO/Company Bill", vbInformation, "Debt Adjustment"
            '    Call cmdSplit_Click 'rem so as to finish with this bill
            '    Exit Sub
            'End If



    Dim subAmt As Double
    subAmt = 0


Dim dblDebt As Double
Dim strDebt As String
Dim oldDebt As Double
oldDebt = CDbl(lblDebt.Caption)

strDebt = InputBox("Adjust Debt", "Adjust Debt", "0")

If IsNumeric(Trim(strDebt)) Then
    dblDebt = CDbl(strDebt)
Else
    'MsgBox "Enter a Figure"
    Exit Sub
End If


If dblDebt = oldDebt Then
    MsgBox "Specified Value same as Debt", vbInformation
    Exit Sub
End If

If dblDebt < 0 Then
    MsgBox "Debt cannot be less than Zero"
    Exit Sub
End If

Dim dblPrf As Double


      On Error GoTo TransFail

    Dim Cmd As Command
    Set Cmd = New Command
    Dim connTran As New Connection
    connTran.ConnectionString = conStr
    connTran.Open

    Cmd.ActiveConnection = connTran
    Cmd.CommandType = adCmdText


      connTran.BeginTrans

      Dim dblDisc As String
      Dim dblAmtPaid2 As Double
      Dim dblBillPayable As Double
      Dim totAmountBilled As Double
      Dim dblDiscAmt As Double




      dblAmtPaid2 = CDbl(lblDep.Caption)
      totAmountBilled = CDbl(lblTotal.Caption) 'shld come b4 discount



        dblDiscAmt = CDbl(lblDiscount.Caption)
        dblBillPayable = (totAmountBilled + dblDebt) - dblDiscAmt


                Dim debtRemarks As String
                debtRemarks = "Debt Reviewed from " & FormatNumber(oldDebt, 2) & " to " & FormatNumber(dblDebt, 2) & " by " & m_fullname & " on " & sysDate & ":" & sysTime

             If dblDebt = 0 Then

                    Cmd.CommandText = "Update Billing set DebtBF=0 where BillNo  = '" & strCon & "'"
                    Cmd.Execute 'no need anyway
                    
                    
                    Cmd.CommandText = "Update hPatients set debt=0,debtBF=0, TranStartDateForDebt = '" & sysDate & "',LastCheckDateForDebt = '" & sysDate & "' where PNo  = '" & strPatient & "'"
                    Cmd.Execute
                    
                    '
                    Cmd.CommandText = "Delete from Tranxaction where PNo  = '" & strPatient & "'"
                    Cmd.Execute

                    Cmd.CommandText = "Delete from hDebtReview where PNo  = '" & strPatient & "'"
                    Cmd.Execute



                    'Call RemoveDebtByPat(connTran, strPatient, strCon)
                    'Call ReloadTranxForLastBillNo(connTran, strPatient, strCon) ' nece for this current last Bill


                    Call Auditrail(m_Username, "Adjust Debt of " & FormatNumber(oldDebt, 2) & " to 0 for " & strName, strCon, debtRemarks, strHostName)
            
            Else

                    Cmd.CommandText = "Update hPatients set debt=" & dblDebt & " ,debtBF=" & dblDebt & ", TranStartDateForDebt = '" & sysDate & "',LastCheckDateForDebt = '" & sysDate & "' where PNo  = '" & strPatient & "'"
                    Cmd.Execute

                    Cmd.CommandText = "Update Billing set DebtBF=" & dblDebt & "  where BillNo  = '" & strCon & "'"
                    Cmd.Execute

                    'Call RemoveDebtByPat(connTran, strPatient, strCon)
                    'Call Tranx(connTran, sysDate, strPatient, strCon, (dblDebt), strBillTo, debtRemarks, 1, "DEBT")
                    'Call ReloadTranxForLastBillNo(connTran, strPatient, strCon) ' nece ' remove and then add latest debt'

                        
                        '''''''insert into hDebtReview after RemoveDebtByPat has removed all recs for this Pat in hDebtReview'''''''''''

                        Dim rsDetails As New Recordset
                        With rsDetails

                            .Open "select * from hDebtReview where 1=2", connTran, adOpenStatic, adLockOptimistic
                            .AddNew
                            !billNo = strCon
                            !PNo = strPatient 'now returned
                            !DtDate = sysDate 'value Date
                            !Debt = oldDebt
                            !adjustTo = dblDebt
                            !Remarks = debtRemarks
                            .Update
                        End With

                    Call Auditrail(m_Username, "Adjust Debt for " & strName, strCon, debtRemarks, strHostName)

            End If


            connTran.CommitTrans



    'debt has no PostToAccounts

    Call UpdatePay(Me, strCon)



Exit Sub

TransFail:
connTran.RollbackTrans
MsgBox Err.Description

Exit Sub
errH:
MsgBox Err.Description


End Sub



Private Sub OKButton_Click()

On Error GoTo errH

Dim intSave As Integer
Dim connTran As New Connection
Dim strTrks As String
Dim flgOmit As Boolean

''''check to ensure all  details are entered
blnSave = False
isSaved = False 'also ok here
'''''''''''''''

If cboVehNo.Text = "" Then
    MsgBox "Please Select Patient Name"
    Exit Sub
End If

'''' ok, already remarked in cboveh_click ####'''''''''''''''''''''''
If (strCoy = strPrivate Or StrClientCatX = ClientCatPrivate) Then    '''strCoy = strPrivate Then
    'fraReceipt.Enabled = True
    'chkRct.Value = vbChecked
    
    If strPrint = "" And Print_From_Small_Printer = "YES" Then
        MsgBox "Please specify POS Printer for printing"
        Call MdiSapid.mnuPrint_Click
        Exit Sub
    End If
    
    '''deposit
    If CDbl(txtEmp.Text) <= 0 And CDbl(txtHt.Text) > 0 Then
        MsgBox "No Bill to process? Deposit is Allowed", vbInformation
        cboPayFor.Text = "Deposit"
        'SSTab1.Tab = 1
        'Exit Sub
    End If
        
    'If Print_From_Small_Printer = "YES" Then
        'If CDbl(txtEmp.Text) <= 0 Then '' nnow above
        ''    MsgBox "No Bill to process? Enter Deposit in Receipt section if any"
        ''    SSTab1.Tab = 1
        ''    Exit Sub
        'Else
        '    ' no payment action yet
        '    If lblClinic.Caption <> "(IN-PATIENT)" Then
        '        If CDbl(txtEmp.Text) > 0 And CDbl(txtHt.Text) <= 0 Then
        '            'default values already in cboveh_click
        '            chkExact(0).Value = vbChecked 'POS_PayType_Default_Cash = "YES"
        '            cboPayFor.Text = "Medical Services"
        '            'txtHt.Text = FormatNumber(CDbl(txtEmp.Text), 2)
        '            'cboPay.Text = "CASH"
        '        End If
        '    End If
        'End If
    
    'End If
Else
    'chkRct.Value = False
    'fraReceipt.Enabled = False
End If

''''''''''''''''''''''''''''''

    
If lblBill.Caption <> strCon Then
    MsgBox "Please Re-Select Patient Name! Bill No (StrCon needed)"
    cboVehNo.SetFocus
    Exit Sub
End If

If lblBill.Caption = "" Then
    MsgBox "Bill No field cannot be empty"
    cboVehNo.SetFocus
    Exit Sub
End If
 
        ''If isOnAdmit = True And isDisch = False Then 'setting of Adm and Disch dates Allowed
        'If Not IsNull(dtAdmit.Value) Then
        '    If IsNull(dtDisch.Value) And flgAllowPay = False Then
        '        MsgBox "Please Specify Discharge date of Patient OR tick either 'Deposit' OR 'Refund' OR 'Remove from Payment' Checkbox to make an Entry"
        '        dtDisch.Value = sysDate
        '        dtDisch.Value = Null
        '        dtDisch.Enabled = True
        '        Exit Sub
        '    End If
        'End If
        '
        '
        'If IsNull(dtAdmit.Value) Then
        '    If Not IsNull(dtDisch.Value) Then
        '        MsgBox "Discharge date of Patient Requires Admission Date"
        '        dtDisch.Value = sysDate
        '        dtDisch.Enabled = True
        '        dtDisch.Value = Null
        '
        '        dtAdmit.Value = sysDate
        '        dtAdmit.Enabled = True
        '        dtAdmit.Value = Null
        '
        '        Exit Sub
        '    End If
        'End If


'If txtEmp.Text < 0 Then
'    MsgBox "Receipt cannot be Issued!!! Patient Qualifies for REFUND"
'    'txtEmp.SetFocus
'    Exit Sub
'End If


If fraReceipt.Enabled = True Then
   If Trim(txtHt.Text) = "" And chkRct.Value = vbChecked Then
        MsgBox "Please enter Amount Paid"
        isSaved = False 'ok here
        SSTab1.Tab = 1
        Exit Sub
    End If
    
    If txtHt.Text = "0" Or txtHt.Text = "0.00" Then
        MsgBox "Please enter Amount Paid"
        isSaved = False 'ok here
        SSTab1.Tab = 1
        Exit Sub
    End If
    
    'If txtHt.Text = "0" And chkRct.Value = vbChecked Then
    '    MsgBox "Please enter Amount Paid"
    '    isSaved = False 'ok here
    '    SSTab1.Tab = 1
    '    Exit Sub
    'End If
 
    
    If (strCoy = strPrivate Or StrClientCatX = ClientCatPrivate) Then
    
        
            'Dim I As Integer
            'Dim PayType As String
            'For I = 0 To 3
            '    If Trim(txtAmt(I).Text) > 0 Then
            '        PayType = PayType & AmtTot + lblAmt(I).Caption & ","
            '    End If
            'Next
            'cboPay.Text = Mid(PayType, 1, Len(PayType) - 1)
        
            'If cboBill.Text = "" Then
            'MsgBox "Please enter Bill No"
            'Exit Sub
            'End If
            
            'If IsNull(dtRctDate.Value) Then
            '    'MsgBox "Please enter Receipt Date"
            '    dtRctDate.Value = sysDate
            '    'Exit Sub
            'End If
        
        If Trim(cboPayFor.Text) = "" Then
            'cboPayFor.Text = "Medical Services"
            MsgBox "Please enter Payment Purpose (Being Payment for)"
            SSTab1.Tab = 1
            cboPayFor.SetFocus
            Exit Sub
        End If
        
        'If cboReceived.Text = "" Then
        'MsgBox "Please enter Name of Receiver"
        'Exit Sub
        'End If
        
        If cboPay.Text = "" Then
        MsgBox "Please enter Payment type"
        cboPay.SetFocus
        Exit Sub
        End If
        
        If txtAmt(2).Text > 0 Then  'Or txtAmt(3).Text > 0
            If Trim(txtCheque.Text) = "" Then
                MsgBox "Please enter Cheque No"
                txtCheque.SetFocus
                Exit Sub
            End If
        End If
        
        If txtHt.Text = "" Or txtHt.Text = 0 Then
        MsgBox "Please enter Amount Paid"
        txtHt.SetFocus
        Exit Sub
        End If
        
        If isNormalPay = True Then
            Dim AmtHt As Double
            AmtHt = CDbl(txtHt.Text)
            If AmtHt < 0 Then
            MsgBox "Amount Paid Cannot be Negative for Normal Payment"
            txtHt.SetFocus
            Exit Sub
            End If
        End If
        
        
        If Trim(txtEmp.Text) = "" Then txtEmp.Text = 0
            
            
        'If chkDep.Value = False And chkNil.Value = False Then
        '    MsgBox "Please tick Deposit CheckBox to Specify Deposit"
        '    txtEmp.SetFocus
        '    Exit Sub
        'End If
        
        
        If AcctPostOn = True Then
            Dim IZ As Integer
            For IZ = 1 To 3 'cash excluded' ' since Bank to Deposit cash is unknown at this point
                If CDbl(txtAmt(IZ).Text) > 0 And cboBank(IZ).Text = "" Then
                    MsgBox "Specify Bank Name for " & lblAmt(IZ).Caption, vbInformation, "Bank Name"
                    cboBank(IZ).SetFocus
                    Exit Sub
                End If
            Next
       End If
        
       
        'If cboClinic.Text = "" Then
        'MsgBox "Please enter Clinic type"
        'Exit Sub
        'End If
    End If
End If


    'If Len(Trim(txtProf.Text)) <= 0 And Not IsEmpty(lblAdmit.Caption) Then
    '    txtProf.SetFocus
    '    MsgBox "Confirm Prof Fee Entry", vbInformation, "Prof Fee"
    'End If
    
If dtAttndDate = vbEmpty Then
    MsgBox "Invalid Attendance Date! Please ReSelect Patient Name"
    Exit Sub
End If

''''prompt to save if on admission
'If lblClinic.Caption = "(IN-PATIENT)" Then
'    intSave = MsgBox("Are you sure to save? Confirm Prof Fee Entry if Any", vbYesNo, "About to save")
'Else
'    If (strCoy = strPrivate Or StrClientCatX = ClientCatPrivate) Then
'        Dim strPaidX As String
'        Dim strPaid As String
'        strPaidX = txtHt.Text ''FormatNumber(CDbl(txtHt.Text), 2)
'        strPaid = strName & " Paid " & strPaidX
'        lblScreen.Caption = strPaid
'        intSave = MsgBox("Are you sure to save?" & vbNewLine & strPaid, vbYesNo, "About to save")
'    Else ''HMOs
'        intSave = MsgBox("Are you sure to save?", vbYesNo, "About to save")
'    End If
'
'End If

If (strCoy = strPrivate Or StrClientCatX = ClientCatPrivate) Then
    Dim strPaidX As String
    Dim strPaid As String
    strPaidX = txtHt.Text ''FormatNumber(CDbl(txtHt.Text), 2)
    strPaid = strName & " Paid " & strPaidX
    lblScreen.Caption = strPaid
    intSave = MsgBox("Are you sure to save?" & vbNewLine & strPaid, vbYesNo, "About to save")
Else ''HMOs
    intSave = MsgBox("Are you sure to save?", vbYesNo, "About to save")
End If
    
If intSave = vbNo Then
    Exit Sub
End If
    
        

        
        strBillNo = lblBill.Caption ' strcon used instead
        
        If (strCoy = strPrivate Or StrClientCatX = ClientCatPrivate) Then
        
            Call genIDNo 'ok here before beginTrans
            
            If lblRCt.Caption = "" Then
                MsgBox "Invalid Receipt No! System Failed to Generate Receipt"
                Exit Sub
            End If
            
        
        End If
        
        
Screen.MousePointer = vbHourglass
        
        dtSys = getSysDateTime 'dtAttndDate     '
        
        Dim AttdDateSave As Date
        AttdDateSave = CDate(lblDate.Caption)
        
        If isProcess = False And isLock = False Then
            '''''''call updateBill before beginTrans here and UpdatePay below after CommitTrans''''bcos they run in their own tran'''''''''''''''''''''''''''''''''''
            Call updateBill(strCon, strCoy, strPatient)
            
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        End If
        
        
        
        connTran.ConnectionString = conStr
        connTran.Open
        Dim rsDetails As New Recordset
        
        On Error GoTo TransFail
        
        connTran.BeginTrans
        
        Dim dblX As Double
        Dim rsAccum As New Recordset
        Dim rsBLV As New Recordset
        Dim Cmd As New ADODB.Command
        Dim rsRct As New Recordset
        Dim amtRct As Double
        Dim amtRct2 As Double
        'Dim rsRct As New Recordset
        Dim rsIns As New ADODB.Recordset
        Dim dblAmtDue As Double
        Dim dblAmtPaid As Double

        Dim oldDebt As Double
        Dim newDebt As Double
      
        
        
''''''''''''''''''''''''''''''''''''''''begin of billAccum update/insert
             'Dim cmd As New ADODB.Command
             'With cmd
            Cmd.ActiveConnection = connTran
            Cmd.CommandType = adCmdText
            
        If isProcess = False Then  'Or isLock = False
        
            'nece here firstly
            Cmd.CommandText = "update  hrecords set exitDate='" & AttdDateSave & "', BillDate='" & AttdDateSave & "'  where consultID = '" & strCon & "'"
            Cmd.Execute
        
             If lblAdmit.Caption = "" Then 'either adm or disch date can be used ' either both have values or None
                
                Cmd.CommandText = "update  billing set ApprvCode='" & Trim(Replace(txtApprv.Text, "'", "''")) & "', diagnosis='" & UCase(Trim(Replace(txtDiag.Text, "'", "''"))) & "' where billNo = '" & strCon & "'"
                Cmd.Execute 'no update for billing month and year
                
                'already handled in billing insert in updateBill 'done only once there
                'Dim BDate As Date 'billing already exists
                'Dim rsVerX As New Recordset
                'rsVerX.Open "select bDate from billing where billNo='" & strCon & "'", connTran, adOpenForwardOnly, adLockReadOnly
                'If Not rsVerX.EOF Then
                '    BDate = rsVerX!BDate
                'End If
                '
                'cmd.CommandText = "update  hrecords set exitDate='" & AttdDateSave & "', BillDate='" & AttdDateSave & "'  where consultID = '" & strCon & "'"
                'cmd.Execute
            Else
                If lblDisch.Caption = "" Then
                    Cmd.CommandText = "update  billing set ApprvCode='" & Trim(Replace(txtApprv.Text, "'", "''")) & "', diagnosis='" & UCase(Trim(Replace(txtDiag.Text, "'", "''"))) & "', AdmDate='" & CDate(lblAdmit.Caption) & "' where billNo = '" & strCon & "'"
                    Cmd.Execute
                        
                    'cmd.CommandText = "update  hrecords set exitDate='" & dtDisch.Value & "', BillDate='" & dtDisch.Value & "'  where consultID = '" & strCon & "'"
                    'cmd.Execute
                Else
                    Cmd.CommandText = "update  billing set ApprvCode='" & Trim(Replace(txtApprv.Text, "'", "''")) & "', diagnosis='" & UCase(Trim(Replace(txtDiag.Text, "'", "''"))) & "', BillingMonth='" & MonthName(Month(CDate(lblDisch.Caption))) & "', BillingYear='" & Year(CDate(lblDisch.Caption)) & "' ,  AdmDate='" & CDate(lblAdmit.Caption) & "',DischDate='" & CDate(lblDisch.Caption) & " ' where billNo = '" & strCon & "'"
                    Cmd.Execute
                        
                    Cmd.CommandText = "update  hrecords set exitDate='" & CDate(lblDisch.Caption) & "', BillDate='" & CDate(lblDisch.Caption) & "'  where consultID = '" & strCon & "'"
                    Cmd.Execute
                End If
            End If
       End If
       
    '''allow save without payment
    If chkRct.Value = False And strCoy = strPrivate Then
        connTran.CommitTrans
        On Error GoTo errH:
        flg_Enforce_Saving = False
        isReversePay = False
        Call clearFields
        Call enableFields(False)
        SetButtons True
        isAdmission = False
        SSTab1.Tab = 0
        Screen.MousePointer = vbDefault
        MsgBox "Record saved Without Payment", vbInformation
        Exit Sub
    End If
          
        '''''''''receipt saving starts here'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                
            If (strCoy = strPrivate Or StrClientCatX = ClientCatPrivate) Then
                
                'Call genIDNo  'not here ' already up before beginsTranss
                     
                'insert receipt info
                    strRct = ""
                    strRct = lblRCt.Caption
                    entryDate = Format(dtSys, "Short Date")  'sysDate 'server date ''dtRctDate.Value  ' 'nece for PTree 'adjustment can be done in PTree
                    entryTime = Format(dtSys, "short Time")
                    PaymentFor = Trim(cboPayFor.Text)
                    AmountPaid = CDbl(txtHt.Text)
                    AmountDue = CDbl(txtEmp.Text)
                    With rsRct
                         .Open "select * from Payments where 1=2", connTran, adOpenStatic, adLockOptimistic
                         .AddNew
                         !ReceiptDate = entryDate 'dtRctDate.Value    'sysDate ' now sysdate    'dtbilldate 'rct Date 'adjustable to date of receipt
                         !ReceiptNo = strRct
                         !ClinicID = strClinicID 'cboClinic.Text
                         !billNo = strCon
                         !PNo = strPatient
                         !PaymentFor = PaymentFor
                         !AmountBilled = AmountDue 'CDbl(txtEmp.Text)
                         !AmountPaid = AmountPaid
                         !AmountInWord = Trim(txtWord.Text)
                        ' !Balance = CDbl(txtEmp.Text) - CDbl(txtHt.Text)
                         !ReceivedBy = strEmpID 'cboReceived.Text
                         !PayType = cboPay.Text
                         
                         If cboPay.Text = "CHEQUE" Then
                            !ChequeNo = Trim(txtCheque.Text)
                         End If
                         
                         !rTime = entryTime ' Format(dtSys, "short Time")
                         
                         If isReversePay = True Then
                            !Remarks = "REVERSAL"
                         Else
                            !Remarks = "PAYMENT"
                         End If
                         
                         .Update
                         
                         .Close
                         
                        Cmd.CommandText = "update Billing set AmountPaid=isnull(AmountPaid,0)+" & AmountPaid & " where BillNo = '" & strCon & "'"
                        Cmd.Execute
                         
             
                         '''''''''''''''''''''''''Payment Types/Details update'''''''''''''''''''''''''''''''''''''
                         
                         Dim IY As Integer
                         Dim rsType As New Recordset
                         
                         'lblAcctNo(0).Caption = ""
                         'lblAcctNo(1).Caption = ""
                         'lblAcctNo(2).Caption = ""
                         'lblAcctNo(3).Caption = ""
                         
                         
                         'neg amt allowed here ' only for cash 'refund,overpayment mistake etc
                         rsType.Open "select * from PaymentTypes where 1=2", connTran, adOpenStatic, adLockOptimistic
                            For IY = 0 To 3
                                'If IY = 0 Then 'strBCode0 for cash mot nece  ' since Bank to Deposit cash is unknown at this point
                                '    lblAcctNo(IY).Caption = AcctNoCash
                                ' ElseIf IY = 1 Then
                                '    lblAcctNo(IY).Caption = strBCodePOS
                                ' ElseIf IY = 2 Then
                                '       lblAcctNo(IY).Caption = strBCodeCHQ
                                'ElseIf IY = 3 Then
                                '    lblAcctNo(IY).Caption = strBCodeTRF
                                'End If
                            
                                If CDbl(txtAmt(IY).Text) <> 0 Then 'neg amt allowed here ' only for cash field 'refund,overpayment mistake etc
                                    rsType.AddNew
                                    rsType!ReceiptDate = entryDate
                                    rsType!ReceiptNo = strRct
                                    rsType!AmountPaid = CDbl(txtAmt(IY).Text)
                                    rsType!PayType = lblAmt(IY).Caption
                                    
                                    Select Case lblAmt(IY).Caption
                                    Case "CASH"
                                        rsType!AccountNo = AcctNoCash
                                    Case "POS"
                                        rsType!AccountNo = strBCodePOS
                                    Case "CHEQUE"
                                        rsType!AccountNo = strBCodeCHQ
                                    Case "TRANSFER"
                                        rsType!AccountNo = strBCodeTRF
                                    End Select
                                    
                                    If chkRefund.Value = vbChecked Then  'And Voucher_Module_Active = "YES" Then
                                        rsType!suppres = 1
                                    Else
                                         rsType!suppres = 0
                                    End If
                                    
                                   rsType!isPost = 0
                                    
                                    rsType.Update
                                End If
                                
                            Next
                        
                        If AmountPaid > 0 Then 'no neg amt here 'rev heads
                         
                             Dim rsAccts As New Recordset
                             Dim rsPayX As New Recordset
                             Dim K As Integer
                             Dim rctTotal As Double
                             Dim AmtAcct As Double
                             Dim Discount As Double
                             Dim CurrentBill As Double
                             Dim AmountBilled As Double
                             Dim AmountDebt As Double
                             Dim SNo As Long
                             
                             rctTotal = AmountPaid
                             Discount = CDbl(lblDiscount.Caption)
                             CurrentBill = CDbl(lblTotal.Caption)
                             AmountBilled = (CurrentBill - Discount)
                             AmountDebt = (AmountBilled - AmountPaid) 'no prev debt here
                            
                                        'If Discount > 0 Then
                                        '    rctTotal = rctTotal + Discount 'to avoid pat paying debt 'it will = sum(subAmount) in billingDetails
                                        'Else '<=0
                                        '    ''do nothing
                                        '    'rctTotal = rctTotal
                                        'End If
                             
                            With rsPayX
                            .Open "select *  from PaymentDetails where 1=2", connTran, adOpenStatic, adLockOptimistic
                                rsAccts.Open "select SNo,billDate,BillItem,AmtDiff ,revtype,AccountNo from vwhRevenueForAccts where isRct=0 and  billno='" & strCon & "' order by Serial", connTran, adOpenStatic, adLockOptimistic
                                If Not rsAccts.EOF Then
                                    For K = 1 To rsAccts.RecordCount
                                        SNo = rsAccts!SNo
                                        
                                        .AddNew
                                        !snoID = SNo ' rsAccts!SNo
                                        !ReceiptDate = entryDate
                                        !BillDate = rsAccts!BillDate
                                        !ReceiptNo = strRct
                                        !billNo = strCon
                                        
                                        
                                        If rctTotal >= rsAccts!AmtDiff Then
                                            AmtAcct = rsAccts!AmtDiff 'paym complete
                                            Cmd.CommandText = "update billingDetails set AmtPaid=isnull(AmtPaid,0)+" & AmtAcct & " where SNo=" & SNo  'BillNo = '" & strCon & "' and drgName ='" & rsAccts!BillItem & "'"
                                            Cmd.Execute
                                            Cmd.CommandText = "update BillAccum set isRct=1 where SNo=" & SNo  'consultID = '" & strCon & "' and drgName ='" & rsAccts!BillItem & "'"
                                            Cmd.Execute
                                            Cmd.CommandText = "update billingDetails set isRct=1 where SNo=" & SNo  'where billNo = '" & strCon & "' and drgName ='" & rsAccts!BillItem & "'"
                                            Cmd.Execute
                                            
                                        Else
                                            AmtAcct = rctTotal  'paym inComplete
                                            Cmd.CommandText = "update billingDetails set AmtPaid=isnull(AmtPaid,0)+" & AmtAcct & " where SNo=" & SNo  'BillNo = '" & strCon & "' and drgName ='" & rsAccts!BillItem & "'"
                                            Cmd.Execute
                                        End If
                                        
                                        !AmountPaid = AmtAcct
                                        !AmountToPay = rsAccts!AmtDiff
                                        !AccountNo = rsAccts!AccountNo & ""
                                        !revType = rsAccts!revType & ""
                                        !BillItem = rsAccts!BillItem & ""
                                        !isPost = 0
                                        .Update
                                        
                                        
                                        rctTotal = rctTotal - AmtAcct 'rsAccts!AmtDiff
                                        If rctTotal <= 0 Then Exit For
                                        rsAccts.MoveNext
                                    Next
                                    
                                    If rctTotal > 0 Then 'in case of excess payment/Deposit
                                        .AddNew
                                        !snoID = 0  'rsAccts!SNo ' not an item from BillingDetails
                                        !ReceiptDate = entryDate  'entrydate
                                        !BillDate = entryDate  ' rsAccts!BillDate
                                        !ReceiptNo = strRct
                                        !billNo = strCon
                                        !AmountPaid = rctTotal
                                        !AmountToPay = 0  'rctTotal '? 'shld it be zero
                                        !AccountNo = AcctNoSales & ""  'default recv acct
                                        !revType = Rev_Type_Def & ""
                                        !BillItem = Rev_Type_Def_Desc & ""
                                        !isPost = 0
                                        .Update
                                        
                                        'no bill to update in bllingDetails/BillAccum
                                        'Cmd.CommandText = "update BillAccum set isRct=1 where consultID = '" & strCon & "' and revType='OTHERS'"
                                        'Cmd.Execute
                                        'Cmd.CommandText = "update billingDetails set isRct=1 where billNo = '" & strCon & "' and revType='OTHERS'"
                                        'Cmd.Execute
                                    End If
                                
                                Else ' in case of Deposit payment 'also ok
                                        .AddNew
                                        !snoID = 0  'rsAccts!SNo ' not an item from BillingDetails
                                        !ReceiptDate = entryDate  'entrydate
                                        !BillDate = entryDate  ' rsAccts!BillDate
                                        !ReceiptNo = strRct
                                        !billNo = strCon
                                        !AmountPaid = rctTotal
                                        !AmountToPay = 0  'rctTotal '? 'shld it be zero
                                        !AccountNo = AcctNoSales & "" 'default recv acct
                                        !revType = Rev_Type_Def & ""
                                        !BillItem = Rev_Type_Def_Desc & ""
                                        !isPost = 0
                                        .Update
                                
                                End If
                            End With
                        End If
                        
                        
                        
                         ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        If AmountPaid >= 0 Then '-ve AmountPaid stands for refund
                            Call Tranx(connTran, entryDate, strPatient, strCon, -(AmountPaid), BillTo, "Payment for " & PaymentFor & " (" & strRct & ")", 3) 'now -ve for payment
                        Else 'Refund
                            If chkRefund.Value = vbChecked Then
                                Call Tranx(connTran, entryDate, strPatient, strCon, -(AmountPaid), BillTo, "Refund for " & PaymentFor & "  (" & strRct & ")", 3)
                            ElseIf chkNil.Value = vbChecked Then
                                Call Tranx(connTran, entryDate, strPatient, strCon, -(AmountPaid), BillTo, "Payment Reversal: " & PaymentFor & "  (" & strRct & ")", 3)
                            'Else
                                'Call Tranx(connTran, entryDate, strPatient, strCon, -(AmountPaid), BillTo, "Payment For " & PaymentFor & "  (" & strRct & ")", 3)
                            End If
                        End If
                         
                        
                    If AmountPaid >= 0 Then '-ve AmountPaid stands for refund
                        Call Auditrail(m_Username, "insert Payment for: " & strName, strCon, CDbl(txtHt.Text), strHostName)
                    Else 'Refund
                        If chkRefund.Value = vbChecked Then
                            Call Auditrail(m_Username, "Refund for: " & strName, strCon, CDbl(txtHt.Text), strHostName)
                        ElseIf chkNil.Value = vbChecked Then
                            Call Auditrail(m_Username, "Reversed Payment for: " & strName, strCon, CDbl(txtHt.Text), strHostName)
                        Else
                            Call Auditrail(m_Username, "insert Payment for: " & strName, strCon, CDbl(txtHt.Text), strHostName)
                        End If
                    End If
                        
                        
                        
                        
                        If Not IsNumeric(lblAmtDue.Caption) Then lblAmtDue.Caption = 0
                        dblAmtDue = CDbl(lblAmtDue.Caption)
                        dblAmtPaid = CDbl(txtHt.Text)
                    
                    End With ' for rsRct
         
                
            End If 'for if chkRct is checked
        
                Set rsRct = Nothing 'ok here
                
                
                
        '''''update BillDate in hrecords 'det if processing will be defered to next mth or not'''''''''''
        If Has_Bill_End_Date = "YES" Then
            Dim rsProc As New Recordset
            Dim NewBatchNo As Long
            Dim dbBatchNo As Long
            With rsProc 'both pats and ext pats
                .Open "select recDate,BillDate from qryhRecordsUnion where consultID = '" & strCon & "'", connTran, adOpenKeyset, adLockOptimistic
                If Not .EOF Then
                   AttndDate = !recDate
                       If BillEndDate >= DatePart("d", AttndDate) Then   'det which mth  bill will be processed
                           BillDate = AttndDate
                       Else
                           BillDate = CDate("01/" & Month(DateAdd("m", 1, AttndDate)) & "/" & Year(AttndDate))
                           BillDate = Format(BillDate, "Short Date") 'next month
                       
                          'det if batch for  this trans. is already processed
                          'right now BillDate in hRecords is still null' for fresh update
                           'nece since batchNo for processing is based on billDate
                           NewBatchNo = Year(BillDate) & Right("00" & Month(BillDate), 2)
       
                           If rsProc.State = adStateOpen Then rsProc.Close
                           .Open "select  top 1 batchVal,coyCode  from vwBillsForClientsBatchVal where coycode='" & strBillTo & "' order by batchval desc ", conStr, adOpenStatic, adLockOptimistic
                           If Not .EOF Then
                               dbBatchNo = !batchVal
                               If NewBatchNo <= dbBatchNo Then  'det if batch for  this trans. is already processed
                                   BillDate = CDate("01/" & Mid(NewBatchNo, 5) & "/" & Mid(NewBatchNo, 1, 4))
                                   BillDate = Format(DateAdd("m", 1, BillDate), "Short Date") 'increment by 1 'next month
                               Else
                                   'do nothing 'yet to be processed
                               End If
                           End If
                       End If
                       
    
                       Cmd.CommandText = "update  hrecords set BillDate='" & BillDate & "' where consultID = '" & strCon & "'"
                       Cmd.Execute
                       
                       Cmd.CommandText = "update  hRecordsPublic set BillDate='" & BillDate & "' where consultID = '" & strCon & "'"
                       Cmd.Execute
                End If
            End With
        End If
            
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        
        
        If isReversePay = True Then
            Cmd.CommandText = "update  payments set Remarks='REVERSAL' where ReceiptNo = '" & strReceiptNo & "'"
            Cmd.Execute
        End If
        
        
        
        connTran.CommitTrans
        blnSave = True

    
    On Error GoTo errH
            

                         
        '''''''call UpdatePay below after CommitTrans''''''''''''''''''''''''''''''''''''''''
        Call UpdatePay(Me, strCon) 'nece here 'for payment update if any, though already done above 'UpdatePay only updates payment and figures on the form'
        
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                         
                        
        
        If (strCoy = strPrivate Or StrClientCatX = ClientCatPrivate) Then
            strReceiptNo = strRct
            RctDateFroRpt = entryDate
            strCashier = m_fullname   ''"- - -"
            Screen.MousePointer = vbDefault
            
            If strPrint = "(NORMAL RECEIPT)" Then '''"(SMALL RECEIPT)"
                Screen.MousePointer = vbDefault
                frmCashReceipt.Hide
                frmCashReceipt.Show
            Else
                Call PrintFromPOS
                'frmCashReceiptPOS.Hide
                'frmCashReceiptPOS.Show vbModal
            End If

            'MsgBox "Record Succesfully saved!!! Click Print Button to print Bill or Receipt"
            cmdPrint.Enabled = True
            'cmdPR.Enabled = True
        Else
            Screen.MousePointer = vbDefault
            MsgBox "Record Succesfully saved"
            cmdPrint.Enabled = True
            'Call cmdPrint_Click
            'cmdPR.Enabled = False
        End If
        
        
        Set rsDetails = Nothing
        Set rsBLV = Nothing
        Set Cmd = Nothing
    
        isSaved = True
        
        
        If isFromHidden = True Then
            SSTab1.Tab = 2
            Call cmdHidden_Click '  to refresh the UnBilled Items yet to be attended to
        End If
        
        
        
        
        
        '''''''post to Accounts''''''''''''AcctPostType not here but in  PostToAcct sub''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '''''Only Cash dealt with here, AmountDebt = (AmountBilled - AmountPaid) ' will be treated during mth end processng
        '''''no need for AcctNoDebt ''''''''''''''''''''''
  '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If chkRefund.Value = False Then ' And Voucher_Module_Active = "YES" Then
            If AcctPostOn = True And AcctPostType_Cash = "AUTO" And chkRct.Value = vbChecked Then 'debit side of Acct
                    
                    Dim rsAccts2 As New Recordset
                    Dim KX As Integer
                    Dim connTran2 As New Connection
                    connTran2.ConnectionString = conStrAccts
                    connTran2.Open
                    
                Screen.MousePointer = vbHourglass
                    Dim cmd2 As New Command
                    cmd2.ActiveConnection = connTran2
                    cmd2.CommandType = adCmdText
    
                On Error GoTo TransFailAccts
    
                   connTran2.BeginTrans
                    
            
                    Call getTranID(connTran2) 'very nece 'outside the for---next statement of vwRctForAccts
                    Period = getPeriod(connTran2, entryDate) 'entryDate as for rct
                    Dim rsRet As New Recordset
                    With rsRet '''only for private -- cash receipt entry
                        .Open "SELECT AccountNo FROM hospital..vwhRetainership where isnull(AccountNo,'') ='' and retainCode='0001'", connTran2, adOpenForwardOnly, adLockReadOnly
                        If Not .EOF Then
                            Call CreateAccounts(connTran2, "RECEIVABLE") 'after getPeriod
                        End If
                    End With
                    
                'cash'''''''''''''''''''''''''''
                Dim IX As Integer
                For IX = 0 To 3
                    'If IX = 0 Then
                    '    lblAcctNo(IX).Caption = AcctNoCash
                    ' ElseIf IX = 1 Then
                    '    lblAcctNo(IX).Caption = strBCodePOS
                    ' ElseIf IX = 2 Then
                    '       lblAcctNo(IX).Caption = strBCodeCHQ
                    'ElseIf IX = 3 Then
                    '    lblAcctNo(IX).Caption = strBCodeTRF
                    'End If
                            
                     PaymentFor = "" 'nece here
                            
                        If CDbl(txtAmt(IX).Text) <> 0 Then  'neg amt allowed here ' only for cash 'refund,mistake etc
                        Select Case lblAmt(IX).Caption
                        Case "CASH"
                            PaymentFor = "Paid Cash:( Rct No: " & strRct & ")  " & PaymentFor
                            Call PostToAccounts(connTran2, entryDate, AcctNoCash, CDbl(txtAmt(IX).Text), PaymentFor, "ASSET", "h") 'debit
                        Case "POS"
                            PaymentFor = "Paid Via POS:( Rct No: " & strRct & ")  " & PaymentFor
                            Call PostToAccounts(connTran2, entryDate, strBCodePOS, CDbl(txtAmt(IX).Text), PaymentFor, "ASSET", "i")   'debit
                        Case "CHEQUE"
                            PaymentFor = "Paid via Cheque ( Cheq No: " & Trim(txtCheque.Text) & " And Rct No: " & strRct & ") for: " & PaymentFor
                            Call PostToAccounts(connTran2, entryDate, strBCodeCHQ, CDbl(txtAmt(IX).Text), PaymentFor, "ASSET", "i")   'debit side
                        Case "TRANSFER"
                            PaymentFor = "Paid via Transfer. Rct No: " & strRct & ") for: " & PaymentFor
                            Call PostToAccounts(connTran2, entryDate, strBCodeTRF, CDbl(txtAmt(IX).Text), PaymentFor, "ASSET", "i")   'debit side
                        End Select
                    End If
                Next
                    
                'receiable'''''''''''''cash credit'''''''''''
                    PaymentFor = "Payment by " & strName & " Rct No: (" & strRct & ")"
                    Call PostToAccounts(connTran2, entryDate, AccountNo_Recv, -(AmountPaid), PaymentFor, "ASSET", "h") 'credit side
                
                '''''''''''''''''Confirm Dr=Cr'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                
                
                Dim rsAmt As New Recordset
                Select Case AcctPostType
                Case "AUTO"
                    rsAmt.Open "select  dbo.TranBalance('" & Period & "','" & coyID & "') as Amount", connTran2, adOpenStatic, adLockOptimistic
                Case "BATCH"
                    rsAmt.Open "select  dbo.TranBalanceJournal('" & Period & "','" & coyID & "') as Amount", connTran2, adOpenStatic, adLockOptimistic
                End Select
        
                If Not rsAmt.EOF Then
                    If rsAmt!Amount <> 0 Then
                        connTran2.RollbackTrans
                        Call clearFields
                        Call enableFields(False)
                        SetButtons True
                        Exit Sub
                    End If
                Else
                    connTran2.RollbackTrans
                    Call clearFields
                    Call enableFields(False)
                    SetButtons True
                    Exit Sub
                End If
                
                
                cmd2.CommandText = "update " & DBName & "..Payments set isPost=1 where ReceiptNo = '" & strRct & "'"
                cmd2.Execute
                
                cmd2.CommandText = "update " & DBName & "..PaymentDetails set isPost=1 where ReceiptNo = '" & strRct & "'"
                cmd2.Execute
                
                cmd2.CommandText = "update " & DBName & "..PaymentTypes set isPost=1 where ReceiptNo = '" & strRct & "'"
                cmd2.Execute
  
        
                connTran2.CommitTrans
        
            End If ' for posting
        
        End If 'for chkRefund=false
        
        
        'Screen.MousePointer = vbDefault
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
     
        On Error GoTo errH:
        
        
        
        
        '''''''''''''''''''''''''''''''''''''''''''''''
        Screen.MousePointer = vbDefault
        flg_Enforce_Saving = False
        isReversePay = False
        Call clearFields
        Call enableFields(False)
        SetButtons True
        isAdmission = False
    
    'Screen.MousePointer = vbDefault

        'ok after tranx commit
        If chkRefund.Value = vbChecked And Voucher_Module_Active = "YES" Then
            'raise Voucher
            Call genVouchNo
            Call raiseVoucher
        
        End If
        
        
    

Screen.MousePointer = vbDefault
Exit Sub

TransFail:
    Screen.MousePointer = vbDefault
    connTran.RollbackTrans
    MsgBox "Could not Save, re-enter details afresh!!! " & vbCrLf & vbCrLf & Err.Description
    
    Set rsDetails = Nothing
    Set connTran = Nothing

Exit Sub

TransFailAccts:
Screen.MousePointer = vbDefault
    connTran2.RollbackTrans
    MsgBox "Could not Save to Account Module!!! " & vbCrLf & vbCrLf & Err.Description

    Set connTran2 = Nothing
'
'        Call clearFields
'        Call EnableFields(False)
'        SetButtons True
'

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description


End Sub


Public Sub genVouchNo()
On Error GoTo errH
     Dim rsGen As New ADODB.Recordset
     With rsGen
     
     
            .Open "select MAX(cast(SUBSTRING(VouchNo, 3, 7)as bigint))  as ID from hExpense", conStr, adOpenForwardOnly, adLockReadOnly
            If Not .EOF Then
                If IsNull(!ID) Or !ID = 0 Then
                    vouchNo = "VN" & "0000001"
                Else
                   vouchNo = "VN" & Right("0000000" & CStr(!ID + 1), 7)
                End If
            Else
                    vouchNo = "VN" & "0000001"
            End If
                
                 
                
            Set rsGen = Nothing
            
    End With
    
    
  Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
End Sub



Private Sub raiseVoucher()
 On Error GoTo errH
       
        
        Dim catCode As String
        Dim itemCode As String
        'Dim isBYPASS As Boolean
        
        Dim connTranV As New Connection
        Dim strCatV As String
        Dim IntQV As Integer
        Dim strDrV As String
        
        
        Dim CmdV As Command
        Set CmdV = New Command
        connTranV.ConnectionString = conStr
        connTranV.Open
        
        Dim rsExPV As New Recordset
        Dim rsDetailsV As New Recordset
        Dim rsDetailsApprvFirst As New Recordset
        Dim rsPresc As New Recordset
        Dim rsDress As New Recordset
        Dim rsInj As New Recordset
        
        Dim rsStock As New Recordset
        Dim rsUnit As New Recordset
        Dim intID As Long
        Dim rsBLV As New Recordset
        Dim dboTotal As Double
        Dim strIDVal As String
        Dim strConVouch As String
        strConVouch = ""
        strConVouch = vouchNo
        
            Dim rsVV As New Recordset
            With rsVV
                .Open "select CatCode from hExpenseCat where CatName='REFUND'", conStr, adOpenStatic, adLockOptimistic
                If Not .EOF Then
                    catCode = !catCode & ""
                Else
                    MsgBox "No Expense Cat for Redund to Raisse Voucher"
                    Exit Sub
                End If
                
                .Close
                .Open "select ItemCode from hExpenseItems where ItemName='REFUND'", conStr, adOpenStatic, adLockOptimistic
                If Not .EOF Then
                    itemCode = !itemCode & ""
                Else
                    MsgBox "No Expense Item for Redund to Raisse Voucher"
                    Exit Sub
                End If
            End With
            
        On Error GoTo TransFail
        
        connTranV.BeginTrans
        
        rsExPV.Open "hExpense where 1=2", connTranV, adOpenStatic, adLockOptimistic
        rsDetailsV.Open "hExpenseDetails where 1=2", connTranV, adOpenStatic, adLockOptimistic
        rsDetailsApprvFirst.Open "hExpenseDetailsApprvFirst where 1=2", connTranV, adOpenStatic, adLockOptimistic
               
                 rsExPV.AddNew 'for hExpense
                 rsExPV!ExpDate = entryDate
                 rsExPV!ExpTime = entryTime
                 rsExPV!vouchNo = strConVouch
                 
                 rsExPV!Amount = Abs(AmountPaid) 'no -ve
                 'rsExPV!AmountInWord = Trim(txtDuty.Text)
                 rsExPV!Paidby = "XXX"  'strEmpID   ' Trim(cboReason.Text)
                 'rsExPV!Apprvdby = Trim(cboApprvd.Text)
                 rsExPV!ReceivedBy = "PATIENT"
                 
                    'rsDetailsV!retainCode = OrderGrid.TextMatrix(i, 0)
                 
                    rsExPV!PayType = "CASH"
                    'rsExPV!ChequeNo = Trim(txtCheque.Text)
                    'rsExPV!valuedate = Trim(dtValue.Value)
                    'rsExPV!bankcode = Trim(cboBank.Text)
                    'rsExPV!ChequeDate = Trim(dtCheque.Value)
                    rsExPV!Remarks = "REFUND"  'Trim(cboPayee.Text)
                 rsExPV.Update
                 ''''''''''''''''''''''''''''''''''''''''''
'        Dim rsX As New Recordset
'        rsX.Open "select top 1 SNO from hExpense order by SNO desc", connTranV, adOpenForwardOnly, adLockReadOnly
'        strIDVal = rsX!sno
'        Set rsX = Nothing
        
        
        If Voucher_ByPass_For_Refund = "YES" Then
                rsExPV.Close
                rsExPV.Open "hExpenseApprvFirst where 1=2", connTranV, adOpenStatic, adLockOptimistic
               
                 rsExPV.AddNew 'for hExpense
                 rsExPV!ExpDate = entryDate
                 rsExPV!ExpTime = entryTime
                 rsExPV!vouchNo = strConVouch
                 
                 rsExPV!Amount = Abs(AmountPaid)
                 'rsExPV!AmountInWord = Trim(txtDuty.Text)
                 rsExPV!Paidby = "XXX"  'strEmpID   ' Trim(cboReason.Text)
                 rsExPV!Apprvdby = "BYPASSED" 'supplier voucher bypasses the first approval
                 rsExPV!ReceivedBy = "PATIENT"
                 
                    'rsDetailsV!retainCode = OrderGrid.TextMatrix(i, 0)
                 
                    rsExPV!PayType = "CASH"
                    'rsExPV!ChequeNo = Trim(txtCheque.Text)
                    'rsExPV!valuedate = Trim(dtValue.Value)
                    'rsExPV!bankcode = Trim(cboBank.Text)
                    'rsExPV!ChequeDate = Trim(dtCheque.Value)
                    rsExPV!Remarks = "REFUND" 'Trim(cboPayee.Text)
                 rsExPV.Update


        End If


        
        Dim I As Integer
            For I = 1 To 1
                If Voucher_ByPass_For_Refund = "YES" Then
                    rsDetailsV.AddNew
                    rsDetailsV!ExpDate = entryDate
                    rsDetailsV!vouchNo = strConVouch ' Trim(txtVouch.Text)
                    rsDetailsV!ExpName = itemCode
                    rsDetailsV!ExpCat = catCode
                    rsDetailsV!price = Abs(AmountPaid)
                    'intQty = CDbl(OrderGrid.TextMatrix(i, 3))    '''intQty is also used elsewhere
                    rsDetailsV!Qty = 1
                    rsDetailsV!SubTotal = Abs(AmountPaid)
                    rsDetailsV!Description = "Refund to " & strName & "(Bill No: " & strCon & ")"
                    rsDetailsV!attendedto = 1 ' 0 'bypasses the first approval
                    rsDetailsV!isApprv = 1   '0
                    rsDetailsV.Update
                    
                    'supplier voucher bypasses the first approval
                    rsDetailsApprvFirst.AddNew
                    rsDetailsV!ExpDate = entryDate
                    rsDetailsApprvFirst!vouchNo = strConVouch
                    rsDetailsApprvFirst!ExpName = itemCode
                    rsDetailsApprvFirst!ExpCat = catCode
                    rsDetailsApprvFirst!price = Abs(AmountPaid)
                    'intQty = CDbl(OrderGrid.TextMatrix(i, 3))    '''intQty is also used elsewhere
                    rsDetailsApprvFirst!Qty = 1
                    rsDetailsApprvFirst!SubTotal = Abs(AmountPaid)
                    rsDetailsApprvFirst!Description = "Refund to " & strName & "(Bill No: " & strCon & ")"
                    rsDetailsApprvFirst!attendedto = 0
                    rsDetailsApprvFirst!isApprv = 0
                    rsDetailsApprvFirst.Update
                Else
                    rsDetailsV.AddNew
                    rsDetailsV!ExpDate = entryDate
                    rsDetailsV!vouchNo = strConVouch
                    rsDetailsV!ExpName = itemCode
                    rsDetailsV!ExpCat = catCode
                    rsDetailsV!price = Abs(AmountPaid)
                    'intQty = CDbl(OrderGrid.TextMatrix(i, 3))    '''intQty is also used elsewhere
                    rsDetailsV!Qty = 1
                    rsDetailsV!SubTotal = Abs(AmountPaid)
                    rsDetailsV!Description = "Refund to " & strName & "(Bill No: " & strCon & ")"
                    rsDetailsV!attendedto = 0
                    rsDetailsV!isApprv = 0
                    rsDetailsV.Update
                End If
                
     
                
            Next I
            
            
  
        
        
        connTranV.CommitTrans
        Screen.MousePointer = vbDefault
        MsgBox "Voucher has been Raised for this Refund"
        
    

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
    
Exit Sub
TransFail:
 Screen.MousePointer = vbDefault
   connTranV.RollbackTrans
    MsgBox "Could not Raise Refund Voucher!!! " & vbCrLf & vbCrLf & Err.Description

    Set connTranV = Nothing


End Sub




Private Function xxPostToPeachtreeXX(strRct2 As String, strName2 As String, AmountPaid2 As Double)
'            On Error Resume Next
'            Dim blnSuccess As Boolean
'
'            'call PTree
'            blnSuccess = DemoWriteToCashRcptJrnlAppliedToRevenues(strRct2, strName2, AmountPaid2)
'
'            If blnSuccess Then  'update isposted col in payments
'                Dim cmdX As New Command
'                cmdX.ActiveConnection = conStr
'                cmdX.CommandType = adCmdText
'                cmdX.CommandText = "update payments  set ispost=1 where ReceiptNo='" & strRct & "'"
'                cmdX.Execute
'            End If

End Function


Private Function getNum2String(dblV As Double) As String
Dim objCon As New figToWrd
Dim strWord As String
strWord = objCon.Num2String(dblV)
getNum2String = UCase(strWord)
Set objCon = Nothing
'0
End Function

Public Sub clearFields()
flgPrivate = False
'is_Private_Patient = False
flg_Enforce_Saving = False

txtAmt(0).Locked = False
txtAmt(1).Locked = False
txtAmt(2).Locked = False
txtAmt(3).Locked = False


lblAdmit.Caption = ""
lblDisch.Caption = ""

strReceiptNo = ""
isReversePay = False
'strRctNos = ""
'intRctNum = 0
isProcess = True 'ok
isLock = True

dtBillDate = vbEmpty
lblBillDate.Caption = ""

lblTGen.Visible = False
lblTGenCap.Visible = False

flgAllowPay = False
lblDate.Caption = ""
lblClinic.Caption = ""
'isAdmission = False
BillTo = ""
isNormalPay = True
txtAmt(0).Locked = False 'nece for AmountPaid Removal
chkNil.Value = False
lblStatus.Caption = ""
chkUnlock.Value = False
chkLock.Value = False
lblPNo.Caption = ""
EnrolleNo = ""
PolicyType = ""
gStrCoy = ""
strCompany = ""
'optHide = 0
'isFromAttendGrid = False
'isFromHidden = False
VerifyFlg = ""
strBCodePOS = ""
strBCodeCHQ = ""
strBCodeTRF = ""
strPatient = ""
gStrPatient = ""
gPatNo = ""
gStrCon = ""
gStrCoy = ""
'strCon = "" ' used by print invoice

    txtCheque.Text = ""
    chkCurr.Value = False
      chkDep.Value = False
cboPayFor.Text = ""
  chkExact(0).Value = False
  chkExact(1).Value = False
  chkExact(2).Value = False
  chkExact(3).Value = False
    
  
  
  
  If isBillNo = False Then
    txtSearch.Text = ""
  End If
    
    txtSearchRct.Text = ""
  
    'lblDate2.Caption = ""
    lblDateLast.Caption = ""
    lblLastClinic.Caption = ""
    
    lblDebt.Caption = 0#
    lblDiscount.Caption = 0#
    lblBillPayable.Caption = 0#
    lblGen.Caption = 0#
    lblBilled.Caption = 0#
    lblTotal.Caption = 0#
    lblPaid.Caption = 0#
     txtProf.Text = ""
    txtNHIS.Text = ""
    txtDiag.Text = ""
    cmdAddBill.Enabled = False
    cmdBill.Enabled = False
    cmdCoy.Enabled = False
    cmdSplit.Enabled = False
    cmdNHIS.Enabled = False
    'DTPicker1.Value = sysDate
    'DTPicker2.Value = sysDate
    
    lblCat.Caption = ""
    lblBill.Caption = ""
    cboVehNo.ListIndex = -1
   ' txtDiag.Text = ""
    txtDuty.Text = ""
    'dtCon.Value = ""
    lblTotal.Caption = 0
    lblCoy.Caption = ""
    'lblSub.Caption = 0
    'lblProf.Caption = 0
    'lblbf.Caption = 0
    'lblPay.Caption = 0
    lblTotal.Caption = 0
    
    lblDep.Caption = 0
    dblSub = 0
    dblMed = 0
    
    If is_Private_Patient = True Then
        lblScreen.Caption = "Generate Cash Receipt"
    Else
        lblScreen.Caption = "Generate Bill"
    End If
    
    Set grdData.DataSource = Nothing
    txtHt.Text = "0"
    txtEmp.Text = "0"
    
    txtAmt(0).Text = "0"
    txtAmt(1).Text = "0"
    txtAmt(2).Text = "0"
    txtAmt(3).Text = "0"
    
    lblAmtDue.Caption = 0
    'cboReceived.ListIndex = -1
    'cboPayFor.Text = "CONSULTATION AND DRUGS"
    'cboClinic.ListIndex = -1
    cboPay.ListIndex = -1  '.Text = "CASH"
    txtWord.Text = ""
    lblRCt.Caption = ""
    'chkRct.Value = False 'for now
'    strName = ""
'    strpCatID = ""
'    strCon = ""
'    strBillAdm = ""
'    strCoy = ""
    
    cboPayFor.ListIndex = -1
    cboBank(1).ListIndex = -1
    cboBank(2).ListIndex = -1
    cboBank(3).ListIndex = -1
    
    Set grdData.DataSource = Nothing
    Set grdDataRct.DataSource = Nothing
    'Set grdAttend.DataSource = Nothing
    
    'lblAcctNo(0).Caption = "AutoNoCash"
    'lblAcctNo(1).Caption = "AutoNoPOS"
    'lblAcctNo(2).Caption = "AutoNoCheque"
    'lblAcctNo(3).Caption = "AutoNoTransfer"

End Sub



Public Sub getRctValue2()
'  '''''''
'  On Error GoTo errH
' Dim rsGen As New ADODB.Recordset
' Dim strRctNo as long
' With rsGen
'
'    Select Case strCode
'
'    Case "BR"
'            'do nothing
'    Case Else
'        .Open "select receiptNo as ID from vwReceiptNo ", conStr, adOpenForwardOnly, adLockReadOnly
'        If Not .EOF Then
'
'                    strRctNo = !ID)
'
'                           Dim cmd As New ADODB.Command
'
'                           'iDNo = iDNo + 1 'genIDNo should increment and not this indIDNo
'                            With cmd
'                               .ActiveConnection = conStr
'                               .CommandType = adCmdText
'                               .CommandText = "Update iDGen set ID=" & strRctNo & " where DestName = 'Receipt'"
'                               .Execute
'                            End With
'                       Set cmd = Nothing
'        Else
'            MsgBox "Invalid ReceiptNo " & Err.Description
'        End If
'
'    End Select
'Set rsGen = Nothing
'End With
'  '''''''
'  Exit Sub
'errH:
''If rsGen.EOF Then rsGen!ID = 0
''Resume Next
'MsgBox Err.Description
End Sub

Public Sub getRctValue()
  '''''''
'  On Error GoTo errH
' Dim rsGen As New ADODB.Recordset
' Dim strRctNo as long
' With rsGen
'
'    Select Case strCode
'
'    Case "BR"
'            'do nothing
'    Case Else
'        .Open "select receiptNo as ID from vwReceiptNo ", conStr, adOpenForwardOnly, adLockReadOnly
'        If Not .EOF Then
'
'                    strRctNo = !ID)
'
'                           Dim cmd As New ADODB.Command
'
'                           'iDNo = iDNo + 1 'genIDNo should increment and not this indIDNo
'                            With cmd
'                               .ActiveConnection = conStr
'                               .CommandType = adCmdText
'                               .CommandText = "Update iDGen set ID=" & strRctNo & " where DestName = 'Receipt'"
'                               .Execute
'                            End With
'                       Set cmd = Nothing
'        Else
'            MsgBox "Invalid ReceiptNo " & Err.Description
'        End If
'
'    End Select
'Set rsGen = Nothing
'End With
'  '''''''
'  Exit Sub
'errH:
''If rsGen.EOF Then rsGen!ID = 0
''Resume Next
'MsgBox Err.Description
End Sub

Public Sub genIDNo()
  '''''''
  On Error GoTo errH
  
   Dim rsBL As New Recordset
    With rsBL

        .Open "select clinicType from qryhVisitsForAttend where consultID ='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
        If Not .EOF Then
            strClinic = !ClinicType & ""
            strCode = ""
            strRctCode = ""
            strClinicID = !ClinicType & ""
            
            lblClinic2.Caption = strClinic
        Else
        '    .Close
        '    .Open "select ItemName,ItemCode ,clinicType from vwClinicCodePublic where consultID ='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
        '    If Not .EOF Then
        '        strClinic = !clinicType & ""
        '        strCode = !ItemCode & ""
        '        strRctCode = !ItemCode & ""
        '        lblClinic2.Caption = strClinic
        '    Else
            strClinic = ""
            strCode = ""
            strRctCode = ""
            strClinicID = ""
            'MsgBox "ReceiptNo cannot be generated!!! No Tran ID for this transaction"
            'Exit Sub
            'End If
        End If
    End With
 
    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
     Dim rsGen As New ADODB.Recordset
    Dim cVal As Long
    Dim cVal2 As Long
    Dim strValX As String
    Dim strCval As String
'
'With rsGen
'
''    Select Case strCode
''
''    Case "BR"
'
'            .Open "select MAX(cast(SUBSTRING(ReceiptNo, 6, 9)as bigint))  as ID from Payments , conStr, adOpenForwardOnly, adLockReadOnly"
'            '.Open "select MAX(cast(SUBSTRING(consultID, 4, 9)as bigint))  as ID from hRecords WHERE  (SUBSTRING(consultID, 1, 3)='" & strHospID & " ')", conStr, adOpenForwardOnly, adLockReadOnly
'
'            '.Open "select MAX(cast(RIGHT(ReceiptNo,7)as bigint))  as ID from Payments WHERE  SUBSTRING(ReceiptNo, 4, 2)='" & strRctCode & " ' and SUBSTRING(ReceiptNo, 1, 3)='" & strHospID & " '", conStr, adOpenForwardOnly, adLockReadOnly
'
'            '.Open "select MAX(CAST(Right(DNoteID,7)  as bigint)) as ID from DebitNote", conStr, adOpenForwardOnly, adLockReadOnly
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'        If .EOF Then
'            strValX = strRctCode & "0000000001"
'        Else
'             cVal = IIf(IsNull(!ID), 0, !ID)
'
'             If cVal <= 0 Then
'                .Close
'                    .Open "select MAX(cast(right(ReceiptNo,  7)as bigint))  as ID from Payments WHERE  (SUBSTRING(ReceiptNo, 1, 2)='" & strRctCode & " ')", conStr, adOpenForwardOnly, adLockReadOnly
'                If .EOF Then
'                    cVal = 0  'IIf(IsNull(!conIdVal), 0, !conIdVal)
'                Else
'                    cVal = IIf(IsNull(!ID), 0, !ID)) '+1
'
'                End If
'            End If
'
'            cVal2 = cVal + 1
'            strValX = Right("0000000000" & CStr(cVal2), 9)
'
'        End If
'
'
'        strCval = strHospID & strRctCode & strValX
'        lblRct.Caption = strCval


    getID_No = "" 'ok b4 call of getIDNo
    Call getIDNoRctMax("RECEIPT2")
    If getID_No = "" Then
        MsgBox "Unable to generate No!!! Function getIDNo Failed! Receipt"
        Unload Me
        'Exit Sub
    End If
    
    strCval = ""
    
    If isReversePay = True Then
        strCval = strReceiptNo & "R"
    Else
        strCval = getID_No
    End If
    
    lblRCt.Caption = strCval

    'strCval = ""
    'strCval = strHospID & strRctCode & strValX
    'lblRCt.Caption = strCval
                
                    
            
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'If Not .EOF Then
            '    If IsNull(!ID) Or !ID = 0 Then
            '        lblRCt.Caption = strRctCode & "0000001"
            '    Else
            '        lblRCt.Caption = strRctCode & Right("0000000" & CStr(!ID + 1), 7)
            '    End If
            'Else
            '        lblRCt.Caption = strRctCode & "0000001"
            'End If
                
            Set rsGen = Nothing
            

'End With

  
  Exit Sub
errH:
'If rsGen.EOF Then rsGen!ID = 0
'Resume Next
MsgBox Err.Description
End Sub

Public Sub insIDNo(conn As Connection) 'off for now
On Error GoTo errH
'
'
'Select Case strCode
'Case "BR"
'  'do nothing
'Case Else
'
'
'    '''''''
     Dim DbVal As Long
      Dim rsBLV As New Recordset
      With rsBLV
        .Open "select ID from IDgen where DestName = 'Receipt2'", conStr, adOpenForwardOnly, adLockReadOnly
        If Not .EOF Then
            DbVal = !ID
        End If
      End With

       iDNo = lblRCt.Caption
       If iDNo >= DbVal Then
           Dim Cmd As New ADODB.Command
           Dim strHN
           strHN = iDNo

           'iDNo = iDNo + 1 'genIDNo should increment and not this insIDNo
            With Cmd
               .ActiveConnection = conn
               .CommandType = adCmdText
               .CommandText = "Update iDGen set ID=" & iDNo & " where DestName = 'Receipt2'"
               .Execute
            End With
       End If
       Set Cmd = Nothing
'        'Set cN = Nothing
'    End Select
'  '''''''
' Exit Sub
'errH:
' MsgBox "Problems Saving generated Receipt No with error " & Err.Description
'
''On Error GoTo errh
''  '''''''
'' Dim cmd As New ADODB.Command
'' Dim strHN
'' strHN = lblRCt.Caption
''
''    iDNo = lblRCt.Caption)
''    iDNo = iDNo + 1
''
'' With cmd
''.ActiveConnection = conn
''.CommandType = adCmdText
''.CommandText = "Update iDGen set ID=" & iDNo & " where DestName = 'Receipt'"
''.Execute
''Set cmd = Nothing
'''Set cN = Nothing
'' End With
''  '''''''
 Exit Sub
errH:
 MsgBox "Problems Saving generated Billing No with error " & Err.Description

  
  End Sub



Private Sub txtAmt_Change(Index As Integer)
On Error GoTo errH
If isSaved = True Then Exit Sub

'If Trim(txtAmt(Index).Text) = "" Then Trim(txtAmt(Index).Text) = 0 'Then Exit Sub
Dim AmtTot As Double
Dim I As Integer
Dim PayType As String

For I = 0 To 3
    If Trim(txtAmt(I).Text) = "" Then txtAmt(I).Text = 0
    AmtTot = AmtTot + CDbl(txtAmt(I).Text)
    
    If Trim(txtAmt(I).Text) > 0 Then
        PayType = PayType & lblAmt(I).Caption & ","
    End If
Next
txtHt.Text = FormatNumber(AmtTot, 2)

If Trim(PayType) <> "" Then
    cboPay.Text = Mid(PayType, 1, Len(PayType) - 1)
End If

Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub txtEmp_change()
On Error GoTo errH
If Trim(txtEmp.Text) = "" Then Exit Sub
'txtEmp.Text = CDbl(lblPay.Caption)
Dim objCon As New figToWrd
Dim strWord As String
strWord = objCon.Num2String(Val(Replace(txtEmp.Text, ",", "")))
txtDuty.Text = UCase(strWord)
Set objCon = Nothing

'''''''''####''''Also in okButton_click proc'''''''''''''
'If chkRct.Value = vbChecked Then   '''strCoy = strPrivate Then ''''''And strApp = "BILLING" Then '"PRIVATE" Or strCoy = "0001" Then
If (strCoy = strPrivate Or StrClientCatX = ClientCatPrivate) Then    '''strCoy = strPrivate Then ''''''And strApp = "BILLING" Then '"PRIVATE" Or strCoy = "0001" Then
    'fraReceipt.Enabled = True
    If CDbl(txtEmp.Text) > 0 Then ''ok
        If lblClinic.Caption <> "(IN-PATIENT)" Then
            'If Print_From_Small_Printer = "YES" Then
            '
            '    ''''nece
            '    If CDbl(txtEmp.Text) > 0 Then
            '        'enter default values
            '        If chkExact(0).Value = vbChecked Then
            '            'chkExact(0).Value = False
            '            Call chkExact_Click(0)
            '        Else
            '            chkExact(0).Value = vbChecked 'POS_PayType_Default_Cash = "YES"
            '        End If
            '        cboPayFor.Text = "Medical Services"
            '        'txtHt.Text = FormatNumber(CDbl(txtEmp.Text), 2)
            '        'cboPay.Text = "CASH"
            '
            '        ''error then reverse
            '        If CDbl(txtHt.Text) > CDbl(txtEmp.Text) Then
            '             chkExact(0).Value = False
            '         End If
            '    Else
            '        chkExact(0).Value = False
            '    End If
            '
            '     ''''''''
            'End If
        End If
    End If
Else
    'chkRct.Value = False
    'fraReceipt.Enabled = False
End If

Exit Sub
errH:
MsgBox Err.Description
End Sub


Private Sub txtHt_Change()
If Trim(txtHt.Text) = "" Then Exit Sub
Dim objCon As New figToWrd
Dim strWord As String
strWord = objCon.Num2String(Abs(Replace(txtHt.Text, ",", "")))
txtWord.Text = UCase(strWord)

 Exit Sub
errH:
    MsgBox Err.Description
  End Sub


Public Sub getVal()
'Dim proF As Double
'Dim nonProf As Double
'Dim exP As Double
''If txtProf.Text = "" Then txtProf.Text = 0
''If txtNonProf.Text = "" Then txtNonProf.Text = 0
''If txtExp.Text = "" Then txtExp.Text = 0
'
'
'proF = Val(txtProf.Text)
'nonProf = Val(txtNonProf.Text)
'exP = Val(txtExp.Text)
'
'txtCharged.Text = CStr(proF + nonProf + exP)
End Sub



Private Sub txtCharged_Change()

End Sub

Private Sub txtExp_LostFocus()
getVal
End Sub

Private Sub txtNonProf_LostFocus()
getVal
End Sub

Private Sub txtProf_LostFocus()
getVal
End Sub

Private Sub OrderGrid_Click()
'Dim intSave As Integer
'intSave = MsgBox("Are you sure to Delete?", vbYesNo, "Check before Delete")
'If intSave = vbYes Then
'dblVal = dblVal - Val(OrderGrid.TextMatrix(newRow, 4))
'lblTotal.Caption = CStr(dblVal)
'If OrderGrid.RowSel > 1 Then
'    OrderGrid.RemoveItem (OrderGrid.RowSel)
'Else
'    OrderGrid.TextMatrix(newRow, 0) = ""
'    OrderGrid.TextMatrix(newRow, 1) = ""
'    OrderGrid.TextMatrix(newRow, 2) = ""
'    OrderGrid.TextMatrix(newRow, 3) = ""
'    OrderGrid.TextMatrix(newRow, 4) = ""
'    OrderGrid.TextMatrix(newRow, 5) = ""
'    OrderGrid.TextMatrix(newRow, 6) = ""
'
'lblTotal.Caption = 0
'End If
'
''nuM = nuM - 1
''Label4.Caption = nuM & " Trucks added"
'
'MsgBox "Item Removed"
'End If
'
End Sub

Private Sub txtQty_LostFocus()
'Dim proF As Double
'Dim nonProf As Double
'Dim exP As Double
''If txtProf.Text = "" Then txtProf.Text = 0
''If txtNonProf.Text = "" Then txtNonProf.Text = 0
''If txtExp.Text = "" Then txtExp.Text = 0
'
'If txtQty.Text = "" Then
'MsgBox "Please enter a figure in the Quantity field"
'Exit Sub
'End If
'
'If Not IsNumeric(txtQty.Text) Then
'MsgBox "Characters not allowed"
'Exit Sub
'End If
'
'proF = Val(txtQty.Text)
''nonProf = Val(txtNonProf.Text)
''exP = Val(txtExp.Text)
'
'lblSub.Caption = CStr(proF * CDbl(lblPrice.Caption))

End Sub

Public Sub generateBill()
'Dim connTran As New Connection

'    connTran.ConnectionString = conSTR
'    connTran.Open
    'On Error GoTo TransFail
'    connTran.BeginTrans
    
'      Dim rsBR As New Recordset
'      Dim rsINS As New Recordset
'      Dim rsDetails As New Recordset

'            rsDetails.Open "select * from billingDetails", connTran, adOpenStatic, adLockOptimistic
'            rsBR.Open "select * from BillReceipt", connTran, adOpenStatic, adLockOptimistic
'
'            rsINS.Open "select * from billing", connTran, adOpenStatic, adLockOptimistic
'            rsINS.AddNew
'            rsINS!billno = strBillNo
'            rsINS!bdate = txtDepart.Text
'            rsINS!resvno = lblResv.Caption
'            rsINS!acctRenderTo = txtAcct.Text
'
'            AcctCode = txtAcct.Text
'
'            rsINS.Update
            
'            For i = 1 To tNum
'            rsDetails.AddNew
'            rsDetails!bdate = rsBLV!bdate
'            rsDetails!billno = strBillNo
'            rsDetails!accmdCode = rsBLV!accmdCode
'            rsDetails!guestname = rsBLV!guestname
'            rsDetails!totalAmountCharged = rsBLV!totalAmountCharged
'            rsDetails!AmountChargedInWord = rsBLV!AmountChargedInWord
'            rsDetails!purpose = rsBLV!purpose
'            rsDetails!bTime = rsBLV!bTime
'           rsDetails.Update
'
'            'insert into billReceipt table
'            rsBR.AddNew
'            rsBR!dtDate = rsBLV!bdate
'            rsBR!docNo = strBillNo
'            rsBR!resvno = lblResv.Caption
'            rsBR!doctype = "BILL"
'            rsBR!Desc = rsBLV!purpose
'            rsBR!AmtBill = rsBLV!totalAmountCharged
'            rsBR!AmtReceipt = 0#
'            rsBR!acctRenderTo = txtAcct.Text
'            rsBR!accmdCode = txtCode.Text
'            rsBR!guestname = txtName.Text
'
'            rsBR.Update
'
'           rsBLV.MoveNext
'           Next i
'

End Sub


Public Sub preProcessing() 'no more nece now in UpdateBill   'preprocessing
On Error GoTo errH
            'Dim Cmd As New Command
            'Cmd.ActiveConnection = conStr
            'Cmd.CommandType = adCmdText
            '
            'Cmd.CommandText = "update BillAccum set capitated='NO' where capitated='BIL' and consultID = '" & strCon & "'"
            'Cmd.Execute 'ok here
            '
            '
            'Cmd.CommandText = "update BillAccum set CoyName='" & strCoy & "' where coyName is null and consultID = '" & strCon & "'"
            'Cmd.Execute 'ok here
            '
            'Cmd.CommandText = "update BillAccum set BillTo='" & strBillTo & "' where BillTo is null and consultID = '" & strCon & "'"
            'Cmd.Execute 'ok here

            '  Select Case lblCat.Caption 'no more nece now in UpdateBill   'preprocessing
            '
            '  Case "NHIS", "HMO", "PHIS"
            '      If lblDisplay.Caption = "YES" Then
            '              'nece 'capitated='no' here
            '              Cmd.CommandText = "update BillAccum set capitated='NO' where consultID = '" & strCon & "'"
            '              Cmd.Execute
            '      Else
            '              Cmd.CommandText = "update BillAccum set capitated='NO' where capitated='' and consultID = '" & strCon & "'"
            '              Cmd.Execute
            '      End If
            '
            '  Case Else
            '      'nece 'capitated='no' here
            '      Cmd.CommandText = "update BillAccum set capitated='NO' where consultID = '" & strCon & "'"
            '      Cmd.Execute
            '  End Select
            
Exit Sub
errH:
MsgBox Err.Description
End Sub


Public Sub getAccumBill()
On Error GoTo errH
    cmdCoy.Enabled = True
    cmdSplit.Enabled = True
        cmdBill.Enabled = True
        cmdNHIS.Enabled = True
    
    Dim cntVal As Double
    Dim cntValTotal As Double
    Dim dblDiscount As Double
    'Dim currDebt As Double
    
    
''verify if already processed''then disallow update'''''''where isprocess=1 nece''''''''''''''''''''''''''''''
'Dim rsVerX As New Recordset
'rsVerX.Open "select billNo from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
'If Not rsVerX.EOF Then 'ie if its processed
'   'do nothing if bill is already processed, isprocess=1
'Else
'    If isFromAttendGrid = True Then
'        'no  update for BillNo from AttendGrid for now
'        isFromAttendGrid = False 'nece here
'    Else
'        isFromAttendGrid = False
'        isLockBill = False
'
'        If rsVerX.State = adStateOpen Then rsVerX.Close 'Locked Bill
'        rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
'        If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
'            'no update 'cos it is locked
'        Else
'            Call updateBill(strCon, strCoy, strPatient) 'for isprocess=0 and non-existent bills
'        End If
'    End If
'End If

        'verify if already processed''then disallow update'''''''where isprocess=1 nece''''''''''''''''''''''''''''''
        Dim rsVerX As New Recordset
        rsVerX.Open "select billNo from billing where isprocess=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
        If Not rsVerX.EOF Then 'ie if its processed
           isProcess = True
        Else
           isProcess = False
        End If

        
        If rsVerX.State = adStateOpen Then rsVerX.Close 'Locked Bill
        rsVerX.Open "select * from billing where isSigned=1 and billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
        If Not rsVerX.EOF Then 'ie its Locked, Check for Mgt Prev
            isLock = True
        Else
            isLock = False
        End If

    If isProcess = False And isLock = False Then
        Call updateBill(strCon, strCoy, strPatient) 'for isprocess=0 and non-existent bills
    End If
    
    Call UpdatePay(Me, strCon) 'to come after UpdateAmountPaid(strCon)

   


    lblScreen.Caption = "Bill for " & strName & vbNewLine & "=N=" & lblAmtDue.Caption
                
                'fill grid with billItems from consultID using qryBillAccumAll
                Dim rsTot As New Recordset
                rsTot.CursorLocation = adUseClient
                rsTot.Open "select distinct ROW_NUMBER() OVER (order by Service) AS Num , [Date],Service,SNo,conID,case AttendedTo when 1  then 'YES' else 'NO' end as AttendedTo ,Capitated ,case isBilled when 0  then 'NO' when 1 then 'YES' end as Billed,Qty,UnitPrice,Subtotal,RevType,BillType,CoyName,BillTo,ConsultID,EntryDate,EntryTime,AppName,ClientName from qryBillAccumAll where consultid='" & strCon & "'", conStr, adOpenStatic, adLockOptimistic
                If Not rsTot.EOF Then
                    Set grdData.DataSource = rsTot
                    
                    grdData.Columns("Subtotal").NumberFormat = "#,###.00"
                    grdData.Columns("Subtotal").Alignment = dbgRight
                    grdData.Columns("UnitPrice").NumberFormat = "#,###.00"
                    grdData.Columns("UnitPrice").Alignment = dbgRight
                    grdData.Columns("Qty").NumberFormat = "#,##;(#,##0)"  '"#,###.00"
                    grdData.Columns("Qty").Alignment = dbgRight
                
                
                    grdData.Columns("SNO").Visible = False
                    'grdData.Columns("Billtype").Visible = False
                    grdData.Columns("conID").Visible = False
                    'grdData.Columns("clientcat").Visible = False
                    'grdData.Columns("Referal").Visible = False
                    'grdData.Columns("clientcatID").Visible = False
                    grdData.Columns("Capitated").Width = 800
                    grdData.Columns("Num").Width = 500
                    grdData.Columns("Qty").Width = 1000
                    grdData.Columns("UnitPrice").Width = 1200
                    grdData.Columns("Subtotal").Width = 1500
                    grdData.Columns("Date").Width = 1200
                    'grdData.Columns("dosage").Visible = False
                    intRec = rsTot.RecordCount
                    
                    If isbillAdjust = True Then
                        rsTot.Find "Service='" & adjustTo & "'"
                        isbillAdjust = False
                    End If
                    
                Else
                    Set grdData.DataSource = Nothing
                End If
        'If dblBF > 0 Then
            'MsgBox "This Patient has a Debt of =N=" & FormatNumber(dblBF, 2) & " to pay"
        'End If

    isbillAdjust = False 'very nece here also

''''''''''''''''''''''''''''''''''''''''''''''


        Call getBillStatus
        
        
        '''''''''''''''''Receipt History'''''''''''''''''''''''''''''
        intRctNum = 0
        strRctNos = ""
        Set grdDataRct.DataSource = Nothing
        grdDataRct.Caption = "Payment History"
        Dim XV As Integer
        Dim rsRct As New Recordset
        With rsRct
         .CursorLocation = adUseClient
         .Open "select ReceiptDate,rTime as Time,ReceiptNo,BillNo,AmountBilled as AmountDue,AmountPaid,Balance,PaymentFor,PayType,ClinicID as Clinic,ReceivedBy from qryhBillingIncome where BillNo='" & strCon & "' order by ReceiptNo", conStr, adOpenStatic, adLockOptimistic
         If Not .EOF Then
         
         
            'intRctNum = .RecordCount
             .MoveFirst
             Do While Not .EOF
                strRctNos = strRctNos & !ReceiptNo & ", "
                .MoveNext
             Loop
         
             
             Set grdDataRct.DataSource = rsRct
             
             grdDataRct.Columns("AmountDue").NumberFormat = "#,###.00"
             grdDataRct.Columns("AmountDue").Alignment = dbgRight
             grdDataRct.Columns("AmountPaid").NumberFormat = "#,###.00"
             grdDataRct.Columns("AmountPaid").Alignment = dbgRight
             grdDataRct.Columns("Balance").NumberFormat = "#,###.00"
             grdDataRct.Columns("Balance").Alignment = dbgRight
             
             
        Else
             Set grdDataRct.DataSource = Nothing
        End If
    End With
Exit Sub
errH:
MsgBox Err.Description



End Sub


Private Sub getBillStatus()
On Error GoTo errH
'Set rsBL = Nothing
Dim strStatus As String
strStatus = ""
lblStatus.Caption = ""
Dim rsVer2 As New Recordset
rsVer2.Open "select isNull(isprocess,0) as isprocess ,isNull(isSigned,0)as isSigned from billing where billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsVer2.EOF Then
    If rsVer2!isProcess = True And rsVer2!isSigned = True Then
        isProcess = True
        isLock = True
        strStatus = "Processed, Locked"
    
    ElseIf rsVer2!isProcess = False And rsVer2!isSigned = False Then
        isProcess = False
        isLock = False
        strStatus = "Not Processed, Not Locked"
    
    ElseIf rsVer2!isProcess = True And rsVer2!isSigned = False Then
        isProcess = True
        isLock = False
        strStatus = "Processed, but Not Locked"
    
    ElseIf rsVer2!isProcess = False And rsVer2!isSigned = True Then
        isProcess = False
        isLock = True
        strStatus = "Not Processed, but Locked"
    End If
Else
        'will hardly be run
        isProcess = True
        isLock = True
        strStatus = "No Bill Yet"
End If

lblStatus.Caption = strStatus

Exit Sub
errH:
MsgBox Err.Description


End Sub



Private Sub getDebtXXX()
'On Error GoTo errH
'  Dim rsChkBill As New Recordset
'    rsChkBill.Open "select debtBF from hPatients where pno='" & strPatient & "'", conStr, adOpenStatic, adLockOptimistic
'    If Not rsChkBill.EOF Then ' existing bill, will already have amtBF set
'        dblBF = 0
'        dblBF = IIf(IsNull(rsChkBill!debtBF), 0, rsChkBill!debtBF)
'        'dblBF = dblBF - dblAmtBal  'dblAmtBal is already part of current Bill
'        'lblBF.Caption = FormatNumber(CDbl(dblBF), 2) ' not here
'
'    Else
'        dblBF = 0
'        'lblBF.Caption = FormatNumber(0, 2) ' not here
'    End If
'
'Set rsChkBill = Nothing
'
'Exit Sub
'errH:
'MsgBox Err.Description


End Sub

Public Sub SetButtons(bVal As Boolean)
cmdAdd.Visible = bVal
'cmdRefresh.Visible = Not bVal
cmdCancel.Visible = Not bVal
'cmdPrint.Visible = Not bVal
OKButton.Visible = Not bVal

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
txtSearchRct.Enabled = True

'dtStart.Enabled = True
'dtEnd.Enabled = True

DTAttnd1.Enabled = True
DTAttnd2.Enabled = True
cboGroup.Enabled = True

End Sub


Public Function isPatOnInj() As Boolean
 Dim rsGen As New ADODB.Recordset
rsGen.Open "select attendedto,consultID from hconsulting where consultid='" & strCon & "' AND attendedTo = 0 ", conStr, adOpenForwardOnly, adLockReadOnly
If Not rsGen.EOF Then
    isPatOnInj = True
Else
    isPatOnInj = False

End If

'Set rsGen = Nothing
  '''''''

End Function



Public Sub getDiagnosis()
  strDiag = ""
  Dim rsBLvX As New Recordset
  With rsBLvX
  
  .Open "select Diagnosis from billing where billNo='" & strCon & "'", conStr, adOpenForwardOnly, adLockReadOnly
    If Not .EOF Then
        If IsNull(!diagnosis) Or Len(Trim(!diagnosis) <= 0) Then
        
            .Close
            .Open "select Diagnosis from hconsulting where consultID='" & strCon & "' order by ID ", conStr, adOpenForwardOnly, adLockReadOnly
              If Not .EOF Then
                  .MoveFirst
                  Do While Not .EOF
                      strDiag = strDiag & Replace(!diagnosis & "", vbNewLine, ";") & vbNewLine
                      .MoveNext
                  Loop
              Else
                  strDiag = ""
              End If
            
            txtDiag.Text = Trim(strDiag)
        
        Else
            txtDiag.Text = Trim(!diagnosis)
        End If
        
    Else
        .Close
        .Open "select Diagnosis from hconsulting where consultID='" & strCon & "' order by ID", conStr, adOpenForwardOnly, adLockReadOnly
          If Not .EOF Then
              .MoveFirst
              Do While Not .EOF
                  strDiag = strDiag & Replace(!diagnosis & "", vbNewLine, ";") & vbNewLine
                  .MoveNext
              Loop
          Else
              strDiag = ""
          End If
        
        txtDiag.Text = Trim(strDiag)
    End If
    
    
    End With
    Set rsBLvX = Nothing


End Sub

Public Sub getApprvCode()
  On Error GoTo errH
  Dim rsBLvX As New Recordset
  Dim strApprv As String
  With rsBLvX
  .Open "select ApprvCode from hApprvCode where consultID='" & strCon & "' order by SNo", conStr, adOpenForwardOnly, adLockReadOnly
    
    If Not .EOF Then
        .MoveFirst
        Do While Not .EOF
            strApprv = strApprv & !ApprvCode & "" & vbNewLine
            .MoveNext
        Loop
        txtApprv.Text = Trim(strApprv)
    Else
        txtApprv.Text = ""
    End If
    
    Set rsBLvX = Nothing
End With
Exit Sub
errH:
MsgBox Err.Description
End Sub



Private Sub getValInWord(dblCurrBill As Double)

On Error GoTo errH
'txtEmp.Text = CDbl(lblPay.Caption)
Dim objCon As New figToWrd
strWord = ""
strWord = objCon.Num2String(Val(Replace(dblCurrBill, ",", "")))
strWord = UCase(strWord)
'txtDuty.Text = UCase(strWord)
Set objCon = Nothing
Exit Sub
errH:
MsgBox Err.Description
End Sub



Private Sub txtSearch_DblClick()
On Error GoTo errH
If Trim(txtSearch.Text) = "" Then
    txtSearch.Text = strConRecall 'to recall last strCon
End If
Exit Sub
errH:
MsgBox Err.Description
End Sub

