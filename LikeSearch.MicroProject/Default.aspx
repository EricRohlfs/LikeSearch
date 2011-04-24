<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Samples.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      Display Name: 
        <asp:TextBox ID="DisplayName" runat="server"></asp:TextBox>
       <br />
        <asp:Button ID="ButtonSearch" runat="server" Text="Button" 
            onclick="ButtonSearch_Click" />
      
        <asp:Literal ID="LiteralGrid1" runat="server"></asp:Literal>
    </div>
    </form>
</body>
</html>
