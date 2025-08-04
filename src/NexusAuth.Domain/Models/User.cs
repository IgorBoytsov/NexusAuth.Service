using NexusAuth.Domain.Enums;
using NexusAuth.Domain.Events.User;
using NexusAuth.Domain.Exceptions;
using NexusAuth.Domain.Exceptions.Guard;
using NexusAuth.Domain.Primitives;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Domain.Models
{
    public sealed class User : AggregateRoot<Guid>
    {
        /*--Основная информация о пользователе--*/
      
        public Login Login { get; private set; } = null!;
        public UserName UserName { get; private set; } = null!;
        public PasswordHash PasswordHash { get; private set; } = null!;
        public Email Email { get; private set; } = null!;
        public Phone? Phone { get; private set; }

        /*--Даты--*/

        public DateTime DateRegistration { get; private set; }
        public DateTime? DateEntry { get; private set; }
        public DateTime DateUpdate { get; private set; }

        /*--Связанные данные--*/

        public StatusId IdStatus { get; private set; }
        public RoleId IdRole { get; private set; }
        public GenderId? IdGender { get; private set; }
        public CountryId? IdCountry { get; private set; }

        /*--Навигационные свойства--*/

        public Status Status { get; private set; } = null!;
        public Role Role { get; private set; } = null!;
        public Gender? Gender { get; private set; }
        public Country? Country { get; private set; }

        private User() { }

        private User(Guid id, Login login, UserName userName, PasswordHash passwordHash, Email email, RoleId roleId)
            : base(id)
        {
            Login = login;
            UserName = userName;
            PasswordHash = passwordHash;
            Email = email;
            IdRole = roleId;

            DateRegistration = DateTime.UtcNow;
            DateUpdate = DateTime.UtcNow;
            IdStatus = StatusId.From((int)StatusAccount.Active);
        }

        public static User Create(
            string login, string userName, string passwordHash, string email, string? phone,
            int roleId, int? genderId, int? countryId)
        {
            var loginVo = Login.Create(login);
            var userNameVo = UserName.Create(userName);
            var passwordHashVo = PasswordHash.Create(passwordHash);
            var emailVo = Email.Create(email);
            var roleIdVo = RoleId.From(roleId);

            var user = new User(Guid.NewGuid(), loginVo, userNameVo, passwordHashVo, emailVo, roleIdVo);

            if (phone is not null)
                user.Phone = Phone.Create(phone);

            if (genderId.HasValue)
                user.IdGender = GenderId.From(genderId.Value);

            if (countryId.HasValue)
                user.IdCountry = CountryId.From(countryId.Value);

            user.AddDomainEvent(new UserCreatedEvent(Guid.NewGuid(), user.Id, user.Email, user.UserName, DateTime.UtcNow));

            return user;
        }

        public void ChangeUserName(string userName, Guid? changedByUserId)
        {
            if (userName != UserName.Value)
            {
                var oldUserName = UserName;
                UserName = UserName.Create(userName);

                AddDomainEvent(new UserChangedUserNameEvent(Guid.NewGuid(), DateTime.UtcNow, Id, UserName, oldUserName, changedByUserId));
            }
        }

        public void UpdatePassword(string passwordHash)
        {
            var newHash = PasswordHash.Create(passwordHash);

            Guard.Against.That(newHash == PasswordHash, () => new IdenticalPasswordsException("Новый пароль не должен быть таким же, как и предыдущий"));

            PasswordHash = newHash;

            AddDomainEvent(new UserUpdatePasswordEvent(Guid.NewGuid(), DateTime.UtcNow, Id));
        }

        public void UpdateLastEntryDate() => DateEntry = DateTime.UtcNow;
    }
}