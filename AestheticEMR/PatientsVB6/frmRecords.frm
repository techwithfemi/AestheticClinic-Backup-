VERSION 5.00
Object = "{CDE57A40-8B86-11D0-B3C6-00A0C90AEA82}#1.0#0"; "msdatgrd.ocx"
Object = "{86CF1D34-0C5F-11D2-A9FC-0000F8754DA1}#2.0#0"; "mscomct2.ocx"
Begin VB.Form frmRecords 
   BackColor       =   &H00FFC0C0&
   BorderStyle     =   3  'Fixed Dialog
   Caption         =   "ATTENDANCE FOR TODAY :"
   ClientHeight    =   9870
   ClientLeft      =   4095
   ClientTop       =   1185
   ClientWidth     =   12105
   LinkTopic       =   "Form1"
   LockControls    =   -1  'True
   MaxButton       =   0   'False
   MDIChild        =   -1  'True
   MinButton       =   0   'False
   ScaleHeight     =   9870
   ScaleWidth      =   12105
   ShowInTaskbar   =   0   'False
   Begin VB.CheckBox chkAll 
      Caption         =   "Show All"
      Height          =   285
      Left            =   4230
      TabIndex        =   65
      Top             =   2070
      Width           =   1005
   End
   Begin VB.Frame fraSearch 
      Height          =   510
      Left            =   2025
      TabIndex        =   58
      Top             =   675
      Visible         =   0   'False
      Width           =   7305
      Begin VB.TextBox txtName 
         Appearance      =   0  'Flat
         BackColor       =   &H00FFFFC0&
         Height          =   285
         Left            =   780
         TabIndex        =   62
         Top             =   120
         Width           =   2055
      End
      Begin VB.CommandButton cmdSys 
         Caption         =   "Bill No"
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
         Left            =   3450
         TabIndex        =   61
         Top             =   90
         Width           =   945
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
         Height          =   330
         Left            =   5790
         TabIndex        =   60
         Top             =   90
         Visible         =   0   'False
         Width           =   1080
      End
      Begin VB.CommandButton cmdOK 
         Caption         =   "Surname"
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
         Height          =   330
         Left            =   5220
         TabIndex        =   59
         Top             =   90
         Visible         =   0   'False
         Width           =   1395
      End
      Begin VB.Label Label7 
         Alignment       =   1  'Right Justify
         BackStyle       =   0  'Transparent
         Caption         =   "Search"
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
         Left            =   -990
         TabIndex        =   64
         Top             =   165
         Width           =   1680
      End
      Begin VB.Label Label7 
         Alignment       =   2  'Center
         BackStyle       =   0  'Transparent
         Caption         =   "By"
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
         Left            =   2970
         TabIndex        =   63
         Top             =   165
         Width           =   240
      End
   End
   Begin VB.ComboBox cboDoc 
      Enabled         =   0   'False
      Height          =   315
      ItemData        =   "frmRecords.frx":0000
      Left            =   2070
      List            =   "frmRecords.frx":0002
      Sorted          =   -1  'True
      Style           =   2  'Dropdown List
      TabIndex        =   7
      Top             =   4005
      Width           =   3120
   End
   Begin VB.ComboBox cboRefHmo 
      Enabled         =   0   'False
      Height          =   315
      ItemData        =   "frmRecords.frx":0004
      Left            =   2010
      List            =   "frmRecords.frx":0006
      Sorted          =   -1  'True
      Style           =   2  'Dropdown List
      TabIndex        =   55
      Top             =   4830
      Width           =   1185
   End
   Begin VB.ComboBox cboClient 
      Height          =   315
      ItemData        =   "frmRecords.frx":0008
      Left            =   2025
      List            =   "frmRecords.frx":000A
      Style           =   2  'Dropdown List
      TabIndex        =   2
      Top             =   2040
      Width           =   2085
   End
   Begin VB.TextBox txtEmp 
      Height          =   330
      Left            =   2025
      TabIndex        =   4
      Top             =   2790
      Width           =   3300
   End
   Begin VB.ComboBox txtPolicy 
      Height          =   315
      ItemData        =   "frmRecords.frx":000C
      Left            =   2025
      List            =   "frmRecords.frx":000E
      Style           =   2  'Dropdown List
      TabIndex        =   3
      Top             =   2400
      Width           =   3300
   End
   Begin VB.ComboBox cboPat 
      Height          =   315
      Left            =   2055
      Style           =   2  'Dropdown List
      TabIndex        =   1
      Top             =   1620
      Width           =   5655
   End
   Begin VB.ComboBox cboRef 
      Enabled         =   0   'False
      Height          =   315
      ItemData        =   "frmRecords.frx":0010
      Left            =   2040
      List            =   "frmRecords.frx":0012
      Sorted          =   -1  'True
      Style           =   2  'Dropdown List
      TabIndex        =   8
      Top             =   4440
      Width           =   1185
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
      Height          =   375
      Left            =   9720
      TabIndex        =   50
      Top             =   5085
      Width           =   2070
   End
   Begin VB.PictureBox piX2 
      Appearance      =   0  'Flat
      BackColor       =   &H80000005&
      ForeColor       =   &H80000008&
      Height          =   2040
      Left            =   11835
      ScaleHeight     =   2010
      ScaleWidth      =   2325
      TabIndex        =   45
      Top             =   4095
      Visible         =   0   'False
      Width           =   2355
   End
   Begin VB.CheckBox chkCon 
      BackColor       =   &H00E0E0E0&
      Caption         =   "tick this box only if Attendee is getting a card without seeing the Doctor"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   8.25
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   600
      Left            =   5355
      TabIndex        =   30
      Top             =   7020
      Visible         =   0   'False
      Width           =   7305
   End
   Begin VB.CommandButton cmdAdd 
      Caption         =   "- - -"
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
      Left            =   1350
      TabIndex        =   10
      Top             =   5265
      Width           =   1215
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
      Height          =   375
      Left            =   6705
      TabIndex        =   14
      Top             =   5265
      Width           =   1215
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
      Height          =   375
      Left            =   2700
      TabIndex        =   11
      Top             =   5265
      Width           =   1215
   End
   Begin VB.CommandButton cmdDel 
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
      Left            =   5400
      TabIndex        =   13
      Top             =   5265
      Width           =   1215
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
      Height          =   375
      Left            =   4050
      TabIndex        =   12
      Top             =   5265
      Width           =   1215
   End
   Begin VB.ComboBox cboPurpose 
      Enabled         =   0   'False
      Height          =   315
      ItemData        =   "frmRecords.frx":0014
      Left            =   2040
      List            =   "frmRecords.frx":0016
      Sorted          =   -1  'True
      Style           =   2  'Dropdown List
      TabIndex        =   6
      Top             =   3585
      Width           =   2850
   End
   Begin VB.ComboBox cboType 
      Enabled         =   0   'False
      Height          =   315
      ItemData        =   "frmRecords.frx":0018
      Left            =   2040
      List            =   "frmRecords.frx":001A
      Sorted          =   -1  'True
      Style           =   2  'Dropdown List
      TabIndex        =   5
      Top             =   3180
      Width           =   3750
   End
   Begin VB.ComboBox cboVehNo 
      Enabled         =   0   'False
      Height          =   315
      Left            =   2070
      Sorted          =   -1  'True
      Style           =   2  'Dropdown List
      TabIndex        =   0
      Top             =   1245
      Width           =   6945
   End
   Begin VB.TextBox txtRem 
      Height          =   1005
      Left            =   2070
      MultiLine       =   -1  'True
      TabIndex        =   17
      Top             =   7020
      Visible         =   0   'False
      Width           =   3750
   End
   Begin MSComCtl2.DTPicker dtDate 
      Height          =   330
      Left            =   6045
      TabIndex        =   9
      Top             =   2025
      Width           =   1365
      _ExtentX        =   2408
      _ExtentY        =   582
      _Version        =   393216
      Enabled         =   0   'False
      Format          =   150274049
      CurrentDate     =   38611
   End
   Begin MSComCtl2.DTPicker dtNext 
      Height          =   375
      Left            =   1575
      TabIndex        =   16
      Top             =   6795
      Visible         =   0   'False
      Width           =   3840
      _ExtentX        =   6773
      _ExtentY        =   661
      _Version        =   393216
      Enabled         =   0   'False
      CheckBox        =   -1  'True
      Format          =   150274049
      CurrentDate     =   38611
   End
   Begin MSDataGridLib.DataGrid grdData 
      Height          =   4080
      Left            =   270
      TabIndex        =   15
      Top             =   5715
      Width           =   11535
      _ExtentX        =   20346
      _ExtentY        =   7197
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
   Begin VB.CommandButton OKButton 
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
      Left            =   1350
      TabIndex        =   28
      Top             =   5265
      Width           =   1215
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
      Height          =   375
      Left            =   2700
      TabIndex        =   29
      Top             =   5265
      Width           =   1215
   End
   Begin VB.Frame Frame1 
      BackColor       =   &H00FFC0C0&
      Caption         =   "Doctors' waiting List : Double click  an Item on the List to View Details"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   2265
      Left            =   9765
      TabIndex        =   47
      Top             =   585
      Visible         =   0   'False
      Width           =   11400
      Begin MSDataGridLib.DataGrid grdDoc 
         Height          =   1860
         Left            =   90
         TabIndex        =   48
         Top             =   270
         Width           =   11220
         _ExtentX        =   19791
         _ExtentY        =   3281
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
   End
   Begin VB.Timer Timer1 
      Interval        =   30000
      Left            =   45
      Top             =   1320
   End
   Begin VB.Timer tmrBill 
      Enabled         =   0   'False
      Interval        =   500
      Left            =   90
      Top             =   600
   End
   Begin VB.Label Label39 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Doctor to see"
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
      TabIndex        =   57
      Top             =   4050
      Width           =   1995
   End
   Begin VB.Label Label6 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Referal?"
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
      Left            =   600
      TabIndex        =   56
      Top             =   4875
      Width           =   1365
   End
   Begin VB.Label Label26 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Billing Cat (Tariff)"
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
      Left            =   45
      TabIndex        =   54
      Top             =   2085
      Width           =   1905
   End
   Begin VB.Label Label18 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Enrollee No"
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
      Left            =   855
      TabIndex        =   53
      Top             =   2880
      Width           =   1095
   End
   Begin VB.Label Label16 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "HMO Enrollee Plan"
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
      TabIndex        =   52
      Top             =   2445
      Width           =   1830
   End
   Begin VB.Label Label17 
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
      Height          =   195
      Index           =   0
      Left            =   840
      TabIndex        =   51
      Top             =   1665
      Width           =   1140
   End
   Begin VB.Image piX 
      Height          =   1995
      Left            =   9540
      Stretch         =   -1  'True
      Top             =   840
      Width           =   2505
   End
   Begin VB.Label lblFound 
      Alignment       =   1  'Right Justify
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
      Left            =   6090
      TabIndex        =   49
      Top             =   2880
      Width           =   2895
   End
   Begin VB.Label Label19 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "FeeForService?"
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
      TabIndex        =   46
      Top             =   4485
      Width           =   1365
   End
   Begin VB.Label LblOccup 
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
      ForeColor       =   &H00C00000&
      Height          =   240
      Left            =   6255
      TabIndex        =   44
      Top             =   3735
      Width           =   2580
   End
   Begin VB.Label Label12 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Occupation:"
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
      Left            =   4770
      TabIndex        =   43
      Top             =   3690
      Width           =   1365
   End
   Begin VB.Label Label17 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Expiration Date"
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
      Left            =   8100
      TabIndex        =   42
      Top             =   4815
      Width           =   1365
   End
   Begin VB.Label lblExpire 
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
      ForeColor       =   &H00C00000&
      Height          =   240
      Left            =   9585
      TabIndex        =   41
      Top             =   4815
      Width           =   2400
   End
   Begin VB.Label Label16 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Assigned to Doctor"
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
      Left            =   -1485
      TabIndex        =   40
      Top             =   3330
      Visible         =   0   'False
      Width           =   1815
   End
   Begin VB.Label lblPolicy 
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
      ForeColor       =   &H00C00000&
      Height          =   240
      Left            =   6435
      TabIndex        =   39
      Top             =   4050
      Width           =   1545
   End
   Begin VB.Label Label15 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Policy Type:"
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
      Left            =   5220
      TabIndex        =   38
      Top             =   4050
      Width           =   1095
   End
   Begin VB.Label Label14 
      BackStyle       =   0  'Transparent
      Caption         =   "REFERAL"
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
      Height          =   240
      Left            =   10890
      TabIndex        =   37
      Top             =   -1530
      Width           =   1140
   End
   Begin VB.Label lblNo 
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
      ForeColor       =   &H00C00000&
      Height          =   240
      Left            =   9585
      TabIndex        =   35
      Top             =   4095
      Width           =   2400
   End
   Begin VB.Label Label10 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Patient Sys No:"
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
      Left            =   8100
      TabIndex        =   34
      Top             =   4095
      Width           =   1365
   End
   Begin VB.Label Label9 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Card No:"
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
      Left            =   5175
      TabIndex        =   33
      Top             =   4455
      Width           =   960
   End
   Begin VB.Label lblOld 
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
      ForeColor       =   &H00C00000&
      Height          =   240
      Left            =   6255
      TabIndex        =   32
      Top             =   4455
      Width           =   1725
   End
   Begin VB.Label lblClient 
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
      ForeColor       =   &H00C00000&
      Height          =   240
      Left            =   10620
      TabIndex        =   31
      Top             =   5805
      Visible         =   0   'False
      Width           =   2580
   End
   Begin VB.Label Label13 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Consult No:"
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
      Left            =   8100
      TabIndex        =   27
      Top             =   4455
      Width           =   1365
   End
   Begin VB.Label lblBill 
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
      ForeColor       =   &H00C00000&
      Height          =   240
      Left            =   9585
      TabIndex        =   26
      Top             =   4455
      Width           =   2400
   End
   Begin VB.Label Label11 
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
      Left            =   180
      TabIndex        =   25
      Top             =   3630
      Width           =   1815
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Next Appt Date"
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
      Left            =   105
      TabIndex        =   24
      Top             =   6825
      Visible         =   0   'False
      Width           =   1365
   End
   Begin VB.Label Label3 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Clinic/Referal/Appt"
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
      Left            =   135
      TabIndex        =   23
      Top             =   3225
      Width           =   1860
   End
   Begin VB.Label lblDisplay 
      BackStyle       =   0  'Transparent
      Caption         =   "- - - "
      Height          =   240
      Left            =   7155
      TabIndex        =   22
      Top             =   6750
      Visible         =   0   'False
      Width           =   1185
   End
   Begin VB.Label Label4 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Remarks/Comment"
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
      Left            =   405
      TabIndex        =   20
      Top             =   7020
      Visible         =   0   'False
      Width           =   1590
   End
   Begin VB.Label Label2 
      Alignment       =   1  'Right Justify
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
      Height          =   195
      Index           =   0
      Left            =   5460
      TabIndex        =   19
      Top             =   2115
      Width           =   510
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
      Left            =   630
      TabIndex        =   18
      Top             =   1290
      Width           =   1365
   End
   Begin VB.Shape Shape1 
      Height          =   1995
      Left            =   9540
      Top             =   1470
      Width           =   2505
   End
   Begin VB.Label Label8 
      Alignment       =   1  'Right Justify
      BackStyle       =   0  'Transparent
      Caption         =   "Occupation:"
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
      TabIndex        =   36
      Top             =   570
      Visible         =   0   'False
      Width           =   1140
   End
   Begin VB.Label Label5 
      Alignment       =   2  'Center
      BackColor       =   &H00000000&
      Caption         =   "PATIENT ATTENDANCE"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   18
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H0080FFFF&
      Height          =   510
      Left            =   0
      TabIndex        =   21
      Top             =   30
      Width           =   12120
   End
