using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Web.UI;
using System.Web.Services;
using System.Data.SqlClient;
using System.Web;

namespace WebApplication_BookHub
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string eventTarget = Request["__EVENTTARGET"];
                string eventArgument = Request["__EVENTARGUMENT"];

                if (eventTarget == "favoriteBook")
                {
                    favoriteBook(eventArgument);
                }
            }

            if (!IsPostBack)
            {
                var username = Session["Username"];
                if (username != null)
                {
                    lblWelcome.Text = "Welcome, " + Session["Username"].ToString() + "!";
                    string profilePicturePath = Db.GetUserProfilePicture(username.ToString());
                    if (!string.IsNullOrEmpty(profilePicturePath))
                    {
                        imgProfilePic.ImageUrl = profilePicturePath;
                    }
                }
                else
                {
                    Response.Redirect("Login.aspx");
                }
            }
        }

        protected async void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string query = txtSearch.Text.Trim();

                if (!string.IsNullOrEmpty(query))
                {
                    await SearchBooks(query);
                }
                else
                {
                    results.InnerHtml = "Please enter a search term.";
                }
            }
            catch (Exception ex)
            {
                results.InnerHtml = "An error occurred: " + ex.Message;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("Login.aspx");
        }

        protected void btnViewProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("Profile.aspx");
        }

        protected async void btnApplyFilters_Click(object sender, EventArgs e)
        {
            try
            {
                string query = txtSearch.Text.Trim();
                string author = txtAuthorFilter.Text.Trim();
                string minPriceText = txtMinPrice.Text.Trim();
                string maxPriceText = txtMaxPrice.Text.Trim();

                decimal? minPrice = string.IsNullOrEmpty(minPriceText) ? (decimal?)null : decimal.Parse(minPriceText);
                decimal? maxPrice = string.IsNullOrEmpty(maxPriceText) ? (decimal?)null : decimal.Parse(maxPriceText);

                if (!string.IsNullOrEmpty(query))
                {
                    await SearchBooks(query, author, minPrice, maxPrice);
                }
                else
                {
                    results.InnerHtml = "Please enter a search term.";
                }
            }
            catch (Exception ex)
            {
                results.InnerHtml = "An error occurred: " + ex.Message;
            }
        }

        private async Task SearchBooks(string query, string author = null, decimal? minPrice = null, decimal? maxPrice = null)
        {
            try
            {
                string apiUrl = $"https://www.googleapis.com/books/v1/volumes?q={query}";

                using (WebClient client = new WebClient())
                {
                    string response = await client.DownloadStringTaskAsync(apiUrl);

                    dynamic data = JsonConvert.DeserializeObject(response);

                    results.InnerHtml = "";
                    List<string> boughtBooks = Session["BoughtBooks"] as List<string>;
                    List<string> favoriteBooks = Session["FavoriteBooks"] as List<string>;

                    if (boughtBooks == null)
                    {
                        boughtBooks = new List<string>();
                        Session["BoughtBooks"] = boughtBooks;
                    }

                    if (favoriteBooks == null)
                    {
                        favoriteBooks = new List<string>();
                        Session["FavoriteBooks"] = favoriteBooks;
                    }

                    foreach (var item in data.items)
                    {
                        string title = item.volumeInfo.title;
                        string authors = string.Join(", ", item.volumeInfo.authors);
                        string description = item.volumeInfo.description;
                        string thumbnailUrl = item.volumeInfo.imageLinks?.thumbnail ?? "https://via.placeholder.com/150";
                        string priceInfo = await GetPriceInINR(item);
                        decimal? price = GetPrice(item);

                        bool matchesFilters = true;

                        if (!string.IsNullOrEmpty(author) && !authors.ToLower().Contains(author.ToLower()))
                        {
                            matchesFilters = false;
                        }

                        if (minPrice.HasValue && price.HasValue && price.Value < minPrice.Value)
                        {
                            matchesFilters = false;
                        }

                        if (maxPrice.HasValue && price.HasValue && price.Value > maxPrice.Value)
                        {
                            matchesFilters = false;
                        }

                        if (matchesFilters)
                        {
                            string isbn = item.volumeInfo.industryIdentifiers[0].identifier;
                            bool isBought = boughtBooks.Contains(isbn);
                            bool isFavorite = favoriteBooks.Contains(isbn);

                            string buyButtonHtml = isBought ? "<p><strong>You already own this book</strong></p>" : $"<button onclick='buyBook(\"{isbn}\", \"{title}\", \"{authors}\", \"{thumbnailUrl}\", \"{priceInfo}\")'>Buy Now</button>";
                            string favoriteButtonHtml = isFavorite ? "<p><strong>Book is in favorites</strong></p>" : $"<button onclick='favoriteBook(\"{isbn}\")'>Add to Favorites</button>";
                            string ratingHtml = GenerateRatingHtml(isbn);

                            // Adding a simple review form
                            string reviewFormHtml = $@"
                                <div class='review-form'>
                                    <form method='post' action='SubmitReview.aspx'>
                                        <label for='review'>Your Review:</label>
                                        <textarea name='review' rows='3' cols='30'></textarea>
                                        <input type='hidden' name='isbn' value='{isbn}' />
                                        <button type='submit'>Submit Review</button>
                                    </form>
                                </div>";

                            string bookHtml = $@"
                                <div class='book-container' style='border: 1px solid #ccc; padding: 10px; margin-bottom: 10px;'>
                                    <div class='book-info'>
                                        <img src='{thumbnailUrl}' style='float: left; margin-right: 10px;' width='100' height='150' />
                                        <div>
                                            <h3>{title}</h3>
                                            <p><strong>Authors:</strong> {authors}</p>
                                            <p><strong>Description:</strong> {description}</p>
                                            <p><strong>Price:</strong> {priceInfo}</p>
                                            {buyButtonHtml}
                                            {favoriteButtonHtml}
                                            {ratingHtml}
                                            {reviewFormHtml}
                                        </div>
                                        <div style='clear: both;'></div>
                                    </div>
                                    <div class='metadata-info' style='display: none;'>
                                        <p><strong>ISBN:</strong> {isbn}</p>
                                        <p><strong>Published Date:</strong> {item.volumeInfo.publishedDate}</p>
                                    </div>
                                </div>";

                            results.InnerHtml += bookHtml;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                results.InnerHtml = "An error occurred: " + ex.Message;
            }
        }

        [WebMethod]
        public static void RateBook(string isbn, int rating)
        {
            string username = HttpContext.Current.Session["Username"].ToString();

            string connectionString = "ConfigurationManager.ConnectionStrings[\"BookHubDB\"].ConnectionString"; // Update with your connection string
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "INSERT INTO reviews (book_id, username, rating, created_at) VALUES (@book_id, @username, @rating, @created_at)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@book_id", isbn);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@rating", rating);
                cmd.Parameters.AddWithValue("@created_at", DateTime.Now);
                cmd.ExecuteNonQuery();
            }
        }

        private async Task<string> GetPriceInINR(dynamic item)
        {
            try
            {
                if (item.saleInfo?.retailPrice != null)
                {
                    string currencyCode = item.saleInfo.retailPrice.currencyCode;
                    decimal price = item.saleInfo.retailPrice.amount;

                    if (currencyCode != "INR")
                    {
                        decimal priceInINR = await ConvertToINR(price, currencyCode);
                        return $"{priceInINR:F2}";
                    }
                    else
                    {
                        return $"{price:F2}";
                    }
                }
                else
                {
                    return "300";
                }
            }
            catch (Exception ex)
            {
                return "Price not available";
            }
        }

        private async Task<decimal> ConvertToINR(decimal amount, string fromCurrency)
        {
            string apiUrl = $"https://api.exchangerate-api.com/v4/latest/{fromCurrency}";

            using (WebClient client = new WebClient())
            {
                string response = await client.DownloadStringTaskAsync(apiUrl);
                dynamic data = JsonConvert.DeserializeObject(response);

                decimal conversionRate = data.rates.INR;
                return amount * conversionRate;
            }
        }

        private decimal? GetPrice(dynamic item)
        {
            if (item.saleInfo?.retailPrice != null)
            {
                return item.saleInfo.retailPrice.amount;
            }
            return null;
        }

        private string GenerateRatingHtml(string isbn)
        {
            string ratingHtml = "<div class='rating'>Rate this book: ";
            for (int i = 1; i
 <= 5; i++)
            {
                ratingHtml += $"<span style='cursor:pointer;' onclick='rateBook(\"{isbn}\", {i})'>{i}★</span> ";
            }
            ratingHtml += "</div>";

            return ratingHtml;
        }

        protected void btnViewBoughtBooks_Click(object sender, EventArgs e)
        {
            List<string> boughtBooks = Session["BoughtBooks"] as List<string>;
            if (boughtBooks != null && boughtBooks.Count > 0)
            {
                string boughtBooksHtml = "<h3>Bought Books</h3><ul>";
                foreach (string isbn in boughtBooks)
                {
                    boughtBooksHtml += $"<li>{isbn}</li>";
                }
                boughtBooksHtml += "</ul>";
                results.InnerHtml = boughtBooksHtml;
            }
            else
            {
                results.InnerHtml = "You haven't bought any books yet.";
            }
        }

        protected void favoriteBook(string isbn)
        {
            List<string> favoriteBooks = Session["FavoriteBooks"] as List<string>;
            if (favoriteBooks == null)
            {
                favoriteBooks = new List<string>();
                Session["FavoriteBooks"] = favoriteBooks;
            }

            if (!favoriteBooks.Contains(isbn))
            {
                favoriteBooks.Add(isbn);
            }
            Response.Redirect("Profile.aspx");
        }

        protected void buyBook(string isbn, string title, string authors, string thumbnailUrl, string priceInfo)
        {
            List<string> boughtBooks = Session["BoughtBooks"] as List<string>;
            if (boughtBooks == null)
            {
                boughtBooks = new List<string>();
                Session["BoughtBooks"] = boughtBooks;
            }
            boughtBooks.Add(isbn);

            Response.Redirect($"Payment.aspx?isbn={isbn}&title={title}&authors={authors}&thumbnailUrl={thumbnailUrl}&price={priceInfo}");
        }
    }
}
