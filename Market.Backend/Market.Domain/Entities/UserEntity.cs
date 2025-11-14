using Market.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ProfileImageURL { get; set; }
        public string? ProfileBio { get; set; }
        public int CityId { get; set; }
        public CityEntity City { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsAdmin { get; set; }
        public CartEntity Cart { get; set; }
        public IReadOnlyCollection<UserGameEntity> UserGames { get; private set; } = new List<UserGameEntity>();
        public IReadOnlyCollection<UserAchievementEntity> UserAchievements { get; private set; } = new List<UserAchievementEntity>();
        public IReadOnlyCollection<FavouriteEntity> Favourites { get; private set; } = new List<FavouriteEntity>();
        public IReadOnlyCollection<NotificationEntity> Notifications { get; private set; } = new List<NotificationEntity>();
        public IReadOnlyCollection<OrderEntity> Orders { get; private set; } = new List<OrderEntity>();
        public IReadOnlyCollection<UserSecurityQuestionEntity> UserSecurityQuestions { get; private set; } = new List<UserSecurityQuestionEntity>();

        public UserEntity()
        {
            CreationDate = DateTime.UtcNow; // automatically set when the user is created
            IsAdmin = false; // default to non-admin
        }


    }
}