End
Attribute VB_Name = "frmRecords"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
''''
Option Explicit
Dim strpCode As String
Dim strParam As String
Dim flgEdit As Boolean
'Dim strEmpID As String
Dim strEmpDoc As String
Dim iDNo As Long
Dim IDConVal As Long
Dim strIDConVal As String
Dim strRefCon As String
Dim strPath As String, strPixNo As String, strPatPix As String
Const fVal As Integer = 2 ' meant to half the service for follow up
Dim dblPrice As Double
Dim strBilling As String
Dim flgSch As Boolean
Dim strName As String
Dim cVal As Long
Dim strCval As String
Dim strBillCat As String
Dim strNo As String
'Dim searchVal As String

Dim strFldPath As String
Dim strPatNo As String
Dim delPath As String
Dim ClientType As String
Dim strFFS As String
Dim planID As String

Dim planName As String
Dim strCoyID As String
Dim searchVal As String
Dim Client As String
Dim strBillTo As String
Dim PixLoc As String

Dim Doctor As String
Dim DocAssigned As String

Private Sub CancelButton_Click()
Unload Me
End Sub

Private Sub cboAppr_Click()
'If cboAppr.ListIndex = -1 Then Exit Sub
'strEmpID = Mid(cboAppr.Text, InStr(cboAppr.Text, "[") + 1, Len(cboAppr.Text) - (InStr(cboAppr.Text, "[") + 1))

End Sub


Private Sub cboClient_Click()
On Error GoTo errH
    strBillCat = cboClient.Text
    Exit Sub
errH:
    Screen.MousePointer = vbDefault
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

Private Sub cboPat_Click()
Dim intSNo As Integer
On Error GoTo errH
Screen.MousePointer = vbHourglass
If cboPat.ListIndex = -1 Or cboPat.ListIndex = 0 Then Exit Sub
    
strCoyID = Mid(cboPat.Text, InStr(cboPat.Text, "[") + 1, Len(cboPat.Text) - (InStr(cboPat.Text, "[") + 1))
Client = Mid(cboPat.Text, 1, InStr(cboPat.Text, "[") - 2)

'intSNo = cboPat.ItemData(cboPat.ListIndex)
'retrieve val for clientcatID and ffs

ClientType = ""

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


    cboClient.Clear '' ok here
    cboClient.AddItem ""
    
    .Close
    .Open "select distinct clientCatID  from billingPrice where clientType='" & ClientType & "' order by clientCatID", conStr, adOpenForwardOnly, adLockReadOnly
    If Not .EOF Then
        .MoveFirst
        Do While Not .EOF
        cboClient.AddItem !clientCatID & ""
        .MoveNext
        Loop
    Else
        Dim rx As New Recordset
        rx.Open "select distinct clientCatID  from billingPrice order by clientCatID", conStr, adOpenForwardOnly, adLockReadOnly
        If Not rx.EOF Then
            rx.MoveFirst
            Do While Not rx.EOF
            cboClient.AddItem rx!clientCatID & ""
            rx.MoveNext
            Loop
        End If
    End If
End With

txtPolicy.Clear
txtPolicy.AddItem ""
txtPolicy.AddItem "NIL"
    Dim rsBL3 As New Recordset
      With rsBL3
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
    
    txtEmp.Text = ""
    
If strCoyID = strPrivate Or ClientType = "PRIVATE" Then
    cboClient.Text = "PRIVATE"
    txtEmp.Text = "NIL"
    txtPolicy.Text = "NIL"
    CboRef.Text = "YES"
    cboRefHmo.Text = "NO"
        
End If

Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Private Sub cboPurpose_Click()
On Error GoTo errH
'If cboPurpose.Text = "" Or flgEdit = True Then Exit Sub
If cboPurpose.Text = "" Then Exit Sub
strPurpose = ""
strPurpose = ""
ScreeningAmount = 0
ConsultAmount = 0
Dim rsBLV As New Recordset
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
    rsBLV.Open "select Price as Amount   from hserviceNHIS where SNo=" & PVT_CONFEE_SNO, conStr, adOpenForwardOnly, adLockReadOnly
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
    If cboType.Text = "" Then
        MsgBox "Specify Clinic"
        Exit Sub
    End If
    
    'frmClinicDates.Hide
    'frmClinicDates.Show vbModal

End If

Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cboRef_Click()
'If cboRef.ListIndex = 0 Then Exit Sub
'strEmpDoc = Mid(cboRef.Text, InStr(cboRef.Text, "[") + 1, Len(cboRef.Text) - (InStr(cboRef.Text, "[") + 1))
End Sub



Private Sub cboType_Click()
On Error GoTo errH
If cboType.ListIndex = -1 Or cboType.ListIndex = 0 Then Exit Sub

'If Len(lblExpire.Caption) <= 0 And strCoyID = strPrivate Then
'    MsgBox " Please Specify Expiration Date for this Private Patient (GO TO REGISTRATION PAGE)"
'    cboType.ListIndex = -1
'    Exit Sub
'End If


'If Len(lblExpire.Caption) > 0 And strCoyID = strPrivate Then
'    'MsgBox DateAdd("YYYY", 1, dtExpDate.Value)
'    If CDate(lblExpire.Caption) <= sysDate Then
'        cboType.ListIndex = -1
'        MsgBox "Patient's Card has Expired"
'
'        Dim cmdExp As New Command
'        Dim strDel As String
'        Dim sSQlx As String
'        sSQlx = "update  hpatients set " & _
'        "expired =1 where pno='" & lblNo.Caption & "'"
'
'        '"oldPno='" & Trim(txtNewCard.Text) & "'," & _
'
'
'        cmdExp.ActiveConnection = conStr
'        cmdExp.CommandText = sSQlx
'        cmdExp.CommandType = adCmdText
'        cmdExp.Execute
'        cmdRenew.Enabled = True
'        Exit Sub
'    Else
'        cmdRenew.Enabled = False
'    End If
'End If

'If cboType.Text = "" Or cboType.Text = "(BOOK APPT)" Then Exit Sub
'        Dim strClinicX As String
'        strClinicX = cboType.Text
'        Dim rsBLV As New Recordset
'          With rsBLV
'          .Open "select distinct Fullname,Clinic from vwhPatientClinicValid where pNo='" & strpCode & "' and clinic='" & strClinicX & "'", conSTR, adOpenForwardOnly, adLockReadOnly
'            If .EOF Then
'                MsgBox "Sorry! You have not Registered for " & strClinicX & " Clinic"
'                Call clearFields
'                Call fillGrid
'                Exit Sub
'
'            End If
'        End With
Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cboVehNo_Click()
If cboVehNo.Text = "" Then Exit Sub

On Error GoTo errH
strNo = ""
Dim isAppt As Boolean
strpCode = ""
dblBF = 0
strpCode = Mid(cboVehNo.Text, InStr(cboVehNo.Text, "[") + 1, Len(cboVehNo.Text) - (InStr(cboVehNo.Text, "[") + 1))

'dblSaveBF = 0 'not nece here
'Call saveDebt(strpCode)   ' not nece anymore

Dim rsBLV As New Recordset
  With rsBLV
  .Open "select CoyName,billingCat,Client,occupation,pno,oldpNo,policytype,ref,HMORef,expirydate from vwhpatients where pNo='" & strpCode & "'", conStr, adOpenForwardOnly, adLockReadOnly
If Not .EOF Then
strCoyID = ""
strCoyID = rsBLV!CoyName & ""
strNo = rsBLV!PNo & ""
lblNo.Caption = strNo
lblOld.Caption = rsBLV!oldPno & ""
LblOccup.Caption = rsBLV!occupation & ""
lblPolicy.Caption = rsBLV!policyType & ""
lblExpire.Caption = IIf(IsNull(rsBLV!expirydate), "", rsBLV!expirydate)

strBilling = !billingCat & ""
    strBillCat = strBilling
    'cboClient.Text = strBillCat
    
 Dim rsBill As New Recordset ' ok as last item
    rsBill.Open "Select retainCode ,retainName from hretainership where retainID='" & strCoyID & "'", conStr, adOpenForwardOnly, adLockOptimistic
    If Not rsBill.EOF Then
        strBillTo = rsBill!retainCode
    Else
        strBillTo = ""
        MsgBox "BillTo Value Cannot be Empty!!! Check your Client Information"
        Exit Sub
    End If
    


    'cboRef.Text = IIf(IsNull(rsBLV!ref), "NO", rsBLV!ref)
    'cboRefHmo.Text = IIf(IsNull(rsBLV!HmoRef), "NO", rsBLV!HmoRef)

Else
    tmrBill.Enabled = False
    cboClient.Text = ""
    lblNo.Caption = ""
    lblOld.Caption = ""
    LblOccup.Caption = ""
    lblPolicy.Caption = ""
    lblExpire.Caption = ""
    cboRefHmo.ListIndex = -1
    CboRef.ListIndex = -1
End If
End With
Set rsBLV = Nothing


'Call AssignDoc

''''''''''''''''''''''''''''''''''''''
lblFound.Caption = "***"
Label5.Caption = Mid(cboVehNo.Text, 1, InStr(cboVehNo.Text, "[") - 1)
strRefCon = ""

'If flgEdit = False Then
'    flgDup = False
'    flgDup = isDuplicate(strNo)
'    If flgDup = True Then
'        If flgDup2 = True Then 'for Admissiom patient
'            MsgBox "Patient Still On Admission!! Discharge Patient Before Attendance can be Taken"
'        Else
'            MsgBox "Patient Attendance already Taken! Duplicate not allowed!! Proceed to see Nurse/Doctor"
'        End If
'
'        SetButtons (True)
'        ClearFields
'        Exit Sub
'    Else
'        'do nothing
'    End If
'
'            '''''''''''''generate consult ID''''''''''''''''''''''''''''''''
'Dim coN As New Connection
'coN.ConnectionString = conStr
'coN.Open
'    'Call genConID(cOn)
'    'Call getCorrectConID(coN)
'    'Call genIDNo 'OLD AND NO MORE insIDNo
'End If

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



If strCoyID = "" Then
    MsgBox "Registration Incomplete! Company Name Required!! Go to Registration Page"
    Call clearFields
End If





Call getPixFromDB(strNo)


'Dim strPath As String, strPixNo As String, strPatPix As String
'strPath = txtPix.Text
'Dim strPatPix As String
'Dim strPixNo As String
'Set piX.Picture = Nothing
'strPixNo = Replace(lblNo.Caption, "/", "")
''strPixPath = App.Path & "\Patients\"
'strPatPix = strPixPath & strPixNo & ".JPG"
'MdiSapid.SbrSapid.Panels(1).Text = strPatPix
''MsgBox strPatPix
'If strPatPix <> "" Then
'    piX.Picture = LoadPicture(strPatPix)
'End If

'strPixNo = Replace(strpCode, "/", "")
'
'strPatPix = strPixPath & strPixNo & ".JPG"
'
'piX.Picture = LoadPicture(strPatPix)






'Dim rsBLxx As New Recordset
'Dim StrDocX As String
'With rsBLxx
'  .Open "select distinct top 1 DocName,empID,RoomNo  from vwDocMinNumOfPat", conSTR, adOpenForwardOnly, adLockReadOnly
'    If Not .EOF Then
'        StrDocX = !DocName & " @ " & !RoomNo & " [" & !EmpID & "]"
'        cboRef.Text = StrDocX
'    Else
'        cboRef.ListIndex = 0
'    End If
'End With

'Call isForAppt
        'cboType.Text = "GENERAL"
'        'cbopurpose.Text = "CONSULTATION"
'
'
'If Len(lblExpire.Caption) > 0 And strCoyID = strPrivate Then
'    If CDate(lblExpire.Caption) <= sysDate Then
'        MsgBox "Patient's Card has Expired"
'
'        Dim cmdExp As New Command
'        Dim strDel As String
'        Dim sSQlx As String
'        sSQlx = "update  hpatients set " & _
'        "expired =1 where pno='" & lblNo.Caption & "'"
'
'        '"oldPno='" & Trim(txtNewCard.Text) & "'," & _
'
'
'        cmdExp.ActiveConnection = conStr
'        cmdExp.CommandText = sSQlx
'        cmdExp.CommandType = adCmdText
'        cmdExp.Execute
'        cmdRenew.Enabled = True
'        Exit Sub
'    Else
'        cmdRenew.Enabled = False
'    End If
'End If


Exit Sub
errH:
MsgBox Err.Description
If Err.number = 53 Then
    piX.Picture = LoadPicture("")
    'MsgBox "No picture for this Employee"
    Resume Next
ElseIf Err.number = 76 Then
    piX.Picture = LoadPicture("")
    'MsgBox "No picture for this Patient"
    Resume Next

Else
    Resume Next
    'MsgBox Err.Number & ": " & Err.Description, vbInformation, Err.Source
End If

End Sub


Public Sub AssignDoc()
'On Error GoTo errH
'  Dim rsBL As New Recordset
'  With rsBL
'  .Open "select DocName,Date from vwDocAssign where date='" & Date & "'", conSTR, adOpenForwardOnly, adLockReadOnly
'  If .EOF Then
'    MsgBox "Please Assign Doctors to Consulting Rooms"
'    frmSchedDoctor.Hide
'    frmSchedDoctor.Show
'    Call clearFields
'    'cboVehNo.ListIndex = 0
'    Exit Sub
'End If
'End With
'Exit Sub
'errH:
'MsgBox Err.Description

End Sub



Public Sub isForAppt()
'Dim rsB As New Recordset
'rsB.Open "select ApptDate,pno,clinictype,remarks,consultID from qryhAppt where pno='" & strpCode & "' and attendedto =0  ", conSTR, adOpenStatic, adLockOptimistic
' With rsB
'If Not .EOF Then
'    MsgBox " This Patient has " & !clinicType & " Appointment Today"
'        'strFollow = "FOLLOW-UP"
'        cboType.Text = !clinicType & ""
''        If !Remarks <> "FOLLOW-UP" Or !Remarks <> "REVIEW" Then
''            cboPurpose.Text = "CONSULTATION"
''            Else
''            cboPurpose.Text = !Remarks
''        End If
'        strRefCon = !consultID
'
'Else
'        'strFollow = ""
'        'cboType.ListIndex = -1
'        'cbopurpose.ListIndex = -1
'        strRefCon = ""
'End If
'End With
'Set rsB = Nothing
End Sub




Private Sub chkAll_Click()
If chkAll.Value = vbChecked Then
    Call loadClientCat
    End If
End Sub

Private Sub cmdAdd_Click()
On Error GoTo errH
'dblSaveBF = 0 'very impt 'to prevent it spilling to subsequent newly reg pat
'enableFields True
'SetButtons (False)
''cboType.Text = "OUT-PATIENT"
'    flgSch = False
' flgEdit = False
' Call fillGrid
 
 Exit Sub
 
errH:
 MsgBox Err.Description
 
End Sub

Private Sub cmdCancel_Click()
'txtName.SetFocus

flgEdit = False

tmrBill.Enabled = False
Label5.BackColor = vbBlack

enableFields False
SetButtons (True)
'cboVehNo.Enabled = True

Call clearFields

Call fillGrid

    flgSch = False
 flgEdit = False


End Sub

Private Sub cmdDel_Click()
  Dim cmd As New Command
  Dim strDel As String
  Dim sSQlx As String
  Dim intOK As Integer
On Error GoTo errH
 intOK = MsgBox("Are you sure to Delete Record", vbYesNo, "Delete")
 If intOK = vbYes Then
  strDel = grdData.Columns("recID")
 sSQlx = "delete from hrecords where recID = '" & strDel & "'"
    cmd.ActiveConnection = conStr
    cmd.CommandText = sSQlx
    cmd.CommandType = adCmdText
    cmd.Execute
        Call Auditrail(m_Username, "Delete Patient: " & grdData.Columns("fullName").Text, lblBill.Caption, "", strHostName)
MsgBox " Record successfully deleted "
    flgSch = False
 flgEdit = False

   Call fillGrid

End If
Exit Sub
errH:
MsgBox Err.Description

End Sub

Private Sub cmdEdit_Click()

''On Error GoTo errH
On Error Resume Next

If grdData.Columns.Count <= 2 Then
    MsgBox "No record in the grid to Edit", vbInformation, "Record Needed"
    Exit Sub
End If


Dim consultID As String
consultID = grdData.Columns("ConsultID").Text
If Len(consultID) = 13 And Mid(consultID, 4, 3) <> "OFL" Then 'split bill
    MsgBox "This ConsultID Cannot be edited! Split Bill", vbInformation, "Split Bill Not Editable"
    Exit Sub
End If


'If flgEdit = True Then
'    MsgBox "Already in Edit Mode Click 'Cancel Button' to cancel the currrent Editing"
'    Exit Sub
'Else
'
'End If

'If flgSch = True Then
'    'MsgBox "Search Result cannot be Edited, Only Attendance"
'    cboVehNo.Clear
'    cboVehNo.AddItem grdData.Columns("fullname") & " [" & grdData.Columns("pno") & "]"
'    cboVehNo.Text = grdData.Columns("fullname") & " [" & grdData.Columns("pno") & "]"
'    flgSch = False
'    Call cmdAdd_Click
'    'Call fillGrid 'already called in cmdAdd_Click
'    'Call cmdCancel_Click
'    Exit Sub
'End If



cboVehNo.Clear
cboVehNo.AddItem grdData.Columns("fullname") & " [" & grdData.Columns("pno") & "]"
cboVehNo.Text = grdData.Columns("fullname") & " [" & grdData.Columns("pno") & "]"
    
flgEdit = True
SetButtons (False)
enableFields True


strName = ""

'cboVehNo.Enabled = False

'    Select Case cbopurpose.Text
'        Case "FEE-PAYING"
'            strTariff = "PRIVATE"
'        Case "CREDIT-PRIVATE"
'            strTariff = "PRIVATE"
'        Case "PHIS"
'            strTariff = "HMO"
'        Case Else
'             strTariff = cbopurpose.Text
'    End Select



    strParam = grdData.Columns("recID")
    strName = grdData.Columns("fullname") & " [" & grdData.Columns("pno") & "]"
    ''coyName must come b4 clientcat
    '' very nece
    cboPat.Text = grdData.Columns("Client") & " [" & grdData.Columns("RetainID") & "]"
    cboPurpose.Text = grdData.Columns("Remarks")
    'lblRef.Caption = grdData.Columns("Referal")
    
    'cboAppr.Text = grdData.Columns("empID")
    cboType.Text = grdData.Columns("Clinictype")
    lblBill.Caption = grdData.Columns("consultID")
    
    'If grdData.Columns("BillingCat") <> "" Then
    '    strBillCat = grdData.Columns("BillingCat")
    '    Select Case strBillCat     'grdData.Columns("BillingCat")  'cbopurpose.Text
    '    Case "PRIVATE"
    '        If grdData.Columns("Client") = "FEE-PAYING" Or grdData.Columns("Client") = "(FEE-PAYING)" Then
    '            txtBilling.Text = "FEE-PAYING"
    '        Else 'credit-Private
    '            txtBilling.Text = "CREDIT-PRIVATE"
    '        End If
    '    Case "HMO"
    '            txtBilling.Text = "PHIS"
    '    Case Else
    '        txtBilling.Text = strBillCat
    '        'strTariff = cbopurpose.Text
    '    End Select
    'End If


    
    'txtBilling.Text = grdData.Columns("BillingCat")
    '!nextApptDate = grdData.Columns("nextApptDate") 'dtNext.Value
    CboRef.Text = grdData.Columns("FeeForService")
    cboRefHmo.Text = grdData.Columns("Referal")
   If grdData.Columns("Date") = "" Then
    dtDate.Value = ""
    Else
    dtDate.Value = grdData.Columns("Date")
    End If
    
'Call cmdSys_Click
txtPolicy.Text = grdData.Columns("policyType") 'must come after cbopat
txtEmp.Text = grdData.Columns("EnrolleNo") 'nece after policyType
'cboVehNo.Text = strName 'ok here
'txtName.Text = grdData.Columns("PatNo")
cboDoc.Text = grdData.Columns("Doctor") & " [" & grdData.Columns("DocAssigned") & "]"
cboClient.Text = grdData.Columns("BillingCat")

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

flgSch = True

searchVal = "Name"
getPatInfo

End Sub

Private Sub cmdCard_Click()
If txtName = "" Then
MsgBox "To locate a patient, Enter Card No and click 'Card No' Button"
'Set grdData.DataSource = Nothing
Exit Sub
End If

flgSch = True
searchVal = "CardNo"
getPatInfo

End Sub


Private Sub cmdSys_Click()
If Trim(txtName.Text) = "" Then
    MsgBox "To locate a patient, Enter Patient Bill No and click 'Bill No' Button"
    'Set grdData.DataSource = Nothing
    txtName.SetFocus
    Exit Sub
End If

flgSch = True
searchVal = "SysNo"
Call getPatInfo

End Sub

Public Sub getPatInfo()
On Error GoTo errH
Screen.MousePointer = vbHourglass


Dim strNameVal As String
strNameVal = Trim(txtName.Text)

Call clearFields '' ok here after strNameVal

txtName.Text = strNameVal '' ok here after clearFields

Dim rsVal As New Recordset
Set grdData.DataSource = Nothing
With rsVal
.CursorLocation = adUseClient
.Open "select distinct Fullname, RecDate as Date, [Time],CoyName as Client, ClientCat as BillingCat, ClinicType, recID, Surname, pNO as PatNo,PNo,Doctor,DocAssigned,empID, Remarks,attendedTo,ConsultID, Referal as FeeForService,HMORef as Referal,RetainID,PolicyType,EmpNo as EnrolleNo from qryhvisitsForSearch where consultID = '" & strNameVal & "'", conStr, adOpenStatic, adLockOptimistic
'.Open "select distinct  Fullname, RecDate as Date, [Time], ClientCat, ClinicType, recID, Surname, DocName, pNO as PatNo, empID, Remarks,attendedTo,ConsultID, Referal from qryhVisitsForToday ORDER BY recID desc", conSTR, adOpenStatic, adLockOptimistic
If Not .EOF Then
Set grdData.DataSource = rsVal
grdData.Columns("recID").Visible = False
grdData.Columns("attendedTo").Visible = False
'grdData.Columns("referal").Visible = False

grdData.Columns("Surname").Visible = False

    grdData.Columns("empID").Visible = False
Me.Caption = "ATTENDANCE FOR TODAY:" & CStr(.RecordCount)
Else
Me.Caption = "ATTENDANCE BY BILL No:"
End If

'Call getDocWaitList

End With
Set rsVal = Nothing

'Dim strNameVal As String
'Dim rsVal As New Recordset
'strNameVal = Trim(txtName.Text)
'Dim sSQL As String
''ssQL = "select pno as FileNo,oldpno as [Old FileNo],psurname as Surname,pfirstname as Firstname ,homeAddress from hpatients where psurname like '" & strNameVal & "%'"
'
'Select Case searchVal
'Case "Name"
'    sSQL = "select  * from vwhpatients where fullname like '" & Replace(strNameVal, "'", "''") & "%'   order by fullname"
'
'Case "CardNo"
'    sSQL = "select  * from vwhpatients where oldPno = '" & Replace(strNameVal, "'", "''") & "'  order by fullname"
'
'Case "SysNo"
'    If InStr(strNameVal, "/") > 0 Then
'        strNameVal = strNameVal
'    Else
'        strNameVal = strNameVal ' strip off zeros ' to allow only sig figures ' flexibility
'        strNameVal = HNo & "/" & Right("000000000" & strNameVal, 9)
'        txtName.Text = strNameVal
'    End If
'    sSQL = "select  * from vwhpatients where Pno = '" & strNameVal & "'   order by fullname"
'End Select
'
' If flgEdit = True Then
'
'        With rsVal
'            '.CursorLocation = adUseClient
'            .Open sSQL, conStr, adOpenStatic, adLockOptimistic
'            'MsgBox ssQL
'
'
'            cboVehNo.Clear
'            cboVehNo.AddItem ""
'            If Not .EOF Then
'                .MoveFirst
'                Do While Not .EOF
'                cboVehNo.AddItem !fullName & " [" & !PNo & "]"
'                .MoveNext
'                Loop
'            End If
'        End With
'Else
'
'        With rsVal
'        .CursorLocation = adUseClient
'        .Open sSQL, conStr, adOpenStatic, adLockOptimistic
'        'MsgBox ssQL
'
'        Set grdData.DataSource = Nothing
'        cboVehNo.Clear
'        cboVehNo.AddItem ""
'        If Not .EOF Then
'            .MoveFirst
'            Do While Not .EOF
'            cboVehNo.AddItem !fullName & " [" & !PNo & "]"
'            .MoveNext
'            Loop
'
'            Set grdData.DataSource = rsVal
'            grdData.Columns("expired").Visible = False
'            grdData.Columns("coyname").Visible = False
'            grdData.Columns("fullname").Visible = False
'            lblFound.Caption = .RecordCount & " Records Found"
'            Else
'            Set grdData.DataSource = Nothing
'            lblFound.Caption = "0 Records Found"
'        End If
'        End With
'
'    'Call cmdAdd_Click 'remarked to allow for search without update of attendance
'End If
'
''Set rsVal = Nothing
Screen.MousePointer = vbDefault
Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
End Sub



Private Sub cmdOK2_Click()
On Error GoTo errH
If txtName = "" Then
MsgBox "To locate a patient, Enter few Letters and click OK Button"
'Set grdData.DataSource = Nothing
Exit Sub
End If

flgSch = True


Dim strNameVal As String
Dim rsVal As New Recordset
strNameVal = txtName.Text
Dim sSQL As String
'ssQL = "select pno as FileNo,oldpno as [Old FileNo],psurname as Surname,pfirstname as Firstname ,homeAddress from hpatients where psurname like '" & strNameVal & "%'"
sSQL = "select  * from vwhpatients where fullname like '" & Replace(strNameVal, "'", "''") & "%'"

 If flgEdit = True Then
 
        With rsVal
            '.CursorLocation = adUseClient
            .Open sSQL, conStr, adOpenStatic, adLockOptimistic
            'MsgBox ssQL
            
            
            cboVehNo.Clear
            cboVehNo.AddItem ""
            If Not .EOF Then
                .MoveFirst
                Do While Not .EOF
                cboVehNo.AddItem !fullName & " [" & !PNo & "]"
                .MoveNext
                Loop
            End If
        End With
Else

        With rsVal
        .CursorLocation = adUseClient
        .Open sSQL, conStr, adOpenStatic, adLockOptimistic
        'MsgBox ssQL
        
        Set grdData.DataSource = Nothing
        cboVehNo.Clear
        cboVehNo.AddItem ""
        If Not .EOF Then
            .MoveFirst
            Do While Not .EOF
            cboVehNo.AddItem !fullName & " [" & !PNo & "]"
            .MoveNext
            Loop
            
            Set grdData.DataSource = rsVal
            grdData.Columns("expired").Visible = False
            grdData.Columns("coyname").Visible = False
            grdData.Columns("fullname").Visible = False
            lblFound.Caption = .RecordCount & " Records Found"
            Else
            Set grdData.DataSource = Nothing
            lblFound.Caption = "0 Records Found"
        End If
        End With

    'Call cmdAdd_Click 'remarked to allow for search without update of attendance
End If

'Set rsVal = Nothing
Exit Sub
errH:
MsgBox Err.Description
End Sub

Private Sub cmdRefresh_Click()
Form_Load
Call clearFields
Label5.BackColor = vbBlack
    flgSch = False
 flgEdit = False
'txtName.SetFocus

End Sub



Private Sub cmdRenew_Click()
'lblNo.Caption = rsBLV!pno & ""
'lblOld.Caption = rsBLV!oldpno & ""
strFullname = Label5.Caption
strPatNos = lblNo.Caption
'cmdRenew.Enabled = False
strForm = "RECORDS"
frmRenew.Hide
frmRenew.Show vbModal
End Sub

Private Sub Form_Load()
On Error GoTo errH
Screen.MousePointer = vbHourglass

If m_Username = "femi" Then
    fraSearch.Visible = True
    'txtName.SetFocus
Else
    fraSearch.Visible = False
End If

flgCardRenew = False

'dtNext.Value = sysDate
'dtNext.Value = ""
dtDate.Value = sysDate

CboRef.Clear
CboRef.AddItem ""
CboRef.AddItem "YES"
CboRef.AddItem "NO"

cboRefHmo.Clear
cboRefHmo.AddItem ""
cboRefHmo.AddItem "YES"
cboRefHmo.AddItem "NO"

'cbopurpose.Clear
'cbopurpose.AddItem ""
'cbopurpose.AddItem "CONSULTATION"
'cbopurpose.AddItem "REVIEW"
'cbopurpose.AddItem "FOLLOW-UP"
'cbopurpose.AddItem "INJECTION"
'cbopurpose.AddItem "DRESSING"
'cbopurpose.AddItem "REG ONLY"
'cbopurpose.AddItem "EXEC SCREENING"

Dim rsBLxx As New Recordset
With rsBLxx
cboPurpose.Clear
cboPurpose.AddItem ""
  .Open "select Distinct Purpose from hClinicPurpose order by Purpose", conStr, adOpenForwardOnly, adLockReadOnly
If Not .EOF Then
Do While Not .EOF
cboPurpose.AddItem !Purpose & ""
.MoveNext
Loop
End If

End With
'
Set rsBLxx = Nothing


  Dim rsBL As New Recordset
  With rsBL
  cboType.Clear
  cboType.AddItem ""
  'cboType.AddItem "(BOOK APPT)"
  '.Open "select distinct ClinicName,ClinicID from clinicTypes where ClinicName not in ('(IN-PATIENT)','IN-PATIENT')  order by clinicName ", conStr, adOpenForwardOnly, adLockReadOnly
  .Open "select distinct ClinicName,ClinicID from clinicTypes order by clinicName ", conStr, adOpenForwardOnly, adLockReadOnly
If Not .EOF Then
.MoveFirst
Do While Not .EOF
cboType.AddItem !clinicName & ""
.MoveNext
Loop
End If
End With
Set rsBL = Nothing

  
 Dim rsBL2 As New Recordset
  With rsBL2
cboPat.Clear
cboPat.AddItem ""
  .Open "select distinct RetainID,ClientName from vwhRetainerShip order by ClientName", conStr, adOpenForwardOnly, adLockReadOnly
If Not .EOF Then
Do While Not .EOF
cboPat.AddItem !cLIENTNAME & " [" & !retainID & "]"
'cboPat.ItemData(cboPat.NewIndex) = !SNo
.MoveNext
Loop
End If
    
    cboDoc.Clear
    cboDoc.AddItem ""
    If .State = adStateOpen Then .Close
    .Open "select distinct empID,fullname from vwUsers where loginRole ='CONSULTING' and AccountStatus='ENABLED' order by Fullname", conStr, adOpenForwardOnly, adLockReadOnly
    If Not .EOF Then
        .MoveFirst
        Do While Not .EOF
            cboDoc.AddItem !fullName & " [" & !empID & "]"
            .MoveNext
        Loop
    End If
End With

    cboClient.Clear
    cboClient.AddItem ""
    Dim rx As New Recordset
    rx.Open "select distinct clientCatID  from billingPrice order by clientCatID", conStr, adOpenForwardOnly, adLockReadOnly
    If Not rx.EOF Then
        rx.MoveFirst
        Do While Not rx.EOF
        cboClient.AddItem rx!clientCatID & ""
        rx.MoveNext
        Loop
    End If

Screen.MousePointer = vbDefault


Call fillGrid

Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
End Sub

Private Sub Form_Resize()
Me.Left = (Screen.Width - Me.Width) \ 2
Me.Top = 0

End Sub

Private Sub Form_Unload(Cancel As Integer)
    flgEdit = False

End Sub

Private Sub grdData_dblClick()
Call cmdEdit_Click
End Sub



Private Sub grdDoc_DblClick()
' 'gDate = Null
''gDoc = ""
'gDate = CDate(grdDoc.Columns("Date").Text)
'gDoc = grdDoc.Columns("DocName").Text
'
'''frmDocWait.Hide
''frmDocWait.Show vbModal

