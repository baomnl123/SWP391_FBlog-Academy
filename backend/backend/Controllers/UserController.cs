using backend.DTO;
using backend.Handlers.IHandlers;
using backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Web;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly string url = "https://api.clerk.com/v1/users";
        private readonly string token = "sk_test_fc0dJDbMQTnTPbWYVy9c4v0LdhDSKNMH1U2YFukB3d";
        private readonly UserRoleConstrant _userRoleConstrant;
        private readonly IUserHandlers _userHandlers;
        private readonly IFollowUserHandlers _followUserHandlers;
        private readonly IUserMajorHandlers _userMajorHandlers;
        private readonly IUserSubjectHandlers _userSubjectHandlers;
        private readonly EmailSender _emailSender;
        private readonly HttpClient _httpClient = new();
        public UserController(IUserHandlers userHandlers, IFollowUserHandlers followUserHandlers, IUserMajorHandlers userMajorHandlers, IUserSubjectHandlers userSubjectHandlers)
        {
            _userHandlers = userHandlers;
            _followUserHandlers = followUserHandlers;
            _emailSender = new EmailSender();
            _userRoleConstrant = new();
            _userMajorHandlers = userMajorHandlers;
            _userSubjectHandlers = userSubjectHandlers;
        }

        /// <summary>
        /// Get list of enable Users. (Moderator | Lecturer | Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("all/enable")]
        public IActionResult getAllUsers()
        {
            var list = _userHandlers.GetAllUsers();
            if (list == null)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }
            return Ok(list);
        }

        /// <summary>
        /// Get list of disable Users. (Moderator | Lecturer | Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("all/disable")]
        public IActionResult getDisableUsers()
        {
            var list = _userHandlers.GetAllDisableUsers();
            if (list == null || list.Count == 0)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }
            return Ok(list);
        }

        [HttpGet("students")]
        public IActionResult GetStudents()
        {
            var getStudents = _userHandlers.GetStudents();
            if(getStudents == null || getStudents.Count == 0)
            {
                return Ok(new List<UserDTO>());
            }
            return Ok(getStudents);
        }

        /// <summary>
        /// Get list of Students and Moderators. (Lecturer | Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("students-and-moderators")]
        public IActionResult GetStudentsAndModerators()
        {
            var list = _userHandlers.GetStudentsAndModerator();
            if (list == null)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }
            return Ok(list);
        }

        /// <summary>
        /// Get list of Lecturers. (Admin)
        /// </summary>
        /// <returns></returns>
        [HttpGet("all/lecturers")]
        public IActionResult GetLecturers()
        {
            var list = _userHandlers.GetLecturers();
            if (list == null)
            {
                return Ok(new List<UserDTO>());
            }
            return Ok(list);
        }

        /// <summary>
        /// Get all banned Users
        /// </summary>
        /// <returns></returns>
        [HttpGet("banned")]
        public async Task<IActionResult> GetBannedResult()
        {
            var getUsers = _userHandlers.GetBannedUsers();
            if(getUsers == null || getUsers.Count == 0)
            {
                return Ok(new List<UserDTO>());
            }
            return Ok(getUsers);
        }
        /// <summary>
        /// Get selected User by his/her ID.
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("{userID}")]
        public IActionResult GetUser(int userID)
        {
            var user = _userHandlers.GetUser(userID);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Get selected User by his/her email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("email/{email}")]
        public IActionResult GetUserByEmail(string email)
        {
            var user = _userHandlers.GetUserByEmail(email);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        /// <summary>
        /// Get list of people that follow selected User. (Student | Moderator)
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        [HttpGet("{userID}/follower")]
        public IActionResult GetAllFollower(int currentUserID, int userID)
        {
            var listFollowers = _followUserHandlers.GetAllFollowerUsers(currentUserID, userID);

            if (listFollowers == null || listFollowers.Count == 0)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }

            return Ok(listFollowers);
        }

        /// <summary>
        /// Get list of people that selected User follows. (Student | Moderator)
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <returns></returns>
        [HttpGet("{currentUserID}/following")]
        public IActionResult GetAllFollowing(int currentUserID, int userID)
        {
            var listFollowings = _followUserHandlers.GetAllFollowingUsers(currentUserID, userID);

            if (listFollowings == null || listFollowings.Count == 0)
            {
                var emptyList = new List<UserDTO>();
                return Ok(emptyList);
            }

            return Ok(listFollowings);
        }

        /// <summary>
        /// Create a new User account. (Guest)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("student")]
        public async Task<IActionResult> CreateUser([FromForm] string name, [FromForm] string? avatarUrl, [FromForm] string email, [FromForm] string? password)
        {
            var user = _userHandlers.CreateUser(name, avatarUrl, email, password);
            if (user == null)
            {
                return BadRequest();
            }
            //send email
            var existedEmail = user.Email;
            var existedSubject = $"Your Account has been created !";
            var existedMessage = $"Your Account has been created ! You will now be able to access to FBlog Academy !\nWelcome {user.Name}!\n\nFaithfully,FBlog Academy";

            await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
            return Ok(user);
        }

        /// <summary>
        /// Create a new Lecturer account. (Admin)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost("lecturer")]
        public async Task<IActionResult> CreateLecturer([FromForm] string name, [FromForm] string? avatarUrl, [FromForm] string email, [FromForm] string password)
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.token}");
            var getUser = _userHandlers.GetUserByEmail(email);
            if (getUser != null)
            {
                if (getUser.Status) return BadRequest("Lecturer is existed !");
            }
            if(password.Length <= 8)
            {
                return BadRequest("Password must be over 8 characters");
            }

            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                first_name = name,
                email_address = new[] { email },
                password = password,
                skip_password_check = "true"
            }), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync(this.url, content);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response.Content);
            }
            var user = _userHandlers.CreateLecturer(name, avatarUrl, email, password);
            if (user == null)
            {
                return BadRequest();
            }

            //send email
            var existedEmail = user.Email;
            var existedSubject = $"Your Account has been created !";
            var existedMessage = $"Your Account has been created ! You will now be able to access to FBlog Academy !\nWelcome {user.Name}!\n\nFaithfully,FBlog Academy";

            await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
            return Ok(user);
        }

        /// <summary>
        /// Follow selected User. (Student | Moderator)
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPost("follow")]
        public async Task<IActionResult> Follow(int currentUserID, int userID)
        {
            var followRelationship = _followUserHandlers.FollowOtherUser(currentUserID, userID);
            if (followRelationship == null)
            {
                return BadRequest();
            }

            //send email
            var existedEmail = followRelationship.Followed.Email;
            var existedSubject = $"You have been Followed !";
            var existedMessage = $"{followRelationship.Follower.Name} has followed you !\n\nFaithfully,FBlog Academy";

            await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
            return Ok(followRelationship);
        }

        [HttpPost("{userID}/unban")]
        public async Task<IActionResult> UnbanUser(int userID)
        {
            var getUser = _userHandlers.GetBannedUser(userID);
            if (getUser == null)
            {
                return BadRequest("User is invalid in DB !");
            }
            if (getUser.Status)
            {
                return BadRequest("User is currently active !");
            }
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.token}");

            HttpResponseMessage response = await _httpClient.GetAsync($"{this.url}?limit=10&offset=0&email_address={HttpUtility.UrlEncode(getUser.Email)}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Don't have info in CLERK\n" + response.Content);
            }

            string jsonContent = await response.Content.ReadAsStringAsync();

            JArray jsonArray = JArray.Parse(jsonContent);

            string clerkUserID = string.Empty;
            foreach (JObject obj in jsonArray)
            {
                if (obj.TryGetValue("id", out JToken idToken))
                {
                    // 'id' property found, you can access its value
                    clerkUserID = idToken.Value<string>();
                }
            }

            if (clerkUserID.Equals(string.Empty))
            {
                return BadRequest("Don't have info in CLERK");
            }

            var content = new StringContent(JsonConvert.SerializeObject(new
            {
            }), Encoding.UTF8, "application/json");

            response = await _httpClient.PostAsync($"{this.url}/{clerkUserID}/unban",content);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Failed to unban\n" + response.Content);
            }

            var getUserDTO = _userHandlers.UnbanUser(userID);
            if(getUserDTO == null)
            {
                return BadRequest("DB Update Failed !");
            }

            //send email
            var existedEmail = getUserDTO.Email;
            var existedSubject = $"Your Account has been unbanned !";
            var existedMessage = $"Your Account has been unbanned !" +
                $"\n\nFaithfully,FBlog Academy";
            await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);

            return Ok(getUserDTO);
        }

        /// <summary>
        /// Update selected User.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="avatarUrl"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPut("{userID}")]
        public IActionResult UpdateUser(int userID, [FromForm] string? avatarUrl, [FromForm] string name, [FromForm] string? password)
        {
            var user = _userHandlers.UpdateUser(userID, name, avatarUrl, password);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        /// <summary>
        /// Promote Student to Moderator (Lecturer)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPut("{userID}/promote")]
        public async Task<IActionResult> PromoteStudent(int userID)
        {
            var user = _userHandlers.PromoteStudent(userID);
            if (user == null)
            {
                return BadRequest();
            }
            //send email
            var existedEmail = user.Email;
            var existedSubject = $"You have been promoted to Moderator !";
            var existedMessage = $"You have been promoted to Moderator ! Congratulations\nKeep supporting FBlog ! Thank you very much !\n\nFaithfully,FBlog Academy";

            await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
            return Ok(user);
        }

        /// <summary>
        /// Demote Moderator to Student (Lecturer)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPut("{userID}/demote")]
        public async Task<IActionResult> DemoteStudent(int userID)
        {
            var user = _userHandlers.DemoteStudent(userID);
            if (user == null)
            {
                return BadRequest();
            }
            //send email
            var existedEmail = user.Email;
            var existedSubject = $"You have been demoted to Student !";
            var existedMessage = $"You have been demoted to Student !" +
                $"\n\nFaithfully,FBlog Academy";

            await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
            return Ok(user);
        }

        /// <summary>
        /// Give award to selected Student. (Moderator | Lecturer)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpPut("{userID}/award")]
        public async Task<IActionResult> GiveAward(int userID)
        {
            var user = _userHandlers.GiveAward(userID);
            if (user == null || !user.Status)
            {
                return BadRequest();
            }
            //send email
            var existedEmail = user.Email;
            var existedSubject = $"You have been awarded !";
            var existedMessage = $"You have been awarded ! Congratulations" +
                $"\nYou will be able to post Blogs without censorship !\nKeep supporting FBlog ! Thank you very much !" +
                $"\n\nFaithfully,FBlog Academy";

            await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
            return Ok(user);
        }

        /// <summary>
        /// Remove Award from selected Student (Moderator | Lecturer)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpDelete("{userID}/award")]
        public async Task<IActionResult> RemoveAward(int userID)
        {
            var user = _userHandlers.RemoveAward(userID);
            if (user == null || !user.Status)
            {
                return BadRequest();
            }

            //send email
            var existedEmail = user.Email;
            var existedSubject = $"Your award has been removed !";
            var existedMessage = $"Your award has been removed !" +
                $"\n\nFaithfully,FBlog Academy";

            await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
            return Ok(user);
        }

        /// <summary>
        /// Delete/Ban selected User. (Admin)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpDelete("{userID}")]
        public async Task<IActionResult> deleteUser(int userID)
        {
            var getUser = _userHandlers.GetUser(userID);
            if (getUser == null || !getUser.Status)
            {
                return BadRequest();
            }
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {this.token}");

            HttpResponseMessage response = await _httpClient.GetAsync($"{this.url}?limit=10&offset=0&email_address={HttpUtility.UrlEncode(getUser.Email)}");

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(response.Content);
            }

            string jsonContent = await response.Content.ReadAsStringAsync();

            JArray jsonArray = JArray.Parse(jsonContent);

            string clerkUserID = string.Empty;
            foreach (JObject obj in jsonArray)
            {
                if (obj.TryGetValue("id", out JToken idToken))
                {
                    // 'id' property found, you can access its value
                    clerkUserID = idToken.Value<string>();
                }
            }

            if (clerkUserID.Equals(string.Empty))
            {
                return BadRequest();
            }

            if (getUser.Role.Equals(_userRoleConstrant.GetLecturerRole()))
            {
                response = await _httpClient.DeleteAsync($"{this.url}/{clerkUserID}");
            }
            else
            {
                var content = new StringContent(JsonConvert.SerializeObject(new
                {
                }), Encoding.UTF8, "application/json");

                response = await _httpClient.PostAsync($"{this.url}/{clerkUserID}/ban", content);
            }
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Call API FAILED !" + response.Content);
            }

            var user = _userHandlers.DisableUser(userID);
            if (user == null)
            {
                return BadRequest();
            }
            //send email
            var existedEmail = user.Email;
            var existedSubject = $"Your Account have been DELETED !";
            var existedMessage = $"Your Account have been DELETED !" +
                $"\n\nFaithfully,FBlog Academy";
            await _emailSender.SendEmailAsync(existedEmail, existedSubject, existedMessage);
            return Ok(user);
        }

        /// <summary>
        /// Unfollow selected User. (Student | Moderator)
        /// </summary>
        /// <param name="currentUserID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpDelete("follow")]
        public IActionResult Unfollow(int currentUserID, int userID)
        {
            var user = _followUserHandlers.UnfollowUser(currentUserID, userID);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        /// <summary>
        /// Get all Majors of selected User. (User)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet("{userID}/majors")]
        public async Task<IActionResult> GetMajorsOf(int userID)
        {
            var getMajors = _userMajorHandlers.GetMajorsOf(userID);
            if (getMajors == null || getMajors.Count == 0) return Ok(new List<MajorDTO>());
            return Ok(getMajors);
        }

        [HttpPost("{userID}/major")]
        public async Task<IActionResult> AddMajor(int userID, [FromQuery] int[] majorID)
        {
            var addMajor = _userMajorHandlers.AddUserMajor(userID, majorID);
            if (addMajor == null) return BadRequest("Added Failed !");
            return Ok(addMajor);
        }

        [HttpDelete("{userID}/major")]
        public async Task<IActionResult> DeleteMajor(int userID, [FromQuery] int[] majorID)
        {
            var deleteMajor = _userMajorHandlers.DeleteUserMajor(userID, majorID);
            if (deleteMajor == null) return BadRequest("Deleted Failed !");
            return Ok(deleteMajor);
        }

        [HttpGet("{userID}/subjects")]
        public async Task<IActionResult> GetSubjectsOf(int userID)
        {
            var getSubjects = _userSubjectHandlers.GetSubjectsOf(userID);
            if (getSubjects == null || getSubjects.Count == 0) return Ok(new List<SubjectDTO>());
            return Ok(getSubjects);
        }

        [HttpPost("{userID}/subject")]
        public async Task<IActionResult> AddSubject(int userID, [FromQuery]int[] subjectID)
        {
            var addSubject = _userSubjectHandlers.AddUserSubject(userID, subjectID);
            if (addSubject == null) return BadRequest("Added Failed !");
            return Ok(addSubject);
        }

        [HttpDelete("{userID}/subject")]
        public async Task<IActionResult> DeleteSubject(int userID, [FromQuery] int[] subjectID)
        {
            var deleteSubject = _userSubjectHandlers.DeleteUserSubject(userID, subjectID);
            if (deleteSubject == null) return BadRequest("Deleted Failed !");
            return Ok(deleteSubject);
        }
    }
}
