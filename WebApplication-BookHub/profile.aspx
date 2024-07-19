<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="WebApplication_BookHub.Profile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Profile</title>
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
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            width: 400px;
        }
        h1 {
            color: #333;
            text-align: center;
        }
        .form-group {
            margin-bottom: 15px;
        }
        .form-group label {
            display: block;
            margin-bottom: 5px;
            color: #555;
        }
        .form-group input,
        .form-group button {
            width: 100%;
            padding: 10px;
            box-sizing: border-box;
            border: 1px solid #ddd;
            border-radius: 4px;
            margin-bottom: 10px;
        }
        .form-group input:focus,
        .form-group button:focus {
            border-color: #007bff;
            outline: none;
        }
        .form-group button {
            background: #007bff;
            color: #fff;
            border: none;
            cursor: pointer;
        }
        .form-group button:hover {
            background: #0056b3;
        }
        .profile-picture {
            text-align: center;
            margin-bottom: 20px;
        }
        .profile-picture img {
            border-radius: 50%;
            border: 2px solid #ddd;
        }
        .actions {
            display: flex;
            justify-content: space-between;
        }
        .actions button {
            width: 48%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <h1>Profile</h1>
        <div class="profile-picture">
            <asp:Image ID="imgProfilePic" runat="server" Width="150px" Height="150px" />
            <br />
            <asp:FileUpload ID="fileUploadProfilePic" runat="server" />
            <asp:Button ID="btnUploadProfilePic" runat="server" Text="Upload Picture" OnClick="btnUploadProfilePic_Click" />
        </div>
        <div class="form-group">
            <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
            <asp:Button ID="btnUpdateEmail" runat="server" Text="Update Email" OnClick="btnUpdateEmail_Click" />
        </div>
        <div class="form-group">
            <asp:Label ID="lblCurrentPassword" runat="server" Text="Current Password:"></asp:Label>
            <asp:TextBox ID="txtCurrentPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Label ID="lblNewPassword" runat="server" Text="New Password:"></asp:Label>
            <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Label ID="lblConfirmNewPassword" runat="server" Text="Confirm New Password:"></asp:Label>
            <asp:TextBox ID="txtConfirmNewPassword" runat="server" TextMode="Password"></asp:TextBox>
            <asp:Button ID="btnUpdatePassword" runat="server" Text="Update Password" OnClick="btnUpdatePassword_Click" />
            <asp:Label ID="lblChangePasswordStatus" runat="server" Text=""></asp:Label>
        </div>
        <div class="actions">
            <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
            <asp:Button ID="btnDashboard" runat="server" Text="Dashboard" OnClick="btnDashboard_Click" />
        </div>
        <br />
        <asp:Literal ID="favorites" runat="server"></asp:Literal>
    </form>
</body>
</html>