End Sub

Private Sub OKButton_Click()
tmrBill.Enabled = False
Label5.BackColor = vbBlack

'If Len(Trim(lblBill.Caption)) <> 9 Then
'MsgBox " Consultation ID is needed for this Patient, select Patient Name OR see the Administrator"
'Exit Sub
'End If


'If IsNull(dtDate.Value) Then ' No Need 'not used
'MsgBox " please specify Attendance date"
'dtDate.SetFocus
'Exit Sub
'End If


If Trim(cboVehNo.Text) = "" Then
MsgBox " please specify Name of Patient"
cboVehNo.SetFocus
Exit Sub
End If

If cboClient.Text = "" Or cboClient.Text = "SERVICES" Then
    MsgBox " Billing Category cannot be empty! Invalid Client Tariff or Billing Category"
    cboClient.SetFocus
    Exit Sub
End If

If cboType.Text = "" Then
MsgBox " please specify clinic for Patient"
cboType.SetFocus
Exit Sub
End If

If Trim(cboPurpose.Text) = "" Then
MsgBox " please specify Purpose of Visit for Patient"
cboPurpose.SetFocus
Exit Sub
End If

If strPurpose = "" Then
    MsgBox "Specify Purpose of Attendance"
    'SSTab1.Tab = 1
    cboPurpose.SetFocus
    'cboRef.Text = "NO"
    Exit Sub
