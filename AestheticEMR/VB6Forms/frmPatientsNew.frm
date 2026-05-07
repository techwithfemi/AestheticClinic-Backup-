VERSION 5.00
Object = "{CDE57A40-8B86-11D0-B3C6-00A0C90AEA82}#1.0#0"; "msdatgrd.ocx"
Object = "{BDC217C8-ED16-11CD-956C-0000C04E4C0A}#1.1#0"; "tabctl32.ocx"
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomct2.ocx"
Object = "{F9043C88-F6F2-101A-A3C9-08002B2F49FB}#1.2#0"; "comdlg32.ocx"
Begin VB.Form frmPatientsNew 
   BackColor       =   &H00FFC0C0&
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "Patient Attendance / Registration"
   ClientHeight    =   9150
   ClientLeft      =   2820
   ClientTop       =   765
   ClientWidth     =   13905
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MDIChild        =   -1  'True
   MinButton       =   0   'False
   ScaleHeight     =   9150
   ScaleWidth      =   13905
   ShowInTaskbar   =   0   'False
   Begin VB.Timer tmrSearch 
      Enabled         =   0   'False
      Interval        =   1000
      Left            =   0
      Top             =   0
   End
   Begin VB.CommandButton cmdClinic 
      BackColor       =   &H00FFFFC0&
      Caption         =   "Add Patient to Other Clinic(s)"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   510
      Left            =   12645
      Style           =   1  'Graphical
      TabIndex        =   52
      Top             =   6075
      Visible         =   0   'False
      Width           =   2655
   End
   Begin VB.CommandButton cmdRefresh 
      BackColor       =   &H8000000C&
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
      Height          =   465
      Left            =   7155
      TabIndex        =   49
      Top             =   8370
      Width           =   1215
   End
   Begin VB.CommandButton cmdDel 
      BackColor       =   &H8000000C&
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
      Height          =   465
      Left            =   8460
      TabIndex        =   48
      Top             =   8370
      Width           =   1215
   End
   Begin VB.CommandButton cmdEdit 
      BackColor       =   &H8000000C&
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
      Height          =   465
      Left            =   5805
      TabIndex        =   47
      Top             =   8370
      Width           =   1215
   End
   Begin VB.CommandButton CancelButton 
      BackColor       =   &H8000000C&
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
      Height          =   465
      Left            =   12285
      TabIndex        =   46
      Top             =   8370
      Width           =   1215
   End
   Begin VB.CommandButton cmdAdd 
      BackColor       =   &H8000000C&
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
      Height          =   465
      Left            =   4455
      TabIndex        =   45
      Top             =   8370
      Width           =   1215
   End
   Begin VB.CommandButton OKButton 
      BackColor       =   &H8000000C&
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
      Height          =   465
      Left            =   4455
      TabIndex        =   50
      Top             =   8370
      Width           =   1215
   End
   Begin VB.CommandButton cmdCancel 
      BackColor       =   &H8000000C&
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
      Height          =   465
      Left            =   5805
      TabIndex        =   51
      Top             =   8370
      Width           =   1215
   End
   Begin TabDlg.SSTab SSTab1 
      Height          =   8520
      Left            =   45
      TabIndex        =   40
      Top             =   540
      Width           =   13785
      _ExtentX        =   24315
      _ExtentY        =   15028
      _Version        =   393216
      Tab             =   1
      TabHeight       =   520
      BackColor       =   16761024
      BeginProperty Font {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   400
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      TabCaption(0)   =   "Search for Existing Record"
      TabPicture(0)   =   "frmPatientsNew.frx":0000
      Tab(0).ControlEnabled=   0   'False
      Tab(0).Control(0)=   "Frame2"
      Tab(0).Control(1)=   "CommonDialog1"
      Tab(0).ControlCount=   2
      TabCaption(1)   =   "Basic Information Entries"
      TabPicture(1)   =   "frmPatientsNew.frx":001C
      Tab(1).ControlEnabled=   -1  'True
      Tab(1).Control(0)=   "Label7(4)"
      Tab(1).Control(0).Enabled=   0   'False
      Tab(1).Control(1)=   "Label7(3)"
      Tab(1).Control(1).Enabled=   0   'False
      Tab(1).Control(2)=   "Label21"
      Tab(1).Control(2).Enabled=   0   'False
      Tab(1).Control(3)=   "Label12(0)"
      Tab(1).Control(3).Enabled=   0   'False
      Tab(1).Control(4)=   "Label37"
      Tab(1).Control(4).Enabled=   0   'False
      Tab(1).Control(5)=   "Label14"
      Tab(1).Control(5).Enabled=   0   'False
      Tab(1).Control(6)=   "Label9(1)"
      Tab(1).Control(6).Enabled=   0   'False
      Tab(1).Control(7)=   "Label4(1)"
      Tab(1).Control(7).Enabled=   0   'False
      Tab(1).Control(8)=   "Label7(0)"
      Tab(1).Control(8).Enabled=   0   'False
      Tab(1).Control(9)=   "Shape1"
      Tab(1).Control(9).Enabled=   0   'False
      Tab(1).Control(10)=   "piX"
      Tab(1).Control(10).Enabled=   0   'False
      Tab(1).Control(11)=   "Label3"
      Tab(1).Control(11).Enabled=   0   'False
      Tab(1).Control(12)=   "Label1"
      Tab(1).Control(12).Enabled=   0   'False
      Tab(1).Control(13)=   "Label8"
      Tab(1).Control(13).Enabled=   0   'False
      Tab(1).Control(14)=   "Label13"
      Tab(1).Control(14).Enabled=   0   'False
      Tab(1).Control(15)=   "Label11"
      Tab(1).Control(15).Enabled=   0   'False
      Tab(1).Control(16)=   "Label19"
      Tab(1).Control(16).Enabled=   0   'False
      Tab(1).Control(17)=   "Label12(5)"
      Tab(1).Control(17).Enabled=   0   'False
      Tab(1).Control(18)=   "Label28"
      Tab(1).Control(18).Enabled=   0   'False
      Tab(1).Control(19)=   "Label10"
      Tab(1).Control(19).Enabled=   0   'False
      Tab(1).Control(20)=   "lblAge"
      Tab(1).Control(20).Enabled=   0   'False
      Tab(1).Control(21)=   "Label29"
      Tab(1).Control(21).Enabled=   0   'False
      Tab(1).Control(22)=   "Label30"
      Tab(1).Control(22).Enabled=   0   'False
      Tab(1).Control(23)=   "Label7(1)"
      Tab(1).Control(23).Enabled=   0   'False
      Tab(1).Control(24)=   "Label7(2)"
      Tab(1).Control(24).Enabled=   0   'False
      Tab(1).Control(25)=   "Label17(0)"
      Tab(1).Control(25).Enabled=   0   'False
      Tab(1).Control(26)=   "Label2"
      Tab(1).Control(26).Enabled=   0   'False
      Tab(1).Control(27)=   "Label15"
      Tab(1).Control(27).Enabled=   0   'False
      Tab(1).Control(28)=   "Label26"
      Tab(1).Control(28).Enabled=   0   'False
      Tab(1).Control(29)=   "Label18"
      Tab(1).Control(29).Enabled=   0   'False
      Tab(1).Control(30)=   "Label16(2)"
      Tab(1).Control(30).Enabled=   0   'False
      Tab(1).Control(31)=   "Label6"
      Tab(1).Control(31).Enabled=   0   'False
      Tab(1).Control(32)=   "Label36"
      Tab(1).Control(32).Enabled=   0   'False
      Tab(1).Control(33)=   "Label23"
      Tab(1).Control(33).Enabled=   0   'False
      Tab(1).Control(34)=   "Label24"
      Tab(1).Control(34).Enabled=   0   'False
      Tab(1).Control(35)=   "Label25"
      Tab(1).Control(35).Enabled=   0   'False
      Tab(1).Control(36)=   "Label31"
      Tab(1).Control(36).Enabled=   0   'False
      Tab(1).Control(37)=   "Label32"
      Tab(1).Control(37).Enabled=   0   'False
      Tab(1).Control(38)=   "Label38"
      Tab(1).Control(38).Enabled=   0   'False
      Tab(1).Control(39)=   "Label20"
      Tab(1).Control(39).Enabled=   0   'False
      Tab(1).Control(40)=   "lblDebtCap"
      Tab(1).Control(40).Enabled=   0   'False
      Tab(1).Control(41)=   "lblDebt"
      Tab(1).Control(41).Enabled=   0   'False
      Tab(1).Control(42)=   "Label34"
      Tab(1).Control(42).Enabled=   0   'False
      Tab(1).Control(43)=   "Label4(0)"
      Tab(1).Control(43).Enabled=   0   'False
      Tab(1).Control(44)=   "fraAttd"
      Tab(1).Control(44).Enabled=   0   'False
      Tab(1).Control(45)=   "dtDate"
      Tab(1).Control(45).Enabled=   0   'False
      Tab(1).Control(46)=   "dtExpDate"
      Tab(1).Control(46).Enabled=   0   'False
      Tab(1).Control(47)=   "dtRegDate"
      Tab(1).Control(47).Enabled=   0   'False
      Tab(1).Control(48)=   "cboStatus"
      Tab(1).Control(48).Enabled=   0   'False
      Tab(1).Control(49)=   "txtOccu"
      Tab(1).Control(49).Enabled=   0   'False
      Tab(1).Control(50)=   "txtOfficeAddress"
      Tab(1).Control(50).Enabled=   0   'False
      Tab(1).Control(51)=   "cboTitle"
      Tab(1).Control(51).Enabled=   0   'False
      Tab(1).Control(52)=   "cboCard"
      Tab(1).Control(52).Enabled=   0   'False
      Tab(1).Control(53)=   "txtHomeAddress"
      Tab(1).Control(53).Enabled=   0   'False
      Tab(1).Control(54)=   "txtfirstNAme"
      Tab(1).Control(54).Enabled=   0   'False
      Tab(1).Control(55)=   "txtsurNAme"
      Tab(1).Control(55).Enabled=   0   'False
      Tab(1).Control(56)=   "txtEmail"
      Tab(1).Control(56).Enabled=   0   'False
      Tab(1).Control(57)=   "txtOLD"
      Tab(1).Control(57).Enabled=   0   'False
      Tab(1).Control(58)=   "txtSupp"
      Tab(1).Control(58).Enabled=   0   'False
      Tab(1).Control(59)=   "txtPhone"
      Tab(1).Control(59).Enabled=   0   'False
      Tab(1).Control(60)=   "txtSex"
      Tab(1).Control(60).Enabled=   0   'False
      Tab(1).Control(61)=   "txtPix"
      Tab(1).Control(61).Enabled=   0   'False
      Tab(1).Control(62)=   "cboFile"
      Tab(1).Control(62).Enabled=   0   'False
      Tab(1).Control(63)=   "cboMat"
      Tab(1).Control(63).Enabled=   0   'False
      Tab(1).Control(64)=   "txtAge"
      Tab(1).Control(64).Enabled=   0   'False
      Tab(1).Control(65)=   "cboNew"
      Tab(1).Control(65).Enabled=   0   'False
      Tab(1).Control(66)=   "txtGeno"
      Tab(1).Control(66).Enabled=   0   'False
      Tab(1).Control(67)=   "txtBG"
      Tab(1).Control(67).Enabled=   0   'False
      Tab(1).Control(68)=   "chkReg"
      Tab(1).Control(68).Enabled=   0   'False
      Tab(1).Control(69)=   "cboPat"
      Tab(1).Control(69).Enabled=   0   'False
      Tab(1).Control(70)=   "cboMStatus"
      Tab(1).Control(70).Enabled=   0   'False
      Tab(1).Control(71)=   "cmdPlan"
      Tab(1).Control(71).Enabled=   0   'False
      Tab(1).Control(72)=   "cboArea"
      Tab(1).Control(72).Enabled=   0   'False
      Tab(1).Control(73)=   "cmdRenew"
      Tab(1).Control(73).Enabled=   0   'False
      Tab(1).Control(74)=   "cmdANC"
      Tab(1).Control(74).Enabled=   0   'False
      Tab(1).Control(75)=   "cmdPatDep"
      Tab(1).Control(75).Enabled=   0   'False
      Tab(1).Control(76)=   "cboClient"
      Tab(1).Control(76).Enabled=   0   'False
      Tab(1).Control(77)=   "txtEmp"
      Tab(1).Control(77).Enabled=   0   'False
      Tab(1).Control(78)=   "txtKinRel"
      Tab(1).Control(78).Enabled=   0   'False
      Tab(1).Control(79)=   "txtNOKPhone"
      Tab(1).Control(79).Enabled=   0   'False
      Tab(1).Control(80)=   "txtKin"
      Tab(1).Control(80).Enabled=   0   'False
      Tab(1).Control(81)=   "txtKinAddress"
      Tab(1).Control(81).Enabled=   0   'False
      Tab(1).Control(82)=   "txtPolicy"
      Tab(1).Control(82).Enabled=   0   'False
      Tab(1).Control(83)=   "cmdTake"
      Tab(1).Control(83).Enabled=   0   'False
      Tab(1).Control(84)=   "cmdPix"
      Tab(1).Control(84).Enabled=   0   'False
      Tab(1).Control(85)=   "cmdComm"
      Tab(1).Control(85).Enabled=   0   'False
      Tab(1).Control(86)=   "cboIntro"
      Tab(1).Control(86).Enabled=   0   'False
      Tab(1).Control(87)=   "cboRefHmo"
      Tab(1).Control(87).Enabled=   0   'False
      Tab(1).Control(88)=   "CboRef"
      Tab(1).Control(88).Enabled=   0   'False
      Tab(1).Control(89)=   "chkAll"
      Tab(1).Control(89).Enabled=   0   'False
      Tab(1).Control(90)=   "Frame1"
      Tab(1).Control(90).Enabled=   0   'False
      Tab(1).Control(91)=   "txtAdmitLimit"
      Tab(1).Control(91).Enabled=   0   'False
      Tab(1).ControlCount=   92
      TabCaption(2)   =   "Attendance History"
      TabPicture(2)   =   "frmPatientsNew.frx":0038
      Tab(2).ControlEnabled=   0   'False
      Tab(2).Control(0)=   "grdAttend"
      Tab(2).Control(1)=   "cmdAttend"
      Tab(2).ControlCount=   2
      Begin VB.TextBox txtAdmitLimit 
         Appearance      =   0  'Flat
         Height          =   285
         Left            =   1845
         TabIndex        =   6
         Text            =   "0"
         Top             =   1980
         Width           =   570
      End
      Begin VB.Frame Frame1 
         Caption         =   "Invi for Appt"
         Height          =   1035
         Left            =   10920
         TabIndex        =   112
         Top             =   6000
         Visible         =   0   'False
         Width           =   2325
         Begin MSComCtl2.DTPicker dtNext 
            Height          =   375
            Left            =   -30
            TabIndex        =   113
            Top             =   510
            Width           =   1470
            _ExtentX        =   2593
            _ExtentY        =   661
            _Version        =   393216
            CalendarTitleBackColor=   14737632
            CheckBox        =   -1  'True
            Format          =   107151361
            CurrentDate     =   38611
         End
         Begin MSComCtl2.DTPicker dtRefTime 
            Height          =   375
            Left            =   1455
            TabIndex        =   114
            Top             =   510
            Width           =   1905
            _ExtentX        =   3360
            _ExtentY        =   661
            _Version        =   393216
            CalendarTitleBackColor=   14737632
            CheckBox        =   -1  'True
            Format          =   107151362
            CurrentDate     =   38611
         End
         Begin VB.Label Label12 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Time"
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
            Left            =   1380
            TabIndex        =   116
            Top             =   240
            Width           =   510
         End
         Begin VB.Label Label9 
            BackStyle       =   0  'Transparent
            Caption         =   "Ref/Appt Date"
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
            Left            =   -30
            TabIndex        =   115
            Top             =   240
            Width           =   1635
         End
      End
      Begin VB.CommandButton cmdAttend 
         Caption         =   "Click to View"
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
         Left            =   -63720
         TabIndex        =   109
         Top             =   420
         Width           =   2010
      End
      Begin VB.CheckBox chkAll 
         Caption         =   "Show All"
         Height          =   285
         Left            =   4005
         TabIndex        =   107
         Top             =   1305
         Width           =   1005
      End
      Begin VB.ComboBox CboRef 
         Height          =   315
         ItemData        =   "frmPatientsNew.frx":0054
         Left            =   1845
         List            =   "frmPatientsNew.frx":0056
         Sorted          =   -1  'True
         Style           =   2  'Dropdown List
         TabIndex        =   8
         Top             =   2700
         Width           =   2265
      End
      Begin VB.ComboBox cboRefHmo 
         Height          =   315
         ItemData        =   "frmPatientsNew.frx":0058
         Left            =   1845
         List            =   "frmPatientsNew.frx":005A
         Sorted          =   -1  'True
         Style           =   2  'Dropdown List
         TabIndex        =   9
         Top             =   3060
         Width           =   1725
      End
      Begin VB.ComboBox cboIntro 
         Height          =   315
         Left            =   6480
         Style           =   2  'Dropdown List
         TabIndex        =   35
         Top             =   5100
         Width           =   5640
      End
      Begin VB.CommandButton cmdComm 
         BackColor       =   &H00FFFFC0&
         Caption         =   "Consultation History"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   8.25
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   300
         Left            =   7050
         TabIndex        =   103
         Top             =   7395
         Width           =   2340
      End
      Begin VB.CommandButton cmdPix 
         Caption         =   "Select Picture"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   11475
         TabIndex        =   59
         Top             =   2865
         Width           =   2205
      End
      Begin VB.CommandButton cmdTake 
         Caption         =   "Take Picture"
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   330
         Left            =   10215
         TabIndex        =   102
         Top             =   2865
         Visible         =   0   'False
         Width           =   1665
      End
      Begin VB.ComboBox txtPolicy 
         Height          =   315
         ItemData        =   "frmPatientsNew.frx":005C
         Left            =   1845
         List            =   "frmPatientsNew.frx":005E
         Style           =   2  'Dropdown List
         TabIndex        =   5
         Top             =   1635
         Width           =   2580
      End
      Begin VB.TextBox txtKinAddress 
         Height          =   735
         Left            =   6480
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   25
         Top             =   1830
         Width           =   3480
      End
      Begin VB.TextBox txtKin 
         Height          =   330
         Left            =   6480
         TabIndex        =   22
         Top             =   660
         Width           =   3390
      End
      Begin VB.TextBox txtNOKPhone 
         Height          =   330
         Left            =   6480
         TabIndex        =   24
         Top             =   1425
         Width           =   3435
      End
      Begin VB.ComboBox txtKinRel 
         Height          =   315
         Left            =   6480
         Sorted          =   -1  'True
         TabIndex        =   23
         Top             =   1065
         Width           =   3435
      End
      Begin VB.TextBox txtEmp 
         Height          =   330
         Left            =   1845
         TabIndex        =   7
         Top             =   2355
         Width           =   3300
      End
      Begin VB.ComboBox cboClient 
         Height          =   315
         ItemData        =   "frmPatientsNew.frx":0060
         Left            =   1845
         List            =   "frmPatientsNew.frx":0062
         Style           =   2  'Dropdown List
         TabIndex        =   4
         Top             =   1275
         Width           =   2130
      End
      Begin VB.CommandButton cmdPatDep 
         BackColor       =   &H00FFFFC0&
         Caption         =   "Add Dependents"
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
         Height          =   345
         Left            =   9525
         TabIndex        =   92
         Top             =   7365
         Width           =   1665
      End
      Begin VB.CommandButton cmdANC 
         BackColor       =   &H00FFFFC0&
         Caption         =   "Update Antenatal Information"
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
         Height          =   555
         Left            =   11355
         TabIndex        =   91
         Top             =   7125
         Width           =   2280
      End
      Begin VB.CommandButton cmdRenew 
         Caption         =   "Renew Card"
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
         Left            =   9945
         TabIndex        =   90
         Top             =   7845
         Width           =   2070
      End
      Begin VB.ComboBox cboArea 
         Height          =   315
         ItemData        =   "frmPatientsNew.frx":0064
         Left            =   1845
         List            =   "frmPatientsNew.frx":0066
         TabIndex        =   13
         Top             =   5010
         Width           =   3165
      End
      Begin VB.CommandButton cmdPlan 
         Caption         =   "New..."
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
         Left            =   4455
         TabIndex        =   89
         Top             =   1635
         Width           =   765
      End
      Begin VB.ComboBox cboMStatus 
         Height          =   315
         Left            =   6480
         TabIndex        =   31
         Top             =   3810
         Width           =   2940
      End
      Begin VB.ComboBox cboPat 
         Height          =   315
         Left            =   1845
         Sorted          =   -1  'True
         Style           =   2  'Dropdown List
         TabIndex        =   3
         Top             =   825
         Width           =   3795
      End
      Begin VB.CheckBox chkReg 
         Caption         =   "Automatic Attendance"
         Height          =   285
         Left            =   6510
         TabIndex        =   36
         Top             =   5835
         Width           =   240
      End
      Begin VB.ComboBox txtBG 
         Height          =   315
         Left            =   10485
         TabIndex        =   29
         Top             =   3405
         Width           =   1410
      End
      Begin VB.ComboBox txtGeno 
         Height          =   315
         Left            =   12915
         TabIndex        =   30
         Top             =   3405
         Width           =   735
      End
      Begin VB.ComboBox cboNew 
         Height          =   315
         ItemData        =   "frmPatientsNew.frx":0068
         Left            =   1845
         List            =   "frmPatientsNew.frx":006A
         Sorted          =   -1  'True
         Style           =   2  'Dropdown List
         TabIndex        =   1
         Top             =   465
         Width           =   1320
      End
      Begin VB.TextBox txtAge 
         Height          =   285
         Left            =   4050
         TabIndex        =   19
         Top             =   7035
         Width           =   2130
      End
      Begin VB.ComboBox cboMat 
         Height          =   315
         ItemData        =   "frmPatientsNew.frx":006C
         Left            =   1845
         List            =   "frmPatientsNew.frx":007F
         Style           =   2  'Dropdown List
         TabIndex        =   17
         Top             =   6585
         Width           =   3120
      End
      Begin VB.ComboBox cboFile 
         Height          =   315
         ItemData        =   "frmPatientsNew.frx":00A8
         Left            =   6480
         List            =   "frmPatientsNew.frx":00AA
         Sorted          =   -1  'True
         Style           =   2  'Dropdown List
         TabIndex        =   27
         Top             =   3000
         Width           =   2985
      End
      Begin VB.TextBox txtPix 
         Enabled         =   0   'False
         Height          =   330
         Left            =   11160
         Locked          =   -1  'True
         TabIndex        =   61
         Top             =   2190
         Visible         =   0   'False
         Width           =   2310
      End
      Begin VB.ComboBox txtSex 
         Height          =   315
         Left            =   1845
         Style           =   2  'Dropdown List
         TabIndex        =   15
         Top             =   5865
         Width           =   1950
      End
      Begin VB.TextBox txtPhone 
         Height          =   330
         Left            =   1800
         MaxLength       =   11
         TabIndex        =   20
         Top             =   7395
         Width           =   3120
      End
      Begin VB.TextBox txtSupp 
         BackColor       =   &H8000000F&
         Height          =   330
         Left            =   10575
         Locked          =   -1  'True
         TabIndex        =   60
         Top             =   2370
         Visible         =   0   'False
         Width           =   3120
      End
      Begin VB.TextBox txtOLD 
         Height          =   330
         Left            =   1845
         TabIndex        =   14
         Top             =   5460
         Width           =   3120
      End
      Begin VB.TextBox txtEmail 
         Height          =   330
         Left            =   1800
         TabIndex        =   21
         Top             =   7755
         Width           =   2520
      End
      Begin VB.TextBox txtsurNAme 
         Height          =   330
         Left            =   1845
         TabIndex        =   10
         Top             =   3435
         Width           =   3525
      End
      Begin VB.TextBox txtfirstNAme 
         Height          =   285
         Left            =   1845
         TabIndex        =   11
         Top             =   3840
         Width           =   3525
      End
      Begin VB.TextBox txtHomeAddress 
         Height          =   690
         Left            =   1845
         MultiLine       =   -1  'True
         ScrollBars      =   2  'Vertical
         TabIndex        =   12
         Top             =   4200
         Width           =   3525
      End
      Begin VB.ComboBox cboCard 
         Height          =   315
         Left            =   1845
         Style           =   2  'Dropdown List
         TabIndex        =   16
         Top             =   6225
         Width           =   3120
      End
      Begin VB.ComboBox cboTitle 
         Height          =   315
         ItemData        =   "frmPatientsNew.frx":00AC
         Left            =   3645
         List            =   "frmPatientsNew.frx":00AE
         Sorted          =   -1  'True
         TabIndex        =   2
         Top             =   465
         Width           =   1320
      End
      Begin VB.Frame Frame2 
         BackColor       =   &H00E0E0E0&
         Height          =   7215
         Left            =   -74730
         TabIndex        =   53
         Top             =   510
         Width           =   13155
         Begin VB.CommandButton cmdWaitList 
            BackColor       =   &H8000000C&
            Caption         =   "Get List"
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
            Left            =   11520
            TabIndex        =   122
            Top             =   4050
            Width           =   1575
         End
         Begin VB.Timer Timer1 
            Interval        =   60000
            Left            =   0
            Top             =   4095
         End
         Begin VB.CommandButton cmdSys 
            Caption         =   "Tel No"
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
            Left            =   7785
            TabIndex        =   88
            Top             =   180
            Width           =   1350
         End
         Begin VB.CommandButton cmdCard 
            Caption         =   "Card No"
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
            Left            =   6345
            TabIndex        =   86
            Top             =   180
            Width           =   1350
         End
         Begin VB.TextBox txtName 
            Appearance      =   0  'Flat
            BackColor       =   &H00FFFF80&
            Height          =   330
            Left            =   3240
            TabIndex        =   0
            Top             =   225
            Width           =   1380
         End
         Begin VB.CommandButton cmdOK 
            Caption         =   "By Name"
            Default         =   -1  'True
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
            Left            =   4725
            TabIndex        =   54
            Top             =   180
            Width           =   1530
         End
         Begin MSDataGridLib.DataGrid grdData 
            Height          =   3405
            Left            =   90
            TabIndex        =   55
            Top             =   630
            Width           =   12975
            _ExtentX        =   22886
            _ExtentY        =   6006
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
            Caption         =   "Double click a row to view information"
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
         Begin MSDataGridLib.DataGrid grdDoc 
            Height          =   2625
            Left            =   120
            TabIndex        =   119
            Top             =   4575
            Width           =   12930
            _ExtentX        =   22807
            _ExtentY        =   4630
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
            Caption         =   "Double click  an Item on the List to View Details"
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
         Begin VB.Label Label35 
            Alignment       =   2  'Center
            BackColor       =   &H00000000&
            Caption         =   "Doctors' Waiting List by Clinic (Daily Attendance)"
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
            Height          =   480
            Left            =   0
            TabIndex        =   120
            Top             =   4065
            Width           =   13920
         End
         Begin VB.Label Label27 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Quick Search "
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
            Left            =   750
            TabIndex        =   57
            Top             =   270
            Width           =   2400
         End
         Begin VB.Label lblFound 
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
            ForeColor       =   &H00FF0000&
            Height          =   195
            Left            =   9945
            TabIndex        =   56
            Top             =   270
            Width           =   2985
         End
      End
      Begin MSComDlg.CommonDialog CommonDialog1 
         Left            =   -75000
         Top             =   3660
         _ExtentX        =   847
         _ExtentY        =   847
         _Version        =   393216
      End
      Begin VB.TextBox txtOfficeAddress 
         Height          =   375
         Left            =   6480
         TabIndex        =   34
         Top             =   4620
         Width           =   7170
      End
      Begin VB.TextBox txtOccu 
         Height          =   330
         Left            =   6480
         TabIndex        =   33
         Top             =   4215
         Width           =   7125
      End
      Begin VB.ComboBox cboStatus 
         Height          =   315
         ItemData        =   "frmPatientsNew.frx":00B0
         Left            =   10485
         List            =   "frmPatientsNew.frx":00B2
         TabIndex        =   32
         Top             =   3765
         Width           =   3165
      End
      Begin MSComCtl2.DTPicker dtRegDate 
         Height          =   330
         Left            =   6480
         TabIndex        =   26
         Top             =   2640
         Width           =   1905
         _ExtentX        =   3360
         _ExtentY        =   582
         _Version        =   393216
         CheckBox        =   -1  'True
         Format          =   72613889
         CurrentDate     =   38611
      End
      Begin MSComCtl2.DTPicker dtExpDate 
         Height          =   375
         Left            =   6480
         TabIndex        =   28
         Top             =   3405
         Width           =   1950
         _ExtentX        =   3440
         _ExtentY        =   661
         _Version        =   393216
         CheckBox        =   -1  'True
         Format          =   72613889
         CurrentDate     =   38611
      End
      Begin MSComCtl2.DTPicker dtDate 
         Height          =   375
         Left            =   1845
         TabIndex        =   18
         Top             =   6990
         Width           =   1770
         _ExtentX        =   3122
         _ExtentY        =   661
         _Version        =   393216
         CheckBox        =   -1  'True
         Format          =   72613889
         CurrentDate     =   38611
      End
      Begin VB.Frame fraAttd 
         Caption         =   "   Take Attendance/Appointment"
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
         Height          =   1440
         Left            =   6420
         TabIndex        =   81
         Top             =   5880
         Width           =   4335
         Begin VB.ComboBox cboDoc 
            Enabled         =   0   'False
            Height          =   315
            ItemData        =   "frmPatientsNew.frx":00B4
            Left            =   1080
            List            =   "frmPatientsNew.frx":00B6
            Sorted          =   -1  'True
            Style           =   2  'Dropdown List
            TabIndex        =   39
            Top             =   1050
            Width           =   3120
         End
         Begin VB.ComboBox cboClin 
            Enabled         =   0   'False
            Height          =   315
            ItemData        =   "frmPatientsNew.frx":00B8
            Left            =   1080
            List            =   "frmPatientsNew.frx":00BA
            Sorted          =   -1  'True
            Style           =   2  'Dropdown List
            TabIndex        =   37
            Top             =   315
            Width           =   3120
         End
         Begin VB.ComboBox cboPurpose 
            Enabled         =   0   'False
            Height          =   315
            ItemData        =   "frmPatientsNew.frx":00BC
            Left            =   1080
            List            =   "frmPatientsNew.frx":00BE
            Sorted          =   -1  'True
            Style           =   2  'Dropdown List
            TabIndex        =   38
            Top             =   660
            Width           =   3120
         End
         Begin VB.Label Label39 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Doctor"
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
            Left            =   -900
            TabIndex        =   121
            Top             =   1095
            Width           =   1905
         End
         Begin VB.Label Label33 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Clinic"
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
            Left            =   -855
            TabIndex        =   83
            Top             =   360
            Width           =   1860
         End
         Begin VB.Label Label22 
            Alignment       =   1  'Right Justify
            BackStyle       =   0  'Transparent
            Caption         =   "Purpose"
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
            Left            =   -810
            TabIndex        =   82
            Top             =   705
            Width           =   1815
         End
      End
      Begin MSDataGridLib.DataGrid grdAttend 
         Height          =   6405
         Left            =   -74670
         TabIndex        =   108
         Top             =   930
         Width           =   12975
         _ExtentX        =   22886
         _ExtentY        =   11298
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
      Begin VB.Label Label4 
         BackColor       =   &H000000FF&
         Caption         =   "enter 0 to ignore"
         ForeColor       =   &H00FFFFFF&
         Height          =   240
         Index           =   0
         Left            =   2460
         TabIndex        =   118
         Top             =   1980
         Width           =   1605
      End
      Begin VB.Label Label34 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "Admission Limit (in hrs)"
         ForeColor       =   &H00FFFFFF&
         Height          =   255
         Left            =   150
         TabIndex        =   117
         Top             =   1995
         Width           =   1605
      End
      Begin VB.Label lblDebt 
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
         Height          =   240
         Left            =   6495
         TabIndex        =   111
         Top             =   5445
         Width           =   1710
      End
      Begin VB.Label lblDebtCap 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "Carry Over Debt"
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
         Left            =   5010
         TabIndex        =   110
         Top             =   5445
         Width           =   1395
      End
      Begin VB.Label Label20 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Fee for Service?"
         Height          =   240
         Left            =   495
         TabIndex        =   106
         Top             =   2745
         Width           =   1275
      End
      Begin VB.Label Label38 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "HMO Referal?"
         Height          =   240
         Left            =   630
         TabIndex        =   105
         Top             =   3150
         Width           =   1140
      End
      Begin VB.Label Label32 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Introduced By"
         Height          =   330
         Left            =   5310
         TabIndex        =   104
         Top             =   5145
         Width           =   1050
      End
      Begin VB.Label Label31 
         BackStyle       =   0  'Transparent
         Caption         =   "One GSM No ONLY"
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
         Left            =   4995
         TabIndex        =   101
         Top             =   7440
         Width           =   2355
      End
      Begin VB.Label Label25 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Address of NOK"
         Height          =   285
         Left            =   5175
         TabIndex        =   100
         Top             =   2100
         Width           =   1185
      End
      Begin VB.Label Label24 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "NOK Relationship"
         Height          =   240
         Left            =   4905
         TabIndex        =   99
         Top             =   1155
         Width           =   1500
      End
      Begin VB.Label Label23 
         BackStyle       =   0  'Transparent
         Caption         =   "Next of Kin (NOK)"
         Height          =   240
         Left            =   6480
         TabIndex        =   98
         Top             =   450
         Width           =   2985
      End
      Begin VB.Label Label36 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "NOK Phone"
         Height          =   330
         Left            =   4995
         TabIndex        =   97
         Top             =   1485
         Width           =   1365
      End
      Begin VB.Label Label6 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "System  No"
         Height          =   195
         Left            =   9450
         TabIndex        =   96
         Top             =   2685
         Visible         =   0   'False
         Width           =   1410
      End
      Begin VB.Label Label16 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "HMO Enrollee Plan"
         Height          =   195
         Index           =   2
         Left            =   270
         TabIndex        =   95
         Top             =   1680
         Width           =   1500
      End
      Begin VB.Label Label18 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Enrollee No"
         Height          =   195
         Left            =   675
         TabIndex        =   94
         Top             =   2445
         Width           =   1095
      End
      Begin VB.Label Label26 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Billing Cat (Tariff)"
         Height          =   240
         Left            =   495
         TabIndex        =   93
         Top             =   1320
         Width           =   1275
      End
      Begin VB.Label Label15 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Area/Location"
         Height          =   240
         Left            =   540
         TabIndex        =   87
         Top             =   5055
         Width           =   1185
      End
      Begin VB.Label Label2 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Marital Status"
         Height          =   240
         Left            =   5310
         TabIndex        =   85
         Top             =   3855
         Width           =   1050
      End
      Begin VB.Label Label17 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "Company Name"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   0
         Left            =   630
         TabIndex        =   84
         Top             =   870
         Width           =   1140
      End
      Begin VB.Label Label7 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Genotype"
         Height          =   195
         Index           =   2
         Left            =   12060
         TabIndex        =   80
         Top             =   3450
         Width           =   780
      End
      Begin VB.Label Label7 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Blood Group"
         Height          =   195
         Index           =   1
         Left            =   9270
         TabIndex        =   79
         Top             =   3450
         Width           =   1185
      End
      Begin VB.Label Label30 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "New/Existing Reg"
         Height          =   240
         Left            =   225
         TabIndex        =   78
         Top             =   510
         Width           =   1545
      End
      Begin VB.Label Label29 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Age"
         Height          =   195
         Left            =   3555
         TabIndex        =   77
         Top             =   7080
         Width           =   375
      End
      Begin VB.Label lblAge 
         BackStyle       =   0  'Transparent
         Caption         =   "Yrs"
         ForeColor       =   &H80000007&
         Height          =   195
         Left            =   4725
         TabIndex        =   76
         Top             =   4380
         Visible         =   0   'False
         Width           =   330
      End
      Begin VB.Label Label10 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Date of Birth"
         Height          =   195
         Left            =   315
         TabIndex        =   75
         Top             =   7080
         Width           =   1410
      End
      Begin VB.Label Label28 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Patient Maturity"
         Height          =   240
         Left            =   450
         TabIndex        =   74
         Top             =   6675
         Width           =   1275
      End
      Begin VB.Label Label12 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Card Duration"
         Height          =   195
         Index           =   5
         Left            =   5355
         TabIndex        =   73
         Top             =   3045
         Width           =   1005
      End
      Begin VB.Label Label19 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Exp Date"
         Height          =   240
         Left            =   5445
         TabIndex        =   72
         Top             =   3450
         Width           =   915
      End
      Begin VB.Label Label11 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Hospital Card No"
         Height          =   195
         Left            =   360
         TabIndex        =   71
         Top             =   5505
         Width           =   1365
      End
      Begin VB.Label Label13 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Reg Date"
         Height          =   195
         Left            =   5580
         TabIndex        =   70
         Top             =   2685
         Width           =   780
      End
      Begin VB.Label Label8 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Phone No"
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
         Left            =   585
         TabIndex        =   69
         Top             =   7455
         Width           =   1050
      End
      Begin VB.Label Label1 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Sex"
         Height          =   240
         Left            =   900
         TabIndex        =   68
         Top             =   5910
         Width           =   825
      End
      Begin VB.Label Label3 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Email"
         Height          =   195
         Left            =   1080
         TabIndex        =   67
         Top             =   7815
         Width           =   600
      End
      Begin VB.Image piX 
         Height          =   1995
         Left            =   11160
         Stretch         =   -1  'True
         Top             =   795
         Width           =   2505
      End
      Begin VB.Shape Shape1 
         Height          =   1995
         Left            =   11160
         Top             =   795
         Width           =   2505
      End
      Begin VB.Label Label7 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "Surname"
         ForeColor       =   &H00FFFFFF&
         Height          =   195
         Index           =   0
         Left            =   360
         TabIndex        =   66
         Top             =   3525
         Width           =   1410
      End
      Begin VB.Label Label4 
         Alignment       =   1  'Right Justify
         BackColor       =   &H000000FF&
         Caption         =   "First / OtherNames"
         ForeColor       =   &H00FFFFFF&
         Height          =   240
         Index           =   1
         Left            =   315
         TabIndex        =   65
         Top             =   3885
         Width           =   1455
      End
      Begin VB.Label Label9 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Home Address"
         Height          =   195
         Index           =   1
         Left            =   495
         TabIndex        =   64
         Top             =   4500
         Width           =   1275
      End
      Begin VB.Label Label14 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Card Type"
         Height          =   240
         Left            =   675
         TabIndex        =   63
         Top             =   6270
         Width           =   1050
      End
      Begin VB.Label Label37 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Title"
         Height          =   240
         Left            =   3240
         TabIndex        =   62
         Top             =   510
         Width           =   285
      End
      Begin VB.Label Label12 
         BackStyle       =   0  'Transparent
         Caption         =   "Clinic"
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
         Left            =   2250
         TabIndex        =   58
         Top             =   9150
         Width           =   2040
      End
      Begin VB.Label Label21 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Office Name"
         Height          =   330
         Left            =   5310
         TabIndex        =   44
         Top             =   4665
         Width           =   1050
      End
      Begin VB.Label Label7 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Occupation"
         Height          =   195
         Index           =   3
         Left            =   5400
         TabIndex        =   43
         Top             =   4260
         Width           =   960
      End
      Begin VB.Label Label7 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Religion"
         Height          =   195
         Index           =   4
         Left            =   9315
         TabIndex        =   42
         Top             =   3810
         Width           =   1050
      End
   End
   Begin VB.Label Label5 
      Alignment       =   2  'Center
      BackColor       =   &H00000000&
      Caption         =   "Patient Attendance / Registration"
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
      Height          =   465
      Left            =   0
      TabIndex        =   41
      Top             =   0
      Width           =   13920
   End
End
Attribute VB_Name = "frmPatientsNew"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
'
Option Explicit
Dim ageVal As Date
Dim iDNo As Long
Dim strCardNo As String
Dim strCDNo As String
Dim strPcat As String
Dim strPcatNum As String
Dim strDtCard As String
Dim strcardCat As String
Dim strCatP As String
Public flgpN As Boolean
Dim vVal As Long
Public strParam As String
Public flgEdit As Boolean
Dim IDConVal As Long
Dim strIDConVal As String
Dim strCval As String
'Dim strpCode As String
'Dim strEmpID As String
Dim strClinicGen As String
Dim strCard As String
Dim flgFill As Boolean
Dim flgHMO As Boolean
Dim strFileOLD As String
Dim strPrincFile As String
Dim strFile As String
'Dim strEmpIDRec As String
Dim strFldPath As String
'Dim Client As String
'Dim coyType As String
'Dim pCatID As String
Dim searchVal As String
Dim PatCat As String
Dim strPatNo As String
Dim delPath As String
Dim strFFS As String
Dim strFFS2 As String
Dim ClientType As String
Dim intSNo As Long

Dim strCoyID As String
Dim strBillTo As String
Dim isInsert As Integer  'to prevent dup inserts

Dim planName As String
Dim planID As String

Dim strIntro As String

Dim isNewEntry As Boolean


Private SelectedCamera As Integer '-1 means none selected.

Dim strAccumIDVal As Long

Dim strPrinNo As String
Dim principalNo As String
Dim isDependant As Boolean

Dim OldClientCode As String
Dim OldClient As String
Dim LastAttndDate As Date
Dim coyTypeForExistingPat As String
Dim CanLoad As Boolean

Const intWait As Integer = 3
Dim cnt2 As Integer


Dim Doctor As String
Dim DocAssigned As String

Private WithEvents m_cnW As ADODB.Connection
Attribute m_cnW.VB_VarHelpID = -1
Private WithEvents m_rsDocW As ADODB.Recordset
Attribute m_rsDocW.VB_VarHelpID = -1
' Flags to manage the state of the async operations
Private m_isConnecting As Boolean
'''Private m_isFetchingDateTime As Boolean

'''Private m_sysDateTimeFetched As Date
' ... (Other declarations)

' A flag to prevent the timer from triggering a new operation while one is in progress
Private m_isAsyncOpRunning As Boolean

Const intDoc  As Integer = 5
Private cntDoc As Integer

'Private WithEvents m_cnW As ADODB.Connection
'Private WithEvents m_cnW As ADODB.Connection



Private Declare Function SendMessage Lib "user32" Alias "SendMessageA" _
        (ByVal hWnd As Long, ByVal wMsg As Long, ByVal wParam As Long, lParam As Any) As Long
    Private Const CB_FINDSTRING = &H14C
    

Private Sub CancelButton_Click()
Unload Me
End Sub

Private Sub cboAppr_Click()
'If cboAppr.Text = "" Then Exit Sub
'strEmpIDRec = Mid(cboAppr.Text, InStr(cboAppr.Text, "[") + 1, Len(cboAppr.Text) - (InStr(cboAppr.Text, "[") + 1))

End Sub



Private Sub cboCard_Click()

If flgEdit = False And cboCard.Text = "FAMILY" Then  '''And strpCode = strPrivate Then
    isDependant = True
End If

'If flgEdit = True Then Exit Sub
'If flgFill = True Then Exit Sub
'
'If flgHMO = False Then
'Call genIDNo
'txtSupp.Locked = True
'Else
'flgHMO = False
'End If
'
''If flgEdit = True Then Exit Sub
'
'If cboCard.Text = "FAMILY" Then
'    strcardCat = "F"
'    strPcat = strPcatNum & "A"   'strPcat=CStr(Right("000000000" & CStr(iDNo), 9))
'
'ElseIf cboCard.Text = "SINGLE" Then
'    strcardCat = "S"
'    strPcat = strPcatNum & "A"   'CStr(Right("000000000" & CStr(iDNo), 9))
'
'
'End If
'
'
'
'
'
'
'
'
'
'
''strDtCard = CStr(Format(dtRegDate.Value, "yy")) & "/" & CStr(Format(dtRegDate.Value, "mm"))
'''strCardNo = "MED" & strCatP & "/" & strcardCat & "/" & strDtCard & "/" & strPcat
''strCardNo = "MED/" & strPcat & "/" & strCatP & strcardCat & "/" & strDtCard
'
'txtSupp.Text = strCardNo
'
'
'
'
'
'
'
'
'
'
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'If cboCard.Text = "FAMILY" Then
'    cboClinic.Text = "(GENERAL)"
'Else
'    cboClinic.ListIndex = 0
'
'
'End If
'
''Call genIDNo
''
''strPcat = strPcatNum
''strCardNo = HNo & "/" & strPcat
''
''txtSupp.Text = strCardNo
'
'
'


End Sub

Private Sub cboCard_LostFocus()
'Dim intSave As Integer
'        If cboCard.Text = "FAMILY" Then
'            intSave = MsgBox(" Do you want to enter Dependants along with this file", vbYesNo, "Dependants File")
'            If intSave = vbYes Then
'                'flgHMO = True
'                txtSupp.Locked = False
'            Else
'                'lgHMO = False
'                txtSupp.Locked = True
'
'            End If
'        End If

End Sub

Private Sub cboCat_Click()
'On Error Resume Next
'
'If cboCat.ListIndex = 0 Or cboCat.ListIndex = -1 Then Exit Sub
'
'cboCat.Enabled = True 'nece
'
'If cboCat.Text = "CREDIT" Then
'    cboCat2.Clear
'    cboCat2.AddItem ""
'    cboCat2.AddItem "CORPORATE"
'    cboCat2.AddItem "CREDIT-PRIVATE"
'    ''cboCat2.AddItem "FEE-PAYING"
'
'    cboCat3.Clear
'    cboCat3.AddItem ""
'    'cboCat4.Clear
'    'cboCat4.AddItem ""
'
'    cboCat2.Enabled = True
'    cboCat3.Enabled = True
'    'cboCat4.Enabled = True
'Else
'    cboCat2.Clear
'    cboCat2.AddItem ""
'    cboCat2.AddItem "CORPORATE"
'    cboCat2.AddItem "CREDIT-PRIVATE"
'    cboCat2.AddItem "FEE-PAYING"
'
'
'
'    If cboCat3.Text <> "FEE-PAYING" Then
'        cboCat3.Clear
'        cboCat3.AddItem ""
'        cboCat3.AddItem "COMPANY"
'        cboCat3.AddItem "HMO"
'        'cboCat3.AddItem "FEE-PAYING"
'        cboCat3.Enabled = True
'    Else
'        cboCat3.Enabled = False
'    End If
'
'
'    cboCat.Enabled = False
'    cboCat2.Enabled = False
'    cboCat3.Enabled = False
'    'cboCat4.Enabled = False
'
'    cboCat2.AddItem "FEE-PAYING"
'    cboCat3.AddItem "FEE-PAYING"
'    'cboCat4.AddItem "FEE-PAYING"
'
'    cboCat2.Text = "FEE-PAYING"
'    cboCat3.Text = "FEE-PAYING"
'    'cboCat4.Text = "FEE-PAYING"
'
'
'    cboClient.Clear
'    cboClient.AddItem ""
'    cboClient.AddItem "FEE-PAYING"
'    cboClient.Text = "FEE-PAYING"
'    cboClient.Enabled = False
'
'
'End If
'
'
'
'
'
''If flgEdit = True Then Exit Sub
''If flgFill = True Then Exit Sub
''If flgHMO = False Then
''Call genIDNo
''txtSupp.Locked = True
''Else
''flgHMO = False
''End If
''
''If cboCard.Text = "FAMILY" Then
''strPcat = strPcatNum '& "A" 'strPcat = strPcat & "A"   'strPcat=CStr(Right("000000000" & CStr(iDNo), 9))
''
''Else
''strPcat = strPcatNum '& "A" 'its ok 'CStr(Right("000000000" & CStr(iDNo), 9))
''End If
''
'''If cboCat.Text = "CORPORATE" Then
'''Frame1.Enabled = True
'''Else
'''Frame1.Enabled = False
'''End If
''
''If cboCat.Text = "PRIVATE" Then
''
''    'cbo.Text = "PRIVATE"
''    'cboPat.Text = "PRIVATE"
''    cboClient.Text = "PRIVATE"   'getClientCatID(cboCat.Text)
''
''
''    strCatP = "P"
''
''    cboFile.Text = "TILL END OF YEAR"
''    Call cboFile_Click
''
''Else
''
'''    cboType.ListIndex = 0
'''    cboPat.ListIndex = 0
'''    cboClient.ListIndex = 0
''
''    strCatP = "C"
''
''
''    'dtExpDate.Enabled = False
''End If
''
''
''
''strDtCard = CStr(Format(dtRegDate.Value, "yy")) & "/" & CStr(Format(dtRegDate.Value, "mm"))
''strCardNo = "MED/" & strPcat & "/" & strCatP & strcardCat & "/" & strDtCard
''
''txtSupp.Text = strCardNo
'
End Sub



Private Sub cboCat2_Click()

'If cboCat2.ListIndex = 0 Or cboCat2.ListIndex = -1 Then Exit Sub
'
'  If cboCat3.ListCount <= 0 Then
'        cboCat3.Clear
'        cboCat3.AddItem ""
'        cboCat3.AddItem "COMPANY"
'        cboCat3.AddItem "HMO"
'        ''cboCat3.AddItem "FEE-PAYING"
'    End If
'
'    If cboCat2.Text = "CREDIT-PRIVATE" Then
'
'
''        cboCat.Enabled = False
''        cboCat2.Enabled = False
'        cboCat3.Enabled = False
'        'cboCat4.Enabled = False
'
'        'cboCat2.AddItem "FEE-PAYING"
'        cboCat3.AddItem "CREDIT-PRIVATE"
'        'cboCat4.AddItem "CREDIT-PRIVATE"
'
'        'cboCat2.Text = "FEE-PAYING"
'        cboCat3.Text = "CREDIT-PRIVATE"
'        'cboCat4.Text = "CREDIT-PRIVATE"
'
'
'        cboClient.Clear
'        cboClient.AddItem ""
'        cboClient.AddItem "CREDIT-PRIVATE"
'        cboClient.Text = "CREDIT-PRIVATE"
'        cboClient.Enabled = False
'
'    ElseIf cboCat2.Text = "FEE-PAYING" Then
'
'        cboCat3.Clear
'        cboCat3.AddItem ""
'        cboCat3.AddItem "FEE-PAYING"
'        cboCat3.Text = "FEE-PAYING" '"PRIVATE"   'getClientCatID(cboCat.Text)
'
'    Else
'
'
''        cboCat.Enabled = False
''        cboCat2.Enabled = False
'        cboCat3.Enabled = True
'        'cboCat4.Enabled = True
'
'      If cboCat3.Text <> "FEE-PAYING" Then
'            cboCat3.Clear
'            cboCat3.AddItem ""
'            cboCat3.AddItem "COMPANY"
'            cboCat3.AddItem "HMO"
'        cboCat3.Enabled = True
'      Else
'        cboCat3.Enabled = False
'       End If
'        'cboCat4.Clear
'        'cboCat4.AddItem ""
'
'        cboClient.Clear
'        cboClient.AddItem ""
'        'cboCat4.Enabled = True
'        cboClient.Enabled = True
'
'
'        ''cboCat3.AddItem "FEE-PAYING"
'        'cboClient.AddItem "FEE-PAYING"
'        'cboCat2.AddItem "PRIVATE"
'        ''cboClient.AddItem "MTHLY"
'        ''cboClient.AddItem "3MTHLY"
'        ''cboClient.AddItem "6MTHLY"
'        ''cboClient.AddItem "CBN"
'        ''cboClient.AddItem "NEPA"
'    End If
'
'
'
'
'
'
'
'' End If
'

End Sub

Private Sub cboCat3_Click()
'If cboCat3.ListIndex = 0 Or cboCat3.ListIndex = -1 Then Exit Sub
'
'    If cboCat3.Text = "COMPANY" Then
'        cboClient.Clear
'        cboClient.AddItem ""
'        'cboCat2.AddItem "NHIS"
'        'cboCat2.AddItem "PHIS"
'        'cboClient.AddItem "FEE-PAYING"
'        'cboCat2.AddItem "PRIVATE"
'        cboClient.AddItem "MTHLY"
'        cboClient.AddItem "3MTHLY"
'        cboClient.AddItem "6MTHLY"
'        cboClient.AddItem "CBN"
'        cboClient.AddItem "NEPA"
'    Else
'
'        If cboCat3.Text <> "FEE-PAYING" Then
'            cboClient.Clear
'            cboClient.AddItem ""
'            cboClient.AddItem "NHIS"
'            cboClient.AddItem "PHIS"
'            'cboClient.AddItem "FEE-PAYING"
'            'cboCat2.AddItem "PRIVATE"
'            ''cboClient.AddItem "MTHLY"
'            ''cboClient.AddItem "3MTHLY"
'            ''cboClient.AddItem "6MTHLY"
'            ''cboClient.AddItem "CBN"
'            ''cboClient.AddItem "NEPA"
'        End If
'    End If
'
'
'
'
''    If cboCat3.Text = "FEE-PAYING" Then
''        'cboCat4.Clear
''        'cboCat4.AddItem ""
''        'cboCat4.AddItem "NHIS"
''        'cboCat4.AddItem "PHIS"
''        'cboCat4.AddItem "FEE-PAYING"
''    Else
''        'cboCat4.Clear
''        'cboCat4.AddItem ""
''        'cboCat4.AddItem "NHIS"
''        'cboCat4.AddItem "PHIS"
''        '''cboCat4.AddItem "FEE-PAYING"
''
''    End If
End Sub

Private Sub cboClient_Click()
On Error Resume Next
If cboClient.Text = "" Then Exit Sub

Screen.MousePointer = vbHourglass
    strTariff = cboClient.Text


'If cboClient.Text = "PRIVATE" Then  'not strprivate
'    'pCatID = "PRIVATE"
'    coyType = "PRIVATE"
'Else
'    'pCatID = "COMPANY"
'    Select Case cboClient.Text
'    Case "HMO", "PHIS"
'        coyType = "HMO"
'    Case "NHIS"
'    coyType = "NHIS"
'    Case Else
'    coyType = "COMPANY"
'    End Select
'End If

Dim rs As New Recordset
With rs
.Open "select distinct clientType  from billingPrice where clientCatID='" & cboClient.Text & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not .EOF Then
    coyType = !ClientType
Else
    coyType = "COMPANY"
End If
End With
    

If cboClient.Text = "NHIS" Or cboClient.Text = "HMO" Or cboClient.Text = "PHIS" Then
    Call loadPlan
Else
    txtPolicy.Clear 'very ok here
    txtPolicy.AddItem ""
    
    txtPolicy.AddItem "NIL"
    txtPolicy.Text = "NIL"
End If

Screen.MousePointer = vbDefault


End Sub



Private Sub cboClinic_Click()
'If cboClinic.Text = "GENERAL" Or cboClinic.Text = "(GENERAL)" Or cboClinic.Text = "OUT-PATIENT" Then ' ie Out-Patient
'    strClinicGen = "(GENERAL)"
'    'cboCard.ListIndex = 0
'Else
'    strClinicGen = cboClinic.Text
'    'cboCard.Text = "SINGLE"
'End If
End Sub


Private Sub getDocWithMinNumOfPat()
On Error GoTo errH
Dim rsBL As New Recordset
Dim StrDocX As String
With rsBL
    '.Open "select top 1 DocName,empID,RoomNo  from vwDocMinNumOfPat where ClinicID='" & strClinicID & "'", conSTR, adOpenForwardOnly, adLockReadOnly
    .Open "select  top 1 EmpID,Doctor as fullName from vwDocMinNumOfPat where Date='" & sysDate & "' and ClinicID='" & strClinicID & "'  order by NumOfPat asc", conStr, adOpenForwardOnly, adLockReadOnly
    If Not .EOF Then
        StrDocX = !fullName & " [" & !empID & "]"
        cboDoc.Text = StrDocX
    Else
        cboDoc.ListIndex = -1
    End If
End With

Exit Sub
errH: '''silent eror
'MsgBox Err.Description
End Sub

Private Sub cboClin_Click()
On Error GoTo errH

If cboClin.ListIndex = -1 Or cboClin.ListIndex = 0 Then Exit Sub

strClinicID = cboClin.Text

    cboDoc.Clear
    cboDoc.AddItem ""
    Dim rsDoc As New Recordset
    With rsDoc
        If Enforce_Assign_To_Doctor_In_Attendance = "YES" Then
            '.Open "select distinct empID,fullname from vwUsers where loginRole ='CONSULTING' and AccountStatus='ENABLED' order by Fullname", conSTR, adOpenForwardOnly, adLockReadOnly
            .Open "select distinct EmpID,Doctor as fullname from vwDocAssignedOnDutyRoster where ClinicID='" & strClinicID & "'  order by Doctor", conStr, adOpenForwardOnly, adLockReadOnly
        Else
            '.Open "select distinct empID,fullname from vwUsers where loginRole ='CONSULTING' and AccountStatus='ENABLED' order by Fullname", conSTR, adOpenForwardOnly, adLockReadOnly
             .Open "select distinct EmpID,Doctor as fullname from qryUserRoleClinic where ClinicID='" & strClinicID & "'  order by Doctor", conStr, adOpenForwardOnly, adLockReadOnly
        End If
        If Not .EOF Then
            .MoveFirst
            Do While Not .EOF
                cboDoc.AddItem !fullName & " [" & !empID & "]"
                .MoveNext
            Loop
            
            '''auto assign doctor based on min num of pats
            If Enforce_Assign_To_Doctor_In_Attendance = "YES" Then
                If cboDoc.ListCount > 1 Then ''''not 0, first item is ""
                    Call getDocWithMinNumOfPat
                End If
            Else
                If cboDoc.ListCount = 2 Then
                    cboDoc.Text = cboDoc.List(1)
                Else
                    cboDoc.ListIndex = -1
                End If
            End If
        Else
            If .State = adStateOpen Then .Close
            .Open "select distinct empID,fullname from vwUsers where loginRole ='CONSULTING' and AccountStatus='ENABLED' order by Fullname", conStr, adOpenForwardOnly, adLockReadOnly
            If Not .EOF Then
                .MoveFirst
                Do While Not .EOF
                    cboDoc.AddItem !fullName & " [" & !empID & "]"
                    .MoveNext
                Loop
            End If
        End If
    .Close
End With

       


'Dim rsDoc As New ADODB.Recordset
'With rsDoc
'
'    If Enforce_Assign_To_Doctor_In_Attendance = "YES" Then
'        .Open "select DocNo from hConRoomAssign where schdDate='" & sysDate & "'", conSTR, adOpenStatic, adLockOptimistic
'        If .EOF Then
'            MsgBox "Please Assign Doctors to Consulting Rooms before Attendance can be Taken", vbInformation, "OK"
'            cboClin.ListIndex = -1
'            frmSchedDoctor.Hide
'            frmSchedDoctor.Show
'            Exit Sub
'        End If
'    End If
'
'    If .State = adStateOpen Then .Close
'    If Assign_Doctors_to_Consulting_Rooms = "YES" Then
'        .Open "select distinct EmpID,Doctor,ConRoomNo as RoomNo from vwDocClinicAndPatAssignedAll where IsOff=0 and Date='" & sysDate & "' and ClinicID='" & strClinicID & "'  order by Doctor", conSTR, adOpenForwardOnly, adLockReadOnly
'    Else
'        .Open "select distinct EmpID,Doctor,'RM XXX' as RoomNo from qryUserRoleClinic where ClinicID='" & strClinicID & "'  order by Doctor", conSTR, adOpenForwardOnly, adLockReadOnly
'    End If
'
'    If Not .EOF Then
'        Do While Not .EOF
'            cboDoc.AddItem !Doctor & " @ " & !RoomNo & " [" & !empID & "]"
'            .MoveNext
'        Loop
'
'        Call getDocWithMinNumOfPat
'    End If
'
'
'End With
    


If LockAttendanceForInPatients = "YES" Then
        Dim rsAdm As New Recordset
        With rsAdm
            .Open "select pno from qryhAdmission where pno='" & txtSupp.Text & "'", conStr, adOpenStatic, adLockOptimistic
            If Not .EOF Then
                MsgBox "This Patient is Still On Admission!! Discharge Patient Before Attendance can be Taken", vbInformation, "LockAttendanceForInPatients"
                cboClin.ListIndex = -1
                'SetButtons (True)
                'clearFields
                Exit Sub
            End If
        End With
End If





    If LockMultipleAttendnace = "YES" Then
        flgDup = False
        flgDup = isDuplicate(txtSupp.Text)
        If flgDup = True Then
            MsgBox "Patient Attendance already Taken! Duplicate not allowed!! Proceed to see Nurse/Doctor", vbInformation, "LockMultipleAttendnace"
            cboClin.ListIndex = -1
            'SetButtons (True)
            'clearFields
            Exit Sub
        End If
    End If
    
    
    
    
If Enforce_Yearly_Re_Registration_Private = "YES" Then 'nece here

    If IsNull(dtExpDate.Value) And strpCode = strPrivate Then 'nece here
        MsgBox " Please Specify Expiration Date for this Private Patient"
        dtExpDate.SetFocus
        cboClin.ListIndex = -1
        Exit Sub
    End If



    'If IsNull(dtExpDate.Value) And strpCode = strPrivate Then
    If strpCode = strPrivate Then
        'MsgBox DateAdd("YYYY", 1, dtExpDate.Value)
        If CDate(dtExpDate.Value) <= sysDate Then
            cboClin.ListIndex = -1
            MsgBox "Patient's Card has Expired"
            
            Dim cmdExp As New Command
            Dim strDel As String
            Dim sSQlx As String
            sSQlx = "update  hpatients set " & _
            "expired =1 where pno='" & txtSupp.Text & "'"
        
            '"oldPno='" & Trim(txtNewCard.Text) & "'," & _

            
            cmdExp.ActiveConnection = conStr
            cmdExp.CommandText = sSQlx
            cmdExp.CommandType = adCmdText
            cmdExp.Execute
            cmdRenew.Enabled = True
            
            Exit Sub
        Else
            cmdRenew.Enabled = False
        End If
    End If
End If

    


Exit Sub
errH:
MsgBox Err.Description
End Sub


Private Sub cboDoc_Click()
On Error GoTo errH
DocAssigned = ""
If cboDoc.ListIndex = -1 Or cboDoc.ListIndex = 0 Then Exit Sub
    
DocAssigned = Mid(cboDoc.Text, InStr(cboDoc.Text, "[") + 1, Len(cboDoc.Text) - (InStr(cboDoc.Text, "[") + 1))
Doctor = Trim(Mid(cboDoc.Text, 1, InStr(cboDoc.Text, "[") - 2))

Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cboFile_Click()
On Error GoTo errH
If cboFile.ListIndex = -1 Then Exit Sub

If flgFill = True Then Exit Sub

If Enforce_Yearly_Re_Registration_Private = "YES" Then
    dtExpDate.Value = sysDate
    If cboFile.Text = "WHILE STILL IN SERVICE" And flgEdit = False Then
        dtExpDate.Value = ""
    
    ElseIf cboFile.Text = "ONE YEAR" And flgEdit = False Then
        dtExpDate.Enabled = True
        dtExpDate.CheckBox = True
        dtExpDate.Value = DateAdd("YYYY", 1, dtRegDate.Value)
    ElseIf cboFile.Text = "TILL END OF YEAR" And flgEdit = False Then
        dtExpDate.Enabled = True
        dtExpDate.CheckBox = True
        dtExpDate.Value = CDate("31-dec-" & Year(dtExpDate.Value))
    
    Else
        dtExpDate.Value = ""
    End If
Else
    'cboFile.Text = "WHILE STILL IN SERVICE"
    dtExpDate.Value = ""
End If

Exit Sub
errH:
MsgBox Err.Description
End Sub



Private Sub cboIntro_Click()
If cboIntro.ListIndex = -1 Or cboIntro.ListIndex = 0 Then Exit Sub
    
strIntro = ""

strIntro = Mid(cboIntro.Text, InStr(cboIntro.Text, "[") + 1, Len(cboIntro.Text) - (InStr(cboIntro.Text, "[") + 1))
'Client = Mid(cboPat.Text, 1, InStr(cboPat.Text, "[") - 2)


End Sub

Private Sub cboMat_Click()
On Error Resume Next
If cboMat.Text = "NEONATE" Then
    cboClin.Text = "(IN-PATIENT)"
Else
    cboClin.ListIndex = -1
End If
End Sub

Private Sub cboNew_Click()
'On Error GoTo errH
'  Dim rsBL As New Recordset
'  With rsBL
'  .Open "select DocName,Date from vwDocAssign where date='" & Date & "'", conSTR, adOpenForwardOnly, adLockReadOnly
'  If .EOF Then
'    MsgBox "Please Assign Doctors to Consulting Rooms"
'    frmSchedDoctor.Hide
'    frmSchedDoctor.Show
'    cboNew.ListIndex = 0
'    Exit Sub
'End If
'End With
'Exit Sub
'errH:
'MsgBox Err.Description
End Sub



Private Sub cboPat_Click()
On Error GoTo errH
If cboPat.ListIndex = -1 Or cboPat.ListIndex = 0 Then Exit Sub
    
Screen.MousePointer = vbHourglass

    
    dtExpDate.Value = ""

strpCode = Mid(cboPat.Text, InStr(cboPat.Text, "[") + 1, Len(cboPat.Text) - (InStr(cboPat.Text, "[") + 1))
Client = Trim(Mid(cboPat.Text, 1, InStr(cboPat.Text, "[") - 2))

strCoyID = ""
strCoyID = strpCode
intSNo = cboPat.ItemData(cboPat.ListIndex)
'retrieve val for clientcatID and ffs
ClientType = ""

cboClient.Clear
cboClient.AddItem ""


 Dim rsBL As New Recordset
  With rsBL
strFFS = ""
.Open "select distinct ClientType,retainCode from hRetainerShip where retainID='" & strCoyID & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not .EOF Then
    ClientType = !ClientType & ""
    strBillTo = !retainCode & ""
        
Else
    'strFFS = "NO"
    'strFFS2 = "NO"
End If

'CboRef.Text = strFFS


.Close
.Open "select distinct clientCatID  from billingPrice where clientType='" & ClientType & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not .EOF Then
    .MoveFirst
    Do While Not .EOF
    cboClient.AddItem !clientCatID
    .MoveNext
    Loop
Else
    Dim rx As New Recordset
    rx.Open "select distinct clientCatID  from billingPrice", conStr, adOpenForwardOnly, adLockReadOnly
    If Not rx.EOF Then
        rx.MoveFirst
        Do While Not rx.EOF
        cboClient.AddItem rx!clientCatID
        rx.MoveNext
        Loop
    End If
End If
End With

If strCoyID = strPrivate Or ClientType = ClientCatPrivate Then
    cboClient.Text = "PRIVATE"
    txtEmp.Text = "NIL"
    txtPolicy.Text = "NIL"
    cboRef.Text = "YES"
    cboRefHmo.Text = "NO"
    
    If Enforce_Yearly_Re_Registration_Private = "YES" Then
        cboFile.Text = "ONE YEAR"
    Else
        cboFile.Text = "WHILE STILL IN SERVICE"
    End If
    
Else
    ''for now '' in cmdAdd
'    cboFile.ListIndex = -1
'    cboClient.ListIndex = -1
'    txtEmp.Text = ""
'    txtPolicy.ListIndex = -1
'    CboRef.ListIndex = -1
'    cboRefHmo.ListIndex = -1
'    'cboCard.Text = "SINGLE"
End If

    cboCard.Text = "SINGLE"

'If strCoyID = "" Then not nece here 'strpCode shld have a val
'    MsgBox "Registration Incomplete! Company Name Required!! Go to Registration Page"
'    Call clearFields
'End If

'cboCat.Clear
'cboCat.AddItem ""
'cboCat.AddItem "CREDIT"
'cboCat.AddItem "FEE-PAYING"





'    If Client = "(FEE-PAYING)" Or Client = "FEE-PAYING" Then      'strpCode = "PRIVATE" Then
'
'
'        cboCat3.Clear
'        cboCat3.AddItem ""
'        cboCat3.AddItem "FEE-PAYING"
'        cboCat3.Text = "FEE-PAYING" '"PRIVATE"   'getClientCatID(cboCat.Text)
'         cboCat3.Enabled = False '"PRIVATE"   'getClientCatID(cboCat.Text)
'
'
'        cboClient.Clear
'        cboClient.AddItem ""
'        cboClient.AddItem "FEE-PAYING"
'        cboClient.Text = "FEE-PAYING" '"PRIVATE"   'getClientCatID(cboCat.Text)
'        strCatP = "P"
'        cboFile.Text = "ONE YEAR"
'        cboCat.Text = "FEE-PAYING"
'        Call cboFile_Click
'
'     ElseIf Client = "(CREDIT-PRIVATE)" Or Client = "CREDIT-PRIVATE" Then      'strpCode = "PRIVATE" Then
'
'
'        cboCat.Clear
'        cboCat.AddItem ""
'        cboCat.AddItem "CREDIT"
'        cboCat.Text = "CREDIT" '"PRIVATE"   'getClientCatID(cboCat.Text)
'        'cboCat3.Enabled = False '"PRIVATE"   'getClientCatID(cboCat.Text)
'
'        cboCat2.Clear
'        cboCat2.AddItem ""
'        cboCat2.AddItem "CREDIT-PRIVATE"
'        cboCat2.Text = "CREDIT-PRIVATE" '"PRIVATE"   'getClientCatID(cboCat.Text)
'
'
'        'cboClient.Clear
'        'cboClient.AddItem ""
'        'cboClient.AddItem "FEE-PAYING"
'        'cboClient.Text = "FEE-PAYING" '"PRIVATE"   'getClientCatID(cboCat.Text)
'        strCatP = "P"
'        cboFile.Text = "ONE YEAR"
'        'cboCat.Text = "FEE-PAYING"
'
'        strCatP = "P"
'
'        Call cboFile_Click
'
'   Else
'        cboCat.Text = "CREDIT"
'        cboClient.ListIndex = -1
'        'cboType.ListIndex = 0
'        'cboPat.ListIndex = 0
'        strCatP = "C"
'
'        'dtExpDate.Enabled = false
'    End If
'
'
'If flgFill = True Then Exit Sub 'ok here below strpCode
'
'    'If cboPat.Text = "" Then Exit Sub
'    'strpCode = Right("0000" & cboPat.ItemData(cboPat.ListIndex), 4)
'
'    'strCoyID = Mid(cboPat.Text, InStr(cboPat.Text, "[") + 1, Len(cboPat.Text) - (InStr(cboPat.Text, "[") + 1))
'
'    'If cboPat.Text = "PRIVATE" Then
'    '    cboCat.Text = "PRIVATE"
'    '    cboClient.Text = "PRIVATE"
'    '    cboFile.Text = "TILL END OF YEAR"
'    'Else
'    '    cboFile.Text = "WHILE STILL IN SERVICE"
'    '    cboCat.Text = "COMPANY"
'    '    'cboClient.ListIndex = 0
'    'End If
'
' If flgEdit = False Then
'        If flgHMO = False Then
'        'Call genIDNo
'        txtSupp.Locked = True
'        Else
'        flgHMO = False
'        End If
'
''    strPcat = strPcatNum & "A"   'strPcat=CStr(Right("000000000" & CStr(iDNo), 9))
''    strDtCard = CStr(Format(dtRegDate.Value, "yy")) & "/" & CStr(Format(dtRegDate.Value, "mm"))
''    'strCardNo = "MED" & strCatP & "/" & strcardCat & "/" & strDtCard & "/" & strPcat
''    strCardNo = "MED/" & strPcat & "/" & strCatP & strcardCat & "/" & strDtCard
''
''    txtSupp.Text = strCardNo
'
'    'strDtCard = CStr(Format(dtRegDate.Value, "yy")) & "/" & CStr(Format(dtRegDate.Value, "mm"))
'    'strCardNo = "MED/" & strPcat & "/" & strCatP & strcardCat & "/" & strDtCard
'    '
'    'txtSupp.Text = strCardNo
'
'
'
'    'cboClient.Text = getClientCatID(cboPat.Text)
'
'    '  Dim rsBL As New Recordset
'    '  With rsBL
'    '  .Open "select RetainID,RetainName from hRetainerShip", conSTR, adOpenForwardOnly, adLockReadOnly
'    '  If Not .EOF Then
'    ''Do While Not .EOF
'    'lblDisplay.Caption = !RetainName
'    ''.MoveNext
'    ''Loop
'    'End If
'    'End With
'    '
'    '
'    'Set rsBL = Nothing
'End If
Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
End Sub

Private Sub cboPat_GotFocus()
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

Private Sub cboPat_KeyPress(KeyAscii As Integer)
KeyAscii = Asc(UCase(Chr(KeyAscii)))

End Sub

Private Sub cboPat_KeyUp(KeyCode As Integer, Shift As Integer)
'        On Error GoTo errH 'Resume Next
'
'        CanLoad = False
'
'       Dim sCurrentText As String
'        Dim lItemIndex As Long
'
'        'Allow the backspace
'        If KeyCode = 8 Then Exit Sub  'backspace
'
'        'Get the current text
'        sCurrentText = cboPat.Text 'Trim(cbopat.Text)
'
'        'search for a pattern match
'        lItemIndex = SendMessage(cboPat.HWnd, CB_FINDSTRING, -1, ByVal sCurrentText)
'        If lItemIndex = -1 Then Exit Sub
'
'        'Set the index to the first matched item
'        cboPat.ListIndex = lItemIndex  'lstNdx
'
'        'Select the remaining text of the matched item
'        cboPat.SelStart = Len(sCurrentText)
'        cboPat.SelLength = Len(cboPat.Text) - Len(sCurrentText)
'
'        cboPat.Text = sCurrentText
'        cboPat.SelStart = Len(sCurrentText) + 1
'Exit Sub
'errH:
'MsgBox Err.Description
'
'
End Sub

Private Sub cboPurpose_Click()
On Error GoTo errH
Dim rsBLV As New Recordset
'If cboPurpose.Text = "" Or flgEdit = True Then Exit Sub
If cboPurpose.Text = "" Then Exit Sub
strPurpose = ""
ScreeningAmount = 0
ConsultAmount = 0
If cboPurpose.Text = "PRE-EMPLOYMENT" Or cboPurpose.Text = "EXEC-SCREENING" Or cboPurpose.Text = "SCREENING" Then
    strPurpose = "SCREENING"
    
    rsBLV.Open "select Amount   from hScreeningAmount where coycode='" & strCoyID & "'", conStr, adOpenForwardOnly, adLockReadOnly
    If Not rsBLV.EOF Then
        ScreeningAmount = IIf(IsNull(rsBLV!Amount), 0, rsBLV!Amount)
    Else
        ScreeningAmount = 0
    End If
    Set rsBLV = Nothing
ElseIf cboPurpose.Text = "(CONSULTATION)" Or strPurpose = "CONSULTATION" Then
    strPurpose = cboPurpose.Text
    
    'Dim rsBLV As New Recordset
    rsBLV.Open "select ConAmount as Amount   from hRetainership where retainID='" & strCoyID & "'", conStr, adOpenForwardOnly, adLockReadOnly
    If Not rsBLV.EOF Then
        ConsultAmount = IIf(IsNull(rsBLV!Amount), 0, rsBLV!Amount)
    Else
        ConsultAmount = 0
    End If
    Set rsBLV = Nothing

Else
    strPurpose = cboPurpose.Text
End If


If cboPurpose.Text = "APPOINTMENT" Then
    If True Then
        MsgBox "Please use the APPOINTMENT page", vbInformation
        cboPurpose.ListIndex = -1
        Exit Sub
    End If

    If cboClin.Text = "" Then
        MsgBox "Specify Clinic"
        Exit Sub
    End If
    
    
    Dim rsClin As New Recordset
    With rsClin
    .Open "select distinct Clinic  from vwClinicDays where clinic='" & strClinicID & "'", conStr, adOpenForwardOnly, adLockReadOnly
    If Not .EOF Then
        DoEvents
        Set frmID = Me
       frmClinicDates.Hide
        frmClinicDates.Show vbModal
        DoEvents
    
    End If
    End With



End If


Exit Sub
errH:
MsgBox Err.Description


End Sub

Private Sub cboRef_Click()
strFFS = ""
strFFS = cboRef.Text
End Sub

Private Sub cboRefHmo_Click()
On Error Resume Next
If cboRefHmo.ListIndex = 0 Or cboRefHmo.ListIndex = -1 Then Exit Sub
If cboRefHmo.Text = "" Then
    cboRefHmo.Text = "NO"
    Exit Sub
End If

If ClientType = "HMO" Then
    If cboRefHmo.Text = "YES" Then
        strFFS = "YES"
        cboRef.Text = strFFS
    End If
End If
            '    ElseIf cboRefHmo.Text = "NO" And strFFS2 = "YES" Then
            '        strFFS = "YES"
            '    ElseIf cboRefHmo.Text = "NO" And strFFS2 = "NO" Then
            '        strFFS = "NO"
            '    ElseIf cboRefHmo.Text = "NO" Then
            '        strFFS = "NO"
            '    End If
            '    CboRef.Text = strFFS
            'Else
            '        strFFS = "NO"

End Sub

Private Sub chkAll_Click()
On Error GoTo errH
Screen.MousePointer = vbHourglass
If chkAll.Value = vbChecked Then
    Dim rx As New Recordset
    rx.Open "select distinct clientCatID  from billingPrice", conStr, adOpenForwardOnly, adLockReadOnly
    If Not rx.EOF Then
        cboClient.Clear
        cboClient.AddItem ""
        rx.MoveFirst
        Do While Not rx.EOF
        cboClient.AddItem rx!clientCatID & ""
        rx.MoveNext
        Loop
    End If
End If
Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
End Sub

Private Sub chkReg_Click()
On Error GoTo errH


If chkReg.Value = vbChecked Then
    fraAttd.Enabled = True
ElseIf chkReg.Value = False Then
    fraAttd.Enabled = False
    'cboAppr.ListIndex = -1
    cboPurpose.ListIndex = -1
    cboClin.ListIndex = -1
End If

If strApp <> "RECORDS" Then
    chkReg.Value = False
    MsgBox "Only FrontDesk Module can take Attendance! Edit Allowed", vbCritical
    Exit Sub
End If


Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cmdAdd_Click()
On Error GoTo errH
If Admission_Limit_Exists = "YES" Then
    txtAdmitLimit.Text = ""
Else
    txtAdmitLimit.Text = "0"
End If

If isDependant = False Then
    strpCode = ""
End If

lblDebtCap.Caption = "Carry Over Debt"
lblDebt.Caption = "0"
Label5.Caption = "Patient Attendance / Registration"

isDependant = False 'nece here 'but can be overruled by 'Add Dependant' button

dblSaveBF = 0 'very impt 'to prevent it spilling to subsequent newly reg pat

isNewEntry = True

OKButton.Enabled = True

dblBF = 0
strPatNo = ""
SSTab1.Tab = 1

strClinicGen = "(GENERAL)" 'idle

dtRegDate.Value = Date
'dtRegDate.Value = ""

SetButtons (False)
enableFields True
'dtRegDate.Value = Null
'cboRef.Text = "NO"
cboRefHmo.Text = "NO"


'Call genIDNo 'for now 'now b4 insert in OKButton
txtSupp.Text = "" ' to be set by genIDNo during insert



'
'strPcat = strPcatNum
'strCardNo = HNo & "/" & strPcat
'
'txtSupp.Text = strCardNo

'dtRegDate.Value = Null
'cboRef.Text = "NO"

    If strApp = "RECORDS" Then
        chkReg.Value = vbChecked
    Else
        chkReg.Value = False
    End If
    

dtDate.Value = sysDate
dtDate.Value = ""


   
    If AUTO_CARD_NO = "YES" And Trim(txtOLD.Text) = "" Then
        strIDConVal = strCardNo
        Dim strCDNo As String
        strCDNo = strHospID & "/" & Right(CStr(Year(sysDate)), 2) & "/" & Right(strCardNo, 6)
        txtOLD.Text = strCDNo
    Else
        'do nothing
        'txtOLD.Text = ""
    End If

    cboFile.Text = "WHILE STILL IN SERVICE"
    'cboClient.Text = "PRIVATE"
    txtEmp.Text = "NIL"
    cboRef.Text = "YES"
    cboRefHmo.Text = "NO"

    txtPolicy.Clear 'very ok here
    txtPolicy.AddItem ""
    
    txtPolicy.AddItem "NIL"
    txtPolicy.Text = "NIL"

Exit Sub

errH:
MsgBox Err.Description

End Sub

Private Sub cmdANC_Click()
If txtSex.Text = "FEMALE" Then
    cmdANC.Enabled = True
Else
    cmdANC.Enabled = False
    MsgBox "Patient Sex has to be female"
    Exit Sub
End If


strPatNos = ""
strFullname = ""
strPatNos = txtSupp.Text
strFullname = txtSurNAme.Text & " " & txtfirstNAme.Text
If strPatNos = "" Then
    MsgBox "Patient System No Required! Please select a Patient"
    Exit Sub
End If

frmAnteNatalReg.Hide
frmAnteNatalReg.Show vbModal
End Sub

Private Sub cmdAttend_Click()
On Error GoTo errH

'If txtSupp.Text = "" Then
'    MsgBox "No Patient! Please select a Patient"
'    Exit Sub
'End If


Dim rsVal As New Recordset
Set grdAttend.DataSource = Nothing
With rsVal
    Dim sSQL As String
    .CursorLocation = adUseClient
    If Trim(txtSupp.Text) = "" Then '' display today's attnd
        sSQL = "select distinct recDate as Date,htime as AttendTime,FullName,ClinicType as Clinic,Remarks as Purpose,RetainName as Company,ConsultID from vwhRecords where RecDate='" & sysDate & "' order by FullName"
    Else '' '' display patients's attnd history
        sSQL = "select distinct recDate as Date,htime as AttendTime,FullName,ClinicType as Clinic,Remarks as Purpose,RetainName as Company,ConsultID from vwhRecords where pno='" & txtSupp.Text & "' order by RecDate desc"
    End If
    
    .Open sSQL, conStr, adOpenStatic, adLockOptimistic
    'MsgBox ssQL
    If Not .EOF Then
        Set grdAttend.DataSource = Nothing
        Set grdAttend.DataSource = rsVal
       
           If Trim(txtSupp.Text) = "" Then '' display today's attnd
                grdAttend.Caption = "Today's Attendance"
            Else
                strFullname = Trim(txtSurNAme.Text) & " " & Trim(txtfirstNAme.Text)
                grdAttend.Caption = strFullname & " Attendance History"
            End If
        'grdAttend.Columns("expired").Visible = False
        'grdAttend.Columns("coyname").Visible = False
    Else
        Set grdAttend.DataSource = Nothing
    End If
End With
Set rsVal = Nothing


Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cmdCancel_Click()

flgFill = False
SetButtons (True)
flgEdit = False
Call clearFields
enableFields False
End Sub



Private Sub cmdClinic_Click()
'On Error GoTo errH
'If Trim(txtSupp.Text) = "" Then
'    MsgBox "Patient No Required"
'    'txtSupp.SetFocus
'    Exit Sub
'End If
'frmPatientClinic.txtPno.Text = txtSupp.Text
'frmPatientClinic.Hide
'frmPatientClinic.Show
'Exit Sub
'errH:
'MsgBox Err.Description
End Sub

Private Sub cmdComm_Click()
strpNO = ""
strpNO = Trim(txtSupp.Text)

If strpNO = "" Then
    MsgBox "No Patient is specified"
    Exit Sub
End If

'strpNO = "" ' now up
'strpNO = Trim(txtSupp.Text)

If Show_New_Con_Hist_Page = "YES" Then
    frmConsultInfo.Show vbModal
Else
    frmConsultInfo.Show vbModal
    'frmConsultInfoOLD.Show vbModal
End If

'Me.Hide
End Sub

Private Sub cmdDel_Click()


 Dim rsBLV As New Recordset
      With rsBLV
        .Open "select username from vwusers where loginrole = 'MANAGEMENT' and username='" & m_Username & "'", conStr, adOpenForwardOnly, adLockReadOnly
        If .EOF Then
            MsgBox "You dont have Permission to Reverse an Entry"
            Exit Sub
        End If
      End With
      
      
  Dim cmd As New Command
  Dim strDel As String
  Dim sSQlx As String
  Dim intOK As Integer
  Dim strDel2 As String
  
On Error GoTo errH
 intOK = MsgBox("Are you sure to Delete Record", vbYesNo, "Delete")
 If intOK = vbYes Then
  strDel = grdData.Columns("pno")
    Dim rsNur As New Recordset
  Dim rsVal As New Recordset
Dim sSQL As String
'ssQL = "select pno as FileNo,oldpno as [Old FileNo],psurname as Surname,pfirstname as Firstname ,homeAddress from hpatients where psurname like '" & strNameVal & "%'"
    rsNur.Open "select TOP 1 * from hPreConsult where pno ='" & strDel & "'", conStr, adOpenStatic, adLockOptimistic
    If rsNur.EOF Then
   
    
        rsVal.Open "select TOP 1 * from hconsulting where pno ='" & strDel & "'", conStr, adOpenStatic, adLockOptimistic
        If rsVal.EOF Then
            cmd.ActiveConnection = conStr
            cmd.CommandType = adCmdText
            cmd.CommandText = "insert into hpatientsArchive select * from hpatients where pno = '" & strDel & "'"
            cmd.Execute
    
            sSQlx = "delete from hpatients where pno = '" & strDel & "'"
            cmd.CommandText = sSQlx
            cmd.Execute
            Call Auditrail(m_Username, "Delete Patient: " & txtSurNAme.Text & " " & txtfirstNAme.Text, txtSupp.Text, "", strHostName)
            MsgBox " Record successfully deleted "
            'Set grdData.DataSource = Nothing
            'grdData.clearFields
            
            searchVal = "Name"
            getPatInfo (0)
            
            'Call clearFields
        Else
            MsgBox "Patient Cannot be Deleted!!! Has Consultation Information"
            Exit Sub
        End If
    Else
        MsgBox "Patient Cannot be Deleted!!! Has Vital Signs Information"
        Exit Sub
    End If
   'Call fillGrid

End If
Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub cmdEdit_Click()
On Error Resume Next

If Trim(txtSupp.Text) = "" Then
    MsgBox "Specify Patient Info to Edit by Searching"
    txtName.SetFocus
    SSTab1.Tab = 0
    Exit Sub
End If

OKButton.Enabled = True

SSTab1.Tab = 1
'If grdData.Columns(0).Text = "" Then Exit Sub
flgEdit = True
SetButtons (False)
enableFields True

'cboClinic.Enabled = False 'nece

'frmPatientsNew.txtSupp.Locked = True
'ssQL = "select psurname as Surname,pfirstname as Firstname,pno as NewFileNo,oldpno as [Old FileNo],homeAddress from hpatients where psurname = '" & strV & "'"

strParam = grdData.Columns("pno")

If flgFill = False Then Call editVal

''''''''''''''''''''''''''''''''''
chkReg.Value = vbChecked

Exit Sub
'errH:
'MsgBox "Cannot select a record from an empty grid " & vbCrLf & Err.Description
End Sub

Private Sub cmdGrid_Click()
'On Error GoTo errH
''''''check against duplicate values
'Dim newRow As Integer
'Dim I As Integer
'
''strClinic = Mid(cboRef.Text, InStr(cboRef.Text, "[") + 1, Len(cboRef.Text) - (InStr(cboRef.Text, "[") + 1))
'
'
'For I = 1 To OrderGrid.Rows - 2
' If OrderGrid.TextMatrix(I, 0) = cboClinic.Text Then
'MsgBox "Duplicate Clinic Entry not allowed"
'Exit Sub
'End If
'Next I
''''''''''''''''''''
''
'
'If cboClinic.Text = " " Or cboClinic.Text = "" Then
'    MsgBox "Please specify a Clinic"
'    cboClinic.SetFocus
'    Exit Sub
'End If
'
'
'If cboFile.Text = "" Then
'    MsgBox "Please specify Duration of this Registration"
'    cboFile.SetFocus
'    Exit Sub
'End If
'
''If IsNull(dtExpDate.Value) Or dtExpDate.Value = "" Then
''    MsgBox "Please specify an Exp Date"
''    dtExpDate.SetFocus
''    Exit Sub
''End If
'
'
'    If cboClient.Text = "PRIVATE" Or cboClient.Text = "FEE-PAYING" Or cboClient.Text = "(FEE-PAYING)" Then
'         dtExpDate.Value = DateAdd("YYYY", 1, dtRegDate.Value)
'    Else
'        dtExpDate.Value = Null
'    End If
'
'
'
'''''''''''''''''''''''''''''''
'newRow = OrderGrid.Rows - 1
'OrderGrid.TextMatrix(newRow, 0) = cboClinic.Text     'cboRef.Text
'OrderGrid.TextMatrix(newRow, 1) = cboFile.Text
'OrderGrid.TextMatrix(newRow, 2) = dtExpDate.Value & ""
''OrderGrid.TextMatrix(newRow, 3) = txtApptRem.Text
'
'OrderGrid.Rows = OrderGrid.Rows + 1
'
'cboClinic.ListIndex = -1
'cboFile.ListIndex = -1
'dtExpDate.Value = ""
'
'Exit Sub
'errH:
'MsgBox Err.Description
End Sub

Private Sub cmdPlan_Click()
On Error GoTo errH

' If strApp = "TARIFF" Or strApp = "MANAGEMENT" Then
'    frmPlan.Hide
'    frmPlan.Show
'Else
'    frmPlan.Hide
'    frmPlan.Show vbModal
'End If

    
    frmPlan.cboHMO.AddItem cboPat.Text      'txtPolicy.Text = gPlanName & " [" & gPlanID & "]"
    frmPlan.cboHMO.Text = cboPat.Text     'txtPolicy.Text = gPlanName & " [" & gPlanID & "]"
    'frmPlan.Hide
    frmPlan.Show vbModal 'records mod has its own frmPlan that is modal
    

'Unload frmPlan

txtPolicy.Clear
txtPolicy.AddItem ""

Call loadPlan

txtPolicy.Text = gPlanName & " [" & gPlanID & "]"

Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cmdOK_Click()
If txtName = "" Then
MsgBox "To locate a patient, Enter few Letters and click OK Button"
'Set grdData.DataSource = Nothing
Exit Sub
End If

'flgSch = True

searchVal = "Name"
getPatInfo (0)

End Sub

Private Sub cmdCard_Click()
If txtName = "" Then
MsgBox "To locate a patient, Enter Card No and click 'Card No' Button"
'Set grdData.DataSource = Nothing
Exit Sub
End If

'flgSch = True
searchVal = "CardNo"
getPatInfo (0)

End Sub


Private Sub cmdRenew_Click()
'lblNo.Caption = rsBLV!pno & ""
'lblOld.Caption = rsBLV!oldpno & ""
strFullname = txtSurNAme.Text & " " & txtfirstNAme.Text
strPatNos = txtSupp.Text
'cmdRenew.Enabled = False
strForm = "REG"
flgCardRenew = True
frmRenew.Hide
frmRenew.Show vbModal
End Sub


Public Sub cmdSys_Click()
If Trim(txtName.Text) = "" Then
MsgBox "To locate a patient, Enter Patient Tel No and click 'Tel No' Button"
'Set grdData.DataSource = Nothing
Exit Sub
End If

'flgSch = True
searchVal = "SysNo"
getPatInfo (0)

End Sub

Private Sub getPatInfo(numOfChar As Integer)

On Error GoTo errH
Dim strNameVal As String
strNameVal = Trim(txtName.Text)

If strNameVal = "" Then
    Set grdData.DataSource = Nothing
    lblFound.Caption = "0 Records Found"
    Exit Sub
End If
    
Dim sSQL As String


Select Case searchVal
Case "Name"
    If numOfChar = 0 Then 'unlimited 'click from button
        sSQL = "select  * from vwhpatients where fullname like '%" & strNameVal & "%' or PhoneNo like '%" & strNameVal & "%'  order by fullname"
    Else
        sSQL = "select top " & numOfChar & "  * from vwhpatients where fullname like '%" & strNameVal & "%'  or PhoneNo like '%" & strNameVal & "%' order by fullname"
    End If
Case "CardNo"
    sSQL = "select  * from vwhpatients where oldPno = '" & Replace(strNameVal, "'", "''") & "'  order by fullname"

Case "SysNo"
    'If InStr(strNameVal, "/") > 0 Then
    '    strNameVal = strNameVal
    'Else
    '    strNameVal = strNameVal ' strip off zeros ' to allow only sig figures ' flexibility
    '    strNameVal = HNo & "/" & Right("000000000" & strNameVal, 9)
    '    txtName.Text = strNameVal
    'End If
    sSQL = "select  * from vwhpatients where PhoneNo like '%" & strNameVal & "%' order by fullname"
End Select

        Set grdData.DataSource = Nothing

        Dim rsVal As New Recordset
        Dim cnn As New ADODB.Connection
        cnn.Open conStr

        With rsVal
            .CursorLocation = adUseClient
            Screen.MousePointer = vbHourglass
                .Open sSQL, cnn, adOpenStatic, adLockOptimistic
            Screen.MousePointer = vbDefault
            
            If Not .EOF Then
                Set grdData.DataSource = rsVal
                grdData.Columns("Surname").Width = 2880
                grdData.Columns("expired").Visible = False
                grdData.Columns("coyname").Visible = False
                grdData.Columns("fullname").Visible = False
                lblFound.Caption = FormatNumber(.RecordCount, 0) & " Records Found"
                
                'If searchVal = "Name" Then
                '    If strNameVal <> "" Then
                '        'locate updated row
                '        'On Error GoTo Er
                '        .Find "Surname = " & strNameVal
                '      End If
                'End If
                
            Else
                Set grdData.DataSource = Nothing
                lblFound.Caption = "0 Records Found"
            End If

        End With

        ''Release reference to connection
        Set rsVal.ActiveConnection = Nothing
         Set rsVal = Nothing
   
    'Call cmdAdd_Click 'remarked to allow for search without update of attendance

'Set rsVal = Nothing
Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

''''''''''''''''''''''''''''''''''''''''

End Sub

Private Sub cmdPatDep_Click()
On Error GoTo errH
If PatCat = "DEPENDANT" Then
    MsgBox "Select a Principal Patient! Cannot Add Dependants to existing Dependant!!"
    SSTab1.Tab = 0
    Call clearFields
    Exit Sub
End If

If strPrinNo = "" Then
    If txtSupp.Text = "" Then
        MsgBox "Select Principal Patient to add Dependants to"
        Exit Sub
    End If
End If

''strFileOLD = txtSupp.Text
''strFile = txtSupp.Text
''strPrincFile = txtSupp.Text
''If Mid(strFile, 14, 1) = "/" Then
''MsgBox "There is no Principal Character Letter in this patient No" & vbCrLf & "Click OK to add a Character And Save"
''txtSupp.Text = Left(strFile, 13) & "A" & Mid(strFile, 14)
''
''flgEdit = True
''strParam = strFileOLD
''Exit Sub
''End If
'
'Call addDependants
'
'frmPatientsDep.Show vbModal


principalNo = strPrinNo 'txtSupp.Text 'strPatNo ''nece before clearing txtsupp.text by cmdAdd_Click call


txtfirstNAme.Text = ""
dtDate.Value = ""
txtAge.Text = ""
txtPhone.Text = ""
txtEmail.Text = ""
txtOccu.Text = ""
txtOfficeAddress.Text = ""

txtSex.ListIndex = -1
cboMat.ListIndex = -1
txtKinRel.ListIndex = 0
cboTitle.ListIndex = 0
cboNew.ListIndex = -1

Call cmdAdd_Click 'new entry

isDependant = True   'nece here after cmdAdd_Click call
flgEdit = False 'new rec


Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cmdPix_Click()
On Error GoTo errH
txtPix.Text = ""
If Trim(txtSupp.Text) = "" Then
    txtSupp.Text = "0000" 'cos pno is auto gen now
    'MsgBox "Invalid Sys No!"
    'Exit Sub
End If
CommonDialog1.DialogTitle = "Select the JPG Image  File..."

CommonDialog1.flags = cdlOFNFileMustExist
CommonDialog1.Filter = "Image Files(*.JPG)|*.JPG;"
CommonDialog1.ShowOpen

If Len(CommonDialog1.FileName) <> 0 Then

    txtPix.Text = CommonDialog1.FileName

    If Trim(txtPix.Text) = "" Then Exit Sub
    Dim strPath As String, strPixNo As String, strPatPix As String
    strPath = txtPix.Text
    strPixNo = Replace(Trim(txtPix.Text), "/", "")
    'strPixPath = App.Path & "\Patients\"
    If InStr(1, strPixNo, ".JPG", vbTextCompare) = 0 Then
        strPixNo = strPixNo & ".JPG"
    Else
        piX.Picture = LoadPicture(strPixNo)
    End If
Else
    piX.Picture = Nothing
End If


Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub cmdRefresh_Click()
Call loadBoxes
'Call clearFields
Call fillGrid
Call getDocWaitList
End Sub

Private Sub DeletePrevAssign()
'  Dim cmd As New Command
'  Dim strDel As String
'  Dim sSQlx As String
'  Dim intOK As Integer
'On Error GoTo errH
' sSQlx = "Delete hConRoomAssign where SchdDate < '" & sysDate & "'"
'    cmd.ActiveConnection = conSTR
'    cmd.CommandText = sSQlx
'    cmd.CommandType = adCmdText
'    cmd.Execute
'   Call fillGrid


Exit Sub
errH:
MsgBox Err.Description

End Sub


Private Sub cmdTake_Click()
'frmCamera.Hide
'frmCamera.Show vbModal
End Sub

Private Sub cmdWaitList_Click()
Call getDocWaitList
End Sub

Private Sub dtDate_Change()
'On Error GoTo errH
'If IsDate(dtDate.Value) Then
'ageVal = dtDate.Value
'txtAge.Text = CStr(DateDiff("yyyy", ageVal, Date))
'Else
'txtAge.Text = ""
'End If
'Exit Sub
'errH:
'MsgBox Err.Description
End Sub

Private Sub dtDate_LostFocus()
On Error GoTo errH
Dim dtVal As Date
'If IsDate(dtDate.Value) Then
'ageVal = dtDate.Value
'dtVal = dtDate.Value
'txtAge.Text = CStr(DateDiff("yyyy", ageVal, Date))
'dtDate.Value = dtVal 'nece for dob to remain intact
'Else
'    txtAge.Text = ""
'End If


If IsDate(dtDate.Value) Then
    Dim PatAge As String
    PatAge = CalcAge(dtDate.Value)
    txtAge.Text = PatAge

Else
    txtAge.Text = ""
End If



Exit Sub
errH:
MsgBox Err.Description

End Sub



Private Sub Form_Initialize()
    'skfFrmPatientsNew.LoadSkin App.Path + "\Styles\Vista.cjstyles", ""
    'skfFrmPatientsNew.ApplyWindow Me.hWnd

End Sub

Private Sub Form_Load()
On Error GoTo errH

'skfFrmPatientsNew.LoadSkin App.Path + "\Styles\Vista.cjstyles", ""
'skfFrmPatientsNew.ApplyWindow Me.hWnd

flgCardRenew = False

If strApp = "NURSING" Then
    cmdANC.Enabled = True
Else
    cmdANC.Enabled = False
End If

fraAttd.Enabled = False

flgFill = False

SSTab1.Tab = 0

strCardNo = ""
strPcat = ""
strDtCard = ""
strcardCat = ""

dtDate.Value = Date
dtDate.Value = ""

dtExpDate.Value = sysDate
dtRegDate.Value = sysDate


cboTitle.Clear
cboTitle.AddItem ""
cboTitle.AddItem "MR"
cboTitle.AddItem "MRS"
cboTitle.AddItem "MISS"
cboTitle.AddItem "MASTER"
cboTitle.AddItem "DR"
cboTitle.AddItem "PROF"
cboTitle.AddItem "ALHAJI"
cboTitle.AddItem "ALHAJA"
cboTitle.AddItem "PASTOR"
cboTitle.AddItem "CHIEF"


txtKinRel.Clear
txtKinRel.AddItem ""
txtKinRel.AddItem "FATHER"
txtKinRel.AddItem "MOTHER"
txtKinRel.AddItem "UNCLE"
txtKinRel.AddItem "AUNT"
txtKinRel.AddItem "BROTHER"
txtKinRel.AddItem "SISTER"
txtKinRel.AddItem "COUSIN"
txtKinRel.AddItem "NIECE"
txtKinRel.AddItem "WIFE"
txtKinRel.AddItem "HUSBAND"
txtKinRel.AddItem "SON"
txtKinRel.AddItem "DAUGHTER"
txtKinRel.AddItem "OTHERS"






'cboClient.Clear
'cboClient.AddItem ""
'cboClient.AddItem "NHIS"
'cboClient.AddItem "PHIS"
'cboClient.AddItem "FEE-PAYING"
''cboClient.AddItem "PRIVATE"
'cboClient.AddItem "MTHLY"
'cboClient.AddItem "3MTHLY"
'cboClient.AddItem "6MTHLY"
'cboClient.AddItem "CBN"
'cboClient.AddItem "NEPA"

txtBG.Clear
txtBG.AddItem ""
txtBG.AddItem "O-NEGATIVE"
txtBG.AddItem "O-POSITIVE"
txtBG.AddItem "A-NEGATIVE"
txtBG.AddItem "A-POSITIVE"
txtBG.AddItem "B-NEGATIVE"
txtBG.AddItem "B-POSITIVE"
txtBG.AddItem "AB-NEGATIVE"
txtBG.AddItem "AB-POSITIVE"

txtGeno.Clear
txtGeno.AddItem ""
txtGeno.AddItem "AA"
txtGeno.AddItem "AS"
txtGeno.AddItem "SS"
txtGeno.AddItem "AC"

cboStatus.Clear
cboStatus.AddItem ""
cboStatus.AddItem "CHRISTIANITY"
cboStatus.AddItem "ISLAM"
cboStatus.AddItem "OTHERS"


cboFile.Clear
cboFile.AddItem ""
cboFile.AddItem "WHILE STILL IN SERVICE"
cboFile.AddItem "ONE YEAR"
cboFile.AddItem "TILL END OF YEAR"





cboCard.Clear
cboCard.AddItem ""
cboCard.AddItem "SINGLE"
cboCard.AddItem "FAMILY"

cboMStatus.Clear
cboMStatus.AddItem ""
cboMStatus.AddItem "SINGLE"
cboMStatus.AddItem "MARRIED"

'cboCard.AddItem "ANTE-NATAL CLINIC"
'cboCard.AddItem "CHILD"

cboRef.Clear
cboRef.AddItem ""
cboRef.AddItem "YES"
cboRef.AddItem "NO"

cboRefHmo.Clear
cboRefHmo.AddItem ""
cboRefHmo.AddItem "YES"
cboRefHmo.AddItem "NO"

cboNew.Clear
cboNew.AddItem ""
cboNew.AddItem "NEW"
cboNew.AddItem "EXISTING"

'cboCard.AddItem "EMERGENCY"
'cboCard.AddItem "ANTENATAL"

txtSex.Clear
txtSex.AddItem ""
txtSex.AddItem "MALE"
txtSex.AddItem "FEMALE"

 
 Call loadBoxes
 
'OrderGrid.FormatString = "--------------------Clinic-------------------|----------------Duration-------------------|----------------Exp Date-------------------"

'Dim rsBLV As New Recordset
'cboAppr.Clear
'cboAppr.AddItem ""
'  With rsBLV
'        .Open "select distinct empID,lastname,firstname from employees where deptID in('MED/REC','NUR')", conSTR, adOpenForwardOnly, adLockReadOnly
'    If Not .EOF Then
'    .MoveFirst
'    Do While Not .EOF
'    cboAppr.AddItem !LastName & " " & !FirstName & " [" & !EmpID & "]"
'    .MoveNext
'    Loop
'    End If
'End With
'Set rsBLV = Nothing



enableFields False
Call fillGrid

getDocWaitList

Exit Sub
errH:
MsgBox Err.Description
End Sub


Private Sub Form_Resize()
Me.Left = (Screen.Width - Me.Width) \ 2
Me.Top = 0

End Sub

Private Sub Form_Unload(Cancel As Integer)
    flgEdit = False
flgHMO = False
End Sub

Public Sub grdData_dblClick()
'If grdData.DataSource = Nothing Then Exit Sub
'SSTab1.Tab = 1
flgFill = True
Call editVal
cmdPatDep.Enabled = True
Call cmdEdit_Click


Call PastPatInfo(strPatNo)


End Sub

Private Sub grdDoc_DblClick()
If grdDoc.Columns("NumOfPat").Text = 0 Then
    MsgBox "This Doctor has No Patients on Queue"
    Exit Sub
End If

gDate = vbEmpty
gDoc = ""
gClinicID = ""
gDate = CDate(grdDoc.Columns("Date").Text)
gDoc = grdDoc.Columns("EmpID").Text
gClinicID = grdDoc.Columns("ClinicID").Text

frmDocWait.Hide
frmDocWait.Show vbModal


End Sub

Private Sub lblDebt_DblClick()
On Error GoTo errH
   
If flgEdit = True Then
    MsgBox "Debt Information Cannot be Edited Here! Only Carry Over Debt can be Entered"
    Exit Sub
End If
   
Dim dblDebt As Double

dblDebt = 0
dblDebt = InputBox("Carry Over Debt", "Debt", "0")

'for new pat 'where flgEdit=false
dblSaveBF = dblDebt 'ok here and also after call to saveDebt in editVal

lblDebt.Caption = FormatNumber(dblSaveBF, 2)
    
    '
    'Dim Cmd As Command
    'Set Cmd = New Command
    'Dim connTran As New Connection
    'connTran.ConnectionString = conSTR
    'connTran.Open
    '
    'Cmd.ActiveConnection = connTran
    'Cmd.CommandType = adCmdText
    
    'Cmd.CommandText = "update hPatients set isRev = 1, DebtBF=" & dblDebt & "where pno = '" & strPatient & "'"
    'Cmd.Execute 'Direct update nece

    'Call Auditrail(m_Username, "Adjust Debt for " & strName, strCon, debtRemarks, strHostName)
        
      
Exit Sub
errH:
MsgBox Err.Description


End Sub


Private Sub OKButton_Click()

On Error GoTo errX


Dim rsExP As Recordset

If cboNew.Text = "" Then
    MsgBox "Specify if Registration is new OR Existing"
    cboNew.SetFocus
    Exit Sub
End If

If cboTitle.Text = "" Then
    MsgBox "Specify Patient Title/Salutation"
    cboTitle.SetFocus
    Exit Sub
End If

If Trim(cboPat.Text) = "" Then
    MsgBox " Company Name cannot be empty"
    cboPat.SetFocus
    Exit Sub
End If

If strpCode = "" Then
    MsgBox " Select Company Name", vbCritical, "Empty Company Code not Allowed"
    cboPat.SetFocus
    Exit Sub
End If

If Len(Trim(strpCode)) > 7 Then '' coyID
    MsgBox " Re-Select Company Name", vbCritical, "Invalid Company Code"
    cboPat.SetFocus
    Exit Sub
End If





If cboClient.Text = "NHIS" Or cboClient.Text = "HMO" Or cboClient.Text = "PHIS" Then
    'If Trim(txtPolicy.Text) = "" Then
    '    MsgBox "HMO Enrolle Plan Needed"
    '    txtPolicy.SetFocus
    '    Exit Sub
    'End If
    
    If Trim(txtEmp.Text) = "" Then
        MsgBox "HMO Enrolle No Needed"
        txtEmp.SetFocus
        Exit Sub
    End If

    If planID = "" Then
        'planID = Trim(txtPolicy.Text)
        'MsgBox "Select HMO Enrolle Plan"
        'txtPolicy.SetFocus
        'Exit Sub
    End If
    
    
    If txtAdmitLimit.Text = "" And Admission_Limit_Exists = "YES" Then
        MsgBox "Specify Maximum No of hrs to Stay in Admission, if Admitted! (AdmissionLimit) Enter zero if Unlimited"
        txtAdmitLimit.SetFocus
        Exit Sub
    Else
        'txtAdmitLimit.Text = 0
    End If
Else
    If txtAdmitLimit.Text = "" Then
        txtAdmitLimit.Text = 0
    End If
    
End If



If cboCard.Text = "" Then
    MsgBox " Card Category cannot be empty"
    cboCard.SetFocus
    Exit Sub
End If

If Enforce_Yearly_Re_Registration_Private = "YES" Then
    If IsNull(dtExpDate.Value) And strpCode = strPrivate Then
        MsgBox " Please Specify Expiration Date for this Private Patient"
        dtExpDate.SetFocus
        Exit Sub
    End If
End If

If IsNull(dtRegDate.Value) Or dtRegDate.Value = "" Then
    MsgBox " Please Specify Date Patient Registered"
    dtRegDate.SetFocus
    Exit Sub
End If


If flgEdit = True Then 'edit rec
    If Trim(txtSupp.Text) = "" Then
        MsgBox " Patient Card No cannot be empty"
        txtSupp.SetFocus
        Exit Sub
    End If
Else ' new rec

    txtSupp.Text = "" ' to be set by genIDNo during insert
End If


If cboMat.Text = "" Or cboMat.Text = " " Then
    MsgBox " Patient Maturity  Cannot be Blank"
    SSTab1.Tab = 1
    cboMat.SetFocus
    Exit Sub
End If

If flgEdit = False Then ' ie new record insert
    If cboNew.Text = "" Then
        MsgBox " Specify Registration type 'NEW/OLD'"
        cboNew.SetFocus
        Exit Sub
    End If
End If


'If cboClinic.Text = "" Then
'        MsgBox " Specify Patient Initial Clinic "
'        cboClinic.SetFocus
'        Exit Sub
'    End If
'End If


If cboRef.Text = "" Then
    MsgBox "Fee for Service Field  Cannot be Blank"
    'SSTab1.Tab = 1
    cboRef.SetFocus
    'cboRef.Text = "NO"
    Exit Sub
End If

If cboRefHmo.Text = "" Or cboRefHmo.Text = " " Then
    MsgBox "HMO Referal Field  Cannot be Blank"
    'SSTab1.Tab = 1
    cboRefHmo.SetFocus
    'cboRef.Text = "NO"
    Exit Sub
End If

            If Trim(cboArea.Text) = "" Then
                MsgBox "Specify Patient Residential Area"
                cboArea.SetFocus
                Exit Sub
            End If



'If cboType.Text = "" Then
'    Frame1.Enabled = True
'    MsgBox " Company Type cannot be empty !!! Select either HMO or NON-HMO"
'    'cboType.SetFocus
'    Exit Sub
'End If

'If cboAppr.Text = "" Then
'MsgBox " please specify Name of record Officer"
'cboAppr.SetFocus
'Exit Sub
'End If


If Trim(txtSurNAme.Text) = "" Then
    MsgBox " Surname cannot be empty"
    txtSurNAme.SetFocus
    Exit Sub
End If

If Trim(txtfirstNAme.Text) = "" Then
    MsgBox " Firstname cannot be empty"
    txtfirstNAme.SetFocus
    Exit Sub
End If



'If cboCat.Text = "" Then
'    MsgBox "Company Cat cannot be empty"
'    cboCat.SetFocus
'    Exit Sub
'End If
'
'If cboCat2.Text = "" Then
'    MsgBox "Company Class cannot be empty"
'    cboCat2.SetFocus
'    Exit Sub
'End If
'If cboCat3.Text = "" Then
'    MsgBox "Company Level cannot be empty"
'    cboCat3.SetFocus
'    Exit Sub
'End If

If cboPat.Text = "" Then
    MsgBox " Company Name cannot be empty"
    cboPat.SetFocus
    Exit Sub
End If

If cboClient.Text = "" Then
    MsgBox "Billing Cat (Client Tariff) cannot be empty"
    cboClient.SetFocus
    Exit Sub
End If

If chkReg.Value = vbChecked And cboClin.Text = "" Then
    MsgBox "Specify Clinic for Attendance"
    'SSTab1.Tab = 1
    cboClin.SetFocus
    'cboRef.Text = "NO"
    Exit Sub
End If

If chkReg.Value = vbChecked And cboPurpose.Text = "" Then
    MsgBox "Specify Purpose of Attendance"
    'SSTab1.Tab = 1
    cboPurpose.SetFocus
    'cboRef.Text = "NO"
    Exit Sub
End If

If chkReg.Value = vbChecked And strPurpose = "" Then
    MsgBox "Specify Purpose of Attendance"
    'SSTab1.Tab = 1
    cboPurpose.SetFocus
    'cboRef.Text = "NO"
    Exit Sub
End If

'If Enforce_Assign_To_Doctor_In_Attendance = "YES" Then
    If cboPurpose.Text = "(CONSULTATION)" And chkReg.Value = vbChecked And cboDoc.Text = "" Then
        MsgBox "Assign a Doctor to this Patient", vbCritical
        'SSTab1.Tab = 1
        cboDoc.SetFocus
        Exit Sub
    Else
        'cboDoc.ListIndex = -1
    End If
'End If


If Trim(txtPhone.Text) = "" Then
    MsgBox "Patient's Phone No Required! 11 Digits Onlt!! GSM Only. eg 08031234567"
    SSTab1.Tab = 1
    txtPhone.SetFocus
    Exit Sub
End If


'If Len(txtPhone.Text) <> 11 Then
'    MsgBox "Patient's Phone No must be 11 digits!! GSM Only. eg 08031234567"
'    SSTab1.Tab = 1
'    txtPhone.SetFocus
'    Exit Sub
'End If




'If chkReg.Value = vbChecked And cboAppr.Text = "" Then
'    MsgBox "Specify Records Officer"
'    SSTab1.Tab = 1
'    cboAppr.SetFocus
'    'cboRef.Text = "NO"
'    Exit Sub
'End If


If Trim(txtKin.Text) = "" Then
    MsgBox "Next of Kin Required"
    SSTab1.Tab = 1
    txtKin.SetFocus
    Exit Sub
End If


If Trim(txtNOKPhone.Text) = "" Then
    MsgBox "Next of Kin Phone No Required"
    SSTab1.Tab = 1
    txtNOKPhone.SetFocus
    Exit Sub
End If

If IsNull(dtDate.Value) Or dtDate.Value > sysDate Then
    MsgBox "Specify Date of Birth (DOB)  Or DOB must not be greater than today"
    dtDate.SetFocus
    Exit Sub
End If

'If cboType.Text = "" And cboCat.Text = "COMPANY" Then
'    MsgBox " Company Type cannot be empty !!! Select either HMO or NON-HMO"
'    cboType.SetFocus
'    Exit Sub
'End If

'If cboCard.Text = "FAMILY" And txtMem.Text = "" Then
    'MsgBox " Please enter the names of your family members. You requested for Family Card"
    'Exit Sub
'End If

'If cboType.Text = "HMO" And cboCard.Text = "FAMILY" Then
'Dim strFN As String
'Dim strCom As String

Dim flgVerify As Boolean
Dim strVerifyPno As String
strVerifyPno = txtSupp.Text



Dim intSave As Integer
intSave = MsgBox("Are you sure to save", vbYesNo, "Check before save")
If intSave = vbYes Then
    Screen.MousePointer = vbHourglass
    
    OKButton.Enabled = False 'ok here
    
    '    Select Case cboClient.Text
    '        Case "FEE-PAYING"
    '            strTariff = "PRIVATE"
    '        Case "CREDIT-PRIVATE"
    '            strTariff = "PRIVATE"
    '        Case "PHIS"
    '            strTariff = "HMO"
    '        Case Else
    '             strTariff = cboClient.Text
    '    End Select
    
        'strTariff = cboClient.Text
    
    
        On Error GoTo errH
            Dim dtSys As Date
            dtSys = getSysDateTime
    
            Set rsExP = New ADODB.Recordset
            With rsExP
                .CursorLocation = adUseClient
                .ActiveConnection = conStr
    
            Dim cmd As New ADODB.Command
            Dim rsC As New ADODB.Recordset
            Dim connTran As New Connection
            connTran.ConnectionString = conStr
            connTran.Open
            connTran.BeginTrans
            
            isInsert = 0
    
    
                    If flgEdit Then
                    
    
                        .Open "select * from hpatients where pNO='" & strParam & "'", connTran, adOpenStatic, adLockOptimistic
                        '!pCatID = pCatID ' for now 'cos of dependant
                        '!username = m_Username
                        !Title = cboTitle.Text
                        !PNo = Trim(txtSupp.Text)
                        !oldPno = Trim(txtOLD.Text)
                        !fileduration = cboFile.Text
                        !regDate = dtRegDate.Value
                        If Not IsNull(dtExpDate.Value) Then
                            !expirydate = dtExpDate.Value
                        End If
                        !psurname = Trim(Replace(Replace(txtSurNAme.Text, "[", ""), "]", ""))
                        !pfirstName = Trim(Replace(Replace(txtfirstNAme.Text, "[", ""), "]", ""))
                        !homeAddress = txtHomeAddress.Text
                        !Area = cboArea.Text
                        !officeAddress = txtOfficeAddress.Text
                        !pPhoneNo = Trim(txtPhone.Text)
                        If Not IsNull(dtDate.Value) Then
                            !DOB = dtDate.Value
                        End If
                        !Maturity = cboMat.Text
                        !ref = cboRef.Text 'ffs
                        !HmoRef = cboRefHmo.Text 'referal
                        !Email = txtEmail.Text
                        !sex = txtSex.Text
                        !clientCatID = strTariff   'cboClient.Text
                        !coyType = coyType   'cboClient.Text      'cboType.Text
                        !CoyName = strpCode
                        '!coyClass = cboCat2   'cboClient.Text     'cboType.Text
                        '!coytype = cboCat3   'cboClient.Text     'cboType.Text
                        !empno = Trim(txtEmp.Text)
                        '!branch = txtBr.Text
                        '!Status = txtStatus.Text
                        '!relationToStaff = txtRel.Text
                        !introducedby = strIntro
                        !policyType = txtPolicy.Text
                        !CardType = cboCard.Text
                        !MStatus = cboMStatus.Text
                        '!pMembers = txtMem.Text
                        !bloodGroup = txtBG.Text
                        !genotype = txtGeno.Text
                        !occupation = txtOccu.Text
                        !religion = cboStatus.Text
                        !nextOfKin = txtKin.Text
                        !NOKPhone = txtNOKPhone.Text
                        !relationToKin = txtKinRel.Text
                        !kinAddress = txtKinAddress.Text
                        !AdmissionDaysLimit = txtAdmitLimit.Text
                        
                        'If cboNew.Text = "NEW" Then
                        '    !NewReg = "YES"
                        'Else
                        '    !NewReg = "NO"
                        'End If
                        
    
                        .Update
                    
    
                'Call Auditrail(m_Username, "Edit Reg: " & txtsurNAme.Text & " " & txtfirstNAme.Text, txtSupp.Text, "", strHostName)
    
                'With cmd   'not needed
                '.ActiveConnection = connTran
                '.CommandType = adCmdText
                '.CommandText = "delete hPatientClinics where pno='" & txtSupp.Text & "'"
                '.Execute
                'End With
    
                '     rsC.Open "select * from hPatientClinics where 1=2", connTran, adOpenStatic, adLockOptimistic
                '        rsC.AddNew
                '        rsC!pno = Trim(txtSupp.Text)
                '        rsC!regDate = dtRegDate.Value
                '        rsC!Clinic = cboClinic.Text
                '        rsC!Remarks = ""
                '
                '        If Not IsNull(dtExpDate.Value) Then
                '            rsC!expireDate = dtExpDate.Value
                '        End If
                '
                '        rsC!Active = 1
                
                '        rsC.Update
    
    
    
                              If chkReg.Value = vbChecked Then
                                Call genConID(connTran) 'ok inside chkReg
                                If getID_No = "" Then
                                    connTran.RollbackTrans
                                    OKButton.Enabled = True
                                    Screen.MousePointer = vbDefault
                                    MsgBox "Unable to generate No!!! Function getIDNo Failed! ConsultID"
                                    Exit Sub
                                End If
                                Call getAttendance(connTran) 'attnd fee also embedded but zero
                                'If strCoyID = strPrivate Then
                                    Call getServiceFeeConsulting(connTran) ' needed for consultation fee
                                'End If
                            End If
    
                        
                        If flgCardRenew = True Then
                            
                            Call getCardRenewFee(connTran)
                        
                        End If
                        
                        
                        connTran.CommitTrans
                        flgEdit = False
                    
                    If OldClientCode = strpCode Then
                        Call Auditrail(m_Username, "Edit Reg for " & txtSurNAme.Text & " " & txtfirstNAme.Text, txtSupp.Text, "", strHostName)
                     Else
                        Call Auditrail(m_Username, "Edit Reg: " & txtSurNAme.Text & " " & txtfirstNAme.Text, txtSupp.Text, "Changed Company from " & OldClientCode & ":" & OldClient & " to " & strpCode & ":" & Client, strHostName)
                    End If
                        
                        
                    'send sms routine called by both insert and edit for this page ONLY
                    If chkReg.Value = vbChecked And cboPurpose.Text <> "APPOINTMENT" Then
                        ''''''''''''''Send SMS'''''''ATTEND''''''''''''''''''''''''''''''
                        'Call sendToSmsCenter(Trim(txtSupp.Text), strCval, "ATTEND", "ATTENDANCE", sysDate, sysTime)
                        Call sendToSmsCenter(Trim(txtSupp.Text), strCval, "ATTEND", "ATTENDANCE", sysDate, sysTime, Trim(txtPhone.Text), Trim(txtEmail.Text))
    
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''
                    End If
                    
                    
                    If chkReg.Value = vbChecked And cboPurpose.Text = "APPOINTMENT" Then            'nece here for Appt
                        ''''''''''''''Send SMS'''''''''''''''''''''''''''''''''''''
                         dtNext.CheckBox = True
                         dtRefTime.CheckBox = True
                        Call sendToSmsCenter(Trim(txtSupp.Text), strCval, "APPT", cboClin.Text, dtNext.Value, dtRefTime.Value, Trim(txtPhone.Text), Trim(txtEmail.Text))
                
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''
                    End If
                        
                    MsgBox "Record Succesfully Edited and Updated"
    
    
    Else
    
    
    
                        'carry over debt
                        If Not IsNumeric(lblDebt.Caption) Then lblDebt.Caption = 0
     
                        Call genIDNo 'nece  for now  'getCorrectConID(connTran)       ' this increments gen num in case another user has inserted
                        If getID_No = "" Then
                            connTran.RollbackTrans
                            OKButton.Enabled = True
                            Screen.MousePointer = vbDefault
                            MsgBox "Unable to generate No!!! Function getIDNo Failed"
                            Exit Sub
                        End If
                        

                        .Open "select * from hpatients where 1=2", connTran, adOpenStatic, adLockOptimistic
                        .AddNew
                        
                        
                        !Username = strEmpID
                        !Title = cboTitle.Text
                        !PNo = Trim(strCardNo)
                        
                        If AUTO_CARD_NO = "YES" Then
                            !oldPno = strHospID & "/" & Right(CStr(Year(sysDate)), 2) & "/" & Right(strCardNo, 6)  'Trim(txtOLD.Text)
                        Else
                            !oldPno = Trim(txtOLD.Text)
                        End If
                        
                        !fileduration = cboFile.Text
                        !regDate = dtRegDate.Value
                        If Enforce_Yearly_Re_Registration_Private = "YES" Then
                            !expirydate = dtExpDate.Value
                        Else
                            !expirydate = Null
                        End If
                        !psurname = Trim(Replace(Replace(txtSurNAme.Text, "[", ""), "]", ""))
                        !pfirstName = Trim(Replace(Replace(txtfirstNAme.Text, "[", ""), "]", ""))
                        !homeAddress = txtHomeAddress.Text
                        !Area = cboArea.Text
                        !officeAddress = txtOfficeAddress.Text
                        !pPhoneNo = Trim(txtPhone.Text)
                        If Not IsNull(dtDate.Value) Then
                            !DOB = dtDate.Value
                        End If
                        !Email = txtEmail.Text
                        !sex = txtSex.Text
                        !Maturity = cboMat.Text
                        !ref = cboRef.Text ''ffs
                        !HmoRef = cboRefHmo.Text 'referal
                        '!coyClass = cboCat2   'cboClient.Text     'cboType.Text
                        '!coytype = cboCat3   'cboClient.Text     'cboType.Text
                        !clientCatID = strTariff   'cboClient.Text
                        !coyType = coyType   'cboClient.Text      'cboType.Text
                        !CoyName = strpCode
                        !empno = Trim(txtEmp.Text)
                        '!branch = txtBr.Text
                        '!Status = txtStatus.Text
                        '!relationToStaff = txtRel.Text
                        '!introducedby = strIntro
                        !policyType = txtPolicy.Text
                        !CardType = cboCard.Text
                        !MStatus = cboMStatus.Text
                        '!pMembers = txtMem.Text
                        !bloodGroup = txtBG.Text
                        !genotype = txtGeno.Text
                        !occupation = txtOccu.Text
                        !religion = cboStatus.Text
                        !nextOfKin = txtKin.Text
                        !NOKPhone = Trim(txtNOKPhone.Text)
                        !relationToKin = txtKinRel.Text
                        !kinAddress = txtKinAddress.Text
                        
                        If Trim(txtAdmitLimit.Text) = "" Then
                            !AdmissionDaysLimit = 0
                        Else
                            !AdmissionDaysLimit = txtAdmitLimit.Text
                        End If
                        If cboNew.Text = "NEW" Then
                            !NewReg = "YES"
                        Else
                            !NewReg = "NO"
                        End If
                        
                        If dblSaveBF <> 0 Then 'debt info
                            !Debt = dblSaveBF
                            !DebtBF = dblSaveBF
                        End If
                        
                        .Update
                        
    
                                Call genConID(connTran) 'ok outside chkReg ' consultID nece before insert into billAccum
    
                              If chkReg.Value = vbChecked Then
    
                                Call getAttendance(connTran) 'FOR r-jOLAD AND OTHERS
                                Call getServiceFeeConsulting(connTran)  'in case of other clinics
    
                            End If
    
        Dim I As Integer
    
    
    
                'rsC.Open "select * from hPatientClinics where 1=2", connTran, adOpenStatic, adLockOptimistic
                'rsC.AddNew
                'rsC!pNo = Trim(txtSupp.Text)
                'rsC!regDate = dtRegDate.Value
                'rsC!Clinic = "(GENERAL)"
                'rsC!Remarks = ""
                '
                'If Not IsNull(dtExpDate.Value) Then
                '    rsC!expireDate = dtExpDate.Value
                'End If
                '
                'rsC!Active = 1
                'rsC.Update
    
                If cboNew.Text = "NEW" Then
                    'If strCoyID = strPrivate Then
                        Call getServiceFeeForReg(connTran, "(GENERAL)")  'reg fee
                    'End If
                End If
                
                
    
    
                    If flgHMO = False Then
    
    
    
                         dtSys = getSysDateTime ' current time and date from server
                            'Call insIDNo(connTran)
                    
                    End If
    
                    strCval = ""
                    
                    If isInsert <= 0 Then
                        connTran.CommitTrans
                        isInsert = 1
                        'isDependant = False 'rem nece now done by cmdAdd_Click routine
                    Else
                                
                        SSTab1.Tab = 0
                        OKButton.Enabled = True
                        Call clearFields
                        Call SetButtons(True)
                        enableFields False
                        Call fillGrid
                        Call getDocWaitList
                        'isDependant = False 'rem nece now done by cmdAdd_Click routine
                        If connTran.State = adStateOpen Then
                            Set connTran = Nothing
                        End If
                        Exit Sub
                    End If
                    
                    
                    SSTab1.Tab = 0
                    Call Auditrail(m_Username, "Insert Reg for " & txtSurNAme.Text & " " & txtfirstNAme.Text, txtSupp.Text, "", strHostName)
                    
                    'send sms routine called by both insert and edit for this page ONLY
                    If chkReg.Value = vbChecked And cboPurpose.Text <> "APPOINTMENT" Then 'Attendance
                        ''''''''''''''Send SMS'''''''ATTEND''''''''''''''''''''''''''''''
                        'Call sendToSmsCenter(Trim(txtSupp.Text), strCval, "ATTEND", "ATTENDANCE", sysDate, sysTime)
                        Call sendToSmsCenter(Trim(txtSupp.Text), strCval, "ATTEND", "ATTENDANCE", sysDate, sysTime, Trim(txtPhone.Text), Trim(txtEmail.Text))
    
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''
                    End If
                    
                    
                    
                    If chkReg.Value = vbChecked And cboPurpose.Text = "APPOINTMENT" Then            'nece here for Appt
                        ''''''''''''''Send SMS'''''''''''''''''''''''''''''''''''''
                        Call sendToSmsCenter(Trim(txtSupp.Text), strCval, "APPT", cboClin.Text & " Clinic Appt", dtClinic, tClinic, Trim(txtPhone.Text), Trim(txtEmail.Text))
                
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''
                    End If
                        
                    
                    MsgBox "Record Succesfully saved"
                End If
    End With
    
          On Error GoTo errX
    
                Call setPix 'ok here
    
    
                'Dim rsArea As New Recordset
                'With rsArea
                '    .Open "select * from hPatientArea where AreaName='" & Trim(cboArea.Text) & "'", conSTR, adOpenStatic, adLockOptimistic
                '    If Not .EOF Then
                '    'do nothing
                '    Else
                '        .Close
                '        .Open "select * from hPatientArea where 1=2", conSTR, adOpenStatic, adLockOptimistic
                '        .AddNew
                '        !AreaName = Trim(cboArea.Text)
                '        .Update
                '    End If
                'End With
    
            'isDependant = False 'rem nece now done by cmdAdd_Click routine
            Set connTran = Nothing
            Set rsExP = Nothing
            
            isNewEntry = False
            flgOn = False
            SSTab1.Tab = 0
            Call SetButtons(True)
            enableFields False
            Call fillGrid
            Call getDocWaitList
            If isDependant = True Then
                Dim intSave2 As Integer
                intSave2 = MsgBox("Any other Dependant?", vbYesNo, "Addtional Dependant")
                If intSave2 = vbYes Then
                    Call cmdPatDep_Click
                Else
                    Call clearFields
                End If
            Else
                Call clearFields
            End If
            
    
    
    
    
    '        If strFldPath <> "" Then
    '            Call DeleteAllFiles(strFldPath)
    '        End If
    

Else
    Exit Sub
End If
Screen.MousePointer = vbDefault
Exit Sub
errH:
        'If Err.number = "-2147217900" Then  'violation of Primary key
        '    'OKButton.Enabled = True
        '    'iDNo = iDNo + 1
        '    Call genIDNo   'getCorrectConID(connTran)
        '    rsExP!pNo = strCardNo
        '    Resume
        'Else
    OKButton.Enabled = True
    Screen.MousePointer = vbDefault
    connTran.RollbackTrans
    MsgBox Err.Description
    Set connTran = Nothing
        'End If
        
        
Call updateConsultID
Call updatePatientID
        
Screen.MousePointer = vbDefault
Exit Sub

errX:

Screen.MousePointer = vbDefault
MsgBox Err.Description
Set connTran = Nothing
End Sub

Public Sub enableFields(xVal As Boolean)
Dim ctl As Control
For Each ctl In Me.Controls
    If TypeOf ctl Is TextBox Then
        ctl.Locked = Not xVal 'for locked
    End If
Next

For Each ctl In Me.Controls
    If TypeOf ctl Is ComboBox Then
        ctl.Enabled = xVal
    End If
Next

For Each ctl In Me.Controls
    If TypeOf ctl Is DTPicker Then
        ctl.Enabled = xVal
    End If
Next


'Frame3.Enabled = True
txtName.Enabled = True
txtName.Locked = False


 'Call txtSupp_Change

     'cboCat.Enabled = False
    'cboCat2.Enabled = True
    'cboCat3.Enabled = True
    'cboCat4.Enabled = True

'dtExpDate.Enabled = False

End Sub

Public Sub clearFields()

On Error GoTo errH
grdAttend.Caption = "Attendance History"
    txtAdmitLimit.Text = 0
    dtNext.Value = ""
    dtRefTime.Value = ""

Label5.Caption = "Patient Attendance / Registration"
strpCode = ""
lblDebtCap.Caption = "Carry Over Debt"
lblDebt.Caption = "0"

flgCardRenew = False
MdiSapid.lblDebt.Caption = 0
dblSaveBF = 0
blnInstantFee = False
dblInstantFee = 0
strItemName = ""
isDependant = False 'nece here
strPrinNo = ""
principalNo = ""
dblSaveBF = 0 'very impt 'to prevent it spilling to subsequent newly reg pat
flgOn = False
Call MdiSapid.Timer2_Timer



planID = ""
planName = ""

cmdPatDep.Enabled = False
cmdANC.Enabled = False
cmdRenew.Enabled = False

cboArea.ListIndex = 0
cboStatus.ListIndex = 0

cboRef.ListIndex = -1
SSTab1.Tab = 0
chkReg.Value = False
flgFill = False
    cboTitle.ListIndex = -1
    txtPix.Text = ""
    piX.Picture = Nothing
    lblFound.Caption = "***"
    txtSupp.Text = ""
    dtRegDate.Value = Date
    dtExpDate.Value = ""
    txtSurNAme.Text = ""
    txtfirstNAme.Text = ""
    txtHomeAddress.Text = ""
    cboArea.ListIndex = 0
    txtOfficeAddress.Text = ""
    txtPhone.Text = ""
    dtDate.Value = ""
    txtEmail.Text = ""
    'txtSex.Text = ""
    'cboCat.Text = ""
    'cboType.Text = ""
    'cboPat.Text = ""
    txtEmp.Text = ""
    'txtBr.Text = ""
    'txtStatus.Text = ""
    'txtRel.Text = ""
    strIntro = ""
    'txtBr.Text = ""
    cboMStatus.ListIndex = -1
    cboCard.ListIndex = -1
    'txtMem.Text = ""
    'txtBG.Text = ""
    'txtGeno.Text = ""
    txtOccu.Text = ""
    cboStatus.ListIndex = 0
    txtKin.Text = ""
    txtNOKPhone.Text = ""
    txtKinRel.Text = ""
    txtKinAddress.Text = ""
    txtAge.Text = ""
    'cboFile.Text = ""
    txtOLD.Text = ""
    txtPolicy.ListIndex = -1
    'cboClient.Text = ""
    txtName.Text = ""
    strPcat = ""
    cboTitle.ListIndex = -1
    'cboClient.Text = ""
    Dim ctl As Control
    For Each ctl In Me.Controls
        If TypeOf ctl Is ComboBox Then
        ctl.ListIndex = -1
        End If
    Next
    txtAge = ""
    txtSupp.Text = ""
Exit Sub

errH:
MsgBox Err.Description
End Sub

Public Sub genIDNo()
  '''''''
  On Error GoTo errH
        ' Dim rsGen As New ADODB.Recordset
        ' With rsGen
        ''.Open "select ID from IDgen where DestName = 'Patient'", conSTR, adOpenStatic, adLockOptimistic
        '.Open "select MAX(cast(SUBSTRING(Pno, 5, 9)as bigint))  as ID from hPatients WHERE  (SUBSTRING(Pno, 1, 3)='" & strHospID & " ')", conSTR, adOpenForwardOnly, adLockReadOnly
        'If rsGen.EOF Then
        '    'MsgBox "File No generator has encountered some problems"
        '    strIDConVal = "000000001"
        'Else
        '     iDNo = IIf(IsNull(!ID), 0, !ID)
        '     iDNo = iDNo + 1
        '    ' vVal = iDNo
        '
        ''strPcatNum = CStr(Right("000000000" & CStr(iDNo), 9))
        '
        '    strIDConVal = Right("000000000" & CStr(iDNo), 9)
        '
        'End If
        '
        '    strPcat = strIDConVal
        '    strCardNo = HNo & "/" & strPcat
        
    getID_No = "" 'ok b4 call of getIDNo
    Call getIDNo("PATIENT2")
    'strIDConVal = getID_No 'Right("000000000" & CStr(getID_No), 9)
    If getID_No = "" Then
        'MsgBox "Unable to generate No!!! Function getIDNo Failed"
        'Unload Me
        Exit Sub
    End If
    
    strCardNo = getID_No
    txtSupp.Text = strCardNo
    
    
    If AUTO_CARD_NO = "YES" And Trim(txtOLD.Text) = "" Then
        strIDConVal = strCardNo
        strCDNo = ""
        strCDNo = Right(CStr(Year(sysDate)), 2) & "/" & Right(strIDConVal, 5)
        txtOLD.Text = strCDNo
    Else
        'do nothing
        'txtOLD.Text = ""
    End If


'txtSupp.Text = Right("0000000" & CStr(iDNo), 7)
'Set rsGen = Nothing
' End With
  '''''''
  Exit Sub
errH:
'If rsGen.EOF Then rsGen!ID = 0
'Resume Next
MsgBox Err.Description
End Sub

Public Sub insIDNo(connTran As Connection)
'On Error GoTo errH
'  '''''''
' Dim Cmd As New ADODB.Command
' 'Dim strHN
' 'strHN = lblBill.Caption
' With Cmd
'.ActiveConnection = connTran
'.CommandType = adCmdText
'.CommandText = "Update iDGen set ID=" & iDNo & " where DestName = 'Patient'"
'.Execute
'Set Cmd = Nothing
' End With
'  '''''''
' Exit Sub
'errH:
' MsgBox "Problems Saving generated Patient No with error " & Err.Description
'
'
  End Sub


Private Sub OrderGrid_DblClick()
'On Error GoTo errH
'Dim intSave As Integer
'intSave = MsgBox("Are you sure to Delete?", vbYesNo, "Check before Delete")
'If intSave = vbYes Then
'    If OrderGrid.RowSel > 1 Then
'
'    OrderGrid.RemoveItem (OrderGrid.RowSel)
'
'    Else
'            '    OrderGrid.TextMatrix(OrderGrid.RowSel, 0) = ""
'            '    OrderGrid.TextMatrix(newRow, 1) = ""
'            '    OrderGrid.TextMatrix(newRow, 2) = ""
'            '    OrderGrid.TextMatrix(newRow, 3) = ""
'            '    OrderGrid.TextMatrix(newRow, 4) = ""
'
'
'    OrderGrid.RemoveItem (OrderGrid.RowSel)
'
'    End If
'
'    'nuM = nuM - 1
'    'Label4.Caption = nuM & " Items added"
'
'    MsgBox "Item Removed"
'End If
'
'Exit Sub
'errH:
'MsgBox Err.Description
End Sub

Private Sub SSTab1_Click(PreviousTab As Integer)
'If SSTab1.Tab = 2 Then
'    Dim StartResult As Integer
'
'    LoadSettings
'
'    cmdSnap.Enabled = True
'        'If SelectedCamera >= 0 Then
'        '    If dspPreview.StartCamera(mnuCamerasChoice(SelectedCamera).Caption) Then
'        '        cmdSnap.Enabled = True
'        '    Else
'        '        DeselectFailedCamera dspPreview.Error
'        '    End If
'        'End If
'End If

End Sub

Private Sub LoadSettings()
'    Dim F As Integer
'    Dim c As Integer
'    Dim CameraName As String
'
'    SelectedCamera = -1 'None.
'    On Error Resume Next
'    GetAttr "Settings.txt"
'    If Err.number = 0 Then
'        On Error GoTo 0
'        F = FreeFile(0)
'        Open "Settings.txt" For Input As #F
'        Input #F, SelectedCamera
'        Do Until EOF(F)
'            Input #F, CameraName
'            If c > 0 Then Load mnuCamerasChoice(c)
'            With mnuCamerasChoice(c)
'                .Enabled = True
'                .Caption = CameraName
'                .Checked = c = SelectedCamera
'            End With
'            c = c + 1
'        Loop
'        Close #F
'        mnuCamerasRemove.Enabled = True
'    End If
End Sub

Private Sub tmrSearch_Timer()
'On Error GoTo errH
'
'Do While cnt2 <= (intWait - 1)
'
'    MsgWaitObj (1000)
'    cnt2 = cnt2 + 1
'Loop
'
'searchVal = "Name"
'getPatInfo (CInt(SearchValue)) '
'
'Exit Sub
'errH:
''MsgBox Err.Description
'

End Sub

Private Sub txtAdmitLimit_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
Case Is < 32 ' Control keys are OK.
Case 46 ' This is a period.
    If KeyAscii = 46 Then
         If InStr(1, txtAdmitLimit.Text, ".") > 0 Then
             KeyAscii = 0
        End If
    End If
Case 48 To 57 ' This is a digit.
Case Else ' Reject any other key.
KeyAscii = 0
End Select
End Sub

Private Sub txtAge_Change()
On Error GoTo errH
If isNewEntry = True Then
    'dtDate.Value = Date
    Dim intAge As Integer
    If IsNumeric(txtAge.Text) Then
        intAge = txtAge.Text
        dtDate.Value = CDate(DateAdd("yyyy", -intAge, Date))
    Else
        'txtAge.Text = ""
    End If
Else
    'MsgBox "Specify Age using the Date Picker"
    'dtDate.SetFocus
End If
Exit Sub
errH:
MsgBox Err.Description

End Sub





Private Sub txtName_Change()
On Error GoTo errH  ' Resume Next

Dim tName As String
tName = Trim(txtName.Text)


If tName = "" Then
 Set grdData.DataSource = Nothing
 lblFound.Caption = "0 Records Found"
 Exit Sub
End If

If Len(tName) <= 2 Then Exit Sub '3 chars or above



'If InStr(tName, 0) > 0 Or InStr(tName, 1) > 0 Then Exit Sub


Screen.MousePointer = vbHourglass

'flgSch = True
    
    Call MsgWaitObj(CInt(SearchWaitTime))  'wait fn in millisec
    
    Label5.Caption = "Retrieving data...Please wait"
    Label5.BackColor = vbRed
    Label5.Refresh
        
    searchVal = "Name"
    getPatInfo (CInt(SearchValue)) 'only here in txtSearch_Change
    
    Label5.Caption = "Patient Attendance / Registration"
    Label5.BackColor = vbBlack
    Label5.Refresh
    
    
Screen.MousePointer = vbDefault

Exit Sub

errH:
Screen.MousePointer = vbDefault
'MsgBox Err.Description

End Sub

Private Sub txtPhone_KeyPress(KeyAscii As Integer)
Select Case KeyAscii
Case Is < 32 ' Control keys are OK.
Case 46 ' This is a period.
    KeyAscii = 0

'    If KeyAscii = 46 Then
'         If InStr(1, txtPhone.Text, ".") > 0 Then
'             KeyAscii = 0
'        End If
'    End If
Case 48 To 57 ' This is a digit.
Case Else ' Reject any other key.
    KeyAscii = 0
End Select
End Sub

Private Sub txtPolicy_Click()
On Error GoTo errH
'If flgEdit = True Then Exit Sub
 If txtPolicy.ListIndex = 0 Or txtPolicy.ListIndex = -1 Then Exit Sub
 planID = ""
 planName = ""
 planID = Mid(txtPolicy.Text, InStr(txtPolicy.Text, "[") + 1, Len(txtPolicy.Text) - (InStr(txtPolicy.Text, "[") + 1))
 planName = Mid(txtPolicy.Text, 1, InStr(txtPolicy.Text, "[") - 2)
 

Exit Sub
errH:
'MsgBox Err.Description
'Call fillGridHMO
End Sub

Private Sub txtSex_Click()
'If txtSex.Text = "FEMALE" Then
'    cmdANC.Enabled = True
'Else
'    cmdANC.Enabled = False
'End If
End Sub

Private Sub txtSupp_Change()
'If Trim(txtSupp.Text) <> "" Then
'    cmdPatDep.Enabled = True
'Else
'    cmdPatDep.Enabled = False
'End If
End Sub


Private Sub txtsurNAme_KeyPress(KeyAscii As Integer)
KeyAscii = Asc(UCase(Chr(KeyAscii)))
End Sub

Private Sub txtfirstNAme_KeyPress(KeyAscii As Integer)
KeyAscii = Asc(UCase(Chr(KeyAscii)))

End Sub

Private Sub txtsurNAme_LostFocus()
On Error GoTo errH
If Trim(txtSurNAme.Text) = "" Then Exit Sub
Dim rsVal As New Recordset
Dim strV As String
strV = Replace(txtSurNAme.Text, "'", "''")
Dim sSQL As String
sSQL = "select distinct psurname as Surname,pfirstname as Firstname,pno as NewFileNo,oldpno as [Old FileNo],homeAddress from hpatients where psurname = '" & strV & "'"
With rsVal
.CursorLocation = adUseClient
.Open sSQL, conStr, adOpenStatic, adLockOptimistic
'MsgBox ssQL
If Not .EOF Then
MsgBox "One or more Surnames match !!! Please Confirm for Duplicates"
Set frmVerify.grdData.DataSource = Nothing
Set frmVerify.grdData.DataSource = rsVal
frmVerify.Show vbModal

'frmVerify.Show
Else
'Set frmVerify.grdData.DataSource = Nothing
End If
End With
'Set rsVal = Nothing
Exit Sub
errH:
MsgBox Err.Description
End Sub

Public Sub addDependants()
'
'        On Error Resume Next
'
''''''''''''''''''''''''''''''''''''
'        'frmPatientsDep.txtSupp.Text = Left(strSupp, 13) & XYZ & Mid(strSupp, 15) 'txtSupp.Text
'        frmPatientsDep.cboTitle.Text = ""
'        frmPatientsDep.txtOLD.Text = txtOLD.Text
'        frmPatientsDep.cboFile.Text = cboFile.Text
'       frmPatientsDep.dtRegDate.Value = dtSys   'dtRegDate.Value
'
'       If dtExpDate.Value = "" Then
'       frmPatientsDep.dtExpDate.Value = ""
'       Else
'        frmPatientsDep.dtExpDate.Value = dtExpDate.Value
'        End If
'
'        frmPatientsDep.txtsurNAme.Text = txtsurNAme.Text
'       frmPatientsDep.txtfirstNAme.Text = txtfirstNAme.Text
'        frmPatientsDep.txtHomeAddress.Text = txtHomeAddress.Text
'        'frmPatientsDep.txtOfficeAddress.Text = txtOfficeAddress.Text  'not nece
'        frmPatientsDep.txtPhone.Text = txtPhone.Text
'        frmPatientsDep.dtDate.Value = ""  'dtDate.Value
'        frmPatientsDep.txtEmail.Text = txtEmail.Text
'        frmPatientsDep.txtSex.Text = ""
'        frmPatientsDep.txtArea.Text = cboArea.Text
'        'frmPatientsDep.cboCat.Text = cboCat.Text
'        'frmPatientsDep.cboType.Text = cboClient.Text      'cboType.Text
'        'frmPatientsDep.cboPat.Text = strpCode       'cboPat.Text
'        frmPatientsDep.txtEmp.Text = ""  ' txtEmp.Text
'        'frmPatientsDep.txtBr.Text = txtBr.Text
'        'frmPatientsDep.txtStatus.Text = txtStatus.Text
'        'frmPatientsDep.txtRel.Text = txtRel.Text
'        'frmPatientsDep.txtIntro.Text = txtIntro.Text
'        frmPatientsDep.txtPolicy.Text = txtPolicy.Text
'        'frmPatientsDep.cboCard.Text = cboCard.Text
'        '!clientCatID = "XXX" 'cboClient.Text
'        'frmPatientsDep.txtMem.Text = txtMem.Text
'        frmPatientsDep.txtBG.Text = "" 'txtBG.Text
'        frmPatientsDep.txtGeno.Text = ""  'txtGeno.Text
'        frmPatientsDep.txtOccu.Text = txtOccu.Text
'        frmPatientsDep.cboStatus.Text = cboStatus.Text
'        frmPatientsDep.txtKin.Text = txtKin.Text
'        frmPatientsDep.txtNOKPhone.Text = txtNOKPhone.Text
'        frmPatientsDep.txtKinRel.Text = txtKinRel.Text
'        frmPatientsDep.txtKinAddress.Text = txtKinAddress.Text
'        'frmPatientsDep.cboClient.Text = strTariff
'        ''
'            'frmPatientsDep.txtMem.Text = ""
'            'frmPatientsDep.txtKin.Text = ""
'            'frmPatientsDep.txtNOKPhone.Text = ""
'            frmPatientsDep.txtPhone.Text = ""
'            frmPatientsDep.dtDate.Value = ""
'            frmPatientsDep.txtfirstNAme = ""
'
'
'


End Sub

Private Sub SetButtons(bVal As Boolean)
  cmdAdd.Visible = bVal
  cmdEdit.Visible = bVal
  OKButton.Visible = Not bVal
  cmdCancel.Visible = Not bVal
  cmdDel.Visible = bVal
  'cmdRefresh.Visible = bVal
End Sub


Public Function isPnoValid(strpNO As String) As Boolean
'If Mid(strpNO, 1, 4) = "MED/" Then
    If IsNumeric(Mid(strpNO, 5, 9)) Then
        isPnoValid = True
    Else
    isPnoValid = False
    End If
'Else
'    isPnoValid = False
'End If


End Function

Public Sub getCorrectConID(connTran As Connection)
'strCardNo = ""
'Dim rsBL As New Recordset
'  With rsBL
'  .Open "select top 1 IdVal from qryidgen2", connTran, adOpenForwardOnly, adLockReadOnly
''.MoveFirst
'If .EOF Then
'IDConVal = iDNo
'Else
'IDConVal = !idval) + 1
'End If
'End With
'Set rsBL = Nothing
'
'    If iDNo < IDConVal Then
'        iDNo = IDConVal
'    End If
'
'    strIDConVal = Right("000000000" & CStr(iDNo), 9)
'
'
'
'        'Call genIDNo
'
'    strPcat = strIDConVal
'    strCardNo = HNo & "/" & strPcat
'    txtSupp.Text = strCardNo



End Sub



Public Sub getServiceFeeForReg(connTran As Connection, Clinic As String)
'On Error GoTo errH
    Dim dtSys As Date
    dtSys = getSysDateTime

Select Case Clinic
Case "GENERAL", "OUT-PATIENT", "(GENERAL)"
    If cboCard.Text = "SINGLE" Then
        strCard = "SINGLE"
    ElseIf cboCard.Text = "FAMILY" Then
        strCard = "FAMILY"
    End If
Case Else
    strCard = Clinic
End Select

'strCard = cboCard.Text

Dim DBLfEE As Double
Dim dblCost As Double
Dim dblVal As Double
Dim rsVal As New Recordset
dblVal = 0
With rsVal
'If Clinic = "GENERAL" Or Clinic = "OUT-PATIENT" Or Clinic = "(GENERAL)" Then
'    .Open "select price from hServiceNHIS where sno=" & PVT_REGFEE_SNO, connTran, adOpenStatic, adLockOptimistic
'     If Not .EOF Then
'        DBLfEE = FormatNumber(IIf(IsNull(!Price), 0, !Price), 2)
'    Else
'        DBLfEE = 0
'    End If
'Else
'    .Open "select clinicID,ClinicName,Regfee from clinicTypes where clinicID='" & strCard & "'", connTran, adOpenStatic, adLockOptimistic
'        If Not .EOF Then
'            DBLfEE = FormatNumber(IIf(IsNull(!regfee), 0, !regfee), 2)
'        Else
'            DBLfEE = 0
'        End If
'End If

.Open "select RegAmount from hRetainership where retainID='" & strCoyID & "'", connTran, adOpenStatic, adLockOptimistic
        If Not .EOF Then
            DBLfEE = FormatNumber(IIf(IsNull(!RegAmount), 0, !RegAmount), 2)
        Else
            DBLfEE = 0
        End If

'If Not .EOF Then
    .Close
    .Open "select *  from billAccum where 1=2", connTran, adOpenStatic, adLockOptimistic
    .AddNew
    !dtDate = Format(dtSys, "Short Date")
    !drgName = "REGISTRATION"
    !Price = DBLfEE
    !Qty = 1
    !SubTotal = DBLfEE
    !PNo = txtSupp.Text
    !consultID = strCval
    !billType = "SERVICE"     'strCard & " REG"
    !ConID = Null
    !Category = "REGISTRATION"
    !attendedto = 0
    !suppres = 0
    !Capitated = "NO"
    !isbilled = 0
    !CoyName = strpCode
    !billTo = strBillTo
    !revType = "REGISTRATION"
    .Update


End With

Set rsVal = Nothing

Call Auditrail(m_Username, "Reg Fee for " & txtSurNAme.Text & " " & txtfirstNAme.Text, txtSupp.Text, "", strHostName)


End Sub


Public Sub getCardRenewFee(connTran As Connection)
'On Error GoTo errH


Dim DBLfEE As Double
Dim dblCost As Double
Dim dblVal As Double
Dim rsVal As New Recordset
dblVal = 0
With rsVal


.Open "select CardRenewAmount from hRetainership where retainID='" & strCoyID & "'", connTran, adOpenStatic, adLockOptimistic
        If Not .EOF Then
            DBLfEE = FormatNumber(IIf(IsNull(!CardRenewAmount), 0, !CardRenewAmount), 2)
        Else
            DBLfEE = 0
        End If

    .Close
    .Open "select *  from billAccum where 1=2", connTran, adOpenStatic, adLockOptimistic
    .AddNew
    !dtDate = Format(dtSys, "Short Date")
    !drgName = "CARD RENEWAL FEE"
    !Price = DBLfEE
    !Qty = 1
    !SubTotal = DBLfEE
    !PNo = txtSupp.Text
    !consultID = strCval
    !billType = "SERVICE"     'strCard & " REG"
    !ConID = Null
    !Category = "CARD_RENEW"
    !attendedto = 0
    !suppres = 0
    !Capitated = "NO"
    !isbilled = 0
    !CoyName = strpCode
    !billTo = strBillTo
    !revType = "REGISTRATION"
    .Update


End With

Set rsVal = Nothing

Call Auditrail(m_Username, "Card Renew Fee for " & txtSurNAme.Text & " " & txtfirstNAme.Text, txtSupp.Text, "", strHostName)


End Sub




Public Sub getServiceFeeForAttendance(conn As Connection)
'
'
'Dim DBLfEE As Double
'Dim dblCost As Double
'Dim dblVal As Double
'Dim rsVal As New Recordset
'dblVal = 0
'
'    Dim dtSys As Date
'    dtSys = getSysDateTime
'
'With rsVal
'.Open "select * from clinicTypes where clinicID ='" & cboClin.Text & "'", conN, adOpenStatic, adLockOptimistic
'If Not .EOF Then
'        DBLfEE = FormatNumber(IIf(IsNull(!ConFee), 0, !ConFee), 2)
'
'.Close
'.Open "select *  from billAccum where 1=2", conN, adOpenStatic, adLockOptimistic
'.AddNew
'!dtDate = Format(dtSys, "Short Date")
'!drgNAME = cboClin.Text & " ATTENDANCE"     'strClinicGen & " ATTENDANCE"
'!Price = DBLfEE
'!Qty = 1
'!subTotal = DBLfEE
'!pno = txtSupp.Text
'!consultID = strCval
'!billtype = cboClin.Text & " ATTENDANCE"     'strClinicGen & " ATTENDANCE"
'!conID = strCval
'
'
'.Update
'
'
''    Select Case cboClient.Text
''        Case "PRIVATE", "DEFAULT"
''        DBLfEE = FormatNumber(!Private + (!Private * dblVal), 2)
''        Case "HMO"
''        DBLfEE = FormatNumber(!HMO + (!HMO * dblVal), 2)
''        Case "NHIS"
''        DBLfEE = FormatNumber(!NHIS + (!NHIS * dblVal), 2)
''        Case "MTHLY"
''        DBLfEE = FormatNumber(!MTHLY + (!MTHLY * dblVal), 2)
''        Case "3MTHLY"
''        DBLfEE = FormatNumber(![3MTHLY] + (![3MTHLY] * dblVal), 2)
''        Case "6MTHLY"
''        DBLfEE = FormatNumber(![6MTHLY] + (![6MTHLY] * dblVal), 2)
''        Case "CBN"
''        DBLfEE = FormatNumber(![CBN] + (![CBN] * dblVal), 2)
''        Case "NEPA", "PHCN"
''        DBLfEE = FormatNumber(![NEPA] + (![NEPA] * dblVal), 2)
''        Case Else
''        DBLfEE = FormatNumber(!Private, 2)
''    End Select
'End If
'
'
'
'
'
'
'End With
'
'Set rsVal = Nothing
'
End Sub


Public Sub genConID(connTran As Connection) 'TO GET LAST CONsultID
Dim cVal As Long
Dim cVal2 As Long
Dim strValX As String

'Dim rsBL As New Recordset
'  With rsBL
'.Open "select MAX(cast(SUBSTRING(consultID, 4, 9)as bigint))  as ID from hRecords WHERE   SUBSTRING(consultID, 4, 3)<>'ofl' and (SUBSTRING(consultID, 1, 3)='" & strHospID & " ')", conSTR, adOpenForwardOnly, adLockReadOnly
'If .EOF Then
'    'MsgBox "File No generator has encountered some problems"
'    strValX = "000000001"
'Else
'     cVal = IIf(IsNull(!ID), 0, !ID)
'
'     If cVal <= 0 Then
'        .Close
'        .Open "select top 1 conIdVal from qryidgenCon ORDER BY conIdVal DESC", connTran, adOpenForwardOnly, adLockReadOnly
'        If .EOF Then
'            cVal = 0  'IIf(IsNull(!conIdVal), 0, !conIdVal)
'        Else
'            cVal = IIf(IsNull(!conIdVal), 0, !conIdVal)) '+1
'
'        End If
'    End If
'
'    cVal2 = cVal + 1
'    strValX = Right("000000000" & CStr(cVal2), 9)
'
'End If
'
'strCval = ""
'strCval = HNo & strValX
  

'End With

    getID_No = "" 'ok b4 call of getIDNo
    Call getIDNo("ConsultID2")
            If getID_No = "" Then
                'MsgBox "Unable to generate No!!! Function getIDNo Failed! ConsultID"
                'Unload Me
                Exit Sub
            End If
    
    strCval = getID_No
    gStrCval = strCval
    



'Set rsBL = Nothing
End Sub

Public Sub getAttendance(connTran As Connection)
    
    Dim dtSys As Date
    Dim AttndPNo As String
    Dim AttndFullName As String
    Dim entryDate As Date
    Dim entryTime As Date
    
    dtSys = getSysDateTime
    
    AttndPNo = Trim(txtSupp.Text)
    AttndFullName = Trim(txtSurNAme.Text) & " " & Trim(txtfirstNAme.Text)
    entryDate = Format(dtSys, "Short Date")
    entryTime = Format(dtSys, "Short Time")
    
    
    If cboPurpose.Text = "APPOINTMENT" Then
        Dim rsAppt2 As New ADODB.Recordset
        rsAppt2.Open "Select * from happointment where 1=2", connTran, adOpenStatic, adLockOptimistic
                    rsAppt2.AddNew
                    rsAppt2!consultID = strCval
                    rsAppt2!clientCat = strTariff
                    rsAppt2!clinicType = cboClin.Text
                    rsAppt2!ApptDate = dtClinic
                    rsAppt2!ApptTime = tClinic
                    rsAppt2!Remarks = cboClin.Text & " Appointment"
                    rsAppt2!PNo = AttndPNo
                    rsAppt2!entryDate = entryDate
                    rsAppt2!entryTime = entryTime
                    rsAppt2!attendedto = 0
                    
                    rsAppt2.Update
                
                Call Auditrail(m_Username, "Insert Appt for: " & txtSurNAme.Text & " " & txtfirstNAme.Text, strCval, cboClin.Text, strHostName)
        
            
            'nece here for Appt
            ''''''''''''''Send SMS'''''''''''''''''''''''''''''''''''''
                'causes tiemout expire
                'Call sendToSmsCenter(Trim(txtSupp.Text), strCval, "APPT", cboClin.Text & " Clinic Appt", dtClinic, tClinic, Trim(txtPhone.Text), Trim(txtEmail.Text))
        
            ''''''''''''''''''''''''''''''''''''''''''''''''''''
    End If

        'Dim rsBLxx As New Recordset  'assign Doctor
        'Dim StrDocX As String
        'Dim strEmpDoc As String
        'With rsBLxx
        '  .Open "select distinct top 1 DocName,empID,RoomNo  from vwDocMinNumOfPat", connTran, adOpenForwardOnly, adLockReadOnly
        '    If Not .EOF Then
        '        StrDocX = !DocName & " @ " & !RoomNo & " [" & !EmpID & "]"
        '        'cboRef.Text = StrDocX
        '        strEmpDoc = !EmpID
        'Else
        '        'cboRef.ListIndex = 0
        '    End If
        'End With
        
            dblBF = 0
                
                
                'If flgEdit = True Then 'existing pat ONLY for debt
                Dim cmd As New Command
                cmd.ActiveConnection = connTran
                cmd.CommandText = "update hPatients set LastPurpose='" & cboPurpose.Text & "', LastClinicVisited ='" & cboClin.Text & "', LastAttndDate='" & entryDate & "', LastConsultID = '" & strCval & "' where pno='" & AttndPNo & "'"
                cmd.Execute
                'End If
                
                'get last info from hRecords b4 insert
                Dim LastConsultID As String
                Dim LastAttndDate As Date
                Dim LastClinicVisited As String
                Dim LastPurpose As String
               
                Dim isFound As Boolean
                Dim rsLast As New Recordset
                rsLast.Open "select  top 1 ConsultID,recDate,clinicType,Remarks from hRecords  where pno='" & AttndPNo & "' order by RecID desc", connTran, adOpenStatic, adLockOptimistic
                If Not rsLast.EOF Then
                    isFound = True
                    LastConsultID = rsLast!consultID
                    LastAttndDate = rsLast!recDate
                    LastClinicVisited = rsLast!clinicType & ""
                    LastPurpose = rsLast!Remarks & ""
                Else
                    isFound = False
                End If
                
                Set rsLast = Nothing
                
            Dim rsIns As New Recordset
            With rsIns
                .Open "select  * from hRecords where 1=2", connTran, adOpenStatic, adLockOptimistic
                .AddNew
                !recDate = entryDate 'sysDate  '
                !PNo = AttndPNo      'strCardNo
                !consultID = strCval
                !empID = strEmpID
                !clinicType = cboClin.Text
                !Remarks = cboPurpose.Text   'not strpurpose   ' "CONSULTATION"
                '!nextApptDate = ""  l'dtNext.Value
                !htime = entryTime
                !HmoRef = cboRefHmo.Text   'real referal
                !referal = cboRef.Text 'ffs  its ref in hPatients 'its OK
                '
                If cboDoc.Text <> "" Then
                    !DocAssigned = DocAssigned   'strEmpDoc
                    !PatVal = 1
                End If
                
                !suppres = 0
                !attendedto = 0
                !attendedtoByDoc = 0
                '!PatVal = 1
                !attendedToByNurse = 0
                '!ExitDate = entryDate 'formaula field
                !BillDate = entryDate 'entryDate
                
                !clientCat = strTariff    'cboClient.Text
                !CoyName = strpCode
                !Debt = dblSaveBF
                
                If isFound = True Then
                    !LastConsultID = LastConsultID
                    !LastAttndDate = LastAttndDate
                    !LastClinicVisited = LastClinicVisited
                    !LastPurpose = LastPurpose
                End If
                
                
                .Update
                
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                .Close
                .Open "select *  from billAccum where 1=2", connTran, adOpenStatic, adLockOptimistic
                .AddNew
                !dtDate = entryDate
                !drgName = "ATTENDANCE"  'strConAttd & " ATTENDANCE"
                !Price = 0 'seed   ' DBLfEE
                !Qty = 1
                !SubTotal = 0 'seed  ' DBLfEE
                !PNo = AttndPNo
                !consultID = strCval
                !billType = "SERVICE" '& " ATTENDANCE"    '"ATTENDANCE"
                !ConID = Null
                !CoyName = strpCode
                !billTo = strBillTo
                !attendedto = 0
                !isbilled = 0
                !revType = Revtype_Consult
                !AppVersion = App.Major
                
                .Update
                
                
                    ''''''''''''''''''''''''
                If blnInstantFee = True Then
                    .Close
                    .Open "select *  from billAccum where 1=2", connTran, adOpenStatic, adLockOptimistic
                    .AddNew
                    !dtDate = entryDate
                    !drgName = strItemName
                    !Price = dblInstantFee
                    !Qty = 1
                    !SubTotal = dblInstantFee
                    !PNo = AttndPNo
                    !consultID = strCval
                    !billType = "SERVICE" '& " ATTENDANCE"    '"ATTENDANCE"
                    !ConID = Null
                    !CoyName = strpCode
                    !billTo = strBillTo
                    !attendedto = 0
                    !isbilled = 0
                    !revType = Revtype_Consult
                    !AppVersion = App.Major
                    
                    .Update
                End If
                
                    
                
            
            If Not IsNumeric(lblDebt.Caption) Then lblDebt.Caption = 0
            dblSaveBF = CDbl(lblDebt.Caption) 'ok here 'this lbl keeps dblSaveBF whether flgEdit is true or false
           
            If flgEdit = False Then 'new rec
                If dblSaveBF <> 0 Then
                    'insert into hPatientsDebtBF
                    'hPatients update already in insert stmt
                    Dim strName As String
                    strName = Trim(txtSurNAme.Text) & " " & Trim(txtfirstNAme.Text)
                    
                If .State = adStateOpen Then .Close
                .Open "select *  from billAccum where 1=2", connTran, adOpenStatic, adLockOptimistic
                .AddNew
                !dtDate = entryDate
                !drgName = "OPENING_BALANCE_DEBT"  'strConAttd & " ATTENDANCE"
                !Price = dblSaveBF
                !Qty = 1
                !SubTotal = dblSaveBF
                !PNo = AttndPNo
                !consultID = strCval
                !billType = "SERVICE" '& " ATTENDANCE"    '"ATTENDANCE"
                !ConID = Null
                !CoyName = strpCode
                !billTo = strBillTo
                !attendedto = 0
                !isbilled = 0
                !revType = Revtype_Consult
                !AppVersion = App.Major
                
                .Update
                End If
            End If
            
            
            End With  'ok here
            Set rsIns = Nothing
            
            
    dblSaveBF = 0
    Call saveDebt(AttndPNo, connTran)

            ''''''''''''''''''''''''''''''''''''''''''''''''''''
    
    
            
            
            Dim rsTime As New Recordset
            rsTime.Open "select  * from hPatientsTimeline where 1=2", connTran, adOpenStatic, adLockOptimistic
            rsTime.AddNew
            rsTime!consultID = strCval
            rsTime!ServicePoint = "ATTENDANCE"
            rsTime!EntryOrExit = "ENTRY|EXIT"
            rsTime.Update
            
            'Call getDocWaitList

                    '
                    '
                    '
                    'If (cboMat.Text = "NEONATE" And cboClin.Text = "(IN-PATIENT)") Or (cboMat.Text = "NEONATE" And cboClin.Text = "IN-PATIENT") Then
                    '     Dim rsDetailsCon As New Recordset
                    '    rsDetailsCon.Open "select * from hreferal where 1=2", connTran, adOpenStatic, adLockOptimistic
                    '        rsDetailsCon.AddNew
                    '        rsDetailsCon!consultID = strCval
                    '        rsDetailsCon!clientCat = cboClient.Text
                    '
                    '        rsDetailsCon!referto = "(IN-PATIENT)"
                    '        rsDetailsCon!ApptDate = entryDate
                    '        rsDetailsCon!ApptTime = entryTime
                    '        rsDetailsCon!refReason = "DELIVERY"
                    '        rsDetailsCon!PNo =AttndPNo
                    '        rsDetailsCon!refDate = entryDate
                    '        rsDetailsCon!refTime = entryTime
                    '    rsDetailsCon.Update
                    '
                    'End If
                    '
          
                
                Call Auditrail(m_Username, "Insert Attendance for " & txtSurNAme.Text & " " & txtfirstNAme.Text, strCval, Client & "/" & cboClient.Text & "/" & cboClin.Text & "/" & cboPurpose.Text, strHostName)
                'Call Auditrail(m_Username, "Insert Attnd Fee for " & txtsurNAme.Text & " " & txtfirstNAme.Text, strCval, "", strHostName)
            
            
            
                'If dblSaveBF <> 0 Then '-ve val means debt from tranxaction tbl. add minus to its subtotal to make it a bill
                '    Call Auditrail(m_Username, "Insert Debt for " & txtsurNAme.Text & " " & txtfirstNAme.Text, strCval, -(dblSaveBF), strHostName)
                'End If
                
            
            
            
            
        'nece here for Attnd
        ''''''''''''''Send SMS'''''''ATTEND''''''''''''''''''''''''''''''
        'causes timeout expire
        'Call sendToSmsCenter(Trim(txtSupp.Text), strCval, "ATTEND", "ATTENDANCE", entryDate, entryTime, Trim(txtPhone.Text), Trim(txtEmail.Text))
        ''''''''''''''''''''''''''''''''''''''''''''''''''''

    
End Sub


Public Sub fillGrid()
On Error GoTo errH
Dim rsVal As New Recordset
Set grdData.DataSource = Nothing
With rsVal
Dim sSQL As String
.CursorLocation = adUseClient
sSQL = "select  * from vwhpatients where RegDate='" & Date & "' order by fullname"
.Open sSQL, conStr, adOpenStatic, adLockOptimistic
'MsgBox ssQL
If Not .EOF Then
Set grdData.DataSource = Nothing
Set grdData.DataSource = rsVal
grdData.Columns("expired").Visible = False
grdData.Columns("coyname").Visible = False
Else
Set grdData.DataSource = Nothing
End If
End With
Set rsVal = Nothing
Exit Sub
errH:
'rsVal.Close
'MsgBox Err.Description

End Sub


Private Sub getPix()

                'Dim strPatPix As String
                'Dim strPixNo As String
                'Set piX.Picture = Nothing
                'strPixNo = Replace(lblNo.Caption, "/", "")
                'strPixPath = App.Path & "\Patients\"
                'strPatPix = strPixPath & strPixNo & ".JPG"
                ''MsgBox strPatPix
                '
                'piX.Picture = LoadPicture(strPatPix)
                '
                '
                'strPixNo = Replace(strpCode, "/", "")
                '
                'strPatPix = strPixPath & strPixNo & ".JPG"
                '
                'piX.Picture = LoadPicture(strPatPix)

              If IsNull(grdData.Columns("empID")) Then
              piX.Picture = LoadPicture("")
              Else
              Dim vPix As String
              vPix = Replace(grdData.Columns("pno"), "/", "")
'                If Not IsNumeric(vPix)) Then
'                    MsgBox "Invalid Image ID"
'                    Exit Sub
'                End If
              piX.Picture = LoadPicture(strPixPath & vPix & ".JPG")
              End If



End Sub


Public Sub editVal()
On Error GoTo errH

isNewEntry = False


strPatNo = ""
strPrinNo = ""

SSTab1.Tab = 1
Label5.Caption = grdData.Columns("FullName")
frmPatientsNew.txtAdmitLimit.Text = grdData.Columns("AdmissionDaysLimit")
frmPatientsNew.cboTitle.Text = grdData.Columns("title")
strPatNo = grdData.Columns("pno")
strPrinNo = grdData.Columns("pno")
gPno = strPatNo
frmPatientsNew.txtSupp.Text = strPatNo  'grdData.Columns("pno")
frmPatientsNew.txtOLD.Text = grdData.Columns("oldPno")
frmPatientsNew.dtRegDate.Value = grdData.Columns("regDate")
frmPatientsNew.txtSurNAme.Text = grdData.Columns("Surname")

frmPatientsNew.txtfirstNAme.Text = grdData.Columns("Firstname")
frmPatientsNew.txtHomeAddress.Text = grdData.Columns("homeAddress")
frmPatientsNew.cboArea.Text = grdData.Columns("Area")
frmPatientsNew.txtOfficeAddress.Text = grdData.Columns("OfficeAddress")
frmPatientsNew.txtPhone.Text = grdData.Columns("PhoneNo")

If IsNull(grdData.Columns("DOB")) Or grdData.Columns("DOB") = "" Then
    frmPatientsNew.dtDate.Value = sysDate
    frmPatientsNew.dtDate.Value = ""
    txtAge.Text = ""
Else
    frmPatientsNew.dtDate.Value = grdData.Columns("DOB")
    If IsDate(dtDate.Value) Then
        'Dim dtVal As Date
        'ageVal = dtDate.Value
        'dtVal = dtDate.Value
        'txtAge.Text = CStr(DateDiff("yyyy", ageVal, Date))
        'dtDate.Value = dtVal 'nece
        
            'txtAge.Text = ""
            Dim PatAge As String
            PatAge = CalcAge(dtDate.Value)
            txtAge.Text = PatAge
        
    Else
        txtAge.Text = ""
    End If
End If

OldClientCode = grdData.Columns("coyname")
OldClient = grdData.Columns("Client")

frmPatientsNew.txtEmail.Text = grdData.Columns("email")
frmPatientsNew.txtSex.Text = grdData.Columns("Sex")

If grdData.Columns("client") <> "" Then
    frmPatientsNew.cboPat.Text = grdData.Columns("client") & " [" & grdData.Columns("coyname") & "]"
End If
If grdData.Columns("BillingCat") <> "" Then
    frmPatientsNew.cboClient.Text = grdData.Columns("BillingCat")
End If


PatCat = grdData.Columns("patcat")

'If grdData.Columns("patcat") <> "" Then
'    frmPatientsNew.cboCat.Text = grdData.Columns("patcat")
'End If
'If grdData.Columns("coyclass") <> "" Then
'    frmPatientsNew.cboCat2.Text = grdData.Columns("coyclass")
'End If
'If grdData.Columns("coyType") <> "" Then
'    frmPatientsNew.cboCat3.Text = grdData.Columns("coyType")
'End If
'If grdData.Columns("BillingCat") <> "" Then
'    Select Case grdData.Columns("BillingCat")  'cboClient.Text
'    Case "PRIVATE"
'        If strpCode = "PRIVATE" Then
'            cboClient.AddItem ""
'            cboClient.AddItem "FEE-PAYING"
'            frmPatientsNew.cboClient.Text = "FEE-PAYING"
'        Else 'credit-Private
'            cboClient.AddItem ""
'            cboClient.AddItem "CREDIT-PRIVATE"
'            frmPatientsNew.cboClient.Text = "CREDIT-PRIVATE"
'        End If
'    Case "HMO"
'            cboClient.AddItem ""
'            cboClient.AddItem "PHIS"
'            frmPatientsNew.cboClient.Text = "PHIS"
'    Case Else
'        frmPatientsNew.cboClient.Text = grdData.Columns("BillingCat")
'        'strTariff = cboClient.Text
'    End Select
'End If

frmPatientsNew.txtEmp.Text = grdData.Columns("empNo")
'frmPatientsNew.txtBr.Text = grdData.Columns("branch")
'frmPatientsNew.txtStatus.Text = grdData.Columns("status")
'frmPatientsNew.txtRel = grdData.Columns("relationToStaff")
frmPatientsNew.cboIntro.Text = grdData.Columns("introducedBy")
frmPatientsNew.cboCard.Text = grdData.Columns("CardType")
frmPatientsNew.cboMStatus.Text = grdData.Columns("Marital")
'frmPatientsNew.txtMem.Text = grdData.Columns("famMembers")
frmPatientsNew.txtBG.Text = grdData.Columns("bloodGroup")
frmPatientsNew.txtGeno.Text = grdData.Columns("genotype")
frmPatientsNew.txtOccu = grdData.Columns("occupation")
frmPatientsNew.cboStatus.Text = grdData.Columns("religion")
frmPatientsNew.txtKin.Text = grdData.Columns("nextOfKin")
frmPatientsNew.txtNOKPhone.Text = grdData.Columns("nokPhone")
frmPatientsNew.txtKinRel.Text = grdData.Columns("relationToKin")
frmPatientsNew.txtKinAddress.Text = grdData.Columns("kinAddress")

If grdData.Columns("fileDuration") <> "" Then
frmPatientsNew.cboFile.Text = grdData.Columns("fileDuration")
End If
If grdData.Columns("Maturity") <> "" Then
    frmPatientsNew.cboMat.Text = grdData.Columns("Maturity")
End If

If grdData.Columns("ref") <> "" Then
    frmPatientsNew.cboRef.Text = grdData.Columns("ref")
Else
    frmPatientsNew.cboRef.Text = "NO"
End If

If grdData.Columns("HmoRef") <> "" Then
    frmPatientsNew.cboRefHmo.Text = grdData.Columns("HmoRef")
Else
    frmPatientsNew.cboRefHmo.Text = "NO"
End If

If grdData.Columns("ExpiryDate") = "" Then
    frmPatientsNew.dtExpDate.Value = ""
Else
    frmPatientsNew.dtExpDate.Value = CDate(grdData.Columns("ExpiryDate"))
End If

If grdData.Columns("newReg") = "YES" Then
    frmPatientsNew.cboNew.Text = "NEW"
Else
    frmPatientsNew.cboNew.Text = "EXISTING"
End If



frmPatientsNew.txtPolicy.Text = grdData.Columns("policyType")

If strApp = "NURSING" Then
    If txtSex.Text = "FEMALE" Then
        cmdANC.Enabled = True
    Else
        cmdANC.Enabled = False
        'MsgBox "Patient Sex has to be female"
        'Exit Sub
    End If
Else
        cmdANC.Enabled = False
End If

If IsDate(grdData.Columns("LastAttndDate")) Then 'Or Trim(grdData.Columns("LastAttndDate")) = "" Then
    LastAttndDate = grdData.Columns("LastAttndDate")
Else
    'do nothing
End If

coyTypeForExistingPat = grdData.Columns("coyType")


'frmPatients.txtFields(11).Text = grdData.Columns("expired")

'If cboCard.Text = "FAMILY" Then
'Label15.Enabled = True
'txtMem.Enabled = True
'Else
'Label15.Enabled = False
'txtMem.Enabled = False
'End If

'dblSaveBF = 0
'Call saveDebt(strPatNo)
'
'lblDebtCap.Caption = "Debt"
'lblDebt.Caption = FormatNumber(dblSaveBF, 2)



Call getPixFromDB

''''''''''''''''''''''''''''''''pix
'Dim strPatPix As String
'Dim strPixNo As String
'Set piX.Picture = Nothing
'strPixNo = Replace(txtSupp.Text, "/", "")
''strPixPath = App.Path & "\Patients\"
'strPatPix = strPixPath & strPixNo & ".JPG"
''MsgBox strPatPix
'If strPatPix <> "" Then
'    piX.Picture = LoadPicture(strPatPix)
'    'MsgBox strPatPix
'End If

Exit Sub
errH:
'MsgBox Err.Description
Resume Next
MsgBox Err.Description
End Sub



Public Sub getPixFromDB()
'On Error GoTo errH

Dim strPatNo2 As String
delPath = ""
strPatNo2 = Replace(strPatNo, "/", "") 'strPatNo is set from editval
delPath = App.path & "\" & strPatNo2 & ".JPG"
'MsgBox delPath

If PixLoc = "FILE" Then
    Dim strPatPix As String
    Dim strPixNo As String
    Set piX.Picture = Nothing
    strPixNo = Replace(txtSupp.Text, "/", "")
    'strPixPath = App.Path & "\Patients\"
    strPatPix = strPixPath & strPixNo & ".JPG"
    'MsgBox strPatPix
    If strPatPix <> "" Then
        piX.Picture = LoadPicture(strPatPix)
        'MsgBox strPatPix
    End If
Else ' DB/others

    Dim rsPix As New ADODB.Recordset
    rsPix.Open "Select patPix from hpatients where pno='" & strPatNo & "'", conStr, adOpenKeyset, adLockOptimistic
       If rsPix Is Nothing Then
            Exit Sub
            piX.Picture = Nothing
        End If
    
    If Not rsPix.EOF Then
        If Not IsNull(rsPix.Fields("PatPix").Value) Then
            Dim msStream As New ADODB.Stream
            msStream.Type = adTypeBinary
            msStream.Open
            msStream.Write rsPix.Fields("PatPix").Value
            msStream.SaveToFile delPath, adSaveCreateOverWrite
            piX.Picture = LoadPicture(delPath)
            Kill (delPath)
        Else
            'Exit Sub
            piX.Picture = Nothing
        End If
    Else
            'Exit Sub
            piX.Picture = Nothing
    End If
End If

'Exit Sub
'errH:
'MsgBox Err.Description

End Sub



Public Sub DeleteAllFiles(ByVal FolderSpec As String)

'Deletes all files in folder specified
'by parameter FolderSpec.  Does not delete
'subfolders or files within subfolders

'Returns True if sucessful, false otherwise

'Requires a reference the Microsoft Scripting Runtime

'EXAMPLE: DeleteAllFiles "C:\Test"

Dim oFs As New FileSystemObject
Dim oFolder As Folder
Dim oFile As File


If oFs.FolderExists(FolderSpec) Then
    Set oFolder = oFs.GetFolder(FolderSpec)
    On Error Resume Next
    For Each oFile In oFolder.Files
        oFile.Delete True 'setting force to true
                        'deletes read-only file
    Next
    'DeleteAllFiles = oFolder.Files.Count = 0
End If

End Sub

Private Sub setPix()
On Error GoTo errH
If Trim(txtPix.Text) = "" Then Exit Sub

Dim strPath As String, strPixNo As String, strPatPix As String

strPath = txtPix.Text
strPixNo = txtSupp.Text 'ok
strPatNo = txtSupp.Text 'ok

Dim PixPath2 As String

strPatNo = ""
strPatNo = txtSupp.Text

'Dim strPatNo2 As String
'strPatNo2 = Replace(strPatNo, "/", "")
PixPath2 = txtPix.Text   'App.Path & "\" & strPatNo2 & ".JPG"
'MsgBox PixPath2

If PixLoc = "FILE" Then
    If Trim(txtPix.Text) = "" Then Exit Sub
    strPath = txtPix.Text
    strPixNo = Replace(Trim(txtSupp.Text), "/", "")
    'strPixPath = App.Path & "\Patients\"
    strPatPix = strPixPath & strPixNo & ".JPG"
    
    If strPath <> "" Then
    'MsgBox strPath
        
        Dim fSys As New FileSystemObject
        
        With fSys
            .CopyFile strPath, strPatPix, True
            piX.Picture = LoadPicture(strPatPix)
            .DeleteFile strPath, True
            strFldPath = Mid(strPath, 1, InStrRev(strPath, "\") - 1) 'for delAllFiles routine to del all pix in the folder
            'MsgBox strFldPath
        End With
        
    End If

Else ' DB/others


        Dim rsPix As New ADODB.Recordset
        rsPix.Open "Select patPix from hpatients where pno='" & strPatNo & "'", conStr, adOpenKeyset, adLockOptimistic
           If rsPix Is Nothing Then
                Exit Sub
                piX.Picture = Nothing
            End If
        
        If Not rsPix.EOF Then
            'If Not IsNull(rsPix.Fields("PatPix").Value) Then
                Dim msStream As New ADODB.Stream
                msStream.Type = adTypeBinary
                msStream.Open
                'MsgBox PixPath2
                msStream.LoadFromFile PixPath2
        
                rsPix.Fields("PatPix").Value = msStream.Read
                rsPix.Update
                'piX.Picture = LoadPicture(sFileName)
        
                Kill (PixPath2)
        
            'End If
        End If



End If

Exit Sub
errH:
If Err.Description = "Permission denied" Then
    MsgBox Err.Description
Else
    MsgBox Err.Description
End If
End Sub

Private Sub getServiceFeeConsulting(coN As Connection) ' now done by consulting module

'If cboClin.Text = "ANTE-NATAL" Or cboClin.Text = "(ANTE-NATAL)" Then Exit Sub 'no payment for ANC GP Consult
'
'If LastAttndDate = "12:00:00 AM" Then
'    'New Patient will pay con fee
'Else
'    If IsDate(LastAttndDate) And coyTypeForExistingPat = "HMO" Then 'for existing patients.LastAttndDate is "12:00:00 AM" for new pat
'        If DateDiff("m", LastAttndDate, sysDate) = 0 Then Exit Sub 'only one payment for HMO GP Consult within a month
'        'MsgBox DateDiff("m", LastAttndDate, sysDate)
'    End If
'End If
'
'
'
''
'Dim rsVal As New Recordset
'With rsVal
'
'        .Open "select *  from billAccum where 1=2", coN, adOpenStatic, adLockOptimistic
'        .AddNew
'        !dtDate = Format(dtSys, "Short Date")
'        !drgNAME = "CONSULTATION"
'        !Price = ConsultAmount
'        !Qty = 1
'        !SubTotal = ConsultAmount
'        !PNo = txtSupp.Text
'        !consultID = strCval
'        !billType = "SERVICE"     'strCard & " REG"
'        !conID = Null
'        !category = "CONSULTATION"
'        !attendedto = 0
'        !isbilled = 0
'        !suppres = 0
'        !Capitated = "NO"
'        !CoyName = strpCode
'        !billTo = strBillTo
'        !revType = Revtype_Consult
'        .Update
'
'
'
'        '.Update
'
'
'End With
'
'Set rsVal = Nothing
'Call Auditrail(m_Username, "Insert Con Fee for " & txtsurNAme.Text & " " & txtfirstNAme.Text, strCval, "", strHostName)

End Sub


Public Sub loadBoxes()
On Error GoTo errH
CanLoad = True 'ok here for autoComplete in cboPat
 Dim rsBL As New Recordset
  With rsBL
cboPat.Clear
cboPat.AddItem ""
'cboPat.AddItem "(PRIVATE) [0001]"

'.Open "select distinct RetainID,ClientName from vwhRetainerShip order by ClientName", conSTR, adOpenForwardOnly, adLockReadOnly
.Open "select distinct RetainID,ClientName from vwhRetainerShip order by ClientName", conStr, adOpenStatic, adLockOptimistic
If Not .EOF Then
    If .RecordCount > 1 Then
        cboPat.AddItem "(PRIVATE) [0001]"
    End If
    Do While Not .EOF
        cboPat.AddItem !cLIENTNAME & " [" & !retainID & "]"
        'cboPat.ItemData(cboPat.NewIndex) = !SNo
    .MoveNext
    Loop
End If

'.Close
'cboClient.Clear
'cboClient.AddItem ""
'  .Open "select clientcatID from billingprice", conSTR, adOpenForwardOnly, adLockReadOnly
'If Not .EOF Then
'Do While Not .EOF
'cboClient.AddItem !clientcatID
'.MoveNext
'Loop
'End If

.Close
cboPurpose.Clear
cboPurpose.AddItem ""
  .Open "select Distinct Purpose from hClinicPurpose order by Purpose", conStr, adOpenForwardOnly, adLockReadOnly
If Not .EOF Then
Do While Not .EOF
cboPurpose.AddItem !Purpose & ""
.MoveNext
Loop
End If



.Close
cboArea.Clear
cboArea.AddItem ""
  .Open "select Distinct AreaName from hPatientArea order by AreaName", conStr, adOpenForwardOnly, adLockReadOnly
If Not .EOF Then
Do While Not .EOF
cboArea.AddItem !AreaName & ""
.MoveNext
Loop
End If


End With




Set rsBL = Nothing


'cboPurpose.AddItem "CONSULTATION"
'cboPurpose.AddItem "REVIEW"
'cboPurpose.AddItem "FOLLOW-UP"
'cboPurpose.AddItem "INJECTION"
'cboPurpose.AddItem "DRESSING"
'cboPurpose.AddItem "REG ONLY"
'cboPurpose.AddItem "EXEC SCREENING"


' Dim rsBLV As New Recordset
'  With rsBLV
'  cboAppr.Clear
'    cboAppr.AddItem ""
'.Open "select distinct empID,fullname from vwUsers where loginRole ='RECORDS' and accountStatus='enabled'", conSTR, adOpenForwardOnly, adLockReadOnly
'If Not .EOF Then
'.MoveFirst
'Do While Not .EOF
'cboAppr.AddItem !fullName & " [" & !EmpID & "]"
'.MoveNext
'Loop
'End If
'End With
'Set rsBLV = Nothing




Dim rsBLc As New Recordset
With rsBLc
    '  cboClinic.Clear
    '  cboClinic.AddItem ""
      cboClin.Clear
      cboClin.AddItem ""
    
      .Open "select distinct ClinicName,ClinicID from clinicTypes where ClinicName not in ('(IN-PATIENT)','IN-PATIENT')  order by clinicName ", conStr, adOpenForwardOnly, adLockReadOnly
    If Not .EOF Then
        .MoveFirst
        Do While Not .EOF
            'cboClinic.AddItem !ClinicID
            cboClin.AddItem !clinicName & ""
            .MoveNext
        Loop
    End If
End With
Set rsBLc = Nothing



Dim rsBLV As New Recordset
cboIntro.Clear
cboIntro.AddItem ""
  With rsBLV
        .Open "select distinct empID,lastname,firstname from employees", conStr, adOpenForwardOnly, adLockReadOnly
    If Not .EOF Then
    .MoveFirst
    Do While Not .EOF
    cboIntro.AddItem !LastName & " " & !FirstName & " [" & !empID & "]"
    .MoveNext
    Loop
    End If
End With
Set rsBLV = Nothing


Exit Sub
errH:
MsgBox Err.Description

End Sub


Private Sub loadPlan()
txtPolicy.Clear
txtPolicy.AddItem ""
    Dim rsBL As New Recordset
      With rsBL
        .Open "select distinct PlanName,PlanID,retainID from vwhmoPlan where retainID='" & strCoyID & "'  order by PlanName", conStr, adOpenForwardOnly, adLockReadOnly
        If Not .EOF Then
            Do While Not .EOF
            txtPolicy.AddItem !planName & " [" & !planID & "]"
            'cboPat.ItemData(cboPat.NewIndex) = !SNo
            .MoveNext
            Loop
        Else
        
            txtPolicy.Clear 'very ok here
            txtPolicy.AddItem ""
            
            txtPolicy.AddItem "NIL"
            txtPolicy.Text = "NIL"
        
        End If
    End With

End Sub

Public Sub getDocWaitListAsync()
On Error GoTo errH

       ' Prevent re-entrancy. This is CRUCIAL for async operations.
        If m_isAsyncOpRunning Then Exit Sub
        
        m_isAsyncOpRunning = True
        
        ' Check if we have an open connection.
        If m_cnW Is Nothing Then
            Set m_cnW = New ADODB.Connection
            'm_cnW.ConnectionString = conStr
        End If
        
        If m_cnW.State <> adStateOpen Then
            ' Connection is not open, open it asynchronously.
            m_cnW.Open conStr, strUserID, strPWD, adAsyncConnect
        Else
            ' Connection is already open, proceed directly to the queries.
            Call RunAsyncQueries
        End If
        
Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub Timer1_Timer()
    '   ' Only run every hour.
    '   If cntDoc >= (intDoc - 1) Then
    '       cntDoc = 0
    '       Call getDocWaitListAsync
    '
    '   Else
    '       cntDoc = cntDoc + 1
    '   End If
End Sub

' -----------------------------------------------------------------------------
'   Event Handlers
' -----------------------------------------------------------------------------

Private Sub m_cnW_ConnectComplete(ByVal pError As ADODB.Error, _
                                 adStatus As ADODB.EventStatusEnum, _
                                 ByVal pConnection As ADODB.Connection)
    
    If adStatus = adStatusErrorsOccurred Then
        ''' Connection failed. Handle the error.
        'MsgBox "Connection to database failed: " & pError.Description, vbCritical
        m_isAsyncOpRunning = False
        Set m_cnW = Nothing ' Release the object
    ElseIf adStatus = adStatusOK Then
        ' Connection was successful! Now, start the recordset operations.
        Call RunAsyncQueries
    End If

End Sub

Private Sub m_rsDocW_FetchComplete(ByVal pError As ADODB.Error, _
                                  adStatus As ADODB.EventStatusEnum, _
                                  ByVal pRecordset As ADODB.Recordset)

    ' Reset the flag now that the operation is complete.
    m_isAsyncOpRunning = False

    If adStatus = adStatusErrorsOccurred Then
        ''' Handle errors.
        'MsgBox "Error fetching referral data: " & pError.Description, vbCritical
    ElseIf adStatus = adStatusOK Then
        ' Process successful results.
        Set grdDoc.DataSource = Nothing
        If Not pRecordset.EOF Then
            flgDoc = True
            Set grdDoc.DataSource = pRecordset ' Use pRecordset, not rsBL
            grdDoc.Columns("Doctor").Width = 4000
            grdDoc.Columns("EmpID").Visible = False
        Else
            flgDoc = False
            Set grdDoc.DataSource = Nothing
        End If
    End If
    
    ' Close the recordset and connection after all operations are complete.
    If Not pRecordset Is Nothing Then pRecordset.Close
    If Not m_rsDocW Is Nothing Then m_rsDocW.Close
    If Not m_cnW Is Nothing Then m_cnW.Close
    Set m_rsDocW = Nothing
    Set m_cnW = Nothing

End Sub

' -----------------------------------------------------------------------------
'   Query Execution
' -----------------------------------------------------------------------------
Private Sub RunAsyncQueries()
    On Error GoTo ErrorHandler
    
    If m_rsDocW Is Nothing Then Set m_rsDocW = New ADODB.Recordset
    
    ' Close the recordset if it's already open.
    If m_rsDocW.State <> adStateClosed Then m_rsDocW.Close

    ' Open the recordset. We can use synchronous options now since we are
    ' processing the results immediately after this line.
    ' If you still want the slight performance benefit of async execution,
    ' you can leave adAsyncExecute, but it's not strictly necessary here.
    m_rsDocW.Open "select distinct Doctor,NumOfPat,ClinicID,[Date],EmpID from vwDocWaitingListGrouped where date='" & sysDate & "' order by Doctor", _
                 m_cnW, adOpenStatic, adLockReadOnly, adAsyncExecute Or adAsyncFetch
                 
    ' The recordset is now fully populated. Process the data immediately.
    Set grdDoc.DataSource = Nothing
    If Not m_rsDocW.EOF Then
        flgDoc = True
        Set grdDoc.DataSource = m_rsDocW
        grdDoc.Columns("Doctor").Width = 4000
        grdDoc.Columns("EmpID").Visible = False
    Else
        flgDoc = False
        Set grdDoc.DataSource = Nothing
    End If
    
     ''' Reset the flag.
    m_isAsyncOpRunning = False
   
    '''' Clean up resources.
    ' If Not m_rsDocW Is Nothing Then m_rsDocW.Close
    ' If Not m_cnW Is Nothing Then m_cnW.Close
    '
    ' ''' Set objects to Nothing after they are closed.
    ' Set m_rsDocW = Nothing
    ' Set m_cnW = Nothing
    
Exit Sub
    
ErrorHandler:
    MsgBox "Error fetching referral data: " & Err.Description, vbCritical
    m_isAsyncOpRunning = False
    If Not m_rsDocW Is Nothing Then m_rsDocW.Close
    If Not m_cnW Is Nothing Then m_cnW.Close
    Set m_rsDocW = Nothing
    Set m_cnW = Nothing
End Sub


' A new sub to handle closing the resources gracefully.
Public Sub CleanupResources()
    On Error Resume Next ' Use error handling for closing.
    
    If Not m_rsDocW Is Nothing Then
        If m_rsDocW.State <> adStateClosed Then m_rsDocW.Close
        Set m_rsDocW = Nothing
    End If
    
    If Not m_cnW Is Nothing Then
        If m_cnW.State <> adStateClosed Then m_cnW.Close
        Set m_cnW = Nothing
    End If
End Sub

Private Sub RunAsyncQueriesZZZ()
    ' The connection is now guaranteed to be open and valid.
    
    If m_rsDocW Is Nothing Then Set m_rsDocW = New ADODB.Recordset
    
    ' Close the recordset if it's already open to ensure
    ' the next .Open call is treated as a new, asynchronous operation.
    If m_rsDocW.State <> adStateClosed Then
        m_rsDocW.Close
    End If
    
    ' The key change: The adAsyncFetch flag is critical.
    m_rsDocW.Open "select distinct Doctor,NumOfPat,ClinicID,[Date],EmpID from vwDocWaitingListGrouped where date='" & sysDate & "' order by Doctor", _
                 m_cnW, adOpenStatic, adLockReadOnly, adAsyncExecute Or adAsyncFetch
                 
    ' Do not set the m_isAsyncOpRunning flag to False here.
    ' It must remain True until FetchComplete fires.
End Sub

Private Sub RunAsyncQueriesYYY()
    ' The connection is now guaranteed to be open and valid.
    
    If m_rsDocW Is Nothing Then Set m_rsDocW = New ADODB.Recordset
    
    ' This check prevents starting a new query while one is already running.
    If m_rsDocW.State <> adStateClosed Then
        Exit Sub
    End If
    
    ' The key change: add adAsyncFetch to the options.
    ' This tells ADO to raise the FetchComplete event.
    m_rsDocW.Open "select distinct Doctor,NumOfPat,ClinicID,[Date],EmpID from vwDocWaitingListGrouped where date='" & sysDate & "' order by Doctor", _
                 m_cnW, adOpenForwardOnly, adLockReadOnly, adAsyncExecute Or adAsyncFetch
                 
    ''' Do not set the m_isAsyncOpRunning flag to False here.
    ' It must remain True until FetchComplete fires.
End Sub


Private Sub RunAsyncQueriesXXX()
    ' The connection is now guaranteed to be open and valid.
    
    If m_rsDocW Is Nothing Then Set m_rsDocW = New ADODB.Recordset
    
    ' This check prevents starting a new query while one is already running.
    If m_rsDocW.State <> adStateClosed Then
        ' Exit if busy; the event handler will handle completion.
        Exit Sub
    End If
    
    m_rsDocW.Open "select distinct Doctor,NumOfPat,ClinicID,[Date],EmpID from vwDocWaitingListGrouped where date='" & sysDate & "' order by Doctor", _
                m_cnW, adOpenForwardOnly, adLockReadOnly, adAsyncExecute
                
    ' Do not set the m_isAsyncOpRunning flag to False here.
    ' It must remain True until FetchComplete fires.
End Sub

''''''''''''''''''''''''''''''''''
'Private Sub Timer1_Timer()
'If cntDoc >= (intDoc - 1) Then 'every hr
'    cntDoc = 0
'
'    ' Prevent re-entrancy. This is CRUCIAL for async operations.
'    If m_isAsyncOpRunning Then Exit Sub
'
'    ''' Call the date/time function. It's a quick, local call, so it's fine here.
'    'Call setDateAndTime
'
'    ''' Check if we have an open connection. If not, open one asynchronously.
'    If m_cnW Is Nothing Then
'        Set m_cnW = New ADODB.Connection
'        m_cnW.ConnectionString = conStr
'    End If
'
'    If m_cnW.State <> adStateOpen Then
'        ' The connection is not open. Open it asynchronously.
'        m_isAsyncOpRunning = True
'        m_cnW.Open conStr, strUserID, strPWD, adAsyncConnect
'    End If
'
'    ''' The connection is already open. Proceed to the recordset operations.
'    ''' We will call a dedicated sub to handle the queries.
'    'Call RunAsyncQueries
'
'    'Set m_rsDocW = Nothing
'    'm_cnW.Close
'    'Set m_cnW = Nothing
'
'
'
'Else
'    cntDoc = cntDoc + 1
'End If
'
''Call getDocWaitList
'End Sub
'
'Private Sub RunAsyncQueries()
'
'    ''' Set the flag to true because we are starting an async operation
'    m_isAsyncOpRunning = True
'
'    ''' First Query: qryhreferal
'    If m_rsDocW Is Nothing Then Set m_rsDocW = New ADODB.Recordset
'    ' Check if rsDoc is busy from a previous call
'    If m_rsDocW.State <> adStateClosed And m_rsDocW.State <> adStateOpen Then
'        ''' It's busy, so we'll just exit and let the existing op finish.
'        m_isAsyncOpRunning = False
'        Exit Sub
'    End If
'
'    If m_rsDocW.State = adStateOpen Then m_rsDocW.Close
'    m_rsDocW.Open "select distinct Doctor,NumOfPat,ClinicID,[Date],EmpID from vwDocWaitingListGrouped where date='" & sysDate & "' order by Doctor", _
'                m_cnW, adOpenForwardOnly, adLockReadOnly, adAsyncExecute
'
'        '''' Second Query: qryhAdmission (if needed)
'        'If Admission_Limit_Exists = "YES" Then
'        '    m_isAdmissionLimitCheckNeeded = True ' Set a flag for the next event handler
'        '    If m_rsEx Is Nothing Then Set m_rsEx = New ADODB.Recordset
'        '    If m_rsEx.State <> adStateClosed And m_rsEx.State <> adStateOpen Then
'        '        ''' It's busy, so we exit. The existing op will finish.
'        '        Exit Sub
'        '    End If
'        '    m_rsEx.Open "select ClientCat,ExtendAdmissionLimitTo from qryhAdmission where getDate()>= IsNull(ExtendAdmissionLimitTo,getDate())", _
'        '                m_cnW, adOpenForwardOnly, adLockReadOnly, adAsyncExecute
'        'Else
'        '    m_isAdmissionLimitCheckNeeded = False ' No need to check
'        'End If
'
'    ''' After initiating the queries, we can release the flag so the timer can fire again.
'    ''' We will handle the completion in the event handlers.
'    m_isAsyncOpRunning = False
'
'End Sub
'
'Private Sub m_cnW_ConnectComplete(ByVal pError As ADODB.Error, _
'                                 adStatus As ADODB.EventStatusEnum, _
'                                 ByVal pConnection As ADODB.Connection)
'
'    m_isConnecting = False ''' Reset the flag
'
'    If adStatus = adStatusErrorsOccurred Then
'        ' Connection failed. Handle the error.
'        'MsgBox "Connection to database failed: " & pError.Description, vbCritical
'        ''' You might want to disable the timer here
'        ''' Timer1.Enabled = False
'        Set m_cnW = Nothing ' Release the object
'
'        ''' Update UI to reflect the error
'        'lblDate.Caption = "---"
'        'lblTime.Caption = "---"
'
'    ElseIf adStatus = adStatusOK Then
'        ' Connection was successful!
'        ' Now that we have a connection, we can re-run the logic
'        ' The next time setDateAndTime is called, it will go to the 'Else' block
'        ' and start the recordset fetch.
'        Call RunAsyncQueries
'    End If
'
'End Sub
'
'Private Sub m_rsDocW_FetchComplete(ByVal pError As ADODB.Error, adStatus As ADODB.EventStatusEnum, ByVal pRecordset As ADODB.Recordset)
'
'    ''' Handle errors
'    If adStatus = adStatusErrorsOccurred Then
'    '''not nece
'        'MsgBox "Error fetching referral data: " & pError.Description, vbCritical
'        tmrNotify.Enabled = False
'        tmrSound.Enabled = False
'        Exit Sub
'    End If
'
'    ''' Process successful results
'    If Not pRecordset.EOF Then
'        flgDoc = True
'        Set grdDoc.DataSource = rsBL
'        grdDoc.Columns("Doctor").Width = 4000
'        grdDoc.Columns("EmpID").Visible = False
'    Else
'        flgDoc = False
'        Set grdDoc.DataSource = Nothing
'    End If
'
'    '''Close the recordset when done.
'    pRecordset.Close
'
'End Sub



Public Sub getDocWaitList()
On Error GoTo errH
Screen.MousePointer = vbHourglass
    Set grdDoc.DataSource = Nothing
    Dim rsBL As New Recordset
    With rsBL
      'cboType.Clear
      'cboType.AddItem " "
    Dim strList As String
    strList = ""
    .CursorLocation = adUseClient
      .Open "select distinct Doctor,NumOfPat,ClinicID,[Date],EmpID from vwDocWaitingListGrouped where date='" & sysDate & "' order by Doctor", conStr, adOpenStatic, adLockOptimistic
    If Not .EOF Then
    Set grdDoc.DataSource = rsBL
    grdDoc.Columns("Doctor").Width = 4000
    grdDoc.Columns("EmpID").Visible = False
    
        'Call DeletePrevAssign
        
        
        ''    strList = strList & "Date" & vbTab & vbTab & "Doctor" & vbTab & vbTab & vbTab & vbTab & vbTab & "NumOfPat" & vbNewLine
        ''    strList = strList & "-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" & vbNewLine
        ''    Do While Not .EOF
        ''        strList = strList & ![Date] & vbTab & vbTab & !Docname & " (" & !RoomNo & ")" & vbTab & vbTab & !NumOfPat & vbNewLine
        ''    .MoveNext
        ''    Loop
        ''    txtDocPat.Text = strList
    Else
        Set grdDoc.DataSource = Nothing
    End If
    End With
    Set rsBL = Nothing
    
    '  Dim rsBL As New Recordset
    '  With rsBL
    '
    '  'cboType.Clear
    '  'cboType.AddItem " "
    'Dim strList As String
    'strList = ""
    '  .Open "select distinct [Date],RoomNo,DocName,NumOfPat from vwDocWaitingListGrouped where date='" & dtDate.Value & "'", conSTR, adOpenForwardOnly, adLockReadOnly
    'If Not .EOF Then
    'strList = strList & "Date" & vbTab & vbTab & "Doctor" & vbTab & vbTab & vbTab & vbTab & vbTab & "NumOfPat" & vbNewLine
    'strList = strList & "-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" & vbNewLine
    'Do While Not .EOF
    '    strList = strList & ![Date] & vbTab & vbTab & !DocName & " (" & !RoomNo & ")" & vbTab & vbTab & !NumOfPat & vbNewLine
    '.MoveNext
    'Loop
    'txtDocPat.Text = strList
    'Else
    'txtDocPat.Text = ""
    'End If
    'End With
    'Set rsBL = Nothing
Screen.MousePointer = vbDefault
Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
End Sub
