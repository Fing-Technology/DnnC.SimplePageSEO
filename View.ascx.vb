' --- Copyright (c) notice DnnConsulting.nl ---
'  Copyright (c) 2014 DnnConsulting.nl.  www.DnnConsulting.nl. BSD License.
' Author: G. M. Barlow
' ------------------------------------------------------------------------
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' ------------------------------------------------------------------------
' This copyright notice may NOT be removed, obscured or modified without written consent from the author.
' --- End copyright notice --- 

Imports DotNetNuke.UI.Modules
Imports DotNetNuke.Services.Localization
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Common.Globals
Imports DotNetNuke.Entities
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.FileSystem
Imports DotNetNuke.Entities.Tabs
Imports System

Partial Class View
    Inherits SimplePageSeoModuleBase

#Region " Event Methods "

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                BindPages()

                If seoTabId > 0 Then
                    FillPageData()
                End If

            End If
        Catch exc As Exception
            panelMainError.Visible = True
            lblInputError.Text = Localization.GetString("lblMainError", Me.LocalResourceFile) & " " & exc.Message
            Exceptions.ProcessModuleLoadException(Me, exc)
        End Try
    End Sub

    Private Sub cmdCancelTab_Click(sender As Object, e As EventArgs) Handles cmdCancelTab.Click
        Response.Redirect(NavigateURL())
    End Sub

    Private Sub cmdSaveTab_Click(sender As Object, e As EventArgs) Handles cmdSaveTab.Click
        Dim ctrlTab As TabController = New TabController()
        Dim intTab As Integer = Convert.ToInt32(hdTabId.Value)
        Dim objTab As TabInfo = ctrlTab.GetTab(intTab, PortalId, True)

        objTab.Title = CleanText(txtTitle.Text)
        objTab.Description = CleanText(txtDescription.Text)
        objTab.KeyWords = CleanText(txtKeyWords.Text)

        Try
            ctrlTab.UpdateTab(objTab)
            DotNetNuke.Common.Utilities.DataCache.ClearTabsCache(PortalId)
            Response.Redirect(NavigateURL())
        Catch ex As Exception
            panelInputError.Visible = True
            lblInputError.Text = Localization.GetString("lblInputError", Me.LocalResourceFile) & " " & ex.Message
        End Try
    End Sub

#End Region

#Region "Private methods"

    Private Sub BindPages()
        Dim dt As New DataTable
        dt.Columns.Add("TabId", GetType(Integer))
        dt.Columns.Add("TabName", GetType(String))
        dt.Columns.Add("IndentedTabName", GetType(String))
        dt.Columns.Add("TabLevel", GetType(Integer))
        dt.Columns.Add("HasTitle", GetType(Boolean))
        dt.Columns.Add("HasDescrition", GetType(Boolean))
        dt.Columns.Add("HasKeyWords", GetType(Boolean))

        Dim sTabs As New List(Of TabInfo)
        sTabs = TabController.GetPortalTabs(PortalId, -1, False, True, False, True)

        Dim adminTabId As Integer = 0
        Dim socialTabId As Integer = 0

        For Each pTab As TabInfo In sTabs
            If LCase(pTab.TabName) = "admin" Then adminTabId = pTab.TabID
            If LCase(pTab.TabName) = "activity feed" Then socialTabId = pTab.TabID

            If Not pTab.TabID = adminTabId And Not pTab.ParentId = adminTabId Then
                Dim dr As DataRow = dt.NewRow
                dr("TabId") = pTab.TabID
                dr("TabName") = pTab.TabName
                dr("IndentedTabName") = pTab.IndentedTabName
                dr("TabLevel") = pTab.Level
                dr("TabName") = pTab.TabName
                If pTab.Title = String.Empty Then dr("HasTitle") = False Else dr("HasTitle") = True
                If pTab.Description = String.Empty Then dr("HasDescrition") = False Else dr("HasDescrition") = True
                If pTab.KeyWords = String.Empty Then dr("HasKeyWords") = False Else dr("HasKeyWords") = True
                dt.Rows.Add(dr)
            End If
        Next

        grdTabs.DataSource = dt
        grdTabs.DataBind()
    End Sub

    Private Sub FillPageData()
        Dim ctrlTab As New TabController
        Dim objTab As TabInfo = ctrlTab.GetTab(CInt(seoTabId), PortalId, True)

        hdTabId.Value = objTab.TabID
        lblHeaderTitle.Text = Localization.GetString("lblActivePage", Me.LocalResourceFile) & " " & objTab.TabName
        txtTitle.Text = objTab.Title
        txtDescription.Text = objTab.Description
        txtKeyWords.Text = objTab.KeyWords

        panelInput.Visible = True
    End Sub

    Private Function CleanText(str As String) As String
        Dim cleanStr As String = String.Empty
        cleanStr = Replace(str, "'", "")

        Return cleanStr
    End Function

#End Region

#Region " DataGrid Methods"

    Private Sub grdTabs_ItemCommand(source As Object, e As DataGridCommandEventArgs) Handles grdTabs.ItemCommand
        If e.CommandName.ToLower = "cmdedit" Then
            Response.Redirect(NavigateURL(Me.TabId, "", "mid=" & ModuleId, "tId=" & e.CommandArgument))
        End If
    End Sub

    Private Sub grdTabs_ItemDataBound(sender As Object, e As DataGridItemEventArgs) Handles grdTabs.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim imgTitle As Image = DirectCast(e.Item.FindControl("imgTitle"), Image)
            Dim imgDesc As Image = DirectCast(e.Item.FindControl("imgDesc"), Image)
            Dim imgKeys As Image = DirectCast(e.Item.FindControl("imgKeys"), Image)

            If Not e.Item.DataItem("HasTitle") Then
                imgTitle.ImageUrl = ControlPath & "images/title_empty.png"
                imgTitle.Attributes.Add("Title", Localization.GetString("attTitleEmpty", Me.LocalResourceFile))
            Else
                imgTitle.ImageUrl = ControlPath & "images/title.png"
                imgTitle.Attributes.Add("Title", Localization.GetString("attTitle", Me.LocalResourceFile))
            End If

            If Not e.Item.DataItem("HasDescrition") Then
                imgDesc.ImageUrl = ControlPath & "images/description_empty.png"
                imgDesc.Attributes.Add("Title", Localization.GetString("attDescEmpty", Me.LocalResourceFile))
            Else
                imgDesc.ImageUrl = ControlPath & "images/description.png"
                imgDesc.Attributes.Add("Title", Localization.GetString("attDesc", Me.LocalResourceFile))
            End If

            If Not e.Item.DataItem("HasKeyWords") Then
                imgKeys.ImageUrl = ControlPath & "images/keywords_empty.png"
                imgKeys.Attributes.Add("Title", Localization.GetString("attKeysEmpty", Me.LocalResourceFile))
            Else
                imgKeys.ImageUrl = ControlPath & "images/keywords.png"
                imgKeys.Attributes.Add("Title", Localization.GetString("attKeys", Me.LocalResourceFile))
            End If

        End If
    End Sub

#End Region

End Class