End If


'If Enforce_Assign_To_Doctor_In_Attendance = "YES" Then
    If cboPurpose.Text = "(CONSULTATION)" And cboDoc.Text = "" Then
        MsgBox "Assign a Doctor to this Patient", vbCritical
        'SSTab1.Tab = 1
        cboDoc.SetFocus
        Exit Sub
    Else
        'cboDoc.ListIndex = -1
    End If
'End If


If cboClient.Text = "NHIS" Or cboClient.Text = "HMO" Or cboClient.Text = "PHIS" Then
    'If Trim(txtPolicy.Text) = "" Then
    '    MsgBox "HMO Enrolle Plan Needed"
    '    txtPolicy.SetFocus
    '    Exit Sub
    'End If
    
    If Trim(txtEmp.Text) = "" Then
        MsgBox "HMO Enrolle No Needed OR enter NIL if No Enrolle Num "
        txtEmp.SetFocus
        Exit Sub
    End If

    If planID = "" Then
        'planID = Trim(txtPolicy.Text)
        'MsgBox "Select HMO Enrolle Plan"
        'txtPolicy.SetFocus
        'Exit Sub
    End If
    
End If

'If cboAppr.Text = "" Then
'MsgBox " please specify Name of record Officer"
'cboAppr.SetFocus
'Exit Sub
'End If


