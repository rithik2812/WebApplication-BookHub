<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication_BookHub.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
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
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
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
        .btn-register {
            background-color: #28a745;
        }
        .btn-register:hover {
            background-color: #218838;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>BookHub Login</h1>
        <div>
            <asp:Label Text="Username:" runat="server" AssociatedControlID="txtUsername"></asp:Label>
            <asp:TextBox ID="txtUsername" runat="server" Placeholder="Enter your username"></asp:TextBox>
            <br />
            <asp:Label Text="Password:" runat="server" AssociatedControlID="txtPassword"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Placeholder="Enter your password"></asp:TextBox>
            <br />
            <asp:Button ID="btnLogin" Text="Login" CssClass="btn" OnClick="btnLogin_Click" runat="server" />
            <asp:Button ID="btnRegister" Text="Register" CssClass="btn btn-register" OnClick="btnRegister_Click" runat="server" />
        </div>
    </form>
</body>
</html>
