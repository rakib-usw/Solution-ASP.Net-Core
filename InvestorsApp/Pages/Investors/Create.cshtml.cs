using InvestorsApp.Pages.Investors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;


namespace InvestorsApp.Pages.Investors
{
	public class CreateModel : PageModel
	{
		public InvestorInfo InvestorInfo = new InvestorInfo();
		public String errorMessage = "";
		public String successMessage = "";

		public void OnGet()
		{
		}
		public void OnPost()
		{
			InvestorInfo.company_name = Request.Form["company_name"];
			InvestorInfo.company_code = Request.Form["company_code"];
			InvestorInfo.share_price = Request.Form["share_price"];
			


			if (InvestorInfo.company_name.Length == 0 ||
				InvestorInfo.company_code.Length == 0 )
			{
				errorMessage = "You myst enter company name and company code";
				return;
			}
			
			if (System.Text.RegularExpressions.Regex.IsMatch(InvestorInfo.company_code, "^[a-zA-Z0-9\x20]+$"))
			{
				// Good-to-go
			}
			else
			{
				errorMessage = "Only Alpha Numeric acceptable for company code. ex (a-zA-Z0-9)";
				return;
			}
			if (InvestorInfo.share_price.Length == 0)
			{
				InvestorInfo.share_price = "0";
			}

			if (System.Text.RegularExpressions.Regex.IsMatch(InvestorInfo.share_price, "^0$|^[1-9]\\d*$|^\\.\\d+$|^0\\.\\d*$|^[1-9]\\d*\\.\\d*$"))
			{
				// Good-to-go
			}
			else
			{
				errorMessage = "Only Numeric acceptable (0-9)";
				return;
			}

			{
				try
				{
					string ConnectionString = "Data Source=.\\sqlexpress;Initial Catalog=downing;Integrated Security=True";

					using (SqlConnection connection = new SqlConnection(ConnectionString))
					{
						connection.Open();
						String sql = "INSERT INTO investors " +
					   "(company_name, company_code, share_price) VALUES" +
					   "(@company_name, @company_code, @share_price);";

						using (SqlCommand command = new SqlCommand(sql, connection))
						{
							command.Parameters.AddWithValue("company_name", InvestorInfo.company_name);
							command.Parameters.AddWithValue("company_code", InvestorInfo.company_code.ToUpper());
							command.Parameters.AddWithValue("share_price", InvestorInfo.share_price);
							command.ExecuteNonQuery();
						}
					}

				}
				catch (Exception ex)
				{
					errorMessage = ex.Message;
					return;

				}

				InvestorInfo.company_name = "";
				InvestorInfo.company_code = "";
				InvestorInfo.share_price = "";

				successMessage = "New Company Addedd Sucessfully!";
				Response.Redirect("/Investors/Index");

			}
		}

	}
}