If CboRef.ListIndex = 0 Then
    MsgBox " please specify Whether Patient is FeeForService"
    CboRef.SetFocus
    Exit Sub
End If


If cboRefHmo.ListIndex = 0 Then
    MsgBox " please specify Whether Patient is a Referal"
    cboRefHmo.SetFocus
    Exit Sub
End If


'If cboRef.Text = "" Then
'MsgBox " please specify Name of Doctor"
'cboRef.SetFocus
'Exit Sub
'End If

Dim connTran As New Connection
connTran.ConnectionString = conStr
connTran.Open

Dim rsExP As Recordset
Set rsExP = New ADODB.Recordset
With rsExP
        .CursorLocation = adUseClient
        .ActiveConnection = conStr
        
            Dim cmdX As New ADODB.Command
            Dim dtSys As Date
            dtSys = getSysDateTime
            'sDate = Format(dtSys, "Short Date")
            'sTime = Format(dtSys, "Short Time")
        
        On Error GoTo dbFail
        connTran.BeginTrans
        
        
        If flgEdit Then
            .Open "select * from hRecords where recID=" & strParam, connTran, adOpenStatic, adLockOptimistic
            '!recdate = Format(dtSys, "Short Date")
            '!consultID = lblBill.Caption 'cannot be altered
            !clientCat = strBillCat    'cboBilling.Text
            !clinicType = cboType.Text
            !Remarks = cboPurpose.Text  'not strpurpose
            !HmoRef = cboRefHmo.Text   'real referal
            !referal = CboRef.Text 'ffs  its ref in hPatients 'its OK
            !empID = strEmpID
            !CoyName = strCoyID
            
            If cboDoc.Text <> "" Then
                !DocAssigned = DocAssigned   'strEmpDoc
                '!PatVal = 1
            End If
            
            '!attendedtoByDoc = 0
            '!PNo = lblNo.Caption
            '!htime = Format(dtSys, "Short Time")
            '!DocAssigned = strEmpDoc
            '!PatVal = 1
            '!attendedToByNurse = 0
            '!EmpID = strEmpID
            .Update
            
            
                 With cmdX
                    .ActiveConnection = connTran
                    .CommandText = "Update hpatients set " & _
                    "ref='" & CboRef.Text & "', " & _
                    "HmoRef='" & cboRefHmo.Text & "', " & _
                    "PolicyType='" & Replace(txtPolicy.Text, "'", "''") & "', " & _
                    "EmpNo='" & Trim(Replace(txtEmp.Text, "'", "''")) & "', " & _
                    "ClientCatID='" & strBillCat & "', " & _
                    "CoyName='" & strCoyID & "' " & _
                    "where pno = '" & strpCode & "'"
                    cmdX.Execute
                    Set cmdX = Nothing
                    ''"clientCatID='" & cboBilling.Text & "'," & _

                 End With
        
           connTran.CommitTrans
            Screen.MousePointer = vbDefault
            flgSch = False
            flgEdit = False
            Call Auditrail(m_Username, "Edit Attendance for " & cboVehNo.Text, lblBill.Caption, Client & "/" & cboClient.Text & "/" & cboType.Text & "/" & cboPurpose.Text, strHostName)
         MsgBox "Record Succesfully Edited and Updated"
        'Call fillGrid
        
        Else
            'Call getCorrectConID(connTran)
            
            Call getIDNo("ConsultID2")
            lblBill.Caption = getID_No
            
                'If iDNo < IDConVal Then
                '    iDNo = IDConVal
                '    strIDConVal = Right("000000000" & CStr(iDNo), 9)
                '    lblBill.Caption = strIDConVal
                'End If
            
            .Open "select  * from hRecords where 1=2", connTran, adOpenStatic, adLockOptimistic
            .AddNew
            !recDate = Format(dtSys, "Short Date")
            !PNo = strpCode
            !consultID = lblBill.Caption
            !empID = strEmpID
            !clinicType = cboType.Text
            !Remarks = cboPurpose.Text   'not strpurpose
            '!nextApptDate = ""  'dtNext.Value
            !htime = Format(dtSys, "Short Time")
            !HmoRef = cboRefHmo.Text   'real referal
            !referal = CboRef.Text 'ffs  its ref in hPatients 'its OK
            '!DocAssigned = strEmpDoc
            !suppres = 0
            !attendedto = 0
            !attendedtoByDoc = 0
            '!PatVal = 1
            !attendedToByNurse = 0
            '!ExitDate = sysDate 'formaula field
            !BillDate = Format(dtSys, "Short Date")  'sysDate
                
            !clientCat = strBillCat 'cboBilling.Text
            !CoyName = strCoyID
            !Debt = dblSaveBF
            
                'If strBillCat = "HMO" Or strBillCat = "PHIS" Then
                '    !Tariff = strCoyID 'CoyName or retainID for each HMO
                'Else
                '    If strBillCat = "PRIVATE" Then
                '        !Tariff = strPrivate 'could be private or 0001
                '    Else
                '        !Tariff = strBillCat 'ClientCatID in BillingPrice tbl 'eg MTHLY,NHIS,6MTHLY
                '    End If
                'End If
            
            .Update
            
