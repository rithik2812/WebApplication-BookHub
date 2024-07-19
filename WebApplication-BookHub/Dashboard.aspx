<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="WebApplication_BookHub.Dashboard" Async="true" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BookHub Dashboard</title>
    <style>
        body {
            background: url('https://img.freepik.com/premium-photo/light-bulb-yellow-books-yellow-background-with-copy-space-3d-illustration_856223-1441.jpg') no-repeat center center fixed; 
            background-size: cover;
        }
        .top-right {
            position: absolute;
            top: 10px;
            right: 10px;
        }
        .profile-icon {
            width: 80px;
            height: 80px;
            border-radius: 40px;
            background: url('https://www.kindpng.com/picc/m/171-1712282_profile-icon-png-profile-icon-vector-png-transparent.png') no-repeat;
            background-size: cover;
        }
        .book-container {
            background-color: white;
            border: 1px solid #ccc;
            padding: 10px;
            margin-bottom: 10px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }
        .book-info {
            overflow: hidden;
        }
        .book-info img {
            float: left;
            margin-right: 10px;
            width: 100px;
            height: 150px;
        }
        .buy-button {
            margin-top: 10px;
            padding: 5px 10px;
            background-color: #4CAF50;
            color: white;
            border: none;
            cursor: pointer;
        }
        .buy-button:hover {
            background-color: #45a049;
        }
        .dashboard-title {
            color: aqua;
            font-weight: bold;
            font-size: 32px;
        }
        .welcome-message {
            color: aqua;
            font-weight: bold;
            font-size: 24px;
        }
    </style>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script>
        function buyBook(isbn, title, authors, thumbnailUrl, priceInfo) {
            var confirmPurchase = confirm(`Buy ${title} for ${priceInfo}?`);
            if (confirmPurchase) {
                $.ajax({
                    type: "POST",
                    url: "Dashboard.aspx/RateBook",
                    data: JSON.stringify({ isbn: isbn, rating: rating }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        alert("Rating submitted successfully!");
                    },
                    failure: function (response) {
                        alert("Failed to submit rating.");
                    }
                });
            }
        }

        function addToFavorites(isbn, title, authors, thumbnailUrl, priceInfo) {
            $.ajax({
                type: "POST",
                url: "Dashboard.aspx/AddToFavorites",
                data: JSON.stringify({ isbn: isbn, title: title, authors: authors, thumbnailUrl: thumbnailUrl, priceInfo: priceInfo }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    alert(response.d);
                },
                error: function (error) {
                    alert("Failed to add to favorites: " + error.responseJSON.Message);
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1 class="dashboard-title">BookHub Dashboard</h1>
            <div class="top-right">
                <a href="Profile.aspx">
                    <asp:Image ID="imgProfilePic" runat="server" ImageUrl="https://www.kindpng.com/picc/m/171-1712282_profile-icon-png-profile-icon-vector-png-transparent.png" CssClass="profile-icon" /> 
                </a>
            </div>
            <asp:Label ID="lblWelcome" runat="server" Text="" CssClass="welcome-message"></asp:Label>
            <br />
            <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter book title or author"></asp:TextBox>
            <asp:Button ID="btnSearch" Text="Search" OnClick="btnSearch_Click" runat="server" />
            <asp:Button ID="btnLogout" Text="Logout" OnClick="btnLogout_Click" runat="server" />
            <br /><br />

            <asp:TextBox ID="txtAuthorFilter" runat="server" Placeholder="Filter by author"></asp:TextBox>
            <asp:TextBox ID="txtMinPrice" runat="server" Placeholder="Min Price"></asp:TextBox>
            <asp:TextBox ID="txtMaxPrice" runat="server" Placeholder="Max Price"></asp:TextBox>
            <asp:Button ID="btnApplyFilters" runat="server" Text="Apply Filters" OnClick="btnApplyFilters_Click" />
            <br /><br />

            <div id="results" runat="server"></div>

        </div>
    </form>
</body>
</html>
