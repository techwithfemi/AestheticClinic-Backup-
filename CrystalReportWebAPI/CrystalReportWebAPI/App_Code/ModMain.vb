Imports System.Web.Configuration
'Imports System.Web.Configuration.WebConfigurationManager
Imports System.Configuration
Imports System.Web.HttpContext
Imports System.Data.SqlClient
Imports System.Data
Imports SmartPay.NAPS.SQLSeverDAL

Namespace CrystalReportsVB
    Public NotInheritable Class ModMain

        Private provider As String = "RSAProtectedConfigurationProvider"

        Private section As String = "connectionStrings"

        Public conStr As String = ""
        Public conStr2 As String = ""
        Public conStrHR As String = ""
        Public connectionstring As String = ""
        Public constrSMS As String = ""
        Dim svrName As String
        Dim strSVR As String
        Public sysDate As Date
        Public sysTime As Date
        Public SendSMS As String

        Public dtSys As Date

        '''for Staff Attendance'''''''''''''''to be moved later
        Public strPixPath As String
        Public strPixPathHR As String
        Public strSubPath As String
        Public intSvr As String
        Public isPixInDB As Boolean
        Public strPNo As String
        Public StrPNoID As String
        Public maxTemplate As Int64
        Public strRem As String = ""
        Public timeInterval As Int16 = 10000
        Public SignOutValue As Int16 = 3

        Public UserName As String = ""
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''



        Sub New()

            Dim fName As String = ""
            Dim fVal As Integer = 0
            Dim NextLine As String = ""

            Dim fnum As Integer
            fnum = FreeFile()
            Dim Path As String = ""
            svrName = ""





            ' ''''''''''''''''''''''''''''''''''''''''''''''
            Try

                'Call EncryptConnString()

                'Call DeCryptConnString()


                ' the encrypted section is automatically decrypted!!
                conStr = WebConfigurationManager.ConnectionStrings("connSmart").ConnectionString
                conStr2 = WebConfigurationManager.ConnectionStrings("ConstrAcct").ConnectionString
                connectionstring = WebConfigurationManager.ConnectionStrings("connSmart").ConnectionString
                constrSMS = WebConfigurationManager.ConnectionStrings("ConnStrSMS").ConnectionString
                conStrHR = WebConfigurationManager.ConnectionStrings("conStrHR").ConnectionString




                'conStr = System.Configuration.ConfigurationManager.ConnectionStrings("connSmart").ToString
                'connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings("connSmart").ToString
                'constrSMS = System.Configuration.ConfigurationManager.ConnectionStrings("ConnStrSMS").ToString



                '    fVal = FreeFile()

                '    Path = My.Application.Info.DirectoryPath & "\made.sys"
                '    fName = My.Application.Info.DirectoryPath & "\made.sys"

                '    If File.Exists(Path) = True Then
                '        FileOpen(fnum, Path, OpenMode.Input)
                '        While Not EOF(fnum)
                '            Input(fnum, NextLine)
                '            If Mid(NextLine, 1, 3) = "SVR" Then
                '                strSVR = Mid(NextLine, 5)
                '                svrname = Mid(NextLine, 5)

                '                FileClose(fnum)
                '                Exit While
                '            End If
                '        End While
                '    Else
                '        FileClose(fnum)
                '        MsgBox("A System file bAccum required for this Application is MISSING")
                '        'End
                '    End If
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                'strSVR = "logic\sql2008" ' from a made.sys above
                'dsource = "Data Source=" & strSVR
                'connectionstring = "Persist Security Info=False;Initial catalog=Hospital;uid=smart;pwd=smartsys;" & dsource
                'conStr = System.Configuration.ConfigurationManager.ConnectionStrings("connSmart").ToString
                'connectionstring = System.Configuration.ConfigurationManager.ConnectionStrings("connSmart").ToString

                ''Call getUserName() 'not needed


            Catch ex As Exception
                Throw New Exception(ex.Message)
                'End
            End Try

        End Sub


        Private Sub EncryptConnString()
            Try

                Dim confg As System.Configuration.Configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)

                Dim confStrSect As ConfigurationSection = confg.GetSection(section)

                If Not confStrSect Is Nothing Then

                    confStrSect.SectionInformation.ProtectSection(provider)

                    confg.Save()

                End If

                '' the encrypted section is automatically decrypted!!
                'Response.Write("Configuration Section " & "<b>" & WebConfigurationManager.ConnectionStrings("MyConnString").ConnectionString & "</b>" & " is automatically decrypted")

            Catch ex As Exception

                Throw ex

            End Try
        End Sub


        Private Sub DeCryptConnString()
            Try

                Dim confg As Configuration = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath)

                Dim confStrSect As ConfigurationSection = confg.GetSection(section)

                If Not confStrSect Is Nothing AndAlso confStrSect.SectionInformation.IsProtected Then

                    confStrSect.SectionInformation.UnprotectSection()

                    confg.Save()

                End If



            Catch ex As Exception



            End Try
        End Sub



        Public Sub DisplayClientError(ByVal errorDesc As String, sender As Object)


            Dim script As String = "alert('" + errorDesc + "');"

            ScriptManager.RegisterStartupScript(sender, GetType(Page), "UserSecurity", script, True)
        End Sub


        Public Sub ConfirmBox(ByVal strMessage As String, ByRef btn As WebControls.Button)
            btn.Attributes.Add("onclick", "return confirm('" & strMessage & "');")
        End Sub


        Public Function Encrypt(ByVal Password As String) As String
            Dim Jarg As String
            Jarg = ""
            Dim I As Integer, j As Integer, Enc As String
            Enc = ""
            For I = 1 To Len(Password)
                j = Asc(Mid(Password, I, 1))
                If j > 32 And j < 127 Then
                    Enc = Enc + Mid(Jarg, j - 32, 1)
                Else
                    Throw New Exception("Invalid character detected!")
                End If
            Next
            Encrypt = Enc
        End Function


        Public Sub getSysDateTime()
            Dim dr As SqlDataReader
            Dim conn As New SqlConnection(conStr)
            Try


                conn.Open()
                Dim cmd As New SqlCommand("select sysDT from qrySysDateTime", conn)
                dr = cmd.ExecuteReader

                If dr.Read Then
                    'sysDate = CDate(dr.Item("sysdt"))
                    sysDate = Format(dr.Item("sysdt"), "Short Date")
                    sysTime = Format(dr.Item("sysdt"), "Short Time")
                End If
                dr = Nothing
                conn = Nothing
                cmd = Nothing
                Exit Sub
            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try
        End Sub


        Public Function GetColumnIndexByName(row As GridViewRow, columnName As String) As Integer
            'GridView does not act as column names, 
            'as that's it's datasource property to know those things.
            'If you still need to know the index given a column name, 
            'then you can create a helper method to do this as the gridview Header 
            'normally contains this information.

            Dim columnIndex As Integer = 0
            columnIndex = 0
            For Each cell As DataControlFieldCell In row.Cells
                If TypeOf cell.ContainingField Is BoundField Then
                    If String.Equals(DirectCast(cell.ContainingField, BoundField).DataField, columnName, StringComparison.OrdinalIgnoreCase) Then
                        Exit For
                    End If
                End If
                ' keep adding 1 while we don't have the correct name
                columnIndex += 1
            Next
            Return columnIndex



            'remember that the code above will use a BoundField... then use it like:
            'protected void GridView_RowDataBound(object sender, GridViewRowEventArgs e)
            '{
            '    if (e.Row.RowType == DataControlRowType.DataRow)
            '    {
            '        int index = GetColumnIndexByName(e.Row, "myDataField");
            '        string columnValue = e.Row.Cells[index].Text;
            '    }
            '}

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'I would strongly suggest that you use the TemplateField to have your own controls, then it's easier to grab those controls like:
            '<asp:GridView ID="gv" runat="server">
            '    <Columns>
            '        <asp:TemplateField>
            '            <ItemTemplate>
            '                <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' />
            '            </ItemTemplate>
            '        </asp:TemplateField>
            '    </Columns>
            '</asp:GridView>

            'and then use
            'string columnValue = ((Label)e.Row.FindControl("lblName")).Text;

        End Function


        Public Sub EnableControls(page As Control, enable As Boolean)
            For Each ctrl As Control In page.Controls
                ' not sure exactly which controls you want to affect so just doing TextBox
                ' in this example.  You could just try testing for 'WebControl' which has
                ' the Enabled property.
                If TypeOf ctrl Is TextBox Then
                    DirectCast(ctrl, TextBox).Enabled = enable
                End If

                If TypeOf ctrl Is DropDownList Then
                    DirectCast(ctrl, DropDownList).Enabled = enable
                End If

                'If TypeOf ctrl Is CheckBox Then
                '    DirectCast(ctrl, CheckBox).Enabled = enable
                'End If

                'If TypeOf ctrl Is AjaxControlToolkit.ComboBox Then
                '    DirectCast(ctrl, AjaxControlToolkit.ComboBox).Enabled = enable
                'End If

                ' You could do this in an else but incase you want to affect controls
                ' like Panels, you could check every control for nested controls
                If ctrl.Controls.Count > 0 Then
                    ' Use recursion to find all nested controls
                    EnableControls(ctrl, enable)
                End If
            Next
        End Sub


        Public Sub LoopTruControls(controlCollection As ControlCollection, xVal As Boolean)
            'Dim txt As TextBox = Nothing
            'Dim drp As DropDownList = Nothing
            'Dim cbo As AjaxControlToolkit.ComboBox = Nothing

            For Each ctl As Control In controlCollection
                If TypeOf ctl Is TextBox Then
                    DirectCast(ctl, TextBox).Enabled = xVal
                End If

                If TypeOf ctl Is DropDownList Then
                    DirectCast(ctl, DropDownList).Enabled = xVal
                End If

                'If TypeOf ctl Is AjaxControlToolkit.ComboBox Then
                '    DirectCast(ctl, AjaxControlToolkit.ComboBox).Enabled = xVal
                'End If

                If TypeOf ctl Is CheckBox Then
                    DirectCast(ctl, CheckBox).Enabled = xVal
                End If


                If ctl.Controls IsNot Nothing Then
                    LoopTruControls(ctl.Controls, xVal)
                End If
            Next
        End Sub


    End Class

End Namespace