'''''''''''''''''''''''''''''''''''''''debt info''''''''''''''''''''''''''''''
                If flgCardRenew = True Then
                    Dim DBLfEE As Double
                    .Close
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
                        !PNo = strpCode
                        !consultID = strCval
                        !billType = "SERVICE"     'strCard & " REG"
                        !ConID = Null
                        !Category = "REGISTRATION"
                        !attendedto = 0
                        !suppres = 0
                        !Capitated = "NO"
                        !isbilled = 0
                        !CoyName = strCoyID
                        !billTo = strBillTo
                        !revType = "REGISTRATION"
                        .Update
                                    
                                    
                
                End If
                
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                
                
                
                Dim cmd As New ADODB.Command
                 With cmd
                    .ActiveConnection = connTran
                    .CommandType = adCmdText
                    .CommandText = "Update hpatients set " & _
                    "ref='" & CboRef.Text & "', " & _
                    "HmoRef ='" & cboRefHmo.Text & "', " & _
                    "PolicyType='" & txtPolicy.Text & "', " & _
                    "EmpNo='" & Trim(txtEmp.Text) & "', " & _
                    "ClientCatID='" & strBillCat & "', " & _
                    "CoyName='" & strCoyID & "' " & _
                    "where pno = '" & strpCode & "'"
                    .Execute
                    Set cmd = Nothing
                 End With
  

                 
                 
                ''     "clientCatID='" & cboBilling.Text & "'," & _
                'If cboBilling.Text <> strBilling Then
                '       Dim cmd As New ADODB.Command
                '        With cmd
                '           .ActiveConnection = connTran
                '           .CommandType = adCmdText
                '           .CommandText = "Update hpatients set clientCatID='" & cboBilling.Text & "' where pno = '" & strpCode & "'"
                '           .Execute
                '           'Set cmd = Nothing
                '        End With
                '   End If
            
        
        flgSch = False
        'Call insIDNo(connTran)
        Call getServiceFee(connTran)    ''seeding only
        
        'If cboPurpose.Text = "PRE-EMPLOYMENT" Or cboPurpose.Text = "EXEC-SCREENING" Or cboPurpose.Text = "SCREENING" Then
            'Call getScreening(connTran)  '''pre-employment
        'End If
        
        If strCoyID = strPrivate Then
            Call getServiceFeeConsulting(connTran) ' needed for consultation fee
        End If
        
        Call updateRegInBillAccum(connTran) '---if blank consultID exists
            
         
         If strRefCon <> "" Then
                'Dim cmd As New ADODB.Command
                cmd.ActiveConnection = connTran
                cmd.CommandType = adCmdText
                cmd.CommandText = "Update hreferal set attendedTo = 1 where ConsultID = '" & strRefCon & "'"
                cmd.Execute
         End If
        
        connTran.CommitTrans
            flgSch = False
         flgEdit = False
        Call Auditrail(m_Username, "Insert Attendance for " & cboVehNo.Text, lblBill.Caption, "", strHostName)
            
    
        'If dblSaveBF <> 0 Then '-ve val means debt from tranxaction tbl. add minus to its subtotal to make it a bill
        '   Call Auditrail(m_Username, "Insert Debt for " & cboVehNo.Text, lblBill.Caption, -(dblSaveBF), strHostName)
        'End If
                
               
                
        
        ''''''''''''''Send SMS'''''''ATTEND''''''''''''''''''''''''''''''
            'Call sendToSmsCenter(strpCode, lblBill.Caption, "ATTEND", "ATTENDANCE", sysDate, sysTime, "", "")
        
        ''''''''''''''''''''''''''''''''''''''''''''''''''''
            
        MsgBox "Record Succesfully saved"
            
        'Call fillGrid
        
        End If
        Set rsExP = Nothing
End With

On Error GoTo errH

'txtName.SetFocus
'Call fillGrid

If cboType.Text = "(BOOK APPT)" Then
'    MsgBox "Please enter Patient into Appointment Book"
'    frmReferalEdit.Hide
'    frmReferalEdit.Show
End If

flgOn = False
Call clearFields
Call SetButtons(True)
enableFields False
'cboVehNo.Enabled = True
    flgSch = False
 ''flgEdit = False
 flgEdit = True ''page is edit only
Call fillGrid
Call loadClientCat '' ok here
Screen.MousePointer = vbDefault

Exit Sub
errH:
Screen.MousePointer = vbDefault

MsgBox "check Consult ID OR try to save again" & vbCrLf & Err.Description
flgEdit = False
Set rsExP = Nothing

Exit Sub
dbFail:
Screen.MousePointer = vbDefault

connTran.RollbackTrans
MsgBox Err.Description

End Sub

Public Sub clearFields()
On Error GoTo errH
dblSaveBF = 0 'very impt 'to prevent it spilling to subsequent newly reg pat

flgCardRenew = False

flgOn = False

'Call MdiSapid.Timer2_Timer

dblSaveBF = 0
cmdRenew.Enabled = False
flgSch = False
 flgEdit = True
txtName.Text = ""
lblFound.Caption = ""
    'dtDate.Value = ""
    cboVehNo.ListIndex = -1
    'cboAppr.ListIndex = -1
    txtRem.Text = ""
    cboType.ListIndex = -1
    'lblDisplay.Caption = ""
    dtNext.Value = ""
    cboPurpose.ListIndex = -1
    CboRef.ListIndex = -1
    cboRefHmo.ListIndex = -1
txtEmp.Text = ""
'lblRef.Caption = ""
lblBill.Caption = ""
cboPat.ListIndex = -1
cboClient.ListIndex = -1
txtPolicy.ListIndex = -1
Label5.Caption = "PATIENT ATTENDANCE"
strPurpose = ""
'strFollow = ""
lblNo.Caption = ""
lblOld.Caption = ""
LblOccup.Caption = ""
lblPolicy.Caption = ""
lblExpire.Caption = ""
piX.Picture = Nothing
MdiSapid.SbrSapid.Panels(1).Text = "Records"
cboDoc.ListIndex = -1
chkAll.Value = False
Exit Sub
Screen.MousePointer = vbDefault
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description
End Sub


Private Sub loadClientCat()
On Error GoTo errH
Screen.MousePointer = vbHourglass
    cboClient.Clear
    cboClient.AddItem ""
    Dim rx As New Recordset
    rx.Open "select distinct clientCatID  from billingPrice order by clientCatID", conStr, adOpenForwardOnly, adLockReadOnly
    If Not rx.EOF Then
        rx.MoveFirst
        Do While Not rx.EOF
        cboClient.AddItem rx!clientCatID & ""
        rx.MoveNext
        Loop
    End If

