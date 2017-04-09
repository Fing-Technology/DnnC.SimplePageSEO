<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="View.ascx.vb" Inherits="DnnC.Modules.SimplePageSeo.View" %>
<%@ Register TagPrefix="dnn" TagName="label" Src="~/controls/LabelControl.ascx" %>

<div>

    <div class="errorPanel">
        <asp:Panel ID="panelMainError" runat="server" Visible="false">
            <div class="dnnFormItem">
                <div class="dnnFormMessage dnnFormValidationSummary">
                    <asp:Label ID="lblMainError" runat="server" />
                </div>
            </div>
        </asp:Panel>
    </div>

    <!-- Begin Page list -->
    <div style="float:left; width:30%;">        
        <asp:datagrid id="grdTabs" 
            DataKeyField="TabId"
            Width="98%" 
            AutoGenerateColumns="false" 
            runat="server" 
            BorderStyle="None" 
            GridLines="None" 
            ShowHeader="false" 
            ShowFooter="false" 
            CssClass="dnnGrid" >

            <AlternatingItemStyle  CssClass="dnnGridAltItem" />
            <ItemStyle  CssClass="dnnGridItem" />

            <columns>

                <asp:templatecolumn ItemStyle-Width="15px" ItemStyle-HorizontalAlign="Center">
                    <itemtemplate>
                        <asp:Image ID="imgTitle" runat="server" />
                    </itemtemplate>
                </asp:templatecolumn>

                <asp:templatecolumn ItemStyle-Width="15px" ItemStyle-HorizontalAlign="Center">
                    <itemtemplate>
                        <asp:Image ID="imgDesc" runat="server" />
                    </itemtemplate>
                </asp:templatecolumn>

                <asp:templatecolumn ItemStyle-Width="15px" ItemStyle-HorizontalAlign="Center">
                    <itemtemplate>
                        <asp:Image ID="imgKeys" runat="server" />
                    </itemtemplate>
                </asp:templatecolumn>

                <asp:templatecolumn>
                    <itemtemplate>
                        <asp:Label ID="lblTabTitle" runat="server" Text='<%# Eval("IndentedTabName")%>'></asp:Label>
                    </itemtemplate>
                </asp:templatecolumn>              

                <asp:templatecolumn ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                    <ItemStyle Wrap="False"></ItemStyle>
                    <itemtemplate>
                        <asp:ImageButton ID="cmdEditItem" runat="server" CommandName="cmdEdit" CommandArgument='<%# Eval("TabId")%>' ImageUrl="~/DesktopModules/DnnC_SimplePageSeo/images/edit.png" />
                    </itemtemplate>
                </asp:templatecolumn>

            </columns>
        </asp:datagrid>        
    </div><!-- End Page list -->

    <!-- Begin page data input -->
    <div style="float:right; width:65%;">
        <asp:Panel ID="panelInput" runat="server" Visible="false">
            <asp:HiddenField ID="hdTabId" runat="server" />

            <div class="hdPageName">
                <h1><asp:Label ID="lblHeaderTitle" runat="server" /></h1>
                <hr />
            </div>

            <div class="dnnForm">

                <asp:Panel ID="panelInputError" runat="server" Visible="false">
                    <div class="dnnFormItem">
                        <div class="dnnFormMessage dnnFormValidationSummary">
                            <asp:Label ID="lblInputError" runat="server" />
                        </div>
                    </div>
                </asp:Panel>


                <div class="dnnFormItem">
                    <dnn:Label ID="plTitle" runat="server" ResourceKey="plTitle" Suffix=":" ControlName="txtTitle"/>
                    <asp:TextBox ID="txtTitle" CssClass="NormalTextBox" runat="server" MaxLength="200" Width="100%"/>
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="plDescription" runat="server" ResourceKey="plDescription" Suffix=":" ControlName="txtDescription"/>
                    <asp:TextBox ID="txtDescription" CssClass="NormalTextBox" runat="server" MaxLength="255" Width="100%" TextMode="MultiLine" Rows="4"/>
                </div>

                <div class="dnnFormItem">
                    <dnn:Label ID="plKeywords" runat="server" ResourceKey="plKeywords" Suffix=":" ControlName="txtKeyWords"/>
                    <asp:TextBox ID="txtKeyWords" CssClass="NormalTextBox" runat="server" MaxLength="255" Width="100%" TextMode="MultiLine" Rows="4"/>
                </div>

            </div>

            <!-- Begin command buttons -->
            <ul class="dnnActions dnnClear">
	            <li><asp:LinkButton ID="cmdSaveTab" runat="server" resourcekey="cmdSaveTab" CssClass="dnnPrimaryAction" ValidationGroup="tabs" /></li>
                <li><asp:LinkButton ID="cmdCancelTab" runat="server" resourcekey="cmdCancel" CssClass="dnnSecondaryAction" CausesValidation="false" /></li>
            </ul><!-- End command buttons -->

        </asp:Panel>
    </div><!-- Begin page data input -->

</div>