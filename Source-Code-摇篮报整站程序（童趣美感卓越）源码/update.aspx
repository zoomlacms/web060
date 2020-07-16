<%@ Page Language="C#" AutoEventWireup="true" CodeFile="update.aspx.cs" Inherits="Tools_update" MasterPageFile="~/Common/Master/Empty.master" %>
<asp:Content runat="server" ContentPlaceHolderID="head"><title>站点升级</title></asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Content">
    <table class="table table-bordered table-striped">
        <tr><td><asp:Button runat="server" ID="NodeXmlToDB_Btn" CssClass="btn btn-info" Text="将节点从XML升级为数据库" OnClick="NodeXmlToDB_Btn_Click" OnClientClick="return confirm('确定要导入节点文件吗');"  /></td></tr>
        <tr><td><asp:Button runat="server" ID="ModelXmlToDB_Btn" CssClass="btn btn-info" Text="将模型从XML升级为数据库" OnClick="ModelXmlToDB_Btn_Click" OnClientClick="return confirm('确定要导入模型文件吗');" /></td></tr>
    </table>
</asp:Content>
<asp:Content runat="server" ContentPlaceHolderID="Script"></asp:Content>
