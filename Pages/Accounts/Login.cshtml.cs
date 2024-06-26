using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using toshokan.Data;
using toshokan.Models;

namespace toshokan.Pages.Accounts
{
    public class LoginModel : PageModel
    {
        private readonly toshokanContext _context;
        public LoginModel(toshokanContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Member Members { get; set; }
        public class Member
        {
            [StringLength(40)]
            [Required(ErrorMessage = "Username is required.")]
            public string Username { get; set; }

            [StringLength(15)]
            [Required(ErrorMessage = "Password is required.")]
            [DataType(DataType.Password)]
            public string Password { get; set; }

        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var member = _context.Member.FirstOrDefault(m => m.Username == Members.Username && m.Password == Members.Password);
            if(member == null)
            {
                ModelState.AddModelError(string.Empty, "Incorrect username or password!");
                return Page();
                
            }
            else
            {
                HttpContext.Session.SetString("isLoggedIn", "true");
                HttpContext.Session.SetString("Username", member.Username);
                HttpContext.Session.SetInt32("UserID", member.MemberID);
                return RedirectToPage("/Index");
            }
        }
        public IActionResult onGet()
        {
            if(HttpContext.Session.GetString("isLoggedIn") == "true")
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