Screen.MousePointer = vbDefault
Exit Sub
errH:
Screen.MousePointer = vbDefault
MsgBox Err.Description

End Sub

Public Sub fillGrid()
On Error GoTo errH
Dim rsVal As New Recordset
Set grdData.DataSource = Nothing
With rsVal
.CursorLocation = adUseClient
.Open "select distinct Fullname, RecDate as Date, [Time],CoyName as Client, ClientCat as BillingCat, ClinicType, recID, Surname, pNO as PatNo,PNo,Doctor,DocAssigned,empID, Remarks,attendedTo,ConsultID, Referal as FeeForService,HMORef as Referal,RetainID,PolicyType,EmpNo as EnrolleNo from qryhVisitsForToday ORDER BY recID desc", conStr, adOpenStatic, adLockOptimistic
'.Open "select distinct  Fullname, RecDate as Date, [Time], ClientCat, ClinicType, recID, Surname, DocName, pNO as PatNo, empID, Remarks,attendedTo,ConsultID, Referal from qryhVisitsForToday ORDER BY recID desc", conSTR, adOpenStatic, adLockOptimistic
If Not .EOF Then
Set grdData.DataSource = rsVal
grdData.Columns("recID").Visible = False
grdData.Columns("attendedTo").Visible = False
'grdData.Columns("referal").Visible = False

grdData.Columns("Surname").Visible = False

    grdData.Columns("empID").Visible = False
Me.Caption = "ATTENDANCE FOR TODAY:" & CStr(.RecordCount)
Else
Me.Caption = "ATTENDANCE FOR TODAY:"
End If

'Call getDocWaitList

End With
Set rsVal = Nothing
Exit Sub
errH:
'rsVal.Close
MsgBox Err.Description

End Sub

Private Sub getScreening(ByVal connTran As Connection)
'''''''''''''''''''''''''''''''''''''''''PRE-EMPLOYMENT''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'    If cboPurpose.Text = "PRE-EMPLOYMENT" Or cboPurpose.Text = "EXEC-SCREENING" Or cboPurpose.Text = "SCREENING" Then
'        Dim strPrescX As String
'        Dim rsExP As New Recordset
'        Dim rsLab As New Recordset
'
'      strPrescX = "PRE-EMPLYMENT/SCREENING TESTS"
'
'        rsExP.Open "select * from hConsulting where 1=2", connTran, adOpenKeyset, adLockOptimistic
'        rsExP.AddNew
'        rsExP!ConsultID = strCval
'        rsExP!clientCat = txtBilling.Text
'        rsExP!CDate = sysDate
'        rsExP!pNo = strpCode
'        rsExP!wardID = "XXX"
'        rsExP!referto = strPrescX   'Trim(cboRef.Text)
'        rsExP!treatedby = "SYS0000"  'strEmpID  'cboReason.Text
'        'rsExP!nextApptDate = dtNext.Value
'        rsExP!preconsult = ""    'txtRem.Text)
'        rsExP!TreatType = "PRIMARY"     'txtRem.Text)
'
'
'
'        rsExP!investigate = strPrescX
'        rsExP!attendedto = 0
'        'strPrescX = strPrescX & vbNewLine & "-----Lab Items------" & vbNewLine & Trim(txtLab.Text)
'
'
'        rsExP!Services = strPrescX  'Trim(txtServ.Text)
'        rsExP!attendedto = 0
'
'        rsExP!Prescription = "NO PRESCRIPTION"
'
'        rsExP!attendedTobyLab = 0
'        rsExP!attendedToByPharm = 1 'for pre-emp 0
'        rsExP!investigate = strPrescX
'
'        'rsExP!Complaints = Trim(txtComp.Text)
'        ''rsExP!sysreview = Trim(strSys) & vbNewLine & "Gen Sys Exam: " & Trim(txtGenSys.Text)     'txtSys.Text)
'        'rsExP!phyexam = Trim(strPhy) & vbNewLine & "Gen Phy Exam: " & Trim(txtGenPhy.Text)         'txtPhy.Text
'        'rsExP!diagnosis = Trim(txtDiag.Text)
'        ''rsExP!diffdiagnosis = Trim(txtDiff.Text)
'        'rsExP!hpc = Trim(txtHPC.Text)
'        ''rsExP!pmh = Trim(txtPMH.Text)
'        ''rsExP!drugHx = Trim(txtDrugHx.Text)
'        ''rsExP!informt = Trim(txtInfo.Text)
'
'        ''rsExP!gensys = Trim(txtGenSys.Text)
'        ''rsExP!genphy = Trim(txtGenPhy.Text)
'        rsExP!treatPlan = strPrescX
'
'        ''rsExP!treatDone = Trim(txtDone.Text)
'        ''rsExP!extraOralExam = Trim(txtExtra.Text)
'        ''rsExP!intraOralExam = Trim(txtIntra.Text)
'
'
'        '!isReview = blnRevw
'        rsExP!isAlarm = 0
'        rsExP!cTime = sysTime        'Time
'
'        rsExP!Clinic = "(GENERAL)" ' cboClin.Text  '"(GENERAL)"
'        ''rsExP!ClinicRemarks = Trim(txtOtherClinic.Text)
'
'        rsExP!Remarks = "(GENERAL)"
'
'        rsExP!attendedto = 0
'        rsExP!isDrug = 0
'        rsExP!isLab = 0
'        rsExP!isServ = 0
'
'
'
'    rsExP.Update
'
'    Call Auditrail(m_Username, "Insert Consultation for: " & Label5.Caption, strCval, strPrescX, strHostName)
'
'    Dim strConIDVal As Long
'    'strConIDVal = rsExP!ID
'    Dim rsX As New Recordset
'    rsX.Open "select top 1 ID from hconsulting order by ID desc", connTran, adOpenForwardOnly, adLockReadOnly
'    strConIDVal = rsX!ID
'    Set rsX = Nothing
'
'
'        Dim cmd As New ADODB.Command
'        cmd.ActiveConnection = connTran
'        cmd.CommandType = adCmdText
'
'    ''''''''''call other inserts
'
'            rsLab.Open "select * from hInvestigate where 1=2", connTran, adOpenStatic, adLockOptimistic
'            rsLab.AddNew
'            rsLab!ConsultID = strCval
'            rsLab!invDate = dtDate.Value
'            rsLab!pNo = strpCode
'            rsLab!investigate = strPrescX  'strLabX  '
'            rsLab!clientCat = cboPurpose.Text
'            rsLab!attendedto = 0
'            rsLab!attendedTobyLab = 0
'            rsLab!conID = strConIDVal 'from hconsulting ID
'            rsLab!suppres = 0
'
'            rsLab.Update
'
'
'            cmd.CommandText = "update hInvestigate set investigate ='" & strPrescX & "' where conID =" & strConIDVal
'            cmd.Execute
'            Call Auditrail(m_Username, "Insert LabTest for: " & Label5.Caption, strCval, strPrescX, strHostName)
'
    
    
    
     
     
        
'    End If
    
 ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    
End Sub





Public Sub getServiceFee(conn As Connection)
Dim DBLfEE As Double
Dim dblCost As Double
Dim dblVal As Double
Dim rsVal As New Recordset
dblVal = 0

Dim strConAttd As String
strConAttd = cboType.Text

With rsVal

            .Open "select *  from billAccum where 1=2", conn, adOpenStatic, adLockOptimistic
            .AddNew
            !dtDate = Format(dtSys, "Short Date")
            !drgName = "ATTENDANCE"  'strConAttd & " ATTENDANCE"
            !Price = 0 'seed   ' DBLfEE
            !Qty = 1
            !SubTotal = 0 'seed  ' DBLfEE
            !PNo = lblNo.Caption
            !consultID = strCval
            !billType = "SERVICE" '& " ATTENDANCE"    '"ATTENDANCE"
            !ConID = Null
            !CoyName = strpCode
            !billTo = strBillTo
            !attendedto = 0
            !isbilled = 0
            !revType = "CONSULTATION"
            
            .Update
            
            '.Close
            '.Open "select *  from billAccum where 1=2", conN, adOpenStatic, adLockOptimistic
            '.AddNew
            '!dtDate = Format(dtSys, "Short Date")
            '!drgNAME = cboPurpose.Text  'not strpurpose  '"ATTENDANCE"  'strConAttd & " ATTENDANCE"
            'If strPurpose = "SCREENING" Then
            '    !Price = ScreeningAmount 'seed   ' DBLfEE
            '    !Qty = 1
            '   !SubTotal = ScreeningAmount
            'ElseIf strPurpose = "(CONSULTATION)" Or strPurpose = "CONSULTATION" Then
            '    !Price = ConsultAmount 'seed   ' DBLfEE
            '    !Qty = 1
            '    !SubTotal = ConsultAmount
            'Else
            '    !Price = 0 'seed   ' DBLfEE
            '    !Qty = 1
            '    !SubTotal = 0 'seed  ' DBLfEE
            'End If
            '!PNo = lblNo.Caption
            '!consultID = strCval
            '!billType = "SERVICE" '& " ATTENDANCE"    '"ATTENDANCE"
            '!conID = strCval
            '!CoyName = strCoyID
            '!billTo = strBillTo
            '!attendedto = 0
            '!isbilled = 0
            '!revType = Revtype_Consult
            '.Update

                '    Dim strAccumIDVal As Long
                '    Dim rsX As New Recordset
                '    rsX.Open "select top 1 SNo from BillAccum order by SNo desc", conN, adOpenForwardOnly, adLockReadOnly
                '    strAccumIDVal = rsX!SNo
                '    Set rsX = Nothing
                '
                '
                '    Dim rsDetails As New Recordset
                '    rsDetails.Open "select *  from BillingDetails where 1=2", conN, adOpenStatic, adLockOptimistic
                '    rsDetails.AddNew
                '    rsDetails!dtDate = Format(dtSys, "Short Date")
                '    rsDetails!billNo = strCval
                '    rsDetails!SNo = strAccumIDVal  'rsIns!SNo 'sno from billAccum
                '    rsDetails!drgName = cboPurpose.Text
                '
                '    If strPurpose = "SCREENING" Then
                '        rsDetails!Price = ScreeningAmount 'seed   ' DBLfEE
                '        rsDetails!Qty = 1
                '       rsDetails!SubTotal = ScreeningAmount
                '    ElseIf strPurpose = "(CONSULTATION)" Or strPurpose = "CONSULTATION" Then
                '       rsDetails!Price = ConsultAmount 'seed   ' DBLfEE
                '        rsDetails!Qty = 1
                '        rsDetails!SubTotal = ConsultAmount
                '    Else
                '        rsDetails!Price = 0 'seed   ' DBLfEE
                '        rsDetails!Qty = 1
                '        rsDetails!SubTotal = 0 'seed  ' DBLfEE
                '    End If
                '
                '    rsDetails!billType = "SERVICE"
                '    rsDetails!conID = Null
                '    rsDetails!dosage = ""
                '    rsDetails!CoyName = strCoyID
                '    rsDetails!billTo = strBillTo
                '    rsDetails!revType = Revtype_Consult
                '    rsDetails.Update


