<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebApplication_BookHub.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            background: url('https://w0.peakpx.com/wallpaper/127/366/HD-wallpaper-books-on-bookshelf.jpg') no-repeat center center fixed;
            background-size: cover;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
        }
        form {
            background: #ffffff;
            padding: 20px;
            border-radius: 8px;
            width: 300px;
            text-align: center;
        }
        h1 {
            color: #333;
            margin-bottom: 20px;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            color: #555;
        }
        .form-group input {
            width: 100%;
            padding: 8px;
            box-sizing: border-box;
            border: 1px solid #ddd;
            border-radius: 4px;
        }
        .form-group input:focus {
            border-color: #007bff;
            outline: none;
        }
        .form-group button {
            width: 100%;
            padding: 10px;
            background: #007bff;
            color: #fff;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 16px;
        }
        .form-group button:hover {
            background: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>BookHub Registration</h1>
        <div class="form-group">
            <asp:Label Text="Username:" runat="server" AssociatedControlID="txtUsername"></asp:Label>
            <asp:TextBox ID="txtUsername" runat="server" Placeholder="Enter your username"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label Text="Password:" runat="server" AssociatedControlID="txtPassword"></asp:Label>
            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Placeholder="Enter your password"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label Text="Confirm Password:" runat="server" AssociatedControlID="txtConfirmPassword"></asp:Label>
            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" Placeholder="Confirm your password"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Button ID="btnRegister" Text="Register" OnClick="btnRegister_Click" runat="server" />
        </div>
    </form>
</body>
</html>
