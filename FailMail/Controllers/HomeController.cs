using System.Threading.Tasks;
using System.Web.Mvc;
using FailMail.FailMail.Interfaces;

namespace FailMail.Controllers
{
    public class HomeController : Controller
    {
        private readonly IParseEmail _parseEmailRepository;

        private readonly ICreateIssue _createIssueRepository;

        public HomeController(IParseEmail parseEmailRepository, ICreateIssue createIssueRepository)
        {
            _parseEmailRepository = parseEmailRepository;

            _createIssueRepository = createIssueRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Parse an incoming email and create an issue from it
        /// </summary>
        /// <returns>Appropriate HTTP status code</returns>
        [HttpPost]
        public async Task<ActionResult> ParseEmail()
        {
            // Accept POSTed email
            var incomingEmail = _parseEmailRepository.ParseIncomingPostData(Request);

            // Create issue model from email
            var issue = _parseEmailRepository.FillIssueModel(incomingEmail);

            // Create the issue from the issue model
            var createdIssue = await _createIssueRepository.CreateIssue(issue);

            return createdIssue == true ? new HttpStatusCodeResult(202) : new HttpStatusCodeResult(400);
        }
    }
}