If strPurpose = "SCREENING" Then
    .Close
    .Open "select *  from hhScreeningAttnd where 1=2", conn, adOpenStatic, adLockOptimistic
    .AddNew
    !recDate = Format(dtSys, "Short Date")
    !consultID = strCval
    !Remarks = strPurpose  'strpurpose ok here
    .Update
End If



End With

Set rsVal = Nothing

Call Auditrail(m_Username, "Insert Attnd Fee for " & cboVehNo.Text, lblBill.Caption, "", strHostName)


End Sub


Public Sub genConID(Optional connTran As Connection)
'Dim cVal As Long
'Dim cVal2 As Long
'Dim strValX As String
'
'Dim rsBL As New Recordset
'  With rsBL
''.Open "select MAX(cast(SUBSTRING(consultID, 4, 9)as bigint))  as ID from hRecords WHERE  (SUBSTRING(consultID, 1, 3)='" & strHospID & " ')", conSTR, adOpenForwardOnly, adLockReadOnly
'.Open "select MAX(cast(SUBSTRING(consultID, 4, 9)as bigint))  as ID from hRecords WHERE   SUBSTRING(consultID, 4, 3)<>'ofl' and (SUBSTRING(consultID, 1, 3)='" & strHospID & " ')", connTran, adOpenForwardOnly, adLockReadOnly
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
'
'lblBill.Caption = strCval
'
'End With
'Set rsBL = Nothing


End Sub

Public Sub genIDNo()
'  '''''''
'  On Error GoTo errH
' Dim rsGen As New ADODB.Recordset
' With rsGen
'.Open "select ID from IDgen where DestName = 'Consulting'", conSTR, adOpenForwardOnly, adLockReadOnly
'If rsGen.EOF Then
'MsgBox "Bill No generator has encountered some problems"
'Else
'     iDNo = !ID
'     iDNo = iDNo + 1
'
'        strIDConVal = Right("000000000" & CStr(iDNo), 9)
'        lblBill.Caption = strIDConVal
'End If
'Set rsGen = Nothing
' End With
'  '''''''
'  Exit Sub
'errH:
''If rsGen.EOF Then rsGen!ID = 0
''Resume Next
'MsgBox Err.Description
End Sub

Public Sub insIDNo(conn As Connection) 'not nece
'On Error GoTo errH
'  '''''''
'      If iDNo < IDConVal Then
'        iDNo = IDConVal
'    End If
'
' Dim Cmd As New ADODB.Command
' With Cmd
'.ActiveConnection = conN
'.CommandType = adCmdText
'.CommandText = "Update iDGen set ID=" & iDNo & " where DestName = 'Consulting'"
'.Execute
'Set Cmd = Nothing
''Set cN = Nothing
' End With
'  '''''''
' Exit Sub
'errH:
' MsgBox "Problems Saving generated Consult No with error " & Err.Description
  End Sub

Private Sub SetButtons(bVal As Boolean)
  cmdAdd.Visible = bVal
  cmdEdit.Visible = bVal
  OKButton.Visible = Not bVal
  cmdCancel.Visible = Not bVal
  cmdDel.Visible = bVal
  cmdRefresh.Visible = bVal
End Sub

Public Sub getCorrectConID(connTran As Connection)
Dim cVal As Long
Dim cVal2 As Long
Dim strValX As String

    getID_No = "" 'ok b4 call of getIDNo
    Call getIDNo("ConsultID2")
    If getID_No = "" Then
        MsgBox "Unable to generate No!!! Function getIDNo Failed! ConsultID"
        Unload Me
        'Exit Sub
    End If
    
    strCval = getID_No
    
lblBill.Caption = strCval




End Sub

Private Sub enableFields(xVal As Boolean)
    cboVehNo.Enabled = xVal
    cboType.Enabled = xVal
    cboPurpose.Enabled = xVal
    'cboBilling.Enabled = xVal
    dtDate.Enabled = xVal
    CboRef.Enabled = xVal
    cboRefHmo.Enabled = xVal
    cboDoc.Enabled = xVal
    'cboAppr.Enabled = xVal
    
End Sub

Private Sub Timer1_Timer()
Call getDocWaitList
End Sub
Public Sub getDocWaitList()
  Dim rsBL As New Recordset
  With rsBL
    Set grdDoc.DataSource = Nothing
  'cboType.Clear
  'cboType.AddItem " "
Dim strList As String
strList = ""
.CursorLocation = adUseClient
  .Open "select distinct [Date],DocName,NumOfPat from vwDocWaitingListGrouped where date='" & dtDate.Value & "'", conStr, adOpenStatic, adLockOptimistic
If Not .EOF Then
Set grdDoc.DataSource = rsBL
grdDoc.Columns("DocName").Width = 4000

'    strList = strList & "Date" & vbTab & vbTab & "Doctor" & vbTab & vbTab & vbTab & vbTab & vbTab & "NumOfPat" & vbNewLine
'    strList = strList & "-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------" & vbNewLine
'    Do While Not .EOF
'        strList = strList & ![Date] & vbTab & vbTab & !Docname & " (" & !RoomNo & ")" & vbTab & vbTab & !NumOfPat & vbNewLine
'    .MoveNext
'    Loop
'    txtDocPat.Text = strList
Else
    Set grdDoc.DataSource = Nothing
End If
End With
Set rsBL = Nothing
End Sub

Private Sub tmrBill_Timer()
Static blnFlg As Boolean
    If blnFlg Then
        Label5.BackColor = vbBlack
    Else
        Label5.BackColor = vbRed
    End If
        blnFlg = Not blnFlg
End Sub


Public Sub getServiceFeeConsulting(coN As Connection)

'Dim DBLfEE As Double
'Dim dblCost As Double
'Dim dblVal As Double
'Dim rsVal As New Recordset
'dblVal = 0
'    Dim dtSys As Date
'    dtSys = getSysDateTime
'Dim strConAttd As String
'strConAttd = cboType.Text
'
'With rsVal
'    .Open "select price from hServiceNHIS where sno =" & PVT_CONFEE_SNO, coN, adOpenStatic, adLockOptimistic
'    If Not .EOF Then
'                DBLfEE = FormatNumber(IIf(IsNull(!Price), 0, !Price), 2)
'
'            Select Case strPurpose
'            Case "CONSULTATION", "(CONSULTATION)", ""
'                DBLfEE = DBLfEE
'            Case "FOLLOW-UP"
'                DBLfEE = DBLfEE \ fVal
'            Case "REVIEW"
'                DBLfEE = 0   'DBLfEE + (DBLfEE * dblVal) \ fVal
'            End Select
'    Else
'                DBLfEE = 0
'    End If
'
'        .Close
'        .Open "select *  from billAccum where 1=2", coN, adOpenStatic, adLockOptimistic
'        .AddNew
'        !dtDate = Format(dtSys, "Short Date")
'        !drgNAME = "CONSULTATION" ' (" & strConAttd & ")"
'        !Price = DBLfEE
'        !Qty = 1
'        !SubTotal = DBLfEE
'        !pNo = lblNo.Caption
'        !consultID = lblBill.Caption
'        !billType = "SERVICE"
'        !category = "CONSULTATION"
'        !CoyName = strCoyID
'        !billTo = strBillTo
'
'        .Update
'
'
'End With
'
'Set rsVal = Nothing
'
'Call Auditrail(m_Username, "Insert Con Fee for " & cboVehNo.Text, lblBill.Caption, "", strHostName)
'

End Sub


Public Sub updateRegInBillAccum(coN As Connection)
Dim cmd As New ADODB.Command
cmd.ActiveConnection = coN
cmd.CommandText = "update billaccum set consultID='" & lblBill.Caption & "' where pno = '" & strpCode & "' and consultID = ''"
cmd.CommandType = adCmdText
cmd.Execute
End Sub


Public Sub getPixFromDB(PatNo As String)
'On Error GoTo errH

Dim strPatNo2 As String
delPath = ""
strPatNo2 = Replace(strpCode, "/", "") 'strPatNo is set from editval
delPath = App.path & "\" & strPatNo2 & ".JPG"
'MsgBox delPath

If PixLoc = "FILE" Then
    Dim strPatPix As String
    Dim strPixNo As String
    Set piX.Picture = Nothing
    strPixNo = Replace(strpCode, "/", "")
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


''On Error GoTo errH
'Dim delPath As String
'Dim strPatNo2 As String
'delPath = ""
'strPatNo2 = Replace(patNo, "/", "") 'strPatNo is set from editval
'delPath = App.Path & "\" & strPatNo2 & ".JPG"
''MsgBox delPath
'Dim rsPix As New ADODB.Recordset
'rsPix.Open "Select patPix from hpatients where pno='" & patNo & "'", conSTR, adOpenKeyset, adLockOptimistic
'   If rsPix Is Nothing Then
'        Exit Sub
'        piX.Picture = Nothing
'    End If
'
'If Not rsPix.EOF Then
'    If Not IsNull(rsPix.Fields("PatPix").Value) Then
'        Dim msStream As New ADODB.Stream
'        msStream.Type = adTypeBinary
'        msStream.Open
'        msStream.Write rsPix.Fields("PatPix").Value
'        msStream.SaveToFile delPath, adSaveCreateOverWrite
'        piX.Picture = LoadPicture(delPath)
'        Kill (delPath)
'    Else
'        'Exit Sub
'        piX.Picture = Nothing
'    End If
'Else
'        'Exit Sub
'        piX.Picture = Nothing
'End If
'
'
''Exit Sub
''errH:
''MsgBox Err.Description

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
