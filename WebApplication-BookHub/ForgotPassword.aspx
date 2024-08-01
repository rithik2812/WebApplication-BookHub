<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="WebApplication_BookHub.ForgotPassword" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Forgot Password</title>
    <style>
        body {
            font-family: 'Arial', sans-serif;
            background: url('https://w0.peakpx.com/wallpaper/127/366/HD-wallpaper-books-on-bookshelf.jpg') no-repeat center center fixed;
            background-size: cover;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
        }
        form {
            background-color: #ffffff;
            padding: 20px;
            border-radius: 10px;
            width: 300px;
        }
        h1 {
            color: #333;
            text-align: center;
        }
        label {
            display: block;
            margin-bottom: 8px;
            font-weight: bold;
        }
        input[type="text"], input[type="password"] {
            width: calc(100% - 24px);
            padding: 10px;
            margin-bottom: 15px;
            border: 1px solid #ccc;
            border-radius: 5px;
        }
        .btn {
            width: 100%;
            padding: 10px;
            border: none;
            border-radius: 5px;
            background-color: #007bff;
            color: #ffffff;
            font-size: 16px;
            cursor: pointer;
            margin-bottom: 10px;
        }
        .btn:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Forgot Password</h1>
        <div>
            <asp:Label Text="Username:" runat="server" AssociatedControlID="txtUsername"></asp:Label>
            <asp:TextBox ID="txtUsername" runat="server" Placeholder="Enter your username"></asp:TextBox>
            <br />
            <asp:Label Text="New Password:" runat="server" AssociatedControlID="txtNewPassword"></asp:Label>
            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" Placeholder="Enter new password"></asp:TextBox>
            <br />
            <asp:Label Text="Confirm Password:" runat="server" AssociatedControlID="txtConfirmPassword"></asp:Label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" Placeholder="Confirm new password"></asp:TextBox>
            <br />
            <asp:Button ID="btnResetPassword" Text="Reset Password" CssClass="btn" OnClick="btnResetPassword_Click" runat="server" />
        </div>
    </form>
</body>
</html>
